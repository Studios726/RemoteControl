using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct SunningWaterForDustPreventionData
{
    /// <summary>
    /// 干雾系统气压低
    /// </summary>
    public bool isDryFogSysAirPressureLow;
    /// <summary>
    /// 干雾系统水压低
    /// </summary>
    public bool isDryFogSysWaterPressureLow;
    /// <summary>
    /// 干雾系统过滤器堵塞
    /// </summary>
    public bool isDryFogSysFilterClogged;
    /// <summary>
    /// 水箱液位低开关
    /// </summary>
    public bool isWaterTankLevelLowSwitch;
    /// <summary>
    /// 干雾系统喷雾状态
    /// </summary>
    public bool isDryFogSysSprayStatus;
    /// <summary>
    /// 干雾系统喷雾运行
    /// </summary>
    public bool isDryFogSysSprayRunning;
    /// <summary>
    /// 干雾系统自动运行
    /// </summary>
    public bool isDryFogSysAutoRunning;
    /// <summary>
    /// 干雾系统手动运行
    /// </summary>
    public bool isDryFogSysManualRunning;
    /// <summary>
    /// 干雾抑尘远程启动运行
    /// </summary>
    public bool isDryFogDustSuppressionRemoteStartRunning;
    /// <summary>
    /// 干雾抑尘远程停止运行
    /// </summary>
    public bool isDryFogDustSuppressionRemoteStopRunning;
    /// <summary>
    /// 干雾抑尘堆料运行
    /// </summary>
    public bool isDryFogDustSuppressionStockpileRunning;
    /// <summary>
    /// 干雾取料运行
    /// </summary>
    public bool isDryFogMaterialFetchingRunning;
    /// <summary>
    /// 干雾抑尘分流运行
    /// </summary>
    public bool isDryFogDustSuppressionDiversionRunning;
}
/// <summary>
/// 洒水抑尘
/// </summary>
public class SunningWaterForDustPreventionItem : StatusParmItemBase<SunningWaterForDustPreventionData>
{
    /// <summary>
    /// 干雾系统气压低
    /// </summary>
    public Toggle DryFogSysAirPressureLow;
    /// <summary>
    /// 干雾系统水压低
    /// </summary>
    public Toggle DryFogSysWaterPressureLow;
    /// <summary>
    /// 干雾系统过滤器堵塞
    /// </summary>
    public Toggle DryFogSysFilterClogged;
    /// <summary>
    /// 水箱液位低开关
    /// </summary>
    public Toggle WaterTankLevelLowSwitch;
    /// <summary>
    /// 干雾系统喷雾状态
    /// </summary>
    public Toggle DryFogSysSprayStatus;
    /// <summary>
    /// 干雾系统喷雾运行
    /// </summary>
    public Toggle DryFogSysSprayRunning;
    /// <summary>
    /// 干雾系统自动运行
    /// </summary>
    public Toggle DryFogSysAutoRunning;
    /// <summary>
    /// 干雾系统手动运行
    /// </summary>
    public Toggle DryFogSysManualRunning;
    /// <summary>
    /// 干雾抑尘远程启动运行
    /// </summary>
    public Toggle DryFogDustSuppressionRemoteStartRunning;
    /// <summary>
    /// 干雾抑尘远程停止运行
    /// </summary>
    public Toggle DryFogDustSuppressionRemoteStopRunning;
    /// <summary>
    /// 干雾抑尘堆料运行
    /// </summary>
    public Toggle DryFogDustSuppressionStockpileRunning;
    /// <summary>
    /// 干雾取料运行
    /// </summary>
    public Toggle DryFogMaterialFetchingRunning;
    /// <summary>
    /// 干雾抑尘分流运行
    /// </summary>
    public Toggle DryFogDustSuppressionDiversionRunning;
    public override void UpdateData(SunningWaterForDustPreventionData data)
    {
        SetToggleState(DryFogSysAirPressureLow,data.isDryFogSysAirPressureLow);
        SetToggleState(DryFogSysWaterPressureLow, data.isDryFogSysWaterPressureLow);
        SetToggleState(DryFogSysFilterClogged, data.isDryFogSysFilterClogged);
        SetToggleState(WaterTankLevelLowSwitch, data.isWaterTankLevelLowSwitch);
        SetToggleState(DryFogSysSprayStatus, data.isDryFogSysSprayStatus);
        SetToggleState(DryFogSysSprayRunning, data.isDryFogSysSprayRunning);
        SetToggleState(DryFogSysAutoRunning, data.isDryFogSysAutoRunning);
        SetToggleState(DryFogSysManualRunning, data.isDryFogSysManualRunning);
        SetToggleState(DryFogDustSuppressionRemoteStartRunning, data.isDryFogDustSuppressionRemoteStartRunning);
        SetToggleState(DryFogDustSuppressionRemoteStopRunning, data.isDryFogDustSuppressionRemoteStopRunning);
        SetToggleState(DryFogDustSuppressionStockpileRunning, data.isDryFogDustSuppressionStockpileRunning);
        SetToggleState(DryFogMaterialFetchingRunning, data.isDryFogMaterialFetchingRunning);
        SetToggleState(DryFogDustSuppressionDiversionRunning, data.isDryFogDustSuppressionDiversionRunning);
    }
}
