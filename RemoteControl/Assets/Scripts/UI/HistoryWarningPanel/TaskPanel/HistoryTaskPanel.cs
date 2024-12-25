using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
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
            mySqlDataReader = DataManager.Instance.GetHistoryTaskMc(100);
            ReadRecord();
        }
        else
        {
            searchPanel.searchBtn.onClick.Invoke();
        }
  
    }

    private void OnEnable()
    {
        InitRecord();
    }

    private void OnDisable()
    {
        // searchPanel.Reset();
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
            data.takePileLength = mySqlDataReader[ConstStr.DATA_TASK_TAKE_MATE_HIGH].ToString();
            data.layerHigh = mySqlDataReader[ConstStr.DATA_TASK_LAYER_HIGH].ToString();
            data.timeAt = mySqlDataReader[ConstStr.DATA_TIMEDAT].ToString();
            data.quantity = mySqlDataReader[ConstStr.DATA_QUANTITY].ToString();
            data.operationName = mySqlDataReader[ConstStr.DATA_OPERATOR].ToString();
            data.state = mySqlDataReader[ConstStr.DATA_TASK_STATE].ToString();
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
        mySqlDataReader =DataManager.Instance.GetHistoryTaskMcBySql(sql);
        ReadRecord();
    }
    public void RefreshRecord(List<HistoryTaskData> datas)
    {
        UpdateDateDic();
        HistoryScrollViewHeighChange.SetScrollRectPosition(1);
        historyTaskList.RefreshList(datas);
    }
}
