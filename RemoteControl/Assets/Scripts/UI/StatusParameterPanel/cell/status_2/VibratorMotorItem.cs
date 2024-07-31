using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct VibratorMotorData
{
    /// <summary>
    /// ���������·��
    /// </summary>
    public bool isVibrationMotorMainCircuitBreaker;
    /// <summary>
    /// ���������
    /// </summary>
    public bool isVibrationMotorOverload;
    /// <summary>
    /// ���������-
    /// </summary>
    public bool isVibrationMotorRunning;
}
/// <summary>
/// �����
/// </summary>
public class VibratorMotorItem : StatusParmItemBase<VibratorMotorData>
{
    /// <summary>
    /// ���������·��
    /// </summary>
    public Toggle VibrationMotorMainCircuitBreaker;
    /// <summary>
    /// ���������
    /// </summary>
    public Toggle VibrationMotorOverload;
    /// <summary>
    /// ���������-
    /// </summary>
    public Toggle VibrationMotorRunning;
    public override void UpdateData(VibratorMotorData data)
    {
        SetToggleState(VibrationMotorMainCircuitBreaker, data.isVibrationMotorMainCircuitBreaker);
        SetToggleState(VibrationMotorOverload, data.isVibrationMotorOverload);
        SetToggleState(VibrationMotorRunning, data.isVibrationMotorRunning);
    }
}
