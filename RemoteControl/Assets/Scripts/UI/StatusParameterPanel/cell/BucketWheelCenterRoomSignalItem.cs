using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct BucketWheelCenterRoomSignalData
{
    /// <summary>
    /// ��������ź�
    /// </summary>
    public bool isAllowPileMaterSignal;
    /// <summary>
    /// ���ֻ���������
    /// </summary>
    public bool isBucketWheelPileMaterRune;
    /// <summary>
    /// ����ȡ���ź�
    /// </summary>
    public bool isAllowTakeMaterSignal;
    /// <summary>
    /// ���ֻ�ȡ������
    /// </summary>
    public bool isBucketWheelTakeMaterRun;
    /// <summary>
    /// ��������ź�
    /// </summary>
    public bool isAllowShuntSignal;
    /// <summary>
    /// ���ֻ���������
    /// </summary>
    public bool isBucketWheelShuntRun;
    /// <summary>
    /// Զ�̼�ͣ
    /// </summary>
    public bool isLongDistanceCtrScramStop;
    /// <summary>
    /// ���ֻ�����
    /// </summary>
    public bool isBucketWheelMalfunction;
}
/// <summary>
/// ���ֻ����п����ź�
/// </summary>
public class BucketWheelCenterRoomSignalItem : StatusParmItemBase<BucketWheelCenterRoomSignalData>
{
    /// <summary>
    /// ��������ź�
    /// </summary>
    public Toggle AllowPileMaterSignalToggle;
    /// <summary>
    /// ���ֻ���������
    /// </summary>
    public Toggle BucketWheelPileMaterRunToggle;
    /// <summary>
    /// ����ȡ���ź�
    /// </summary>
    public Toggle AllowTakeMaterSignalToggle;
    /// <summary>
    /// ���ֻ�ȡ������
    /// </summary>
    public Toggle BucketWheelTakeMaterRunToggle;
    /// <summary>
    /// ��������ź�
    /// </summary>
    public Toggle AllowShuntSignalToggle;
    /// <summary>
    /// ���ֻ���������
    /// </summary>
    public Toggle BucketWheelShuntRunToggle;
    /// <summary>
    /// Զ�̼�ͣ
    /// </summary>
    public Toggle LongDistanceCtrScramStopToggle;
    /// <summary>
    /// ���ֻ�����
    /// </summary>
    public Toggle BucketWheelMalfunctionToggle;

    public override void UpdateData(BucketWheelCenterRoomSignalData data)
    {
        SetToggleState(AllowPileMaterSignalToggle, data.isAllowPileMaterSignal);
        SetToggleState(BucketWheelPileMaterRunToggle, data.isAllowPileMaterSignal);
        SetToggleState(AllowTakeMaterSignalToggle, data.isAllowTakeMaterSignal);
        SetToggleState(BucketWheelTakeMaterRunToggle, data.isBucketWheelTakeMaterRun);
        SetToggleState(AllowShuntSignalToggle, data.isAllowShuntSignal);
        SetToggleState(BucketWheelShuntRunToggle, data.isBucketWheelShuntRun);
        SetToggleState(LongDistanceCtrScramStopToggle, data.isLongDistanceCtrScramStop);
        SetToggleState(BucketWheelMalfunctionToggle, data.isBucketWheelMalfunction);
    }


}
