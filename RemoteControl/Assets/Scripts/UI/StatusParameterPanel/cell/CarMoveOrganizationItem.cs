using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct CarMoveOrganizationData
{
    /// <summary>
    /// 主断路器
    /// </summary>
    public bool isMainCircuitBreakerToggle;
    /// <summary>
    /// 电机断路器
    /// </summary>
    public bool isMotorCircuitBreakerToggle;
    /// <summary>
    /// 制动器断路器
    /// </summary>
    public bool isBrakeCircuitBreakersToggle;
    /// <summary>
    /// 变频器运行
    /// </summary>
    public bool isFrequencyConverterOperationToggle;
    /// <summary>
    /// 制动器运行
    /// </summary>
    public bool isBrakeOperationToggle;
    /// <summary>
    /// 前进运行
    /// </summary>
    public bool isForwardOperationToggle;
    /// <summary>
    /// 后退运行
    /// </summary>
    public bool isReverseOperationToggle;

    /// <summary>
    /// 大车故障
    /// </summary>
    public bool isCraneFaultToggle;
    /// <summary>
    /// 变频器故障
    /// </summary>
    public bool isFrequencyConverterFaultToggle;
    /// <summary>
    /// 制动器故障
    /// </summary>
    public bool isBrakeFaultToggle;
    /// <summary>
    /// 大车集中润滑低油位：
    /// </summary>
    public bool isLowOilLevelInBigVehicleCentralLubricationToggle;
    /// <summary>
    /// 大车集中润滑堵油
    /// </summary>
    public bool isBlockedOilInBigVehicleCentralLubricationToggle;

    /// <summary>
    /// 前进限位
    /// </summary>
    public bool isForwardLimitToggle;
    /// <summary>
    /// 前进极限
    /// </summary>
    public bool isForwardLimitExceedToggle;
    /// <summary>
    /// 后退限位
    /// </summary>
    public bool isReverseLimitToggle;
    /// <summary>
    /// 后退极限
    /// </summary>
    public bool isReverseLimitExceedToggle;
}
/// <summary>
/// 大车行走机构
/// </summary>
public class CarMoveOrganizationItem : StatusParmItemBase<CarMoveOrganizationData>
{
    /// <summary>
    /// 主断路器
    /// </summary>
    public ToggleDIY MainCircuitBreakerToggle;
    /// <summary>
    /// 电机断路器
    /// </summary>
    public ToggleDIY MotorCircuitBreakerToggle;
    /// <summary>
    /// 制动器断路器
    /// </summary>
    public ToggleDIY BrakeCircuitBreakersToggle;
    /// <summary>
    /// 变频器运行
    /// </summary>
    public ToggleDIY FrequencyConverterOperationToggle;
    /// <summary>
    /// 制动器运行
    /// </summary>
    public ToggleDIY BrakeOperationToggle;
    /// <summary>
    /// 前进运行
    /// </summary>
    public ToggleDIY ForwardOperationToggle;
    /// <summary>
    /// 后退运行
    /// </summary>
    public ToggleDIY ReverseOperationToggle;

    /// <summary>
    /// 大车故障
    /// </summary>
    public ToggleDIY CraneFaultToggle;
    /// <summary>
    /// 变频器故障
    /// </summary>
    public ToggleDIY FrequencyConverterFaultToggle;
    /// <summary>
    /// 制动器故障
    /// </summary>
    public ToggleDIY BrakeFaultToggle;
    /// <summary>
    /// 大车集中润滑低油位：
    /// </summary>
    public ToggleDIY LowOilLevelInBigVehicleCentralLubricationToggle;
    /// <summary>
    /// 大车集中润滑堵油
    /// </summary>
    public ToggleDIY BlockedOilInBigVehicleCentralLubricationToggle;

    /// <summary>
    /// 前进限位
    /// </summary>
    public ToggleDIY ForwardLimitToggle;
    /// <summary>
    /// 前进极限
    /// </summary>
    public ToggleDIY ForwardLimitExceedToggle;
    /// <summary>
    /// 后退限位
    /// </summary>
    public ToggleDIY ReverseLimitToggle;
    /// <summary>
    /// 后退极限
    /// </summary>
    public ToggleDIY ReverseLimitExceedToggle;

    public override void UpdateData(CarMoveOrganizationData data)
    {
        SetToggleState(MainCircuitBreakerToggle,data.isMainCircuitBreakerToggle);
        SetToggleState(MotorCircuitBreakerToggle, data.isMotorCircuitBreakerToggle);
        SetToggleState(BrakeCircuitBreakersToggle, data.isBrakeCircuitBreakersToggle);
        SetToggleState(FrequencyConverterOperationToggle, data.isFrequencyConverterOperationToggle);
        SetToggleState(BrakeOperationToggle, data.isBrakeOperationToggle);
        SetToggleState(ForwardOperationToggle, data.isForwardOperationToggle);
        SetToggleState(ReverseOperationToggle, data.isReverseOperationToggle);

        SetToggleState(CraneFaultToggle, data.isCraneFaultToggle);
        SetToggleState(FrequencyConverterFaultToggle, data.isFrequencyConverterFaultToggle);
        SetToggleState(BrakeFaultToggle, data.isBrakeFaultToggle);
        SetToggleState(LowOilLevelInBigVehicleCentralLubricationToggle, data.isLowOilLevelInBigVehicleCentralLubricationToggle);
        SetToggleState(BlockedOilInBigVehicleCentralLubricationToggle, data.isBlockedOilInBigVehicleCentralLubricationToggle);

        SetToggleState(ForwardLimitToggle, data.isForwardLimitToggle);
        SetToggleState(ForwardLimitExceedToggle, data.isForwardLimitExceedToggle);
        SetToggleState(ReverseLimitToggle, data.isReverseLimitToggle);
        SetToggleState(ReverseLimitExceedToggle, data.isReverseLimitExceedToggle);

    }

}
