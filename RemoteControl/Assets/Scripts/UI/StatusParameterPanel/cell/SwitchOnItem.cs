using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct SwitchOnData
{
    /// <summary>
    /// ��ն�·����բ
    /// </summary>
    public bool isVacuumCircuitBreakerClosedToggle;
    /// <summary>
    /// ��ѹ���Ƶ�Դ
    /// </summary>
    public bool isLowVoltageControlPowerClosedToggle;
    /// <summary>
    /// ��ѹ������Դ
    /// </summary>
    public bool isLowVoltagePowerClosedToggle;
}
/// <summary>
/// ��բ��Ϣ
/// </summary>
public class SwitchOnItem : StatusParmItemBase<SwitchOnData>
{
    /// <summary>
    /// ��ն�·����բ
    /// </summary>
    public Toggle VacuumCircuitBreakerClosedToggle;
    /// <summary>
    /// ��ѹ���Ƶ�Դ
    /// </summary>
    public Toggle LowVoltageControlPowerClosedToggle;   
    /// <summary>
    /// ��ѹ������Դ
    /// </summary>
    public Toggle LowVoltagePowerClosedToggle;
    public override void UpdateData(SwitchOnData switchOnData)
    {
        SetToggleState(VacuumCircuitBreakerClosedToggle, switchOnData.isVacuumCircuitBreakerClosedToggle);
        SetToggleState(LowVoltageControlPowerClosedToggle, switchOnData.isLowVoltageControlPowerClosedToggle);
        SetToggleState(LowVoltagePowerClosedToggle, switchOnData.isLowVoltagePowerClosedToggle);
    }
}
