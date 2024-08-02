using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct CableWindingDeviceData
{
    /// <summary>
    /// 主断路器 
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
    /// 卷筒电机过载 
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
    public Toggle MainCircuitBreaker;
    /// <summary>
    /// 卷筒过紧限位1 
    /// </summary>
    public Toggle ReelOverTightLimit1;
    /// <summary>
    /// 卷筒过松限位1 
    /// </summary>
    public Toggle ReelOverLooseLimit1;
    /// <summary>
    /// 卷筒空盘开关
    /// </summary>
    public Toggle ReelEmptyDiskSwitch;
    /// <summary>
    ///  卷筒中闸开关 
    /// </summary>
    public Toggle ReelMiddleBrakeSwitch;
    /// <summary>
    /// 卷筒电机过载 
    /// </summary>
    public Toggle ReelMotorOverload;
    /// <summary>
    /// 卷筒过紧限位2 
    /// </summary>
    public Toggle ReelOverTightLimit2;
    /// <summary>
    /// 卷筒过松限位2 
    /// </summary>
    public Toggle ReelOverLooseLimit2;
    /// <summary>
    /// 卷筒满盘开关 
    /// </summary>
    public Toggle ReelFullDiskSwitch;
    /// <summary>
    /// 动力卷筒运行
    /// </summary>
    public Toggle PowerReelRunning;
    public override void UpdateData(CableWindingDeviceData data)
    {
        SetToggleState(MainCircuitBreaker, data.isMainCircuitBreaker);
        SetToggleState(ReelOverTightLimit1, data.isReelOverTightLimit1);
        SetToggleState(ReelOverLooseLimit1, data.isReelOverLooseLimit1);
        SetToggleState(ReelEmptyDiskSwitch, data.isReelEmptyDiskSwitch);
        SetToggleState(ReelMiddleBrakeSwitch, data.isReelMiddleBrakeSwitch);
        SetToggleState(ReelMotorOverload, data.isReelMotorOverload);
        SetToggleState(ReelOverTightLimit2, data.isReelOverTightLimit2);
        SetToggleState(ReelOverLooseLimit2, data.isReelOverLooseLimit2);
        SetToggleState(ReelFullDiskSwitch, data.isReelFullDiskSwitch);
        SetToggleState(PowerReelRunning, data.isPowerReelRunning);
    }
}
