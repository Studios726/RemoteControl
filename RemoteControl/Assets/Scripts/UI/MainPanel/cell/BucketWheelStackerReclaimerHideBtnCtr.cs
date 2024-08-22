using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            SendMessageToServer(COMMAND_NAME.BELT_STACK);
            pileMaterBtn.SetSelectState(true);
            cantileverTakeMaterStartBtn.SetSelectState(false);
            cantileverTakeMaterStopBtn.SetSelectState(false);
        });
        
        AddOnClickListener(cantileverTakeMaterStartBtn, () =>
        {
            SendMessageToServer(COMMAND_NAME.BELT_TAKE);
            pileMaterBtn.SetSelectState(false);
            cantileverTakeMaterStartBtn.SetSelectState(true);
            cantileverTakeMaterStopBtn.SetSelectState(false);
        });
        
        AddOnClickListener(cantileverTakeMaterStopBtn, () =>
        {
            SendMessageToServer(COMMAND_NAME.BELT_STOP);
            pileMaterBtn.SetSelectState(false);
            cantileverTakeMaterStartBtn.SetSelectState(false);
            cantileverTakeMaterStopBtn.SetSelectState(true);
        });
        
        AddOnClickListener(pileMaterUpBtn, () =>
        {
            SendMessageToServer(COMMAND_NAME.XBTB_UP_BUTTON);
            pileMaterUpBtn.SetSelectState(true);
            takeMaterDownBtn.SetSelectState(false);
            takeMaterStopBtn.SetSelectState(false);
        });
        
        AddOnClickListener(takeMaterDownBtn, () =>
        {
            SendMessageToServer(COMMAND_NAME.XBTB_DOWN_BUTTON);
            pileMaterUpBtn.SetSelectState(false);
            takeMaterDownBtn.SetSelectState(true);
            takeMaterStopBtn.SetSelectState(false);
        });
        
        AddOnClickListener(takeMaterStopBtn, () =>
        {
            SendMessageToServer(COMMAND_NAME.XBTB_STOP_BUTTON);
            pileMaterUpBtn.SetSelectState(false);
            takeMaterDownBtn.SetSelectState(false);
            takeMaterStopBtn.SetSelectState(true);
        });
        
        AddOnClickListener(damBoardPileMaterDownBtn, () =>
        {
            SendMessageToServer(COMMAND_NAME.SKRIT_STACK_START);
            damBoardPileMaterDownBtn.SetSelectState(true);
            damBoardPileMaterUpBtn.SetSelectState(false);
            damBoardPileMaterStopBtn.SetSelectState(false);
        });
        AddOnClickListener(damBoardPileMaterUpBtn, () =>
        {
            SendMessageToServer(COMMAND_NAME.SKRIT_TAKE_START);
            damBoardPileMaterUpBtn.SetSelectState(true);
            damBoardPileMaterDownBtn.SetSelectState(false);
            damBoardPileMaterStopBtn.SetSelectState(false);
        });
        AddOnClickListener(damBoardPileMaterStopBtn, () =>
        {
            SendMessageToServer(COMMAND_NAME.SKRIT_TAKE_STOP);
            damBoardPileMaterStopBtn.SetSelectState(true);
            damBoardPileMaterDownBtn.SetSelectState(false);
            damBoardPileMaterUpBtn.SetSelectState(false);
        });
    }
}
