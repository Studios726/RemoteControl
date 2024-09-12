using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct CableWindingDeviceData
{
    /// <summary>
    ///主断路器
    /// </summary>
    public bool isMainCircuitBreaker;
    /// <summary>
    /// 卷筒过紧限位1
    /// </summary>
    public bool isReelOverTightLimit1;
    /// <summary>
    /// 卷筒过松限位1
    /// </summary>
    public bool isReelOverLooseLimit1;
    /// <summary>
    /// 卷筒空盘开关
    /// </summary>
    public bool isReelEmptyDiskSwitch;
    /// <summary>
    ///  卷筒中闸开关
    /// </summary>
    public bool isReelMiddleBrakeSwitch;
    /// <summary>
    ///  卷筒电机过载 
    /// </summary>
    public bool isReelMotorOverload;
    /// <summary>
    /// 卷筒过紧限位2
    /// </summary>
    public bool isReelOverTightLimit2;
    /// <summary>
    /// 卷筒过松限位2
    /// </summary>
    public bool isReelOverLooseLimit2;
    /// <summary>
    /// 卷筒满盘开关
    /// </summary>
    public bool isReelFullDiskSwitch;
    /// <summary>
    /// 动力卷筒运行
    /// </summary>
    public bool isPowerReelRunning;
}
/// <summary>
/// 电缆卷筒
/// </summary>
public class CableWindingDeviceItem : StatusParmItemBase<CableWindingDeviceData>
{
    /// <summary>
    /// 主断路器
    /// </summary>
    public ToggleDIY MainCircuitBreaker;
    /// <summary>
    /// 卷筒过紧限位1
    /// </summary>
    public ToggleDIY ReelOverTightLimit1;
    /// <summary>
    ///  卷筒过松限位1
    /// </summary>
    public ToggleDIY ReelOverLooseLimit1;
    /// <summary>
    /// 卷筒空盘开关
    /// </summary>
    public ToggleDIY ReelEmptyDiskSwitch;
    /// <summary>
    ///  卷筒中闸开关
    /// </summary>
    public ToggleDIY ReelMiddleBrakeSwitch;
    /// <summary>
    /// 卷筒电机过载 
    /// </summary>
    public ToggleDIY ReelMotorOverload;
    /// <summary>
    /// 卷筒过紧限位2
    /// </summary>
    public ToggleDIY ReelOverTightLimit2;
    /// <summary>
    /// 卷筒过松限位2
    /// </summary>
    public ToggleDIY ReelOverLooseLimit2;
    /// <summary>
    /// 卷筒满盘开关
    /// </summary>
    public ToggleDIY ReelFullDiskSwitch;
    /// <summary>
    /// 动力卷筒运行
    /// </summary>
    public ToggleDIY PowerReelRunning;
    public override void UpdateData(CableWindingDeviceData data,bool isConnect=false)
    {
        SetToggleState(MainCircuitBreaker, data.isMainCircuitBreaker,false,isConnect);
        SetToggleState(ReelOverTightLimit1, data.isReelOverTightLimit1,true,isConnect);
        SetToggleState(ReelOverLooseLimit1, data.isReelOverLooseLimit1,true,isConnect);
        SetToggleState(ReelEmptyDiskSwitch, data.isReelEmptyDiskSwitch,true,isConnect);
        SetToggleState(ReelMiddleBrakeSwitch, data.isReelMiddleBrakeSwitch,false,isConnect);
        SetToggleState(ReelMotorOverload, data.isReelMotorOverload,true,isConnect);
        SetToggleState(ReelOverTightLimit2, data.isReelOverTightLimit2,true,isConnect);
        SetToggleState(ReelOverLooseLimit2, data.isReelOverLooseLimit2,true,isConnect);
        SetToggleState(ReelFullDiskSwitch, data.isReelFullDiskSwitch,true,isConnect);
        SetToggleState(PowerReelRunning, data.isPowerReelRunning,false,isConnect);
    }
}
