using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public struct PileTakeFlowStateData
{
    /// <summary>
    /// ������������
    /// </summary>
    public bool isSuspensoidPileMaterRunToggle;
    /// <summary>
    /// �����������
    /// </summary>
    public bool isBafflePileMaterPos;
    /// <summary>
    /// �������λ��
    /// </summary>
    public bool isBaffleShuntPos;
    /// <summary>
    /// ���ϲ۶���λ
    /// </summary>
    public bool isFeedChannelPileMater;
    /// <summary>
    /// ����ȡ������
    /// </summary>
    public bool isSuspensoidTakeMaterRun;
    /// <summary>
    /// ���ֻ�����
    /// </summary>
    public bool isBucketWheelRun;
    /// <summary>
    /// ���ϲ�ȡ��
    /// </summary>
    public bool isFeedChannelTake;
}
/// <summary>
/// ��ȡ������״̬
/// </summary>
public class PileTakeFlowStateItem : StatusParmItemBase<PileTakeFlowStateData>
{
    /// <summary>
    /// ������������
    /// </summary>
    public Toggle SuspensoidPileMaterRunToggle;
    /// <summary>
    /// �����������
    /// </summary>
    public Toggle BafflePileMaterPosToggle;
    /// <summary>
    /// �������λ��
    /// </summary>
    public Toggle BaffleShuntPosToggle;
    /// <summary>
    /// ���ϲ۶���λ
    /// </summary>
    public Toggle FeedChannelPileMaterToggle;
    /// <summary>
    /// ����ȡ������
    /// </summary>
    public Toggle SuspensoidTakeMaterRunToggle;
    /// <summary>
    /// ���ֻ�����
    /// </summary>
    public Toggle BucketWheelRunToggle;
    /// <summary>
    /// ���ϲ�ȡ��
    /// </summary>
    public Toggle FeedChannelTakeToggle;


    public override void UpdateData(PileTakeFlowStateData data)
    {
        SetToggleState(SuspensoidPileMaterRunToggle, data.isSuspensoidPileMaterRunToggle);
        SetToggleState(BafflePileMaterPosToggle, data.isBafflePileMaterPos);
        SetToggleState(BaffleShuntPosToggle, data.isBaffleShuntPos);
        SetToggleState(FeedChannelPileMaterToggle, data.isFeedChannelPileMater);

        SetToggleState(SuspensoidTakeMaterRunToggle, data.isSuspensoidTakeMaterRun);
        SetToggleState(BucketWheelRunToggle, data.isBucketWheelRun);
        SetToggleState(FeedChannelTakeToggle, data.isFeedChannelTake);
    }
}
