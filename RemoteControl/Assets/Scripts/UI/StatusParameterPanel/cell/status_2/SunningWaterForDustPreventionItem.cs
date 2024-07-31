using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct SunningWaterForDustPreventionData
{
    /// <summary>
    /// ����ϵͳ��ѹ��
    /// </summary>
    public bool isDryFogSysAirPressureLow;
    /// <summary>
    /// ����ϵͳˮѹ��
    /// </summary>
    public bool isDryFogSysWaterPressureLow;
    /// <summary>
    /// ����ϵͳ����������
    /// </summary>
    public bool isDryFogSysFilterClogged;
    /// <summary>
    /// ˮ��Һλ�Ϳ���
    /// </summary>
    public bool isWaterTankLevelLowSwitch;
    /// <summary>
    /// ����ϵͳ����״̬
    /// </summary>
    public bool isDryFogSysSprayStatus;
    /// <summary>
    /// ����ϵͳ��������
    /// </summary>
    public bool isDryFogSysSprayRunning;
    /// <summary>
    /// ����ϵͳ�Զ�����
    /// </summary>
    public bool isDryFogSysAutoRunning;
    /// <summary>
    /// ����ϵͳ�ֶ�����
    /// </summary>
    public bool isDryFogSysManualRunning;
    /// <summary>
    /// �����ֳ�Զ����������
    /// </summary>
    public bool isDryFogDustSuppressionRemoteStartRunning;
    /// <summary>
    /// �����ֳ�Զ��ֹͣ����
    /// </summary>
    public bool isDryFogDustSuppressionRemoteStopRunning;
    /// <summary>
    /// �����ֳ���������
    /// </summary>
    public bool isDryFogDustSuppressionStockpileRunning;
    /// <summary>
    /// ����ȡ������
    /// </summary>
    public bool isDryFogMaterialFetchingRunning;
    /// <summary>
    /// �����ֳ���������
    /// </summary>
    public bool isDryFogDustSuppressionDiversionRunning;
}
/// <summary>
/// ��ˮ�ֳ�
/// </summary>
public class SunningWaterForDustPreventionItem : StatusParmItemBase<SunningWaterForDustPreventionData>
{
    /// <summary>
    /// ����ϵͳ��ѹ��
    /// </summary>
    public Toggle DryFogSysAirPressureLow;
    /// <summary>
    /// ����ϵͳˮѹ��
    /// </summary>
    public Toggle DryFogSysWaterPressureLow;
    /// <summary>
    /// ����ϵͳ����������
    /// </summary>
    public Toggle DryFogSysFilterClogged;
    /// <summary>
    /// ˮ��Һλ�Ϳ���
    /// </summary>
    public Toggle WaterTankLevelLowSwitch;
    /// <summary>
    /// ����ϵͳ����״̬
    /// </summary>
    public Toggle DryFogSysSprayStatus;
    /// <summary>
    /// ����ϵͳ��������
    /// </summary>
    public Toggle DryFogSysSprayRunning;
    /// <summary>
    /// ����ϵͳ�Զ�����
    /// </summary>
    public Toggle DryFogSysAutoRunning;
    /// <summary>
    /// ����ϵͳ�ֶ�����
    /// </summary>
    public Toggle DryFogSysManualRunning;
    /// <summary>
    /// �����ֳ�Զ����������
    /// </summary>
    public Toggle DryFogDustSuppressionRemoteStartRunning;
    /// <summary>
    /// �����ֳ�Զ��ֹͣ����
    /// </summary>
    public Toggle DryFogDustSuppressionRemoteStopRunning;
    /// <summary>
    /// �����ֳ���������
    /// </summary>
    public Toggle DryFogDustSuppressionStockpileRunning;
    /// <summary>
    /// ����ȡ������
    /// </summary>
    public Toggle DryFogMaterialFetchingRunning;
    /// <summary>
    /// �����ֳ���������
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
