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
    /// ����
    /// </summary>
    public ToggleDIY SingleActionToggle;
    /// <summary>
    /// ����
    /// </summary>
    public ToggleDIY LinkageActionToggle;
    /// <summary>
    /// �Զ�
    /// </summary>
    public ToggleDIY AutoActionToggle;
    /// <summary>
    /// ����
    /// </summary>
    public ToggleDIY LocalActionToggle;
    /// <summary>
    /// Զ��
    /// </summary>
    public ToggleDIY LongRangeActionToggle;
    public override void UpdateData(OperatingModeData operatingModeData,bool isConnect=false)
    {
        SetToggleState(SingleActionToggle, operatingModeData.isSingleAction,false,isConnect);
        SetToggleState(LinkageActionToggle, operatingModeData.isLinkageAction,false,isConnect);
        SetToggleState(LocalActionToggle, operatingModeData.isLocalAction,false,isConnect);
        SetToggleState(AutoActionToggle, operatingModeData.isAutoAction,false,isConnect);
        SetToggleState(LongRangeActionToggle, operatingModeData.isLongRangeAction,false,isConnect);
    }
}
