using System;
using System.Collections;
using System.Collections.Generic;
using ShenYangRemoteSystem.Subclass;
using Unity.VisualScripting;
using UnityEngine;

public class AnticollisionDeviceData
{
   /// <summary>
   /// 大车左前防碰撞设定
   /// </summary>
   public float DC_SAS_LF_DSV;

   public float DCZQ_FZ_VALUE;
   /// <summary>
   /// 大车左后防碰撞设定
   /// </summary>
   public float DC_SAS_LB_DSV;

   public float DCZH_FZ_VALUE;
   /// <summary>
   /// 大车右前防碰撞设定
   /// </summary>
   public float DC_SAS_RF_DSV;

   public float DCYQ_FZ_VALUE;

   /// <summary>
   /// 大车右后防碰撞设定
   /// </summary>
   /// <returns></returns>
   public float DC_SAS_RB_DSV;

   public float DCYH_FZ_VALUE;
   /// <summary>
   /// 大车前进极限设定
   /// </summary>
   public  float DC_FWD_SPSV;
   
   /// <summary>
   /// 大车后退极限设定
   /// </summary>
   public float DC_REV_SPSV;
   /// <summary>
   /// 左转极限设定设定
   /// </summary>
   public float Slew_L_SPSV;
   /// <summary>
   /// 右转极限设定设定
   /// </summary>
   public float Slew_R_SPSV;
   /// <summary>
   /// 上仰极限设定设定
   /// </summary>
   public  float Luff_U_SPSV;
   /// <summary>
   /// 下俯极限设定设定
   /// </summary>
   public float Luff_D_SPSV;
   /// <summary>
   /// 悬臂左前防撞
   /// </summary>
   public float XBZQ_FZ_VALUE;
   /// <summary>
   /// 悬臂左前防撞设定
   /// </summary>
   public float SR1_Boom_SAS_LF_Ult_DSV;
   /// <summary>
   /// 悬臂左中防撞
   /// </summary>
   public float XBZZ_FZ_VALUE;
   /// <summary>
   /// 悬臂左中防撞设定
   /// </summary>
   public float SR1_Boom_SAS_LM_Ult_DSV;
   /// <summary>
   /// 悬臂左后防撞
   /// </summary>
   public float XBZH_FZ_VALUE;
   /// <summary>
   /// 悬臂左后防撞设定
   /// </summary>
   public float SR1_Boom_SAS_LB_Ult_DSV;
   /// <summary>
   /// 悬臂右前防撞
   /// </summary>
   public float XBYQ_FZ_VALUE;
   /// <summary>
   /// 悬臂右前防撞设定
   /// </summary>
   public float Boom_SAS_R_Radar_DSV;
   /// <summary>
   /// 悬臂右中防撞
   /// </summary>
   public float XBYZ_FZ_VALUE;
   
   /// <summary>
   /// 悬臂右中防撞设定
   /// </summary>
   public float Boom_SAS_L_Radar_DSV;
   /// <summary>
   /// 悬臂右后防撞
   /// </summary>
   public float XBYH_FZ_VALUE;
   /// <summary>
   /// 悬臂右后防撞设定
   /// </summary>
   public float Boom_SAS_R_Ult_DSV;
}
/// <summary>
///防碰撞设备参数设置
/// </summary>
public class AntiCollisionDeviceParameterSetting : MonoBehaviour
{
   /// <summary>
   /// 大车左前防碰撞
   /// </summary>
   public SetParameterItem DC_SAS_LF_DSV;
   /// <summary>
   /// 大车左后防碰撞
   /// </summary>
   public SetParameterItem DC_SAS_LB_DSV;
   /// <summary>
   /// 大车右前防碰撞
   /// </summary>
   public SetParameterItem DC_SAS_RF_DSV;

   /// <summary>
   /// 大车右后防碰撞
   /// </summary>
   /// <returns></returns>
   public SetParameterItem DC_SAS_RB_DSV;
   /// <summary>
   /// 大车前进极限
   /// </summary>
   public  SetParameterItem DC_FWD_SPSV;
   /// <summary>
   /// 大车后退极限
   /// </summary>
   public SetParameterItem DC_REV_SPSV;
   /// <summary>
   /// 左转极限设定
   /// </summary>
   public SetParameterItem Slew_L_SPSV;
   /// <summary>
   /// 右转极限设定
   /// </summary>
   public SetParameterItem Slew_R_SPSV;
   /// <summary>
   /// 上仰极限设定
   /// </summary>
   public  SetParameterItem Luff_U_SPSV;
   /// <summary>
   /// 下俯极限设定
   /// </summary>
   public SetParameterItem Luff_D_SPSV;
   
   /// <summary>
   /// 悬臂左前防撞
   /// </summary>
   public SetParameterItem XBZQ_FZ_VALUE;
   /// <summary>
   /// 悬臂左中防撞
   /// </summary>
   public SetParameterItem XBZZ_FZ_VALUE;
   /// <summary>
   /// 悬臂左后防撞
   /// </summary>
   public SetParameterItem XBZH_FZ_VALUE;
   /// <summary>
   /// 悬臂右前防撞
   /// </summary>
   public SetParameterItem XBYQ_FZ_VALUE;
   /// <summary>
   /// 悬臂右中防撞
   /// </summary>
   public SetParameterItem XBYZ_FZ_VALUE;
   /// <summary>
   /// 悬臂右后防撞
   /// </summary>
   public SetParameterItem XBYH_FZ_VALUE;

   private bool IsUpdate;
   private void OnEnable()
   {
      IsUpdate = true;
   }

   public void SetCommandName(Machine machine)
   {
      if (machine==Machine.BucketWheelStackerReclaimer)
      {
         DC_SAS_LF_DSV.SetCommandName(machine,COMMAND_NAME.CAR_LEFT_FRONT_SET+"_1",COMMAND_NAME.BTLFCP_BYPASS+"_1","大车左前防碰撞");
         DC_SAS_LB_DSV.SetCommandName(machine,COMMAND_NAME.CAR_LEFT_BACK_SET+"_1",COMMAND_NAME.BTLRCP_BYPASS+"_1","大车左后防碰撞");
         DC_SAS_RF_DSV.SetCommandName(machine,COMMAND_NAME.CAR_RIGHT_FRONT_SET+"_1",COMMAND_NAME.BTRFCP_BYPASS+"_1","大车右前防碰撞");
         DC_SAS_RB_DSV.SetCommandName(machine,COMMAND_NAME.CAR_RIGHT_BACK_SET+"_1",COMMAND_NAME.BTRRCP_BYPASS+"_1","大车右后防碰撞");
         DC_FWD_SPSV.SetCommandName(machine,COMMAND_NAME.FORWARD_LIMIT_SET+"_1",COMMAND_NAME.BTCP_BYPASS+"_1","大车前进极限");
         DC_REV_SPSV.SetCommandName(machine,COMMAND_NAME.BACKWARD_LIMIT_SET+"_1",COMMAND_NAME.BTCP_BYPASS+"_1","大车后退极限");
         Slew_L_SPSV.SetCommandName(machine,COMMAND_NAME.LEFT_LIMIT_SET+"_1",COMMAND_NAME.RAP_BYPASS+"_1","左转极限");
         Slew_R_SPSV.SetCommandName(machine,COMMAND_NAME.RIGHT_LIMIT_SET+"_1",COMMAND_NAME.RAP_BYPASS+"_1","右转极限");
         Luff_U_SPSV.SetCommandName(machine,COMMAND_NAME.UP_LIMIT_SET+"_1",COMMAND_NAME.PAP_BYPASS+"_1","上仰极限");
         Luff_D_SPSV.SetCommandName(machine,COMMAND_NAME.DOWN_LIMIT_SET+"_1",COMMAND_NAME.PAP_BYPASS+"_1","下俯极限");
         
         XBZQ_FZ_VALUE.SetCommandName(machine,COMMAND_NAME.ULT_LEFT_FRONT_SET+"_1",COMMAND_NAME.SASLF_BYPASS+"_1","悬臂左前防撞");
         XBZZ_FZ_VALUE.SetCommandName(machine,COMMAND_NAME.ULT_LEFT_MIDDLE_SET+"_1",COMMAND_NAME.SASLM_BYPASS+"_1","悬臂左中防撞");
         XBZH_FZ_VALUE.SetCommandName(machine,COMMAND_NAME.ULT_LEFT_BACK_SET+"_1",COMMAND_NAME.SASLB_BYPASS+"_1","悬臂左后防撞");
         XBYQ_FZ_VALUE.SetCommandName(machine,COMMAND_NAME.ULT_RIGHT_FRONT_SET+"_1",COMMAND_NAME.ARFRUCP_BYPASS+"_1","悬臂右前防撞");
         XBYZ_FZ_VALUE.SetCommandName(machine,COMMAND_NAME.ULT_RIGHT_MIDDLE_SET+"_1",COMMAND_NAME.ARMUCP_BYPASS+"_1","悬臂右中防撞");
         XBYH_FZ_VALUE.SetCommandName(machine,COMMAND_NAME.ULT_RIGHT_BACK_SET+"_1",COMMAND_NAME.ARREUCP_BYPASS+"_1","悬臂右后防撞");
      }
      else
      {
         DC_SAS_LF_DSV.SetCommandName(machine,COMMAND_NAME.CAR_LEFT_FRONT_SET+"_2",COMMAND_NAME.BTLFCP_BYPASS+"_2","大车左前防碰撞");
         DC_SAS_LB_DSV.SetCommandName(machine,COMMAND_NAME.CAR_LEFT_BACK_SET+"_2",COMMAND_NAME.BTLRCP_BYPASS+"_2","大车左后防碰撞");
         DC_SAS_RF_DSV.SetCommandName(machine,COMMAND_NAME.CAR_RIGHT_FRONT_SET+"_2",COMMAND_NAME.BTRFCP_BYPASS+"_2","大车右前防碰撞");
         DC_SAS_RB_DSV.SetCommandName(machine,COMMAND_NAME.CAR_RIGHT_BACK_SET+"_2",COMMAND_NAME.BTRRCP_BYPASS+"_2","大车右后防碰撞");
         DC_FWD_SPSV.SetCommandName(machine,COMMAND_NAME.FORWARD_LIMIT_SET+"_2",COMMAND_NAME.BTCP_BYPASS+"_2","大车前进极限");
         DC_REV_SPSV.SetCommandName(machine,COMMAND_NAME.BACKWARD_LIMIT_SET+"_2",COMMAND_NAME.BTCP_BYPASS+"_2","大车后退极限");
         Slew_L_SPSV.SetCommandName(machine,COMMAND_NAME.LEFT_LIMIT_SET+"_2",COMMAND_NAME.RAP_BYPASS+"_2","左转极限");
         Slew_R_SPSV.SetCommandName(machine,COMMAND_NAME.RIGHT_LIMIT_SET+"_2",COMMAND_NAME.RAP_BYPASS+"_2","右转极限");
         Luff_U_SPSV.SetCommandName(machine,COMMAND_NAME.UP_LIMIT_SET+"_2",COMMAND_NAME.PAP_BYPASS+"_2","上仰极限");
         Luff_D_SPSV.SetCommandName(machine,COMMAND_NAME.DOWN_LIMIT_SET+"_2",COMMAND_NAME.PAP_BYPASS+"_2","下俯极限");
         
         XBZQ_FZ_VALUE.SetCommandName(machine,COMMAND_NAME.ULT_LEFT_FRONT_SET+"_2",COMMAND_NAME.SASLF_BYPASS+"_2","悬臂左前防撞");
         XBZZ_FZ_VALUE.SetCommandName(machine,COMMAND_NAME.ULT_LEFT_MIDDLE_SET+"_2",COMMAND_NAME.SASLM_BYPASS+"_2","悬臂左中防撞");
         XBZH_FZ_VALUE.SetCommandName(machine,COMMAND_NAME.ULT_LEFT_BACK_SET+"_2",COMMAND_NAME.SASLB_BYPASS+"_2","悬臂左后防撞");
         XBYQ_FZ_VALUE.SetCommandName(machine,COMMAND_NAME.ULT_RIGHT_FRONT_SET+"_2",COMMAND_NAME.ARFRUCP_BYPASS+"_2","悬臂右前防撞");
         XBYZ_FZ_VALUE.SetCommandName(machine,COMMAND_NAME.ULT_RIGHT_MIDDLE_SET+"_2",COMMAND_NAME.ARMUCP_BYPASS+"_2","悬臂右中防撞");
         XBYH_FZ_VALUE.SetCommandName(machine,COMMAND_NAME.ULT_RIGHT_BACK_SET+"_2",COMMAND_NAME.ARREUCP_BYPASS+"_2","悬臂右后防撞");
      }
     
   }

   public void UpdateParameter(SystemVariables systemVariables, Machine machine)
   {
      if (machine==Machine.BucketWheelStackerReclaimer)
      {
         DC_SAS_LF_DSV.SetButtonColor(systemVariables.DC_SAS_LF_Bypass==false);
         DC_SAS_RF_DSV.SetButtonColor(systemVariables.DC_SAS_RF_Bypass==false);
         DC_SAS_RB_DSV.SetButtonColor(systemVariables.DC_SAS_RB_Bypass==false);
         DC_SAS_LB_DSV.SetButtonColor(systemVariables.DC_SAS_LB_Bypass == false);
         
         DC_FWD_SPSV.SetButtonColor(systemVariables.SlewAngle_Bypass==false);
         DC_REV_SPSV.SetButtonColor(systemVariables.SlewAngle_Bypass==false);
         Slew_L_SPSV.SetButtonColor(systemVariables.SlewAngle_Bypass==false);
         Slew_R_SPSV.SetButtonColor(systemVariables.SlewAngle_Bypass==false);
         Luff_U_SPSV.SetButtonColor(systemVariables.LuffAngle_Bypass==false);
         Luff_D_SPSV.SetButtonColor(systemVariables.LuffAngle_Bypass==false);
         
         XBZQ_FZ_VALUE.SetButtonColor(systemVariables.SR1_Boom_SAS_LF_Ult_Bypass==false);
         XBZZ_FZ_VALUE.SetButtonColor(systemVariables.SR1_Boom_SAS_LM_Ult_Bypass==false);
         XBZH_FZ_VALUE.SetButtonColor(systemVariables.SR1_Boom_SAS_LB_Ult_Bypass==false);
         XBYQ_FZ_VALUE.SetButtonColor(systemVariables.SR1_Boom_SAS_RF_Ult_Bypass==false);
         XBYZ_FZ_VALUE.SetButtonColor(systemVariables.SR1_Boom_SAS_RM_Ult_Bypass==false);
         XBYH_FZ_VALUE.SetButtonColor(systemVariables.SR1_Boom_SAS_RB_Ult_Bypass==false);
         
         DC_FWD_SPSV.SetText(systemVariables.DC_Pos);
         DC_REV_SPSV.SetText(systemVariables.DC_Pos);
         Slew_L_SPSV.SetText(systemVariables.SLEW_Angle);
         Slew_R_SPSV.SetText(systemVariables.SLEW_Angle);
         Luff_U_SPSV.SetText(systemVariables.Luff_Angle);
         Luff_D_SPSV.SetText(systemVariables.Luff_Angle);
      }
      else
      {
         DC_SAS_LF_DSV.SetButtonColor(systemVariables.DC_SAS_LF_Bypass_2==false);
         DC_SAS_RF_DSV.SetButtonColor(systemVariables.DC_SAS_RF_Bypass_2==false);
         DC_SAS_RB_DSV.SetButtonColor(systemVariables.DC_SAS_RB_Bypass_2==false);
         DC_SAS_LB_DSV.SetButtonColor(systemVariables.DC_SAS_LB_Bypass_2 == false);
         
         DC_FWD_SPSV.SetButtonColor(systemVariables.SlewAngle_Bypass_2==false);
         DC_REV_SPSV.SetButtonColor(systemVariables.SlewAngle_Bypass_2==false);
         Slew_L_SPSV.SetButtonColor(systemVariables.SlewAngle_Bypass_2==false);
         Slew_R_SPSV.SetButtonColor(systemVariables.SlewAngle_Bypass_2==false);
         Luff_U_SPSV.SetButtonColor(systemVariables.LuffAngle_Bypass_2==false);
         Luff_D_SPSV.SetButtonColor(systemVariables.LuffAngle_Bypass_2==false);
         
         XBZQ_FZ_VALUE.SetButtonColor(systemVariables.SR1_Boom_SAS_LF_Ult_Bypass_2==false);
         XBZZ_FZ_VALUE.SetButtonColor(systemVariables.SR1_Boom_SAS_LM_Ult_Bypass_2==false);
         XBZH_FZ_VALUE.SetButtonColor(systemVariables.SR1_Boom_SAS_LB_Ult_Bypass_2==false);
         XBYQ_FZ_VALUE.SetButtonColor(systemVariables.SR1_Boom_SAS_RF_Ult_Bypass_2==false);
         XBYZ_FZ_VALUE.SetButtonColor(systemVariables.SR1_Boom_SAS_RM_Ult_Bypass_2==false);
         XBYH_FZ_VALUE.SetButtonColor(systemVariables.SR1_Boom_SAS_RB_Ult_Bypass_2==false);
         
         DC_FWD_SPSV.SetText(systemVariables.DC_Pos);
         DC_REV_SPSV.SetText(systemVariables.DC_Pos);
         Slew_L_SPSV.SetText(systemVariables.SLEW_Angle);
         Slew_R_SPSV.SetText(systemVariables.SLEW_Angle);
         Luff_U_SPSV.SetText(systemVariables.Luff_Angle);
         Luff_D_SPSV.SetText(systemVariables.Luff_Angle);
      }
   }
   public void UpdateParameter(AnticollisionDeviceData deviceData)
   {
      if (IsUpdate)
      {
         IsUpdate=false;
         DC_SAS_LF_DSV.SetInputField(deviceData.DC_SAS_LF_DSV);
         DC_SAS_LB_DSV.SetInputField(deviceData.DC_SAS_LB_DSV);
         DC_SAS_RF_DSV.SetInputField(deviceData.DC_SAS_RF_DSV);
         DC_SAS_RB_DSV.SetInputField(deviceData.DC_SAS_RB_DSV);
         DC_FWD_SPSV.SetInputField(deviceData.DC_FWD_SPSV);
         DC_REV_SPSV.SetInputField(deviceData.DC_REV_SPSV);
         Slew_L_SPSV.SetInputField(deviceData.Slew_L_SPSV);
         Slew_R_SPSV.SetInputField(deviceData.Slew_R_SPSV);
         Luff_U_SPSV.SetInputField(deviceData.Luff_U_SPSV);
         Luff_D_SPSV.SetInputField(deviceData.Luff_D_SPSV);
         XBZQ_FZ_VALUE.SetInputField(deviceData.SR1_Boom_SAS_LF_Ult_DSV);
         XBZZ_FZ_VALUE.SetInputField(deviceData.SR1_Boom_SAS_LM_Ult_DSV);
         XBZH_FZ_VALUE.SetInputField(deviceData.SR1_Boom_SAS_LB_Ult_DSV);
         XBYQ_FZ_VALUE.SetInputField(deviceData.Boom_SAS_R_Radar_DSV);
         XBYZ_FZ_VALUE.SetInputField(deviceData.Boom_SAS_L_Radar_DSV);
         XBYH_FZ_VALUE.SetInputField(deviceData.Boom_SAS_R_Ult_DSV);
      }
      
      DC_SAS_LF_DSV.SetText(deviceData.DCZQ_FZ_VALUE);
    
      DC_SAS_LB_DSV.SetText(deviceData.DCZH_FZ_VALUE);
    
      DC_SAS_RF_DSV.SetText(deviceData.DCYQ_FZ_VALUE);
    
      DC_SAS_RB_DSV.SetText(deviceData.DCYH_FZ_VALUE);
      
      XBZQ_FZ_VALUE.SetText(deviceData.XBZQ_FZ_VALUE);
 
      XBZZ_FZ_VALUE.SetText(deviceData.XBZZ_FZ_VALUE);
     
      XBZH_FZ_VALUE.SetText(deviceData.XBZH_FZ_VALUE);
     
      XBYQ_FZ_VALUE.SetText(deviceData.XBYQ_FZ_VALUE);
     
      XBYZ_FZ_VALUE.SetText(deviceData.XBYZ_FZ_VALUE);
     
      XBYH_FZ_VALUE.SetText(deviceData.XBYH_FZ_VALUE);
    
      
   }

}
