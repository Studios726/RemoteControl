using System;
using System.Collections.Generic;
using RemoteControl.Event;
using UnityEngine.UI;

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
    public InputField endLeftPileMaterText;
    public InputField pileMaterHeightText;
    public ButtonCell pileResetTaskBtn;
    public ButtonCell pileMaterStartBtn;
    public ButtonCell pileMaterStopBtn;
    public ButtonCell pileMaterEndBtn;
    public ButtonCell curPileTaskButtonCell;
    public override void Start()
    {
        base.Start();
        InputFieldValueRange(startPileMaterText, 0, 260,0);
        InputFieldValueRange(endPileMaterText, 0, 260,260);
        InputFieldValueRange(startLeftPileMaterText, 12,90,90);
        InputFieldValueRange(endLeftPileMaterText, 12, 90,90);
        InputFieldValueRange(pileMaterHeightText, 0, 15,10);
        AddOnClickListener(pileResetTaskBtn,(() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否重置自动作业？",GameDataManager.Instance.GetMachineName(machine), null, () => SendPileMaterCommand(OperationType.RESET)));
        }));
        AddOnClickListener(pileMaterStartBtn,(() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否启动自动作业？", GameDataManager.Instance.GetMachineName(machine),null, () => SendPileMaterCommand(OperationType.START)));
          
        }));
        AddOnClickListener(pileMaterStopBtn,(() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs(pileMaterStopBtn.red.activeSelf?"是否恢复自动作业?":"是否暂停自动作业？",GameDataManager.Instance.GetMachineName(machine), null, () => SendPileMaterCommand(OperationType.PAUSE)));
      
        }));
        AddOnClickListener(pileMaterEndBtn,(() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否结束自动作业？", GameDataManager.Instance.GetMachineName(machine),null, () =>    SendPileMaterCommand(OperationType.END)));
        
        }));
        AddOnClickListener(leftPileMaterToggle,(() =>
        {
            leftPileMaterToggle.SetSystemState(true,true);
            rightPileMaterToggle.SetSystemState(false,true);
        } ));
        AddOnClickListener(rightPileMaterToggle,(() =>
        {
            rightPileMaterToggle.SetSystemState(true,true);
            leftPileMaterToggle.SetSystemState(false,true);
        } ));
        AddOnClickListener(PileAutoMaxToggle,(() =>
        {
            PileAutoMaxToggle.SetSystemState(true,true);
            PileSemiAutoToggle.SetSystemState(false,true);
        } ));
        AddOnClickListener(PileSemiAutoToggle,(() =>
        {
            PileAutoMaxToggle.SetSystemState(false,true);
            PileSemiAutoToggle.SetSystemState(true,true);
        } ));
        
        AddOnClickListener(PileRightAngleToggle,(() =>
        {
            PileRightAngleToggle.SetSystemState(true,true);
            PileObliqueAngleToggle.SetSystemState(false,true);
        } ));
        AddOnClickListener(PileObliqueAngleToggle,(() =>
        {
            PileRightAngleToggle.SetSystemState(false,true);
            PileObliqueAngleToggle.SetSystemState(true,true);
        } ));
        EventManager.Instance.TriggerEvent(EventName.UpdatePcData, null);
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
            DataManager.Instance.InsertHistoryLogMc("自动堆料-任务重置", GameDataManager.Instance.GetUserName(), machine);
            pileResetTaskBtn.SetSelectState(true);
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
