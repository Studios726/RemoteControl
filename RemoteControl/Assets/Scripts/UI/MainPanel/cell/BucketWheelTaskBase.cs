using System;
using System.Collections.Generic;
using RemoteControl.Event;
using ShenYangRemoteSystem.Subclass;
using UnityEngine;
using UnityEngine.UI;
using Utility;

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

    public virtual void Start()
    {
        Init();
    }

    public virtual void UpdateDes(List<WarningCellData> datas)
    {
        // warningTexts[0].text ="";
        // warningTexts[1].text ="";
        // warningTexts[2].text ="";
        // if (queue.Count>0)
        // {
        //     int index = 0;
        //     foreach (var data in queue)
        //     {
        //         warningTexts[index].text = data.Des;
        //         index = index + 1;
        //     }
        // }
        warningList.RefreshList(datas);
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
        if (taskCommand.SideSelection == "LIFT")
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
        RightAngleToggle.SetSystemState(taskCommand.AngleEntryMode==AngleEntryMode.RIGHTANGLE,true);
        ObliqueAngleToggle.SetSystemState(taskCommand.AngleEntryMode==AngleEntryMode.OBLIQUEANGLE,true);
        leftTakeMaterText.text = taskCommand.LeftRightRange.startValue.ToString();
        rightTakeMaterText.text = taskCommand.LeftRightRange.endValue.ToString();
        takeMaterStep.text = taskCommand.StepLength.ToString();
        layerHigh.text = taskCommand.LayerHigh.ToString();
        timeOpenToggle.isOn = taskCommand.IsTimed;
        useTimeBtn.SetSystemState(taskCommand.IsTimed,true);
        if (timeOpenToggle.isOn)
        {
            quantityOpenToggle.isOn = false;
            timeHourText.text = Mathf.FloorToInt(taskCommand.TimedAt / 60).ToString();
            timeMinuteText.text = (taskCommand.TimedAt % 60).ToString();
        }
        else
        {
            quantityOpenToggle.isOn = true;
            timeOpenToggle.isOn = false;
            takeMaterNum.text = taskCommand.Quantity.ToString();
        }

        takeMaterStartBtn.SetSystemState(taskCommand.AllData.OperationCommandList[0] == 1,true);
        takeMaterStopBtn.SetSystemState(taskCommand.AllData.OperationCommandList[1] == 1,true);
        resetTaskBtn.SetSystemState(taskCommand.ResetState==1,true);
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
                        new ConfirmPanelArgs("是否确认复位急停？", null, () =>  SendPlcCommand(COMMAND_NAME.EMERGENCY_STOP)));
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
                    new ConfirmPanelArgs("是否确认复位？", null, () => SendPlcCommand(COMMAND_NAME.ERR_RESET)));
            }));
        // AddOnClickListener(warningBtn, (() => { SendPlcCommand(COMMAND_NAME.STARTUP_ALARM); }));
        AddOnClickListener(resetTaskBtn,(() =>
        {
              UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                            new ConfirmPanelArgs("是否重置自动作业参数？", null, () => 
                                SendTaskCommand(OperationType.RESET)));
        }));
        AddOnClickListener(takeMaterStartBtn, (() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否启动自动作业？", null, () => SendTaskCommand(OperationType.START)));
            
        }));
        AddOnClickListener(takeMaterStopBtn, (() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否暂停自动作业？", null, () => SendTaskCommand(OperationType.PAUSE)));
         
        }));
        AddOnClickListener(takeMaterReversingBtn, (() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否换向自动作业？", null, () =>   SendTaskCommand(OperationType.REVERSING)));
          
        }));
        AddOnClickListener(takeMaterEndBtn, (() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否结束自动作业？", null, () =>    SendTaskCommand(OperationType.END)));
          
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
        } ));
        AddOnClickListener(SemiAutoToggle,(() =>
        {
            AutoMaxToggle.SetSystemState(false,true);
            SemiAutoToggle.SetSystemState(true,true);
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
        
        AddOnClickListener(useTimeBtn,(() =>
        {
            useTimeBtn.SetSystemState(!useTimeBtn.red.activeSelf,true);
        }));
        if (machine==Machine.BucketWheelStackerReclaimer)
        {
            InputFieldValueRange(startTakeMaterText, 0, 260);
            InputFieldValueRange(stopTakeMaterText, 0, 260);
        }
        else
        {
            InputFieldValueRange(startTakeMaterText, 153, 323);
            InputFieldValueRange(stopTakeMaterText, 153, 323);
        }
       
        InputFieldValueRange(leftTakeMaterText, 0, 42);
        InputFieldValueRange(rightTakeMaterText, 0, 42);
        InputFieldValueRange(timeHourText, 0, 99);
        InputFieldValueRange(timeMinuteText, 0, 60);
        InputFieldValueRange(takeMaterStep, 0.1f, 1);
        InputFieldValueRange(takeMaterNum, 0, 99999);
        InputFieldValueRange(layerHigh, 0, 10);
        EventManager.Instance.TriggerEvent(EventName.UpdatePcData, null);
    }

    public virtual void WarningDown()
    {
        Debug.LogError("按下"); //1
        warningBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        warningBtn.transform.Find("Image").gameObject.SetActive(true);
        warningBtn.transform.FindComponent<Text>("Text").color = new Color(1, 0, 0.1803922f, 1);
    // private Color textColor1 = new Color(0.1411765f, 1, 1, 1);
    // private Color textColor2 = new Color(1, 0, 0.1803922f, 1);
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
            UIManager.Instance.OpenUI(UIID.ConfirmPanel, new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
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
                new ConfirmPanelArgs(ConstStr.TASK_SERVER_CONNECTION_FAIL_TIP));
            return;
        }

        if (TaskDataManager.Instance.IsCanSendTaskCommond(machine, TaskType.TAKEMATER, operationType) != -1)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel, new ConfirmPanelArgs("当前操作无效的"));
            return;
        }
        TaskCommand taskCommand = new TaskCommand();
        if (operationType == OperationType.START)
        {
            DataManager.Instance.InsertHistoryLogMc("取料机-启动任务", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.OperationCommand = operationType;
            UpdateCurCtrMode(ref curTaskButtonCell, takeMaterStartBtn);
        }
        else if (operationType == OperationType.PAUSE)
        {
            DataManager.Instance.InsertHistoryLogMc("取料机-暂停任务", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.OperationCommand = operationType;
            UpdateCurCtrMode(ref curTaskButtonCell, takeMaterStopBtn);
        }
        else if (operationType == OperationType.REVERSING)
        {
            DataManager.Instance.InsertHistoryLogMc("取料机-任务换向", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.OperationCommand = operationType;
            UpdateCurCtrMode(ref curTaskButtonCell, takeMaterReversingBtn);
        }
        else if (operationType == OperationType.END)
        {
            DataManager.Instance.InsertHistoryLogMc("取料机-任务结束", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.OperationCommand = operationType;
            UpdateCurCtrMode(ref curTaskButtonCell, takeMaterEndBtn);
        }else if (operationType == OperationType.RESET)
        {
            resetTaskBtn.SetSelectState(true);
            DataManager.Instance.InsertHistoryLogMc("取料机-任务重置", GameDataManager.Instance.GetUserName(), machine);
            taskCommand.ResetState = 1;
        }
        taskCommand.QuerySystem = "MC";
        //taskCommand.Command_Type = 0;
        taskCommand.TaskType = TaskType.TAKEMATER;
        taskCommand.Machine = machine;
        taskCommand.OperatorName = GameDataManager.Instance.GetUserName();
        taskCommand.TaskCreateTime = DateTime.Now; //.ToString("yyyy-MM-dd HH:mm:ss")
        taskCommand.OperatorSystem = "MC";
        if (operationType == OperationType.START || operationType == OperationType.RESET)
        {
            taskCommand.AutoMode = AutoMaxToggle.red.activeSelf ? AutoMode.AUTOMAX : AutoMode.SemiAuto;
            taskCommand.AngleEntryMode=RightAngleToggle.red.activeSelf?AngleEntryMode.RIGHTANGLE:AngleEntryMode.OBLIQUEANGLE;
            taskCommand.Command_Type = operationType == OperationType.RESET?2:0;
            float startValue = startTakeMaterText.text == "" ? 0 : float.Parse(startTakeMaterText.text);
            float endValue = stopTakeMaterText.text == "" ? 0 : float.Parse(stopTakeMaterText.text);
            taskCommand.MaterialRange = new TaskRange(startValue, endValue);
            taskCommand.SideSelection = leftToggle.red.activeSelf ? "LIFT" : "RIGHT";
            float startLeftRightRangeValue = leftTakeMaterText.text == "" ? 0 : float.Parse(leftTakeMaterText.text);
            float endLeftRightRangeValue = rightTakeMaterText.text == "" ? 0 : float.Parse(rightTakeMaterText.text);
            taskCommand.LeftRightRange = new TaskRange(startLeftRightRangeValue, endLeftRightRangeValue);
            taskCommand.StepLength = takeMaterStep.text == "" ? 0 : float.Parse(takeMaterStep.text);
            ;
            taskCommand.IsTimed =useTimeBtn.red.activeSelf;
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
        RightAngleToggle.SetSystemState(true, true);
        ObliqueAngleToggle.SetSystemState(false, true);
    }

    public virtual void InputFieldValueRange(InputField inputField, float min, float max)
    {
        inputField.text =min.ToString();
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