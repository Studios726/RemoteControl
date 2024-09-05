using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;

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
    public InputField layerHigh;
    public InputField timeHourText;
    public InputField timeMinuteText;
    public Toggle timeOpenToggle;
    public Toggle timeCloseToggle;
    public InputField takeMaterNum;
    public Toggle quantityOpenToggle  ;
    public Toggle quantityCloseToggle;
    public ButtonCell takeMaterStartBtn;
    public ButtonCell takeMaterStopBtn;
    public ButtonCell takeMaterReversingBtn;
    public ButtonCell takeMaterEndBtn;
    public ButtonCell curTaskButtonCell;
    public Machine machine;
    private Timer reversingTimer;

    public virtual void Start()
    {
        Init();
    }

    public virtual void UpdateDes(Queue<string> queue)
    {
        int index = 0;
        foreach (var des in queue)
        {
            warningTexts[index].text = des;
            index = index + 1;
        }
    }
    public virtual void UpdateData(TaskCommand taskCommand)
    {
        startTakeMaterText.text = taskCommand.MaterialRange.startValue.ToString();
        stopTakeMaterText.text = taskCommand.MaterialRange.endValue.ToString();
        if (taskCommand.SideSelection=="LIFT")
        {
            leftToggle.isOn = true;
            rightToggle.isOn = false;
        }
        else
        {
            leftToggle.isOn = false;
            rightToggle.isOn = true;
        }
        leftTakeMaterText.text = taskCommand.LeftRightRange.startValue.ToString();
        rightTakeMaterText.text = taskCommand.LeftRightRange.endValue.ToString();
        takeMaterStep.text = taskCommand.StepLength.ToString();
        layerHigh.text = taskCommand.LayerHigh.ToString();
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
        takeMaterStartBtn.SetSystemState(taskCommand.AllData.OperationCommandList[0] == 1);
        takeMaterStopBtn.SetSystemState(taskCommand.AllData.OperationCommandList[1] == 1);
        if (taskCommand.AllData.OperationCommandList[2] == 1)
        {
            if (reversingTimer!=null)
            {
                reversingTimer?.Cancel();
            }
            reversingTimer=Timer.Register(2, () =>
            {
                takeMaterReversingBtn.SetSystemState(false);
                curTaskButtonCell?.SetSelectState(false);
                
            });
            takeMaterReversingBtn.SetSystemState(true);
        }
        else
        {
            takeMaterReversingBtn.SetSystemState(taskCommand.AllData.OperationCommandList[2] == 1);
        }
        takeMaterEndBtn.SetSystemState(taskCommand.AllData.OperationCommandList[3] == 1);

    }
    public virtual void Init()
    {
        AddOnClickListener(scramStopBtn, (() => { SendPlcCommand(COMMAND_NAME.EMERGENCY_STOP); }));
        AddOnClickListener(resetBtn, (() => { SendPlcCommand(COMMAND_NAME.ERR_RESET); }));
        AddOnClickListener(warningBtn, (() => { SendPlcCommand(COMMAND_NAME.STARTUP_ALARM); }));
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
        InputFieldValueRange(layerHigh, 0, 45);
    }

    public virtual void WarningDown()
    {
        Debug.LogError("按下");//1
        SendPlcCommand(COMMAND_NAME.STARTUP_ALARM,1);
    }
    public virtual void WarningUp()
    {
        Debug.LogError("抬起");//0
        SendPlcCommand(COMMAND_NAME.STARTUP_ALARM,0);
    }
    public virtual void SendPlcCommand(COMMAND_NAME mCommandName,int dataInt=0)
    {
        if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
            return;
        }
        string commandName=machine == Machine.BucketWheelStackerReclaimer ? mCommandName.ToString()+"_1" : mCommandName.ToString()+"_2";
        // int dataInt = 0;
        if (mCommandName==COMMAND_NAME.EMERGENCY_STOP)
        {
            
        }else if (COMMAND_NAME.ERR_RESET == mCommandName)
        {
            
        }else if (COMMAND_NAME.STARTUP_ALARM == mCommandName)
        {
            
        }
        Debug.Log($" commandName {commandName} {dataInt}");
        GameDataManager.Instance.SendServerCommandByName(commandName,dataInt);
    }
    public virtual void SendTaskCommand(OperationType operationType)
    {
        if (GameDataManager.Instance.GameMain.connectionPC.isConnect==false)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.TASK_SERVER_CONNECTION_FAIL_TIP));
            return;
        }

        if (TaskDataManager.Instance.IsCanSendTaskCommond(machine,TaskType.TAKEMATER,operationType)!=-1)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("当前操作无效的"));
            return;
        }
        
        if (operationType==OperationType.START)
        {
            DataManager.Instance.InsertHistoryLogMc("启动任务", GameDataManager.Instance.GetUserName(), machine);
            UpdateCurCtrMode(ref curTaskButtonCell,takeMaterStartBtn);
        }else if (operationType==OperationType.PAUSE)
        {
            DataManager.Instance.InsertHistoryLogMc("暂停任务", GameDataManager.Instance.GetUserName(), machine);
            UpdateCurCtrMode(ref curTaskButtonCell,takeMaterStopBtn);
        }else if (operationType==OperationType.REVERSING)
        {
            DataManager.Instance.InsertHistoryLogMc("任务换向", GameDataManager.Instance.GetUserName(), machine);
            UpdateCurCtrMode(ref curTaskButtonCell,takeMaterReversingBtn);
        }else if (operationType == OperationType.END)
        {
            DataManager.Instance.InsertHistoryLogMc("任务结束", GameDataManager.Instance.GetUserName(), machine);
            UpdateCurCtrMode(ref curTaskButtonCell,takeMaterEndBtn);
        }
        Debug.Log($"message {machine} {operationType}");
        TaskCommand taskCommand = new TaskCommand();
        taskCommand.QuerySystem = "MC";
        //taskCommand.Command_Type = 0;
        taskCommand.OperationCommand = operationType;
        taskCommand.TaskType = TaskType.TAKEMATER;
        taskCommand.Machine = machine;
        taskCommand.OperatorName = GameDataManager.Instance.GetUserName();
        taskCommand.TaskCreateTime = DateTime.Now;//.ToString("yyyy-MM-dd HH:mm:ss")
        taskCommand.OperatorSystem = "MC";
        if (operationType == OperationType.START)
        {
            taskCommand.Command_Type = 0;
            float startValue = startTakeMaterText.text == "" ? 0 : float.Parse(startTakeMaterText.text);
            float endValue = stopTakeMaterText.text == "" ? 0 : float.Parse(stopTakeMaterText.text);
            taskCommand.MaterialRange = new TaskRange(startValue, endValue);
            taskCommand.SideSelection = leftToggle.isOn ? "LIFT" : "RIGHT";
            float startLeftRightRangeValue = leftTakeMaterText.text == "" ? 0 : float.Parse(leftTakeMaterText.text);
            float endLeftRightRangeValue = rightTakeMaterText.text == "" ? 0 : float.Parse(rightTakeMaterText.text);
            taskCommand.LeftRightRange = new TaskRange(startLeftRightRangeValue, endLeftRightRangeValue);
            taskCommand.StepLength = takeMaterStep.text == "" ? 0 : float.Parse(takeMaterStep.text); ;
            taskCommand.IsTimed = timeOpenToggle.isOn;
            taskCommand.TimedAt = int.Parse(timeHourText.text) * 60 + int.Parse(timeMinuteText.text);
            taskCommand.IsQuantified = quantityOpenToggle.isOn;
            taskCommand.Quantity = int.Parse(takeMaterNum.text);
            taskCommand.TaskID = DateTime.Now.ToString("yyMMddHHmmss");
            taskCommand.LayerHigh = layerHigh.text == "" ? 0 : float.Parse(layerHigh.text);
            AllData allData = new AllData();
            taskCommand.AllData = allData;
        }
        else
        {
            taskCommand.Command_Type = 2;
        }
        Debug.LogError("TaskID" + taskCommand.TaskID);
        TaskDataManager.Instance.SendTaskCommand(taskCommand);
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
        takeMaterStartBtn.SetSystemState(false);
        takeMaterStopBtn.SetSystemState(false);
        takeMaterReversingBtn.SetSystemState(false);
        takeMaterEndBtn.SetSystemState(false);
        takeMaterStartBtn.SetSelectState(false);
        takeMaterStopBtn.SetSelectState(false);
        takeMaterReversingBtn.SetSelectState(false);
        takeMaterEndBtn.SetSelectState(false);
        curTaskButtonCell?.SetSelectState(false);
    }
    public virtual void InputFieldValueRange(InputField inputField, int min, int max)
    {
        inputField.text = "0";
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
