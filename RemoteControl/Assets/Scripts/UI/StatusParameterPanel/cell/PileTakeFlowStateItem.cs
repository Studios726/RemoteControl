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
    /// 挡板堆料运行
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
    /// 导料槽取料
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
    public Toggle SuspensoidPileMaterRunToggle;
    /// <summary>
    /// 挡板堆料运行
    /// </summary>
    public Toggle BafflePileMaterPosToggle;
    /// <summary>
    /// 挡板分流位置
    /// </summary>
    public Toggle BaffleShuntPosToggle;
    /// <summary>
    /// 导料槽堆料位
    /// </summary>
    public Toggle FeedChannelPileMaterToggle;
    /// <summary>
    /// 悬胶取料运行
    /// </summary>
    public Toggle SuspensoidTakeMaterRunToggle;
    /// <summary>
    /// 斗轮机运行
    /// </summary>
    public Toggle BucketWheelRunToggle;
    /// <summary>
    /// 导料槽取料
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
