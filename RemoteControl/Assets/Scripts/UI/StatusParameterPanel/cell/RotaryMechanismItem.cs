using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct RotaryMechanismData
{
    /// <summary>
    /// ����·��
    /// </summary>
    public bool isMainCircuitBreaker;
    /// <summary>
    /// ��Ƶ������
    /// </summary>
    public bool isFrequencyConverterRunning;
    /// <summary>
    /// �ƶ�������
    /// </summary>
    public bool isBrakeOperating;
    /// <summary>
    /// �������
    /// </summary>
    public bool isFanOperating;
    /// <summary>
    /// ��ת����
    /// </summary>
    public bool isLeftTurnOperating;
    /// <summary>
    /// ��ת����
    /// </summary>
    public bool isRightTurnOperating;

    /// <summary>
    /// ��Ƶ������
    /// </summary>
    public bool isFrequencyConverterFault;
    /// <summary>
    /// �ƶ�������
    /// </summary>
    public bool isBrakeOverload;
    /// <summary>
    /// �������
    /// </summary>
    public bool isFanOverload;
    /// <summary>
    /// �ƶ����賬��
    /// </summary>
    public bool isBrakingResistorOverheating;
    /// <summary>
    /// ��ת����
    /// </summary>
    public bool isRotaryFault;

    /// <summary>
    /// ��ת��λ
    /// </summary>
    public bool isLeftTurnLimit;
    /// <summary>
    /// ��ת����
    /// </summary>
    public bool isLeftTurnLimitExceed;
    /// <summary>
    /// ��ת������λ
    /// </summary>
    public bool isLeftTurnRestrictedZoneLimit;
    /// <summary>
    /// ��ת��ײ��λ
    /// </summary>
    public bool isLeftTurnCollisionPreventionLimit;
    /// <summary>
    /// ��ת��λ
    /// </summary>
    public bool isRightTurnLimit;
    /// <summary>
    /// ��ת����
    /// </summary>
    public bool isRightTurnLimitExceed;
    /// <summary>
    /// ��ת������λ
    /// </summary>
    public bool isRightTurnRestrictedZoneLimit;
    /// <summary>
    /// ��ת��ײ��λ
    /// </summary>
    public bool isRightTurnCollisionPreventionLimit;

    /// <summary>
    /// ��ת������
    /// </summary>
    public bool isRotaryOverTorque;
    /// <summary>
    /// ��ת��λ��λ
    /// </summary>
    public bool isRotaryZeroPositionLimit;
    /// <summary>
    /// �ƶ�����բ��λ
    /// </summary>
    public bool isBrakeReliefLimit;
    /// <summary>
    /// ��ת�����󻬶���
    /// </summary>
    public bool isRotaryCentralLubricationBlockedOil;
    /// <summary>
    /// ��ת�����󻬵���λ
    /// </summary>
    public bool isRotaryCentralLubricationLowOilLevel;
}
/// <summary>
/// ��ת����
/// </summary>
public class RotaryMechanismItem : StatusParmItemBase<RotaryMechanismData>
{
    /// <summary>
    /// ����·��
    /// </summary>
    public Toggle MainCircuitBreaker;
    /// <summary>
    /// ��Ƶ������
    /// </summary>
    public Toggle FrequencyConverterRunning;
    /// <summary>
    /// �ƶ�������
    /// </summary>
    public Toggle BrakeOperating;
    /// <summary>
    /// �������
    /// </summary>
    public Toggle FanOperating;
    /// <summary>
    /// ��ת����
    /// </summary>
    public Toggle LeftTurnOperating;
    /// <summary>
    /// ��ת����
    /// </summary>
    public Toggle RightTurnOperating;

    /// <summary>
    /// ��Ƶ������
    /// </summary>
    public Toggle FrequencyConverterFault;
    /// <summary>
    /// �ƶ�������
    /// </summary>
    public Toggle BrakeOverload;
    /// <summary>
    /// �������
    /// </summary>
    public Toggle FanOverload;
    /// <summary>
    /// �ƶ����賬��
    /// </summary>
    public Toggle BrakingResistorOverheating;
    /// <summary>
    /// ��ת����
    /// </summary>
    public Toggle RotaryFault;

    /// <summary>
    /// ��ת��λ
    /// </summary>
    public Toggle LeftTurnLimit;
    /// <summary>
    /// ��ת����
    /// </summary>
    public Toggle LeftTurnLimitExceed;
    /// <summary>
    /// ��ת������λ
    /// </summary>
    public Toggle LeftTurnRestrictedZoneLimit;
    /// <summary>
    /// ��ת��ײ��λ
    /// </summary>
    public Toggle LeftTurnCollisionPreventionLimit;
    /// <summary>
    /// ��ת��λ
    /// </summary>
    public Toggle RightTurnLimit;
    /// <summary>
    /// ��ת����
    /// </summary>
    public Toggle RightTurnLimitExceed;
    /// <summary>
    /// ��ת������λ
    /// </summary>
    public Toggle RightTurnRestrictedZoneLimit;
    /// <summary>
    /// ��ת��ײ��λ
    /// </summary>
    public Toggle RightTurnCollisionPreventionLimit;

    /// <summary>
    /// ��ת������
    /// </summary>
    public Toggle RotaryOverTorque;
    /// <summary>
    /// ��ת��λ��λ
    /// </summary>
    public Toggle RotaryZeroPositionLimit;
    /// <summary>
    /// �ƶ�����բ��λ
    /// </summary>
    public Toggle BrakeReliefLimit;
    /// <summary>
    /// ��ת�����󻬶���
    /// </summary>
    public Toggle RotaryCentralLubricationBlockedOil;
    /// <summary>
    /// ��ת�����󻬵���λ
    /// </summary>
    public Toggle RotaryCentralLubricationLowOilLevel;

    public override void UpdateData(RotaryMechanismData data)
    {
        SetToggleState(MainCircuitBreaker, data.isMainCircuitBreaker);
        SetToggleState(FrequencyConverterRunning, data.isFrequencyConverterRunning);
        SetToggleState(BrakeOperating, data.isBrakeOperating);
        SetToggleState(FanOperating, data.isFanOperating);
        SetToggleState(LeftTurnOperating, data.isLeftTurnOperating);
        SetToggleState(RightTurnOperating, data.isRightTurnOperating);

        SetToggleState(FrequencyConverterFault, data.isFrequencyConverterFault);
        SetToggleState(BrakeOverload, data.isBrakeOverload);
        SetToggleState(FanOverload, data.isFanOverload);
        SetToggleState(BrakingResistorOverheating, data.isBrakingResistorOverheating);
        SetToggleState(RotaryFault, data.isRotaryFault);

        SetToggleState(LeftTurnLimit, data.isLeftTurnLimit);
        SetToggleState(LeftTurnLimitExceed, data.isLeftTurnLimitExceed);
        SetToggleState(LeftTurnRestrictedZoneLimit, data.isLeftTurnRestrictedZoneLimit);
        SetToggleState(LeftTurnCollisionPreventionLimit, data.isLeftTurnCollisionPreventionLimit);
        SetToggleState(RightTurnLimit, data.isRightTurnLimit);
        SetToggleState(RightTurnLimitExceed, data.isRightTurnLimitExceed);
        SetToggleState(RightTurnRestrictedZoneLimit, data.isRightTurnRestrictedZoneLimit);
        SetToggleState(RightTurnCollisionPreventionLimit, data.isRightTurnCollisionPreventionLimit);

        SetToggleState(RotaryOverTorque, data.isRotaryOverTorque);
        SetToggleState(RotaryZeroPositionLimit, data.isRotaryZeroPositionLimit);
        SetToggleState(BrakeReliefLimit, data.isBrakeReliefLimit);
        SetToggleState(RotaryCentralLubricationBlockedOil, data.isRotaryCentralLubricationBlockedOil);
        SetToggleState(RotaryCentralLubricationLowOilLevel, data.isRotaryCentralLubricationLowOilLevel);
    }


}
