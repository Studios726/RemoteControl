using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct RailGrippingDeviceData
{
    /// <summary>
    ///主断路器
    /// </summary>
    public bool isMainCircuitBreaker;
    /// <summary>
    ///  左夹轨器电机运行
    /// </summary>
    public bool isLeftRailClamperMotorRunning;
    /// <summary>
    /// 左夹轨器电磁阀
    /// </summary>
    public bool isLeftRailClamperSolenoidValve;
    /// <summary>
    /// 左侧锚碇限位
    /// </summary>
    public bool isLeftAnchorLimit;
    /// <summary>
    ///左夹轨器放松限位
    /// </summary>
    public bool isLeftRailClamperReleaseLimit;
    /// <summary>
    /// 夹轨器故障
    /// </summary>
    public bool isClampFault;
    /// <summary>
    /// 电机过载
    /// </summary>
    public bool isMotorOverload;
    /// <summary>
    /// 右夹轨器电机运行
    /// </summary>
    public bool isRightRailClamperMotorRunning;
    /// <summary>
    /// 右夹轨器电磁阀
    /// </summary>
    public bool isRightRailClamperSolenoidValve;
    /// <summary>
    /// 右侧锚碇限位
    /// </summary>
    public bool isRightAnchorLimit;
    /// <summary>
    /// 右夹轨器放松限位
    /// </summary>
    public bool isRightRailClamperReleaseLimit;
}

/// <summary>
/// 夹轨器
/// </summary>
public class RailGrippingDeviceItem : StatusParmItemBase<RailGrippingDeviceData>
{
    /// <summary>
    /// 主断路器
    /// </summary>
    public ToggleDIY MainCircuitBreaker;
    /// <summary>
    /// 左夹轨器电机运行
    /// </summary>
    public ToggleDIY LeftRailClamperMotorRunning;
    /// <summary>
    /// 左夹轨器电磁阀
    /// </summary>
    public ToggleDIY LeftRailClamperSolenoidValve;
    /// <summary>
    /// 左侧锚碇限位
    /// </summary>
    public ToggleDIY LeftAnchorLimit;
    /// <summary>
    /// 左夹轨器放松限位
    /// </summary>
    public ToggleDIY LeftRailClamperReleaseLimit;
    /// <summary>
    /// 夹轨器故障
    /// </summary>
    public ToggleDIY ClampFault;
    /// <summary>
    /// 电机过载
    /// </summary>
    public ToggleDIY MotorOverload;
    /// <summary>
    /// 右夹轨器电机运行
    /// </summary>
    public ToggleDIY RightRailClamperMotorRunning;
    /// <summary>
    /// 右夹轨器电磁阀
    /// </summary>
    public ToggleDIY RightRailClamperSolenoidValve;
    /// <summary>
    /// 右侧锚碇限位
    /// </summary>
    public ToggleDIY RightAnchorLimit;
    /// <summary>
    /// 右夹轨器放松限位
    /// </summary>
    public ToggleDIY RightRailClamperReleaseLimit;
    public override void UpdateData(RailGrippingDeviceData data,bool isConnect=false)
    {
        SetToggleState(MainCircuitBreaker, data.isMainCircuitBreaker,false,isConnect);
        SetToggleState(LeftRailClamperMotorRunning, data.isLeftRailClamperMotorRunning,false,isConnect);
        SetToggleState(LeftRailClamperSolenoidValve, data.isLeftRailClamperSolenoidValve,false,isConnect);
        SetToggleState(LeftAnchorLimit, data.isLeftAnchorLimit,true,isConnect);
        SetToggleState(LeftRailClamperReleaseLimit, data.isLeftRailClamperReleaseLimit,false,isConnect);
        SetToggleState(ClampFault, data.isClampFault,true,isConnect);
        SetToggleState(MotorOverload, data.isMotorOverload,true,isConnect);
        SetToggleState(RightRailClamperMotorRunning, data.isRightRailClamperMotorRunning,false,isConnect);
        SetToggleState(RightRailClamperSolenoidValve, data.isRightRailClamperSolenoidValve,false,isConnect);
        SetToggleState(RightAnchorLimit, data.isRightAnchorLimit,true,isConnect);
        SetToggleState(RightRailClamperReleaseLimit, data.isRightRailClamperReleaseLimit,false,isConnect);
    }

}
