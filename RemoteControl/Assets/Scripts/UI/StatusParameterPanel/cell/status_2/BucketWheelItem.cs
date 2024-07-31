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
    /// 斗轮过力矩开关
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
    public Toggle MainCircuitBreaker;
    /// <summary>
    /// 斗轮运行
    /// </summary>
    public Toggle BucketWheelRunning;
    /// <summary>
    /// 润滑油泵运行
    /// </summary>
    public Toggle LubricationPumpRunning;
    /// <summary>
    /// 电机过载
    /// </summary>
    public Toggle MotorOverload;
    /// <summary>
    /// 斗轮过力矩开关
    /// </summary>
    public Toggle BucketWheelOverTorqueSwitch;
    /// <summary>
    /// 润滑油泵流量开关
    /// </summary>
    public Toggle LubricatingOilPumpFlowSwitch;
    public override void UpdateData(BucketWheelData data)
    {
        SetToggleState(MainCircuitBreaker,data.isMainCircuitBreaker);
        SetToggleState(BucketWheelRunning, data.isBucketWheelRunning);
        SetToggleState(LubricationPumpRunning, data.isLubricationPumpRunning);
        SetToggleState(MotorOverload, data.isMotorOverload);
        SetToggleState(BucketWheelOverTorqueSwitch, data.isBucketWheelOverTorqueSwitch);
        SetToggleState(LubricatingOilPumpFlowSwitch, data.isLubricatingOilPumpFlowSwitch);
    }
}
