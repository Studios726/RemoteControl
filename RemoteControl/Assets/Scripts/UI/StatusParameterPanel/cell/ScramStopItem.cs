using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct ScramStopData
{
    /// <summary>
    /// 电气室急停
    /// </summary>
    public bool isElectricalRoomEmergency;
    /// <summary>
    /// 司机室急停
    /// </summary>
    public bool isDriverRoomEmergency;
    /// <summary>
    /// 急停继电器
    /// </summary>
    public bool isEmergencyStopRelay;
}
/// <summary>
/// 急停信息
/// </summary>
public class ScramStopItem : StatusParmItemBase<ScramStopData>
{
    /// <summary>
    /// 电气室急停
    /// </summary>
    public ToggleDIY ElectricalRoomEmergencyToggle;
    /// <summary>
    /// 司机室急停
    /// </summary>
    public ToggleDIY DriverRoomEmergencyToggle;
    /// <summary>
    /// 急停继电器
    /// </summary>
    public ToggleDIY EmergencyStopRelayToggle;
    public override void UpdateData(ScramStopData scramStopData,bool isConnect=false)
    {
      
        SetToggleState(ElectricalRoomEmergencyToggle, scramStopData.isElectricalRoomEmergency,false,isConnect);
        SetToggleState(DriverRoomEmergencyToggle, scramStopData.isDriverRoomEmergency,false,isConnect);
        SetToggleState(EmergencyStopRelayToggle, scramStopData.isEmergencyStopRelay,true,isConnect);
    }

}
