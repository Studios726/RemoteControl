using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnticollisionDeviceData
{
   /// <summary>
   /// 大车左前防碰撞
   /// </summary>
   public float DC_SAS_LF_DSV;
   /// <summary>
   /// 大车左后防碰撞
   /// </summary>
   public float DC_SAS_LB_DSV;
   /// <summary>
   /// 大车右前防碰撞
   /// </summary>
   public float DC_SAS_RF_DSV;

   /// <summary>
   /// 大车右后防碰撞
   /// </summary>
   /// <returns></returns>
   public float DC_SAS_RB_DSV;
   /// <summary>
   /// 大车前进极限
   /// </summary>
   public  float DC_FWD_SPSV;
   /// <summary>
   /// 大车后退极限
   /// </summary>
   public float DC_REV_SPSV;
   /// <summary>
   /// 左转极限设定
   /// </summary>
   public float Slew_L_SPSV;
   /// <summary>
   /// 右转极限设定
   /// </summary>
   public float Slew_R_SPSV;
   /// <summary>
   /// 上仰极限设定
   /// </summary>
   public  float Luff_U_SPSV;
   /// <summary>
   /// 下俯极限设定
   /// </summary>
   public float Luff_D_SPSV;
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

   public void SetCommandName(Machine machine)
   {
      if (machine==Machine.BucketWheelStackerReclaimer)
      {
         DC_SAS_LF_DSV.SetCommandName(COMMAND_NAME.CAR_LEFT_FRONT_SET.ToString());
         DC_SAS_LB_DSV.SetCommandName(COMMAND_NAME.CAR_LEFT_BACK_SET.ToString());
         DC_SAS_RF_DSV.SetCommandName(COMMAND_NAME.CAR_RIGHT_FRONT_SET.ToString());
         DC_SAS_RB_DSV.SetCommandName(COMMAND_NAME.CAR_RIGHT_BACK_SET.ToString());
         DC_FWD_SPSV.SetCommandName(COMMAND_NAME.FORWARD_LIMIT_SET.ToString());
         DC_REV_SPSV.SetCommandName(COMMAND_NAME.BACKWARD_LIMIT_SET.ToString());
         Slew_L_SPSV.SetCommandName(COMMAND_NAME.LEFT_LIMIT_SET.ToString());
         Slew_R_SPSV.SetCommandName(COMMAND_NAME.RIGHT_LIMIT_SET.ToString());
         Luff_U_SPSV.SetCommandName(COMMAND_NAME.UP_LIMIT_SET.ToString());
         Luff_D_SPSV.SetCommandName(COMMAND_NAME.DOWN_LIMIT_SET.ToString());
      }
      else
      {
         DC_SAS_LF_DSV.SetCommandName(COMMAND_NAME.CAR_LEFT_FRONT_SET+"_2");
         DC_SAS_LB_DSV.SetCommandName(COMMAND_NAME.CAR_LEFT_BACK_SET+"_2");
         DC_SAS_RF_DSV.SetCommandName(COMMAND_NAME.CAR_RIGHT_FRONT_SET+"_2");
         DC_SAS_RB_DSV.SetCommandName(COMMAND_NAME.CAR_RIGHT_BACK_SET+"_2");
         DC_FWD_SPSV.SetCommandName(COMMAND_NAME.FORWARD_LIMIT_SET+"_2");
         DC_REV_SPSV.SetCommandName(COMMAND_NAME.BACKWARD_LIMIT_SET+"_2");
         Slew_L_SPSV.SetCommandName(COMMAND_NAME.LEFT_LIMIT_SET+"_2");
         Slew_R_SPSV.SetCommandName(COMMAND_NAME.RIGHT_LIMIT_SET+"_2");
         Luff_U_SPSV.SetCommandName(COMMAND_NAME.UP_LIMIT_SET+"_2");
         Luff_D_SPSV.SetCommandName(COMMAND_NAME.DOWN_LIMIT_SET+"_2");
      }
     
   }

   public void UpdateParameter(AnticollisionDeviceData deviceData)
   {
      DC_SAS_LF_DSV.SetText(deviceData.DC_SAS_LF_DSV);
      DC_SAS_LB_DSV.SetText(deviceData.DC_SAS_LB_DSV);
      DC_SAS_RF_DSV.SetText(deviceData.DC_SAS_RF_DSV);
      DC_SAS_RB_DSV.SetText(deviceData.DC_SAS_RB_DSV);
      DC_FWD_SPSV.SetText(deviceData.DC_FWD_SPSV);
      DC_REV_SPSV.SetText(deviceData.DC_REV_SPSV);
      Slew_L_SPSV.SetText(deviceData.Slew_L_SPSV);
      Slew_R_SPSV.SetText(deviceData.Slew_R_SPSV);
      Luff_U_SPSV.SetText(deviceData.Luff_U_SPSV);
      Luff_D_SPSV.SetText(deviceData.Luff_D_SPSV);
      
   }

}
