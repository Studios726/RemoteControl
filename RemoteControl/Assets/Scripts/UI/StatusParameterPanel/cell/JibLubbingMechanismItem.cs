using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct JibLubbingMechanismData
{
    /// <summary>
    /// 主断路器
    /// </summary>
    public bool isMainBreakerToggle;
    /// <summary>
    /// 上仰运行
    /// </summary>
    public bool isFaceUpwardRunToggle;
    /// <summary>
    /// 下仰运行
    /// </summary>
    public bool isFaceDownRunToggle;
    /// <summary>
    /// 司机室前平衡阀
    /// </summary>
    public bool isDriverRoomForwordBalanceValveToggle;
    /// <summary>
    /// 司机室后平衡阀
    /// </summary>
    public bool isDriverRoomBackBalanceValveToggle;
    /// <summary>
    /// 主电机过载
    /// </summary>
    public bool isMainElectricalMachineryOverloadToggle;
    /// <summary>
    /// 上仰限位
    /// </summary>
    public bool isFaceUpwardLimitToggle;
    /// <summary>
    /// 下仰限位
    /// </summary>
    public bool isFaceDownLimitToggle;
    /// <summary>
    /// 下附限位
    /// </summary>
    public bool isBendDownLimitToggle;
    /// <summary>
    /// 下附极限
    /// </summary>
    public bool isBendDownMaxToggle;
    /// <summary>
    /// 下附禁区
    /// </summary>
    public bool isBendDownRestrictedZoneToggle;
    /// <summary>
    /// 加热器启动信号
    /// </summary>
    public bool isHeaterStartSignalToggle;
    /// <summary>
    /// 风机启动信号
    /// </summary>
    public bool isDraughtFanStartSignalToggle;
    /// <summary>
    /// 泵站高温报警
    /// </summary>
    public bool isPumpStationHighTemperatureWarningToggle;
    /// <summary>
    /// 油液位低信号
    /// </summary>
    public bool isOilLowSignalToggle;
    /// <summary>
    /// 液位超低信号
    /// </summary>
    public bool isOilUltralowSignalToggle;
    /// <summary>
    /// 泵站堵油信号
    /// </summary>
    public bool isPumpStationBlockUpOilSignalToggle;
    /// <summary>
    /// 主油泵运行
    /// </summary>
    public bool isMainOilPumpRunToggle;
    /// <summary>
    /// 风机运行
    /// </summary>
    public bool isDraughtFanRunToggle;
    /// <summary>
    /// 油加热器运行
    /// </summary>
    public bool isOilHeaterRunToggle;
    /// <summary>
    /// 上升电磁阀
    /// </summary>
    public bool isUpSolenoidValveToggle;
    /// <summary>
    /// 下降电磁阀
    /// </summary>
    public bool isDownSolenoidValveToggle;
    /// <summary>
    /// 升压电磁阀
    /// </summary>
    public bool isStepUpSolenoidValveToggle;
}
/// <summary>
/// 变幅机构
/// </summary>
public class JibLubbingMechanismItem :StatusParmItemBase<JibLubbingMechanismData>
{
    /// <summary>
    /// 主断路器
    /// </summary>
    public Toggle MainBreakerToggle;
    /// <summary>
    /// 上仰运行
    /// </summary>
    public Toggle FaceUpwardRunToggle;
    /// <summary>
    /// 下仰运行
    /// </summary>
    public Toggle FaceDownRunToggle;
    /// <summary>
    /// 司机室前平衡阀
    /// </summary>
    public Toggle DriverRoomForwordBalanceValveToggle;
    /// <summary>
    /// 司机室后平衡阀
    /// </summary>
    public Toggle DriverRoomBackBalanceValveToggle;
    /// <summary>
    /// 主电机过载
    /// </summary>
    public Toggle MainElectricalMachineryOverloadToggle;
    /// <summary>
    /// 上仰限位
    /// </summary>
    public Toggle FaceUpwardLimitToggle;
    /// <summary>
    /// 下仰限位
    /// </summary>
    public Toggle FaceDownLimitToggle;
    /// <summary>
    /// 下附限位
    /// </summary>
    public Toggle BendDownLimitToggle;
    /// <summary>
    /// 下附极限
    /// </summary>
    public Toggle BendDownMaxToggle;
    /// <summary>
    /// 下附禁区
    /// </summary>
    public Toggle BendDownRestrictedZoneToggle;
    /// <summary>
    /// 加热器启动信号
    /// </summary>
    public Toggle HeaterStartSignalToggle;
    /// <summary>
    /// 风机启动信号
    /// </summary>
    public Toggle DraughtFanStartSignalToggle;
    /// <summary>
    /// 泵站高温报警
    /// </summary>
    public Toggle PumpStationHighTemperatureWarningToggle;
    /// <summary>
    /// 油液位低信号
    /// </summary>
    public Toggle OilLowSignalToggle;
    /// <summary>
    /// 液位超低信号
    /// </summary>
    public Toggle OilUltralowSignalToggle;
    /// <summary>
    /// 泵站堵油信号
    /// </summary>
    public Toggle PumpStationBlockUpOilSignalToggle;
    /// <summary>
    /// 主油泵运行
    /// </summary>
    public Toggle MainOilPumpRunToggle;
    /// <summary>
    /// 风机运行
    /// </summary>
    public Toggle DraughtFanRunToggle;
    /// <summary>
    /// 油加热器运行
    /// </summary>
    public Toggle OilHeaterRunToggle;
    /// <summary>
    /// 上升电磁阀
    /// </summary>
    public Toggle UpSolenoidValveToggle;
    /// <summary>
    /// 下降电磁阀
    /// </summary>
    public Toggle DownSolenoidValveToggle;
    /// <summary>
    /// 升压电磁阀
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
