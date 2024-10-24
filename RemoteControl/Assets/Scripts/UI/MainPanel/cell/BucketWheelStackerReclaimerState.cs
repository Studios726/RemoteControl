using ShenYangRemoteSystem.Subclass;

public class BucketWheelStackerReclaimerState : BucketWheelStateBase
{
   /// <summary>
   /// 允许堆料信号
   /// </summary>
   public ToggleDIY storeSpaceSignal;
   /// <summary>
   /// 堆料运行
   /// </summary>
   public ToggleDIY storeSpaceRun;
   /// <summary>
   /// 分流信号
   /// </summary>
   public ToggleDIY shuntSignal;
   /// <summary>
   /// 分流运行
   /// </summary>
   public ToggleDIY shuntRun;
   /// <summary>
   /// 悬胶堆料运行
   /// </summary>
   public ToggleDIY suspensoidPileMaterRun;
   /// <summary>
   /// 导料槽堆料位
   /// </summary>
   public ToggleDIY bucketWheelSlotLiftLimit;
   /// <summary>
   /// 挡板堆料位置
   /// </summary>
   public ToggleDIY baffleDownLimit;
   /// <summary>
   /// 挡板分流位置
   /// </summary>
   public ToggleDIY diversionPlateLimit;
   public override void UpdateData(SystemVariables data)
   {
        
        SetToggleState(storeSpaceSignal, data.AllowBucketWheelMaterialLoading,false,data.D1PLC1CommunicationState);
        SetToggleState(storeSpaceRun, data.BucketWheelMaterialLoadingRunning,false,data.D1PLC1CommunicationState);
        SetToggleState(shuntSignal, data.AllowBucketWheelDiversion,false,data.D1PLC1CommunicationState);
        SetToggleState(shuntRun, data.BucketWheelDiversionRunning,false,data.D1PLC1CommunicationState);
        
        SetToggleState(localControl,!data.Remote,false,data.D1PLC1CommunicationState);
        SetToggleState(lowVoltagePowerClosed,data.LowVoltagePowerClosed,false,data.D1PLC1CommunicationState);
        SetToggleState(remoteControl, data.Remote, false, data.D1PLC1CommunicationState);
        SetToggleState(powerSupplyClose, data.LowVoltageControlPowerClosed, false, data.D1PLC1CommunicationState);
        SetToggleState(systemChain, data.SystemInterlockSwitch, false, data.D1PLC1CommunicationState);
        // SetToggleState(recondition, data.SystemInterlockSwitch, false, data.D1PLC1CommunicationState);
        SetToggleState(bucketWheelMalfunction, data.BucketWheelFault, true, data.D1PLC1CommunicationState);
        SetToggleState(buzzerAlarm, data.StartAlarmStatus, true, data.D1PLC1CommunicationState);
        // SetToggleState(buzzerAlarm, data.BucketWheelFault, true, data.D1PLC1CommunicationState);
        SetToggleState(bucketWheelRun, data.BucketWheelMotorRunning, false, data.D1PLC1CommunicationState);
        SetToggleState(reclaimerSignal, data.AllowBucketWheelMaterialUnloading, false, data.D1PLC1CommunicationState);
        SetToggleState(reclaimerRun, data.BucketWheelMaterialUnloadingRunning, false, data.D1PLC1CommunicationState);
        SetToggleState(pitchingUp, data.VariableAmplitudeUpperElectromagneticValveOpen, false, data.D1PLC1CommunicationState);
        SetToggleState(pitchingDown, data.VariableAmplitudeLowerElectromagneticValveOpen, false, data.D1PLC1CommunicationState);
        SetToggleState(leftTurnRun, data.RotaryLeftTurnCommand, false, data.D1PLC1CommunicationState);
        SetToggleState(rightTurnRun, data.RotaryRightTurnCommand, false, data.D1PLC1CommunicationState);
        SetToggleState(backTurnRun, data.LargeCarReverseCommand, false, data.D1PLC1CommunicationState);
        SetToggleState(fowardTurnRun, data.LargeCarForwardCommand, false, data.D1PLC1CommunicationState);
        
        SetToggleState(leftSideRun, data.SLEW_Angle<0, false, data.D1PLC1CommunicationState);
        SetToggleState(rightSideRun, data.SLEW_Angle>0, false, data.D1PLC1CommunicationState);
        
        SetToggleState(suspensoidPileMaterRun, data.SuspensionBeltMaterialLoadingRunningContact, false, data.D1PLC1CommunicationState);
        SetToggleState(bucketWheelSlotLiftLimit, data.BucketWheelSlotLiftLimit, false, data.D1PLC1CommunicationState);
        SetToggleState(baffleDownLimit, data.BaffleDownLimit, false, data.D1PLC1CommunicationState);
        SetToggleState(diversionPlateLimit, data.DiversionPlateLimit, false, data.D1PLC1CommunicationState);
        SetToggleState(suspensoidTakeMaterRun, data.SuspensionBeltMaterialUnloadingRunningContact, false, data.D1PLC1CommunicationState);
        SetToggleState(bucketWheelSlotLowerLimit, data.BucketWheelSlotLowerLimit, false, data.D1PLC1CommunicationState);

   }
}
