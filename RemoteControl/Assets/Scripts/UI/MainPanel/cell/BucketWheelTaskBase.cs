using System;
using System.Collections.Generic;
using System.Diagnostics;
using RemoteControl.Event;
using ShenYangRemoteSystem.Subclass;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Debug = UnityEngine.Debug;

public class BucketWheelTaskBase : PanelBase
{
    public ButtonCell scramStopBtn;
    public GameObject scramStopYellowBtn;
    public ButtonCell resetBtn;
    public Button warningBtn;
    public Button confirmWarningBtn;
    public Text[] warningTexts;
    public InputField startTakeMaterText;
    public InputField stopTakeMaterText;
    public ButtonCell leftToggle;
    public ButtonCell rightToggle;
    public ButtonCell resetTaskBtn;
    public ButtonCell AutoMaxToggle;
    public ButtonCell SemiAutoToggle;
    public ButtonCell RightAngleToggle;
    public ButtonCell ObliqueAngleToggle;
    /// <summary>
    /// 左转
    /// </summary>
    public ButtonCell leftTurnToggle;
    /// <summary>
    /// 右转
    /// </summary>
    public ButtonCell rightTurnToggle;
    /// <summary>
    /// 边界确认
    /// </summary>
    public ButtonCell confirmTurnBtn;
    public InputField leftTakeMaterText;
    public InputField rightTakeMaterText;
    public InputField takeMaterStep;
    public InputField layerHigh;
    public InputField timeHourText;
    public InputField timeMinuteText;
    public Toggle timeOpenToggle;
    public Toggle timeCloseToggle;
    public ButtonCell useTimeBtn;
    public InputField takeMaterNum;
    public Toggle quantityOpenToggle;
    public Toggle quantityCloseToggle;
    public ButtonCell takeMaterStartBtn;
    public ButtonCell takeMaterStopBtn;
    public ButtonCell takeMaterReversingBtn;
    public ButtonCell takeMaterEndBtn;
    public ButtonCell curTaskButtonCell;
    public Machine machine;
    private Timer reversingTimer;
    private Timer scramStopTimer;
    public WarningList warningList;
    public TaskLogList taskLogList;
    public ScrollRect taskLogScrollRect;
    public int RefrashLogCount = 0;
    public bool isRefrash;
    public virtual void Start()
    {
        Init();
    }

    public virtual void UpdateDes(List<WarningCellData> datas)
    {
        warningList.RefreshList(datas);
    }

    public virtual void UpdateTaskLog(List<TaskLogCellData> datas)
    {
        // taskLogScrollRect.verticalNormalizedPosition = 0;
        taskLogScrollRect.verticalNormalizedPosition = 0.5f;
        try
        {
            taskLogList.RefreshList(datas);
            taskLogScrollRect.verticalNormalizedPosition = 1f;
        }
        catch (Exception e)
        {
            RefrashLogCount++;
            isRefrash = true;
            StackTrace stackTrace = new StackTrace(e, true);
          
            foreach (var frame in stackTrace.GetFrames())
            {
                Debug.LogError($"Method: {frame.GetMethod().Name}, Line: {frame.GetFileLineNumber()}>>>");
            }
            Debug.LogError($">>>>>>>>>>>>>>>>>>>>{e.Message} {RefrashLogCount}");
        }

        if (isRefrash&&RefrashLogCount<5)
        {
            taskLogList.Refrash();
            taskLogScrollRect.verticalNormalizedPosition =0.4f;
            TaskDataManager.Instance.SetTaskVariables(TaskDataManager.Instance.TaskVariables);
        }
        else
        {
            RefrashLogCount = 0;
        }

        isRefrash = false;
    }
    public void UpdatePlc(SystemVariables data)
    {
        if (machine == Machine.BucketWheelStackerReclaimer)
        {
            scramStopBtn.SetSystemState(data.System_Emergence);
            ScramStopFicker(data.System_Emergence);
            resetBtn.SetSystemState(data.HMI_ErrReset,true);
        }
        else
        {
            scramStopBtn.SetSystemState(data.System_Emergence_2);
            ScramStopFicker(data.System_Emergence_2);
            resetBtn.SetSystemState(data.HMI_ErrReset_2,true);
        }
    }

    public virtual void ScramStopFicker(bool isFicker)
    {
        if (isFicker)
        {
            if (scramStopTimer == null)
            {
                // scramStopTimer.Cancel();
                scramStopTimer = Timer.Register(1, true, true,
                    (() => { scramStopYellowBtn.SetActive(!scramStopYellowBtn.activeSelf); }));
            }
           
        }
        else
        {
            if (scramStopTimer != null)
            {
                scramStopTimer.Cancel();
                scramStopTimer = null;
            }

            scramStopYellowBtn.SetActive(false);
        }
    }

    public virtual void UpdateData(TaskCommand taskCommand)
    {
        startTakeMaterText.text = taskCommand.MaterialRange.startValue.ToString();
        stopTakeMaterText.text = taskCommand.MaterialRange.endValue.ToString();
        if (taskCommand.SideSelection == "LEFT")
        {
            leftToggle.SetSystemState(true,true);
            rightToggle.SetSystemState(false,true);
        }
        else
        {
            leftToggle.SetSystemState(false,true);
            rightToggle.SetSystemState(true,true);
        }
        AutoMaxToggle.SetSystemState(taskCommand.AutoMode==AutoMode.AUTOMAX,true);
        SemiAutoToggle.SetSystemState(taskCommand.AutoMode==AutoMode.SemiAuto,true);
        confirmTurnBtn.gameObject.SetActive(taskCommand.AutoMode==AutoMode.SemiAuto);
        RightAngleToggle.SetSystemState(taskCommand.AngleEntryMode==AngleEntryMode.RIGHTANGLE,true);
        ObliqueAngleToggle.SetSystemState(taskCommand.AngleEntryMode==AngleEntryMode.OBLIQUEANGLE,true);
        leftTurnToggle.SetSystemState(taskCommand.TurnMode==TurnMode.LEFTTURN,true);
        rightTurnToggle.SetSystemState(taskCommand.TurnMode==TurnMode.RIGHTTURN,true);
        leftTakeMaterText.text = taskCommand.LeftRightRange.startValue.ToString();
        rightTakeMaterText.text = taskCommand.LeftRightRange.endValue.ToString();
        takeMaterStep.text = taskCommand.StepLength.ToString();
        // layerHigh.text = taskCommand.LayerHigh.ToString();
        // timeOpenToggle.isOn = taskCommand.IsTimed;
        // useTimeBtn.SetSystemState(taskCommand.IsTimed,true);
        // if (timeOpenToggle.isOn)
        // {
        //     quantityOpenToggle.isOn = false;
        //     timeHourText.text = Mathf.FloorToInt(taskCommand.TimedAt / 60).ToString();
        //     timeMinuteText.text = (taskCommand.TimedAt % 60).ToString();
        // }
        // else
        // {
        //     quantityOpenToggle.isOn = true;
        //     timeOpenToggle.isOn = false;
        //     takeMaterNum.text = taskCommand.Quantity.ToString();
        // }

        takeMaterStartBtn.SetSystemState(taskCommand.AllData.OperationCommandList[0] == 1,true);
        takeMaterStopBtn.SetSystemState(taskCommand.AllData.OperationCommandList[1] == 1,true);
        resetTaskBtn.SetSystemState(taskCommand.ResetState==1,true);
        confirmTurnBtn.SetSystemState(taskCommand.TurnConfirmState==1,true);
        if (taskCommand.AllData.OperationCommandList[2] == 1)
        {
            if (reversingTimer != null)
            {
                reversingTimer?.Cancel();
            }

            reversingTimer = Timer.Register(2, () =>
            {
                takeMaterReversingBtn.SetSystemState(false,true);
                curTaskButtonCell?.SetSelectState(false);
            });
            takeMaterReversingBtn.SetSystemState(true,true);
        }
        else
        {
            takeMaterReversingBtn.SetSystemState(taskCommand.AllData.OperationCommandList[2] == 1,true);
        }

        takeMaterEndBtn.SetSystemState(taskCommand.AllData.OperationCommandList[3] == 1,true);
    }

    public virtual void Init()
    {
        AddOnClickListener(scramStopBtn, (() =>
        {
            {
                if (scramStopBtn.red.activeSelf)
                {
                    UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                        new ConfirmPanelArgs("是否确认复位急停？",GameDataManager.Instance.GetMachineName(machine), null, () =>  SendPlcCommand(COMMAND_NAME.EMERGENCY_STOP)));
                }
                else
                {
                    SendPlcCommand(COMMAND_NAME.EMERGENCY_STOP);
                }
            }
        }));
        AddOnClickListener(resetBtn,
            (() =>
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs("是否确认复位？",GameDataManager.Instance.GetMachineName(machine), null, () => SendPlcCommand(COMMAND_NAME.ERR_RESET)));
            }));
        // AddOnClickListener(warningBtn, (() => { SendPlcCommand(COMMAND_NAME.STARTUP_ALARM); }));
        AddOnClickListener(resetTaskBtn,(() =>
        {
              UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                            new ConfirmPanelArgs("是否重置自动作业参数？", GameDataManager.Instance.GetMachineName(machine),null, () => 
                                SendTaskCommand(OperationType.RESET)));
        }));
        
        AddOnClickListener(confirmTurnBtn, (() =>
        {
            
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否边界确认？",GameDataManager.Instance.GetMachineName(machine), null, () => 
                    SendTaskCommand(OperationType.TurnConfirm)));
        }));
        AddOnClickListener(takeMaterStartBtn, (() =>
        {
            if (leftTurnToggle.red.activeSelf==false&&rightTurnToggle.red.activeSelf==false)
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs("请选择初始设定，稍微重试",GameDataManager.Instance.GetMachineName(machine)));
            }
            else
            {
                // UIManager.Instance.OpenUI(UIID.ConfirmStartTaskPanel,
                //     new ConfirmTaskPanelArgs( null, () => SendTaskCommand(OperationType.START)));
                SendTaskCommand(OperationType.START);
            }
        }));
        AddOnClickListener(takeMaterStopBtn, (() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs(takeMaterStopBtn.red.activeSelf?"是否恢复自动作业？":"是否暂停自动作业？",GameDataManager.Instance.GetMachineName(machine), null, () => SendTaskCommand(OperationType.PAUSE)));
         
        }));
        AddOnClickListener(takeMaterReversingBtn, (() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否换向自动作业？",GameDataManager.Instance.GetMachineName(machine), null, () =>   SendTaskCommand(OperationType.REVERSING)));
          
        }));
        AddOnClickListener(takeMaterEndBtn, (() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否结束自动作业？", GameDataManager.Instance.GetMachineName(machine),null, () =>    SendTaskCommand(OperationType.END)));
          
        }));
        confirmWarningBtn.onClick.AddListener((() =>
        {
             GameDataManager.Instance.UpdateWarningConfirmTime(machine);
             GameDataManager.Instance.UpdatePlcWarningRecordData();
        }));
        quantityOpenToggle.onValueChanged.AddListener(((bool isOn) =>
        {
            if (isOn && timeOpenToggle.isOn)
            {
                timeCloseToggle.isOn = true;
            }
        }));
        timeOpenToggle.onValueChanged.AddListener((((bool isOn) =>
        {
            if (isOn && quantityOpenToggle.isOn)
            {
                quantityCloseToggle.isOn = true;
            }
        })));
        AddOnClickListener(leftToggle,(() =>
        {
            leftToggle.SetSystemState(true,true);
            rightToggle.SetSystemState(false,true);
        } ));
        AddOnClickListener(rightToggle,(() =>
        {
            rightToggle.SetSystemState(true,true);
            leftToggle.SetSystemState(false,true);
        } ));
        
        AddOnClickListener(AutoMaxToggle,(() =>
        {
            AutoMaxToggle.SetSystemState(true,true);
            SemiAutoToggle.SetSystemState(false,true);
            confirmTurnBtn.gameObject.SetActive(false);
        } ));
        AddOnClickListener(SemiAutoToggle,(() =>
        {
            AutoMaxToggle.SetSystemState(false,true);
            SemiAutoToggle.SetSystemState(true,true);
            confirmTurnBtn.gameObject.SetActive(true);
        } ));
        
        AddOnClickListener(RightAngleToggle,(() =>
        {
            RightAngleToggle.SetSystemState(true,true);
            ObliqueAngleToggle.SetSystemState(false,true);
        } ));
        
        AddOnClickListener(ObliqueAngleToggle,(() =>
        {
            RightAngleToggle.SetSystemState(false,true);
            ObliqueAngleToggle.SetSystemState(true,true);
        } ));
        
        AddOnClickListener(leftTurnToggle,(() =>
        {
            leftTurnToggle.SetSystemState(true,true);
            rightTurnToggle.SetSystemState(false,true);
        } ));
        AddOnClickListener(rightTurnToggle,(() =>
        {
            leftTurnToggle.SetSystemState(false,true);
            rightTurnToggle.SetSystemState(true,true);
        } ));
        // AddOnClickListener(useTimeBtn,(() =>
        // {
        //     useTimeBtn.SetSystemState(!useTimeBtn.red.activeSelf,true);
        // }));
        if (machine==Machine.BucketWheelStackerReclaimer)
        {
            InputFieldValueRange(startTakeMaterText, 0, 260,0);
            InputFieldValueRange(stopTakeMaterText, 0, 260,0);
        }
        else
        {
            InputFieldValueRange(startTakeMaterText, 153, 323,153);
            InputFieldValueRange(stopTakeMaterText, 153, 323,323);
        }
       
        InputFieldValueRange(leftTakeMaterText, 12, 90,12);
        InputFieldValueRange(rightTakeMaterText, 12, 90,90);
        InputFieldValueRange(timeHourText, 0, 99,0);
        InputFieldValueRange(timeMinuteText, 0, 60,0);
        InputFieldValueRange(takeMaterStep, 0.1f, 3,0.7f);
        InputFieldValueRange(takeMaterNum, 0, 99999,0);
        InputFieldValueRange(layerHigh, 0, 10,0);
        EventManager.Instance.TriggerEvent(EventName.UpdatePcData, null);
    }

    public virtual void WarningDown()
    {
        Debug.LogError("按下"); //1
        warningBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        warningBtn.transform.Find("Image").gameObject.SetActive(true);
        warningBtn.transform.FindComponent<Text>("Text").color = new Color(1, 0, 0.1803922f, 1);
        SendPlcCommand(COMMAND_NAME.STARTUP_ALARM, 1);
    }

    public virtual void WarningUp()
    {
        Debug.LogError("抬起"); //0
        warningBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        warningBtn.transform.Find("Image").gameObject.SetActive(false);
        warningBtn.transform.FindComponent<Text>("Text").color =new Color(0.1411765f, 1, 1, 1);
        SendPlcCommand(COMMAND_NAME.STARTUP_ALARM, 0);
    }

    public virtual void SendPlcCommand(COMMAND_NAME mCommandName, int dataInt = 0)
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
        if (mCommandName == COMMAND_NAME.EMERGENCY_STOP)
        {
            scramStopBtn.SetSelectState(!scramStopBtn.select.activeSelf);
            dataInt = scramStopBtn.red.activeSelf ? 0 : 1;
        }
        else if (COMMAND_NAME.ERR_RESET == mCommandName)
        {
            resetBtn.SetSelectState(true, 2);
            dataInt = resetBtn.red.activeSelf ? 0 : 1; //HMI_ErrReset
        }
        else if (COMMAND_NAME.STARTUP_ALARM == mCommandName)
        {
        }

        Debug.Log($" commandName {commandName} {dataInt}");
        GameDataManager.Instance.SendServerCommandByName(commandName, dataInt);
    }

    public virtual void SendTaskCommand(OperationType operationType)
    {
        if (GameDataManager.Instance.GameMain.connectionPC.isConnect == false)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs(ConstStr.TASK_SERVER_CONNECTION_FAIL_TIP,GameDataManager.Instance.GetMachineName(machine)));
            return;
        }

        if (TaskDataManager.Instance.IsCanSendTaskCommond(machine, TaskType.TAKEMATER, operationType) != -1)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel, new ConfirmPanelArgs("当前操作无效的",GameDataManager.Instance.GetMachineName(machine)));
            return;
        }
        TaskCommand taskCommand = new TaskCommand();
        if (operationType == OperationType.START)
        {
            DataManager.Instance.InsertHistoryLogMc("自动取料-启动任务", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.OperationCommand = operationType;
            UpdateCurCtrMode(ref curTaskButtonCell, takeMaterStartBtn);
        }
        else if (operationType == OperationType.PAUSE)
        {
            DataManager.Instance.InsertHistoryLogMc(takeMaterStopBtn.red.activeSelf?"自动取料-恢复任务":"自动取料-暂停任务", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.OperationCommand = operationType;
            UpdateCurCtrMode(ref curTaskButtonCell, takeMaterStopBtn);
            // int dataInt = takeMaterStopBtn.red.activeSelf?0:1;
            // string commandName=machine==Machine.BucketWheelStackerReclaimer?COMMAND_NAME.TAKE_PAUSE.ToString()+"_1":COMMAND_NAME.TAKE_PAUSE.ToString()+"_2";
            // GameDataManager.Instance.SendServerCommandRC(commandName,6,2,dataInt);
        }
        else if (operationType == OperationType.REVERSING)
        {
            DataManager.Instance.InsertHistoryLogMc("自动取料-任务换向", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.OperationCommand = operationType;
            UpdateCurCtrMode(ref curTaskButtonCell, takeMaterReversingBtn);
        }
        else if (operationType == OperationType.END)
        {
            DataManager.Instance.InsertHistoryLogMc("自动取料-任务结束", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.OperationCommand = operationType;
            UpdateCurCtrMode(ref curTaskButtonCell, takeMaterEndBtn);
        }else if (operationType == OperationType.RESET)
        {
            resetTaskBtn.SetSelectState(true);
            DataManager.Instance.InsertHistoryLogMc("自动取料-任务重置", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.ResetState = 1;
        }else if (operationType==OperationType.TurnConfirm)
        {
            confirmTurnBtn.SetSelectState(true);
            DataManager.Instance.InsertHistoryLogMc("自动取料-边界确认", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.TurnConfirmState = 1;
            // taskCommand.OperationCommand = operationType;
        }
        taskCommand.QuerySystem = "MC";
        //taskCommand.Command_Type = 0;
        taskCommand.TaskType = TaskType.TAKEMATER;
        taskCommand.Machine = machine;
        taskCommand.OperatorName = GameDataManager.Instance.GetUserName();
        taskCommand.TaskCreateTime = DateTime.Now; //.ToString("yyyy-MM-dd HH:mm:ss")
        taskCommand.OperatorSystem = "MC";
        taskCommand.FinishMethod =new List<int>(){0,0};
        if (operationType == OperationType.START || operationType == OperationType.RESET)
        {
            taskCommand.AutoMode = AutoMaxToggle.red.activeSelf ? AutoMode.AUTOMAX : AutoMode.SemiAuto;
            taskCommand.AngleEntryMode=RightAngleToggle.red.activeSelf?AngleEntryMode.RIGHTANGLE:AngleEntryMode.OBLIQUEANGLE;
            taskCommand.Command_Type = operationType == OperationType.RESET?2:0;
            taskCommand.TurnMode=leftTurnToggle.red.activeSelf?TurnMode.LEFTTURN:TurnMode.RIGHTTURN;
            float startValue = startTakeMaterText.text == "" ? 0 : float.Parse(startTakeMaterText.text);
            float endValue = stopTakeMaterText.text == "" ? 0 : float.Parse(stopTakeMaterText.text);
            taskCommand.MaterialRange = new TaskRange(startValue, endValue);
            taskCommand.SideSelection = leftToggle.red.activeSelf ? "LEFT" : "RIGHT";
            float startLeftRightRangeValue = leftTakeMaterText.text == "" ? 0 : float.Parse(leftTakeMaterText.text);
            float endLeftRightRangeValue = rightTakeMaterText.text == "" ? 0 : float.Parse(rightTakeMaterText.text);
            taskCommand.LeftRightRange = new TaskRange(startLeftRightRangeValue, endLeftRightRangeValue);
            taskCommand.StepLength = takeMaterStep.text == "" ? 0 : float.Parse(takeMaterStep.text);
            // taskCommand.IsTimed = false;//useTimeBtn.red.activeSelf;
            // taskCommand.TimedAt = 0;// int.Parse(timeHourText.text) * 60 + int.Parse(timeMinuteText.text);
            // taskCommand.IsQuantified = false;// quantityOpenToggle.isOn;
            // taskCommand.Quantity = 0;// int.Parse(takeMaterNum.text);
            taskCommand.TaskID = DateTime.Now.ToString("yyMMddHHmmss");
            // taskCommand.LayerHigh = layerHigh.text == "" ? 0 : float.Parse(layerHigh.text);
            // taskCommand.TakeMateHigh = 0;
            taskCommand.PileMateHigh = 0;
            AllData allData = new AllData();
            taskCommand.AllData = allData;
        }else if (operationType == OperationType.TurnConfirm )
        {
            taskCommand.Command_Type = 3;
        }
        else
        {
            taskCommand.Command_Type = 2;
        }

        if (operationType== OperationType.END)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmTaskPanel, new ConfirmTaskPanelArgs(taskCommand,GameDataManager.Instance.GetMachineName(machine)));
        }else if (operationType==OperationType.START)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmStartTaskPanel,
                new ConfirmTaskPanelArgs(taskCommand,GameDataManager.Instance.GetMachineName(machine)));
        }
        else
        {
            TaskDataManager.Instance.SendTaskCommand(taskCommand);
        }
     
    }

    public virtual void UpdateCurCtrMode(ref ButtonCell ctr, ButtonCell btn)
    {
        if (ctr != null)
        {
            ctr.SetSelectState(false);
        }

        ctr = btn;
        ctr.SetSelectState(true);
    }

    public virtual void ResetState()
    {
        takeMaterStartBtn.SetSystemState(false,true);
        takeMaterStopBtn.SetSystemState(false,true);
        takeMaterReversingBtn.SetSystemState(false,true);
        takeMaterEndBtn.SetSystemState(false,true);
        takeMaterStartBtn.SetSelectState(false);
        takeMaterStopBtn.SetSelectState(false);
        takeMaterReversingBtn.SetSelectState(false);
        takeMaterEndBtn.SetSelectState(false);
        resetBtn.SetSelectState(false);
        resetBtn.SetSystemState(false,true);
        curTaskButtonCell?.SetSelectState(false);

        AutoMaxToggle.SetSystemState(false, true);
        SemiAutoToggle.SetSystemState(true, true);
        RightAngleToggle.SetSystemState(false, true);
        ObliqueAngleToggle.SetSystemState(true, true);
        
        // leftTurnToggle.SetSystemState(false, true);
        // rightTurnToggle.SetSystemState(false, true);
        confirmTurnBtn.SetSystemState(false,true);
    }

    public virtual void InputFieldValueRange(InputField inputField, float min, float max,float defaultValue)
    {
        inputField.text =defaultValue.ToString();
        inputField.onEndEdit.AddListener(((string value) =>
        {
            float num = 0;
            if (float.TryParse(value, out num))
            {
                num = num < min ? min : num;
                num = num > max ? max : num;
            }

            inputField.text = num.ToString();
        }));
    }
}