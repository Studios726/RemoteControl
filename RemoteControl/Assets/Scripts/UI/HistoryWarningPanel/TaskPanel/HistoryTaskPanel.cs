using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HistoryTaskPanel : MonoBehaviour
{
    public SearchPanel searchPanel;
    List<HistoryTaskData> historyTaskDatas = new List<HistoryTaskData>();
    public HistoryTaskList historyTaskList;
    private DateCell dateCell;
    MySqlDataReader mySqlDataReader;
    public Button historyBtn;//历史记录
    public Button dynamicBtn;//实时数据
    public HistoryScrollViewHeighChange HistoryScrollViewHeighChange;
    public float Heigh;
    private void Awake()
    {
        HistoryScrollViewHeighChange.Init();
        searchPanel.SetSearchAction(SearchRecord);
        historyBtn.onClick.AddListener(() =>
        {
            historyBtn.gameObject.SetActive(false);
            dynamicBtn.gameObject.SetActive(true);
            dateCell.IsDynamic = true;
            searchPanel.gameObject.SetActive(false);
            HistoryScrollViewHeighChange.ChangeHeigh(Heigh);
            InitRecord();
        });
        dynamicBtn.onClick.AddListener(() =>
        {
            historyBtn.gameObject.SetActive(true);
            dynamicBtn.gameObject.SetActive(false);
            dateCell.IsDynamic = false;
            searchPanel.gameObject.SetActive(true);
            searchPanel.searchBtn.onClick.Invoke();
            HistoryScrollViewHeighChange.ChangeHeigh(0);
            InitRecord();
        });
        Heigh = searchPanel.transform.GetComponent<RectTransform>().rect.height;
        InitDateDic();
        SetSearchPanelDate();
    }

    private void Start()
    {
        // dateCell.IsDynamic = true;
        // InitRecord();
    }

    private void InitDateDic()
    {
        DateTime now = DateTime.Now;
        string startDate = $"{now.Year}-{now.Month}-{now.Day}" + "-0-0";
        string endDate = $"{now.Year}-{now.Month}-{now.Day}" + "-23-59";
        dateCell= new DateCell(startDate, endDate);
    }
    private void SetSearchPanelDate()
    {
        if (searchPanel != null&&dateCell!=null)
        {
            string[] startTime = dateCell.StartTime.Split("-");
            string[] endTime = dateCell.EndTime.Split("-");
            if (startTime.Length>=4&&endTime.Length>=4)
            {
                searchPanel.SetDateText(startTime, endTime);
            }
            historyBtn.gameObject.SetActive(dateCell.IsDynamic==false);
            dynamicBtn.gameObject.SetActive(dateCell.IsDynamic);
            searchPanel.gameObject.SetActive(dateCell.IsDynamic==false);
        }
    }
    
    private void UpdateDateDic()
    {
        dateCell.StartTime =searchPanel?.GetStartDateText();
        dateCell.EndTime =searchPanel?.GetEndDateText();
    }

    public void InitRecord()
    {
        if (dateCell.IsDynamic)
        {
            GetHistoryTaskMcAsync(100);
            // mySqlDataReader = DataManager.Instance.GetHistoryTaskMc(100);
            // if (mySqlDataReader==null)
            // {
            //     return;
            // }
            // ReadRecord();
        }
        else
        {
            searchPanel.searchBtn.onClick.Invoke();
        }
  
    }
    
    private void OnEnable()
    {
        historyBtn.onClick.Invoke();
        //
        // InitRecord();
    }

    private void OnDisable()
    {
        // searchPanel.Reset();
    }

    public async void GetHistoryTaskMcAsync(int limit)
    {
        DataSet dataSet = null;
        await Task.Run((() =>
        {
            dataSet=DataManager.Instance.GetHistoryTaskMcByLimit(100);
        }));
        if (dataSet!=null)
        {
            ReadRecordAsync(dataSet);
        }
    }

    public async void GetHistoryTaskMcAsyncBySql(string sql)
    {
        DataSet dataSet = null;
        await Task.Run((() =>
        {
            dataSet=  DataManager.Instance.GetHistoryTaskMcBySql(sql);
        }));
        if (dataSet!=null)
        {
            ReadRecordAsync(dataSet);
        }
      
    }
    public void ReadRecordAsync(DataSet dataSet)
    {
        List<HistoryTaskData> historyTaskDatas = new List<HistoryTaskData>();
        DataRowCollection dataRowCollection = dataSet.Tables[0].Rows;
        int counter = 0;
        for (int i = 0; i < dataRowCollection.Count; i++)
        {
            HistoryTaskData data = new HistoryTaskData();
            data.id = dataRowCollection[i][ConstStr.DATA_HISTORY_LOGS_ID].ToString();
            
            data.id = dataRowCollection[i][ConstStr.DATA_TASK_ID].ToString(); //DateTime.Now.ToString("yyMMddHHmmss");
            data.time = dataRowCollection[i][ConstStr.DATA_TASK_CREATE_TIME].ToString();
            data.machine = dataRowCollection[i][ConstStr.DATA_MACHINE].ToString() == Machine.BucketWheelStackerReclaimer.ToString() ? "1#" : "2#";
            data.taskType = dataRowCollection[i][ConstStr.DATA_TASK_TYPE].ToString() == TaskType.PILEMATER.ToString() ? "堆料" : "取料";
            data.thingRange = dataRowCollection[i][ConstStr.DATA_MATERIAL_RANGE_START].ToString() + "-" + dataRowCollection[i][ConstStr.DATA_MATERIAL_RANGE_END].ToString();
            data.leftRightRange = dataRowCollection[i][ConstStr.DATA_LEFT_RIGHT_RANGE_START].ToString() + "-" + dataRowCollection[i][ConstStr.DATA_LEFT_RIGHT_RANGE_END].ToString();
            data.leftRightSelect = dataRowCollection[i][ConstStr.DATA_SIDE_SELECTION].ToString()=="LEFT"?"左":"右";
            data.takePileLength = dataRowCollection[i][ConstStr.DATA_STEP_LENGTH].ToString();
            data.takeStepLength= dataRowCollection[i][ConstStr.DATA_STEP_LENGTH].ToString();
            data.pileHigh=dataRowCollection[i][ConstStr.DATA_TASK_PILE_MATE_HEIGH].ToString();
            data.layerHigh = dataRowCollection[i][ConstStr.DATA_TASK_LAYER_HIGH].ToString();
            data.timeAt = dataRowCollection[i][ConstStr.DATA_TIMEDAT].ToString();
            data.quantity = dataRowCollection[i][ConstStr.DATA_QUANTITY].ToString();
            data.operationName = dataRowCollection[i][ConstStr.DATA_OPERATOR].ToString();
            data.state = dataRowCollection[i][ConstStr.DATA_TASK_STATE2].ToString();
            data.autoMode= dataRowCollection[i][ConstStr.DATA_TASK_AUTO_MODE].ToString()==AutoMode.SemiAuto.ToString()?"人工":"自动";
            data.angleExpansionFactor =dataRowCollection[i][ConstStr.DATA_TASK_ANGLE_ENTRY_VALUE].ToString();
          
            historyTaskDatas.Add(data);
            ++counter;
            if (counter == 1000)
            {
                break;
            }
           
        }
          RefreshRecord(historyTaskDatas);
    }
    public void ReadRecord()
    {
        int counter = 0;
        historyTaskDatas.Clear();
        while (mySqlDataReader.Read())
        {
            HistoryTaskData data = new HistoryTaskData();
            data.id = mySqlDataReader[ConstStr.DATA_TASK_ID].ToString(); //DateTime.Now.ToString("yyMMddHHmmss");
            data.time = mySqlDataReader[ConstStr.DATA_TASK_CREATE_TIME].ToString();
            data.machine = mySqlDataReader[ConstStr.DATA_MACHINE].ToString() == Machine.BucketWheelStackerReclaimer.ToString() ? "1#" : "2#";
            data.taskType = mySqlDataReader[ConstStr.DATA_TASK_TYPE].ToString() == TaskType.PILEMATER.ToString() ? "堆料" : "取料";
            data.thingRange = mySqlDataReader[ConstStr.DATA_MATERIAL_RANGE_START].ToString() + "-" + mySqlDataReader[ConstStr.DATA_MATERIAL_RANGE_END].ToString();
            data.leftRightRange = mySqlDataReader[ConstStr.DATA_LEFT_RIGHT_RANGE_START].ToString() + "-" + mySqlDataReader[ConstStr.DATA_LEFT_RIGHT_RANGE_END].ToString();
            data.leftRightSelect = mySqlDataReader[ConstStr.DATA_SIDE_SELECTION].ToString()=="LEFT"?"左":"右";
            data.takePileLength = mySqlDataReader[ConstStr.DATA_STEP_LENGTH].ToString();
            data.takeStepLength= mySqlDataReader[ConstStr.DATA_STEP_LENGTH].ToString();
            data.pileHigh=mySqlDataReader[ConstStr.DATA_TASK_PILE_MATE_HEIGH].ToString();
            data.layerHigh = mySqlDataReader[ConstStr.DATA_TASK_LAYER_HIGH].ToString();
            data.timeAt = mySqlDataReader[ConstStr.DATA_TIMEDAT].ToString();
            data.quantity = mySqlDataReader[ConstStr.DATA_QUANTITY].ToString();
            data.operationName = mySqlDataReader[ConstStr.DATA_OPERATOR].ToString();
            data.state = mySqlDataReader[ConstStr.DATA_TASK_STATE2].ToString();
            data.autoMode= mySqlDataReader[ConstStr.DATA_TASK_AUTO_MODE].ToString()==AutoMode.SemiAuto.ToString()?"人工":"自动";
            data.angleExpansionFactor = "0";
            historyTaskDatas.Add(data);
            ++counter;
            if (counter == 1000)
            {
                counter = 0;
                break;
            }
        }
        RefreshRecord(historyTaskDatas);
    }
    public void SearchRecord(string startTime, string endTime, MechanicalType mechanicalType,string user)
    {
        string sql = $"SELECT * FROM {ConstStr.DATABASE_HISTORY_TASK_MC} WHERE"+$"`{ConstStr.DATA_TASK_CREATE_TIME}` BETWEEN '{startTime}' AND '{endTime}' ORDER BY `{ConstStr.DATA_TASK_CREATE_TIME}` DESC;";
        GetHistoryTaskMcAsyncBySql(sql);
        // mySqlDataReader =DataManager.Instance.GetHistoryTaskMcBySql(sql);
        // if (mySqlDataReader!=null)
        // {
        //     ReadRecord();
        // }
    }
    public void RefreshRecord(List<HistoryTaskData> datas)
    {
        UpdateDateDic();
        HistoryScrollViewHeighChange.SetScrollRectPosition(1);
        historyTaskList.RefreshList(datas);
    }
}
