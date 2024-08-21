using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct RotaryMechanismData
{
    /// <summary>
    /// 主断路器
    /// </summary>
    public bool isMainCircuitBreaker;
    /// <summary>
    /// 变频器运行
    /// </summary>
    public bool isFrequencyConverterRunning;
    /// <summary>
    /// 制动器运行
    /// </summary>
    public bool isBrakeOperating;
    /// <summary>
    /// 风机运行
    /// </summary>
    public bool isFanOperating;
    /// <summary>
    /// 左转运行
    /// </summary>
    public bool isLeftTurnOperating;
    /// <summary>
    /// 右转运行
    /// </summary>
    public bool isRightTurnOperating;

    /// <summary>
    /// 变频器故障
    /// </summary>
    public bool isFrequencyConverterFault;
    /// <summary>
    /// 制动器过载
    /// </summary>
    public bool isBrakeOverload;
    /// <summary>
    /// 风机过载
    /// </summary>
    public bool isFanOverload;
    /// <summary>
    /// 制动电阻超温
    /// </summary>
    public bool isBrakingResistorOverheating;
    /// <summary>
    /// 回转故障
    /// </summary>
    public bool isRotaryFault;

    /// <summary>
    /// 左转限位
    /// </summary>
    public bool isLeftTurnLimit;
    /// <summary>
    /// 左转极限
    /// </summary>
    public bool isLeftTurnLimitExceed;
    /// <summary>
    /// 左转禁区限位
    /// </summary>
    public bool isLeftTurnRestrictedZoneLimit;
    /// <summary>
    /// 左转防撞限位
    /// </summary>
    public bool isLeftTurnCollisionPreventionLimit;
    /// <summary>
    /// 右转限位
    /// </summary>
    public bool isRightTurnLimit;
    /// <summary>
    /// 右转极限
    /// </summary>
    public bool isRightTurnLimitExceed;
    /// <summary>
    /// 右转禁区限位
    /// </summary>
    public bool isRightTurnRestrictedZoneLimit;
    /// <summary>
    /// 右转防撞限位
    /// </summary>
    public bool isRightTurnCollisionPreventionLimit;

    /// <summary>
    /// 回转过力矩
    /// </summary>
    public bool isRotaryOverTorque;
    /// <summary>
    /// 回转零位限位
    /// </summary>
    public bool isRotaryZeroPositionLimit;
    /// <summary>
    /// 制动器松闸限位
    /// </summary>
    public bool isBrakeReliefLimit;
    /// <summary>
    /// 回转集中润滑堵油
    /// </summary>
    public bool isRotaryCentralLubricationBlockedOil;
    /// <summary>
    /// 回转集中润滑低油位
    /// </summary>
    public bool isRotaryCentralLubricationLowOilLevel;
}
/// <summary>
/// 回转机构
/// </summary>
public class RotaryMechanismItem : StatusParmItemBase<RotaryMechanismData>
{
    /// <summary>
    /// 主断路器
    /// </summary>
    public ToggleDIY MainCircuitBreaker;
    /// <summary>
    /// 变频器运行
    /// </summary>
    public ToggleDIY FrequencyConverterRunning;
    /// <summary>
    /// 制动器运行
    /// </summary>
    public ToggleDIY BrakeOperating;
    /// <summary>
    /// 风机运行
    /// </summary>
    public ToggleDIY FanOperating;
    /// <summary>
    /// 左转运行
    /// </summary>
    public ToggleDIY LeftTurnOperating;
    /// <summary>
    /// 右转运行
    /// </summary>
    public ToggleDIY RightTurnOperating;

    /// <summary>
    /// 变频器故障
    /// </summary>
    public ToggleDIY FrequencyConverterFault;
    /// <summary>
    /// 制动器过载
    /// </summary>
    public ToggleDIY BrakeOverload;
    /// <summary>
    /// 风机过载
    /// </summary>
    public ToggleDIY FanOverload;
    /// <summary>
    /// 制动电阻超温
    /// </summary>
    public ToggleDIY BrakingResistorOverheating;
    /// <summary>
    /// 回转故障
    /// </summary>
    public ToggleDIY RotaryFault;

    /// <summary>
    /// 左转限位
    /// </summary>
    public ToggleDIY LeftTurnLimit;
    /// <summary>
    /// 左转极限
    /// </summary>
    public ToggleDIY LeftTurnLimitExceed;
    /// <summary>
    /// 左转禁区限位
    /// </summary>
    public ToggleDIY LeftTurnRestrictedZoneLimit;
    /// <summary>
    /// 左转防撞限位
    /// </summary>
    public ToggleDIY LeftTurnCollisionPreventionLimit;
    /// <summary>
    /// 右转限位
    /// </summary>
    public ToggleDIY RightTurnLimit;
    /// <summary>
    /// 右转极限
    /// </summary>
    public ToggleDIY RightTurnLimitExceed;
    /// <summary>
    /// 右转禁区限位
    /// </summary>
    public ToggleDIY RightTurnRestrictedZoneLimit;
    /// <summary>
    /// 右转防撞限位
    /// </summary>
    public ToggleDIY RightTurnCollisionPreventionLimit;

    /// <summary>
    /// 回转过力矩
    /// </summary>
    public ToggleDIY RotaryOverTorque;
    /// <summary>
    /// 回转零位限位
    /// </summary>
    public ToggleDIY RotaryZeroPositionLimit;
    /// <summary>
    /// 制动器松闸限位
    /// </summary>
    public ToggleDIY BrakeReliefLimit;
    /// <summary>
    /// 回转集中润滑堵油
    /// </summary>
    public ToggleDIY RotaryCentralLubricationBlockedOil;
    /// <summary>
    /// 回转集中润滑低油位
    /// </summary>
    public ToggleDIY RotaryCentralLubricationLowOilLevel;

    public override void UpdateData(RotaryMechanismData data,bool isConnect=false)
    {
        SetToggleState(MainCircuitBreaker, data.isMainCircuitBreaker,false,isConnect);
        SetToggleState(FrequencyConverterRunning, data.isFrequencyConverterRunning,false,isConnect);
        SetToggleState(BrakeOperating, data.isBrakeOperating,false,isConnect);
        SetToggleState(FanOperating, data.isFanOperating,false,isConnect);
        SetToggleState(LeftTurnOperating, data.isLeftTurnOperating,false,isConnect);
        SetToggleState(RightTurnOperating, data.isRightTurnOperating,false,isConnect);

        SetToggleState(FrequencyConverterFault, data.isFrequencyConverterFault,true,isConnect);
        SetToggleState(BrakeOverload, data.isBrakeOverload,true,isConnect);
        SetToggleState(FanOverload, data.isFanOverload,true,isConnect);
        SetToggleState(BrakingResistorOverheating, data.isBrakingResistorOverheating,true,isConnect);
        SetToggleState(RotaryFault, data.isRotaryFault,true,isConnect);

        SetToggleState(LeftTurnLimit, data.isLeftTurnLimit,true,isConnect);
        SetToggleState(LeftTurnLimitExceed, data.isLeftTurnLimitExceed,true,isConnect);
        SetToggleState(LeftTurnRestrictedZoneLimit, data.isLeftTurnRestrictedZoneLimit);
        SetToggleState(LeftTurnCollisionPreventionLimit, data.isLeftTurnCollisionPreventionLimit,true,isConnect);
        SetToggleState(RightTurnLimit, data.isRightTurnLimit,true,isConnect);
        SetToggleState(RightTurnLimitExceed, data.isRightTurnLimitExceed,true,isConnect);
        SetToggleState(RightTurnRestrictedZoneLimit, data.isRightTurnRestrictedZoneLimit,true,isConnect);
        SetToggleState(RightTurnCollisionPreventionLimit, data.isRightTurnCollisionPreventionLimit,true,isConnect);

        SetToggleState(RotaryOverTorque, data.isRotaryOverTorque,true,isConnect);
        SetToggleState(RotaryZeroPositionLimit, data.isRotaryZeroPositionLimit,true,isConnect);
        SetToggleState(BrakeReliefLimit, data.isBrakeReliefLimit,true,isConnect);
        SetToggleState(RotaryCentralLubricationBlockedOil, data.isRotaryCentralLubricationBlockedOil,true,isConnect);
        SetToggleState(RotaryCentralLubricationLowOilLevel, data.isRotaryCentralLubricationLowOilLevel,true,isConnect);
    }


}
