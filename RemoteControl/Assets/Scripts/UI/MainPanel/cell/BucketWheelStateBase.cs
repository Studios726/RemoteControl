using ShenYangRemoteSystem.Subclass;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BucketWheelStateBase : MonoBehaviour
{
    /// <summary>
    /// 本地控制
    /// </summary>
    public ToggleDIY localControl;

    /// <summary>
    /// 远程控制
    /// </summary>
    public ToggleDIY remoteControl;

    /// <summary>
    /// 电源合闸
    /// </summary>
    public ToggleDIY powerSupplyClose;

    /// <summary>
    /// 动力电源
    /// </summary>
    public ToggleDIY lowVoltagePowerClosed;

    /// <summary>
    /// 与系统连锁
    /// </summary>
    public ToggleDIY systemChain;

    /// <summary>
    /// 通訊状态
    /// </summary>
    public ToggleDIY communicationStatus;

    /// <summary>
    /// 通訊状态 红色灯
    /// </summary>
    public GameObject red;

    /// <summary>
    /// 通訊状态 黄色灯
    /// </summary>
    public GameObject yellow;

    /// <summary>
    /// 检修
    /// </summary>
    public ToggleDIY recondition;

    /// <summary>
    /// 斗轮机故障
    /// </summary>
    public ToggleDIY bucketWheelMalfunction;

    /// <summary>
    /// 蜂鸣报警
    /// </summary>
    public ToggleDIY buzzerAlarm;

    /// <summary>
    /// 斗轮运行
    /// </summary>
    public ToggleDIY bucketWheelRun;

    /// <summary>
    /// 允许取料信号
    /// </summary>
    public ToggleDIY reclaimerSignal;

    /// <summary>
    /// 斗轮机取料运行
    /// </summary>
    public ToggleDIY reclaimerRun;

    /// <summary>
    /// 俯仰上俯
    /// </summary>
    public ToggleDIY pitchingUp;

    /// <summary>
    /// 俯仰下俯
    /// </summary>
    public ToggleDIY pitchingDown;

    /// <summary>
    /// 左转运行
    /// </summary>
    public ToggleDIY leftTurnRun;

    /// <summary>
    /// 右转运行
    /// </summary>
    public ToggleDIY rightTurnRun;

    /// <summary>
    /// 后退运行
    /// </summary>
    public ToggleDIY backTurnRun;

    /// <summary>
    /// 前进运行
    /// </summary>
    public ToggleDIY fowardTurnRun;

    /// <summary>
    /// 左侧运行
    /// </summary>
    public ToggleDIY leftSideRun;

    /// <summary>
    /// 右侧运行
    /// </summary>
    public ToggleDIY rightSideRun;
    /// <summary>
    /// 悬胶取料运行
    /// </summary>
    public ToggleDIY suspensoidTakeMaterRun;
    /// <summary>
    /// 导料槽取料位
    /// </summary>
    public ToggleDIY bucketWheelSlotLowerLimit;
    public float pastTime = 0;
    public Machine machine;

    public virtual void UpdateData(SystemVariables data)
    {
        //Debug.Log("更新参数状态");
        SetToggleState(localControl, !data.Remote_2, false, data.D1PLC1CommunicationState);
        SetToggleState(lowVoltagePowerClosed, data.LowVoltagePowerClosed_2, false, data.D1PLC1CommunicationState);
        SetToggleState(remoteControl, data.Remote_2, false, data.D1PLC1CommunicationState);
        SetToggleState(powerSupplyClose, data.LowVoltageControlPowerClosed_2, false, data.D1PLC1CommunicationState);
        SetToggleState(systemChain, data.SystemInterlockSwitch_2, false, data.D1PLC1CommunicationState);
        // SetToggleState(recondition, data.SystemInterlockSwitch, false, data.D1PLC1CommunicationState);
        SetToggleState(bucketWheelMalfunction, data.BucketWheelFault_2, true, data.D1PLC1CommunicationState);
        SetToggleState(buzzerAlarm, data.StartAlarmStatus_2, true, data.D1PLC1CommunicationState);
        // SetToggleState(buzzerAlarm, data.BucketWheelFault, true, data.D1PLC1CommunicationState);
        SetToggleState(bucketWheelRun, data.BucketWheelMotorRunning_2, false, data.D1PLC1CommunicationState);
        SetToggleState(reclaimerSignal, data.AllowBucketWheelMaterialUnloading_2, false, data.D1PLC1CommunicationState);
        SetToggleState(reclaimerRun, data.BucketWheelMaterialUnloadingRunning_2, false, data.D1PLC1CommunicationState);
        SetToggleState(pitchingUp, data.VariableAmplitudeUpperElectromagneticValveOpen_2, false,
            data.D1PLC1CommunicationState);
        SetToggleState(pitchingDown, data.VariableAmplitudeLowerElectromagneticValveOpen_2, false,
            data.D1PLC1CommunicationState);
        SetToggleState(leftTurnRun, data.RotaryLeftTurnCommand_2, false, data.D1PLC1CommunicationState);
        SetToggleState(rightTurnRun, data.RotaryRightTurnCommand_2, false, data.D1PLC1CommunicationState);
        SetToggleState(backTurnRun, data.LargeCarReverseCommand_2, false, data.D1PLC1CommunicationState);
        SetToggleState(fowardTurnRun, data.LargeCarForwardCommand_2, false, data.D1PLC1CommunicationState);
        SetToggleState(leftSideRun, data.SLEW_Angle_2<0, false, data.D1PLC1CommunicationState);
        SetToggleState(rightSideRun, data.SLEW_Angle_2>0, false, data.D1PLC1CommunicationState);
        
        SetToggleState(suspensoidTakeMaterRun, data.SuspensionBeltMaterialUnloadingRunningContact_2, false, data.D1PLC1CommunicationState);
        SetToggleState(bucketWheelSlotLowerLimit, data.BucketWheelSlotLowerLimit_2, false, data.D1PLC1CommunicationState);
    }

    public void Update()
    {
        if (GameDataManager.Instance.GetPlcConnection(machine) == false) //
        {
            if (communicationStatus.curState != 2)
            {
                ConnectionStatus(2);
            }
        }
        else
        {
            if (communicationStatus.curState != 1)
            {
                ConnectionStatus(1);
            }
        }

        if (GameDataManager.Instance.GetPlcConnection(machine) == true)
        {
            return;
        }

        pastTime = pastTime + Time.deltaTime;

        if (pastTime >= 1)
        {
            pastTime = 0;
            red.SetActive(!red.activeSelf);
            if (yellow.activeSelf == false)
            {
                yellow.SetActive(true);
            }
        }
    }

    public void ConnectionStatus(int isSucc)
    {
        communicationStatus.SetState(isSucc);
        red.SetActive(isSucc == 2);
        yellow.SetActive(isSucc == 2);
    }

    public virtual void SetToggleState(ToggleDIY toggle, bool ison, bool isFault = true, bool isConnect = true)
    {
        if (isConnect && ison)
        {
            if (ison)
            {
                if (isFault)
                {
                    toggle?.SetState(2);
                }
                else
                {
                    toggle?.SetState(1);
                }
            }
            else
            {
                if (isFault)
                {
                    toggle?.SetState(1);
                }
                else
                {
                    toggle?.SetState(2);
                }
            }
        }
        else
        {
            toggle?.SetState(0);
        }
    }
}