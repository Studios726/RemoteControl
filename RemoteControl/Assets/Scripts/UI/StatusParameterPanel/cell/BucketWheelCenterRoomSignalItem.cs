using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct BucketWheelCenterRoomSignalData
{
    /// <summary>
    /// 允许堆料信号
    /// </summary>
    public bool isAllowPileMaterSignal;
    /// <summary>
    /// 斗轮机堆料运行
    /// </summary>
    public bool isBucketWheelPileMaterRune;
    /// <summary>
    /// 允许取料信号
    /// </summary>
    public bool isAllowTakeMaterSignal;
    /// <summary>
    /// 斗轮机取料运行
    /// </summary>
    public bool isBucketWheelTakeMaterRun;
    /// <summary>
    /// 允许分流信号
    /// </summary>
    public bool isAllowShuntSignal;
    /// <summary>
    /// 斗轮机分流运行
    /// </summary>
    public bool isBucketWheelShuntRun;
    /// <summary>
    /// 远程急停
    /// </summary>
    public bool isLongDistanceCtrScramStop;
    /// <summary>
    /// 斗轮机故障
    /// </summary>
    public bool isBucketWheelMalfunction;
}
/// <summary>
/// 斗轮机与中控室信号
/// </summary>
public class BucketWheelCenterRoomSignalItem : StatusParmItemBase<BucketWheelCenterRoomSignalData>
{
    /// <summary>
    /// 允许堆料信号
    /// </summary>
    public Toggle AllowPileMaterSignalToggle;
    /// <summary>
    /// 斗轮机堆料运行
    /// </summary>
    public Toggle BucketWheelPileMaterRunToggle;
    /// <summary>
    /// 允许取料信号
    /// </summary>
    public Toggle AllowTakeMaterSignalToggle;
    /// <summary>
    /// 斗轮机取料运行
    /// </summary>
    public Toggle BucketWheelTakeMaterRunToggle;
    /// <summary>
    /// 允许分流信号
    /// </summary>
    public Toggle AllowShuntSignalToggle;
    /// <summary>
    /// 斗轮机分流运行
    /// </summary>
    public Toggle BucketWheelShuntRunToggle;
    /// <summary>
    /// 远程急停
    /// </summary>
    public Toggle LongDistanceCtrScramStopToggle;
    /// <summary>
    /// 斗轮机故障
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
