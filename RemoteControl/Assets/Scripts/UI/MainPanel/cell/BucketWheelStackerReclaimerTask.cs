using System;
using System.Collections.Generic;
using RemoteControl.Event;
using ShenYangRemoteSystem.Subclass;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class BucketWheelStackerReclaimerTask : BucketWheelTaskBase
{
    public InputField startPileMaterText;
    public InputField endPileMaterText;
    public ButtonCell leftPileMaterToggle;
    public ButtonCell rightPileMaterToggle;
    public ButtonCell PileAutoMaxToggle;
    public ButtonCell PileSemiAutoToggle;
    
    public ButtonCell PileRightAngleToggle;
    public ButtonCell PileObliqueAngleToggle;
    
    public InputField startLeftPileMaterText;
    public Button startleftPileAddBtn;
    public Button startleftPileSubBtn;
    public InputField endLeftPileMaterText;
    public Button endleftPileAddBtn;
    public Button endleftPileSubBtn;
    public InputField pileMaterHeightText;
    public InputField pileMaterStepText;
    public Button pileMaterStepAddBtn;
    public Button pileMaterStepSubBtn;
    public ButtonCell pileResetTaskBtn;
    public ButtonCell pileMaterStartBtn;
    public ButtonCell pileMaterStopBtn;
    public ButtonCell pileMaterEndBtn;
    public ButtonCell pileMaterReversingBtn;
    public ButtonCell fixedPointHeap;//定点堆料
    public ButtonCell rotaryHeap;//回转堆料
    public ButtonCell curPileTaskButtonCell;
    public Button ForcedPositioning;
    private Timer refreshTextTimer;
    private bool isRefreshText;
    public override void Start()
    {
        base.Start();
        isRefreshText = true;
        PileInputFieldValueRange(startPileMaterText, 0, 260,0,"START_POS_1");
        PileInputFieldValueRange(endPileMaterText, 0, 260,260,"END_POS_1");
        PileInputFieldValueRange(startLeftPileMaterText, 12,90,90,"LEFT_BORDER_SP_1");
        PileInputFieldValueRange(endLeftPileMaterText, 12, 90,90,"RIGHT_BORDER_SP_1");
        PileInputFieldValueRange(pileMaterHeightText, 0, 15,10,"STACK_HIGH_SET_1");
        PileInputFieldValueRange(pileMaterStepText, 0, 3,0.7f,"DC_REV_1");
        AddOnClickListener(pileResetTaskBtn,(() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否重置自动作业？",GameDataManager.Instance.GetMachineName(machine), null, () => SendPileMaterCommandByRc(OperationType.RESET)));
        }));
        AddOnClickListener(pileMaterStartBtn,(() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否启动自动作业？", GameDataManager.Instance.GetMachineName(machine),null, () => SendPileMaterCommandByRc(OperationType.START)));
          
        }));
        AddOnClickListener(pileMaterStopBtn,(() =>
        {
            if (pileMaterStopBtn.red.activeSelf)
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs(pileMaterStopBtn.red.activeSelf?"是否恢复自动作业?":"是否暂停自动作业？",GameDataManager.Instance.GetMachineName(machine), null, () => SendPileMaterCommandByRc(OperationType.PAUSE)));
            }
            else
            {
                SendPileMaterCommandByRc(OperationType.PAUSE);
            }
          
      
        }));
        AddOnClickListener(pileMaterEndBtn,(() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否结束自动作业？", GameDataManager.Instance.GetMachineName(machine),null, () =>    SendPileMaterCommandByRc(OperationType.END)));
        
        }));
        AddOnClickListener(pileMaterReversingBtn,(() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否换向自动作业？", GameDataManager.Instance.GetMachineName(machine),null, () =>    SendPileMaterCommandByRc(OperationType.REVERSING)));
        
        }));
        // AddOnClickListener(leftPileMaterToggle,(() =>
        // {
        //     leftPileMaterToggle.SetSystemState(true,true);
        //     rightPileMaterToggle.SetSystemState(false,true);
        // } ));
        // AddOnClickListener(rightPileMaterToggle,(() =>
        // {
        //     rightPileMaterToggle.SetSystemState(true,true);
        //     leftPileMaterToggle.SetSystemState(false,true);
        // } ));
        AddOnClickListener(PileAutoMaxToggle,(() =>
        {
            PileAutoMaxToggle.SetSelectState(true);
            // PileAutoMaxToggle.SetSystemState(true,true);
            // PileSemiAutoToggle.SetSystemState(false,true);
            GameDataManager.Instance.SendServerCommandByName(COMMAND_NAME.AUTO_ENABLE.ToString()+"_1",1);
        } ));
        AddOnClickListener(PileSemiAutoToggle,(() =>
        {
            PileSemiAutoToggle.SetSelectState(true);
            // PileAutoMaxToggle.SetSystemState(false,true);
            // PileSemiAutoToggle.SetSystemState(true,true);
            GameDataManager.Instance.SendServerCommandByName(COMMAND_NAME.AUTO_ENABLE.ToString()+"_1",0);
        } ));
        AddOnClickListener(fixedPointHeap,(() =>
        {
            fixedPointHeap.SetSelectState(true);
            // fixedPointHeap.SetSystemState(true,true);
            // rotaryHeap.SetSystemState(false,true);
            GameDataManager.Instance.SendServerCommandByName(COMMAND_NAME.POINT_SEL.ToString()+"_1",0);
        }));
        AddOnClickListener(rotaryHeap,(() =>
        {
            rotaryHeap.SetSelectState(true);
            // rotaryHeap.SetSystemState(true,true);
            // fixedPointHeap.SetSystemState(false,true);
            GameDataManager.Instance.SendServerCommandByName(COMMAND_NAME.SLEW_SEL.ToString()+"_1",0);
        }));
        // AddOnClickListener(PileRightAngleToggle,(() =>
        // {
        //     PileRightAngleToggle.SetSystemState(true,true);
        //     PileObliqueAngleToggle.SetSystemState(false,true);
        // } ));
        // AddOnClickListener(PileObliqueAngleToggle,(() =>
        // {
        //     PileRightAngleToggle.SetSystemState(false,true);
        //     PileObliqueAngleToggle.SetSystemState(true,true);
        // } ));
        startleftPileAddBtn.onClick.AddListener((() =>
        {
            GameDataManager.Instance.SendServerCommandByName("LEFT_BORDER_INC_1",0);
        }));
        startleftPileSubBtn.onClick.AddListener((() =>
        {
            GameDataManager.Instance.SendServerCommandByName("LEFT_BORDER_DES_1",0);
        }));
        endleftPileAddBtn.onClick.AddListener((() =>
        {
            GameDataManager.Instance.SendServerCommandByName("RIGHT_BORDER_INC_1",0);
        }));
        endleftPileSubBtn.onClick.AddListener((() =>
        {
            GameDataManager.Instance.SendServerCommandByName("RIGHT_BORDER_DES_1",0);
        }));
        pileMaterStepAddBtn.onClick.AddListener((() =>
        {
            GameDataManager.Instance.SendServerCommandByName("DC_REV_INC_1",0);
        }));
        pileMaterStepSubBtn.onClick.AddListener((() =>
        {
            GameDataManager.Instance.SendServerCommandByName("DC_REV_DES_1",0);
        }));
        ForcedPositioning.onClick.AddListener((() =>
        {
            GameDataManager.Instance.SendServerCommandByName("POS_FROCE_1",0);
        }));
        EventManager.Instance.TriggerEvent(EventName.UpdatePcData, null);
        EventManager.Instance.AddListener(EventName.UpdateRcData, RefreshPileData);
    }

    public override void UpdateData(TaskCommand taskCommand)
    {
        if (taskCommand.TaskType == TaskType.TAKEMATER)
        {
            base.UpdateData(taskCommand);
            pileResetTaskBtn.SetSystemState(false,true);
            pileMaterStartBtn.SetSystemState(false,true);
            pileMaterStopBtn.SetSystemState(false,true);
            pileMaterEndBtn.SetSystemState(false,true);
        }
        else
        {
            return;
            base.ResetState();
            startPileMaterText.text = taskCommand.MaterialRange.startValue.ToString();
            endPileMaterText.text = taskCommand.MaterialRange.endValue.ToString();
            pileMaterHeightText.text = taskCommand.PileMateHigh.ToString();
            if (taskCommand.SideSelection=="LEFT")
            {
                leftPileMaterToggle.SetSystemState(true,true);
                rightPileMaterToggle.SetSystemState(false,true);
                // leftPileMaterToggle.isOn = true;
                // rightPileMaterToggle.isOn = false;
            }
            else
            {
                
                leftPileMaterToggle.SetSystemState(false,true);
                rightPileMaterToggle.SetSystemState(true,true);
                // leftPileMaterToggle.isOn = false;
                // rightPileMaterToggle.isOn = true;
            }
            startLeftPileMaterText.text=taskCommand.LeftRightRange.startValue.ToString();
            endLeftPileMaterText.text=taskCommand.LeftRightRange.endValue.ToString();
            
            pileResetTaskBtn.SetSystemState(taskCommand.ResetState==1,true);
            pileMaterStartBtn.SetSystemState(taskCommand.AllData.OperationCommandList[0]==1,true);
            pileMaterStopBtn.SetSystemState(taskCommand.AllData.OperationCommandList[1] == 1,true);
            pileMaterEndBtn.SetSystemState(taskCommand.AllData.OperationCommandList[3] == 1,true);
            
            PileAutoMaxToggle.SetSystemState(taskCommand.AutoMode == AutoMode.AUTOMAX,true);
            PileSemiAutoToggle.SetSystemState(taskCommand.AutoMode == AutoMode.SemiAuto,true);
            PileRightAngleToggle.SetSystemState(taskCommand.AngleEntryMode == AngleEntryMode.RIGHTANGLE,true);
            PileObliqueAngleToggle.SetSystemState(taskCommand.AngleEntryMode == AngleEntryMode.OBLIQUEANGLE,true);
        }
    
      
    }

    public void RefreshPileData(object o, EventArgs eventArgs)
    {
        SystemVariables systemVariables = GameDataManager.Instance.SystemVariables;
        if (isRefreshText)
        {
            startPileMaterText.SetTextByFocused(systemVariables.SR1_Stack_Start_Pos.ToString());
            endPileMaterText.SetTextByFocused(systemVariables.SR1_Stack_End_Pos.ToString());
            startLeftPileMaterText.SetTextByFocused(systemVariables.SR1_Stack_LeftBorder_SP.ToString());
            endLeftPileMaterText.SetTextByFocused(systemVariables.SR1_Stack_RightBorder_SP.ToString());
            pileMaterHeightText.SetTextByFocused(systemVariables.SR1_Stack_HighSet.ToString());
            pileMaterStepText.SetTextByFocused(systemVariables.SR1_Stack_DcRevSize.ToString());
        }
        
        PileAutoMaxToggle.SetSystemState(systemVariables.SR1_AutoBorder_Enable,true);
        PileSemiAutoToggle.SetSystemState(systemVariables.SR1_AutoBorder_Enable==false,true);
        rotaryHeap.SetSystemState(systemVariables.SR1_SlewStack_SEL,true);
        fixedPointHeap.SetSystemState(systemVariables.SR1_PointStack_SEL,true);
        
        pileMaterStartBtn.SetSystemState(systemVariables.SR1_Working_Start,true);
        pileMaterStopBtn.SetSystemState(systemVariables.SR1_Stop_Runing,true);
        pileMaterEndBtn.SetSystemState(systemVariables.SR1_Working_Pause,true);
        pileMaterReversingBtn.SetSystemState(systemVariables.SR1_Change_Direct,true);
    }
    //堆料目前使用plc命令 和取料区分开
    public void SendPileMaterCommandByRc(OperationType operationType)
    {
        if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.TASK_SERVER_CONNECTION_FAIL_TIP,GameDataManager.Instance.GetMachineName(machine)));
            return;
        }
        
        if (TaskDataManager.Instance.IsCanSendTaskCommond(machine,TaskType.PILEMATER,operationType)!=-1)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("当前操作无效的",GameDataManager.Instance.GetMachineName(machine)));
            return;
        }

        string commandName = "";
        int dataInt = 0;
        if (operationType==OperationType.START)
        {
            DataManager.Instance.InsertHistoryLogMc("自动堆料-启动任务", GameDataManager.Instance.GetUserName(), machine);
            UpdateCurCtrMode(ref curPileTaskButtonCell,pileMaterStartBtn);
            commandName = machine == Machine.BucketWheelStackerReclaimer
                ? COMMAND_NAME.WORKING_START.ToString() + "_1"
                : COMMAND_NAME.WORKING_START.ToString() + "_2";
        }else if (operationType==OperationType.PAUSE)
        {
            DataManager.Instance.InsertHistoryLogMc(pileMaterStopBtn.red.activeSelf?"自动堆料-恢复任务":"自动堆料-暂停任务", GameDataManager.Instance.GetUserName(), machine);
            UpdateCurCtrMode(ref curPileTaskButtonCell,pileMaterStopBtn);
            commandName = machine == Machine.BucketWheelStackerReclaimer
                ? COMMAND_NAME.WORKING_PAUSE.ToString() + "_1"
                : COMMAND_NAME.WORKING_PAUSE.ToString() + "_2";
            dataInt = pileMaterStopBtn.red.activeSelf ? 0 : 1;
        }else if (operationType == OperationType.END)
        {
            DataManager.Instance.InsertHistoryLogMc("自动堆料-任务结束", GameDataManager.Instance.GetUserName(), machine);
            UpdateCurCtrMode(ref curPileTaskButtonCell,pileMaterEndBtn);
            commandName = machine == Machine.BucketWheelStackerReclaimer
                ? COMMAND_NAME.STOP_RUNING.ToString() + "_1"
                : COMMAND_NAME.STOP_RUNING.ToString() + "_2";
        }else if (operationType == OperationType.REVERSING)
        {
            DataManager.Instance.InsertHistoryLogMc("自动堆料-任务换向", GameDataManager.Instance.GetUserName(), machine);
            pileMaterReversingBtn.SetSelectState(true);
            commandName = machine == Machine.BucketWheelStackerReclaimer
                ? COMMAND_NAME.CHANGE_DIRECT.ToString() + "_1"
                : COMMAND_NAME.CHANGE_DIRECT.ToString() + "_2";
        }
        GameDataManager.Instance.SendServerCommandByName(commandName,dataInt);
    }
    public void SendPileMaterCommand(OperationType operationType)
    {
        if (GameDataManager.Instance.GameMain.connectionPC.isConnect==false)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.TASK_SERVER_CONNECTION_FAIL_TIP,GameDataManager.Instance.GetMachineName(machine)));
            return;
        }
        
        if (TaskDataManager.Instance.IsCanSendTaskCommond(machine,TaskType.PILEMATER,operationType)!=-1)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("当前操作无效的",GameDataManager.Instance.GetMachineName(machine)));
            return;
        }
        
        TaskCommand taskCommand = new TaskCommand();
        if (operationType==OperationType.START)
        {
            DataManager.Instance.InsertHistoryLogMc("自动堆料-启动任务", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.OperationCommand = operationType;
            UpdateCurCtrMode(ref curPileTaskButtonCell,pileMaterStartBtn);
        }else if (operationType==OperationType.PAUSE)
        {
            DataManager.Instance.InsertHistoryLogMc(pileMaterStopBtn.red.activeSelf?"自动堆料-恢复任务":"自动堆料-暂停任务", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.OperationCommand = operationType;
            UpdateCurCtrMode(ref curPileTaskButtonCell,pileMaterStopBtn);
            // int dataInt = pileMaterStopBtn.red.activeSelf?0:1;
            // string commandName=machine==Machine.BucketWheelStackerReclaimer?COMMAND_NAME.STACK_PAUSE.ToString()+"_1":COMMAND_NAME.TAKE_PAUSE.ToString()+"_2";
            // GameDataManager.Instance.SendServerCommandRC(commandName,6,2,dataInt);
        }else if (operationType == OperationType.END)
        {
            DataManager.Instance.InsertHistoryLogMc("自动堆料-任务结束", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.OperationCommand = operationType;
            UpdateCurCtrMode(ref curPileTaskButtonCell,pileMaterEndBtn);
        }else if (operationType == OperationType.RESET)
        {
            DataManager.Instance.InsertHistoryLogMc("自动堆料-任务换向", GameDataManager.Instance.GetUserName(), machine);
            pileMaterReversingBtn.SetSelectState(true);
            taskCommand.ResetState = 1;
        }
     
        taskCommand.QuerySystem = "MC";
        taskCommand.TaskType = TaskType.PILEMATER;
        taskCommand.Machine = machine;
        taskCommand.OperatorName = GameDataManager.Instance.GetUserID();
        taskCommand.TaskCreateTime = DateTime.Now;
        taskCommand.FinishMethod =new List<int>(){0,0};
        if (operationType==OperationType.START|| operationType == OperationType.RESET)
        {
            taskCommand.AutoMode = AutoMode.AUTOMAX;// PileAutoMaxToggle.red.activeSelf ? AutoMode.AUTOMAX : AutoMode.SemiAuto;
            taskCommand.AngleEntryMode = AngleEntryMode.RIGHTANGLE;// PileRightAngleToggle.red.activeSelf ? AngleEntryMode.RIGHTANGLE : AngleEntryMode.OBLIQUEANGLE;
            taskCommand.Command_Type = operationType == OperationType.RESET?2:0;
            float startValue = startPileMaterText.text == "" ? 0 : float.Parse(startPileMaterText.text);
            float endValue = endPileMaterText.text == "" ? 0 : float.Parse(endPileMaterText.text);
            taskCommand.MaterialRange = new TaskRange(startValue, endValue);
            taskCommand.SideSelection = leftPileMaterToggle.red.activeSelf ? "LEFT" : "RIGHT";
            float startLeftRightRangeValue = startLeftPileMaterText.text == "" ? 0 : float.Parse(startLeftPileMaterText.text);
            float endLeftRightRangeValue = endLeftPileMaterText.text == "" ? 0 : float.Parse(endLeftPileMaterText.text);
            taskCommand.LeftRightRange = new TaskRange(startLeftRightRangeValue, endLeftRightRangeValue);
            taskCommand.StepLength = takeMaterStep.text == "" ? 0 : float.Parse(takeMaterStep.text);
            // taskCommand.IsTimed = false; //timeOpenToggle.isOn;
            // taskCommand.TimedAt = 0;//int.Parse(timeHourText.text) * 60 + int.Parse(timeMinuteText.text);
            // taskCommand.IsQuantified =false; // quantityOpenToggle.isOn;
            // taskCommand.Quantity =0; // int.Parse(takeMaterNum.text);
            taskCommand.TaskID = DateTime.Now.ToString("yyMMddHHmmss");
            taskCommand.OperatorSystem = "MC";
            // taskCommand.LayerHigh = 0;
            // taskCommand.TakeMateHigh = 0;
            taskCommand.PileMateHigh= float.Parse(pileMaterHeightText.text);
            AllData allData = new AllData();
            taskCommand.AllData = allData;
        }
        else
        {
            taskCommand.Command_Type = 2;
        }
        if (operationType== OperationType.END)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmTaskPanel, new ConfirmTaskPanelArgs(taskCommand,GameDataManager.Instance.GetMachineName(machine)));
        }
        else
        {
            TaskDataManager.Instance.SendTaskCommand(taskCommand);
        }
    }

    public void PileInputFieldValueRange(InputField inputField, float min, float max, float defaultValue,
        string commandName)
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
            
            isRefreshText = false;
            if (commandName!="")
            {
                GameDataManager.Instance.SendServerCommandByName(commandName,0,num);
            }

            if (refreshTextTimer!=null)
            {
                refreshTextTimer.Cancel();
                refreshTextTimer = null;
            }
            refreshTextTimer= Timer.Register(2, (() =>
            {
                isRefreshText = true;
            }));
        }));
    }
    public override void InputFieldValueRange(InputField inputField, float min, float max,float defaultValue,string commandName)
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
            if (commandName!="")
            {
                GameDataManager.Instance.SendServerCommandByName(commandName,0,num);
            }
        }));
    }
    public override void ResetState()
    {
        base.ResetState();
        pileResetTaskBtn.SetSystemState(false,true);
        pileMaterStartBtn.SetSystemState(false,true);
        pileMaterStopBtn.SetSystemState(false,true);
        pileMaterEndBtn.SetSystemState(false,true);
        
        pileMaterStartBtn.SetSelectState(false);
        pileMaterStopBtn.SetSelectState(false);
        pileMaterEndBtn.SetSelectState(false);
        pileResetTaskBtn.SetSelectState(false);
        PileAutoMaxToggle.SetSystemState(false,true);
        PileSemiAutoToggle.SetSystemState(true,true);
        PileRightAngleToggle.SetSystemState(false,true);
        PileObliqueAngleToggle.SetSystemState(true,true);
    }
}
