using System;
using System.Collections;
using System.Collections.Generic;
using ShenYangRemoteSystem.Subclass;
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
        // AddOnClickListener(highOpenBrake,(() =>SendMessageToServer("高压分闸") ));
        // AddOnClickListener(highCloseBrake,(() =>SendMessageToServer("高压合闸") ));
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
        AddOnClickListener(draughtFanStartBtn,(() =>SendMessageToServer(COMMAND_NAME.LUFF_FAN_START) ));
        AddOnClickListener(draughtFanStopBtn,(() =>SendMessageToServer(COMMAND_NAME.LUFF_FAN_STOP) ));
        AddOnClickListener(bucketWheelStartBtn,(() =>SendMessageToServer(COMMAND_NAME.BUCKET_START) ));
        AddOnClickListener(bucketWheelStopBtn,(() =>SendMessageToServer(COMMAND_NAME.BUCKET_STOP) ));
        AddOnClickListener(heaterStartBtn,(() =>SendMessageToServer(COMMAND_NAME.LUFF_HART_START) ));
        AddOnClickListener(heaterStopBtn,(() =>SendMessageToServer(COMMAND_NAME.LUFF_HART_STOP) ));
        AddOnClickListener(cantileverTakeMaterStartBtn,(() =>SendMessageToServer(COMMAND_NAME.BELT_TAKE) ));
        AddOnClickListener(cantileverTakeMaterStopBtn,(() =>SendMessageToServer(COMMAND_NAME.BELT_STOP) ));
        AddOnClickListener(takeMaterDownBtn,(() =>SendMessageToServer(COMMAND_NAME.XBTB_DOWN_BUTTON) ));
        AddOnClickListener(takeMaterStopBtn,(() =>SendMessageToServer(COMMAND_NAME.XBTB_STOP_BUTTON) ));
    }

    public virtual void UpdateData(SystemVariables data)
    {
        if (machine== Machine.BucketWheelStackerReclaimer)
        {
            shakerStopBtn.SetSystemState(data.VibrationMotorRunning==false);
            shakerStartBtn.SetSystemState(data.VibrationMotorRunning);
            powerSupplyCloseBrakeBtn.SetSystemState(data.LowVoltageControlPowerClosed);
            powerSupplyOpenBrakeBtn.SetSystemState(data.LowVoltageControlPowerClosed==false);
            if (data.LeftClampRelaxLimit==true && data.RightClampRelaxLimit==true)
            {
                disengageClampBtn.SetSystemState(true);
            }
            else
            {
                engageClampBtn.SetSystemState(true);
            }
            impetusSupplyOpenBrakeBtn.SetSystemState(data.LowVoltagePowerClosed==false);
            impetusSupplyCloseBrakeBtn.SetSystemState(data.LowVoltagePowerClosed);
            lightCloseBrakeBtn.SetSystemState(data.LightPowerClosed);
            lightOpenBrakeBtn.SetSystemState(data.LightPowerClosed==false);
            systemUnlockBtn.SetSystemState(data.SR1_Interlock_Swich==false);
            systemLockBtn.SetSystemState(data.SR1_Interlock_Swich);
            oilPumpStopBtn.SetSystemState(data.VariableAmplitudeOilPumpMotorRunning==false);
            oilPumpStartBtn.SetSystemState(data.VariableAmplitudeOilPumpMotorRunning);
            bypassBtn.SetSystemState(data.SR1_SCADA_ByPass_SB);
            bucketWheelStartBtn.SetSystemState(data.BucketWheelMotorRunning);
            bucketWheelStopBtn.SetSystemState(data.BucketWheelMotorRunning==false);
            cantileverTakeMaterStartBtn.SetSystemState(data.SuspensionBeltMaterialUnloadingRunningContact);
            draughtFanStartBtn.SetSystemState(data.VariableAmplitudeFanRunning);
            draughtFanStopBtn.SetSystemState(data.VariableAmplitudeFanRunning==false);
            heaterStartBtn.SetSystemState(data.VariableAmplitudeOilHeaterRunning);
            heaterStopBtn.SetSystemState(data.VariableAmplitudeOilHeaterRunning==false);
            
            if (data.SuspensionBeltMaterialUnloadingRunningContact==false &&data.SuspensionBeltMaterialLoadingRunningContact==false)
            {
                cantileverTakeMaterStopBtn.SetSystemState(true);
            }
            else
            {
                cantileverTakeMaterStopBtn.SetSystemState(false);
            }
            takeMaterDownBtn.SetSystemState(data.BucketWheelSlotLowerLimit);
            if (data.BucketWheelSlotLowerLimit==false&&data.BucketWheelSlotLiftLimit==false)
            {
                takeMaterStopBtn.SetSystemState(true);
            }
        }
        else
        {
            shakerStopBtn.SetSystemState(data.VibrationMotorRunning_2==false);
            shakerStartBtn.SetSystemState(data.VibrationMotorRunning_2);
            powerSupplyCloseBrakeBtn.SetSystemState(data.LowVoltageControlPowerClosed_2);
            powerSupplyOpenBrakeBtn.SetSystemState(data.LowVoltageControlPowerClosed_2==false);
            if (data.LeftClampRelaxLimit_2==true && data.RightClampRelaxLimit_2==true)
            {
                disengageClampBtn.SetSystemState(true);
                engageClampBtn.SetSystemState(false);
            }
            else
            {
                engageClampBtn.SetSystemState(true);
                disengageClampBtn.SetSystemState(false);
            }
            impetusSupplyOpenBrakeBtn.SetSystemState(data.LowVoltagePowerClosed_2==false);
            impetusSupplyCloseBrakeBtn.SetSystemState(data.LowVoltagePowerClosed_2);
            lightCloseBrakeBtn.SetSystemState(data.LightPowerClosed_2);
            lightOpenBrakeBtn.SetSystemState(data.LightPowerClosed_2==false);
            // systemUnlockBtn.SetSystemState(data.SR1_Interlock_Swich==false);
            // systemLockBtn.SetSystemState(data.SR1_Interlock_Swich);
            oilPumpStopBtn.SetSystemState(data.VariableAmplitudeOilPumpMotorRunning_2==false);
            oilPumpStartBtn.SetSystemState(data.VariableAmplitudeOilPumpMotorRunning_2);
            // bypassBtn.SetSystemState(data.SR1_SCADA_ByPass_SB);
            bucketWheelStartBtn.SetSystemState(data.BucketWheelMotorRunning_2);
            bucketWheelStopBtn.SetSystemState(data.BucketWheelMotorRunning_2==false);
            cantileverTakeMaterStartBtn.SetSystemState(data.SuspensionBeltMaterialUnloadingRunningContact_2);
            
            draughtFanStartBtn.SetSystemState(data.VariableAmplitudeFanRunning);
            draughtFanStopBtn.SetSystemState(data.VariableAmplitudeFanRunning==false);
            heaterStartBtn.SetSystemState(data.VariableAmplitudeOilHeaterRunning);
            heaterStopBtn.SetSystemState(data.VariableAmplitudeOilHeaterRunning==false);
            
            if (data.SuspensionBeltMaterialUnloadingRunningContact_2==false &&data.SuspensionBeltMaterialLoadingRunningContact_2==false)
            {
                cantileverTakeMaterStopBtn.SetSystemState(true);
            }
            else
            {
                cantileverTakeMaterStopBtn.SetSystemState(false);
            }
            takeMaterDownBtn.SetSystemState(data.BucketWheelSlotLowerLimit_2);
            if (data.BucketWheelSlotLowerLimit_2==false&&data.BucketWheelSlotLiftLimit_2==false)
            {
                takeMaterStopBtn.SetSystemState(true);
            }
        }
    }
    public void SendMessageToServer(string message)
    {
        Debug.Log($"message { machine }   {message}");
    }
    public void SendMessageToServer(COMMAND_NAME command,Action action=null)
    {
        if (GameDataManager.Instance.GameMain.connectionRC.isConnect==false)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
            return;
        }

        string des = "";
        switch (command)
        {
            case  COMMAND_NAME.RAIL_RELAX:
                des = "是否确认夹轨器放松?";
                    break;
            case  COMMAND_NAME.RAIL_CLAMP: 
                des = "是否确认夹轨器夹紧?";
                break;
            case  COMMAND_NAME.SUPPLYPOWER_ON:
                des = "是否确认动力电源合闸?";
                break;
            case  COMMAND_NAME.SUPPLYPOWER_OFF:   
                des = "是否确认动力电源分闸?";
                break;
            case  COMMAND_NAME.CONTROLPOWER_ON:   
                des = "是否确认控制电源合闸?";
                break;
            case  COMMAND_NAME.CONTROLPOWER_OFF:   
                des = "是否确认控制电源分闸?";
                break;
            case  COMMAND_NAME.BELT_TAKE:   
                des = "是否确认悬臂皮带取料?";
                break;
            case  COMMAND_NAME.BELT_STACK: 
                des = "是否确认悬臂皮带堆料?";
                break;
            case  COMMAND_NAME.BELT_STOP:  
                des = "是否确认悬臂皮带停止?";
                break;
            case  COMMAND_NAME.BUCKET_START:  
                des = "是否确认斗轮启动?";
                break;
            case  COMMAND_NAME.BUCKET_STOP: 
                des = "是否确认斗轮停止?";
                break;
            case  COMMAND_NAME.LIGHTPOWER_ON:   
                des = "是否确认照明合闸?";
                break;
            case  COMMAND_NAME.LIGHTPOWER_OFF:   
                des = "是否确认照明分闸?";
                break;
            case  COMMAND_NAME.OILBUMP_ON:   
                des = "是否确认主车油泵启动?";
                break;
            case  COMMAND_NAME.OILBUMP_OFF:   
                des = "是否确认主车油泵关闭?";
                break;
            case  COMMAND_NAME.BELTTAKE_BUTTON:   
                des = "是否确认取料开关?";
                break;
            case  COMMAND_NAME.BELTSTACK_BUTTON:  
                des = "是否确认堆料开关?";
                break;
            case  COMMAND_NAME.BELTSSTOP_BUTTON:   
                des = "是否确认堆取料停止开关?";
                break;
            case  COMMAND_NAME.SYSTEM_UNLOCK:   
                des = "是否确认与系统解锁?";
                break;
            case  COMMAND_NAME.SYSTEM_LOCK:   
                des = "是否确认与系统解锁?";
                break;
            case  COMMAND_NAME.BYPASS_BUTTON:   
                des = "是否确认上位机旁路?";
                break;
            case  COMMAND_NAME.XBTB_UP_BUTTON:   
                des = "是否确认悬臂头部导料槽抬起（堆料）?";
                break;
            case  COMMAND_NAME.XBTB_DOWN_BUTTON:   
                des = "是否确认悬臂头部导料槽落下（取料）?";
                break;
            case  COMMAND_NAME.XBTB_STOP_BUTTON:   
                des = "是否确认悬臂头部导料槽停止?";
                break;
            case  COMMAND_NAME.VIBRATOR_START:   
                des = "是否确认振打器启动?";
                break;
            case  COMMAND_NAME.VIBRATOR_STOP:   
                des = "是否确认振打器启动?";
                break;
            case  COMMAND_NAME.SKRIT_TAKE_START:   
                des = "是否确认挡板取料变换启动?";
                break;
            case  COMMAND_NAME.SKRIT_STACK_START:   
                des = "是否确认挡板堆料变换启动?";
                break;
            case  COMMAND_NAME.SKRIT_TAKE_STOP:   
                des = "是否确认挡板分流变换停止?";
                break;
            case  COMMAND_NAME.LUFF_HART_START:   
                des = "是否确认变幅油加热器启动?";
                break;
            case  COMMAND_NAME.LUFF_HART_STOP:   
                des = "是否确认变幅油加热器停止?";
                break;
            case  COMMAND_NAME.LUFF_FAN_START:   
                des = "是否确认变幅风机启动?";
                break;
            case  COMMAND_NAME.LUFF_FAN_STOP:   
                des = "是否确认变幅风机停止?";
                break;
            default:
                break;
        }
        UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(des,null,(() =>
        {
            action?.Invoke();
            ConfirmSendMessageToServer(command);
        })));
    }

    public void ConfirmSendMessageToServer(COMMAND_NAME command)
    {
        string commandName=machine == Machine.BucketWheelStackerReclaimer ? command.ToString()+"_1" : command.ToString()+"_2";
        int dataInt = 0;
        Debug.Log($"message { machine }   {commandName}");
        switch (command)
        {
            case  COMMAND_NAME.RAIL_RELAX:
                disengageClampBtn.SetSelectState(true);
                engageClampBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("夹轨器放松", GameDataManager.Instance.GetUserName(), machine);
                    break;
            case  COMMAND_NAME.RAIL_CLAMP: 
                engageClampBtn.SetSelectState(true);
                disengageClampBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("夹轨器夹紧", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.SUPPLYPOWER_ON:   
                impetusSupplyCloseBrakeBtn.SetSelectState(true);
                impetusSupplyOpenBrakeBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("动力电源合闸", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.SUPPLYPOWER_OFF:   
                impetusSupplyOpenBrakeBtn.SetSelectState(true);
                impetusSupplyCloseBrakeBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("动力电源分闸", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.CONTROLPOWER_ON:   
                powerSupplyCloseBrakeBtn.SetSelectState(true);
                powerSupplyOpenBrakeBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("控制电源合闸", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.CONTROLPOWER_OFF:   
                powerSupplyOpenBrakeBtn.SetSelectState(true);
                powerSupplyCloseBrakeBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("控制电源分闸", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.BELT_TAKE:   
                cantileverTakeMaterStartBtn.SetSelectState(true);
                cantileverTakeMaterStopBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("悬臂皮带取料", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.BELT_STACK:   
                DataManager.Instance.InsertHistoryLogMc("悬臂皮带堆料", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.BELT_STOP:  
                cantileverTakeMaterStopBtn.SetSelectState(true);
                cantileverTakeMaterStartBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("悬臂皮带停止", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.BUCKET_START:  
                bucketWheelStartBtn.SetSelectState(true);
                bucketWheelStopBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("斗轮启动", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.BUCKET_STOP: 
                bucketWheelStopBtn.SetSelectState(true);
                bucketWheelStartBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("斗轮停止", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.LIGHTPOWER_ON:   
                lightCloseBrakeBtn.SetSelectState(true);
                lightOpenBrakeBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("照明合闸", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.LIGHTPOWER_OFF:   
                lightOpenBrakeBtn.SetSelectState(true);
                lightCloseBrakeBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("照明分闸", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.OILBUMP_ON:   
                oilPumpStartBtn.SetSelectState(true);
                oilPumpStopBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("主车油泵启动", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.OILBUMP_OFF:   
                oilPumpStopBtn.SetSelectState(true);
                oilPumpStartBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("主车油泵关闭", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.BELTTAKE_BUTTON:   
                DataManager.Instance.InsertHistoryLogMc("取料开关", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.BELTSTACK_BUTTON:   
                DataManager.Instance.InsertHistoryLogMc("堆料开关", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.BELTSSTOP_BUTTON:   
                DataManager.Instance.InsertHistoryLogMc("堆取料停止开关", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.SYSTEM_UNLOCK:   
                systemUnlockBtn.SetSelectState(true);
                systemLockBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("与系统解锁", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.SYSTEM_LOCK:   
                systemLockBtn.SetSelectState(true);
                systemUnlockBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("与系统连锁", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.BYPASS_BUTTON:   
                bypassBtn.SetSelectState(!bypassBtn.select.activeSelf);
                dataInt = bypassBtn.red.activeSelf ? 0 : 1;
                DataManager.Instance.InsertHistoryLogMc("上位机旁路", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.XBTB_UP_BUTTON:   
                DataManager.Instance.InsertHistoryLogMc("悬臂头部导料槽抬起（堆料）", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.XBTB_DOWN_BUTTON:   
                takeMaterDownBtn.SetSelectState(true);
                takeMaterStopBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("悬臂头部导料槽落下（取料）", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.XBTB_STOP_BUTTON:   
                takeMaterStopBtn.SetSelectState(true);
                takeMaterDownBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("悬臂头部导料槽停止", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.VIBRATOR_START:   
                shakerStartBtn.SetSelectState(true);
                shakerStopBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("振打器启动", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.VIBRATOR_STOP:   
                shakerStopBtn.SetSelectState(true);
                shakerStartBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("振打器停止", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.SKRIT_TAKE_START:   
                DataManager.Instance.InsertHistoryLogMc("挡板取料变换启动", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.SKRIT_STACK_START:   
                DataManager.Instance.InsertHistoryLogMc("挡板堆料变换启动", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.SKRIT_TAKE_STOP:   
                DataManager.Instance.InsertHistoryLogMc("挡板分流变换停止", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.LUFF_HART_START:   
                heaterStartBtn.SetSelectState(true);
                heaterStopBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("变幅油加热器启动", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.LUFF_HART_STOP:   
                heaterStartBtn.SetSelectState(false);
                heaterStopBtn.SetSelectState(true);
                DataManager.Instance.InsertHistoryLogMc("变幅油加热器停止", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.LUFF_FAN_START:   
                draughtFanStartBtn.SetSelectState(true);
                draughtFanStopBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("变幅风机启动", GameDataManager.Instance.GetUserName(), machine);
                break;
            case  COMMAND_NAME.LUFF_FAN_STOP:   
                draughtFanStartBtn.SetSelectState(false);
                draughtFanStopBtn.SetSelectState(true);
                DataManager.Instance.InsertHistoryLogMc("变幅风机停止", GameDataManager.Instance.GetUserName(), machine);
                break;
            default:
                break;
        }
        GameDataManager.Instance.SendServerCommandByName(commandName,dataInt);
    }
}