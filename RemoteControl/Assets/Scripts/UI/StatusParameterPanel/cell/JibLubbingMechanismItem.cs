using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct JibLubbingMechanismData
{
    /// <summary>
    /// ����·��
    /// </summary>
    public bool isMainBreakerToggle;
    /// <summary>
    /// ��������
    /// </summary>
    public bool isFaceUpwardRunToggle;
    /// <summary>
    /// ��������
    /// </summary>
    public bool isFaceDownRunToggle;
    /// <summary>
    /// ˾����ǰƽ�ⷧ
    /// </summary>
    public bool isDriverRoomForwordBalanceValveToggle;
    /// <summary>
    /// ˾���Һ�ƽ�ⷧ
    /// </summary>
    public bool isDriverRoomBackBalanceValveToggle;
    /// <summary>
    /// ���������
    /// </summary>
    public bool isMainElectricalMachineryOverloadToggle;
    /// <summary>
    /// ������λ
    /// </summary>
    public bool isFaceUpwardLimitToggle;
    /// <summary>
    /// ������λ
    /// </summary>
    public bool isFaceDownLimitToggle;
    /// <summary>
    /// �¸���λ
    /// </summary>
    public bool isBendDownLimitToggle;
    /// <summary>
    /// �¸�����
    /// </summary>
    public bool isBendDownMaxToggle;
    /// <summary>
    /// �¸�����
    /// </summary>
    public bool isBendDownRestrictedZoneToggle;
    /// <summary>
    /// �����������ź�
    /// </summary>
    public bool isHeaterStartSignalToggle;
    /// <summary>
    /// ��������ź�
    /// </summary>
    public bool isDraughtFanStartSignalToggle;
    /// <summary>
    /// ��վ���±���
    /// </summary>
    public bool isPumpStationHighTemperatureWarningToggle;
    /// <summary>
    /// ��Һλ���ź�
    /// </summary>
    public bool isOilLowSignalToggle;
    /// <summary>
    /// Һλ�����ź�
    /// </summary>
    public bool isOilUltralowSignalToggle;
    /// <summary>
    /// ��վ�����ź�
    /// </summary>
    public bool isPumpStationBlockUpOilSignalToggle;
    /// <summary>
    /// ���ͱ�����
    /// </summary>
    public bool isMainOilPumpRunToggle;
    /// <summary>
    /// �������
    /// </summary>
    public bool isDraughtFanRunToggle;
    /// <summary>
    /// �ͼ���������
    /// </summary>
    public bool isOilHeaterRunToggle;
    /// <summary>
    /// ������ŷ�
    /// </summary>
    public bool isUpSolenoidValveToggle;
    /// <summary>
    /// �½���ŷ�
    /// </summary>
    public bool isDownSolenoidValveToggle;
    /// <summary>
    /// ��ѹ��ŷ�
    /// </summary>
    public bool isStepUpSolenoidValveToggle;
}
/// <summary>
/// �������
/// </summary>
public class JibLubbingMechanismItem :StatusParmItemBase<JibLubbingMechanismData>
{
    /// <summary>
    /// ����·��
    /// </summary>
    public Toggle MainBreakerToggle;
    /// <summary>
    /// ��������
    /// </summary>
    public Toggle FaceUpwardRunToggle;
    /// <summary>
    /// ��������
    /// </summary>
    public Toggle FaceDownRunToggle;
    /// <summary>
    /// ˾����ǰƽ�ⷧ
    /// </summary>
    public Toggle DriverRoomForwordBalanceValveToggle;
    /// <summary>
    /// ˾���Һ�ƽ�ⷧ
    /// </summary>
    public Toggle DriverRoomBackBalanceValveToggle;
    /// <summary>
    /// ���������
    /// </summary>
    public Toggle MainElectricalMachineryOverloadToggle;
    /// <summary>
    /// ������λ
    /// </summary>
    public Toggle FaceUpwardLimitToggle;
    /// <summary>
    /// ������λ
    /// </summary>
    public Toggle FaceDownLimitToggle;
    /// <summary>
    /// �¸���λ
    /// </summary>
    public Toggle BendDownLimitToggle;
    /// <summary>
    /// �¸�����
    /// </summary>
    public Toggle BendDownMaxToggle;
    /// <summary>
    /// �¸�����
    /// </summary>
    public Toggle BendDownRestrictedZoneToggle;
    /// <summary>
    /// �����������ź�
    /// </summary>
    public Toggle HeaterStartSignalToggle;
    /// <summary>
    /// ��������ź�
    /// </summary>
    public Toggle DraughtFanStartSignalToggle;
    /// <summary>
    /// ��վ���±���
    /// </summary>
    public Toggle PumpStationHighTemperatureWarningToggle;
    /// <summary>
    /// ��Һλ���ź�
    /// </summary>
    public Toggle OilLowSignalToggle;
    /// <summary>
    /// Һλ�����ź�
    /// </summary>
    public Toggle OilUltralowSignalToggle;
    /// <summary>
    /// ��վ�����ź�
    /// </summary>
    public Toggle PumpStationBlockUpOilSignalToggle;
    /// <summary>
    /// ���ͱ�����
    /// </summary>
    public Toggle MainOilPumpRunToggle;
    /// <summary>
    /// �������
    /// </summary>
    public Toggle DraughtFanRunToggle;
    /// <summary>
    /// �ͼ���������
    /// </summary>
    public Toggle OilHeaterRunToggle;
    /// <summary>
    /// ������ŷ�
    /// </summary>
    public Toggle UpSolenoidValveToggle;
    /// <summary>
    /// �½���ŷ�
    /// </summary>
    public Toggle DownSolenoidValveToggle;
    /// <summary>
    /// ��ѹ��ŷ�
    /// </summary>
    public Toggle StepUpSolenoidValveToggle;
    public override void UpdateData(JibLubbingMechanismData data)
    {
        SetToggleState(MainBreakerToggle,data.isMainBreakerToggle);
        SetToggleState(FaceUpwardRunToggle, data.isFaceUpwardRunToggle);
        SetToggleState(FaceDownRunToggle, data.isFaceDownRunToggle);
        SetToggleState(DriverRoomForwordBalanceValveToggle, data.isDriverRoomForwordBalanceValveToggle);
        SetToggleState(DriverRoomBackBalanceValveToggle, data.isDriverRoomBackBalanceValveToggle);
        SetToggleState(MainElectricalMachineryOverloadToggle, data.isMainElectricalMachineryOverloadToggle);
        SetToggleState(FaceUpwardLimitToggle, data.isFaceUpwardLimitToggle);
        SetToggleState(FaceDownLimitToggle, data.isFaceDownLimitToggle);
        SetToggleState(BendDownLimitToggle, data.isBendDownLimitToggle);
        SetToggleState(BendDownMaxToggle, data.isBendDownMaxToggle);

        SetToggleState(BendDownRestrictedZoneToggle, data.isBendDownRestrictedZoneToggle);
        SetToggleState(HeaterStartSignalToggle, data.isHeaterStartSignalToggle);
        SetToggleState(DraughtFanStartSignalToggle, data.isDraughtFanStartSignalToggle);
        SetToggleState(PumpStationHighTemperatureWarningToggle, data.isPumpStationHighTemperatureWarningToggle);
        SetToggleState(OilLowSignalToggle, data.isOilLowSignalToggle);

        SetToggleState(OilUltralowSignalToggle, data.isOilUltralowSignalToggle);
        SetToggleState(PumpStationBlockUpOilSignalToggle, data.isPumpStationBlockUpOilSignalToggle);
        SetToggleState(MainOilPumpRunToggle, data.isMainOilPumpRunToggle);

        SetToggleState(DraughtFanRunToggle, data.isDraughtFanRunToggle);
        SetToggleState(OilHeaterRunToggle, data.isOilHeaterRunToggle);
        SetToggleState(UpSolenoidValveToggle, data.isUpSolenoidValveToggle);

        SetToggleState(DownSolenoidValveToggle, data.isDownSolenoidValveToggle);
        SetToggleState(StepUpSolenoidValveToggle, data.isStepUpSolenoidValveToggle);
    }


}
