using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct TailCarBeltDeviceData
{
    /// <summary>
    /// β���Ӷ���Ͳ������ޱ���
    /// </summary>
    public bool isBearingUpperLimitAlarm;
    /// <summary>
    /// β���Ӷ���Ͳ������ޱ���
    /// </summary>
    public bool isBearingLowerLimitAlarm;
    /// <summary>
    /// β������һ����ƫ
    /// </summary>
    public bool isLevelOneDeviation;
    /// <summary>
    /// β������������ƫ
    /// </summary>
    public bool isLevelTwoDeviation;
    /// <summary>
    /// β����ͣ���߿���
    /// </summary>
    public bool isEmergencyStopCableSwitch;
    /// <summary>
    /// β����������˺�ѿ���
    /// </summary>
    public bool isLongitudinalTearSwitch;
}
/// <summary>
/// β������
/// </summary>
public class TailCarBeltDeviceItem : StatusParmItemBase<TailCarBeltDeviceData>
{
    /// <summary>
    /// β���Ӷ���Ͳ������ޱ���
    /// </summary>
    public Toggle BearingUpperLimitAlarm;
    /// <summary>
    /// β���Ӷ���Ͳ������ޱ���
    /// </summary>
    public Toggle BearingLowerLimitAlarm;
    /// <summary>
    /// β������һ����ƫ
    /// </summary>
    public Toggle LevelOneDeviation;
    /// <summary>
    /// β������������ƫ
    /// </summary>
    public Toggle LevelTwoDeviation;
    /// <summary>
    /// β����ͣ���߿���
    /// </summary>
    public Toggle EmergencyStopCableSwitch;
    /// <summary>
    /// β����������˺�ѿ���
    /// </summary>
    public Toggle LongitudinalTearSwitch;
    public override void UpdateData(TailCarBeltDeviceData data)
    {
        SetToggleState(BearingUpperLimitAlarm,data.isBearingUpperLimitAlarm);
        SetToggleState(BearingLowerLimitAlarm, data.isBearingLowerLimitAlarm);
        SetToggleState(LevelOneDeviation, data.isLevelOneDeviation);
        SetToggleState(LevelTwoDeviation, data.isLevelTwoDeviation);
        SetToggleState(EmergencyStopCableSwitch, data.isEmergencyStopCableSwitch);
        SetToggleState(LongitudinalTearSwitch, data.isLongitudinalTearSwitch);

    }

}
