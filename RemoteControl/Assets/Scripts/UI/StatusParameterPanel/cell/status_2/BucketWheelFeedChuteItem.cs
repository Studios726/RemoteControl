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
    public ToggleDIY MainCircuitBreaker;
    /// <summary>
    /// 电机过载 
    /// </summary>
    public ToggleDIY MotorOverload;
    /// <summary>
    /// 抬起限位 
    /// </summary>
    public ToggleDIY LiftLimit;
    /// <summary>
    ///  落下限位 
    /// </summary>
    public ToggleDIY LowerLimit;
    /// <summary>
    /// 抬起运行 
    /// </summary>
    public ToggleDIY LiftRunning;
    /// <summary>
    /// 落下运行
    /// </summary>
    public ToggleDIY LowerRunning;
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
