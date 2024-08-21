using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public struct BucketWheelData
{
    /// <summary>
    /// 主断路器
    /// </summary>
    public bool isMainCircuitBreaker;
    /// <summary>
    /// 斗轮运行
    /// </summary>
    public bool isBucketWheelRunning;
    /// <summary>
    /// 润滑油泵运行
    /// </summary>
    public bool isLubricationPumpRunning;
    /// <summary>
    /// 电机过载
    /// </summary>
    public bool isMotorOverload;
    /// <summary>
    ///斗轮过力矩开关
    /// </summary>
    public bool isBucketWheelOverTorqueSwitch;
    /// <summary>
    /// 润滑油泵流量开关
    /// </summary>
    public bool isLubricatingOilPumpFlowSwitch;
}
/// <summary>
/// 斗轮
/// </summary>
public class BucketWheelItem : StatusParmItemBase<BucketWheelData>
{
    /// <summary>
    /// 主断路器
    /// </summary>
    public ToggleDIY MainCircuitBreaker;
    /// <summary>
    /// 斗轮运行
    /// </summary>
    public ToggleDIY BucketWheelRunning;
    /// <summary>
    /// 润滑油泵运行
    /// </summary>
    public ToggleDIY LubricationPumpRunning;
    /// <summary>
    /// 电机过载
    /// </summary>
    public ToggleDIY MotorOverload;
    /// <summary>
    /// 斗轮过力矩开关
    /// </summary>
    public ToggleDIY BucketWheelOverTorqueSwitch;
    /// <summary>
    /// 润滑油泵流量开关
    /// </summary>
    public ToggleDIY LubricatingOilPumpFlowSwitch;
    public override void UpdateData(BucketWheelData data,bool isConnect=false)
    {
        SetToggleState(MainCircuitBreaker,data.isMainCircuitBreaker,false,isConnect);
        SetToggleState(BucketWheelRunning, data.isBucketWheelRunning,false,isConnect);
        SetToggleState(LubricationPumpRunning, data.isLubricationPumpRunning,false,isConnect);
        SetToggleState(MotorOverload, data.isMotorOverload,true,isConnect);
        SetToggleState(BucketWheelOverTorqueSwitch, data.isBucketWheelOverTorqueSwitch,false,isConnect);
        SetToggleState(LubricatingOilPumpFlowSwitch, data.isLubricatingOilPumpFlowSwitch,false,isConnect);
    }
}
