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
            if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
                return;
            }
            SendMessageToServer(COMMAND_NAME.BELTTAKE_BUTTON);
            takeMaterBtn.SetSelectState(true);
            stopTakeMaterBtn.SetSelectState(false);
            pileMaterTakeBtn.SetSelectState(false);
        }));
        AddOnClickListener(stopTakeMaterBtn, (() =>
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
        }));
        AddOnClickListener(pileMaterTakeBtn,(() =>
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
        }));
        
    }
    public override void UpdateData(SystemVariables data)
    {
        //Debug.Log("更新堆取料机碰撞信息");
        base.UpdateData(data);
    }
}
