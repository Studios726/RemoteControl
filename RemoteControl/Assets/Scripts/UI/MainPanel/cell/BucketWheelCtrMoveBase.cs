using ShenYangRemoteSystem.Subclass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using ShangHaiPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Utility;

public class BucketWheelCtrMoveBase : MonoBehaviour
{
    public Button hideBtn;
    public GameObject show;
    public GameObject hide;

    /// <summary>
    /// 大车左前防碰撞
    /// </summary>
    public Text carLeftForwardDistance;

    /// <summary>
    /// 大车右前防碰撞
    /// </summary>
    public Text carRightForwardDistance;

    /// <summary>
    /// 大车左后防碰撞
    /// </summary>
    public Text carLeftBackDistance;

    /// <summary>
    /// 大车右后防碰撞
    /// </summary>
    public Text carRightBackDistance;

    /// <summary>
    /// 悬臂左前防碰撞
    /// </summary>
    public Text cantileverLeftForwardDistance;

    /// <summary>
    /// 悬臂右前防碰撞
    /// </summary>
    public Text cantileverRightForwardDistance;

    /// <summary>
    /// 悬臂左中方防碰撞
    /// </summary>
    public Text cantileverLeftCenterDistance;

    /// <summary>
    /// 悬臂右中方防碰撞
    /// </summary>
    public Text cantileverRightCenterDistance;

    /// <summary>
    /// 悬臂左后防碰撞
    /// </summary>
    public Text cantileverLeftBackDistance;

    /// <summary>
    /// 悬臂右后防碰撞
    /// </summary>
    public Text cantileverRightBackDistance;

    /// <summary>
    /// 悬臂流量
    /// </summary>
    public Text cantileverFlow;

    /// <summary>
    /// 悬臂料位高度
    /// </summary>
    public Text cantileverHeight;

    /// <summary>
    /// 当次取料量
    /// </summary>
    public Text thisTakeMater;

    /// <summary>
    /// 当天取料量
    /// </summary>
    public Text dayTakeMater;

    /// <summary>
    /// 尾车电流
    /// </summary>
    public Text carTailElectricity;

    /// <summary>
    /// 两机距离
    /// </summary>
    public Text distanceOfTwoCars;

    /// <summary>
    /// 悬臂皮带电流
    /// </summary>
    public Text cantileverBeltElectricity;

    /// <summary>
    /// 重置
    /// </summary>
    public Button takeResetBtn;

    /// <summary>
    /// 单动
    /// </summary>
    public ButtonCell aloneBtn;

    /// <summary>
    /// 联动
    /// </summary>
    public ButtonCell togetherBtn;

    /// <summary>
    /// 自动
    /// </summary>
    public ButtonCell automaticBtn;

    /// <summary>
    /// 取料
    /// </summary>
    public ButtonCell takeMaterBtn;

    /// <summary>
    /// 停止
    /// </summary>
    public ButtonCell stopTakeMaterBtn;

    /// <summary>
    /// 大车电流
    /// </summary>
    public Text carElectricity;

    /// <summary>
    /// 大车位置
    /// </summary>
    public Text carPos;

    /// <summary>
    /// 车快速
    /// </summary>
    public ButtonCell carFastBtn;

    /// <summary>
    /// 车慢速
    /// </summary>
    public ButtonCell carSlowBtn;

    /// <summary>
    /// 车停止
    /// </summary>
    public ButtonCell carStopBtn;

    /// <summary>
    /// 车后退
    /// </summary>
    public ButtonCell carBackBtn;

    /// <summary>
    /// 车前进
    /// </summary>
    public ButtonCell carForwardBtn;

    /// <summary>
    /// 斗轮电流
    /// </summary>
    public Text bucketWheelElectricity;

    /// <summary>
    /// 斗轮位置
    /// </summary>
    public Text bucketWheelPos;

    /// <summary>
    /// 回转电流
    /// </summary>
    public Text rotationElectricity;

    /// <summary>
    /// 回转角度
    /// </summary>
    public Text leftAngle;

    /// <summary>
    /// 回转角度
    /// </summary>
    public Text rotationAngle;

    /// <summary>
    /// 右转角度
    /// </summary>
    public Text rightAngle;

    /// <summary>
    /// 俯仰电流
    /// </summary>
    public Text upElectricity;

    /// <summary>
    /// 俯仰角度
    /// </summary>
    public Text upAngle;

    /// <summary>
    /// 俯仰角度
    /// </summary>
    public Text downAngle;

    /// <summary>
    /// 前进位置
    /// </summary>
    public Text forwardPos;

    /// <summary>
    /// 后退位置
    /// </summary>
    public Text backPos;

    /// <summary>
    /// 俯仰角度
    /// </summary>
    public Text pitchingAngle;

    /// <summary>
    /// 上仰
    /// </summary>
    public ButtonCell upBtn;

    /// <summary>
    /// 下附
    /// </summary>
    public ButtonCell downBtn;

    /// <summary>
    /// 左转
    /// </summary>
    public ButtonCell leftBtn;

    /// <summary>
    /// 右转
    /// </summary>
    public ButtonCell rightBtn;

    /// <summary>
    /// 回转停止
    /// </summary>
    public ButtonCell rotStopBtn;

    /// <summary>
    /// 俯仰停止
    /// </summary>
    public ButtonCell stopBtn;

    // <summary>
    /// 斗轮停止
    /// </summary>
    public ButtonCell bucketWheelStopBtn;

    /// <summary>
    /// 斗轮启动
    /// </summary>
    public ButtonCell bucketWheelStartBtn;

    /// <summary>
    /// 变幅油泵停止
    /// </summary>
    public ButtonCell oilPumpStopBtn;

    /// <summary>
    /// 变幅油泵启动
    /// </summary>
    public ButtonCell oilPumpStartBtn;

    /// <summary>
    /// 夹轨器夹紧
    /// </summary>
    public ButtonCell engageClampBtn;

    /// <summary>
    /// 夹轨器松开
    /// </summary>
    public ButtonCell disengageClampBtn;

    /// <summary>
    /// 振打器停止
    /// </summary>
    public ButtonCell shakerStopBtn;

    /// <summary>
    /// 振打器启动
    /// </summary>
    public ButtonCell shakerStartBtn;

    /// <summary>
    /// 升压电磁阀
    /// </summary>
    public ToggleDIY StepUpSolenoidValveToggle;
    
    /// <summary>
    /// 上仰角度object
    /// </summary>
    public GameObject UpAngleGameObject;

    /// <summary>
    /// 下仰角度object
    /// </summary>
    public GameObject DownAngelGameObject;

    /// <summary>
    /// 左转角度object
    /// </summary>
    public GameObject LeftAngleGameObject;

    /// <summary>
    /// 右转角度object
    /// </summary>
    public GameObject RightAangleGameObject;

    /// <summary>
    /// 前进位置object
    /// </summary>
    public GameObject ForwardPosGameObject;

    /// <summary>
    /// 后退位置object
    /// </summary>
    public GameObject BackPosGameObject;


    private ButtonCell curCtrMode;
    private ButtonCell curPileTakeMode;
    private ButtonCell curCarMoveMode;

    /// <summary>
    /// 当前车俯仰
    /// </summary>
    private ButtonCell curCarPitchingMode;

    private ButtonCell curCarRotMode;
    public Machine machine;
    public Color normalColor = new Color(1, 1, 1, 0.6f);
    public Color runColor = new Color(1, 0, 0.1803922f, 1);

    public virtual void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        // AddOnClickListener(takeResetBtn, (() =>SendMessageToServer("归零")));
        AddOnClickListener(aloneBtn,
            (() =>
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs("是否将控制方式切换为单动模式", null,
                        (() => { SendMessageToServer(COMMAND_NAME.MODE_A); })));
            }));
        AddOnClickListener(togetherBtn,
            (() =>
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs("是否将控制方式切换为联动模式", null,
                        (() => { SendMessageToServer(COMMAND_NAME.MODE_B); })));
            }));
        AddOnClickListener(automaticBtn,
            (() =>
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs("是否将控制方式切换为自动模式", null,
                        (() => { SendMessageToServer(COMMAND_NAME.MODE_C); })));
            }));
        AddOnClickListener(takeMaterBtn,
            (() =>
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs("是否将堆/取料控制切换为取料状态", null,
                        (() => { SendMessageToServer(COMMAND_NAME.BELTTAKE_BUTTON); })));
            }));
        AddOnClickListener(stopTakeMaterBtn,
            (() =>
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs("是否将堆/取料控制切换为停止状态", null,
                        (() => { SendMessageToServer(COMMAND_NAME.BELTSSTOP_BUTTON); })));
            }));
        AddOnClickListener(carFastBtn, (() => SendMessageToServer(COMMAND_NAME.TRAVEL_SPEED_FAST)));
        AddOnClickListener(carSlowBtn, (() => SendMessageToServer(COMMAND_NAME.TRAVEL_SPEED_SLOW)));
        AddOnClickListener(carBackBtn,
            (() =>
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs(GameDataManager.Instance.IsRailClampRelaxed(machine), null,
                        (() => { SendMessageToServer(COMMAND_NAME.MOVE_BACKWARD); })));
            }));
        AddOnClickListener(carStopBtn, (() => SendMessageToServer(COMMAND_NAME.MOVE_STOP)));
        AddOnClickListener(carForwardBtn,
            (() =>
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs(GameDataManager.Instance.IsRailClampRelaxed(machine), null,
                        (() => { SendMessageToServer(COMMAND_NAME.MOVE_FORWARD); })));
            }));
        AddOnClickListener(upBtn,
            (() =>
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs(GameDataManager.Instance.IsOilPumpStarted(machine), null,
                        (() => { SendMessageToServer(COMMAND_NAME.ELEVATE_UP); })));
            }));
        AddOnClickListener(downBtn,
            (() =>
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs(GameDataManager.Instance.IsOilPumpStarted(machine), null,
                        (() => { SendMessageToServer(COMMAND_NAME.ELEVATE_DOWN); })));
            }));
        AddOnClickListener(leftBtn, (() =>
                {
                    UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                        new ConfirmPanelArgs("是否确认左转？", null,
                            (() => { SendMessageToServer(COMMAND_NAME.ROTATE_LEFT); })));
                }
            ));
        AddOnClickListener(rightBtn, (() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("是否确认右转？", null,
                    (() => { SendMessageToServer(COMMAND_NAME.ROTATE_RIGHT); })));
        }));
        AddOnClickListener(rotStopBtn, (() =>
        {
            SendMessageToServer(COMMAND_NAME.ROTATE_STOP);
            SendMessageToServer(COMMAND_NAME.ELEVATE_STOP);
        }));
        AddOnClickListener(stopBtn, (() => SendMessageToServer(COMMAND_NAME.ELEVATE_STOP)));
        
        AddOnClickListener(bucketWheelStartBtn,(() =>SendMessageToServer(COMMAND_NAME.BUCKET_START,null) ));
        AddOnClickListener(bucketWheelStopBtn,(() =>SendMessageToServer(COMMAND_NAME.BUCKET_STOP,null) ));
        
        AddOnClickListener(oilPumpStartBtn,(() =>SendMessageToServer(COMMAND_NAME.OILBUMP_ON,null) ));
        AddOnClickListener(oilPumpStopBtn,(() =>SendMessageToServer(COMMAND_NAME.OILBUMP_OFF,null) ));
        
        AddOnClickListener(engageClampBtn,(() =>SendMessageToServer(COMMAND_NAME.RAIL_CLAMP,null) ));
        AddOnClickListener(disengageClampBtn,(() =>SendMessageToServer(COMMAND_NAME.RAIL_RELAX,null) ));
        
        AddOnClickListener(shakerStartBtn,(() =>SendMessageToServer(COMMAND_NAME.VIBRATOR_START,null) ));
        AddOnClickListener(shakerStopBtn,(() =>SendMessageToServer(COMMAND_NAME.VIBRATOR_STOP,null) ));
    }

    public virtual void UpdateData(SystemVariables data)
    {
        //Debug.Log("更新move  大车碰撞信息 ");
        if (machine == Machine.BucketWheelStackerReclaimer)
        {
            SetText(carElectricity, data.LargeCarElectricCurrent.ToString(), TextType.Electricity);
            SetText(rotationElectricity, data.RotaryElectricCurrent.ToString(), TextType.Electricity);
            SetText(bucketWheelElectricity, data.BucketWheelElectricCurrent.ToString(), TextType.Electricity);
            SetText(cantileverBeltElectricity, data.SuspensionBeltElectricCurrent.ToString(), TextType.Electricity);
            SetText(carPos, data.DC_Pos.ToString("F2"), TextType.Meter);
            SetText(rotationAngle, data.SLEW_Angle.ToString("F2"), TextType.Angle);
            SetText(leftAngle, data.SLEW_Angle.ToString("F2"), TextType.Angle);
            SetText(rightAngle, data.SLEW_Angle.ToString("F2"), TextType.Angle);
            SetText(upAngle, data.Luff_Angle.ToString("F2"), TextType.Angle);
            SetText(downAngle, data.Luff_Angle.ToString("F2"), TextType.Angle);
            SetText(pitchingAngle, data.Luff_Angle.ToString("F2"), TextType.Angle);
            SetText(cantileverHeight, data.XBTB_LWJ_VALUE.ToString("F2"), TextType.Meter);
            SetText(forwardPos, data.DC_Pos.ToString("F2"), TextType.Meter);
            SetText(backPos, data.DC_Pos.ToString("F2"), TextType.Meter);
            aloneBtn.SetSelectState(data.MODE == 0 && data.Single_Action == false, false);
            togetherBtn.SetSelectState(data.MODE == 1 && data.Link_Action == false, false);
            automaticBtn.SetSelectState(data.MODE == 2 && data.AUTO_MODE == false, false);
            aloneBtn.SetSystemState(data.Single_Action, true, false);
            togetherBtn.SetSystemState(data.Link_Action, true, false);
            automaticBtn.SetSystemState(data.AUTO_MODE, true, false);
            StepUpSolenoidValveToggle.SetState(data.VariableAmplitudeBoostValveOpen?1:0);
            
            bucketWheelStartBtn.SetSystemState(data.BucketWheelMotorRunning,true);
            bucketWheelStopBtn.SetSystemState(data.BucketWheelMotorRunning==false,true);
            
            oilPumpStopBtn.SetSystemState(data.VariableAmplitudeOilPumpMotorRunning==false,true);
            oilPumpStartBtn.SetSystemState(data.VariableAmplitudeOilPumpMotorRunning,true);
            
            shakerStopBtn.SetSystemState(data.VibrationMotorRunning==false,true);
            shakerStartBtn.SetSystemState(data.VibrationMotorRunning,true);
            
            if (data.LeftClampRelaxLimit==true && data.RightClampRelaxLimit==true)
            {
                disengageClampBtn.SetSystemState(true,true);
                engageClampBtn.SetSystemState(false,true);
            }
            else
            {
                engageClampBtn.SetSystemState(true,true);
                disengageClampBtn.SetSystemState(false,true);
            }
            
            // if (stopTakeMaterBtn.red.activeSelf&&data.SR1_BeltTS_Stop_Swicth==false&&data.SR1_BeltTake_Swicth==true &&data.SuspensionGlueRunCommand)
            // {
            //     PileTakeMaterPop(TaskType.TAKEMATER,data.BeltRealyDis);
            // }
            takeMaterBtn.SetSystemState(data.SR1_BeltTake_Swicth, true, false);
            stopTakeMaterBtn.SetSystemState(data.SR1_BeltTS_Stop_Swicth, true, false);
            upBtn.SetSystemState(data.VariableAmplitudeUpperElectromagneticValveOpen);
            upBtn.SetTextColor(data.VariableAmplitudeUpperElectromagneticValveOpen ? runColor : normalColor);
            if (UpAngleGameObject.activeSelf != data.VariableAmplitudeUpperElectromagneticValveOpen)
            {
                UpAngleGameObject.SetActive(data.VariableAmplitudeUpperElectromagneticValveOpen);
            }

            if (DownAngelGameObject.activeSelf != data.VariableAmplitudeLowerElectromagneticValveOpen)
            {
                DownAngelGameObject.SetActive(data.VariableAmplitudeLowerElectromagneticValveOpen);
            }

            downBtn.SetSystemState(data.VariableAmplitudeLowerElectromagneticValveOpen);
            downBtn.SetTextColor(data.VariableAmplitudeLowerElectromagneticValveOpen ? runColor : normalColor);
            if (data.LargeCarForwardCommand == false && data.LargeCarReverseCommand == false)
            {
                carStopBtn.SetRedAlpha(1);
                carStopBtn.SetSystemState(true);
            }
            else
            {
                carStopBtn.SetSystemState(false);
            }

            if (data.VariableAmplitudeUpperElectromagneticValveOpen == false &&
                data.VariableAmplitudeLowerElectromagneticValveOpen == false)
            {
                stopBtn.SetSystemState(true);
            }
            else
            {
                stopBtn.SetSystemState(false);
            }

            if (data.RotaryLeftTurnCommand == false && data.RotaryRightTurnCommand == false &&
                data.VariableAmplitudeUpperElectromagneticValveOpen == false &&
                data.VariableAmplitudeLowerElectromagneticValveOpen == false)
            {
                rotStopBtn.SetSystemState(true);
                rotStopBtn.SetRedAlpha(1);
                stopBtn.SetSystemState(true);
            }
            else
            {
                rotStopBtn.SetSystemState(false);
                stopBtn.SetSystemState(false);
            }

            leftBtn.SetSystemState(data.RotaryLeftTurnCommand);
            leftBtn.SetTextColor(data.RotaryLeftTurnCommand ? runColor : normalColor);
            if (LeftAngleGameObject.activeSelf != data.RotaryLeftTurnCommand)
            {
                LeftAngleGameObject.SetActive(data.RotaryLeftTurnCommand);
            }

            if (RightAangleGameObject.activeSelf != data.RotaryRightTurnCommand)
            {
                RightAangleGameObject.SetActive(data.RotaryRightTurnCommand);
            }

            rightBtn.SetSystemState(data.RotaryRightTurnCommand);
            rightBtn.SetTextColor(data.RotaryRightTurnCommand ? runColor : normalColor);
            carBackBtn.SetSystemState(data.LargeCarReverseCommand);
            carBackBtn.SetTextColor(data.LargeCarReverseCommand ? runColor : normalColor);
            carForwardBtn.SetSystemState(data.LargeCarForwardCommand);
            carForwardBtn.SetTextColor(data.LargeCarForwardCommand ? runColor : normalColor);
            ForwardPosGameObject.SetActive(data.LargeCarForwardCommand);
            BackPosGameObject.SetActive(data.LargeCarReverseCommand);
            carSlowBtn.SetSystemState(data.SR1_Travel_Speed_SB == false, true);
            carFastBtn.SetSystemState(data.SR1_Travel_Speed_SB, true);
            float x1 = 40 * Mathf.Cos(Mathf.Abs(data.Luff_Angle) * Mathf.Deg2Rad);
            string x = (53.4 + data.DC_Pos + (x1 * Mathf.Cos(data.SLEW_Angle * Mathf.Deg2Rad))).ToString("F2");
            string y = (40 * Mathf.Sin(data.SLEW_Angle * Mathf.Deg2Rad) - 1.8F).ToString("F2");
            bucketWheelPos.text = $"({x} , {y})";

            FlowMeter_data flowMeterData=GameDataManager.Instance.GetFlowMeterData(machine);
            if (flowMeterData != null)//更新流量
            {
                SetText(thisTakeMater, flowMeterData.Once_extra_weight.ToString(), TextType.Tonne);
                SetText(dayTakeMater, flowMeterData.Oneday_extra_weight.ToString(), TextType.Tonne);
                SetText(cantileverFlow, flowMeterData.FlowRealtime.ToString(), TextType.TonneHour);
            }
            else
            {
                SetText(thisTakeMater, "0", TextType.Tonne);
                SetText(dayTakeMater, "0", TextType.Tonne);
                SetText(cantileverFlow, "0", TextType.TonneHour);
            }
        }
        else
        {
            SetText(carElectricity, data.LargeCarElectricCurrent_2.ToString(), TextType.Electricity);
            SetText(rotationElectricity, data.RotaryElectricCurrent_2.ToString(), TextType.Electricity);
            SetText(bucketWheelElectricity, data.BucketWheelElectricCurrent_2.ToString(), TextType.Electricity);
            SetText(cantileverBeltElectricity, data.SuspensionBeltElectricCurrent_2.ToString(), TextType.Electricity);
            SetText(carPos, data.DC_Pos_2.ToString("F2"), TextType.Meter);
            SetText(rotationAngle, data.SLEW_Angle_2.ToString("F2"), TextType.Angle);
            SetText(leftAngle, data.SLEW_Angle_2.ToString("F2"), TextType.Angle);
            SetText(rightAngle, data.SLEW_Angle_2.ToString("F2"), TextType.Angle);
            SetText(upAngle, data.Luff_Angle_2.ToString("F2"), TextType.Angle);
            SetText(downAngle, data.Luff_Angle_2.ToString("F2"), TextType.Angle);
            SetText(forwardPos, data.DC_Pos_2.ToString("F2"), TextType.Meter);
            SetText(backPos, data.DC_Pos_2.ToString("F2"), TextType.Meter);
            SetText(pitchingAngle, data.Luff_Angle_2.ToString("F2"), TextType.Angle);
            SetText(cantileverHeight, data.XBTB_LWJ_VALUE_2.ToString("F2"), TextType.Meter);
            aloneBtn.SetSelectState(data.MODE_2 == 0 && data.Single_Action_2 == false, false);
            togetherBtn.SetSelectState(data.MODE_2 == 1 && data.Link_Action_2 == false, false);
            automaticBtn.SetSelectState(data.MODE_2 == 2 && data.AUTO_MODE_2 == false, false);
            aloneBtn.SetSystemState(data.Single_Action_2, true, false);
            togetherBtn.SetSystemState(data.Link_Action_2, true, false);
            automaticBtn.SetSystemState(data.AUTO_MODE_2, true, false);
            StepUpSolenoidValveToggle.SetState(data.VariableAmplitudeBoostValveOpen_2?1:0);
            
            bucketWheelStartBtn.SetSystemState(data.BucketWheelMotorRunning_2,true);
            bucketWheelStopBtn.SetSystemState(data.BucketWheelMotorRunning_2==false,true);
            
            oilPumpStopBtn.SetSystemState(data.VariableAmplitudeOilPumpMotorRunning_2==false,true);
            oilPumpStartBtn.SetSystemState(data.VariableAmplitudeOilPumpMotorRunning_2,true);
            
            shakerStopBtn.SetSystemState(data.VibrationMotorRunning_2==false,true);
            shakerStartBtn.SetSystemState(data.VibrationMotorRunning_2,true);
            
            if (data.LeftClampRelaxLimit_2==true && data.RightClampRelaxLimit_2==true)
            {
                disengageClampBtn.SetSystemState(true,true);
                engageClampBtn.SetSystemState(false,true);
            }
            else
            {
                engageClampBtn.SetSystemState(true,true);
                disengageClampBtn.SetSystemState(false,true);
            }
            
            takeMaterBtn.SetSystemState(data.SR1_BeltTake_Swicth_2, true, false);
            stopTakeMaterBtn.SetSystemState(data.SR1_BeltTS_Stop_Swicth_2, true, false);
            upBtn.SetSystemState(data.VariableAmplitudeUpperElectromagneticValveOpen_2);
            if (UpAngleGameObject.activeSelf != data.VariableAmplitudeUpperElectromagneticValveOpen_2)
            {
                UpAngleGameObject.SetActive(data.VariableAmplitudeUpperElectromagneticValveOpen_2);
            }

            upBtn.SetTextColor(data.VariableAmplitudeUpperElectromagneticValveOpen_2 ? runColor : normalColor);
            if (DownAngelGameObject.activeSelf != data.VariableAmplitudeLowerElectromagneticValveOpen_2)
            {
                DownAngelGameObject.SetActive(data.VariableAmplitudeLowerElectromagneticValveOpen_2);
            }

            downBtn.SetSystemState(data.VariableAmplitudeLowerElectromagneticValveOpen_2);
            downBtn.SetTextColor(data.VariableAmplitudeLowerElectromagneticValveOpen_2 ? runColor : normalColor);
            leftBtn.SetSystemState(data.RotaryLeftTurnCommand_2);
            leftBtn.SetTextColor(data.RotaryLeftTurnCommand_2 ? runColor : normalColor);

            if (LeftAngleGameObject.activeSelf != data.RotaryLeftTurnCommand_2)
            {
                LeftAngleGameObject.SetActive(data.RotaryLeftTurnCommand_2);
            }

            if (RightAangleGameObject.activeSelf != data.RotaryRightTurnCommand_2)
            {
                RightAangleGameObject.SetActive(data.RotaryRightTurnCommand_2);
            }

            rightBtn.SetSystemState(data.RotaryRightTurnCommand_2);
            rightBtn.SetTextColor(data.RotaryRightTurnCommand_2 ? runColor : normalColor);
            carBackBtn.SetSystemState(data.LargeCarReverseCommand_2);
            carBackBtn.SetTextColor(data.LargeCarReverseCommand_2 ? runColor : normalColor);
            carForwardBtn.SetSystemState(data.LargeCarForwardCommand_2);
            carForwardBtn.SetTextColor(data.LargeCarForwardCommand_2 ? runColor : normalColor);

            ForwardPosGameObject.SetActive(data.LargeCarForwardCommand_2);
            BackPosGameObject.SetActive(data.LargeCarReverseCommand_2);

            carSlowBtn.SetSystemState(data.SR1_Travel_Speed_SB_2 == false, true);
            carFastBtn.SetSystemState(data.SR1_Travel_Speed_SB_2, true);
            if (data.LargeCarForwardCommand_2 == false && data.LargeCarReverseCommand_2 == false)
            {
                carStopBtn.SetRedAlpha(1);
                carStopBtn.SetSystemState(true);
            }
            else
            {
                carStopBtn.SetSystemState(false);
            }

            if (data.VariableAmplitudeUpperElectromagneticValveOpen_2 == false &&
                data.VariableAmplitudeLowerElectromagneticValveOpen_2 == false)
            {
                stopBtn.SetSystemState(true);
            }
            else
            {
                stopBtn.SetSystemState(false);
            }

            if (data.RotaryLeftTurnCommand_2 == false && data.RotaryRightTurnCommand_2 == false)
            {
              
                rotStopBtn.SetSystemState(true);
                rotStopBtn.SetRedAlpha(1);
            }
            else
            {
                rotStopBtn.SetSystemState(false);
            }

            float x1 = 40 * Mathf.Cos(Mathf.Abs(data.Luff_Angle_2) * Mathf.Deg2Rad);
            string x = (117.74 + data.DC_Pos_2 + (x1 * Mathf.Cos(data.SLEW_Angle_2 * Mathf.Deg2Rad))).ToString("F2");
            string y = (40 * Mathf.Sin(data.SLEW_Angle_2 * Mathf.Deg2Rad) + 1.7).ToString("F2");

            bucketWheelPos.text = $"({x} , {y})";
            FlowMeter_data flowMeterData=GameDataManager.Instance.GetFlowMeterData(machine);
            if (flowMeterData != null)//更新流量
            {
                SetText(thisTakeMater, flowMeterData.Once_extra_weight.ToString(), TextType.Tonne);
                SetText(dayTakeMater, flowMeterData.Oneday_extra_weight.ToString(), TextType.Tonne);
                SetText(cantileverFlow, flowMeterData.FlowRealtime.ToString(), TextType.TonneHour);
            }
            else
            {
                SetText(thisTakeMater, "0", TextType.Tonne);
                SetText(dayTakeMater, "0", TextType.Tonne);
                SetText(cantileverFlow, "0", TextType.TonneHour);
            }
        }

        SetText(distanceOfTwoCars, (Mathf.Abs(data.DC_Pos - data.DC_Pos_2) + 64.34).ToString("F2"), TextType.Meter);
    }

    //堆取料弹窗提示
    public virtual void PileTakeMaterPop(TaskType taskType, int time)
    {
        // if (togetherBtn.red.activeSelf==true)
        // {
        //     if (taskType == TaskType.PILEMATER)
        //     {
        //         UIManager.Instance.OpenUI(UIID.ConfirmPanel, new ConfirmPanelArgs("悬胶堆料运行倒计时 {0}s", null, null, time));
        //     }
        //     else
        //     {
        //         UIManager.Instance.OpenUI(UIID.ConfirmPanel,
        //             new ConfirmPanelArgs("悬胶取料运行倒计时 {0}s", null, null, time));
        //         //斗轮运行倒计时
        //     }
        // }
    }

    public void AddOnClickListener(Button btn, UnityAction action)
    {
        btn.onClick.AddListener(action);
    }

    public void AddOnClickListener(ButtonCell btn, UnityAction action)
    {
        if (btn == null)
        {
            Debug.Log("btn is null");
        }
        else
        {
            btn.AddListener(action);
        }
    }

    public virtual void UpdateCurCtrMode(ref ButtonCell ctr, ButtonCell btn)
    {
        if (ctr != null)
        {
            ctr.SetSelectState(false);
        }

        ctr = btn;
        ctr.SetSelectState(true);
    }

    public virtual void SendMessageToServer(COMMAND_NAME command)
    {
        if (GameDataManager.Instance.GameMain.connectionRC.isConnect == false)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel, new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
            return;
        }

        string commandName = machine == Machine.BucketWheelStackerReclaimer
            ? command.ToString() + "_1"
            : command.ToString() + "_2";
        Debug.Log($"sendMessage {machine} {commandName}");
        switch (command)
        {
            case COMMAND_NAME.BELTSSTOP_BUTTON:
                UpdateCurCtrMode(ref curPileTakeMode, stopTakeMaterBtn);
                DataManager.Instance.InsertHistoryLogMc("堆取料停止开关", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.BELTTAKE_BUTTON:
                UpdateCurCtrMode(ref curPileTakeMode, takeMaterBtn);
                DataManager.Instance.InsertHistoryLogMc("取料开关", GameDataManager.Instance.GetUserName(), machine);
                //command_name = machine == Machine.BucketWheelStackerReclaimer ? "MOVE_FORWARD_1" : "MOVE_FORWARD_2";
                break;
            case COMMAND_NAME.BELTSTACK_BUTTON:
                DataManager.Instance.InsertHistoryLogMc("堆料开关", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.MODE_B:
                // UpdateCurCtrMode(ref curCtrMode, togetherBtn);
                DataManager.Instance.InsertHistoryLogMc("联动", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.MODE_A:
                // UpdateCurCtrMode(ref curCtrMode, aloneBtn);
                DataManager.Instance.InsertHistoryLogMc("单动", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.MODE_C:
                // UpdateCurCtrMode(ref curCtrMode, automaticBtn);
                DataManager.Instance.InsertHistoryLogMc("自动", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.TRAVEL_SPEED_SLOW:
                carSlowBtn.SetSelectState(true);
                carFastBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("大车行走慢速", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.TRAVEL_SPEED_FAST:
                carSlowBtn.SetSelectState(false);
                carFastBtn.SetSelectState(true);
                DataManager.Instance.InsertHistoryLogMc("大车行走快速", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.MOVE_FORWARD:
                UpdateCurCtrMode(ref curCarMoveMode, carForwardBtn);
                DataManager.Instance.InsertHistoryLogMc("大车前进", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.MOVE_STOP:
                UpdateCurCtrMode(ref curCarMoveMode, carStopBtn);
                DataManager.Instance.InsertHistoryLogMc("大车停止", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.MOVE_BACKWARD:
                UpdateCurCtrMode(ref curCarMoveMode, carBackBtn);
                DataManager.Instance.InsertHistoryLogMc("大车后退", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.ELEVATE_UP:
                UpdateCurCtrMode(ref curCarPitchingMode, upBtn);
                DataManager.Instance.InsertHistoryLogMc("俯仰上仰", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.ELEVATE_DOWN:
                UpdateCurCtrMode(ref curCarPitchingMode, downBtn);
                DataManager.Instance.InsertHistoryLogMc("俯仰下附", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.ELEVATE_STOP:
                UpdateCurCtrMode(ref curCarPitchingMode, stopBtn);
                DataManager.Instance.InsertHistoryLogMc("俯仰停止", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.ROTATE_LEFT:
                UpdateCurCtrMode(ref curCarRotMode, leftBtn);
                DataManager.Instance.InsertHistoryLogMc("回转左转", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.ROTATE_RIGHT:
                UpdateCurCtrMode(ref curCarRotMode, rightBtn);
                DataManager.Instance.InsertHistoryLogMc("回转右转", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.ROTATE_STOP:
                UpdateCurCtrMode(ref curCarRotMode, rotStopBtn);
                DataManager.Instance.InsertHistoryLogMc("回转停止", GameDataManager.Instance.GetUserName(), machine);
                break;
            default:
                break;
        }

        GameDataManager.Instance.SendServerCommandByName(commandName);
    }

    public void SendMessageToServer(COMMAND_NAME command, Action action = null)
    {
        if (GameDataManager.Instance.GameMain.connectionRC.isConnect == false)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel, new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP));
            return;
        }

        string des = "";
        switch (command)
        {
            case COMMAND_NAME.RAIL_RELAX:
                des = "是否确认夹轨器放松?";
                break;
            case COMMAND_NAME.RAIL_CLAMP:
                des = "是否确认夹轨器夹紧?";
                break;
            case COMMAND_NAME.BUCKET_START:
                des = "是否确认斗轮启动?";
                break;
            case COMMAND_NAME.BUCKET_STOP:
                des = "是否确认斗轮停止?";
                break;
            case COMMAND_NAME.OILBUMP_ON:
                des = "是否确认主车油泵启动?";
                break;
            case COMMAND_NAME.OILBUMP_OFF:
                des = "是否确认主车油泵停止?";
                break;
            case COMMAND_NAME.VIBRATOR_START:
                des = "是否确认振打器启动?";
                break;
            case COMMAND_NAME.VIBRATOR_STOP:
                des = "是否确认振打器停止?";
                break;
            default:
                break;
        }

        UIManager.Instance.OpenUI(UIID.ConfirmPanel, new ConfirmPanelArgs(des, null, (() =>
        {
            action?.Invoke();
            ConfirmSendMessageToServer(command);
        })));
    }

    public void ConfirmSendMessageToServer(COMMAND_NAME command)
    {
        string commandName = machine == Machine.BucketWheelStackerReclaimer
            ? command.ToString() + "_1"
            : command.ToString() + "_2";
        int dataInt = 0;
        Debug.Log($"message {machine}   {commandName}");
        switch (command)
        {
            case COMMAND_NAME.RAIL_RELAX:
                disengageClampBtn.SetSelectState(true);
                engageClampBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("夹轨器放松", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.RAIL_CLAMP:
                engageClampBtn.SetSelectState(true);
                disengageClampBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("夹轨器夹紧", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.BUCKET_START:
                bucketWheelStartBtn.SetSelectState(true);
                bucketWheelStopBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("斗轮启动", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.BUCKET_STOP:
                bucketWheelStopBtn.SetSelectState(true);
                bucketWheelStartBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("斗轮停止", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.OILBUMP_ON:
                oilPumpStartBtn.SetSelectState(true);
                oilPumpStopBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("主车油泵启动", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.OILBUMP_OFF:
                oilPumpStopBtn.SetSelectState(true);
                oilPumpStartBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("主车油泵关闭", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.VIBRATOR_START:
                shakerStartBtn.SetSelectState(true);
                shakerStopBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("振打器启动", GameDataManager.Instance.GetUserName(), machine);
                break;
            case COMMAND_NAME.VIBRATOR_STOP:
                shakerStopBtn.SetSelectState(true);
                shakerStartBtn.SetSelectState(false);
                DataManager.Instance.InsertHistoryLogMc("振打器停止", GameDataManager.Instance.GetUserName(), machine);
                break;
            default:
                break;
        }

        GameDataManager.Instance.SendServerCommandByName(commandName, dataInt);
    }

    public virtual void SetText(Text text, string value, TextType type)
    {
        if (text == null)
        {
            Debug.LogError(" Text is null 请检查 AngleCurrentValueItem");
            return;
        }

        if (text.text == value)
        {
            return;
        }

        if (value == "")
        {
            value = "0";
        }

        text.SetTextSymbol(value, type);
    }
}