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
    public override void Start()
    {
        base.Start();
        InputFieldValueRange(startLeftPileMaterText, 0, 350);
        InputFieldValueRange(endLeftPileMaterText, 0, 350);
        InputFieldValueRange(pileMaterHeightText, 0, 99);
        AddOnClickListener(pileMaterStartBtn,(() => { SendPileMaterCommand(OperationType.START);}));
        AddOnClickListener(pileMaterStopBtn,(() => {SendPileMaterCommand(OperationType.PAUSE);}));
        AddOnClickListener(pileMaterEndBtn,(() => {SendPileMaterCommand(OperationType.END);}));
    }

    public void SendPileMaterCommand(OperationType operationType)
    {
        TaskCommand taskCommand = new TaskCommand();
        taskCommand.QuerySystem = "MC";
        taskCommand.Command_Type = 0;
        taskCommand.OperationCommand = operationType;
        taskCommand.TaskType = TaskType.TAKEMATER;
        taskCommand.Machine = machine;
        if (operationType==OperationType.START)
        {
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
            //allData.InfoIcon = "哈哈哈";
            taskCommand.AllData = allData;
        }
    }
}
