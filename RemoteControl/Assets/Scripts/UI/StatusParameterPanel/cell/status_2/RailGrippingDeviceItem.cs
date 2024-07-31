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
    public Toggle MainCircuitBreaker;
    /// <summary>
    /// 左夹轨器电机运行
    /// </summary>
    public Toggle LeftRailClamperMotorRunning;
    /// <summary>
    /// 左夹轨器电磁阀
    /// </summary>
    public Toggle LeftRailClamperSolenoidValve;
    /// <summary>
    /// 左侧锚碇限位
    /// </summary>
    public Toggle LeftAnchorLimit;
    /// <summary>
    /// 左夹轨器放松限位
    /// </summary>
    public Toggle LeftRailClamperReleaseLimit;
    /// <summary>
    /// 电机过载
    /// </summary>
    public Toggle MotorOverload;
    /// <summary>
    /// 右夹轨器电机运行
    /// </summary>
    public Toggle RightRailClamperMotorRunning;
    /// <summary>
    /// 右夹轨器电磁阀
    /// </summary>
    public Toggle RightRailClamperSolenoidValve;
    /// <summary>
    /// 右侧锚碇限位
    /// </summary>
    public Toggle RightAnchorLimit;
    /// <summary>
    /// 右夹轨器放松限位
    /// </summary>
    public Toggle RightRailClamperReleaseLimit;
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
