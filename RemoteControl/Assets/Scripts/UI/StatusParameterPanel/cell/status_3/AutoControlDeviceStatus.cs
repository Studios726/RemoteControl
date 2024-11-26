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
   /// <summary>
   /// 倾角仪异常
   /// </summary>
   public bool QJY_CH_FAULT;
   /// <summary>
   /// 垂直料位计异常
   /// </summary>
   public bool XBTB_LWJ_CH_FAULT;
   /// <summary>
   /// 大车左前料位计异常
   /// </summary>
   public bool DCZQ_FZ_CH_FAULT;
   /// <summary>
   /// 大车左后料位计异常
   /// </summary>
   public bool DCZH_FZ_CH_FAULT;
   /// <summary>
   /// 大车右前料位计异常
   /// </summary>
   public bool DCYQ_FZ_CH_FAULT;
   /// <summary>
   /// 大车右后料位计异常
   /// </summary>
   public bool DCYH_FZ_CH_FAULT;
   /// <summary>
   /// 悬臂左前料位计异常
   /// </summary>
   public bool XBZQ_FZ_CH_FAULT;
   /// <summary>
   /// 悬臂左中料位计异常
   /// </summary>
   public bool XBZZ_FZ_CH_FAULT;

   /// <summary>
   /// 悬臂左后料位计异常
   /// </summary>
   public bool XBZH_FZ_CH_FAULT;
   /// <summary>
   /// 悬臂右前料位计异常
   /// </summary>
   public bool XBYQ_FZ_CH_FAULT;
   /// <summary>
   /// 悬臂右中料位计异常
   /// </summary>
   public bool XBYZ_FZ_CH_FAULT;
   /// <summary>
   /// 悬臂右后料位计异常
   /// </summary>
   public bool XBYH_FZ_CH_FAULT;
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
   /// <summary>
   /// 倾角仪异常
   /// </summary>
   public ToggleDIY QJY_CH_FAULT;
   /// <summary>
   /// 垂直料位计异常
   /// </summary>
   public ToggleDIY XBTB_LWJ_CH_FAULT;
   /// <summary>
   /// 大车左前料位计异常
   /// </summary>
   public ToggleDIY DCZQ_FZ_CH_FAULT;
   /// <summary>
   /// 大车左后料位计异常
   /// </summary>
   public ToggleDIY DCZH_FZ_CH_FAULT;
   /// <summary>
   /// 大车右前料位计异常
   /// </summary>
   public ToggleDIY DCYQ_FZ_CH_FAULT;
   /// <summary>
   /// 大车右后料位计异常
   /// </summary>
   public ToggleDIY DCYH_FZ_CH_FAULT;
   /// <summary>
   /// 悬臂左前料位计异常
   /// </summary>
   public ToggleDIY XBZQ_FZ_CH_FAULT;
   /// <summary>
   /// 悬臂左中料位计异常
   /// </summary>
   public ToggleDIY XBZZ_FZ_CH_FAULT;

   /// <summary>
   /// 悬臂左后料位计异常
   /// </summary>
   public ToggleDIY XBZH_FZ_CH_FAULT;
   /// <summary>
   /// 悬臂右前料位计异常
   /// </summary>
   public ToggleDIY XBYQ_FZ_CH_FAULT;
   /// <summary>
   /// 悬臂右中料位计异常
   /// </summary>
   public ToggleDIY XBYZ_FZ_CH_FAULT;
   /// <summary>
   /// 悬臂右后料位计异常
   /// </summary>
   public ToggleDIY XBYH_FZ_CH_FAULT;

   public override void UpdateData(AutoControlDeviceData data, bool isConnect = false)
   {
      SetToggleState(DC_Encoder_ERR, data.DC_Encoder_ERR,true,isConnect);
      SetToggleState(Slew_Encoder_ERR, data.Slew_Encoder_ERR,true,isConnect);
      SetToggleState(QJY_CH_FAULT, data.QJY_CH_FAULT,true,isConnect);
      SetToggleState(XBTB_LWJ_CH_FAULT, data.XBTB_LWJ_CH_FAULT,true,isConnect);
      SetToggleState(DCZQ_FZ_CH_FAULT, data.DCZQ_FZ_CH_FAULT,true,isConnect);
      SetToggleState(DCZH_FZ_CH_FAULT, data.DCZH_FZ_CH_FAULT,true,isConnect);
      SetToggleState(DCYQ_FZ_CH_FAULT, data.DCYQ_FZ_CH_FAULT,true,isConnect);
      SetToggleState(DCYH_FZ_CH_FAULT, data.DCYH_FZ_CH_FAULT,true,isConnect);
      SetToggleState(XBZQ_FZ_CH_FAULT, data.XBZQ_FZ_CH_FAULT,true,isConnect);
      SetToggleState(XBZZ_FZ_CH_FAULT, data.XBZZ_FZ_CH_FAULT,true,isConnect);
      SetToggleState(XBZH_FZ_CH_FAULT, data.XBZH_FZ_CH_FAULT,true,isConnect);
      SetToggleState(XBYQ_FZ_CH_FAULT, data.XBYQ_FZ_CH_FAULT,true,isConnect);
      SetToggleState(XBYZ_FZ_CH_FAULT, data.XBYZ_FZ_CH_FAULT,true,isConnect);
      SetToggleState(XBYH_FZ_CH_FAULT, data.XBYH_FZ_CH_FAULT,true,isConnect);
      
   }
}
