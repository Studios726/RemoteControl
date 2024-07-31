using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct RailGrippingDeviceData
{
    /// <summary>
    /// ����·��
    /// </summary>
    public bool isMainCircuitBreaker;
    /// <summary>
    /// ��й����������
    /// </summary>
    public bool isLeftRailClamperMotorRunning;
    /// <summary>
    /// ��й�����ŷ�
    /// </summary>
    public bool isLeftRailClamperSolenoidValve;
    /// <summary>
    /// ���ê����λ
    /// </summary>
    public bool isLeftAnchorLimit;
    /// <summary>
    /// ��й���������λ
    /// </summary>
    public bool isLeftRailClamperReleaseLimit;
    /// <summary>
    /// �������
    /// </summary>
    public bool isMotorOverload;
    /// <summary>
    /// �Ҽй����������
    /// </summary>
    public bool isRightRailClamperMotorRunning;
    /// <summary>
    /// �Ҽй�����ŷ�
    /// </summary>
    public bool isRightRailClamperSolenoidValve;
    /// <summary>
    /// �Ҳ�ê����λ
    /// </summary>
    public bool isRightAnchorLimit;
    /// <summary>
    /// �Ҽй���������λ
    /// </summary>
    public bool isRightRailClamperReleaseLimit;
}
public class RailGrippingDeviceItem : StatusParmItemBase<RailGrippingDeviceData>
{
    /// <summary>
    /// ����·��
    /// </summary>
    public Toggle MainCircuitBreaker;
    /// <summary>
    /// ��й����������
    /// </summary>
    public Toggle LeftRailClamperMotorRunning;
    /// <summary>
    /// ��й�����ŷ�
    /// </summary>
    public Toggle LeftRailClamperSolenoidValve;
    /// <summary>
    /// ���ê����λ
    /// </summary>
    public Toggle LeftAnchorLimit;
    /// <summary>
    /// ��й���������λ
    /// </summary>
    public Toggle LeftRailClamperReleaseLimit;
    /// <summary>
    /// �������
    /// </summary>
    public Toggle MotorOverload;
    /// <summary>
    /// �Ҽй����������
    /// </summary>
    public Toggle RightRailClamperMotorRunning;
    /// <summary>
    /// �Ҽй�����ŷ�
    /// </summary>
    public Toggle RightRailClamperSolenoidValve;
    /// <summary>
    /// �Ҳ�ê����λ
    /// </summary>
    public Toggle RightAnchorLimit;
    /// <summary>
    /// �Ҽй���������λ
    /// </summary>
    public Toggle RightRailClamperReleaseLimit;
    public override void UpdateData(RailGrippingDeviceData data)
    {
        SetToggleState(MainCircuitBreaker, data.isMainCircuitBreaker);
        SetToggleState(LeftRailClamperMotorRunning, data.isLeftRailClamperMotorRunning);
        SetToggleState(LeftRailClamperSolenoidValve, data.isLeftRailClamperSolenoidValve);
        SetToggleState(LeftAnchorLimit, data.isLeftAnchorLimit);
        SetToggleState(LeftRailClamperReleaseLimit, data.isLeftRailClamperReleaseLimit);
        SetToggleState(MotorOverload, data.isMotorOverload);
        SetToggleState(RightRailClamperMotorRunning, data.isRightRailClamperMotorRunning);
        SetToggleState(RightRailClamperSolenoidValve, data.isRightRailClamperSolenoidValve);
        SetToggleState(RightAnchorLimit, data.isRightAnchorLimit);
        SetToggleState(RightRailClamperReleaseLimit, data.isRightRailClamperReleaseLimit);
    }

}
