using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public struct BucketWheelData
{
    /// <summary>
    /// ����·��
    /// </summary>
    public bool isMainCircuitBreaker;
    /// <summary>
    /// ��������
    /// </summary>
    public bool isBucketWheelRunning;
    /// <summary>
    /// ���ͱ�����
    /// </summary>
    public bool isLubricationPumpRunning;
    /// <summary>
    /// �������
    /// </summary>
    public bool isMotorOverload;
    /// <summary>
    /// ���ֹ����ؿ���
    /// </summary>
    public bool isBucketWheelOverTorqueSwitch;
    /// <summary>
    /// ���ͱ���������
    /// </summary>
    public bool isLubricatingOilPumpFlowSwitch;
}
/// <summary>
/// ����
/// </summary>
public class BucketWheelItem : StatusParmItemBase<BucketWheelData>
{
    /// <summary>
    /// ����·��
    /// </summary>
    public Toggle MainCircuitBreaker;
    /// <summary>
    /// ��������
    /// </summary>
    public Toggle BucketWheelRunning;
    /// <summary>
    /// ���ͱ�����
    /// </summary>
    public Toggle LubricationPumpRunning;
    /// <summary>
    /// �������
    /// </summary>
    public Toggle MotorOverload;
    /// <summary>
    /// ���ֹ����ؿ���
    /// </summary>
    public Toggle BucketWheelOverTorqueSwitch;
    /// <summary>
    /// ���ͱ���������
    /// </summary>
    public Toggle LubricatingOilPumpFlowSwitch;
    public override void UpdateData(BucketWheelData data)
    {
        SetToggleState(MainCircuitBreaker,data.isMainCircuitBreaker);
        SetToggleState(BucketWheelRunning, data.isBucketWheelRunning);
        SetToggleState(LubricationPumpRunning, data.isLubricationPumpRunning);
        SetToggleState(MotorOverload, data.isMotorOverload);
        SetToggleState(BucketWheelOverTorqueSwitch, data.isBucketWheelOverTorqueSwitch);
        SetToggleState(LubricatingOilPumpFlowSwitch, data.isLubricatingOilPumpFlowSwitch);
    }
}
