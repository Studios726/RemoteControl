using System;
using System.Collections;
using System.Collections.Generic;
using ShenYangRemoteSystem.Subclass;
using UnityEngine;

public class PileTakeMaterSecondSysStateParams : MonoBehaviour
{
    public BucketWheelItem BucketWheelItem;
    public BucketWheelFeedChuteItem BucketWheelFeedChuteItem;
    public ShuntPlateItem ShuntPlateItem;
    public VibratorMotorItem VibratorMotorItem;
    public TailCarBeltDeviceItem TailCarBeltDeviceItem;
    public SunningWaterForDustPreventionItem SunningWaterForDustPreventionItem;
    public SuspendedColloidItem SuspendedColloidItem;
    public RailGrippingDeviceItem RailGrippingDeviceItem;
    public CableWindingDeviceItem CableWindingDeviceItem;
    public Machine machine;
    SystemVariables systemVariables;

    public void UpdateData()
    {
        systemVariables = GameDataManager.Instance.SystemVariables;
        UpdateBucketWheel(systemVariables);
        UpdateBucketWheelFeedChute(systemVariables);
        UpdateShuntPlate(systemVariables);
        UpdateVibratorMotor(systemVariables);
        UpdateTailCarBeltDevice(systemVariables);
        UpdateSunningWaterForDustPrevention(systemVariables);
        UpdateSuspendedColloidItem(systemVariables);
        UpdateRailGrippingDeviceItem(systemVariables);
        UpdateCableWindingDeviceItem(systemVariables);

    }

    //斗轮
    public void UpdateBucketWheel(SystemVariables data)
    {
        BucketWheelData bucketWheelData = new BucketWheelData();
        bucketWheelData.isBucketWheelRunning =
            GetToggleState(data.BucketWheelMotorRunning, data.BucketWheelMotorRunning_2);
        bucketWheelData.isMainCircuitBreaker =
            GetToggleState(data.BucketWheelMotorMainCircuitBreaker, data.BucketWheelMotorMainCircuitBreaker_2);
        bucketWheelData.isLubricationPumpRunning =
            GetToggleState(data.BucketWheelLubricationPumpRunning, data.BucketWheelLubricationPumpRunning_2);
        bucketWheelData.isMotorOverload =
            GetToggleState(data.BucketWheelMotorOverload, data.BucketWheelMotorOverload_2);
        bucketWheelData.isBucketWheelOverTorqueSwitch =
            GetToggleState(data.BucketWheelOverTorqueSwitch, data.BucketWheelOverTorqueSwitch_2);
        bucketWheelData.isLubricatingOilPumpFlowSwitch =
            GetToggleState(data.BucketWheelForcedLubricationFlowSwitch, data.BucketWheelForcedLubricationFlowSwitch_2);
        bucketWheelData.isBucketWheelCentralizedLubricationLowOilLevelAlarm = GetToggleState(
            data.BucketWheelCentralizedLubricationLowOilLevelAlarm,
            data.BucketWheelCentralizedLubricationLowOilLevelAlarm_2);
        bucketWheelData.isBucketWheelCentralizedLubricationOilBlockageAlarm = GetToggleState(
            data.BucketWheelCentralizedLubricationOilBlockageAlarm,
            data.BucketWheelCentralizedLubricationOilBlockageAlarm_2);
        BucketWheelItem?.UpdateData(bucketWheelData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }

    /// <summary>
    /// 斗轮导料槽
    /// </summary>
    /// <param name="data"></param>
    public void UpdateBucketWheelFeedChute(SystemVariables data)
    {
        BucketWheelFeedChuteData bucketWheelFeedChuteData = new BucketWheelFeedChuteData();
        bucketWheelFeedChuteData.isMainCircuitBreaker = GetToggleState(data.BucketWheelSlotMainCircuitBreaker,
            data.BucketWheelSlotMainCircuitBreaker_2);
        bucketWheelFeedChuteData.isMotorOverload = GetToggleState(data.BucketWheelMotorOverload,
            data.BucketWheelMotorOverload_2);
        bucketWheelFeedChuteData.isLiftLimit = GetToggleState(data.BucketWheelSlotLiftLimit,
            data.BucketWheelSlotLiftLimit_2);
        bucketWheelFeedChuteData.isLowerLimit = GetToggleState(data.BucketWheelSlotLowerLimit,
            data.BucketWheelSlotLowerLimit_2);
        bucketWheelFeedChuteData.isLiftRunning = GetToggleState(data.BucketWheelSlotLift,
            data.BucketWheelSlotLift_2);
        bucketWheelFeedChuteData.isLowerRunning = GetToggleState(data.BucketWheelSlotLower,
            data.BucketWheelSlotLower_2);
        BucketWheelFeedChuteItem?.UpdateData(bucketWheelFeedChuteData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }

    /// <summary>
    /// 分流挡板
    /// </summary>
    /// <param name="data"></param>
    public void UpdateShuntPlate(SystemVariables data)
    {
        ShuntPlateData shuntPlateData = new ShuntPlateData();
        shuntPlateData.isMainCircuitBreaker = GetToggleState(data.DiversionBaffleMainCircuitBreaker,
            data.DiversionBaffleMainCircuitBreaker_2);
        shuntPlateData.isMotorOverload = GetToggleState(data.DiversionPlateMotorOverload,
            data.DiversionPlateMotorOverload_2);
        shuntPlateData.isLiftLimit = GetToggleState(data.BaffleDownLimit,
            data.BaffleDownLimit_2);
        shuntPlateData.isLowerLimit = GetToggleState(data.BaffleUpLimit,
            data.BaffleUpLimit_2);
        shuntPlateData.isLiftRunning = GetToggleState(data.DiversionBaffleDownRunning,
            data.DiversionBaffleDownRunning_2);
        shuntPlateData.isLowerRunning = GetToggleState(data.DiversionBaffleUpRunning,
            data.DiversionBaffleUpRunning_2);
        shuntPlateData.isDiversionPlateTimeout =
            GetToggleState(data.DiversionPlateTimeout, data.DiversionPlateTimeout_2);
        ShuntPlateItem?.UpdateData(shuntPlateData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }

    /// <summary>
    /// 振打电机
    /// </summary>
    /// <param name="data"></param>
    public void UpdateVibratorMotor(SystemVariables data)
    {
        VibratorMotorData vibratorMotorData = new VibratorMotorData();
        vibratorMotorData.isVibrationMotorMainCircuitBreaker = GetToggleState(data.VibrationMotorMainCircuitBreaker,
            data.VibrationMotorMainCircuitBreaker_2);
        vibratorMotorData.isVibrationMotorOverload = GetToggleState(data.VibrationMotorOverload,
            data.VibrationMotorOverload_2);
        vibratorMotorData.isVibrationMotorRunning = GetToggleState(data.VibrationMotorRunning,
            data.VibrationMotorRunning_2);
        vibratorMotorData.isVibrationMotorFault = GetToggleState(data.VibrationMotorFault, data.VibrationMotorFault_2);
        VibratorMotorItem?.UpdateData(vibratorMotorData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }

    /// <summary>
    /// 尾车胶带
    /// </summary>
    /// <param name="data"></param>
    public void UpdateTailCarBeltDevice(SystemVariables data)
    {
        TailCarBeltDeviceData tailCarBeltDeviceData = new TailCarBeltDeviceData();
        tailCarBeltDeviceData.isBearingUpperLimitAlarm = GetToggleState(data.TailCarDrivenRollerBearingUpperLimitAlarm,
            data.TailCarDrivenRollerBearingUpperLimitAlarm_2);
        tailCarBeltDeviceData.isBearingLowerLimitAlarm = GetToggleState(data.TailCarDrivenRollerBearingLowerLimitAlarm,
            data.TailCarDrivenRollerBearingLowerLimitAlarm_2);
        tailCarBeltDeviceData.isLevelOneDeviation = GetToggleState(data.TailCarFirstLevelDeviationSwitch,
            data.TailCarFirstLevelDeviationSwitch_2);
        tailCarBeltDeviceData.isLevelTwoDeviation = GetToggleState(data.TailCarSecondLevelDeviationSwitch,
            data.TailCarSecondLevelDeviationSwitch_2);
        tailCarBeltDeviceData.isEmergencyStopCableSwitch = GetToggleState(data.TailCarEmergencyStopSwitch,
            data.TailCarEmergencyStopSwitch_2);
        //有疑问？？？ 陶
        tailCarBeltDeviceData.isLongitudinalTearSwitch = GetToggleState(data.TailCarBeltLongitudinalTearing,
            data.TailCarBeltLongitudinalTearing_2);
        TailCarBeltDeviceItem?.UpdateData(tailCarBeltDeviceData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }

    /// <summary>
    /// 晒水抑尘
    /// </summary>
    /// <param name="data"></param>
    public void UpdateSunningWaterForDustPrevention(SystemVariables data)
    {
        SunningWaterForDustPreventionData sunningWaterForDustPreventionData = new SunningWaterForDustPreventionData();
        sunningWaterForDustPreventionData.isDryFogSysAirPressureLow =
            GetToggleState(data.DryFogSystemLowAirPressure, data.DryFogSystemLowAirPressure_2);
        sunningWaterForDustPreventionData.isDryFogSysWaterPressureLow =
            GetToggleState(data.DryFogSystemLowWaterPressure, data.DryFogSystemLowWaterPressure_2);
        sunningWaterForDustPreventionData.isDryFogSysFilterClogged =
            GetToggleState(data.DryFogSystemFilterClogged, data.DryFogSystemFilterClogged_2);
        sunningWaterForDustPreventionData.isWaterTankLevelLowSwitch =
            GetToggleState(data.WaterTankLowLevelSwitch, data.WaterTankLowLevelSwitch_2);
        sunningWaterForDustPreventionData.isDryFogSysSprayStatus =
            GetToggleState(data.DryFogSystemSprayStatus, data.DryFogSystemSprayStatus_2);
        //有疑问 ？？ 陶
        sunningWaterForDustPreventionData.isDryFogSysSprayRunning =
            GetToggleState(data.DryFogSystemHeatRun, data.DryFogSystemHeatRun_2);
        sunningWaterForDustPreventionData.isDryFogSysAutoRunning =
            GetToggleState(data.DryFogSystemAutoRun, data.DryFogSystemAutoRun_2);
        sunningWaterForDustPreventionData.isDryFogSysManualRunning =
            GetToggleState(data.DryFogSystemManualRun, data.DryFogSystemManualRun_2);
        sunningWaterForDustPreventionData.isDryFogDustSuppressionRemoteStartRunning =
            GetToggleState(data.DryFogDustSuppressionRemoteStartRunning,
                data.DryFogDustSuppressionRemoteStartRunning_2);
        sunningWaterForDustPreventionData.isDryFogDustSuppressionRemoteStopRunning =
            GetToggleState(data.DryFogDustSuppressionRemoteStopRunning, data.DryFogDustSuppressionRemoteStopRunning_2);
        sunningWaterForDustPreventionData.isDryFogDustSuppressionStockpileRunning =
            GetToggleState(data.DryFogDustSuppressionStackingRunning, data.DryFogDustSuppressionStackingRunning_2);
        sunningWaterForDustPreventionData.isDryFogMaterialFetchingRunning =
            GetToggleState(data.DryFogDustSuppressionReclaimingRunning, data.DryFogDustSuppressionReclaimingRunning_2);
        sunningWaterForDustPreventionData.isDryFogDustSuppressionDiversionRunning =
            GetToggleState(data.DryFogDustSuppressionDiversionRunning, data.DryFogDustSuppressionDiversionRunning_2);
        SunningWaterForDustPreventionItem?.UpdateData(sunningWaterForDustPreventionData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }

    /// <summary>
    /// 悬胶
    /// </summary>
    /// <param name="data"></param>
    public void UpdateSuspendedColloidItem(SystemVariables data)
    {
        SuspendedColloidData suspendedColloidData = new SuspendedColloidData();
        suspendedColloidData.isMainCircuitBreaker = GetToggleState(data.SuspensionBeltMainCircuitBreaker,
            data.SuspensionBeltMainCircuitBreaker_2);
        suspendedColloidData.isMotorOverload =
            GetToggleState(data.SuspensionBeltMotorOverload, data.SuspensionBeltMotorOverload_2);
        suspendedColloidData.isBrakeOpen = GetToggleState(data.SuspensionBeltBrakeOpen, data.SuspensionBeltBrakeOpen_2);
        suspendedColloidData.isBrakeReleaseLimit =
            GetToggleState(data.SuspendedBeltBrakeRelease, data.SuspendedBeltBrakeRelease_2);
        suspendedColloidData.isStackingOperation = GetToggleState(data.SuspensionBeltMaterialLoadingRunningContact,
            data.SuspensionBeltMaterialLoadingRunningContact_2);
        suspendedColloidData.isFetchingOperation = GetToggleState(data.SuspensionBeltMaterialUnloadingRunningContact,
            data.SuspensionBeltMaterialUnloadingRunningContact_2);
        suspendedColloidData.isFirstLevelDeviationSwitch = GetToggleState(data.SuspensionBeltFirstLevelDeviationSwitch,
            data.SuspensionBeltFirstLevelDeviationSwitch_2);
        suspendedColloidData.isSecondLevelDeviationSwitch = GetToggleState(
            data.SuspensionBeltSecondLevelDeviationSwitch, data.SuspensionBeltSecondLevelDeviationSwitch_2);
        suspendedColloidData.isSlippingDetectionSwitch = GetToggleState(data.SuspendedBeltSlip,
            data.SuspendedBeltSlip_2);
        suspendedColloidData.isLongitudinalTearSwitch = GetToggleState(data.SuspensionBeltLongitudinalTearSwitch,
            data.SuspensionBeltLongitudinalTearSwitch_2);
        suspendedColloidData.isEmergencyStopCableSwitch = GetToggleState(data.SuspensionBeltEmergencyStopSwitch,
            data.SuspensionBeltEmergencyStopSwitch_2);
        suspendedColloidData.isMaterialFlowDetectionSwitch = GetToggleState(
            data.SuspensionBeltMaterialFlowDetectionSwitch, data.SuspensionBeltMaterialFlowDetectionSwitch_2);
        suspendedColloidData.isMiddleHopperCoalBlocking = GetToggleState(!data.CentralMaterialDustDetectionSwitch,
            !data.CentralMaterialDustDetectionSwitch_2);
        suspendedColloidData.isCentralHopperCloggedDetectionSwitch =
            GetToggleState(data.CentralHopperCloggedDetectionSwitch, data.CentralHopperCloggedDetectionSwitch_2);
        suspendedColloidData.isSuspensionBeltFault = GetToggleState(data.SuspensionBeltFault, data.SuspensionBeltFault_2);
        SuspendedColloidItem?.UpdateData(suspendedColloidData ,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }

    public void UpdateRailGrippingDeviceItem(SystemVariables data)
    {
        RailGrippingDeviceData railGrippingDeviceData = new RailGrippingDeviceData();
        railGrippingDeviceData.isMainCircuitBreaker =
            GetToggleState(data.ClampMainCircuitBreaker, data.ClampMainCircuitBreaker_2);
        railGrippingDeviceData.isLeftRailClamperMotorRunning =
            GetToggleState(data.LeftClampPumpRunning, data.LeftClampPumpRunning_2);
        railGrippingDeviceData.isLeftRailClamperSolenoidValve =
            GetToggleState(data.LeftClampElectromagneticValveOpen, data.LeftClampElectromagneticValveOpen_2);
        railGrippingDeviceData.isLeftAnchorLimit =
            GetToggleState(data.LeftAnchorLiftLimit, data.LeftAnchorLiftLimit_2);
        railGrippingDeviceData.isLeftRailClamperReleaseLimit =
            GetToggleState(data.LeftClampRelaxLimit, data.LeftClampRelaxLimit_2);
        railGrippingDeviceData.isMotorOverload =
            GetToggleState(data.ClampingDeviceMotorOverload, data.ClampingDeviceMotorOverload_2);
        railGrippingDeviceData.isRightRailClamperMotorRunning =
            GetToggleState(data.RightClampPumpRunning, data.RightClampPumpRunning_2);
        railGrippingDeviceData.isRightRailClamperSolenoidValve =
            GetToggleState(data.RightClampElectromagneticValveOpen, data.RightClampElectromagneticValveOpen_2);
        railGrippingDeviceData.isRightAnchorLimit =
            GetToggleState(data.RightAnchorLiftLimit, data.RightAnchorLiftLimit_2);
        railGrippingDeviceData.isRightRailClamperReleaseLimit =
            GetToggleState(data.RightClampRelaxLimit, data.RightClampRelaxLimit_2);
        railGrippingDeviceData.isClampFault = GetToggleState(data.ClampFault, data.ClampFault_2);
        RailGrippingDeviceItem?.UpdateData(railGrippingDeviceData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }

    /// <summary>
    /// 电缆卷筒
    /// </summary>
    /// <param name="data"></param>
    public void UpdateCableWindingDeviceItem(SystemVariables data)
    {
        CableWindingDeviceData cableWindingDeviceData = new CableWindingDeviceData();
        cableWindingDeviceData.isMainCircuitBreaker =
            GetToggleState(data.CableReelMainCircuitBreaker, data.CableReelMainCircuitBreaker_2);
        cableWindingDeviceData.isReelOverTightLimit1 =
            GetToggleState(!data.ReelOverTensionLimit1, !data.ReelOverTensionLimit1_2);
        cableWindingDeviceData.isReelOverLooseLimit1 =
            GetToggleState(data.ReelOverLooseLimit1, data.ReelOverLooseLimit1_2);
        cableWindingDeviceData.isReelEmptyDiskSwitch =
            GetToggleState(data.ReelEmptySwitch, data.ReelEmptySwitch_2);
        cableWindingDeviceData.isReelMiddleBrakeSwitch =
            GetToggleState(data.RollerMiddleSwitch, data.RollerMiddleSwitch_2);
        cableWindingDeviceData.isReelMotorOverload =
            GetToggleState(data.CableReelMotorOverload, data.CableReelMotorOverload_2);
        cableWindingDeviceData.isReelOverTightLimit2 =
            GetToggleState(data.RollerOverTightLimit2, data.RollerOverTightLimit2_2);
        cableWindingDeviceData.isReelOverLooseLimit2 =
            GetToggleState(data.RollerOverLooseLimit2, data.RollerOverLooseLimit2_2);
        cableWindingDeviceData.isReelFullDiskSwitch =
            GetToggleState(data.RollerFullDiskSwitch, data.RollerFullDiskSwitch_2);
        cableWindingDeviceData.isPowerReelRunning =
            GetToggleState(data.PowerRollerRunning, data.PowerRollerRunning_2);
        cableWindingDeviceData.isPowerReelCableOverLooseAlarm = GetToggleState(data.PowerReelCableOverLooseAlarm,
            data.PowerReelCableOverLooseAlarm_2);
        cableWindingDeviceData.isPowerReelCableOverTightAlarm = GetToggleState(data.PowerReelCableOverTightAlarm,
            data.PowerReelCableOverTightAlarm_2);
        CableWindingDeviceItem?.UpdateData(cableWindingDeviceData,GameDataManager.Instance.GameMain.connectionRC.isConnect);
    }

    public bool GetToggleState(bool machine1, bool machine2)
    {
        if (machine == Machine.BucketWheelStackerReclaimer)
        {
            return machine1;
        }
        else if (machine == Machine.BucketWheel)
        {
            return machine2;
        }
        else
        {
            return false;
        }
    }
}