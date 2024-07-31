using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct CarMoveOrganizationData
{
    /// <summary>
    /// ����·��
    /// </summary>
    public bool isMainCircuitBreakerToggle;
    /// <summary>
    /// �����·��
    /// </summary>
    public bool isMotorCircuitBreakerToggle;
    /// <summary>
    /// �ƶ�����·��
    /// </summary>
    public bool isBrakeCircuitBreakersToggle;
    /// <summary>
    /// ��Ƶ������
    /// </summary>
    public bool isFrequencyConverterOperationToggle;
    /// <summary>
    /// �ƶ�������
    /// </summary>
    public bool isBrakeOperationToggle;
    /// <summary>
    /// ǰ������
    /// </summary>
    public bool isForwardOperationToggle;
    /// <summary>
    /// ��������
    /// </summary>
    public bool isReverseOperationToggle;

    /// <summary>
    /// �󳵹���
    /// </summary>
    public bool isCraneFaultToggle;
    /// <summary>
    /// ��Ƶ������
    /// </summary>
    public bool isFrequencyConverterFaultToggle;
    /// <summary>
    /// �ƶ�������
    /// </summary>
    public bool isBrakeFaultToggle;
    /// <summary>
    /// �󳵼����󻬵���λ��
    /// </summary>
    public bool isLowOilLevelInBigVehicleCentralLubricationToggle;
    /// <summary>
    /// �󳵼����󻬶���
    /// </summary>
    public bool isBlockedOilInBigVehicleCentralLubricationToggle;

    /// <summary>
    /// ǰ����λ
    /// </summary>
    public bool isForwardLimitToggle;
    /// <summary>
    /// ǰ������
    /// </summary>
    public bool isForwardLimitExceedToggle;
    /// <summary>
    /// ������λ
    /// </summary>
    public bool isReverseLimitToggle;
    /// <summary>
    /// ���˼���
    /// </summary>
    public bool isReverseLimitExceedToggle;
}
/// <summary>
/// �����߻���
/// </summary>
public class CarMoveOrganizationItem : StatusParmItemBase<CarMoveOrganizationData>
{
    /// <summary>
    /// ����·��
    /// </summary>
    public Toggle MainCircuitBreakerToggle;
    /// <summary>
    /// �����·��
    /// </summary>
    public Toggle MotorCircuitBreakerToggle;
    /// <summary>
    /// �ƶ�����·��
    /// </summary>
    public Toggle BrakeCircuitBreakersToggle;
    /// <summary>
    /// ��Ƶ������
    /// </summary>
    public Toggle FrequencyConverterOperationToggle;
    /// <summary>
    /// �ƶ�������
    /// </summary>
    public Toggle BrakeOperationToggle;
    /// <summary>
    /// ǰ������
    /// </summary>
    public Toggle ForwardOperationToggle;
    /// <summary>
    /// ��������
    /// </summary>
    public Toggle ReverseOperationToggle;

    /// <summary>
    /// �󳵹���
    /// </summary>
    public Toggle CraneFaultToggle;
    /// <summary>
    /// ��Ƶ������
    /// </summary>
    public Toggle FrequencyConverterFaultToggle;
    /// <summary>
    /// �ƶ�������
    /// </summary>
    public Toggle BrakeFaultToggle;
    /// <summary>
    /// �󳵼����󻬵���λ��
    /// </summary>
    public Toggle LowOilLevelInBigVehicleCentralLubricationToggle;
    /// <summary>
    /// �󳵼����󻬶���
    /// </summary>
    public Toggle BlockedOilInBigVehicleCentralLubricationToggle;

    /// <summary>
    /// ǰ����λ
    /// </summary>
    public Toggle ForwardLimitToggle;
    /// <summary>
    /// ǰ������
    /// </summary>
    public Toggle ForwardLimitExceedToggle;
    /// <summary>
    /// ������λ
    /// </summary>
    public Toggle ReverseLimitToggle;
    /// <summary>
    /// ���˼���
    /// </summary>
    public Toggle ReverseLimitExceedToggle;

    public override void UpdateData(CarMoveOrganizationData data)
    {
        SetToggleState(MainCircuitBreakerToggle,data.isMainCircuitBreakerToggle);
        SetToggleState(MotorCircuitBreakerToggle, data.isMotorCircuitBreakerToggle);
        SetToggleState(BrakeCircuitBreakersToggle, data.isBrakeCircuitBreakersToggle);
        SetToggleState(FrequencyConverterOperationToggle, data.isFrequencyConverterOperationToggle);
        SetToggleState(BrakeOperationToggle, data.isBrakeOperationToggle);
        SetToggleState(ForwardOperationToggle, data.isForwardOperationToggle);
        SetToggleState(ReverseOperationToggle, data.isReverseOperationToggle);

        SetToggleState(CraneFaultToggle, data.isCraneFaultToggle);
        SetToggleState(FrequencyConverterFaultToggle, data.isFrequencyConverterFaultToggle);
        SetToggleState(BrakeFaultToggle, data.isBrakeFaultToggle);
        SetToggleState(LowOilLevelInBigVehicleCentralLubricationToggle, data.isLowOilLevelInBigVehicleCentralLubricationToggle);
        SetToggleState(BlockedOilInBigVehicleCentralLubricationToggle, data.isBlockedOilInBigVehicleCentralLubricationToggle);

        SetToggleState(ForwardLimitToggle, data.isForwardLimitToggle);
        SetToggleState(ForwardLimitExceedToggle, data.isForwardLimitExceedToggle);
        SetToggleState(ReverseLimitToggle, data.isReverseLimitToggle);
        SetToggleState(ReverseLimitExceedToggle, data.isReverseLimitExceedToggle);

    }

}
