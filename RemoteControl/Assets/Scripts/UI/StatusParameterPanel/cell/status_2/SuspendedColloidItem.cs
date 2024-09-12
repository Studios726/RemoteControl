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
    public ToggleDIY MainCircuitBreaker;
    /// <summary>
    /// 电机过载
    /// </summary>
    public ToggleDIY MotorOverload;
    /// <summary>
    /// 制动器打开
    /// </summary>
    public ToggleDIY BrakeOpen;
    /// <summary>
    /// 制动器松闸限位
    /// </summary>
    public ToggleDIY BrakeReleaseLimit;
    /// <summary>
    /// 堆料运行
    /// </summary>
    public ToggleDIY StackingOperation;
    /// <summary>
    /// 取料运行
    /// </summary>
    public ToggleDIY FetchingOperation;
    /// <summary>
    /// 一级跑偏开关
    /// </summary>
    public ToggleDIY FirstLevelDeviationSwitch;
    /// <summary>
    /// 二级跑偏开关
    /// </summary>
    public ToggleDIY SecondLevelDeviationSwitch;
    /// <summary>
    /// 打滑检测开关
    /// </summary>
    public ToggleDIY SlippingDetectionSwitch;
    /// <summary>
    /// 纵向撕裂开关
    /// </summary>
    public ToggleDIY LongitudinalTearSwitch;
    /// <summary>
    /// 急停拉线开关
    /// </summary>
    public ToggleDIY EmergencyStopCableSwitch;
    /// <summary>
    /// 料流检测开关
    /// </summary>
    public ToggleDIY MaterialFlowDetectionSwitch;
    /// <summary>
    /// 中部料斗堵煤
    /// </summary>
    public ToggleDIY MiddleHopperCoalBlocking;
    public override void UpdateData(SuspendedColloidData data,bool isConnect=false)
    {
        SetToggleState(MainCircuitBreaker, data.isMainCircuitBreaker,false,isConnect);
        SetToggleState(MotorOverload, data.isMotorOverload,true,isConnect);
        SetToggleState(BrakeOpen, data.isBrakeOpen,false,isConnect);
        SetToggleState(BrakeReleaseLimit, data.isBrakeReleaseLimit,false,isConnect);
        SetToggleState(StackingOperation, data.isStackingOperation,false,isConnect);
        SetToggleState(FetchingOperation, data.isFetchingOperation,false,isConnect);
        SetToggleState(FirstLevelDeviationSwitch, data.isFirstLevelDeviationSwitch,true,isConnect);
        SetToggleState(SecondLevelDeviationSwitch, data.isSecondLevelDeviationSwitch,true,isConnect);
        SetToggleState(SlippingDetectionSwitch, data.isSlippingDetectionSwitch,true,isConnect);
        SetToggleState(LongitudinalTearSwitch, data.isLongitudinalTearSwitch,true,isConnect);
        SetToggleState(EmergencyStopCableSwitch, data.isEmergencyStopCableSwitch,true,isConnect);
        SetToggleState(MaterialFlowDetectionSwitch, data.isMaterialFlowDetectionSwitch,true,isConnect);
        SetToggleState(MiddleHopperCoalBlocking, data.isMiddleHopperCoalBlocking,true,isConnect);
    }
}
