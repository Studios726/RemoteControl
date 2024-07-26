using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HideButtonCtrBase : PanelBase
{
    /// <summary>
    /// 高压分闸
    /// </summary>
    public ButtonCell highOpenBrake;

    /// <summary>
    /// 高压关闸
    /// </summary>
    public ButtonCell highCloseBrake;

    /// <summary>
    /// 振打器停止
    /// </summary>
    public ButtonCell shakerStopBtn;

    /// <summary>
    /// 振打器启动
    /// </summary>
    public ButtonCell shakerStartBtn;

    /// <summary>
    /// 控制电源合闸
    /// </summary>
    public ButtonCell powerSupplyCloseBrakeBtn;

    /// <summary>
    /// 控制电源分闸
    /// </summary>
    public ButtonCell powerSupplyOpenBrakeBtn;

    /// <summary>
    /// 夹轨器夹紧
    /// </summary>
    public ButtonCell engageClampBtn;

    /// <summary>
    /// 夹轨器松开
    /// </summary>
    public ButtonCell disengageClampBtn;

    /// <summary>
    /// 动力电源分闸
    /// </summary>
    public ButtonCell impetusSupplyOpenBrakeBtn;

    /// <summary>
    /// 动力电源合闸
    /// </summary>
    public ButtonCell impetusSupplyCloseBrakeBtn;

    /// <summary>
    /// 照明分闸
    /// </summary>
    public ButtonCell lightOpenBrakeBtn;

    /// <summary>
    /// 照明合闸
    /// </summary>
    public ButtonCell lightCloseBrakeBtn;

    /// <summary>
    /// 系统解锁
    /// </summary>
    public ButtonCell systemUnlockBtn;

    /// <summary>
    /// 系统连锁
    /// </summary>
    public ButtonCell systemLockBtn;

    /// <summary>
    /// 变幅油泵停止
    /// </summary>
    public ButtonCell oilPumpStopBtn;

    /// <summary>
    /// 变幅油泵启动
    /// </summary>
    public ButtonCell oilPumpStartBtn;

    /// <summary>
    /// 雨刷器停止
    /// </summary>
    public ButtonCell wiperStopBtn;

    /// <summary>
    /// 雨刷器启动
    /// </summary>
    public ButtonCell wiperStartBtn;

    /// <summary>
    /// 雨刷器喷水
    /// </summary>
    public ButtonCell wiperSprayWaterBtn;

    /// <summary>
    /// 旁路
    /// </summary>
    public ButtonCell bypassBtn;

    /// <summary>
    /// 斗轮停止
    /// </summary>
    public ButtonCell bucketWheelStopBtn;

    /// <summary>
    /// 斗轮启动
    /// </summary>
    public ButtonCell bucketWheelStartBtn;

    /// <summary>
    /// 司机室上升
    /// </summary>
    public ButtonCell cabUpBtn;

    /// <summary>
    /// 司机室下降
    /// </summary>
    public ButtonCell cabDownBtn;

    /// <summary>
    /// 变幅风机停止
    /// </summary>
    public ButtonCell draughtFanStopBtn;

    /// <summary>
    /// 变幅风机启动
    /// </summary>
    public ButtonCell draughtFanStartBtn;

    /// <summary>
    /// 加热器停止
    /// </summary>
    public ButtonCell heaterStopBtn;

    /// <summary>
    /// 加热器启动
    /// </summary>
    public ButtonCell heaterStartBtn;

    /// <summary>
    /// 悬臂取料
    /// </summary>
    public ButtonCell cantileverTakeMaterStartBtn;

    /// <summary>
    /// 悬臂停止
    /// </summary>
    public ButtonCell cantileverTakeMaterStopBtn;

    /// <summary>
    ///斗轮导料槽取料落下
    /// </summary>
    public ButtonCell takeMaterDownBtn;

    /// <summary>
    /// 斗轮导料槽停止
    /// </summary>
    public ButtonCell takeMaterStopBtn;

    public Machine machine;

    public virtual void Start()
    {
        Init();
    }

    public void Init()
    {
        AddOnClickListener(highOpenBrake,(() =>SendMessageToServer("高压分闸") ));
        AddOnClickListener(highCloseBrake,(() =>SendMessageToServer("高压合闸") ));
        AddOnClickListener(shakerStartBtn,(() =>SendMessageToServer("振打器启动") ));
        AddOnClickListener(shakerStopBtn,(() =>SendMessageToServer("振打器停止") ));
        AddOnClickListener(powerSupplyCloseBrakeBtn,(() =>SendMessageToServer("控制电源合闸") ));
        AddOnClickListener(powerSupplyOpenBrakeBtn,(() =>SendMessageToServer("控制电源分闸") ));
        AddOnClickListener(engageClampBtn,(() =>SendMessageToServer("夹轨器夹紧") ));
        AddOnClickListener(disengageClampBtn,(() =>SendMessageToServer("夹轨器松开") ));
        AddOnClickListener(impetusSupplyCloseBrakeBtn,(() =>SendMessageToServer("动力电源合闸") ));
        AddOnClickListener(impetusSupplyOpenBrakeBtn,(() =>SendMessageToServer("动力电源分闸") ));
        AddOnClickListener(lightOpenBrakeBtn,(() =>SendMessageToServer("照明分闸") ));
        AddOnClickListener(lightCloseBrakeBtn,(() =>SendMessageToServer("照面合闸") ));
        AddOnClickListener(systemLockBtn,(() =>SendMessageToServer("系统连锁") ));
        AddOnClickListener(systemUnlockBtn,(() =>SendMessageToServer("系统解锁") ));
        AddOnClickListener(oilPumpStartBtn,(() =>SendMessageToServer("变幅油泵启动") ));
        AddOnClickListener(oilPumpStopBtn,(() =>SendMessageToServer("变幅油泵停止") ));
        AddOnClickListener(wiperStartBtn,(() =>SendMessageToServer("雨刷器启动") ));
        AddOnClickListener(wiperStopBtn,(() =>SendMessageToServer("雨刷器停止") ));
        AddOnClickListener(wiperSprayWaterBtn,(() =>SendMessageToServer("雨刷器喷水") ));
        AddOnClickListener(bypassBtn,(() =>SendMessageToServer("旁路") ));
        AddOnClickListener(bucketWheelStartBtn,(() =>SendMessageToServer("斗轮启动") ));
        AddOnClickListener(bucketWheelStopBtn,(() =>SendMessageToServer("斗轮停止") ));
        AddOnClickListener(cabDownBtn,(() =>SendMessageToServer("司机室下降") ));
        AddOnClickListener(cabUpBtn,(() =>SendMessageToServer("司机室上升") ));
        AddOnClickListener(draughtFanStartBtn,(() =>SendMessageToServer("变幅油箱启动") ));
        AddOnClickListener(draughtFanStopBtn,(() =>SendMessageToServer("变幅油箱停止") ));
        AddOnClickListener(bucketWheelStartBtn,(() =>SendMessageToServer("斗轮启动") ));
        AddOnClickListener(bucketWheelStopBtn,(() =>SendMessageToServer("斗轮停止") ));
        AddOnClickListener(heaterStartBtn,(() =>SendMessageToServer("加热器启动") ));
        AddOnClickListener(heaterStopBtn,(() =>SendMessageToServer("加热器停止") ));
        AddOnClickListener(cantileverTakeMaterStartBtn,(() =>SendMessageToServer("悬臂取料") ));
        AddOnClickListener(cantileverTakeMaterStopBtn,(() =>SendMessageToServer("悬臂停止") ));
        AddOnClickListener(takeMaterDownBtn,(() =>SendMessageToServer("斗轮导料槽取料落下") ));
        AddOnClickListener(takeMaterStopBtn,(() =>SendMessageToServer("斗轮导料槽取料停止") ));
    }

    public void SendMessageToServer(string message)
    {
        Debug.Log($"message { machine }   {message}");
    }
}