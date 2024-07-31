using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct SwitchOnData
{
    /// <summary>
    /// 真空断路器合闸
    /// </summary>
    public bool isVacuumCircuitBreakerClosedToggle;
    /// <summary>
    /// 低压控制电源
    /// </summary>
    public bool isLowVoltageControlPowerClosedToggle;
    /// <summary>
    /// 低压动力电源
    /// </summary>
    public bool isLowVoltagePowerClosedToggle;
}
/// <summary>
/// 合闸信息
/// </summary>
public class SwitchOnItem : StatusParmItemBase<SwitchOnData>
{
    /// <summary>
    /// 真空断路器合闸
    /// </summary>
    public Toggle VacuumCircuitBreakerClosedToggle;
    /// <summary>
    /// 低压控制电源
    /// </summary>
    public Toggle LowVoltageControlPowerClosedToggle;   
    /// <summary>
    /// 低压动力电源
    /// </summary>
    public Toggle LowVoltagePowerClosedToggle;
    public override void UpdateData(SwitchOnData switchOnData)
    {
        SetToggleState(VacuumCircuitBreakerClosedToggle, switchOnData.isVacuumCircuitBreakerClosedToggle);
        SetToggleState(LowVoltageControlPowerClosedToggle, switchOnData.isLowVoltageControlPowerClosedToggle);
        SetToggleState(LowVoltagePowerClosedToggle, switchOnData.isLowVoltagePowerClosedToggle);
    }
}
