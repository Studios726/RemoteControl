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
    /// 分流挡板堆料抬起
    /// </summary>
    public  ButtonCell damBoardPileMaterUpBtn;
    /// <summary>
    /// 分流挡板堆料停止
    /// </summary>
    public  ButtonCell damBoardPileMaterStopBtn;

    public override void Start()
    {
        base.Start();
        AddOnClickListener(pileMaterBtn, () =>SendMessageToServer("堆料"));
        AddOnClickListener(pileMaterUpBtn, () =>SendMessageToServer("堆料抬起"));
        AddOnClickListener(damBoardPileMaterDownBtn, () =>SendMessageToServer("分流挡板堆料落下"));
        AddOnClickListener(damBoardPileMaterUpBtn, () =>SendMessageToServer("分流挡板堆料抬起"));
        AddOnClickListener(damBoardPileMaterStopBtn, () =>SendMessageToServer("分流挡板堆料停止"));
    }
}