using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class BucketWheelTaskBase : PanelBase
{
    public Button scramStopBtn;

    public Button resetBtn;
    public Button warningBtn;
    public Text[] warningTexts;
    public InputField startTakeMaterText;
    public InputField stopTakeMaterText;
    public Toggle leftToggle;
    public Toggle rightToggle;
    public InputField leftTakeMaterText;
    public InputField rightTakeMaterText;
    public InputField takeMaterStep;
    public InputField timeHourText;
    public InputField timeMinuteText;
    public Toggle timeOpenToggle;
    public Toggle timeCloseToggle;
    public InputField takeMaterNum;
    public Toggle quantityOpenToggle;
    public Toggle quantityCloseToggle;
    public Button takeMaterStartBtn;
    public Button takeMaterStopBtn;
    public Button takeMaterReversingBtn;
    public Button takeMaterEndBtn;
    public Machine machine;

    public virtual void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        AddOnClickListener(scramStopBtn, (() => { SendTaskCommand("急停"); }));
        AddOnClickListener(resetBtn, (() => { SendTaskCommand("复位"); }));
        AddOnClickListener(warningBtn, (() => { SendTaskCommand("警告"); }));
        AddOnClickListener(takeMaterStartBtn, (() => { SendTaskCommand("启动"); }));
        AddOnClickListener(takeMaterStopBtn, (() => { SendTaskCommand("暂停"); }));
        AddOnClickListener(takeMaterReversingBtn, (() => { SendTaskCommand("换向"); }));
        AddOnClickListener(takeMaterEndBtn, (() => { SendTaskCommand("结束"); }));
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
        InputFieldValueRange(startTakeMaterText, 0, 350);
        InputFieldValueRange(stopTakeMaterText, 0, 350);
        InputFieldValueRange(leftTakeMaterText, 0, 45);
        InputFieldValueRange(rightTakeMaterText, 0, 45);
        InputFieldValueRange(timeHourText, 0, 99);
        InputFieldValueRange(timeMinuteText, 0, 60);
        InputFieldValueRange(takeMaterStep, 0, 99);
        InputFieldValueRange(takeMaterNum, 0, 99999);
    }

    public virtual void SendTaskCommand(string message)
    {
        Debug.Log($"message {machine} {message}");
        if (message == "启动")
        {
            TaskCommand taskCommand = new TaskCommand();
            taskCommand.QuerySystem = "MC";
            taskCommand.TaskType = TaskType.PILEMATER;
            taskCommand.Machine = machine;
            taskCommand.OperationCommand = OperationType.START;
            taskCommand.MaterialRange = new TaskRange(10, 10);
            taskCommand.SideSelection = "LIFT";
            taskCommand.LeftRightRange = new TaskRange(10, 10);
            taskCommand.StepLength = 0.5F;
            taskCommand.IsTimed = true;
            taskCommand.TimedAt = 8888;
            taskCommand.IsQuantified = false;
            taskCommand.Quantity = 10;
            taskCommand.TaskID = 24073102;
            taskCommand.OperatorSystem ="MC";
            MessageCenter.Instance.SendMessage<TaskCommand>((int)MessageType.Plc, taskCommand);
        }

    }

    public virtual void InputFieldValueRange(InputField inputField, int min, int max)
    {
        inputField.onValueChanged.AddListener(((string value) =>
        {
            int num = 0;
            if (int.TryParse(value, out num))
            {
                num = num < min ? min : num;
                num = num > max ? max : num;

            }
            inputField.text = num.ToString();

        }));
    }
}
