using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct SuspendedColloidData
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
    /// 制动器打开
    /// </summary>
    public bool isBrakeOpen;
    /// <summary>
    /// 制动器松闸限位
    /// </summary>
    public bool isBrakeReleaseLimit;
    /// <summary>
    /// 堆料运行
    /// </summary>
    public bool isStackingOperation;
    /// <summary>
    /// 取料运行
    /// </summary>
    public bool isFetchingOperation;
    /// <summary>
    /// 一级跑偏开关
    /// </summary>
    public bool isFirstLevelDeviationSwitch;
    /// <summary>
    /// 二级跑偏开关
    /// </summary>
    public bool isSecondLevelDeviationSwitch;
    /// <summary>
    /// 打滑检测开关
    /// </summary>
    public bool isSlippingDetectionSwitch;
    /// <summary>
    /// 纵向撕裂开关
    /// </summary>
    public bool isLongitudinalTearSwitch;
    /// <summary>
    /// 急停拉线开关
    /// </summary>
    public bool isEmergencyStopCableSwitch;
    /// <summary>
    /// 料流检测开关
    /// </summary>
    public bool isMaterialFlowDetectionSwitch;
    /// <summary>
    /// 中部料斗堵煤
    /// </summary>
    public bool isMiddleHopperCoalBlocking;
}
/// <summary>
/// 悬胶
/// </summary>
public class SuspendedColloidItem : StatusParmItemBase<SuspendedColloidData>
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
    /// 制动器打开
    /// </summary>
    public Toggle BrakeOpen;
    /// <summary>
    /// 制动器松闸限位
    /// </summary>
    public Toggle BrakeReleaseLimit;
    /// <summary>
    /// 堆料运行
    /// </summary>
    public Toggle StackingOperation;
    /// <summary>
    /// 取料运行
    /// </summary>
    public Toggle FetchingOperation;
    /// <summary>
    /// 一级跑偏开关
    /// </summary>
    public Toggle FirstLevelDeviationSwitch;
    /// <summary>
    /// 二级跑偏开关
    /// </summary>
    public Toggle SecondLevelDeviationSwitch;
    /// <summary>
    /// 打滑检测开关
    /// </summary>
    public Toggle SlippingDetectionSwitch;
    /// <summary>
    /// 纵向撕裂开关
    /// </summary>
    public Toggle LongitudinalTearSwitch;
    /// <summary>
    /// 急停拉线开关
    /// </summary>
    public Toggle EmergencyStopCableSwitch;
    /// <summary>
    /// 料流检测开关
    /// </summary>
    public Toggle MaterialFlowDetectionSwitch;
    /// <summary>
    /// 中部料斗堵煤
    /// </summary>
    public Toggle MiddleHopperCoalBlocking;
    public override void UpdateData(SuspendedColloidData data)
    {
        SetToggleState(MainCircuitBreaker, data.isMainCircuitBreaker);
        SetToggleState(MotorOverload, data.isMotorOverload);
        SetToggleState(BrakeOpen, data.isBrakeOpen);
        SetToggleState(BrakeReleaseLimit, data.isBrakeReleaseLimit);
        SetToggleState(StackingOperation, data.isStackingOperation);
        SetToggleState(FetchingOperation, data.isFetchingOperation);
        SetToggleState(FirstLevelDeviationSwitch, data.isFirstLevelDeviationSwitch);
        SetToggleState(SecondLevelDeviationSwitch, data.isSecondLevelDeviationSwitch);
        SetToggleState(SlippingDetectionSwitch, data.isSlippingDetectionSwitch);
        SetToggleState(LongitudinalTearSwitch, data.isLongitudinalTearSwitch);
        SetToggleState(EmergencyStopCableSwitch, data.isEmergencyStopCableSwitch);
        SetToggleState(MaterialFlowDetectionSwitch, data.isMaterialFlowDetectionSwitch);
        SetToggleState(MiddleHopperCoalBlocking, data.isMiddleHopperCoalBlocking);
    }
}
