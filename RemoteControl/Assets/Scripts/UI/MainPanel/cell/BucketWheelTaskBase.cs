using System;
using System.Collections.Generic;
using System.Diagnostics;
using RemoteControl.Event;
using ShenYangRemoteSystem.Subclass;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Debug = UnityEngine.Debug;

public enum SymbolType
{
    ADD,SUB
}
public enum InputFieldType
{
    LEFTRANGE,RIGHTRANGE,STEPVALUE,ANGLEVALUE
}
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
    public ButtonCell ShuntToggle;
    public ButtonCell RightAngleToggle;
    public ButtonCell ObliqueAngleToggle;
    public InputField AngleEntryText;
    public Button angleEntryAddBtn;
    public Button angleEntrySubBtn;
    public ButtonCell EntryModeClose;
    public ButtonCell EntryModeOpen;
    public GameObject entryModeGo;
    public GameObject rootEntryModeGo;
    public GameObject leftRightTurnGo;

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

    /// <summary>
    /// 定位确认
    /// </summary>
    public ButtonCell PositionConfirmBtn;

    /// <summary>
    /// 两短一长
    /// </summary>
    public ButtonCell TwoShortOneLongBtn;
    
    public InputField leftTakeMaterText;
    public Button leftTakeMaterAddBtn;
    public Button leftTakeMaterSubBtn;
    public InputField rightTakeMaterText;
    public Button rightTakeMaterAddBtn;
    public Button rightTakeMaterSubBtn;
    public InputField takeMaterStep;
    public Button takeMaterStepAddBtn;
    public Button takeMaterStepSubBtn;
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
    public AddSubPanel addSubPanel;
    private ButtonCell curTaskButtonCell;
    private Dictionary<InputFieldType,List<float>> _dictionary=new Dictionary<InputFieldType, List<float>>()
    {
        {InputFieldType.LEFTRANGE,new List<float>(){0.5f, 1 ,   2 ,  5   , 10}},
        {InputFieldType.RIGHTRANGE,new List<float>(){0.5f, 1 ,   2 ,  5   , 10}},
        {InputFieldType.STEPVALUE,new List<float>(){0.1f,   0.2f ,  0.3f,   0.4f  , 0.5f}},
        {InputFieldType.ANGLEVALUE,new List<float>(){0.1f  , 0.2f ,  0.5f,   1f ,  2f}}
    };
    public Machine machine;
    private Timer reversingTimer;
    private Timer scramStopTimer;
    public WarningList warningList;
    public TaskLogList taskLogList;
    public ScrollRect taskLogScrollRect;
    private int RefrashLogCount = 0;
    private bool isRefrash;
    private bool isInitData;
    private Timer refreshResetTextTimer;
    private bool isRefreshResetText;

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

        if (isRefrash && RefrashLogCount < 5)
        {
            taskLogList.Refrash();
            taskLogScrollRect.verticalNormalizedPosition = 0.4f;
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
            resetBtn.SetSystemState(data.HMI_ErrReset, true);
        }
        else
        {
            scramStopBtn.SetSystemState(data.System_Emergence_2);
            ScramStopFicker(data.System_Emergence_2);
            resetBtn.SetSystemState(data.HMI_ErrReset_2, true);
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
        // Debug.Log($"任务数据刷新 {taskCommand.Machine} isRefreshUI:{taskCommand.AllData.isRefreshUI} isInitData:{isInitData} List[3]:{taskCommand.AllData.OperationCommandList[3]} activeSelf：{takeMaterEndBtn.red.activeSelf}");
        if (taskCommand.AllData.isRefreshUI == 0 && isInitData)
        {
            return;
        }
        if (taskCommand.AllData.OperationCommandList[3] == 1 && takeMaterEndBtn.red.activeSelf)
        {
            return;
        }

        isInitData = true;
        if (taskCommand.AllData.OperationCommandList[3] == 1)
        {
            takeMaterStartBtn.SetSystemState(taskCommand.AllData.OperationCommandList[0] == 1, true);
            takeMaterStopBtn.SetSystemState(taskCommand.AllData.OperationCommandList[1] == 1, true);
            takeMaterEndBtn.SetSystemState(taskCommand.AllData.OperationCommandList[3] == 1, true);
            takeMaterReversingBtn.SetSystemState(taskCommand.AllData.OperationCommandList[2] == 1, true);
            resetTaskBtn.SetSystemState(false, true);
            // PositionConfirmBtn.SetSystemState(false, true);
            TwoShortOneLongBtn.SetSystemState(false, true);
        }
        else
        {
            if (isRefreshResetText)
            {
                AngleEntryText.SetTextByFocused(taskCommand.AngleEntryValue.ToString());
                leftTakeMaterText.SetTextByFocused(taskCommand.LeftRightRange.startValue.ToString());
                rightTakeMaterText.SetTextByFocused(taskCommand.LeftRightRange.endValue.ToString());
                takeMaterStep.SetTextByFocused(taskCommand.StepLength.ToString());
            }
            startTakeMaterText.SetTextByFocused(taskCommand.MaterialRange.startValue.ToString());
            stopTakeMaterText.SetTextByFocused(taskCommand.MaterialRange.endValue.ToString());
            if (taskCommand.SideSelection == "LEFT")
            {
                leftToggle.SetSystemState(true, true);
                rightToggle.SetSystemState(false, true);
            }
            else
            {
                leftToggle.SetSystemState(false, true);
                rightToggle.SetSystemState(true, true);
            }

            AutoMaxToggle.SetSystemState(taskCommand.AutoMode == AutoMode.AUTOMAX, true);
            entryModeGo.SetActive(taskCommand.AutoMode == AutoMode.AUTOMAX);
            SemiAutoToggle.SetSystemState(taskCommand.AutoMode == AutoMode.SemiAuto, true);
            ShuntToggle.SetSystemState(taskCommand.AutoMode == AutoMode.Shunt, true);
            rootEntryModeGo.SetActive(taskCommand.AutoMode == AutoMode.SemiAuto);
            leftRightTurnGo.SetActive(taskCommand.AutoMode != AutoMode.Shunt);
            confirmTurnBtn.gameObject.SetActive(taskCommand.AutoMode == AutoMode.SemiAuto);
            // PositionConfirmBtn.gameObject.SetActive(taskCommand.AutoMode == AutoMode.AUTOMAX);
            TwoShortOneLongBtn.gameObject.SetActive(taskCommand.AutoMode == AutoMode.SemiAuto);
            
            leftTurnToggle.SetSystemState(taskCommand.TurnMode == TurnMode.LEFTTURN, true);
            rightTurnToggle.SetSystemState(taskCommand.TurnMode == TurnMode.RIGHTTURN, true);
            takeMaterStartBtn.SetSystemState(taskCommand.AllData.OperationCommandList[0] == 1, true);
            takeMaterStopBtn.SetSystemState(taskCommand.AllData.OperationCommandList[1] == 1, true);
            resetTaskBtn.SetSystemState(taskCommand.ResetState == 1, true);
            confirmTurnBtn.SetSystemState(taskCommand.TurnConfirmState == 1, true);
            // PositionConfirmBtn.SetSystemState(taskCommand.PositionConfirmState == 1, true);
            TwoShortOneLongBtn.SetSystemState(taskCommand.TwoShortOneLongState==1, true);
            EntryModeOpen.SetSystemState(taskCommand.IsUseAngleEntryValue == 1, true);
            EntryModeClose.SetSystemState(taskCommand.IsUseAngleEntryValue == 0, true);
            if (taskCommand.AllData.OperationCommandList[2] == 1)
            {
                if (reversingTimer != null)
                {
                    reversingTimer?.Cancel();
                }

                reversingTimer = Timer.Register(2, () =>
                {
                    takeMaterReversingBtn.SetSystemState(false, true);
                    curTaskButtonCell?.SetSelectState(false);
                });
                takeMaterReversingBtn.SetSystemState(true, true);
            }
            else
            {
                takeMaterReversingBtn.SetSystemState(taskCommand.AllData.OperationCommandList[2] == 1, true);
            }

            takeMaterEndBtn.SetSystemState(taskCommand.AllData.OperationCommandList[3] == 1, true);
        }
    }

    public virtual void Init()
    {
        isRefreshResetText = true;
        AddOnClickListener(scramStopBtn, (() =>
        {
            {
                if (scramStopBtn.red.activeSelf)
                {
                    UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                        new ConfirmPanelArgs("是否确认复位急停？", GameDataManager.Instance.GetMachineName(machine), null,
                            () => SendPlcCommand(COMMAND_NAME.EMERGENCY_STOP)));
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
                    new ConfirmPanelArgs("是否确认复位？", GameDataManager.Instance.GetMachineName(machine), null,
                        () => SendPlcCommand(COMMAND_NAME.ERR_RESET)));
            }));
        AddOnClickListener(resetTaskBtn, (() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否重置自动作业参数？", GameDataManager.Instance.GetMachineName(machine), null, () =>
                    SendTaskCommand(OperationType.RESET)));
        }));

        AddOnClickListener(confirmTurnBtn, (() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否边界确认？", GameDataManager.Instance.GetMachineName(machine), null, () =>
                    SendTaskCommand(OperationType.TurnConfirm)));
        }));

        // AddOnClickListener(PositionConfirmBtn, (() =>
        // {
        //     // UIManager.Instance.OpenUI(UIID.ConfirmPanel,
        //     //     new ConfirmPanelArgs("是否定位确认？", GameDataManager.Instance.GetMachineName(machine), null, () =>
        //     //         SendTaskCommand(OperationType.PositionConfirm)));
        //     if (IsCanSet())
        //     {
        //         PositionConfirmBtn.SetSystemState(!PositionConfirmBtn.red.activeSelf, true);
        //     }
        //     else
        //     {
        //         PositionConfirmBtn.SetSelectState(true);
        //     }
        //   
        // }));
        
        AddOnClickListener(TwoShortOneLongBtn,(() =>
        {
            
            if (IsCanSet())
            {
                TwoShortOneLongBtn.SetSystemState(!TwoShortOneLongBtn.red.activeSelf, true);
            }
            else
            {
                SendTaskCommand(OperationType.TwoShortOneLong);
            }
        }));
        AddOnClickListener(takeMaterStartBtn, (() =>
        {
            if (leftTurnToggle.red.activeSelf == false && rightTurnToggle.red.activeSelf == false&& ShuntToggle.red.activeSelf==false)
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs("请选择初始设定，稍微重试", GameDataManager.Instance.GetMachineName(machine)));
            }
            else
            {
                SendTaskCommand(OperationType.START);
            }
        }));
        AddOnClickListener(takeMaterStopBtn, (() =>
        {
            if (takeMaterStopBtn.red.activeSelf)
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs(takeMaterStopBtn.red.activeSelf ? "是否恢复自动作业？" : "是否暂停自动作业？",
                        GameDataManager.Instance.GetMachineName(machine), null,
                        () => SendTaskCommand(OperationType.PAUSE)));
            }
            else
            {
                SendTaskCommand(OperationType.PAUSE);
            }
        }));
        AddOnClickListener(takeMaterReversingBtn, (() =>
        {
            // UIManager.Instance.OpenUI(UIID.ConfirmPanel,
            //     new ConfirmPanelArgs("是否换向自动作业？",GameDataManager.Instance.GetMachineName(machine), null, () =>   SendTaskCommand(OperationType.REVERSING)));
            SendTaskCommand(OperationType.REVERSING);
        }));
        AddOnClickListener(takeMaterEndBtn, (() =>
        {
            if (ShuntToggle.red.activeSelf==true)
            {
                SendTaskCommand(OperationType.END);
            }
            else
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs("是否结束自动作业？", GameDataManager.Instance.GetMachineName(machine), null,
                        () => SendTaskCommand(OperationType.END)));
            }
          
        }));
        AddOnClickListener(EntryModeOpen, (() =>
        {
            if (IsCanSet())
            {
                EntryModeOpen.SetSystemState(true, true);
                EntryModeClose.SetSystemState(false, true);
            }
            else
            {
                EntryModeOpen.SetSelectState(true);
            }
        }));
        AddOnClickListener(EntryModeClose, (() =>
        {
            if (IsCanSet())
            {
                EntryModeOpen.SetSystemState(false, true);
                EntryModeClose.SetSystemState(true, true);
            }
            else
            {
                EntryModeClose.SetSelectState(true);
            }
        }));
        confirmWarningBtn.onClick.AddListener((() =>
        {
            AlarmDataManager.Instance.UpdateWarningConfirmTime(machine);
            AlarmDataManager.Instance.UpdatePlcWarningRecordData();
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
        AddOnClickListener(leftToggle, (() =>
        {
            if (IsCanSet())
            {
                leftToggle.SetSystemState(true, true);
                rightToggle.SetSystemState(false, true);
            }
            else
            {
                leftToggle.SetSelectState(true);
            }
        }));
        AddOnClickListener(rightToggle, (() =>
        {
            if (IsCanSet())
            {
                rightToggle.SetSystemState(true, true);
                leftToggle.SetSystemState(false, true);
            }
            else
            {
                rightToggle.SetSelectState(true);
            }
        }));

        AddOnClickListener(AutoMaxToggle, (() =>
        {
            if (IsCanSet())
            {
                AutoMaxToggle.SetSystemState(true, true);
                SemiAutoToggle.SetSystemState(false, true);
                ShuntToggle.SetSystemState(false,true);
                entryModeGo.SetActive(false);
                rootEntryModeGo.SetActive(false);
                leftRightTurnGo.SetActive(true);
                confirmTurnBtn.gameObject.SetActive(false);
                // PositionConfirmBtn.gameObject.SetActive(false);
                TwoShortOneLongBtn.gameObject.SetActive(false);
            }
            else
            {
                AutoMaxToggle.SetSelectState(true);
            }
        }));
        AddOnClickListener(SemiAutoToggle, (() =>
        {
            if (IsCanSet())
            {
                AutoMaxToggle.SetSystemState(false, true);
                SemiAutoToggle.SetSystemState(true, true);
                ShuntToggle.SetSystemState(false, true);
                entryModeGo.SetActive(false);
                rootEntryModeGo.SetActive(true);
                leftRightTurnGo.SetActive(true);
                confirmTurnBtn.gameObject.SetActive(true);
                // PositionConfirmBtn.gameObject.SetActive(false);
                TwoShortOneLongBtn.gameObject.SetActive(true);
            }
            else
            {
                SemiAutoToggle.SetSelectState(true);
            }
        }));
        AddOnClickListener(ShuntToggle,(() =>
        {
            if (IsCanSet())
            {
                AutoMaxToggle.SetSystemState(false, true);
                SemiAutoToggle.SetSystemState(false, true);
                ShuntToggle.SetSystemState(true, true);
                entryModeGo.SetActive(false);
                rootEntryModeGo.SetActive(false);
                leftRightTurnGo.SetActive(false);
                confirmTurnBtn.gameObject.SetActive(false);
                // PositionConfirmBtn.gameObject.SetActive(false);
                TwoShortOneLongBtn.gameObject.SetActive(false);
            }
            else
            {
                ShuntToggle.SetSelectState(true);
            }
        }));
        AddOnClickListener(leftTurnToggle, (() =>
        {
            if (IsCanSet())
            {
                leftTurnToggle.SetSystemState(true, true);
                rightTurnToggle.SetSystemState(false, true);
            }
            else
            {
                leftTurnToggle.SetSelectState(true);
            }
        }));
        AddOnClickListener(rightTurnToggle, (() =>
        {
            if (IsCanSet())
            {
                leftTurnToggle.SetSystemState(false, true);
                rightTurnToggle.SetSystemState(true, true);
            }
            else
            {
                rightTurnToggle.SetSelectState(true);
            }
        }));
        leftTakeMaterAddBtn.onClick.AddListener((() =>
        {
            SetInputFieldByAddSubBtn(InputFieldType.LEFTRANGE, SymbolType.ADD, leftTakeMaterAddBtn.transform.position);
        }));
        leftTakeMaterSubBtn.onClick.AddListener((() =>
        {
            SetInputFieldByAddSubBtn(InputFieldType.LEFTRANGE, SymbolType.SUB,leftTakeMaterSubBtn.transform.position);
        }));
        
        rightTakeMaterAddBtn.onClick.AddListener((() =>
        {
            SetInputFieldByAddSubBtn(InputFieldType.RIGHTRANGE, SymbolType.ADD,rightTakeMaterAddBtn.transform.position);
        }));
        rightTakeMaterSubBtn.onClick.AddListener((() =>
        {
            SetInputFieldByAddSubBtn(InputFieldType.RIGHTRANGE, SymbolType.SUB,rightTakeMaterSubBtn.transform.position);
        }));
        
        takeMaterStepAddBtn.onClick.AddListener((() =>
        {
            SetInputFieldByAddSubBtn(InputFieldType.STEPVALUE, SymbolType.ADD, takeMaterStepAddBtn.transform.position);
        }));
        takeMaterStepSubBtn.onClick.AddListener((() =>
        {
            SetInputFieldByAddSubBtn(InputFieldType.STEPVALUE, SymbolType.SUB,takeMaterStepSubBtn.transform.position);
        }));
        
        angleEntryAddBtn.onClick.AddListener((() =>
        {
            SetInputFieldByAddSubBtn(InputFieldType.ANGLEVALUE, SymbolType.ADD,angleEntryAddBtn.transform.position);
        }));
        angleEntrySubBtn.onClick.AddListener((() =>
        {
            SetInputFieldByAddSubBtn(InputFieldType.ANGLEVALUE, SymbolType.SUB,angleEntrySubBtn.transform.position);
        }));
        
        if (machine == Machine.BucketWheelStackerReclaimer)
        {
            InputFieldValueRange(startTakeMaterText, 0, 265, 0);
            InputFieldValueRange(stopTakeMaterText, 0, 265, 0);
        }
        else
        {
            InputFieldValueRange(startTakeMaterText, 153, 330, 153);
            InputFieldValueRange(stopTakeMaterText, 153, 330, 330);
        }

        PositionConfirmBtn.gameObject.SetActive(false);
        entryModeGo.SetActive(false);
        InputFieldValueRange(leftTakeMaterText, 12, 90, 12);
        InputFieldValueRange(rightTakeMaterText, 12, 90, 90);
        InputFieldValueRange(timeHourText, 0, 99, 0);
        InputFieldValueRange(timeMinuteText, 0, 60, 0);
        InputFieldValueRange(takeMaterStep, 0.1f, 3, 0.7f);
        InputFieldValueRange(takeMaterNum, 0, 99999, 0);
        InputFieldValueRange(layerHigh, 0, 10, 0);
        InputFieldValueRange(AngleEntryText, 0, 120, 1);
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
        warningBtn.transform.FindComponent<Text>("Text").color = new Color(0.1411765f, 1, 1, 1);
        SendPlcCommand(COMMAND_NAME.STARTUP_ALARM, 0);
    }

    public virtual void SendPlcCommand(COMMAND_NAME mCommandName, int dataInt = 0)
    {
        if (GameDataManager.Instance.GameMain.connectionRC.isConnect == false)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP,
                    GameDataManager.Instance.GetMachineName(machine)));
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
                new ConfirmPanelArgs(ConstStr.TASK_SERVER_CONNECTION_FAIL_TIP,
                    GameDataManager.Instance.GetMachineName(machine)));
            return;
        }

        if (TaskDataManager.Instance.IsCanSendTaskCommond(machine, TaskType.TAKEMATER, operationType) != -1)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("当前操作无效的", GameDataManager.Instance.GetMachineName(machine)));
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
            DataManager.Instance.InsertHistoryLogMc(takeMaterStopBtn.red.activeSelf ? "自动取料-恢复任务" : "自动取料-暂停任务",
                GameDataManager.Instance.GetUserName(), machine);
            taskCommand.OperationCommand = operationType;
            UpdateCurCtrMode(ref curTaskButtonCell, takeMaterStopBtn);
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
        }
        else if (operationType == OperationType.RESET)
        {
            resetTaskBtn.SetSelectState(true);
            DataManager.Instance.InsertHistoryLogMc("自动取料-任务重置", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.ResetState = 1;
        }
        else if (operationType == OperationType.TurnConfirm)
        {
            confirmTurnBtn.SetSelectState(true);
            DataManager.Instance.InsertHistoryLogMc("自动取料-边界确认", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.TurnConfirmState = 1;
        }
        else if (operationType == OperationType.PositionConfirm)
        {
            PositionConfirmBtn.SetSelectState(true);
            DataManager.Instance.InsertHistoryLogMc("自动取料-定位确认", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.PositionConfirmState = 1;
        }else if (operationType==OperationType.TwoShortOneLong)
        {
            TwoShortOneLongBtn.SetSelectState(true);
            DataManager.Instance.InsertHistoryLogMc("自动取料-两长一短", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.TwoShortOneLongState=TwoShortOneLongBtn.red.activeSelf ? 0 : 1;
        }

        taskCommand.QuerySystem = "MC";
        taskCommand.TaskType = TaskType.TAKEMATER;
        taskCommand.Machine = machine;
        taskCommand.OperatorName = GameDataManager.Instance.GetUserID();
        taskCommand.TaskCreateTime = DateTime.Now; //.ToString("yyyy-MM-dd HH:mm:ss")
        taskCommand.OperatorSystem = "MC";
        taskCommand.FinishMethod = new List<int>() { 0, 0 };
        if (operationType == OperationType.START || operationType == OperationType.RESET)
        {
            if (AutoMaxToggle.red.activeSelf)
            {
                taskCommand.AutoMode = AutoMode.AUTOMAX;
            }else if (SemiAutoToggle.red.activeSelf)
            {
                taskCommand.AutoMode = AutoMode.SemiAuto;
            }else if (ShuntToggle.red.activeSelf)
            {
                taskCommand.AutoMode = AutoMode.Shunt;
            }
            // taskCommand.AngleEntryMode=RightAngleToggle.red.activeSelf?AngleEntryMode.RIGHTANGLE:AngleEntryMode.OBLIQUEANGLE;
            taskCommand.AngleEntryValue = AngleEntryText.text == "" ? 0 : float.Parse(AngleEntryText.text);
            taskCommand.Command_Type = operationType == OperationType.RESET ? 2 : 0;
            taskCommand.TurnMode = leftTurnToggle.red.activeSelf ? TurnMode.LEFTTURN : TurnMode.RIGHTTURN;
            float startValue = startTakeMaterText.text == "" ? 0 : float.Parse(startTakeMaterText.text);
            float endValue = stopTakeMaterText.text == "" ? 0 : float.Parse(stopTakeMaterText.text);
            taskCommand.MaterialRange = new TaskRange(startValue, endValue);
            taskCommand.SideSelection = leftToggle.red.activeSelf ? "LEFT" : "RIGHT";
            float startLeftRightRangeValue = leftTakeMaterText.text == "" ? 0 : float.Parse(leftTakeMaterText.text);
            float endLeftRightRangeValue = rightTakeMaterText.text == "" ? 0 : float.Parse(rightTakeMaterText.text);
            taskCommand.LeftRightRange = new TaskRange(startLeftRightRangeValue, endLeftRightRangeValue);
            taskCommand.StepLength = takeMaterStep.text == "" ? 0 : float.Parse(takeMaterStep.text);
            taskCommand.TaskID = DateTime.Now.ToString("yyMMddHHmmss");
            taskCommand.PileMateHigh = 0;
            AllData allData = new AllData();
            taskCommand.AllData = allData;
            taskCommand.IsUseAngleEntryValue = EntryModeOpen.red.activeSelf ? 1 : 0;
            // taskCommand.PositionConfirmState =PositionConfirmBtn.red.activeSelf ? 1 : 0;
            taskCommand.TwoShortOneLongState =TwoShortOneLongBtn.red.activeSelf ? 1 : 0;
        }
        else if (operationType == OperationType.TurnConfirm)
        {
            taskCommand.Command_Type = 3;
        }
        else if (operationType == OperationType.PositionConfirm)
        {
            taskCommand.Command_Type = 5;
        } 
        else if (operationType == OperationType.TwoShortOneLong)
        {
            taskCommand.Command_Type = 6;
        }
        else
        {
            taskCommand.Command_Type = 2;
        }

        if (operationType == OperationType.END)
        {
            if (ShuntToggle.red.activeSelf==true)
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs("是否结束调车作业?", GameDataManager.Instance.GetMachineName(machine),null,(() =>
                    {
                        TaskDataManager.Instance.SendTaskCommand(taskCommand);
                    })));
            }
            else
            {
                UIManager.Instance.OpenUI(UIID.ConfirmTaskPanel,
                    new ConfirmTaskPanelArgs(taskCommand, GameDataManager.Instance.GetMachineName(machine)));
            }
          
        }
        else if (operationType == OperationType.START)
        {
            if ( AutoMaxToggle.red.activeSelf)
            {
                UIManager.Instance.OpenUI(UIID.ConfirmStartTaskPanel,
                    new ConfirmTaskPanelArgs(taskCommand, GameDataManager.Instance.GetMachineName(machine)));
                // if (PositionConfirmBtn.red.activeSelf)//定位确认需要二次弹窗
                // {
                //     UIManager.Instance.OpenUI(UIID.ConfirmStartTaskPanel,
                //         new ConfirmTaskPanelArgs(taskCommand, GameDataManager.Instance.GetMachineName(machine)));
                // }
                // else
                // {
                //     TaskDataManager.Instance.SendTaskCommand(taskCommand);
                // }
            }else if (ShuntToggle.red.activeSelf)
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs("是否启动调车作业", GameDataManager.Instance.GetMachineName(machine),null,(() =>
                    {
                        TaskDataManager.Instance.SendTaskCommand(taskCommand);
                    })));
            }
            else
            {
                UIManager.Instance.OpenUI(UIID.ConfirmStartTaskPanel,
                    new ConfirmTaskPanelArgs(taskCommand, GameDataManager.Instance.GetMachineName(machine)));
            }
           
         
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

    public bool IsCanSet()
    {
        return TaskDataManager.Instance.IsCanSet(machine);
    }
    public virtual void ResetState()
    {
        takeMaterStartBtn.SetSystemState(false, true);
        takeMaterStopBtn.SetSystemState(false, true);
        takeMaterReversingBtn.SetSystemState(false, true);
        takeMaterEndBtn.SetSystemState(false, true);
        takeMaterStartBtn.SetSelectState(false);
        takeMaterStopBtn.SetSelectState(false);
        takeMaterReversingBtn.SetSelectState(false);
        takeMaterEndBtn.SetSelectState(false);
        resetBtn.SetSelectState(false);
        resetBtn.SetSystemState(false, true);
        curTaskButtonCell?.SetSelectState(false);

        // AutoMaxToggle.SetSystemState(false, true);
        // entryModeGo.SetActive(false);
        // SemiAutoToggle.SetSystemState(true, true);
        // EntryModeOpen.SetSystemState(false, true);
        // EntryModeClose.SetSystemState(true, true);
        // RightAngleToggle.SetSystemState(false, true);
        // ObliqueAngleToggle.SetSystemState(true, true);

        // leftTurnToggle.SetSystemState(false, true);
        // rightTurnToggle.SetSystemState(false, true);
        confirmTurnBtn.SetSystemState(false, true);
        // PositionConfirmBtn.SetSystemState(false, true);
        // confirmTurnBtn.gameObject.SetActive(true);
        // PositionConfirmBtn.gameObject.SetActive(false);
    }

    public void SetInputFieldByAddSubBtn(InputFieldType inputFieldType,SymbolType symbolType,Vector3 position)
    {
        addSubPanel?.SetDataByAddSubBtn(inputFieldType, symbolType,position,_dictionary[inputFieldType],(
            (i, inputField, symbolType) =>
            {
                SetInputFieldValue(i, inputField, symbolType);
            } ));
    }
    private void SetInputFieldValue(int index,InputFieldType inputFieldType ,SymbolType symbolType)
    {
        InputField curInputField = null;
        float num=symbolType == SymbolType.ADD ? (float) _dictionary[inputFieldType][index] : -(float) _dictionary[inputFieldType][index];
        if (inputFieldType==InputFieldType.LEFTRANGE)
        {
            curInputField = leftTakeMaterText;
        }else if (inputFieldType==InputFieldType.RIGHTRANGE)
        {
            curInputField = rightTakeMaterText;
        }else if (inputFieldType==InputFieldType.ANGLEVALUE)
        {
            curInputField = AngleEntryText;
        }else if (inputFieldType==InputFieldType.STEPVALUE)
        {
            curInputField = takeMaterStep;
        }
        else
        {
            Debug.LogError("未定义的InputFieldType");
            return;
        }
        curInputField.text=(float.Parse(curInputField.text) + num).ToString();
        curInputField.onEndEdit?.Invoke(curInputField.text);
        if (IsCanSet()==false)
        {
            isRefreshResetText = false;
            if (refreshResetTextTimer!=null)
            {
                refreshResetTextTimer.Cancel();
                refreshResetTextTimer = null;
            }
            refreshResetTextTimer= Timer.Register(2f, (() =>
            {
                isRefreshResetText = true;
                TaskDataManager.Instance.UpdateTaskData();
            }));
            SendTaskCommand(OperationType.RESET);
        }
        
        Debug.Log($"设置输入框的值{num} {inputFieldType} {symbolType} {IsCanSet()}");
    }
    public virtual void InputFieldValueRange(InputField inputField, float min, float max, float defaultValue,
        string commandName = "")
    {
        inputField.text = defaultValue.ToString();
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