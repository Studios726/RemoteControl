using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BucketWheelStackerReclaimerTask : BucketWheelTaskBase
{
    public InputField startPileMaterText;
    public InputField endPileMaterText;
    public Toggle leftPileMaterToggle;
    public Toggle rightPileMaterToggle;
    public InputField startLeftPileMaterText;
    public InputField endLeftPileMaterText;
    public InputField pileMaterHeightText;
    public ButtonCell pileMaterStartBtn;
    public ButtonCell pileMaterStopBtn;
    public ButtonCell pileMaterEndBtn;
    public ButtonCell curPileTaskButtonCell;
    public override void Start()
    {
        base.Start();
        InputFieldValueRange(startPileMaterText, 0, 350);
        InputFieldValueRange(endPileMaterText, 0, 350);
        InputFieldValueRange(startLeftPileMaterText, 0, 350);
        InputFieldValueRange(endLeftPileMaterText, 0, 350);
        InputFieldValueRange(pileMaterHeightText, 0, 99);
        AddOnClickListener(pileMaterStartBtn,(() => { SendPileMaterCommand(OperationType.START);}));
        AddOnClickListener(pileMaterStopBtn,(() => {SendPileMaterCommand(OperationType.PAUSE);}));
        AddOnClickListener(pileMaterEndBtn,(() => {SendPileMaterCommand(OperationType.END);}));
    }

    public override void UpdateData(TaskCommand taskCommand)
    {
        if (taskCommand.TaskType == TaskType.TAKEMATER)
        {
            base.UpdateData(taskCommand);
        }
        else
        {
            startPileMaterText.text = taskCommand.MaterialRange.startValue.ToString();
            endPileMaterText.text = taskCommand.MaterialRange.endValue.ToString();
            
            if (taskCommand.SideSelection=="LIFT")
            {
                leftPileMaterToggle.isOn = true;
                rightPileMaterToggle.isOn = false;
            }
            else
            {
                leftPileMaterToggle.isOn = false;
                rightPileMaterToggle.isOn = true;
            }
            startLeftPileMaterText.text=taskCommand.LeftRightRange.startValue.ToString();
            endLeftPileMaterText.text=taskCommand.LeftRightRange.endValue.ToString();
            pileMaterStartBtn.SetSystemState(taskCommand.AllData.OperationCommandList[0]==1);
            pileMaterStopBtn.SetSystemState(taskCommand.AllData.OperationCommandList[1] == 1);
            pileMaterEndBtn.SetSystemState(taskCommand.AllData.OperationCommandList[2] == 1);
        }
    
      
    }
    public void SendPileMaterCommand(OperationType operationType)
    {
        if (GameDataManager.Instance.GameMain.connectionPC.isConnect==false)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.TASK_SERVER_CONNECTION_FAIL_TIP));
            return;
        }
        if (operationType==OperationType.START)
        {
            UpdateCurCtrMode(ref curPileTaskButtonCell,pileMaterStartBtn);
        }else if (operationType==OperationType.PAUSE)
        {
            UpdateCurCtrMode(ref curPileTaskButtonCell,pileMaterStopBtn);
        }else if (operationType == OperationType.END)
        {
            UpdateCurCtrMode(ref curPileTaskButtonCell,pileMaterEndBtn);
        }
        TaskCommand taskCommand = new TaskCommand();
        taskCommand.QuerySystem = "MC";
       
        taskCommand.OperationCommand = operationType;
        taskCommand.TaskType = TaskType.PILEMATER;
        taskCommand.Machine = machine;
        taskCommand.OperatorName = GameDataManager.Instance.GetUserName();
        // taskCommand.TaskCreateTime = DateTime.Now;
        if (operationType==OperationType.START)
        {
            taskCommand.Command_Type = 0;
            float startValue = startPileMaterText.text == "" ? 0 : int.Parse(startPileMaterText.text);
            float endValue = endPileMaterText.text == "" ? 0 : int.Parse(endPileMaterText.text);
            taskCommand.MaterialRange = new TaskRange(startValue, endValue);
            taskCommand.SideSelection = leftPileMaterToggle.isOn ? "LIFT" : "RIGHT";
            float startLeftRightRangeValue = startLeftPileMaterText.text == "" ? 0 : int.Parse(startLeftPileMaterText.text);
            float endLeftRightRangeValue = endLeftPileMaterText.text == "" ? 0 : int.Parse(endLeftPileMaterText.text);
            taskCommand.LeftRightRange = new TaskRange(startLeftRightRangeValue, endLeftRightRangeValue);
            taskCommand.StepLength = takeMaterStep.text == "" ? 0 : int.Parse(takeMaterStep.text); ;
            taskCommand.IsTimed = timeOpenToggle.isOn;
            taskCommand.TimedAt = int.Parse(timeHourText.text) * 60 + int.Parse(timeMinuteText.text);
            taskCommand.IsQuantified = quantityOpenToggle.isOn;
            taskCommand.Quantity = int.Parse(takeMaterNum.text);
            taskCommand.TaskID = DateTime.Now.ToString("yyMMddHHmmss");
            taskCommand.OperatorSystem = "MC";
            taskCommand.LayerHigh = 0;
            taskCommand.TakeMateHigh= int.Parse(pileMaterHeightText.text);
            AllData allData = new AllData();
            taskCommand.AllData = allData;
        }
        else
        {
            taskCommand.Command_Type = 2;
        }
        TaskDataManager.Instance.SendTaskCommand(taskCommand);
    }

    public override void ResetState()
    {
        base.ResetState();
        pileMaterStartBtn.SetSystemState(false);
        pileMaterStopBtn.SetSystemState(false);
        pileMaterEndBtn.SetSystemState(false);
        
        pileMaterStartBtn.SetSelectState(false);
        pileMaterStopBtn.SetSelectState(false);
        pileMaterEndBtn.SetSelectState(false);
    }
}
