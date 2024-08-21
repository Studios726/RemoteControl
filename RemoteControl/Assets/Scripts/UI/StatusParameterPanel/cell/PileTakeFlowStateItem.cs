using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public struct PileTakeFlowStateData
{
    /// <summary>
    /// 悬胶堆料运行
    /// </summary>
    public bool isSuspensoidPileMaterRunToggle;
    /// <summary>
    /// 挡板堆料位置
    /// </summary>
    public bool isBafflePileMaterPos;
    /// <summary>
    /// 挡板分流位置
    /// </summary>
    public bool isBaffleShuntPos;
    /// <summary>
    /// 导料槽堆料位
    /// </summary>
    public bool isFeedChannelPileMater;
    /// <summary>
    /// 悬胶取料运行
    /// </summary>
    public bool isSuspensoidTakeMaterRun;
    /// <summary>
    /// 斗轮机运行
    /// </summary>
    public bool isBucketWheelRun;
    /// <summary>
    /// 导料槽取料位
    /// </summary>
    public bool isFeedChannelTake;
}
/// <summary>
/// 堆取料流程状态
/// </summary>
public class PileTakeFlowStateItem : StatusParmItemBase<PileTakeFlowStateData>
{
    /// <summary>
    /// 悬胶堆料运行
    /// </summary>
    public ToggleDIY SuspensoidPileMaterRunToggle;
    /// <summary>
    /// 挡板堆料位置
    /// </summary>
    public ToggleDIY BafflePileMaterPosToggle;
    /// <summary>
    /// 挡板分流位置
    /// </summary>
    public ToggleDIY BaffleShuntPosToggle;
    /// <summary>
    /// 导料槽堆料位
    /// </summary>
    public ToggleDIY FeedChannelPileMaterToggle;
    /// <summary>
    /// 悬胶取料运行
    /// </summary>
    public ToggleDIY SuspensoidTakeMaterRunToggle;
    /// <summary>
    /// 斗轮机运行
    /// </summary>
    public ToggleDIY BucketWheelRunToggle;
    /// <summary>
    /// 导料槽取料
    /// </summary>
    public ToggleDIY FeedChannelTakeToggle;


    public override void UpdateData(PileTakeFlowStateData data,bool isConnect=false)
    {
        SetToggleState(SuspensoidPileMaterRunToggle, data.isSuspensoidPileMaterRunToggle,false,isConnect);
        SetToggleState(BafflePileMaterPosToggle, data.isBafflePileMaterPos,false,isConnect);
        SetToggleState(BaffleShuntPosToggle, data.isBaffleShuntPos,false,isConnect);
        SetToggleState(FeedChannelPileMaterToggle, data.isFeedChannelPileMater,false,isConnect);

        SetToggleState(SuspensoidTakeMaterRunToggle, data.isSuspensoidTakeMaterRun,false,isConnect);
        SetToggleState(BucketWheelRunToggle, data.isBucketWheelRun,false,isConnect);
        SetToggleState(FeedChannelTakeToggle, data.isFeedChannelTake,false,isConnect);
    }
}
