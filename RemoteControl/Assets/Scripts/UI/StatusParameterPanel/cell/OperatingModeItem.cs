using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct OperatingModeData
{
    /// <summary>
    /// ����
    /// </summary>
    public bool isSingleAction;
    /// <summary>
    /// ����
    /// </summary>
    public bool isLinkageAction;
    /// <summary>
    /// �Զ�
    /// </summary>
    public bool isAutoAction;
    /// <summary>
    /// ����
    /// </summary>
    public bool isLocalAction;
    /// <summary>
    /// Զ��
    /// </summary>
    public bool isLongRangeAction;
}
/// <summary>
/// ������ʽ
/// </summary>
public class OperatingModeItem : StatusParmItemBase<OperatingModeData>
{
    /// <summary>
    /// ����
    /// </summary>
    public Toggle SingleActionToggle;
    /// <summary>
    /// ����
    /// </summary>
    public Toggle LinkageActionToggle;
    /// <summary>
    /// �Զ�
    /// </summary>
    public Toggle AutoActionToggle;
    /// <summary>
    /// ����
    /// </summary>
    public Toggle LocalActionToggle;
    /// <summary>
    /// Զ��
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
