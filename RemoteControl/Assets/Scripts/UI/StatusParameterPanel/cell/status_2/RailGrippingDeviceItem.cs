using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct RailGrippingDeviceData
{
    /// <summary>
    /// 主断路器
    /// </summary>
    public bool isMainCircuitBreaker;
    /// <summary>
    /// 左夹轨器电机运行
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
    /// 左夹轨器放松限位
    /// </summary>
    public bool isLeftRailClamperReleaseLimit;
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
    public override void UpdateData(RailGrippingDeviceData data)
    {
        SetToggleState(MainCircuitBreaker, data.isMainCircuitBreaker);
        SetToggleState(LeftRailClamperMotorRunning, data.isLeftRailClamperMotorRunning);
        SetToggleState(LeftRailClamperSolenoidValve, data.isLeftRailClamperSolenoidValve);
        SetToggleState(LeftAnchorLimit, data.isLeftAnchorLimit);
        SetToggleState(LeftRailClamperReleaseLimit, data.isLeftRailClamperReleaseLimit);
        SetToggleState(MotorOverload, data.isMotorOverload);
        SetToggleState(RightRailClamperMotorRunning, data.isRightRailClamperMotorRunning);
        SetToggleState(RightRailClamperSolenoidValve, data.isRightRailClamperSolenoidValve);
        SetToggleState(RightAnchorLimit, data.isRightAnchorLimit);
        SetToggleState(RightRailClamperReleaseLimit, data.isRightRailClamperReleaseLimit);
    }

}
