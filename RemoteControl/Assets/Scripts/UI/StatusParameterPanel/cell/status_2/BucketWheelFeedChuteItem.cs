using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct BucketWheelFeedChuteData
{
    /// <summary>
    /// 主断路器 
    /// </summary>
    public bool isMainCircuitBreaker;
    /// <summary>
    /// 电机过载 
    /// </summary>
    public bool isMotorOverload;
    /// <summary>
    /// 抬起限位 
    /// </summary>
    public bool isLiftLimit;
    /// <summary>
    ///  落下限位 
    /// </summary>
    public bool isLowerLimit;
    /// <summary>
    /// 抬起运行 
    /// </summary>
    public bool isLiftRunning;
    /// <summary>
    /// 落下运行
    /// </summary>
    public bool isLowerRunning;
}
/// <summary>
/// 斗轮导料槽
/// </summary>
public class BucketWheelFeedChuteItem : StatusParmItemBase<BucketWheelFeedChuteData>
{
    /// <summary>
    /// 主断路器 
    /// </summary>
    public Toggle MainCircuitBreaker;
    /// <summary>
    /// 电机过载 
    /// </summary>
    public Toggle MotorOverload;
    /// <summary>
    /// 抬起限位 
    /// </summary>
    public Toggle LiftLimit;
    /// <summary>
    ///  落下限位 
    /// </summary>
    public Toggle LowerLimit;
    /// <summary>
    /// 抬起运行 
    /// </summary>
    public Toggle LiftRunning;
    /// <summary>
    /// 落下运行
    /// </summary>
    public Toggle LowerRunning;
    public override void UpdateData(BucketWheelFeedChuteData data)
    {
        SetToggleState(MainCircuitBreaker, data.isMainCircuitBreaker);
        SetToggleState(MotorOverload, data.isMotorOverload);
        SetToggleState(LiftLimit, data.isLiftLimit);
        SetToggleState(LowerLimit, data.isLowerLimit);
        SetToggleState(LiftRunning, data.isLiftRunning);
        SetToggleState(LowerRunning, data.isLowerRunning);
    }
}
