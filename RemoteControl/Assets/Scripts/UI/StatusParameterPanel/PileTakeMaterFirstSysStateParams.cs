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
    public void UpdateSwitchOnData(SystemVariables data)
    {
        SwitchOnData switchOnData = new SwitchOnData();
        SwitchOnItem?.UpdateData(switchOnData);
    }
    public void UpdateScramStop(SystemVariables data)
    {
        ScramStopData scramStopData = new ScramStopData();
        ScramStopItem?.UpdateData(scramStopData);
    }
    public void UpdateCentralControlRoom(SystemVariables data)
    {
        CentralControlData centralControlData = new CentralControlData();
        CentralControlRoomItem?.UpdateData(centralControlData);
    }
    public void UpdateBucketWheelCenterRoomSignal(SystemVariables data)
    {
        BucketWheelCenterRoomSignalData bucketWheelCenterRoomSignalData = new BucketWheelCenterRoomSignalData();
        BucketWheelCenterRoomSignalItem?.UpdateData(bucketWheelCenterRoomSignalData);
    }
    public void UpdatePileTakeFlowState(SystemVariables data)
    {
        PileTakeFlowStateData pileTakeFlowStateData = new PileTakeFlowStateData();
        PileTakeFlowStateItem?.UpdateData(pileTakeFlowStateData);
    }
    public void UpdateOperatingMode(SystemVariables data)
    {
        OperatingModeData operatingModeData = new OperatingModeData();
        OperatingModeItem?.UpdateData(operatingModeData);
    }
    public void UpdateAngleCurrentValue(SystemVariables data)
    {
        AngleCurrentValueData angleCurrentValueData = new AngleCurrentValueData();
        AngleCurrentValueItem?.UpdateData(angleCurrentValueData);
    }
    public void UpdateJibLubbingMechanism(SystemVariables data)
    {
        JibLubbingMechanismData jibLubbingMechanismData = new JibLubbingMechanismData();
        JibLubbingMechanismItem?.UpdateData(jibLubbingMechanismData);
    }
    public void UpdateRotaryMechanism(SystemVariables data)
    {
        RotaryMechanismData rotaryMechanismData = new RotaryMechanismData();
        RotaryMechanismItem?.UpdateData(rotaryMechanismData);
    }
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
        //carMoveOrganizationData.isBrakeFaultToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarFrequencyConverterFault : data.LargeCarFrequencyConverterFault_2;
        carMoveOrganizationData.isLowOilLevelInBigVehicleCentralLubricationToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarCentralizedLubricationLowOilLevel : data.LargeCarCentralizedLubricationLowOilLevel_2;
        carMoveOrganizationData.isBlockedOilInBigVehicleCentralLubricationToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarCentralizedLubricationOilBlockage : data.LargeCarCentralizedLubricationOilBlockage_2;
        carMoveOrganizationData.isForwardLimitToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarForwardLimit : data.LargeCarForwardLimit_2;
        carMoveOrganizationData.isForwardLimitExceedToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarForwardExtremeLimit : data.LargeCarForwardExtremeLimit_2;
        carMoveOrganizationData.isReverseLimitToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarReverseLimit : data.LargeCarReverseLimit_2;
        carMoveOrganizationData.isReverseLimitExceedToggle = machine == Machine.BucketWheelStackerReclaimer ? data.LargeCarReverseExtremeLimit : data.LargeCarReverseExtremeLimit_2;
        CarMoveOrganizationItem?.UpdateData(carMoveOrganizationData);
    }
}
