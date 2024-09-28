using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoControlDeviceData
{
   /// <summary>
   /// 行走编码器异常
   /// </summary>
   public bool DC_Encoder_ERR;
   /// <summary>
   /// 回转编码器异常
   /// </summary>
   public bool Slew_Encoder_ERR;
}
/// <summary>
/// 自动控制设备状态
/// </summary>
public class AutoControlDeviceStatus : StatusParmItemBase<AutoControlDeviceData>
{
   /// <summary>
   /// 行走编码器异常
   /// </summary>
   public ToggleDIY DC_Encoder_ERR;
   /// <summary>
   /// 回转编码器异常
   /// </summary>
   public ToggleDIY Slew_Encoder_ERR;

   public override void UpdateData(AutoControlDeviceData data, bool isConnect = false)
   {
      SetToggleState(DC_Encoder_ERR, data.DC_Encoder_ERR,true,isConnect);
      SetToggleState(Slew_Encoder_ERR, data.Slew_Encoder_ERR,true,isConnect);
   }
}
