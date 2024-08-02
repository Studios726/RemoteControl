using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct CableWindingDeviceData
{
    /// <summary>
    /// ����·�� 
    /// </summary>
    public bool isMainCircuitBreaker;
    /// <summary>
    /// ��Ͳ������λ1 
    /// </summary>
    public bool isReelOverTightLimit1;
    /// <summary>
    /// ��Ͳ������λ1 
    /// </summary>
    public bool isReelOverLooseLimit1;
    /// <summary>
    /// ��Ͳ���̿���
    /// </summary>
    public bool isReelEmptyDiskSwitch;
    /// <summary>
    ///  ��Ͳ��բ���� 
    /// </summary>
    public bool isReelMiddleBrakeSwitch;
    /// <summary>
    /// ��Ͳ������� 
    /// </summary>
    public bool isReelMotorOverload;
    /// <summary>
    /// ��Ͳ������λ2 
    /// </summary>
    public bool isReelOverTightLimit2;
    /// <summary>
    /// ��Ͳ������λ2 
    /// </summary>
    public bool isReelOverLooseLimit2;
    /// <summary>
    /// ��Ͳ���̿��� 
    /// </summary>
    public bool isReelFullDiskSwitch;
    /// <summary>
    /// ������Ͳ����
    /// </summary>
    public bool isPowerReelRunning;
}
/// <summary>
/// ���¾�Ͳ
/// </summary>
public class CableWindingDeviceItem : StatusParmItemBase<CableWindingDeviceData>
{
    /// <summary>
    /// ����·�� 
    /// </summary>
    public Toggle MainCircuitBreaker;
    /// <summary>
    /// ��Ͳ������λ1 
    /// </summary>
    public Toggle ReelOverTightLimit1;
    /// <summary>
    /// ��Ͳ������λ1 
    /// </summary>
    public Toggle ReelOverLooseLimit1;
    /// <summary>
    /// ��Ͳ���̿���
    /// </summary>
    public Toggle ReelEmptyDiskSwitch;
    /// <summary>
    ///  ��Ͳ��բ���� 
    /// </summary>
    public Toggle ReelMiddleBrakeSwitch;
    /// <summary>
    /// ��Ͳ������� 
    /// </summary>
    public Toggle ReelMotorOverload;
    /// <summary>
    /// ��Ͳ������λ2 
    /// </summary>
    public Toggle ReelOverTightLimit2;
    /// <summary>
    /// ��Ͳ������λ2 
    /// </summary>
    public Toggle ReelOverLooseLimit2;
    /// <summary>
    /// ��Ͳ���̿��� 
    /// </summary>
    public Toggle ReelFullDiskSwitch;
    /// <summary>
    /// ������Ͳ����
    /// </summary>
    public Toggle PowerReelRunning;
    public override void UpdateData(CableWindingDeviceData data)
    {
        SetToggleState(MainCircuitBreaker, data.isMainCircuitBreaker);
        SetToggleState(ReelOverTightLimit1, data.isReelOverTightLimit1);
        SetToggleState(ReelOverLooseLimit1, data.isReelOverLooseLimit1);
        SetToggleState(ReelEmptyDiskSwitch, data.isReelEmptyDiskSwitch);
        SetToggleState(ReelMiddleBrakeSwitch, data.isReelMiddleBrakeSwitch);
        SetToggleState(ReelMotorOverload, data.isReelMotorOverload);
        SetToggleState(ReelOverTightLimit2, data.isReelOverTightLimit2);
        SetToggleState(ReelOverLooseLimit2, data.isReelOverLooseLimit2);
        SetToggleState(ReelFullDiskSwitch, data.isReelFullDiskSwitch);
        SetToggleState(PowerReelRunning, data.isPowerReelRunning);
    }
}
