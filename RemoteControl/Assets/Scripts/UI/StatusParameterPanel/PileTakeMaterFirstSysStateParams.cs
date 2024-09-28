using ShenYangRemoteSystem.Subclass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileTakeMaterFirstSysStateParams : MonoBehaviour
{
    public SwitchOnItem SwitchOnItem;
    public ScramStopItem ScramStopItem;
    public CentralControlRoomItem CentralControlRoomItem;
    public BucketWheelCenterRoomSignalItem BucketWheelCenterRoomSignalItem;
    public PileTakeFlowStateItem PileTakeFlowStateItem;
    public OperatingModeItem OperatingModeItem;
    public AngleCurrentValueItem AngleCurrentValueItem;
    public JibLubbingMechanismItem JibLubbingMechanismItem;
    public RotaryMechanismItem RotaryMechanismItem;
    public CarMoveOrganizationItem CarMoveOrganizationItem;
    public Machine machine;
    SystemVariables systemVariables;
    public void UpdateData()
    {
        systemVariables = GameDataManager.Instance.SystemVariables;
        UpdateSwitchOnData(systemVariables);
        UpdateScramStop(systemVariables);
        UpdateCentralControlRoom(systemVariables);
        UpdateBucketWheelCenterRoomSignal(systemVariables);
        UpdatePileTakeFlowState(systemVariables);
        UpdateOperatingMode(systemVariables);
        UpdateAngleCurrentValue(systemVariables);
        UpdateJibLubbingMechanism(systemVariables);
        UpdateRotaryMechanism(systemVariables);
        UpdateCarMoveOrganization(systemVariables);
    }
    //合闸信息
    public void UpdateSwitchOnData(SystemVariables data)
    {
        SwitchOnData switchOnData = new SwitchOnData();
        switchOnData.isVacuumCircuitBreakerClosedToggle =
            GetToggleState(data.VacuumCircuitBreakerClosed, data.VacuumCircuitBreakerClosed_2);
        switchOnData.isLowVoltageControlPowerClosedToggle =
            GetToggleState(data.LowVoltageControlPowerClosed, data.LowVoltageControlPowerClosed_2);
        switchOnData.isLowVoltagePowerClosedToggle =
            GetToggleState(data.LowVoltagePowerClosed, data.LowVoltagePowerClosed_2);
        SwitchOnItem?.UpdateData(switchOnData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }
    //急停信息
    public void UpdateScramStop(SystemVariables data)
    {
        ScramStopData scramStopData = new ScramStopData();
        scramStopData.isDriverRoomEmergency =
            GetToggleState(data.DriverRoomEmergencyStopButton, data.DriverRoomEmergencyStopButton_2);
        scramStopData.isElectricalRoomEmergency =
            GetToggleState(data.ElectricalRoomEmergencyStopButton, data.ElectricalRoomEmergencyStopButton_2);
        scramStopData.isEmergencyStopRelay =
            GetToggleState(!data.EmergencyStopRelay, !data.EmergencyStopRelay_2);
        ScramStopItem?.UpdateData(scramStopData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }
    //与中控室连锁
    public void UpdateCentralControlRoom(SystemVariables data)
    {
        CentralControlData centralControlData = new CentralControlData();
        centralControlData.isUnlock = GetToggleState(!data.SystemInterlockSwitch, !data.SystemInterlockSwitch_2);
        centralControlData.isLock = GetToggleState(data.SystemInterlockSwitch, data.SystemInterlockSwitch_2);
        CentralControlRoomItem?.UpdateData(centralControlData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }
    //斗轮机与中控室信号
    public void UpdateBucketWheelCenterRoomSignal(SystemVariables data)
    {
        BucketWheelCenterRoomSignalData bucketWheelCenterRoomSignalData = new BucketWheelCenterRoomSignalData();
        bucketWheelCenterRoomSignalData.isAllowPileMaterSignal = GetToggleState(data.AllowBucketWheelMaterialLoading,
            data.AllowBucketWheelMaterialLoading_2);
        bucketWheelCenterRoomSignalData.isAllowTakeMaterSignal=GetToggleState(data.AllowBucketWheelMaterialUnloading,
            data.AllowBucketWheelMaterialUnloading_2);
        bucketWheelCenterRoomSignalData.isAllowShuntSignal=GetToggleState(data.AllowBucketWheelDiversion,
            data.AllowBucketWheelDiversion_2);
        bucketWheelCenterRoomSignalData.isLongDistanceCtrScramStop=GetToggleState(data.RemoteEmergencyStop,
            data.RemoteEmergencyStop_2);
        bucketWheelCenterRoomSignalData.isBucketWheelPileMaterRune=GetToggleState(data.BucketWheelMaterialLoadingRunning,
            data.BucketWheelMaterialLoadingRunning_2);
        bucketWheelCenterRoomSignalData.isBucketWheelTakeMaterRun=GetToggleState(data.BucketWheelMaterialUnloadingRunning,
            data.BucketWheelMaterialUnloadingRunning_2);
        bucketWheelCenterRoomSignalData.isBucketWheelShuntRun=GetToggleState(data.BucketWheelDiversionRunning,
            data.BucketWheelDiversionRunning_2);
        bucketWheelCenterRoomSignalData.isBucketWheelMalfunction=GetToggleState(data.BucketWheelFault,
            data.BucketWheelFault_2);
        BucketWheelCenterRoomSignalItem?.UpdateData(bucketWheelCenterRoomSignalData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }
    //堆取料流程状态
    public void UpdatePileTakeFlowState(SystemVariables data)
    {
        PileTakeFlowStateData pileTakeFlowStateData = new PileTakeFlowStateData();
        pileTakeFlowStateData.isSuspensoidPileMaterRunToggle = GetToggleState(
            data.SuspensionBeltMaterialLoadingRunningContact, data.SuspensionBeltMaterialLoadingRunningContact_2);
        pileTakeFlowStateData.isBafflePileMaterPos = GetToggleState(
            data.BaffleDownLimit, data.BaffleDownLimit_2);
        pileTakeFlowStateData.isBaffleShuntPos = GetToggleState(
            data.DiversionPlateLimit, data.DiversionPlateLimit_2);
        pileTakeFlowStateData.isFeedChannelPileMater = GetToggleState(
            data.BucketWheelSlotLiftLimit, data.BucketWheelSlotLiftLimit_2);
        pileTakeFlowStateData.isSuspensoidTakeMaterRun = GetToggleState(
            data.SuspensionBeltMaterialUnloadingRunningContact, data.SuspensionBeltMaterialUnloadingRunningContact_2);
        pileTakeFlowStateData.isBucketWheelRun = GetToggleState(
            data.BucketWheelMotorRunning, data.BucketWheelMotorRunning_2);
        pileTakeFlowStateData.isFeedChannelTake = GetToggleState(
            data.BucketWheelSlotLowerLimit, data.BucketWheelSlotLowerLimit_2);
        PileTakeFlowStateItem?.UpdateData(pileTakeFlowStateData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }
    //操作方式
    public void UpdateOperatingMode(SystemVariables data)
    {
        OperatingModeData operatingModeData = new OperatingModeData();
        operatingModeData.isSingleAction = GetToggleState(data.SingleAction, data.SingleAction_2);
        operatingModeData.isLinkageAction = GetToggleState(data.LinkAction, data.LinkAction_2);
        operatingModeData.isAutoAction = GetToggleState(data.Automatic, data.Automatic_2);
        operatingModeData.isLocalAction = GetToggleState(!data.Remote, !data.Remote_2);
        operatingModeData.isLongRangeAction = GetToggleState(data.Remote, data.Remote_2);
        OperatingModeItem?.UpdateData(operatingModeData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }
    
    public void UpdateAngleCurrentValue(SystemVariables data)
    {
        AngleCurrentValueData angleCurrentValueData = new AngleCurrentValueData();
        angleCurrentValueData.cabAngleStr = GetText("0","0");
        angleCurrentValueData.slewingAngleStr = GetText(data.SLEW_Angle.ToString("F2"),data.SLEW_Angle_2.ToString("F2"));
        angleCurrentValueData.pitchAngleStr = GetText(data.Luff_Angle.ToString("F2"),data.Luff_Angle_2.ToString("F2"));
        angleCurrentValueData.trolleyPositionStr = GetText(data.DC_Pos.ToString("F2"), data.DC_Pos_2.ToString("F2"));
        angleCurrentValueData.twoMachineDistanceStr = GetText("0","0");
        angleCurrentValueData.diversionBaffleStr = GetText("0","0");
        angleCurrentValueData.slewingCurrentStr = GetText(data.DC_Pos.ToString("F2"), data.DC_Pos_2.ToString("F2"));
        angleCurrentValueData.suspendedGelCurrentStr = GetText(data.SuspensionBeltElectricCurrent.ToString("F2"),data.SuspensionBeltElectricCurrent_2.ToString("F2"));
        angleCurrentValueData.trolleyCurrentStr = GetText(data.LargeCarElectricCurrent.ToString("F2"), data.LargeCarElectricCurrent_2.ToString("F2"));
        angleCurrentValueData.bucketWheelCurrentStr = GetText(data.BucketWheelElectricCurrent.ToString("F2"), data.BucketWheelElectricCurrent_2.ToString("F2"));
        AngleCurrentValueItem?.UpdateData(angleCurrentValueData);
    }
    //变幅机构
    public void UpdateJibLubbingMechanism(SystemVariables data)
    {
        JibLubbingMechanismData jibLubbingMechanismData = new JibLubbingMechanismData();
        jibLubbingMechanismData.isMainBreakerToggle=GetToggleState(data.VariableAmplitudeMainCircuitBreaker, data.VariableAmplitudeMainCircuitBreaker_2);
        jibLubbingMechanismData.isFaceUpwardRunToggle=GetToggleState(data.VariableAmplitudeUpperElectromagneticValveOpen, data.VariableAmplitudeUpperElectromagneticValveOpen_2);
        jibLubbingMechanismData.isFaceDownRunToggle=GetToggleState(data.VariableAmplitudeLowerElectromagneticValveOpen, data.VariableAmplitudeLowerElectromagneticValveOpen_2);
        jibLubbingMechanismData.isDriverRoomForwordBalanceValveToggle=GetToggleState(data.DriverRoomRiseValve, data.DriverRoomRiseValve_2);
        jibLubbingMechanismData.isDriverRoomBackBalanceValveToggle=GetToggleState(data.DriverRoomDescentValve, data.DriverRoomDescentValve_2);
        jibLubbingMechanismData.isMainElectricalMachineryOverloadToggle=GetToggleState(data.VariableAmplitudeMotorOverload, data.VariableAmplitudeMotorOverload_2);
        jibLubbingMechanismData.isFaceUpwardLimitToggle=GetToggleState(data.VariableAmplitudeUpperLimit, data.VariableAmplitudeUpperLimit_2);
       
        jibLubbingMechanismData.isVariableAmplitudeUpperExtremeLimitToggle=GetToggleState(data.VariableAmplitudeUpperExtremeLimit, data.VariableAmplitudeUpperExtremeLimit_2);
        jibLubbingMechanismData.isBendDownLimitToggle=GetToggleState(data.VariableAmplitudeLowerLimit, data.VariableAmplitudeLowerLimit_2);
        
        jibLubbingMechanismData.isBendDownMaxToggle=GetToggleState(data.VariableAmplitudeLowerExtremeLimit, data.VariableAmplitudeLowerExtremeLimit_2);
        jibLubbingMechanismData.isBendDownRestrictedZoneToggle=GetToggleState(data.VariableAmplitudeLowerForbiddenZoneLimit, data.VariableAmplitudeLowerForbiddenZoneLimit_2);
        jibLubbingMechanismData.isHeaterStartSignalToggle=GetToggleState(data.VariableAmplitudeOilHeaterStartup, data.VariableAmplitudeOilHeaterStartup_2);
        jibLubbingMechanismData.isDraughtFanStartSignalToggle=GetToggleState(data.VariableAmplitudeFanStartup, data.VariableAmplitudeFanStartup_2);
        jibLubbingMechanismData.isPumpStationHighTemperatureWarningToggle=GetToggleState(data.VariableAmplitudePumpStationOverheatAlarm, data.VariableAmplitudePumpStationOverheatAlarm_2);
        jibLubbingMechanismData.isOilLowSignalToggle=GetToggleState(data.VariableAmplitudeOilLevelVeryLowSignal, data.VariableAmplitudeOilLevelVeryLowSignal_2);
        jibLubbingMechanismData.isOilUltralowSignalToggle=GetToggleState(data.VariableAmplitudeOilLevelLowSignal, data.VariableAmplitudeOilLevelLowSignal_2);
        jibLubbingMechanismData.isPumpStationBlockUpOilSignalToggle=GetToggleState(data.VariableFrequencyOilBlockageSignal, data.VariableFrequencyOilBlockageSignal_2);
        jibLubbingMechanismData.isMainOilPumpRunToggle=GetToggleState(data.VariableAmplitudeOilPumpMotorRunning, data.VariableAmplitudeOilPumpMotorRunning_2);
        jibLubbingMechanismData.isDraughtFanRunToggle=GetToggleState(data.VariableAmplitudeFanRunning, data.VariableAmplitudeFanRunning_2);
        jibLubbingMechanismData.isOilHeaterRunToggle=GetToggleState(data.VariableAmplitudeOilHeaterRunning, data.VariableAmplitudeOilHeaterRunning_2);
        jibLubbingMechanismData.isUpSolenoidValveToggle=GetToggleState(data.VariableAmplitudeUpperElectromagneticValveOpen, data.VariableAmplitudeUpperElectromagneticValveOpen_2);
        jibLubbingMechanismData.isDownSolenoidValveToggle=GetToggleState(data.VariableAmplitudeLowerElectromagneticValveOpen, data.VariableAmplitudeLowerElectromagneticValveOpen_2);
        jibLubbingMechanismData.isStepUpSolenoidValveToggle=GetToggleState(data.VariableAmplitudeBoostValveOpen, data.VariableAmplitudeBoostValveOpen_2);
        JibLubbingMechanismItem?.UpdateData(jibLubbingMechanismData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }
    //回转机构
    public void UpdateRotaryMechanism(SystemVariables data)
    {
        RotaryMechanismData rotaryMechanismData = new RotaryMechanismData();
        rotaryMechanismData.isMainCircuitBreaker =
            GetToggleState(data.RotaryMainCircuitBreaker, data.RotaryMainCircuitBreaker_2);
        rotaryMechanismData.isFrequencyConverterRunning =
            GetToggleState(data.RotaryFrequencyConverterPowerOn, data.RotaryFrequencyConverterPowerOn_2);
        rotaryMechanismData.isBrakeOperating =
            GetToggleState(data.RotaryBrakeOpen, data.RotaryBrakeOpen_2);
        rotaryMechanismData.isFanOperating =
            GetToggleState(data.RotaryFanRunning, data.RotaryFanRunning_2);
        rotaryMechanismData.isLeftTurnOperating =
            GetToggleState(data.RotaryLeftTurnCommand, data.RotaryLeftTurnCommand_2);
        rotaryMechanismData.isRightTurnOperating =
            GetToggleState(data.RotaryRightTurnCommand, data.RotaryRightTurnCommand_2);
        rotaryMechanismData.isFrequencyConverterFault =
            GetToggleState(data.RotaryFrequencyConverterFault, data.RotaryFrequencyConverterFault_2);
        rotaryMechanismData.isBrakeOverload =
            GetToggleState(data.RotaryBrakeOverload, data.RotaryBrakeOverload_2);
        rotaryMechanismData.isFanOverload =
            GetToggleState(data.RotaryFanOverload, data.RotaryFanOverload_2);
        rotaryMechanismData.isBrakingResistorOverheating =
            GetToggleState(data.RotaryBrakeResistorOverheatSwitch, data.RotaryBrakeResistorOverheatSwitch_2);
        rotaryMechanismData.isRotaryFault =
            GetToggleState(data.RotaryFault, data.RotaryFault_2);
        rotaryMechanismData.isLeftTurnLimit =
            GetToggleState(data.RotaryLeftTurnLimit, data.RotaryLeftTurnLimit_2);
        rotaryMechanismData.isLeftTurnLimitExceed =
            GetToggleState(data.RotaryLeftTurnExtremeLimit, data.RotaryLeftTurnExtremeLimit_2);
        rotaryMechanismData.isLeftTurnRestrictedZoneLimit =
            GetToggleState(data.RotaryLeftTurnForbiddenZoneLimit, data.RotaryLeftTurnForbiddenZoneLimit_2);
        rotaryMechanismData.isLeftTurnCollisionPreventionLimit =
            GetToggleState(data.RotaryLeftTurnForbiddenLimit, data.RotaryLeftTurnForbiddenLimit_2);
        rotaryMechanismData.isRightTurnLimit =
            GetToggleState(data.RotaryRightTurnLimit, data.RotaryRightTurnLimit_2);
        rotaryMechanismData.isRightTurnLimitExceed =
            GetToggleState(data.RotaryRightTurnExtremeLimit, data.RotaryRightTurnExtremeLimit_2);
        rotaryMechanismData.isRightTurnRestrictedZoneLimit =
            GetToggleState(data.RotaryRightTurnForbiddenZoneLimit, data.RotaryRightTurnForbiddenZoneLimit_2);
        rotaryMechanismData.isRightTurnCollisionPreventionLimit =
            GetToggleState(data.RotaryRightTurnForbiddenLimit, data.RotaryRightTurnForbiddenLimit_2);
        rotaryMechanismData.isRotaryOverTorque =
            GetToggleState(data.RotaryOverTorque, data.RotaryOverTorque_2);
        rotaryMechanismData.isRotaryZeroPositionLimit =
            GetToggleState(data.RotaryZeroPositionLimit, data.RotaryZeroPositionLimit_2);
        rotaryMechanismData.isBrakeReliefLimit =
            GetToggleState(data.ReversalBrakeRelease, data.ReversalBrakeRelease_2);
        rotaryMechanismData.isRotaryCentralLubricationBlockedOil =
            GetToggleState(data.RotaryCentralizedLubricationOilBlockageFault, data.RotaryCentralizedLubricationOilBlockageFault_2);
        rotaryMechanismData.isRotaryCentralLubricationLowOilLevel =
            GetToggleState(data.RotaryCentralizedLubricationLowOilLevelFault, data.RotaryCentralizedLubricationLowOilLevelFault_2);
        RotaryMechanismItem?.UpdateData(rotaryMechanismData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }
    //大车行走机构
    public void UpdateCarMoveOrganization(SystemVariables data)
    {
        CarMoveOrganizationData carMoveOrganizationData = new CarMoveOrganizationData();
        carMoveOrganizationData.isMainCircuitBreakerToggle = machine==Machine.BucketWheelStackerReclaimer? data.LargeCarMainCircuitBreaker: data.LargeCarMainCircuitBreaker_2;
        carMoveOrganizationData.isMotorCircuitBreakerToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarMotorCircuitBreaker : data.LargeCarMotorCircuitBreaker_2;
        carMoveOrganizationData.isBrakeCircuitBreakersToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarBrakeCircuitBreaker : data.LargeCarBrakeCircuitBreaker_2;
        carMoveOrganizationData.isFrequencyConverterOperationToggle= machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarFrequencyConverterPowerOn : data.LargeCarFrequencyConverterPowerOn_2;
        carMoveOrganizationData.isBrakeOperationToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarBrakeOpen : data.LargeCarBrakeOpen_2;
        carMoveOrganizationData.isForwardOperationToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarForwardCommand : data.LargeCarForwardCommand_2;
        carMoveOrganizationData.isReverseOperationToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarReverseCommand : data.LargeCarReverseCommand_2;
        carMoveOrganizationData.isCraneFaultToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarFault : data.LargeCarFault_2;
        carMoveOrganizationData.isFrequencyConverterFaultToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarFrequencyConverterFault : data.LargeCarFrequencyConverterFault_2;
        carMoveOrganizationData.isBrakeFaultToggle = machine == Machine.BucketWheelStackerReclaimer ? !data.LargeCarBrakeResistorOverheatSwitch : !data.LargeCarBrakeResistorOverheatSwitch_2;
        carMoveOrganizationData.isLowOilLevelInBigVehicleCentralLubricationToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarCentralizedLubricationLowOilLevel : data.LargeCarCentralizedLubricationLowOilLevel_2;
        carMoveOrganizationData.isBlockedOilInBigVehicleCentralLubricationToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarCentralizedLubricationOilBlockage : data.LargeCarCentralizedLubricationOilBlockage_2;
        carMoveOrganizationData.isForwardLimitToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarForwardLimit : data.LargeCarForwardLimit_2;
        carMoveOrganizationData.isForwardLimitExceedToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarForwardExtremeLimit : data.LargeCarForwardExtremeLimit_2;
        carMoveOrganizationData.isReverseLimitToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarReverseLimit : data.LargeCarReverseLimit_2;
        carMoveOrganizationData.isReverseLimitExceedToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarReverseExtremeLimit : data.LargeCarReverseExtremeLimit_2;
        carMoveOrganizationData.isTwoMachineCollisionAlarmToggle= machine == Machine.BucketWheelStackerReclaimer ? data.TwoMachineCollisionAlarm : data.TwoMachineCollisionAlarm_2;
        CarMoveOrganizationItem?.UpdateData(carMoveOrganizationData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }

    public string GetText(string machine1Str, string machine2Str)
    {
        if (machine==Machine.BucketWheelStackerReclaimer)
        {
            return machine1Str;
        }else if (machine==Machine.BucketWheel)
        {
            return machine2Str;
        }
        else
        {
            return "";
        }
    }
    public bool GetToggleState(bool machine1,bool machine2)
    {
        if (machine==Machine.BucketWheelStackerReclaimer)
        {
            return machine1;
        }else if (machine==Machine.BucketWheel)
        {
            return machine2;
        }
        else
        {
            return false;
        }
    }
}
