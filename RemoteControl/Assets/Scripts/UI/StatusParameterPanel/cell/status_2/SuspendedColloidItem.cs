using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct SuspendedColloidData
{
    /// <summary>
    /// ����·��
    /// </summary>
    public bool isMainCircuitBreaker;
    /// <summary>
    /// �������
    /// </summary>
    public bool isMotorOverload;
    /// <summary>
    /// �ƶ�����
    /// </summary>
    public bool isBrakeOpen;
    /// <summary>
    /// �ƶ�����բ��λ
    /// </summary>
    public bool isBrakeReleaseLimit;
    /// <summary>
    /// ��������
    /// </summary>
    public bool isStackingOperation;
    /// <summary>
    /// ȡ������
    /// </summary>
    public bool isFetchingOperation;
    /// <summary>
    /// һ����ƫ����
    /// </summary>
    public bool isFirstLevelDeviationSwitch;
    /// <summary>
    /// ������ƫ����
    /// </summary>
    public bool isSecondLevelDeviationSwitch;
    /// <summary>
    /// �򻬼�⿪��
    /// </summary>
    public bool isSlippingDetectionSwitch;
    /// <summary>
    /// ����˺�ѿ���
    /// </summary>
    public bool isLongitudinalTearSwitch;
    /// <summary>
    /// ��ͣ���߿���
    /// </summary>
    public bool isEmergencyStopCableSwitch;
    /// <summary>
    /// ������⿪��
    /// </summary>
    public bool isMaterialFlowDetectionSwitch;
    /// <summary>
    /// �в��϶���ú
    /// </summary>
    public bool isMiddleHopperCoalBlocking;
}
/// <summary>
/// ����
/// </summary>
public class SuspendedColloidItem : StatusParmItemBase<SuspendedColloidData>
{
    /// <summary>
    /// ����·��
    /// </summary>
    public Toggle MainCircuitBreaker;
    /// <summary>
    /// �������
    /// </summary>
    public Toggle MotorOverload;
    /// <summary>
    /// �ƶ�����
    /// </summary>
    public Toggle BrakeOpen;
    /// <summary>
    /// �ƶ�����բ��λ
    /// </summary>
    public Toggle BrakeReleaseLimit;
    /// <summary>
    /// ��������
    /// </summary>
    public Toggle StackingOperation;
    /// <summary>
    /// ȡ������
    /// </summary>
    public Toggle FetchingOperation;
    /// <summary>
    /// һ����ƫ����
    /// </summary>
    public Toggle FirstLevelDeviationSwitch;
    /// <summary>
    /// ������ƫ����
    /// </summary>
    public Toggle SecondLevelDeviationSwitch;
    /// <summary>
    /// �򻬼�⿪��
    /// </summary>
    public Toggle SlippingDetectionSwitch;
    /// <summary>
    /// ����˺�ѿ���
    /// </summary>
    public Toggle LongitudinalTearSwitch;
    /// <summary>
    /// ��ͣ���߿���
    /// </summary>
    public Toggle EmergencyStopCableSwitch;
    /// <summary>
    /// ������⿪��
    /// </summary>
    public Toggle MaterialFlowDetectionSwitch;
    /// <summary>
    /// �в��϶���ú
    /// </summary>
    public Toggle MiddleHopperCoalBlocking;
    public override void UpdateData(SuspendedColloidData data)
    {
        SetToggleState(MainCircuitBreaker, data.isMainCircuitBreaker);
        SetToggleState(MotorOverload, data.isMotorOverload);
        SetToggleState(BrakeOpen, data.isBrakeOpen);
        SetToggleState(BrakeReleaseLimit, data.isBrakeReleaseLimit);
        SetToggleState(StackingOperation, data.isStackingOperation);
        SetToggleState(FetchingOperation, data.isFetchingOperation);
        SetToggleState(FirstLevelDeviationSwitch, data.isFirstLevelDeviationSwitch);
        SetToggleState(SecondLevelDeviationSwitch, data.isSecondLevelDeviationSwitch);
        SetToggleState(SlippingDetectionSwitch, data.isSlippingDetectionSwitch);
        SetToggleState(LongitudinalTearSwitch, data.isLongitudinalTearSwitch);
        SetToggleState(EmergencyStopCableSwitch, data.isEmergencyStopCableSwitch);
        SetToggleState(MaterialFlowDetectionSwitch, data.isMaterialFlowDetectionSwitch);
        SetToggleState(MiddleHopperCoalBlocking, data.isMiddleHopperCoalBlocking);
    }
}
