using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct OperatingModeData
{
    /// <summary>
    /// 单动
    /// </summary>
    public bool isSingleAction;
    /// <summary>
    /// 联动
    /// </summary>
    public bool isLinkageAction;
    /// <summary>
    /// 自动
    /// </summary>
    public bool isAutoAction;
    /// <summary>
    /// 本地
    /// </summary>
    public bool isLocalAction;
    /// <summary>
    /// 远程
    /// </summary>
    public bool isLongRangeAction;
}
/// <summary>
/// 操作方式
/// </summary>
public class OperatingModeItem : StatusParmItemBase<OperatingModeData>
{
    /// <summary>
    /// 单动
    /// </summary>
    public Toggle SingleActionToggle;
    /// <summary>
    /// 联动
    /// </summary>
    public Toggle LinkageActionToggle;
    /// <summary>
    /// 自动
    /// </summary>
    public Toggle AutoActionToggle;
    /// <summary>
    /// 本地
    /// </summary>
    public Toggle LocalActionToggle;
    /// <summary>
    /// 远程
    /// </summary>
    public Toggle LongRangeActionToggle;
    public override void UpdateData(OperatingModeData operatingModeData)
    {
        SetToggleState(SingleActionToggle, operatingModeData.isSingleAction);
        SetToggleState(LinkageActionToggle, operatingModeData.isLinkageAction);
        SetToggleState(LocalActionToggle, operatingModeData.isLocalAction);
        SetToggleState(AutoActionToggle, operatingModeData.isAutoAction);
        SetToggleState(LongRangeActionToggle, operatingModeData.isLongRangeAction);
    }
}
