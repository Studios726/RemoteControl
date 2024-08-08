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
    public Toggle quantityOpenToggle  ;
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

    public virtual void UpdateData(TaskCommand taskCommand)
    {
        startTakeMaterText.text = taskCommand.MaterialRange.startValue.ToString();
        stopTakeMaterText.text = taskCommand.MaterialRange.endValue.ToString();
        leftToggle.isOn= taskCommand.SideSelection=="LIFT"?true:false;
        leftTakeMaterText.text = taskCommand.LeftRightRange.startValue.ToString();
        rightTakeMaterText.text = taskCommand.LeftRightRange.endValue.ToString();
        takeMaterStep.text = taskCommand.StepLength.ToString();
        timeOpenToggle.isOn = taskCommand.IsTimed;
        if (timeOpenToggle.isOn) {
            quantityOpenToggle.isOn = false;
            timeHourText.text =Mathf.FloorToInt(taskCommand.TimedAt/60).ToString();
            timeMinuteText.text= (taskCommand.TimedAt % 60).ToString();
        }
        else
        {
            quantityOpenToggle.isOn=true;
            timeOpenToggle.isOn = false;
            takeMaterNum.text= taskCommand.Quantity.ToString();
        }

    }
    public virtual void Init()
    {
        AddOnClickListener(scramStopBtn, (() => { SendPlcCommand("急停"); }));
        AddOnClickListener(resetBtn, (() => { SendPlcCommand("复位"); }));
        AddOnClickListener(warningBtn, (() => { SendPlcCommand("警告"); }));
        AddOnClickListener(takeMaterStartBtn, (() => { SendTaskCommand(OperationType.START); }));
        AddOnClickListener(takeMaterStopBtn, (() => { SendTaskCommand(OperationType.PAUSE); }));
        AddOnClickListener(takeMaterReversingBtn, (() => { SendTaskCommand(OperationType.REVERSING); }));
        AddOnClickListener(takeMaterEndBtn, (() => { SendTaskCommand(OperationType.END); }));
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
    public virtual void SendPlcCommand(string plcCommand)
    {

    }
    public virtual void SendTaskCommand(OperationType operationType)
    {
        Debug.Log($"message {machine} {operationType}");
        TaskCommand taskCommand = new TaskCommand();
        taskCommand.QuerySystem = "MC";
        taskCommand.Command_Type = 0;
        taskCommand.OperationCommand = operationType;
        taskCommand.TaskType = TaskType.PILEMATER;
        taskCommand.Machine = machine;
        if (operationType == OperationType.START)
        {
            float startValue = startTakeMaterText.text == "" ? 0 : int.Parse(startTakeMaterText.text);
            float endValue = stopTakeMaterText.text == "" ? 0 : int.Parse(stopTakeMaterText.text);
            taskCommand.MaterialRange = new TaskRange(startValue, endValue);
            taskCommand.SideSelection = leftToggle.isOn ? "LIFT" : "RIGHT";
            float startLeftRightRangeValue = leftTakeMaterText.text == "" ? 0 : int.Parse(leftTakeMaterText.text);
            float endLeftRightRangeValue = rightTakeMaterText.text == "" ? 0 : int.Parse(rightTakeMaterText.text);
            taskCommand.LeftRightRange = new TaskRange(startLeftRightRangeValue, endLeftRightRangeValue);
            taskCommand.StepLength = takeMaterStep.text == "" ? 0 : int.Parse(takeMaterStep.text); ;
            taskCommand.IsTimed = timeOpenToggle.isOn;
            taskCommand.TimedAt = int.Parse(timeHourText.text) * 60 + int.Parse(timeMinuteText.text);
            taskCommand.IsQuantified = quantityOpenToggle.isOn;
            taskCommand.Quantity = int.Parse(takeMaterNum.text);
            taskCommand.TaskID = DateTime.Now.ToString("yyMMddHHmmss");
            taskCommand.OperatorSystem = "MC";
            taskCommand.LayerHigh = 0;
            AllData allData = new AllData();
            //allData.InfoIcon = "哈哈哈";
            taskCommand.AllData = allData;
        }
        //Debug.LogError("TaskType "+taskCommand.TaskType.ToString());
        //string sql = $"INSERT INTO {ConstStr.DATABASE_HISTORY_TASK_MC} (`{ConstStr.DATA_OPERATO_RSYSTEM}`,`{ConstStr.DATA_TASK_CREATE_TIME}`,`{ConstStr.DATA_MACHINE}`,`{ConstStr.DATA_TASK_TYPE}`,`{ConstStr.DATA_MATERIAL_RANGE_START}`,`{ConstStr.DATA_MATERIAL_RANGE_END}`,`{ConstStr.DATA_SIDE_SELECTION}`,`{ConstStr.DATA_LEFT_RIGHT_RANGE_START}`,`{ConstStr.DATA_LEFT_RIGHT_RANGE_END}`,`{ConstStr.DATA_STEP_LENGTH}`,`{ConstStr.DATA_IS_TIMED}`,`{ConstStr.DATA_TIMEDAT}`,`{ConstStr.DATA_IS_QUANTIFIED}`,`{ConstStr.DATA_QUANTITY}`,`{ConstStr.DATA_OPERATOR}`,`{ConstStr.DATA_TASK_STATE}`,`{ConstStr.DATA_TASK_ID}`) " +
        //    $"VALUES ('{taskCommand.QuerySystem}','{DateTime.Now}','{taskCommand.Machine.ToString()}','{taskCommand.TaskType.ToString()}','{taskCommand.MaterialRange.startValue}','{taskCommand.MaterialRange.endValue}','{taskCommand.SideSelection}','{taskCommand.LeftRightRange.startValue}','{taskCommand.LeftRightRange.endValue}','{taskCommand.StepLength}','{0}','{taskCommand.TimedAt}','{1}','{taskCommand.Quantity}','{"TEST"}','{"完成"}','{taskCommand.TaskID}')";
        //MySqlHelper.ExecuteSql(sql);
        Debug.LogError("TaskID" + taskCommand.TaskID);
        DataManager.Instance.InsertHistoryTaskMc(taskCommand, GameDataManager.Instance.curAccountInfo.name, "0");
        GameDataManager.Instance.SendTaskCommand(taskCommand);
    }

    public virtual void InputFieldValueRange(InputField inputField, int min, int max)
    {
        inputField.text = "0";
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
