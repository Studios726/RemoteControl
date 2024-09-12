using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct TailCarBeltDeviceData
{
    /// <summary>
    ///尾车从动滚筒轴承上限报警
    /// </summary>
    public bool isBearingUpperLimitAlarm;
    /// <summary>
    /// 尾车从动滚筒轴承下限报警
    /// </summary>
    public bool isBearingLowerLimitAlarm;
    /// <summary>
    /// 尾车胶带一级跑偏
    /// </summary>
    public bool isLevelOneDeviation;
    /// <summary>
    /// 尾车胶带二级跑偏
    /// </summary>
    public bool isLevelTwoDeviation;
    /// <summary>
    /// 尾车急停拉线开关
    /// </summary>
    public bool isEmergencyStopCableSwitch;
    /// <summary>
    /// 尾车胶带纵向撕裂开关
    /// </summary>
    public bool isLongitudinalTearSwitch;
}
/// <summary>
/// 尾车胶带
/// </summary>
public class TailCarBeltDeviceItem : StatusParmItemBase<TailCarBeltDeviceData>
{
    /// <summary>
    /// 尾车从动滚筒轴承上限报警
    /// </summary>
    public ToggleDIY BearingUpperLimitAlarm;
    /// <summary>
    /// 尾车从动滚筒轴承下限报警
    /// </summary>
    public ToggleDIY BearingLowerLimitAlarm;
    /// <summary>
    /// 尾车胶带一级跑偏
    /// </summary>
    public ToggleDIY LevelOneDeviation;
    /// <summary>
    /// 尾车胶带二级跑偏
    /// </summary>
    public ToggleDIY LevelTwoDeviation;
    /// <summary>
    /// 尾车急停拉线开关
    /// </summary>
    public ToggleDIY EmergencyStopCableSwitch;
    /// <summary>
    /// 尾车胶带纵向撕裂开关
    /// </summary>
    public ToggleDIY LongitudinalTearSwitch;
    public override void UpdateData(TailCarBeltDeviceData data,bool isConnect=false)
    {
        SetToggleState(BearingUpperLimitAlarm,data.isBearingUpperLimitAlarm,true,isConnect);
        SetToggleState(BearingLowerLimitAlarm, data.isBearingLowerLimitAlarm,true,isConnect);
        SetToggleState(LevelOneDeviation, data.isLevelOneDeviation,true,isConnect);
        SetToggleState(LevelTwoDeviation, data.isLevelTwoDeviation,true,isConnect);
        SetToggleState(EmergencyStopCableSwitch, data.isEmergencyStopCableSwitch,true,isConnect);
        SetToggleState(LongitudinalTearSwitch, data.isLongitudinalTearSwitch,true,isConnect);

    }

}
