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
    /// 振打电机运行-
    /// </summary>
    public bool isVibrationMotorRunning;
}
/// <summary>
/// 振打电机
/// </summary>
public class VibratorMotorItem : StatusParmItemBase<VibratorMotorData>
{
    /// <summary>
    /// 振打电机主断路器
    /// </summary>
    public Toggle VibrationMotorMainCircuitBreaker;
    /// <summary>
    /// 振打电机过载
    /// </summary>
    public Toggle VibrationMotorOverload;
    /// <summary>
    /// 振打电机运行-
    /// </summary>
    public Toggle VibrationMotorRunning;
    public override void UpdateData(VibratorMotorData data)
    {
        SetToggleState(VibrationMotorMainCircuitBreaker, data.isVibrationMotorMainCircuitBreaker);
        SetToggleState(VibrationMotorOverload, data.isVibrationMotorOverload);
        SetToggleState(VibrationMotorRunning, data.isVibrationMotorRunning);
    }
}
