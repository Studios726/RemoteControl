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
    public ToggleDIY VacuumCircuitBreakerClosedToggle;
    /// <summary>
    /// 低压控制电源
    /// </summary>
    public ToggleDIY LowVoltageControlPowerClosedToggle;   
    /// <summary>
    /// 低压动力电源
    /// </summary>
    public ToggleDIY LowVoltagePowerClosedToggle;
    public override void UpdateData(SwitchOnData switchOnData,bool isConnect=false)
    {
        SetToggleState(VacuumCircuitBreakerClosedToggle, switchOnData.isVacuumCircuitBreakerClosedToggle,false,isConnect);
        SetToggleState(LowVoltageControlPowerClosedToggle, switchOnData.isLowVoltageControlPowerClosedToggle,false,isConnect);
        SetToggleState(LowVoltagePowerClosedToggle, switchOnData.isLowVoltagePowerClosedToggle,false,isConnect);
    }
}
