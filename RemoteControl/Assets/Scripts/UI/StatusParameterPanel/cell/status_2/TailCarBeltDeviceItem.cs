using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct TailCarBeltDeviceData
{
    /// <summary>
    /// 尾车从动滚筒轴承上限报警
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
    public Toggle BearingUpperLimitAlarm;
    /// <summary>
    /// 尾车从动滚筒轴承下限报警
    /// </summary>
    public Toggle BearingLowerLimitAlarm;
    /// <summary>
    /// 尾车胶带一级跑偏
    /// </summary>
    public Toggle LevelOneDeviation;
    /// <summary>
    /// 尾车胶带二级跑偏
    /// </summary>
    public Toggle LevelTwoDeviation;
    /// <summary>
    /// 尾车急停拉线开关
    /// </summary>
    public Toggle EmergencyStopCableSwitch;
    /// <summary>
    /// 尾车胶带纵向撕裂开关
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
