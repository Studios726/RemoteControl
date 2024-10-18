using ShenYangRemoteSystem.Subclass;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BucketWheelStackerReclaimerCtrMove : BucketWheelCtrMoveBase
{
    /// <summary>
    /// 当次取料量
    /// </summary>
    public Text thisPileMater;
    /// <summary>
    /// 当天取料量
    /// </summary>
    public Text dayPileMater;
    /// <summary>
    /// d堆料重置
    /// </summary>
    public Button pileMaterResetBtn;
    public ButtonCell pileMaterTakeBtn;

    public override void Start()
    {
        base.Start();
        // AddOnClickListener(pileMaterResetBtn,(() => SendMessageToServer("堆料重置")));
        AddOnClickListener(takeMaterBtn, (() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("是否将堆/取料控制切换为取料状态",null,(() =>
            {
                if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
                {
                    UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
                    return;
                }
                SendMessageToServer(COMMAND_NAME.BELTTAKE_BUTTON);
                takeMaterBtn.SetSelectState(true);
                stopTakeMaterBtn.SetSelectState(false);
                pileMaterTakeBtn.SetSelectState(false);
            })));
            
        }));
        AddOnClickListener(stopTakeMaterBtn, (() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("是否将堆/取料控制切换为停止状态",null,(() =>
            {
                if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
                {
                    UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
                    return;
                }
                SendMessageToServer(COMMAND_NAME.BELTSSTOP_BUTTON);
                takeMaterBtn.SetSelectState(false);
                stopTakeMaterBtn.SetSelectState(true);
                pileMaterTakeBtn.SetSelectState(false);
            })));
           
        }));
        AddOnClickListener(pileMaterTakeBtn,(() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("是否将堆/取料控制切换为堆料状态",null,(() =>
            {
                if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
                {
                    UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
                    return;
                }
                SendMessageToServer(COMMAND_NAME.BELTSTACK_BUTTON);
                takeMaterBtn.SetSelectState(false);
                stopTakeMaterBtn.SetSelectState(false);
                pileMaterTakeBtn.SetSelectState(true);
            })));
            
        }));
        
    }
    
    public override void UpdateData(SystemVariables data)
    {
        // if (machine == Machine.BucketWheelStackerReclaimer)
        // {
        //     if (stopTakeMaterBtn.red.activeSelf&&data.SR1_BeltTS_Stop_Swicth==false&&data.SR1_BeltStack_Swicth==true&&data.SuspensionGlueRunCommand)
        //     {
        //         PileTakeMaterPop(TaskType.PILEMATER,data.BeltRealyDis);
        //     }
        // }
        // else
        // {
        //     if (stopTakeMaterBtn.red.activeSelf&&data.SR1_BeltTS_Stop_Swicth_2==false&&data.SR1_BeltStack_Swicth_2==true&&data.SuspensionGlueRunCommand_2)
        //     {
        //         PileTakeMaterPop(TaskType.PILEMATER,data.BeltRealyDis_2);
        //     }
        // }
       
        base.UpdateData(data);
        pileMaterTakeBtn.SetSystemState(data.SR1_BeltStack_Swicth);
    }
}
