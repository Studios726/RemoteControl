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
    ///干雾抑尘远程启动运行
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
    /// 干雾抑尘取料运行
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
    public ToggleDIY DryFogSysAirPressureLow;
    /// <summary>
    /// 干雾系统水压低
    /// </summary>
    public ToggleDIY DryFogSysWaterPressureLow;
    /// <summary>
    /// 干雾系统过滤器堵塞
    /// </summary>
    public ToggleDIY DryFogSysFilterClogged;
    /// <summary>
    /// 水箱液位低开关
    /// </summary>
    public ToggleDIY WaterTankLevelLowSwitch;
    /// <summary>
    /// 干雾系统喷雾状态
    /// </summary>
    public ToggleDIY DryFogSysSprayStatus;
    /// <summary>
    /// 干雾系统喷雾运行
    /// </summary>
    public ToggleDIY DryFogSysSprayRunning;
    /// <summary>
    /// 干雾系统自动运行
    /// </summary>
    public ToggleDIY DryFogSysAutoRunning;
    /// <summary>
    /// 干雾系统手动运行
    /// </summary>
    public ToggleDIY DryFogSysManualRunning;
    /// <summary>
    /// 干雾抑尘远程启动运行
    /// </summary>
    public ToggleDIY DryFogDustSuppressionRemoteStartRunning;
    /// <summary>
    /// 干雾抑尘远程停止运行
    /// </summary>
    public ToggleDIY DryFogDustSuppressionRemoteStopRunning;
    /// <summary>
    /// 干雾抑尘堆料运行
    /// </summary>
    public ToggleDIY DryFogDustSuppressionStockpileRunning;
    /// <summary>
    /// 干雾抑尘取料运行
    /// </summary>
    public ToggleDIY DryFogMaterialFetchingRunning;
    /// <summary>
    /// 干雾抑尘分流运行
    /// </summary>
    public ToggleDIY DryFogDustSuppressionDiversionRunning;
    public override void UpdateData(SunningWaterForDustPreventionData data,bool isConnect=false)
    {
        SetToggleState(DryFogSysAirPressureLow,data.isDryFogSysAirPressureLow,true,isConnect);
        SetToggleState(DryFogSysWaterPressureLow, data.isDryFogSysWaterPressureLow, true, isConnect);
        SetToggleState(DryFogSysFilterClogged, data.isDryFogSysFilterClogged,true,isConnect);
        SetToggleState(WaterTankLevelLowSwitch, data.isWaterTankLevelLowSwitch,true,isConnect);
        SetToggleState(DryFogSysSprayStatus, data.isDryFogSysSprayStatus,false,isConnect);
        SetToggleState(DryFogSysSprayRunning, data.isDryFogSysSprayRunning,false,isConnect);
        SetToggleState(DryFogSysAutoRunning, data.isDryFogSysAutoRunning,false,isConnect);
        SetToggleState(DryFogSysManualRunning, data.isDryFogSysManualRunning,false,isConnect);
        SetToggleState(DryFogDustSuppressionRemoteStartRunning, data.isDryFogDustSuppressionRemoteStartRunning,false,isConnect);
        SetToggleState(DryFogDustSuppressionRemoteStopRunning, data.isDryFogDustSuppressionRemoteStopRunning,false,isConnect);
        SetToggleState(DryFogDustSuppressionStockpileRunning, data.isDryFogDustSuppressionStockpileRunning,false,isConnect);
        SetToggleState(DryFogMaterialFetchingRunning, data.isDryFogMaterialFetchingRunning,false,isConnect);
        SetToggleState(DryFogDustSuppressionDiversionRunning, data.isDryFogDustSuppressionDiversionRunning,false,isConnect);
    }
}
