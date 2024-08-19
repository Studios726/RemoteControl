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
    public ToggleDIY MainBreakerToggle;
    /// <summary>
    /// 上仰运行
    /// </summary>
    public ToggleDIY FaceUpwardRunToggle;
    /// <summary>
    /// 下仰运行
    /// </summary>
    public ToggleDIY FaceDownRunToggle;
    /// <summary>
    /// 司机室前平衡阀
    /// </summary>
    public ToggleDIY DriverRoomForwordBalanceValveToggle;
    /// <summary>
    /// 司机室后平衡阀
    /// </summary>
    public ToggleDIY DriverRoomBackBalanceValveToggle;
    /// <summary>
    /// 主电机过载
    /// </summary>
    public ToggleDIY MainElectricalMachineryOverloadToggle;
    /// <summary>
    /// 上仰限位
    /// </summary>
    public ToggleDIY FaceUpwardLimitToggle;
    /// <summary>
    /// 下仰限位
    /// </summary>
    public ToggleDIY FaceDownLimitToggle;
    /// <summary>
    /// 下附限位
    /// </summary>
    public ToggleDIY BendDownLimitToggle;
    /// <summary>
    /// 下附极限
    /// </summary>
    public ToggleDIY BendDownMaxToggle;
    /// <summary>
    /// 下附禁区
    /// </summary>
    public ToggleDIY BendDownRestrictedZoneToggle;
    /// <summary>
    /// 加热器启动信号
    /// </summary>
    public ToggleDIY HeaterStartSignalToggle;
    /// <summary>
    /// 风机启动信号
    /// </summary>
    public ToggleDIY DraughtFanStartSignalToggle;
    /// <summary>
    /// 泵站高温报警
    /// </summary>
    public ToggleDIY PumpStationHighTemperatureWarningToggle;
    /// <summary>
    /// 油液位低信号
    /// </summary>
    public ToggleDIY OilLowSignalToggle;
    /// <summary>
    /// 液位超低信号
    /// </summary>
    public ToggleDIY OilUltralowSignalToggle;
    /// <summary>
    /// 泵站堵油信号
    /// </summary>
    public ToggleDIY PumpStationBlockUpOilSignalToggle;
    /// <summary>
    /// 主油泵运行
    /// </summary>
    public ToggleDIY MainOilPumpRunToggle;
    /// <summary>
    /// 风机运行
    /// </summary>
    public ToggleDIY DraughtFanRunToggle;
    /// <summary>
    /// 油加热器运行
    /// </summary>
    public ToggleDIY OilHeaterRunToggle;
    /// <summary>
    /// 上升电磁阀
    /// </summary>
    public ToggleDIY UpSolenoidValveToggle;
    /// <summary>
    /// 下降电磁阀
    /// </summary>
    public ToggleDIY DownSolenoidValveToggle;
    /// <summary>
    /// 升压电磁阀
    /// </summary>
    public ToggleDIY StepUpSolenoidValveToggle;
    public override void UpdateData(JibLubbingMechanismData data)
    {
        bool isConnetction = GameDataManager.Instance.RcConnectionState;
        SetToggleState(MainBreakerToggle,data.isMainBreakerToggle,false,isConnetction);
        SetToggleState(FaceUpwardRunToggle, data.isFaceUpwardRunToggle,false,isConnetction);
        SetToggleState(FaceDownRunToggle, data.isFaceDownRunToggle,false,isConnetction);
        SetToggleState(DriverRoomForwordBalanceValveToggle, data.isDriverRoomForwordBalanceValveToggle,false,isConnetction);
        SetToggleState(DriverRoomBackBalanceValveToggle, data.isDriverRoomBackBalanceValveToggle,false,isConnetction);
        SetToggleState(MainElectricalMachineryOverloadToggle, data.isMainElectricalMachineryOverloadToggle,true,isConnetction);
        SetToggleState(FaceUpwardLimitToggle, data.isFaceUpwardLimitToggle,true,isConnetction);
        SetToggleState(FaceDownLimitToggle, data.isFaceDownLimitToggle,true,isConnetction);
        SetToggleState(BendDownLimitToggle, data.isBendDownLimitToggle,true,isConnetction);
        SetToggleState(BendDownMaxToggle, data.isBendDownMaxToggle,true,isConnetction);

        SetToggleState(BendDownRestrictedZoneToggle, data.isBendDownRestrictedZoneToggle,true,isConnetction);
        SetToggleState(HeaterStartSignalToggle, data.isHeaterStartSignalToggle,false,isConnetction);
        SetToggleState(DraughtFanStartSignalToggle, data.isDraughtFanStartSignalToggle,false,isConnetction);
        SetToggleState(PumpStationHighTemperatureWarningToggle, data.isPumpStationHighTemperatureWarningToggle,true,isConnetction);
        SetToggleState(OilLowSignalToggle, data.isOilLowSignalToggle,true,isConnetction);

        SetToggleState(OilUltralowSignalToggle, data.isOilUltralowSignalToggle,true,isConnetction);
        SetToggleState(PumpStationBlockUpOilSignalToggle, data.isPumpStationBlockUpOilSignalToggle,true,isConnetction);
        SetToggleState(MainOilPumpRunToggle, data.isMainOilPumpRunToggle,false,isConnetction);

        SetToggleState(DraughtFanRunToggle, data.isDraughtFanRunToggle,false,isConnetction);
        SetToggleState(OilHeaterRunToggle, data.isOilHeaterRunToggle,false,isConnetction);
        SetToggleState(UpSolenoidValveToggle, data.isUpSolenoidValveToggle,false,isConnetction);

        SetToggleState(DownSolenoidValveToggle, data.isDownSolenoidValveToggle,false,isConnetction);
        SetToggleState(StepUpSolenoidValveToggle, data.isStepUpSolenoidValveToggle,false,isConnetction);
    }


}
