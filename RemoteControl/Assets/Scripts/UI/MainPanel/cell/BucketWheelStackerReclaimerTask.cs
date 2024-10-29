using System;
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
    public ButtonCell pileMaterStartBtn;
    public ButtonCell pileMaterStopBtn;
    public ButtonCell pileMaterEndBtn;
    public ButtonCell curPileTaskButtonCell;
    public override void Start()
    {
        base.Start();
        InputFieldValueRange(startPileMaterText, 0, 260);
        InputFieldValueRange(endPileMaterText, 0, 260);
        InputFieldValueRange(startLeftPileMaterText, 18, 42);
        InputFieldValueRange(endLeftPileMaterText, 18, 42);
        InputFieldValueRange(pileMaterHeightText, 0, 10);
        AddOnClickListener(pileMaterStartBtn,(() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否启动自动作业？", null, () => SendPileMaterCommand(OperationType.START)));
          
        }));
        AddOnClickListener(pileMaterStopBtn,(() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否暂停自动作业？", null, () => SendPileMaterCommand(OperationType.PAUSE)));
      
        }));
        AddOnClickListener(pileMaterEndBtn,(() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否结束自动作业？", null, () =>    SendPileMaterCommand(OperationType.END)));
        
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
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.TASK_SERVER_CONNECTION_FAIL_TIP));
            return;
        }
        
        if (TaskDataManager.Instance.IsCanSendTaskCommond(machine,TaskType.PILEMATER,operationType)!=-1)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("当前操作无效的"));
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
        taskCommand.TaskCreateTime = DateTime.Now;
        if (operationType==OperationType.START)
        {
            taskCommand.AutoMode = PileAutoMaxToggle.red.activeSelf ? AutoMode.AUTOMAX : AutoMode.SemiAuto;
            taskCommand.AngleEntryMode = PileRightAngleToggle.red.activeSelf ? AngleEntryMode.RIGHTANGLE : AngleEntryMode.OBLIQUEANGLE;
            taskCommand.Command_Type = 0;
            float startValue = startPileMaterText.text == "" ? 0 : float.Parse(startPileMaterText.text);
            float endValue = endPileMaterText.text == "" ? 0 : float.Parse(endPileMaterText.text);
            taskCommand.MaterialRange = new TaskRange(startValue, endValue);
            taskCommand.SideSelection = leftPileMaterToggle.red.activeSelf ? "LIFT" : "RIGHT";
            float startLeftRightRangeValue = startLeftPileMaterText.text == "" ? 0 : float.Parse(startLeftPileMaterText.text);
            float endLeftRightRangeValue = endLeftPileMaterText.text == "" ? 0 : float.Parse(endLeftPileMaterText.text);
            taskCommand.LeftRightRange = new TaskRange(startLeftRightRangeValue, endLeftRightRangeValue);
            taskCommand.StepLength = takeMaterStep.text == "" ? 0 : float.Parse(takeMaterStep.text);
            taskCommand.IsTimed = timeOpenToggle.isOn;
            taskCommand.TimedAt = int.Parse(timeHourText.text) * 60 + int.Parse(timeMinuteText.text);
            taskCommand.IsQuantified = quantityOpenToggle.isOn;
            taskCommand.Quantity = int.Parse(takeMaterNum.text);
            taskCommand.TaskID = DateTime.Now.ToString("yyMMddHHmmss");
            taskCommand.OperatorSystem = "MC";
            taskCommand.LayerHigh = 0;
            taskCommand.TakeMateHigh= float.Parse(pileMaterHeightText.text);
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
        pileMaterStartBtn.SetSystemState(false,true);
        pileMaterStopBtn.SetSystemState(false,true);
        pileMaterEndBtn.SetSystemState(false,true);
        
        pileMaterStartBtn.SetSelectState(false);
        pileMaterStopBtn.SetSelectState(false);
        pileMaterEndBtn.SetSelectState(false);
        
        PileAutoMaxToggle.SetSystemState(false,true);
        PileSemiAutoToggle.SetSystemState(true,true);
        PileRightAngleToggle.SetSystemState(true,true);
        PileObliqueAngleToggle.SetSystemState(false,true);
    }
}
