using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public enum PanelType
{
    AlarmPanel=0,
    LogPanel=1,
    ParameTerPanel=2,
    TaskPanel=3
}
public class HistoryPanelView :UIView<HistoryPanelCtr>
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
    private GameObject _logPanel;
    private HistoryList _logReclaimerList;
    private HistoryList _logmStackerReclaimerList;
    private GameObject _parameterPanel;
    private GameObject _taskPanel;
    private SearchPanel _reclaimer;
    private SearchPanel _stackerReclaimer;
    public PanelType curPanelType;

    public GameObject curOffBtn;
    public GameObject curOnBtn;
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
        _alarmReclaimerList = RootObj.transform.FindComponent<HistoryList>("AlarmPanel/alarmScrollView_2/Scroll View");
        _alarmStackerReclaimerList = RootObj.transform.FindComponent<HistoryList>("AlarmPanel/alarmScrollView_1/Scroll View");
        _logPanel=RootObj.transform.Find("LogPanel").gameObject;
        _logReclaimerList = RootObj.transform.FindComponent<HistoryList>("LogPanel/alarmScrollView_2/Scroll View");
        _logmStackerReclaimerList = RootObj.transform.FindComponent<HistoryList>("LogPanel/alarmScrollView_1/Scroll View");
        _parameterPanel = RootObj.transform.Find("ImportantParamsGraphPanel").gameObject;
        _reclaimer = RootObj.transform.FindComponent<SearchPanel>("reclaimerSearchPanel");
        _stackerReclaimer = RootObj.transform.FindComponent<SearchPanel>("StackerReclaimerSearchPanel");
        _taskPanel = RootObj.transform.Find("TaskPanel").gameObject;


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
        InitRecord();
    }

    private void InitRecord()
    {
        string warning = "history_warning";
        string log = "history_logs";
        string warningSql = "Select * from " + warning + " ORDER BY id DESC LIMIT 10;";
        string warningSql2 = "Select * from " + warning + " ORDER BY id DESC LIMIT 5;";
        string  logSql = "Select * from " + log + " ORDER BY id DESC LIMIT 10;";
        string  logSql2 = "Select * from " + log + " ORDER BY id DESC LIMIT 5;";
        _ctr.RequestData(warningSql, MechanicalType.Reclaimer, PanelType.AlarmPanel);
        _ctr.RequestData(warningSql2, MechanicalType.StackerReclaimer, PanelType.AlarmPanel);
        _ctr.RequestData(logSql, MechanicalType.Reclaimer, PanelType.LogPanel);
        _ctr.RequestData(logSql2, MechanicalType.StackerReclaimer, PanelType.LogPanel);
    }
    private void ShowAlarmPanel()
    {
        RestCurBtn(_alarmBtn.gameObject, _alarmBtnOn.gameObject);
        curPanelType = PanelType.AlarmPanel;
        _alarmPanel.SetActive(true);
        _logPanel.SetActive(false);
        _taskPanel.SetActive(false);
        _parameterPanel.SetActive(false);
        SearchPanelActive(true);
    }
    private void ShowLogPanel()
    {
        RestCurBtn(_operationBtn.gameObject, _operationBtnOn.gameObject);
        curPanelType = PanelType.LogPanel;
        _alarmPanel.SetActive(false);
        _logPanel.SetActive(true);
        _parameterPanel.SetActive(false);
        _taskPanel.SetActive(false);
        SearchPanelActive(true);
    }
    private void ShowParameterPanel()
    {
        RestCurBtn(_parameterBtn.gameObject, _parameterBtnOn.gameObject);
        curPanelType = PanelType.ParameTerPanel;
        _alarmPanel.SetActive(false);
        _logPanel.SetActive(false);
        _taskPanel.SetActive(false);
        _parameterPanel.SetActive(true);
        SearchPanelActive(false);
    }

    void ShowTaskPanel()
    {
        RestCurBtn(_taskBtn.gameObject, _taskBtnOn.gameObject);
        curPanelType=PanelType.TaskPanel;
        _alarmPanel.SetActive(false);
        _logPanel.SetActive(false);
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

    public void ShowStatePane(StatusParameterChildID id)
    {

        Debug.Log($"状态参数打开 {id}");
        
    }
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

    public void RefreshList(List<HistoryData> historyDatas,MechanicalType mechanicalType,PanelType panelType)
    {
        Debug.Log($"RefreshList  mechanicalType{mechanicalType} ,panelType {panelType}");
        if (mechanicalType==MechanicalType.Reclaimer)
        {
            if (panelType==PanelType.AlarmPanel)
            {
                _alarmReclaimerList.RefreshList(historyDatas);
            }
            else if(panelType==PanelType.LogPanel)
            {
                _logReclaimerList.RefreshList(historyDatas);
            }
            else
            {
                
            }
        }
        else
        {
            if (panelType==PanelType.AlarmPanel)
            {
                _alarmStackerReclaimerList.RefreshList(historyDatas);
            }
            else if(panelType==PanelType.LogPanel)
            {
                _logmStackerReclaimerList.RefreshList(historyDatas);
            }
            else
            {
                
            }
        }
    }
}
