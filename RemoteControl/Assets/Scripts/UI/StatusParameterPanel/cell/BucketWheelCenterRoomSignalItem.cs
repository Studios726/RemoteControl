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
    public ToggleDIY AllowPileMaterSignalToggle;
    /// <summary>
    /// 斗轮机堆料运行
    /// </summary>
    public ToggleDIY BucketWheelPileMaterRunToggle;
    /// <summary>
    /// 允许取料信号
    /// </summary>
    public ToggleDIY AllowTakeMaterSignalToggle;
    /// <summary>
    /// 斗轮机取料运行
    /// </summary>
    public ToggleDIY BucketWheelTakeMaterRunToggle;
    /// <summary>
    /// 允许分流信号
    /// </summary>
    public ToggleDIY AllowShuntSignalToggle;
    /// <summary>
    /// 斗轮机分流运行
    /// </summary>
    public ToggleDIY BucketWheelShuntRunToggle;
    /// <summary>
    /// 远程急停
    /// </summary>
    public ToggleDIY LongDistanceCtrScramStopToggle;
    /// <summary>
    /// 斗轮机故障
    /// </summary>
    public ToggleDIY BucketWheelMalfunctionToggle;

    public override void UpdateData(BucketWheelCenterRoomSignalData data,bool isConnect=false)
    {
        SetToggleState(AllowPileMaterSignalToggle, data.isAllowPileMaterSignal,false,isConnect);
        SetToggleState(BucketWheelPileMaterRunToggle, data.isAllowPileMaterSignal,false,isConnect);
        SetToggleState(AllowTakeMaterSignalToggle, data.isAllowTakeMaterSignal,false,isConnect);
        SetToggleState(BucketWheelTakeMaterRunToggle, data.isBucketWheelTakeMaterRun,false,isConnect);
        SetToggleState(AllowShuntSignalToggle, data.isAllowShuntSignal,false,isConnect);
        SetToggleState(BucketWheelShuntRunToggle, data.isBucketWheelShuntRun,false,isConnect);
        SetToggleState(LongDistanceCtrScramStopToggle, data.isLongDistanceCtrScramStop,false,isConnect);
        SetToggleState(BucketWheelMalfunctionToggle, data.isBucketWheelMalfunction,true,isConnect);
    }


}
