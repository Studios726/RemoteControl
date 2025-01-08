using System;
using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using ShenYangRemoteSystem.Subclass;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

public class TopPanelView : UIView<TopPanelCtr>
{
    private Button _controlBtn;
    private Button _superviseBtn;
    private Button _alarmBtn;
    public TMP_Text _controlText;
    public TMP_Text _superviseText;
    public TMP_Text _alarmText;
    private Button _userBtn;
    private TMP_Text _userTxt;
    private Transform _userPnl;
    private Button _settingBtn;
    private Button _logoutBtn;
    private Button _logoutBtn2;
    private Button _closeUserBtn;
    private GameObject StatePanel;
    private TMP_Text _title;
    public GameObject WorkScreenPanel;
    private Color selectColor = new Color(0, 0.98f, 1,1);
    private Color normalColor = Color.white;
    public TMP_Text _lastText;
    public ButtonCell pileScramStopBtn;
    public GameObject pileScramStopYellow;
    public ButtonCell takeScramStopBtn;
    public GameObject takeScramStopYellow;
    public Timer pileScramStopTimer;
    public Timer takeScramStopTimer;

    public override void InitUIElements(UIArgs uiArgs)
    {
        _controlBtn = RootObj.transform.FindComponent<Button>("BG/Control");
        _controlText=RootObj.transform.FindComponent<TMP_Text>("BG/Control/Text");
        _superviseBtn = RootObj.transform.FindComponent<Button>("BG/Supervise");
        _superviseText = RootObj.transform.FindComponent<TMP_Text>("BG/Supervise/Text");
        _alarmBtn = RootObj.transform.FindComponent<Button>("BG/Alarm");
        _alarmText = RootObj.transform.FindComponent<TMP_Text>("BG/Alarm/Text");
        _userBtn = RootObj.transform.FindComponent<Button>("BG/User");
        _closeUserBtn = RootObj.transform.FindComponent<Button>("BG/close");
        _userTxt = RootObj.transform.FindComponent<TMP_Text>("BG/User/Text (TMP)");
        _userPnl = RootObj.transform.Find("BG/UserPnl");
        _settingBtn = _userPnl.FindComponent<Button>("SettingBtn");
        _logoutBtn = _userPnl.FindComponent<Button>("LogoutBtn");
        _logoutBtn2 = _userPnl.FindComponent<Button>("LogoutBtn_2");
        pileScramStopBtn = RootObj.transform.FindComponent <ButtonCell > ("BG/pileScramStop");
        pileScramStopYellow= RootObj.transform.Find("BG/pileScramStop/yellow").gameObject;
        takeScramStopBtn = RootObj.transform.FindComponent <ButtonCell > ("BG/takeScramStop");
        takeScramStopYellow= RootObj.transform.Find("BG/takeScramStop/yellow").gameObject;
        _title = RootObj.transform.FindComponent<TMP_Text>("BG/Title/Text (TMP)");
        _title.text = ConstStr.PROJECT_NAME;
      
        _lastText = _controlText;
        _userTxt.text =GameDataManager.Instance.GetUserName();

        _controlBtn.onClick.AddListener(() => //��Զ�̲�������
        {
            // DataManager.Instance.InsertHistoryLogMc("远程控制", GameDataManager.Instance.GetUserName(), Machine.BucketWheel);
            // DataManager.Instance.InsertHistoryLogMc("远程控制", GameDataManager.Instance.GetUserName(), Machine.BucketWheelStackerReclaimer);
            SetSelectState(_controlText);
            UIManager.Instance.OpenUI(UIID.MainPanel);
            UIManager.Instance.CloseUI(UIID.HistoryPanel);
            UIManager.Instance.CloseUI(UIID.StatusParaeterPanel);
            UIManager.Instance.CloseUI(UIID.SettingPanel);
          
        });
        _superviseBtn.onClick.AddListener(() =>
        {
            // DataManager.Instance.InsertHistoryLogMc("状态参数", GameDataManager.Instance.GetUserName(), Machine.BucketWheel);
            // DataManager.Instance.InsertHistoryLogMc("状态参数", GameDataManager.Instance.GetUserName(), Machine.BucketWheelStackerReclaimer);
            UIManager.Instance.OpenUI(UIID.StatusParaeterPanel);
            UIManager.Instance.CloseUI(UIID.SettingPanel);
            UIManager.Instance.CloseUI(UIID.MainPanel);
            UIManager.Instance.CloseUI(UIID.HistoryPanel);
            SetSelectState(_superviseText);
            Debug.LogError($">>>>>>>>>>>>>>>>>>count {MySqlHelper.count}");
        });
        _alarmBtn.onClick.AddListener(() => //�򿪱����������
        {
            // DataManager.Instance.InsertHistoryLogMc("历史数据", GameDataManager.Instance.GetUserName(), Machine.BucketWheel);
            // DataManager.Instance.InsertHistoryLogMc("历史数据", GameDataManager.Instance.GetUserName(), Machine.BucketWheelStackerReclaimer);
            UIManager.Instance.OpenUI(UIID.HistoryPanel);
            UIManager.Instance.CloseUI(UIID.SettingPanel);
            UIManager.Instance.CloseUI(UIID.MainPanel);
            UIManager.Instance.CloseUI(UIID.StatusParaeterPanel);
            SetSelectState(_alarmText);

        });
        _userBtn.onClick.AddListener(() =>
        {
            // DataManager.Instance.InsertHistoryLogMc("用户管理", GameDataManager.Instance.GetUserName(), Machine.BucketWheel);
            // DataManager.Instance.InsertHistoryLogMc("用户管理", GameDataManager.Instance.GetUserName(), Machine.BucketWheelStackerReclaimer);
            _userPnl.gameObject.SetActive(!_userPnl.gameObject.activeSelf);
            _closeUserBtn.gameObject.SetActive(!_closeUserBtn.gameObject.activeSelf);
        });
        _closeUserBtn.onClick.AddListener((() =>
        {
            _closeUserBtn.gameObject.SetActive(false);
            _userPnl.gameObject.SetActive(false);
        }));
        _settingBtn.onClick.AddListener(() =>
        {
            _userPnl.gameObject.SetActive(false);
            _closeUserBtn.gameObject.SetActive(false);
            UIManager.Instance.OpenUI(UIID.SettingPanel);
            UIManager.Instance.CloseUI(UIID.HistoryPanel);
            UIManager.Instance.CloseUI(UIID.MainPanel);
            UIManager.Instance.CloseUI(UIID.StatusParaeterPanel);
            SetSelectState(_userTxt);
          
        });
        _logoutBtn.onClick.AddListener(() =>
        {
            _userPnl.gameObject.SetActive(false);
            _closeUserBtn.gameObject.SetActive(false);
            _ctr.Logout();
        });
        _logoutBtn2.onClick.AddListener(() =>
        {
            EventManager.Instance.TriggerEvent(EventName.ExitGame,null,null);
            // _userPnl.gameObject.SetActive(false);
            
        });
        _userPnl.gameObject.SetActive(false);
        _closeUserBtn.gameObject.SetActive(false);
        
        AddOnClickListener(pileScramStopBtn, (() =>
        {
            {
                if (pileScramStopBtn.red.activeSelf)
                {
                    UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                        new ConfirmPanelArgs("�Ƿ�ȷ�ϸ�λ��ͣ��", null, () =>  SendPlcCommand(COMMAND_NAME.EMERGENCY_STOP,Machine.BucketWheelStackerReclaimer)));
                }
                else
                {
                    SendPlcCommand(COMMAND_NAME.EMERGENCY_STOP,Machine.BucketWheelStackerReclaimer);
                }
            }
        }));
        
        AddOnClickListener(takeScramStopBtn, (() =>
        {
            {
                if (takeScramStopBtn.red.activeSelf)
                {
                    UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                        new ConfirmPanelArgs("�Ƿ�ȷ�ϸ�λ��ͣ��", null, () =>  SendPlcCommand(COMMAND_NAME.EMERGENCY_STOP,Machine.BucketWheel)));
                }
                else
                {
                    SendPlcCommand(COMMAND_NAME.EMERGENCY_STOP,Machine.BucketWheel);
                }
            }
        }));
    }
    public  void AddOnClickListener(ButtonCell btn, UnityAction action)
    {
        btn.AddListener(action);
    }

    public void UpdateData(object o, EventArgs eventArgs)
    {
        if (GameDataManager.Instance.SystemVariables==null)
        {
            return;
        }
        UpdatePlc(GameDataManager.Instance.SystemVariables);

    }
    public  void SendPlcCommand(COMMAND_NAME mCommandName,Machine machine, int dataInt = 0)
    {
        if (GameDataManager.Instance.GameMain.connectionRC.isConnect == false)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel, new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP,GameDataManager.Instance.GetMachineName(machine)));
            return;
        }

        string commandName = machine == Machine.BucketWheelStackerReclaimer
            ? mCommandName.ToString() + "_1"
            : mCommandName.ToString() + "_2";
        // int dataInt = 0;
        if (machine == Machine.BucketWheelStackerReclaimer)
        {
            commandName = mCommandName.ToString() + "_1";
            dataInt = pileScramStopBtn.red.activeSelf ? 0 : 1;
        }
        else
        {
            commandName = mCommandName.ToString() + "_2";
            dataInt = takeScramStopBtn.red.activeSelf ? 0 : 1;
        }
        Debug.Log($" commandName {commandName} {dataInt}");
        GameDataManager.Instance.SendServerCommandByName(commandName, dataInt);
    }
    public void UpdatePlc(SystemVariables data)
    {
        pileScramStopBtn.SetSystemState(data.System_Emergence||data.RemoteEmergencyStop);
        ScramStopFicker(data.System_Emergence||data.RemoteEmergencyStop,Machine.BucketWheelStackerReclaimer);
        
        takeScramStopBtn.SetSystemState(data.System_Emergence_2||data.RemoteEmergencyStop_2);
        ScramStopFicker(data.System_Emergence_2||data.RemoteEmergencyStop_2,Machine.BucketWheel);
    }
    
    public void ScramStopFicker(bool isFicker,Machine machine)
    {
        
        if (machine==Machine.BucketWheelStackerReclaimer)
        {
            if (isFicker)
            {
                if (pileScramStopTimer == null)
                {
                    // scramStopTimer.Cancel();
                    pileScramStopTimer = Timer.Register(1, true, true,
                        (() => { pileScramStopYellow.SetActive(!pileScramStopYellow.activeSelf); }));
                }
           
            }
            else
            {
                if (pileScramStopTimer != null)
                {
                    pileScramStopTimer.Cancel();
                    pileScramStopTimer = null;
                }

                pileScramStopYellow.SetActive(false);
            }
        }else
        {
            if (isFicker)
            {
                if (takeScramStopTimer == null)
                {
                    // scramStopTimer.Cancel();
                    takeScramStopTimer = Timer.Register(1, true, true,
                        (() => { takeScramStopYellow.SetActive(!takeScramStopYellow.activeSelf); }));
                }
           
            }
            else
            {
                if (takeScramStopTimer != null)
                {
                    takeScramStopTimer.Cancel();
                    takeScramStopTimer = null;
                }

                takeScramStopYellow.SetActive(false);
            } 
        }
        
    }
    public void SetSelectState(TMP_Text tmpText )
    {
        if (_lastText!=null)
        {
            _lastText.color = normalColor;
        }

        _lastText = tmpText;
        _lastText.color = selectColor;
    }
    public void UpdateCurrentAccount(object o, EventArgs eventArgs)
    {
        _userTxt.text =GameDataManager.Instance.GetUserName();
    }
}