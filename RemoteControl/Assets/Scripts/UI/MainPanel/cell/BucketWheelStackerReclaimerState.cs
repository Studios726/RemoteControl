using ShenYangRemoteSystem.Subclass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BucketWheelStackerReclaimerState : BucketWheelStateBase
{
   /// <summary>
   /// 堆料信号
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
        // SetToggleState(buzzerAlarm, data.BucketWheelFault, true, data.D1PLC1CommunicationState);
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
   }
}
