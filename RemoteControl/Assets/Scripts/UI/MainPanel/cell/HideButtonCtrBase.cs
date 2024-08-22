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
        AddOnClickListener(shakerStartBtn,(() =>SendMessageToServer(COMMAND_NAME.VIBRATOR_START) ));
        AddOnClickListener(shakerStopBtn,(() =>SendMessageToServer(COMMAND_NAME.VIBRATOR_STOP) ));
        AddOnClickListener(powerSupplyCloseBrakeBtn,(() =>SendMessageToServer(COMMAND_NAME.CONTROLPOWER_ON) ));
        AddOnClickListener(powerSupplyOpenBrakeBtn,(() =>SendMessageToServer(COMMAND_NAME.CONTROLPOWER_OFF) ));
        AddOnClickListener(engageClampBtn,(() =>SendMessageToServer(COMMAND_NAME.RAIL_CLAMP) ));
        AddOnClickListener(disengageClampBtn,(() =>SendMessageToServer(COMMAND_NAME.RAIL_RELAX) ));
        AddOnClickListener(impetusSupplyCloseBrakeBtn,(() =>SendMessageToServer(COMMAND_NAME.SUPPLYPOWER_ON) ));
        AddOnClickListener(impetusSupplyOpenBrakeBtn,(() =>SendMessageToServer( COMMAND_NAME.SUPPLYPOWER_OFF) ));
        AddOnClickListener(lightOpenBrakeBtn,(() =>SendMessageToServer(COMMAND_NAME.LIGHTPOWER_OFF) ));
        AddOnClickListener(lightCloseBrakeBtn,(() =>SendMessageToServer(COMMAND_NAME.LIGHTPOWER_ON) ));
        AddOnClickListener(systemLockBtn,(() =>SendMessageToServer( COMMAND_NAME.SYSTEM_LOCK) ));
        AddOnClickListener(systemUnlockBtn,(() =>SendMessageToServer(COMMAND_NAME.SYSTEM_UNLOCK) ));
        AddOnClickListener(oilPumpStartBtn,(() =>SendMessageToServer(COMMAND_NAME.OILBUMP_ON) ));
        AddOnClickListener(oilPumpStopBtn,(() =>SendMessageToServer(COMMAND_NAME.OILBUMP_OFF) ));
        AddOnClickListener(wiperStartBtn,(() =>SendMessageToServer("雨刷器启动") ));
        AddOnClickListener(wiperStopBtn,(() =>SendMessageToServer("雨刷器停止") ));
        AddOnClickListener(wiperSprayWaterBtn,(() =>SendMessageToServer("雨刷器喷水") ));
        AddOnClickListener(bypassBtn,(() =>SendMessageToServer( COMMAND_NAME.BYPASS_BUTTON) ));
        AddOnClickListener(cabDownBtn,(() =>SendMessageToServer("司机室下降") ));
        AddOnClickListener(cabUpBtn,(() =>SendMessageToServer("司机室上升") ));
        AddOnClickListener(draughtFanStartBtn,(() =>SendMessageToServer("变幅风机启动") ));
        AddOnClickListener(draughtFanStopBtn,(() =>SendMessageToServer("变幅风机停止") ));
        AddOnClickListener(bucketWheelStartBtn,(() =>SendMessageToServer(COMMAND_NAME.BUCKET_START) ));
        AddOnClickListener(bucketWheelStopBtn,(() =>SendMessageToServer(COMMAND_NAME.BUCKET_STOP) ));
        AddOnClickListener(heaterStartBtn,(() =>SendMessageToServer("加热器启动") ));
        AddOnClickListener(heaterStopBtn,(() =>SendMessageToServer("加热器停止") ));
        AddOnClickListener(cantileverTakeMaterStartBtn,(() =>SendMessageToServer(COMMAND_NAME.BELT_TAKE) ));
        AddOnClickListener(cantileverTakeMaterStopBtn,(() =>SendMessageToServer(COMMAND_NAME.BELT_STOP) ));
        AddOnClickListener(takeMaterDownBtn,(() =>SendMessageToServer(COMMAND_NAME.XBTB_DOWN_BUTTON) ));
        AddOnClickListener(takeMaterStopBtn,(() =>SendMessageToServer(COMMAND_NAME.XBTB_STOP_BUTTON) ));
    }

    public void SendMessageToServer(string message)
    {
        Debug.Log($"message { machine }   {message}");
    }
    public void SendMessageToServer(COMMAND_NAME command)
    {
        string commandName=machine == Machine.BucketWheelStackerReclaimer ? command.ToString()+"_1" : command.ToString()+"_2";
        Debug.Log($"message { machine }   {commandName}");
        switch (command)
        {
            case  COMMAND_NAME.RAIL_RELAX:
                disengageClampBtn.SetSelectState(true);
                engageClampBtn.SetSelectState(false);
                    break;
            case  COMMAND_NAME.RAIL_CLAMP: 
                engageClampBtn.SetSelectState(true);
                disengageClampBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.SUPPLYPOWER_ON:   
                impetusSupplyCloseBrakeBtn.SetSelectState(true);
                impetusSupplyOpenBrakeBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.SUPPLYPOWER_OFF:   
                impetusSupplyOpenBrakeBtn.SetSelectState(true);
                impetusSupplyCloseBrakeBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.CONTROLPOWER_ON:   
                powerSupplyCloseBrakeBtn.SetSelectState(true);
                powerSupplyOpenBrakeBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.CONTROLPOWER_OFF:   
                powerSupplyOpenBrakeBtn.SetSelectState(true);
                powerSupplyCloseBrakeBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.BELT_TAKE:   
                cantileverTakeMaterStartBtn.SetSelectState(true);
                cantileverTakeMaterStopBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.BELT_STACK:   
                break;
            case  COMMAND_NAME.BELT_STOP:  
                cantileverTakeMaterStopBtn.SetSelectState(true);
                cantileverTakeMaterStartBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.BUCKET_START:  
                bucketWheelStartBtn.SetSelectState(true);
                bucketWheelStopBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.BUCKET_STOP: 
                bucketWheelStopBtn.SetSelectState(true);
                bucketWheelStartBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.LIGHTPOWER_ON:   
                lightCloseBrakeBtn.SetSelectState(true);
                lightOpenBrakeBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.LIGHTPOWER_OFF:   
                lightOpenBrakeBtn.SetSelectState(true);
                lightCloseBrakeBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.OILBUMP_ON:   
                oilPumpStartBtn.SetSelectState(true);
                oilPumpStopBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.OILBUMP_OFF:   
                oilPumpStopBtn.SetSelectState(true);
                oilPumpStartBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.BELTTAKE_BUTTON:   
                break;
            case  COMMAND_NAME.BELTSTACK_BUTTON:   
                break;
            case  COMMAND_NAME.BELTSSTOP_BUTTON:   
                break;
            case  COMMAND_NAME.SYSTEM_UNLOCK:   
                systemUnlockBtn.SetSelectState(true);
                systemLockBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.SYSTEM_LOCK:   
                systemLockBtn.SetSelectState(true);
                systemUnlockBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.BYPASS_BUTTON:   
                bypassBtn.SetSelectState(!bypassBtn.select.activeSelf);
                break;
            case  COMMAND_NAME.XBTB_UP_BUTTON:   
                break;
            case  COMMAND_NAME.XBTB_DOWN_BUTTON:   
                takeMaterDownBtn.SetSelectState(true);
                takeMaterStopBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.XBTB_STOP_BUTTON:   
                takeMaterStopBtn.SetSelectState(true);
                takeMaterDownBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.VIBRATOR_START:   
                shakerStartBtn.SetSelectState(true);
                shakerStopBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.VIBRATOR_STOP:   
                shakerStopBtn.SetSelectState(true);
                shakerStartBtn.SetSelectState(false);
                break;
            case  COMMAND_NAME.SKRIT_TAKE_START:   
                break;
            case  COMMAND_NAME.SKRIT_STACK_START:   
                break;
            case  COMMAND_NAME.SKRIT_TAKE_STOP:   
                break;
            default:
                break;
        }
        GameDataManager.Instance.SendServerCommandByName(commandName);
    }
}