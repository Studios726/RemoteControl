using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public enum PanelType
{
    AlarmPanel = 0,
    LogPanel = 1,
    ParameTerPanel = 2,
    TaskPanel = 3
}
public class HistoryPanelView : UIView<HistoryPanelCtr>
{
    private Button _alarmBtn;
    private Button _operationBtn;
    private Button _parameterBtn;
    private Button _taskBtn;
    private Button _alarmBtnOn;
    private Button _operationBtnOn;
    private Button _parameterBtnOn;
    private Button _taskBtnOn;
    private GameObject _alarmPanel;
    private HistoryList _alarmReclaimerList;
    private HistoryList _alarmStackerReclaimerList;
    private HistoryScrollViewHeighChange _alarmReclaimerHeighChange;
    private HistoryScrollViewHeighChange _alarmStackerReclaimerHeighChange;
    private GameObject _logPanel;
    private HistoryList _logReclaimerList;
    private HistoryList _logmStackerReclaimerList;
    private HistoryScrollViewHeighChange _logReclaimerHeighChange;
    private HistoryScrollViewHeighChange _logStackerReclaimerHeighChange;
    private GameObject _parameterPanel;
    private GameObject _taskPanel;
    private SearchPanel _reclaimer;
    private SearchPanel _stackerReclaimer;
    public PanelType curPanelType;

    public GameObject curOffBtn;
    public GameObject curOnBtn;
    public GameObject dateBtnsGo;
    public Button historyPileTakeBtn;
    public Button historyTakeBtn;
    public Button dynamicPileTakeBtn;
    public Button dynamicTakeBtn;
    private float Heigh;
    private Dictionary<string, DateCell> dateDic = new Dictionary<string, DateCell>();
    public override void InitUIElements(UIArgs uiArgs = null)
    {
        _alarmBtn = RootObj.transform.FindComponent<Button>("Btns/alarmBtnOff");
        _operationBtn = RootObj.transform.FindComponent<Button>("Btns/operationOff");
        _parameterBtn = RootObj.transform.FindComponent<Button>("Btns/parameterOff");
        _taskBtn = RootObj.transform.FindComponent<Button>("Btns/taskOff");
        _alarmBtnOn = RootObj.transform.FindComponent<Button>("Btns/alarmBtnOn");
        _operationBtnOn = RootObj.transform.FindComponent<Button>("Btns/operationOn");
        _parameterBtnOn = RootObj.transform.FindComponent<Button>("Btns/parameterOn");
        _taskBtnOn = RootObj.transform.FindComponent<Button>("Btns/taskOn");
        _alarmPanel = RootObj.transform.Find("AlarmPanel").gameObject;
        _alarmReclaimerHeighChange=RootObj.transform.FindComponent<HistoryScrollViewHeighChange>("AlarmPanel/alarmScrollView_2");
        _alarmReclaimerList = RootObj.transform.FindComponent<HistoryList>("AlarmPanel/alarmScrollView_2/Scroll View");
        _alarmStackerReclaimerHeighChange= RootObj.transform.FindComponent<HistoryScrollViewHeighChange>("AlarmPanel/alarmScrollView_1");
        _alarmStackerReclaimerList =
            RootObj.transform.FindComponent<HistoryList>("AlarmPanel/alarmScrollView_1/Scroll View");
        _logPanel = RootObj.transform.Find("LogPanel").gameObject;
        _logReclaimerHeighChange= RootObj.transform.FindComponent<HistoryScrollViewHeighChange>("LogPanel/alarmScrollView_2");
        _logReclaimerList = RootObj.transform.FindComponent<HistoryList>("LogPanel/alarmScrollView_2/Scroll View");
        _logStackerReclaimerHeighChange=RootObj.transform.FindComponent<HistoryScrollViewHeighChange>("LogPanel/alarmScrollView_1");
        _logmStackerReclaimerList =
            RootObj.transform.FindComponent<HistoryList>("LogPanel/alarmScrollView_1/Scroll View");
        _parameterPanel = RootObj.transform.Find("ImportantParamsGraphPanel").gameObject;
        _reclaimer = RootObj.transform.FindComponent<SearchPanel>("reclaimerSearchPanel");
        _stackerReclaimer = RootObj.transform.FindComponent<SearchPanel>("StackerReclaimerSearchPanel");
        _taskPanel = RootObj.transform.Find("TaskPanel").gameObject;

        dateBtnsGo=RootObj.transform.Find("dateBtns").gameObject;
        historyPileTakeBtn = RootObj.transform.FindComponent<Button>("dateBtns/historyPileTakeBtn");
        dynamicPileTakeBtn = RootObj.transform.FindComponent<Button>("dateBtns/dynamicPileTakeBtn");
        dynamicTakeBtn = RootObj.transform.FindComponent<Button>("dateBtns/dynamicTakeBtn");
        historyTakeBtn = RootObj.transform.FindComponent<Button>("dateBtns/historyTakeBtn");

        historyPileTakeBtn.onClick.AddListener(() =>
        {
            historyPileTakeBtn.gameObject.SetActive(false);
            dynamicPileTakeBtn.gameObject.SetActive(true);
            if (curPanelType == PanelType.AlarmPanel)
            {
                dateDic[ConstStr.DATABASE_HISTORY_WARNING1_MC].IsDynamic = true;
                LatestWarningLogsByMachine(Machine.BucketWheelStackerReclaimer, PanelType.AlarmPanel);
                _alarmStackerReclaimerHeighChange.ChangeHeigh(Heigh);
            }else if (curPanelType == PanelType.LogPanel)
            {
                dateDic[ConstStr.DATABASE_HISTORY_LOG1_MC].IsDynamic = true;
                LatestWarningLogsByMachine(Machine.BucketWheelStackerReclaimer, PanelType.LogPanel);
                _logStackerReclaimerHeighChange.ChangeHeigh(Heigh);
            }
            _stackerReclaimer.gameObject.SetActive(false);
          
        });
        dynamicPileTakeBtn.onClick.AddListener(() =>
        {
            historyPileTakeBtn.gameObject.SetActive(true);
            dynamicPileTakeBtn.gameObject.SetActive(false);
            if (curPanelType == PanelType.AlarmPanel)
            {
                dateDic[ConstStr.DATABASE_HISTORY_WARNING1_MC].IsDynamic = false;
                _alarmStackerReclaimerHeighChange.ChangeHeigh(0);
            }else if (curPanelType == PanelType.LogPanel)
            {
                dateDic[ConstStr.DATABASE_HISTORY_LOG1_MC].IsDynamic = false;
                _logStackerReclaimerHeighChange.ChangeHeigh(0);
            }
            _stackerReclaimer.gameObject.SetActive(true);
            _stackerReclaimer.searchBtn.onClick.Invoke();
        });
        historyTakeBtn.onClick.AddListener(() =>
        {
            historyTakeBtn.gameObject.SetActive(false);
            dynamicTakeBtn.gameObject.SetActive(true);
            if (curPanelType == PanelType.AlarmPanel)
            {
                dateDic[ConstStr.DATABASE_HISTORY_WARNING2_MC].IsDynamic = true;
                LatestWarningLogsByMachine(Machine.BucketWheel, PanelType.AlarmPanel);
                _alarmReclaimerHeighChange.ChangeHeigh(Heigh);
            }else if (curPanelType == PanelType.LogPanel)
            {
                dateDic[ConstStr.DATABASE_HISTORY_LOG2_MC].IsDynamic = true;
                LatestWarningLogsByMachine(Machine.BucketWheel, PanelType.LogPanel);
                _logReclaimerHeighChange.ChangeHeigh(Heigh);
            }
            _reclaimer.gameObject.SetActive(false);
        });
        dynamicTakeBtn.onClick.AddListener(() =>
        {
            historyTakeBtn.gameObject.SetActive(true);
            dynamicTakeBtn.gameObject.SetActive(false);
            if (curPanelType == PanelType.AlarmPanel)
            {
                dateDic[ConstStr.DATABASE_HISTORY_WARNING2_MC].IsDynamic = false;
                _alarmReclaimerHeighChange.ChangeHeigh(0);
            }else if (curPanelType == PanelType.LogPanel)
            {
                dateDic[ConstStr.DATABASE_HISTORY_LOG2_MC].IsDynamic = false;
                _logReclaimerHeighChange.ChangeHeigh(0);
            }
            _reclaimer.gameObject.SetActive(true);
            _reclaimer.searchBtn.onClick.Invoke();
        });
        
        
        Heigh= _reclaimer.transform.GetComponent<RectTransform>().rect.height;
        _alarmBtn.onClick.AddListener(ShowAlarmPanel);
        _operationBtn.onClick.AddListener(ShowLogPanel);
        _parameterBtn.onClick.AddListener(ShowParameterPanel);
        _taskBtn.onClick.AddListener(ShowTaskPanel);
        curPanelType = PanelType.AlarmPanel;
        //_reclaimer.SetHistoryPanel(_ctr);
        //_stackerReclaimer.SetHistoryPanel(_ctr);
        _reclaimer.SetSearchAction(_ctr.SearchRecord);
        _stackerReclaimer.SetSearchAction(_ctr.SearchRecord);
        curOffBtn = _alarmBtn.gameObject;
        curOnBtn = _alarmBtnOn.gameObject;
        // InitRecord();
        InitDateDic();
        SetSearchPanelDate(_stackerReclaimer,ConstStr.DATABASE_HISTORY_WARNING1_MC);
        SetSearchPanelDate(_reclaimer,ConstStr.DATABASE_HISTORY_WARNING2_MC);
    }

    private void InitRecord()
    {
        // GetLatestOperationLogs();
        GetLatestWarningLogs();
    }

    public void InitPanelUI()
    {
        if (_alarmBtn)
        {
            _alarmBtn.onClick?.Invoke();
        }
        else
        {
            InitRecord();
        }
    }
    private void GetLatestWarningLogs()
    {
        string warningSql = "";
        string warningSql2 = "";
        if (dateDic[ConstStr.DATABASE_HISTORY_WARNING1_MC].IsDynamic)
        {
            warningSql = $"Select * from {ConstStr.DATABASE_HISTORY_WARNING1_MC}  Order By time DESC  LIMIT 100 ;";
            _ctr.RequestData(warningSql, MechanicalType.StackerReclaimer, PanelType.AlarmPanel);
        }
        else
        {
            _stackerReclaimer.searchBtn.onClick.Invoke();
        }
        if (dateDic[ConstStr.DATABASE_HISTORY_WARNING2_MC].IsDynamic)
        {
            warningSql2 = $"Select * from {ConstStr.DATABASE_HISTORY_WARNING2_MC} Order By time DESC LIMIT 100;";
            _ctr.RequestData(warningSql2, MechanicalType.Reclaimer, PanelType.AlarmPanel);
        }
        else
        {
            _reclaimer.searchBtn.onClick.Invoke();
        }
      
    }

    private void LatestWarningLogsByMachine(Machine machine,PanelType panelType)
    {
        if (machine==Machine.BucketWheelStackerReclaimer)
        {
            if (panelType==PanelType.AlarmPanel)
            {
                string warningSql = $"Select * from {ConstStr.DATABASE_HISTORY_WARNING1_MC}  Order By time DESC  LIMIT 100 ;";
                _ctr.RequestData(warningSql, MechanicalType.StackerReclaimer, PanelType.AlarmPanel);
            }else if (panelType==PanelType.LogPanel)
            {
                string logSql = $"Select * from {ConstStr.DATABASE_HISTORY_LOG1_MC}  Order By time DESC LIMIT 100;";
                _ctr.RequestData(logSql, MechanicalType.StackerReclaimer, PanelType.LogPanel);
            } else
            {
                Debug.Log("PanelType is null");
            }
        }else if (machine==Machine.BucketWheel)
        {
            if (panelType==PanelType.AlarmPanel)
            {
                string warningSql = $"Select * from {ConstStr.DATABASE_HISTORY_WARNING2_MC}  Order By time DESC  LIMIT 100 ;";
                _ctr.RequestData(warningSql, MechanicalType.Reclaimer, PanelType.AlarmPanel);
            }else if (panelType==PanelType.LogPanel)
            {
                string logSql = $"Select * from {ConstStr.DATABASE_HISTORY_LOG2_MC}  Order By time DESC LIMIT 100;";
                _ctr.RequestData(logSql, MechanicalType.Reclaimer, PanelType.LogPanel);
            }
            else
            {
                Debug.Log("PanelType is null");
            }
        }
        else
        {
            Debug.Log("machine is null");
        }
    }
    private void GetLatestOperationLogs()
    {
        string logSql = "";
        string logSql2 = "";
        if (dateDic[ConstStr.DATABASE_HISTORY_LOG1_MC].IsDynamic)
        {
            logSql = $"Select * from {ConstStr.DATABASE_HISTORY_LOG1_MC}  Order By time DESC LIMIT 100;";
            _ctr.RequestData(logSql, MechanicalType.StackerReclaimer, PanelType.LogPanel);
        }
        else
        {
            _stackerReclaimer.searchBtn.onClick.Invoke();
        }
        if (dateDic[ConstStr.DATABASE_HISTORY_LOG2_MC].IsDynamic)
        {
            logSql2 = $"Select * from {ConstStr.DATABASE_HISTORY_LOG2_MC}  Order By time DESC LIMIT 100;";
            _ctr.RequestData(logSql2, MechanicalType.Reclaimer, PanelType.LogPanel);
        }
        else
        {
            _reclaimer.searchBtn.onClick.Invoke();
        }
     
    }

    private void ShowAlarmPanel()
    {
        RestCurBtn(_alarmBtn.gameObject, _alarmBtnOn.gameObject);
        curPanelType = PanelType.AlarmPanel;
        _alarmPanel.SetActive(true);
        _logPanel.SetActive(false);
        _taskPanel.SetActive(false);
        _parameterPanel.SetActive(false);
        dateBtnsGo.SetActive(true);
        SearchPanelActive(true);
        //默认每次显示实时
        dateDic[ConstStr.DATABASE_HISTORY_WARNING1_MC].IsDynamic = true;
        dateDic[ConstStr.DATABASE_HISTORY_WARNING2_MC].IsDynamic = true;
        SetSearchPanelDate(_stackerReclaimer,ConstStr.DATABASE_HISTORY_WARNING1_MC);
        SetSearchPanelDate(_reclaimer,ConstStr.DATABASE_HISTORY_WARNING2_MC);
        historyTakeBtn.onClick.Invoke();
        historyPileTakeBtn.onClick.Invoke();
        GetLatestWarningLogs();
    }

    private void ShowLogPanel()
    {
        RestCurBtn(_operationBtn.gameObject, _operationBtnOn.gameObject);
        curPanelType = PanelType.LogPanel;
        _alarmPanel.SetActive(false);
        _logPanel.SetActive(true);
        _parameterPanel.SetActive(false);
        _taskPanel.SetActive(false);
        dateBtnsGo.SetActive(true);
        SearchPanelActive(true);
        //默认每次显示实时
        dateDic[ConstStr.DATABASE_HISTORY_LOG1_MC].IsDynamic = true;
        dateDic[ConstStr.DATABASE_HISTORY_LOG2_MC].IsDynamic = true;
        SetSearchPanelDate(_stackerReclaimer,ConstStr.DATABASE_HISTORY_LOG1_MC);
        SetSearchPanelDate(_reclaimer,ConstStr.DATABASE_HISTORY_LOG2_MC);
        historyTakeBtn.onClick.Invoke();
        historyPileTakeBtn.onClick.Invoke();
        GetLatestOperationLogs();
      
    }

    private void ShowParameterPanel()
    {
        RestCurBtn(_parameterBtn.gameObject, _parameterBtnOn.gameObject);
        curPanelType = PanelType.ParameTerPanel;
        _alarmPanel.SetActive(false);
        _logPanel.SetActive(false);
        _taskPanel.SetActive(false);
        dateBtnsGo.SetActive(false);
        _parameterPanel.SetActive(true);
        SearchPanelActive(false);
    }

    void ShowTaskPanel()
    {
        RestCurBtn(_taskBtn.gameObject, _taskBtnOn.gameObject);
        curPanelType = PanelType.TaskPanel;
        _alarmPanel.SetActive(false);
        _logPanel.SetActive(false);
        dateBtnsGo.SetActive(false);
        _reclaimer.gameObject.SetActive(false);
        _stackerReclaimer.gameObject.SetActive(false);
        _parameterPanel.SetActive(false);
        _taskPanel.SetActive(true);
    }

    void SearchPanelActive(bool isActive)
    {
        _reclaimer.gameObject.SetActive(isActive);
        _stackerReclaimer.gameObject.SetActive(isActive);
    }

    // public void ShowStatePane(StatusParameterChildID id)
    // {
    //     Debug.Log($"状态参数打开 {id}");
    // }

    public void RestCurBtn(GameObject offgo, GameObject ongo)
    {
        if (curOffBtn != null)
        {
            curOffBtn.SetActive(true);
        }

        if (curOnBtn != null)
        {
            curOnBtn.SetActive(false);
        }

        offgo.SetActive(false);
        ongo.SetActive(true);
        curOffBtn = offgo;
        curOnBtn = ongo;
    }

    public void RefreshList(List<HistoryData> historyDatas, MechanicalType mechanicalType, PanelType panelType)
    {
        // Debug.Log($"RefreshList  mechanicalType{mechanicalType} ,panelType {panelType}");
        if (mechanicalType == MechanicalType.Reclaimer)
        {
            if (panelType == PanelType.AlarmPanel)
            {
                UpdateDateDic(_reclaimer, ConstStr.DATABASE_HISTORY_WARNING2_MC);
                _alarmReclaimerHeighChange.SetScrollRectPosition(1);
                _alarmReclaimerList.RefreshList(historyDatas,panelType);
            }
            else if (panelType == PanelType.LogPanel)
            {
                UpdateDateDic(_reclaimer, ConstStr.DATABASE_HISTORY_LOG2_MC);
                _logReclaimerHeighChange.SetScrollRectPosition(1);
                _logReclaimerList.RefreshList(historyDatas,panelType);
            }
            else
            {
            }
        }
        else
        {
            if (panelType == PanelType.AlarmPanel)
            {
                UpdateDateDic(_stackerReclaimer, ConstStr.DATABASE_HISTORY_WARNING1_MC);
                _alarmStackerReclaimerHeighChange.SetScrollRectPosition(1);
                _alarmStackerReclaimerList.RefreshList(historyDatas,panelType);
            }
            else if (panelType == PanelType.LogPanel)
            {
                UpdateDateDic(_stackerReclaimer, ConstStr.DATABASE_HISTORY_LOG1_MC);
                _logStackerReclaimerHeighChange.SetScrollRectPosition(1);
                _logmStackerReclaimerList.RefreshList(historyDatas,panelType);
            }
            else
            {
            }
        }
    }
    
    private void InitDateDic()
    {
        DateTime now = DateTime.Now;
        string startDate = $"{now.Year}-{now.Month}-{now.Day}" + "-0-0";
        string endDate = $"{now.Year}-{now.Month}-{now.Day}" + "-23-59";
        dateDic.Add(ConstStr.DATABASE_HISTORY_WARNING1_MC, new DateCell(startDate, endDate));
        dateDic.Add(ConstStr.DATABASE_HISTORY_WARNING2_MC, new DateCell(startDate, endDate));
        dateDic.Add(ConstStr.DATABASE_HISTORY_LOG1_MC, new DateCell(startDate, endDate));
        dateDic.Add(ConstStr.DATABASE_HISTORY_LOG2_MC, new DateCell(startDate, endDate));
    }

    private void UpdateDateDic(SearchPanel searchPanel, string key)
    {
        if (dateDic.ContainsKey(key))
        {
            dateDic[key].StartTime =searchPanel?.GetStartDateText();
            dateDic[key].EndTime =searchPanel?.GetEndDateText();
        }
    }

    private void SetSearchPanelDate(SearchPanel searchPanel,string key)
    {
        if (searchPanel != null)
        {
            if (dateDic.ContainsKey(key))
            {
                string[] startTime = dateDic[key].StartTime.Split("-");
                string[] endTime = dateDic[key].EndTime.Split("-");
                if (startTime.Length>=4&&endTime.Length>=4)
                {
                    searchPanel.SetDateText(startTime, endTime);
                }

                if (key==ConstStr.DATABASE_HISTORY_WARNING1_MC||key == ConstStr.DATABASE_HISTORY_LOG1_MC)
                {
                    historyPileTakeBtn.gameObject.SetActive(dateDic[key].IsDynamic==false);
                    dynamicPileTakeBtn.gameObject.SetActive(dateDic[key].IsDynamic);
                    _stackerReclaimer.gameObject.SetActive(dateDic[key].IsDynamic==false);
                    
                }else if (key==ConstStr.DATABASE_HISTORY_WARNING2_MC||key == ConstStr.DATABASE_HISTORY_LOG2_MC)
                {
                    historyTakeBtn.gameObject.SetActive(dateDic[key].IsDynamic==false);
                    dynamicTakeBtn.gameObject.SetActive(dateDic[key].IsDynamic);
                    _reclaimer.gameObject.SetActive(dateDic[key].IsDynamic==false);
                }
                else
                {
                    Debug.Log($"key {key} 不存在");
                }
            }
        }
    }
}