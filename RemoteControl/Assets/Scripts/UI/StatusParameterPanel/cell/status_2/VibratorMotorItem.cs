using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct VibratorMotorData
{
    /// <summary>
    /// 振打电机主断路器
    /// </summary>
    public bool isVibrationMotorMainCircuitBreaker;
    /// <summary>
    /// 振打电机过载
    /// </summary>
    public bool isVibrationMotorOverload;
    /// <summary>
    /// 振打电机运行
    /// </summary>
    public bool isVibrationMotorRunning;
    /// <summary>
    /// 振打器故障
    /// </summary>
    public bool isVibrationMotorFault;
}
/// <summary>
/// 振打电机
/// </summary>
public class VibratorMotorItem : StatusParmItemBase<VibratorMotorData>
{
    /// <summary>
    ///振打电机主断路器
    /// </summary>
    public ToggleDIY VibrationMotorMainCircuitBreaker;
    /// <summary>
    /// 振打电机过载
    /// </summary>
    public ToggleDIY VibrationMotorOverload;
    /// <summary>
    /// 振打电机运行
    /// </summary>
    public ToggleDIY VibrationMotorRunning;
    public ToggleDIY VibrationMotorFault;
    public override void UpdateData(VibratorMotorData data,bool isConnect=false)
    {
        SetToggleState(VibrationMotorMainCircuitBreaker, data.isVibrationMotorMainCircuitBreaker,false,isConnect);
        SetToggleState(VibrationMotorOverload, data.isVibrationMotorOverload,true,isConnect);
        SetToggleState(VibrationMotorRunning, data.isVibrationMotorRunning,false,isConnect);
        SetToggleState(VibrationMotorFault, data.isVibrationMotorFault,true,isConnect);
    }
}
