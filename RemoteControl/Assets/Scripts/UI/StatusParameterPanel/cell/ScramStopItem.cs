using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct ScramStopData
{
    /// <summary>
    /// �����Ҽ�ͣ
    /// </summary>
    public bool isElectricalRoomEmergency;
    /// <summary>
    /// ˾���Ҽ�ͣ
    /// </summary>
    public bool isDriverRoomEmergency;
    /// <summary>
    /// ��ͣ�̵���
    /// </summary>
    public bool isEmergencyStopRelay;
}
/// <summary>
/// ��ͣ��Ϣ
/// </summary>
public class ScramStopItem : StatusParmItemBase<ScramStopData>
{
    /// <summary>
    /// �����Ҽ�ͣ
    /// </summary>
    public Toggle ElectricalRoomEmergencyToggle;
    /// <summary>
    /// ˾���Ҽ�ͣ
    /// </summary>
    public Toggle DriverRoomEmergencyToggle;
    /// <summary>
    /// ��ͣ�̵���
    /// </summary>
    public Toggle EmergencyStopRelayToggle;
    public override void UpdateData(ScramStopData scramStopData)
    {
        SetToggleState(ElectricalRoomEmergencyToggle, scramStopData.isElectricalRoomEmergency);
        SetToggleState(DriverRoomEmergencyToggle, scramStopData.isDriverRoomEmergency);
        SetToggleState(EmergencyStopRelayToggle, scramStopData.isEmergencyStopRelay);
    }

}
