using System;
using System.Collections;
using System.Collections.Generic;
using ShenYangRemoteSystem.Subclass;
using UnityEngine;

public class PileTakeMaterThirdSysStateParams : MonoBehaviour
{
   public AutoControlDeviceStatus AutoControlDeviceStatus;
   public AntiCollisionLimitStatus AntiCollisionLimitStatus;
   public AntiCollisionDeviceParameterSetting AntiCollisionDeviceParameterSetting;
   public Machine machine;
   SystemVariables systemVariables;

   private void Start()
   {
      AntiCollisionDeviceParameterSetting.SetCommandName(machine);
   }

   public void UpdateData()
   {
      systemVariables= GameDataManager.Instance.SystemVariables;
      if (systemVariables==null)
      {
         return;
      }
      UpdateAutoControlDeviceStatus();
      UpdateAntiCollisionLimitStatus();
      SetAntiCollisionDeviceParameterSetting();
   }
   //自动控制设备状态
   public  void UpdateAutoControlDeviceStatus()
   {
      AutoControlDeviceData data =new AutoControlDeviceData();
      data.Slew_Encoder_ERR=GetToggleState(systemVariables.Slew_Encoder_ERR,systemVariables.Slew_Encoder_ERR_2);
      data.DC_Encoder_ERR=GetToggleState(systemVariables.DC_Encoder_ERR,systemVariables.DC_Encoder_ERR_2);
      AutoControlDeviceStatus.UpdateData(data,GameDataManager.Instance.GameMain.connectionRC.isConnect);
   }
   //防冲限速器状态
   public  void UpdateAntiCollisionLimitStatus()
   {
      AntiCollisionLimitData data =new AntiCollisionLimitData();
      data.OverBelt_R_Limit=GetToggleState(systemVariables.OverBelt_R_Limit,systemVariables.OverBelt_R_Limit_2);
      data.OverBelt_L_Limit=GetToggleState(systemVariables.OverBelt_L_Limit,systemVariables.OverBelt_L_Limit_2);
      data.OverBelt_D_Limit=GetToggleState(systemVariables.OverBelt_D_Limit,systemVariables.OverBelt_D_Limit_2);
      data.Slew_SAS_L_Alarm=GetToggleState(systemVariables.Slew_SAS_L_Alarm,systemVariables.Slew_SAS_L_Alarm_2);
      data.Slew_SAS_R_Alarm=GetToggleState(systemVariables.Slew_SAS_R_Alarm,systemVariables.Slew_SAS_R_Alarm_2);
      data.Slew_SAS_RR_Alarm=GetToggleState(systemVariables.Slew_SAS_RR_Alarm,systemVariables.Slew_SAS_RR_Alarm_2);
      data.Slew_SAS_LU_Alarm=GetToggleState(systemVariables.Slew_SAS_LU_Alarm,systemVariables.Slew_SAS_LU_Alarm_2);
      data.Slew_SAS_RU_Alarm=GetToggleState(systemVariables.Slew_SAS_RU_Alarm,systemVariables.Slew_SAS_RU_Alarm_2);
      data.Slew_L_LimitStatus=GetToggleState(systemVariables.Slew_L_LimitStatus,systemVariables.Slew_L_LimitStatus_2);
      data.Slew_R_LimitStatus=GetToggleState(systemVariables.Slew_R_LimitStatus,systemVariables.Slew_R_LimitStatus_2);
      data.Slew_L_Limit_Waring=GetToggleState(systemVariables.Slew_L_Limit_Waring,systemVariables.Slew_L_Limit_Waring_2);
      data.Slew_R_Limit_Waring=GetToggleState(systemVariables.Slew_R_Limit_Waring,systemVariables.Slew_R_Limit_Waring_2);
      data.Slew_SAS_LR_Alarm=GetToggleState(systemVariables.Slew_SAS_LR_Alarm,systemVariables.Slew_SAS_LR_Alarm_2);
      data.OverBelt_R_SoftLimit=GetToggleState(systemVariables.OverBelt_R_SoftLimit,systemVariables.OverBelt_R_SoftLimit_2);
      data.OverBelt_L_SoftLimit=GetToggleState(systemVariables.OverBelt_L_SoftLimit,systemVariables.OverBelt_L_SoftLimit_2);
      data.OverBelt_D_SoftLimit=GetToggleState(systemVariables.OverBelt_D_SoftLimit,systemVariables.OverBelt_D_SoftLimit_2);
      data.Luff_U_Limit_Waring=GetToggleState(systemVariables.Luff_U_Limit_Waring,systemVariables.Luff_U_Limit_Waring_2);
      data.Luff_Down_SoftLimit=GetToggleState(systemVariables.Luff_Down_SoftLimit,systemVariables.Luff_Down_SoftLimit_2);
      data.LuffUp_LimitStatus=GetToggleState(systemVariables.LuffUp_LimitStatus,systemVariables.LuffUp_LimitStatus_2);
      data.Luff_Up_SoftLimit=GetToggleState(systemVariables.Luff_Up_SoftLimit,systemVariables.Luff_Up_SoftLimit_2);
      data.Luff_D_Limit_Waring=GetToggleState(systemVariables.Luff_D_Limit_Waring,systemVariables.Luff_D_Limit_Waring_2);
      data.DcFWD_LimitStatus=GetToggleState(systemVariables.DcFWD_LimitStatus,systemVariables.DcFWD_LimitStatus_2);
      data.DcREV_LimitStatus=GetToggleState(systemVariables.DcREV_LimitStatus,systemVariables.DcREV_LimitStatus_2);
      data.FWD_Limit_Waring=GetToggleState(systemVariables.FWD_Limit_Waring,systemVariables.FWD_Limit_Waring_2);
      data.REV_Limit_Waring=GetToggleState(systemVariables.REV_Limit_Waring,systemVariables.REV_Limit_Waring_2);
      data.DC_SAS_F_Alarm=GetToggleState(systemVariables.DC_SAS_F_Alarm,systemVariables.DC_SAS_F_Alarm_2);
      data.DC_SAS_B_Alarm=GetToggleState(systemVariables.DC_SAS_B_Alarm,systemVariables.DC_SAS_B_Alarm_2);
      data.DC_SAS_LB_Alrm=GetToggleState(systemVariables.DC_SAS_LB_Alrm,systemVariables.DC_SAS_LB_Alrm_2);
      data.DC_FWD_SoftLimit=GetToggleState(systemVariables.DC_FWD_SoftLimit,systemVariables.DC_FWD_SoftLimit_2);
      data.DC_REV_SoftLimit=GetToggleState(systemVariables.DC_REV_SoftLimit,systemVariables.DC_REV_SoftLimit_2);
      data.DC_SAS_LF_Alrm=GetToggleState(systemVariables.DC_SAS_LF_Alrm,systemVariables.DC_SAS_LF_Alrm_2);
      data.DC_SAS_RB_Alrm=GetToggleState(systemVariables.DC_SAS_RB_Alrm,systemVariables.DC_SAS_RB_Alrm_2);
      data.DC_SAS_RF_Alrm=GetToggleState(systemVariables.DC_SAS_RF_Alrm,systemVariables.DC_SAS_RF_Alrm_2);
      data.Slew_R_SoftLimit=GetToggleState(systemVariables.Slew_R_SoftLimit,systemVariables.Slew_R_SoftLimit_2);
      data.Slew_L_SoftLimit=GetToggleState(systemVariables.Slew_L_SoftLimit,systemVariables.Slew_L_SoftLimit_2);
      data.LuffDown_LimitStatus=GetToggleState(systemVariables.LuffDown_LimitStatus,systemVariables.LuffDown_LimitStatus_2);
      AntiCollisionLimitStatus.UpdateData(data,GameDataManager.Instance.GameMain.connectionRC.isConnect);
   }
   //防碰撞设备参数设置
   public void SetAntiCollisionDeviceParameterSetting()
   {
      AnticollisionDeviceData deviceData = new AnticollisionDeviceData();
      deviceData.DC_SAS_LF_DSV = GetParameterValue(systemVariables.DC_SAS_LF_DSV, systemVariables.DC_SAS_LF_DSV_2);
      deviceData.DCZQ_FZ_VALUE = GetParameterValue(systemVariables.DCZQ_FZ_VALUE, systemVariables.DCZQ_FZ_VALUE_2);
      
      deviceData.DC_SAS_LB_DSV = GetParameterValue(systemVariables.DC_SAS_LB_DSV, systemVariables.DC_SAS_LB_DSV_2);
      deviceData.DCZH_FZ_VALUE = GetParameterValue(systemVariables.DCZH_FZ_VALUE, systemVariables.DCZH_FZ_VALUE_2);
      
      deviceData.DC_SAS_RF_DSV = GetParameterValue(systemVariables.DC_SAS_RF_DSV, systemVariables.DC_SAS_RF_DSV_2);
      deviceData.DCYQ_FZ_VALUE = GetParameterValue(systemVariables.DCYQ_FZ_VALUE, systemVariables.DCYQ_FZ_VALUE_2);
       
      deviceData.DC_SAS_RB_DSV = GetParameterValue(systemVariables.DC_SAS_RB_DSV, systemVariables.DC_SAS_RB_DSV_2);
      deviceData.DCYH_FZ_VALUE = GetParameterValue(systemVariables.DCYH_FZ_VALUE, systemVariables.DCYH_FZ_VALUE_2);
      
      deviceData.Luff_D_SPSV = GetParameterValue(systemVariables.Luff_D_SPSV, systemVariables.Luff_D_SPSV_2);
      
      deviceData.Luff_U_SPSV = GetParameterValue(systemVariables.Luff_U_SPSV, systemVariables.Luff_U_SPSV_2);
      
      deviceData.DC_FWD_SPSV = GetParameterValue(systemVariables.DC_FWD_SPSV, systemVariables.DC_FWD_SPSV_2);
      
      deviceData.DC_REV_SPSV = GetParameterValue(systemVariables.DC_REV_SPSV, systemVariables.DC_REV_SPSV_2);
      
      deviceData.Slew_L_SPSV= GetParameterValue(systemVariables.Slew_L_SPSV, systemVariables.Slew_L_SPSV_2);
      
      deviceData.Slew_R_SPSV= GetParameterValue(systemVariables.Slew_R_SPSV, systemVariables.Slew_R_SPSV_2);

      deviceData.XBZQ_FZ_VALUE = GetParameterValue(systemVariables.XBZQ_FZ_VALUE, systemVariables.XBZQ_FZ_VALUE_2);
      
      deviceData.XBZZ_FZ_VALUE = GetParameterValue(systemVariables.XBZZ_FZ_VALUE, systemVariables.XBZZ_FZ_VALUE_2);
      
      deviceData.XBZH_FZ_VALUE = GetParameterValue(systemVariables.XBZH_FZ_VALUE, systemVariables.XBZH_FZ_VALUE_2);
      
      deviceData.XBYQ_FZ_VALUE = GetParameterValue(systemVariables.XBYQ_FZ_VALUE, systemVariables.XBYQ_FZ_VALUE_2);
      
      deviceData.XBYZ_FZ_VALUE = GetParameterValue(systemVariables.XBYZ_FZ_VALUE, systemVariables.XBYZ_FZ_VALUE_2);
      
      deviceData.XBYH_FZ_VALUE = GetParameterValue(systemVariables.XBYH_FZ_VALUE, systemVariables.XBYH_FZ_VALUE_2);
      AntiCollisionDeviceParameterSetting.UpdateParameter(deviceData);
      AntiCollisionDeviceParameterSetting.UpdateParameter(systemVariables, machine);
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
   
   public float GetParameterValue(float machine1,float machine2)
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
         return 0;
      }
   }
}
