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
    public void UpdateData()
    {
        
    }
    public void UpdateSwitchOnData(SwitchOnItem SwitchOnItem)
    {
        SwitchOnData switchOnData = new SwitchOnData();
        SwitchOnItem?.UpdateData(switchOnData);
    }
    public void UpdateScramStop()
    {
        ScramStopData scramStopData = new ScramStopData();
        ScramStopItem?.UpdateData(scramStopData);
    }
    public void UpdateCentralControlRoom()
    {
        CentralControlData centralControlData = new CentralControlData();
        CentralControlRoomItem?.UpdateData(centralControlData);
    }
    public void UpdateBucketWheelCenterRoomSignal()
    {
        BucketWheelCenterRoomSignalData bucketWheelCenterRoomSignalData = new BucketWheelCenterRoomSignalData();
        BucketWheelCenterRoomSignalItem?.UpdateData(bucketWheelCenterRoomSignalData);
    }
    public void UpdatePileTakeFlowState()
    {
        PileTakeFlowStateData pileTakeFlowStateData = new PileTakeFlowStateData();
        PileTakeFlowStateItem?.UpdateData(pileTakeFlowStateData);
    }
    public void UpdateOperatingMode()
    {
        OperatingModeData operatingModeData = new OperatingModeData();
        OperatingModeItem?.UpdateData(operatingModeData);
    }
    public void UpdateAngleCurrentValue()
    {
        AngleCurrentValueData angleCurrentValueData = new AngleCurrentValueData();
        AngleCurrentValueItem?.UpdateData(angleCurrentValueData);
    }
    public void UpdateJibLubbingMechanism()
    {
        JibLubbingMechanismData jibLubbingMechanismData = new JibLubbingMechanismData();
        JibLubbingMechanismItem?.UpdateData(jibLubbingMechanismData);
    }
    public void UpdateRotaryMechanism()
    {
        RotaryMechanismData rotaryMechanismData = new RotaryMechanismData();
        RotaryMechanismItem?.UpdateData(rotaryMechanismData);
    }
    public void UpdateCarMoveOrganization()
    {
        CarMoveOrganizationData carMoveOrganizationData = new CarMoveOrganizationData();
        CarMoveOrganizationItem?.UpdateData(carMoveOrganizationData);
    }
}
