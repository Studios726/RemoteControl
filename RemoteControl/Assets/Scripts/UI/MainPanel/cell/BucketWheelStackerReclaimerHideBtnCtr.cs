
using ShenYangRemoteSystem.Subclass;
public class BucketWheelStackerReclaimerHideBtnCtr : HideButtonCtrBase
{
    /// <summary>
    /// 堆料
    /// </summary>
    public ButtonCell pileMaterBtn;
    /// <summary>
    /// 堆料抬起
    /// </summary>
    public  ButtonCell pileMaterUpBtn;
    /// <summary>
    /// 分流挡板堆料落下
    /// </summary>
    public  ButtonCell damBoardPileMaterDownBtn;
    /// <summary>
    /// 分流挡板取料抬起
    /// </summary>
    public  ButtonCell damBoardPileMaterUpBtn;
    /// <summary>
    /// 分流挡板堆料停止
    /// </summary>
    public  ButtonCell damBoardPileMaterStopBtn;

    public override void Start()
    {
        base.Start();
        AddOnClickListener(pileMaterBtn, () =>
        {
            // if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
            // {
            //     UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
            //     return;
            // }
            SendMessageToServer(COMMAND_NAME.BELT_STACK,(() =>
            {
                pileMaterBtn.SetSelectState(true);
                cantileverTakeMaterStartBtn.SetSelectState(false);
                cantileverTakeMaterStopBtn.SetSelectState(false);
            }));
        });
        
        AddOnClickListener(cantileverTakeMaterStartBtn, () =>
        {
            // if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
            // {
            //     UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
            //     return;
            // }
            SendMessageToServer(COMMAND_NAME.BELT_TAKE,(() =>
            {
                pileMaterBtn.SetSelectState(false);
                cantileverTakeMaterStartBtn.SetSelectState(true);
                cantileverTakeMaterStopBtn.SetSelectState(false);
            }));
          
        });
        
        AddOnClickListener(cantileverTakeMaterStopBtn, () =>
        {
            // if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
            // {
            //     UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
            //     return;
            // }
            SendMessageToServer(COMMAND_NAME.BELT_STOP,(() =>
            {
                pileMaterBtn.SetSelectState(false);
                cantileverTakeMaterStartBtn.SetSelectState(false);
                cantileverTakeMaterStopBtn.SetSelectState(true);
            }));
      
        });
        
        AddOnClickListener(pileMaterUpBtn, () =>
        {
            // if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
            // {
            //     UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
            //     return;
            // }
            SendMessageToServer(COMMAND_NAME.XBTB_UP_BUTTON,(() =>
            {
                pileMaterUpBtn.SetSelectState(true);
                takeMaterDownBtn.SetSelectState(false);
                takeMaterStopBtn.SetSelectState(false);
            }));
         
        });
        
        AddOnClickListener(takeMaterDownBtn, () =>
        {
            // if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
            // {
            //     UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
            //     return;
            // }
            SendMessageToServer(COMMAND_NAME.XBTB_DOWN_BUTTON,(() =>
            {
                pileMaterUpBtn.SetSelectState(false);
                takeMaterDownBtn.SetSelectState(true);
                takeMaterStopBtn.SetSelectState(false);
            }));
     
        });
        
        AddOnClickListener(takeMaterStopBtn, () =>
        {
            // if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
            // {
            //     UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
            //     return;
            // }
            SendMessageToServer(COMMAND_NAME.XBTB_STOP_BUTTON,(() =>
            {
                pileMaterUpBtn.SetSelectState(false);
                takeMaterDownBtn.SetSelectState(false);
                takeMaterStopBtn.SetSelectState(true);
            }));
       
        });
        
        AddOnClickListener(damBoardPileMaterDownBtn, () =>
        {
            // if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
            // {
            //     UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
            //     return;
            // }
            SendMessageToServer(COMMAND_NAME.SKRIT_STACK_START,(() =>
            {
                damBoardPileMaterDownBtn.SetSelectState(true);
                damBoardPileMaterUpBtn.SetSelectState(false);
                damBoardPileMaterStopBtn.SetSelectState(false);
            }));
        
        });
        AddOnClickListener(damBoardPileMaterUpBtn, () =>
        {
            // if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
            // {
            //     UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
            //     return;
            // }
            SendMessageToServer(COMMAND_NAME.SKRIT_TAKE_START,(() =>
            {
                damBoardPileMaterUpBtn.SetSelectState(true);
                damBoardPileMaterDownBtn.SetSelectState(false);
                damBoardPileMaterStopBtn.SetSelectState(false);
            }));
          
        });
        AddOnClickListener(damBoardPileMaterStopBtn, () =>
        {
            // if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
            // {
            //     UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
            //     return;
            // }
            SendMessageToServer(COMMAND_NAME.SKRIT_TAKE_STOP,(() =>
            {
                damBoardPileMaterStopBtn.SetSelectState(true);
                damBoardPileMaterDownBtn.SetSelectState(false);
                damBoardPileMaterUpBtn.SetSelectState(false);
            }));
         
        });
    }

    public override void UpdateData(SystemVariables data)
    {
        if (machine==Machine.BucketWheelStackerReclaimer)
        {
            if (data.SuspensionBeltMaterialLoadingRunningContact==true&&data.Single_Action&&data.SuspensionGlueRunCommand&&cantileverTakeMaterStopBtn.red.activeSelf)
            {
                PileTakeMaterPop(TaskType.PILEMATER,data.BeltRealyDis);
            }
        }
        else
        {
            if (data.SuspensionBeltMaterialLoadingRunningContact_2==true&&data.Single_Action_2&&data.SuspensionGlueRunCommand_2&&cantileverTakeMaterStopBtn.red.activeSelf)
            {
                PileTakeMaterPop(TaskType.TAKEMATER,data.BeltRealyDis_2);
            }
        }
        base.UpdateData(data);
        pileMaterBtn.SetSystemState(data.SuspensionBeltMaterialLoadingRunningContact);
        pileMaterUpBtn.SetSystemState(data.BucketWheelSlotLiftLimit);
        damBoardPileMaterDownBtn.SetSystemState(data.BaffleDownLimit);
        damBoardPileMaterUpBtn.SetSystemState(data.BaffleUpLimit);
        if (data.BaffleDownLimit==false&&data.BaffleUpLimit==false)
        {
            damBoardPileMaterStopBtn.SetSystemState(true);
        }
        else
        {
            damBoardPileMaterStopBtn.SetSystemState(false);
        }
        
    }
}
