using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public enum PanelType
{
    AlarmPanel=0,
    LogPanel=1,
    ParameTerPanel=2
}
public class HistoryPanelView :UIView<HistoryPanelCtr>
{
    private Button _alarmBtn;
    private Button _operationBtn;
    private Button _parameterBtn;
    private GameObject _alarmPanel;
    private HistoryList _alarmReclaimerList;
    private HistoryList _alarmStackerReclaimerList;
    private GameObject _logPanel;
    private HistoryList _logReclaimerList;
    private HistoryList _logmStackerReclaimerList;
    private GameObject _parameterPanel;
    private SearchPanel _reclaimer;
    private SearchPanel _stackerReclaimer;
    public PanelType curPanelType;
    public override void InitUIElements(UIArgs uiArgs = null)
    {
        _alarmBtn = RootObj.transform.FindComponent<Button>("Btns/alarmBtnOff");
        _operationBtn = RootObj.transform.FindComponent<Button>("Btns/operationOff");
        _parameterBtn = RootObj.transform.FindComponent<Button>("Btns/parameterOff");
        _alarmPanel = RootObj.transform.Find("AlarmPanel").gameObject;
        _alarmReclaimerList = RootObj.transform.FindComponent<HistoryList>("AlarmPanel/alarmScrollView_1/Scroll View");
        _alarmStackerReclaimerList = RootObj.transform.FindComponent<HistoryList>("AlarmPanel/alarmScrollView_2/Scroll View");
        _logPanel=RootObj.transform.Find("LogPanel").gameObject;
        _logReclaimerList = RootObj.transform.FindComponent<HistoryList>("LogPanel/alarmScrollView_1/Scroll View");
        _logmStackerReclaimerList = RootObj.transform.FindComponent<HistoryList>("LogPanel/alarmScrollView_2/Scroll View");
        _parameterPanel = RootObj.transform.Find("ParameterPanel").gameObject;
        _reclaimer = RootObj.transform.FindComponent<SearchPanel>("reclaimerSearchPanel");
        _stackerReclaimer = RootObj.transform.FindComponent<SearchPanel>("StackerReclaimerSearchPanel");
        _alarmBtn.onClick.AddListener(ShowAlarmPanel);
        _operationBtn.onClick.AddListener(ShowLogPanel);
        _parameterBtn.onClick.AddListener(ShowParameterPanel);
        curPanelType = PanelType.AlarmPanel;
        _reclaimer.SetHistoryPanel(_ctr);
        _stackerReclaimer.SetHistoryPanel(_ctr);
        InitRecord();
    }

    private void InitRecord()
    {
        string warning = "warning";
        string log = "logs";
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
        curPanelType = PanelType.AlarmPanel;
        _alarmPanel.SetActive(true);
        _logPanel.SetActive(false);
        _parameterPanel.SetActive(false);
        SearchPanelActive(true);
    }
    private void ShowLogPanel()
    {
        curPanelType = PanelType.LogPanel;
        _alarmPanel.SetActive(false);
        _logPanel.SetActive(true);
        _parameterPanel.SetActive(false);
        SearchPanelActive(true);
    }
    private void ShowParameterPanel()
    {
        curPanelType = PanelType.ParameTerPanel;
        _alarmPanel.SetActive(false);
        _logPanel.SetActive(false);
        _parameterPanel.SetActive(true);
        SearchPanelActive(false);
    }

    void SearchPanelActive(bool isActive)
    {
        _reclaimer.gameObject.SetActive(isActive);
        _stackerReclaimer.gameObject.SetActive(isActive);
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
