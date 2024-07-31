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
    public Toggle MainCircuitBreakerToggle;
    /// <summary>
    /// 电机断路器
    /// </summary>
    public Toggle MotorCircuitBreakerToggle;
    /// <summary>
    /// 制动器断路器
    /// </summary>
    public Toggle BrakeCircuitBreakersToggle;
    /// <summary>
    /// 变频器运行
    /// </summary>
    public Toggle FrequencyConverterOperationToggle;
    /// <summary>
    /// 制动器运行
    /// </summary>
    public Toggle BrakeOperationToggle;
    /// <summary>
    /// 前进运行
    /// </summary>
    public Toggle ForwardOperationToggle;
    /// <summary>
    /// 后退运行
    /// </summary>
    public Toggle ReverseOperationToggle;

    /// <summary>
    /// 大车故障
    /// </summary>
    public Toggle CraneFaultToggle;
    /// <summary>
    /// 变频器故障
    /// </summary>
    public Toggle FrequencyConverterFaultToggle;
    /// <summary>
    /// 制动器故障
    /// </summary>
    public Toggle BrakeFaultToggle;
    /// <summary>
    /// 大车集中润滑低油位：
    /// </summary>
    public Toggle LowOilLevelInBigVehicleCentralLubricationToggle;
    /// <summary>
    /// 大车集中润滑堵油
    /// </summary>
    public Toggle BlockedOilInBigVehicleCentralLubricationToggle;

    /// <summary>
    /// 前进限位
    /// </summary>
    public Toggle ForwardLimitToggle;
    /// <summary>
    /// 前进极限
    /// </summary>
    public Toggle ForwardLimitExceedToggle;
    /// <summary>
    /// 后退限位
    /// </summary>
    public Toggle ReverseLimitToggle;
    /// <summary>
    /// 后退极限
    /// </summary>
    public Toggle ReverseLimitExceedToggle;

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
