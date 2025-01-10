using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiCollisionLimitData
{
    /// <summary>
    /// 回转右转过皮带保护限位
    /// </summary>
    public bool OverBelt_R_Limit;
    /// <summary>
    /// 回转左转过皮带保护限位
    /// </summary>
    public bool OverBelt_L_Limit;
    /// <summary>
    /// 回转过皮带俯仰下限位保护
    /// </summary>
    public bool OverBelt_D_Limit;
    /// <summary>
    /// 回转右转过皮带保护软限位
    /// </summary>
    public bool OverBelt_R_SoftLimit;

    /// <summary>
    /// 回转左转过皮带保护软限位
    /// </summary>
    public bool OverBelt_L_SoftLimit;
    /// <summary>
    /// 回转过皮带俯仰下限软限位
    /// </summary>
    public bool OverBelt_D_SoftLimit;
    /// <summary>
    /// 大车前进停止软限位
    /// </summary>
    public bool DC_FWD_SoftLimit;
    /// <summary>
    /// 大车前进限位集合
    /// </summary>
    public bool DcFWD_LimitStatus;
    /// <summary>
    /// 大车后退停止软限位
    /// </summary>
    public bool DC_REV_SoftLimit;
    /// <summary>
    /// 大车后退限位集合
    /// </summary>
    public bool DcREV_LimitStatus;
    /// <summary>
    /// 悬臂右转软限位
    /// </summary>
    public bool Slew_R_SoftLimit;
    /// <summary>
    /// 悬臂右转限位集合
    /// </summary>
    public bool Slew_R_LimitStatus;
    /// <summary>
    /// 悬臂左转软限位
    /// </summary>
    public bool Slew_L_SoftLimit;
    /// <summary>
    /// 悬臂左转限位集合
    /// </summary>
    public bool Slew_L_LimitStatus;
    /// <summary>
    /// 俯仰上极限软限位
    /// </summary>
    public bool Luff_Up_SoftLimit;
    /// <summary>
    /// 俯仰上极限限位集合
    /// </summary>
    public bool LuffUp_LimitStatus;
    /// <summary>
    /// 俯仰下极限软限位
    /// </summary>
    public bool Luff_Down_SoftLimit;

    /// <summary>
    /// 俯仰下极限限位集合
    /// </summary>
    public bool LuffDown_LimitStatus;
    /// <summary>
    /// 悬臂左侧防撞保护动作
    /// </summary>
    public bool Slew_SAS_L_Alarm;
    /// <summary>
    /// 悬臂右侧防撞保护动作
    /// </summary>
    public bool Slew_SAS_R_Alarm;
    /// <summary>
    /// 大车前进防撞保护动作
    /// </summary>
    public bool DC_SAS_F_Alarm;
    /// <summary>
    /// 大车后退防撞保护动作
    /// </summary>
    public bool DC_SAS_B_Alarm;
    /// <summary>
    /// 悬臂右雷达前方有煤垛碰撞警告
    /// </summary>
    public bool Slew_SAS_RR_Alarm;
    /// <summary>
    /// 悬臂左雷达前方有煤垛碰撞警告
    /// </summary>
    public bool Slew_SAS_LR_Alarm;
    /// <summary>
    /// 悬臂右超声波前方有煤垛碰撞警告
    /// </summary>
    public bool Slew_SAS_RU_Alarm;
    /// <summary>
    /// 悬臂左超声波前方有煤垛碰撞警告
    /// </summary>
    public bool Slew_SAS_LU_Alarm;
    /// <summary>
    /// 大车右前方有障碍
    /// </summary>
    public bool DC_SAS_RF_Alrm;
    /// <summary>
    /// 大车右后方有障碍
    /// </summary>
    public bool DC_SAS_RB_Alrm;
    /// <summary>
    /// 大车左前方有障碍
    /// </summary>
    public bool DC_SAS_LF_Alrm;
    /// <summary>
    /// 大车左后方有障碍
    /// </summary>
    public bool DC_SAS_LB_Alrm;
    /// <summary>
    /// 大车前进限位两米预警
    /// </summary>
    public bool FWD_Limit_Waring;
    /// <summary>
    /// 大车后退限位两米预警
    /// </summary>
    public bool REV_Limit_Waring;
    /// <summary>
    /// 回转右转限位两度预警
    /// </summary>
    public bool Slew_R_Limit_Waring;
    /// <summary>
    ///  <summary>
    /// 回转左转限位两度预警
    /// </summary>
    public bool Slew_L_Limit_Waring;
    /// <summary>
    /// 俯仰上仰限位两度预警
    /// </summary>
    public bool Luff_U_Limit_Waring;
    /// <summary>
    /// 俯仰下俯限位两度预警
    /// </summary>
    public bool Luff_D_Limit_Waring;
    /// <summary>
    /// 回转电流过大暂停回转
    /// </summary>
    public bool Slew_Current_Pause_Sle;
    /// <summary>
    /// 斗轮电流过大暂停回转
    /// </summary>
    public bool BUCKET_Current_Pause_SLEW;
}
/// <summary>
//防碰撞限位状态
/// </summary>
public class AntiCollisionLimitStatus: StatusParmItemBase<AntiCollisionLimitData>
{
    /// <summary>
    /// 回转右转过皮带保护限位
    /// </summary>
    public ToggleDIY OverBelt_R_Limit;
    /// <summary>
    /// 回转左转过皮带保护限位
    /// </summary>
    public ToggleDIY OverBelt_L_Limit;
    /// <summary>
    /// 回转过皮带俯仰下限位保护
    /// </summary>
    public ToggleDIY OverBelt_D_Limit;
    /// <summary>
    /// 回转右转过皮带保护软限位
    /// </summary>
    public ToggleDIY OverBelt_R_SoftLimit;

    /// <summary>
    /// 回转左转过皮带保护软限位
    /// </summary>
    public ToggleDIY OverBelt_L_SoftLimit;
    /// <summary>
    /// 回转过皮带俯仰下限软限位
    /// </summary>
    public ToggleDIY OverBelt_D_SoftLimit;
    /// <summary>
    /// 大车前进停止软限位
    /// </summary>
    public ToggleDIY DC_FWD_SoftLimit;
    /// <summary>
    /// 大车前进限位集合
    /// </summary>
    public ToggleDIY DcFWD_LimitStatus;
    /// <summary>
    /// 大车后退停止软限位
    /// </summary>
    public ToggleDIY DC_REV_SoftLimit;
    /// <summary>
    /// 大车后退限位集合
    /// </summary>
    public ToggleDIY DcREV_LimitStatus;
    /// <summary>
    /// 悬臂右转软限位
    /// </summary>
    public ToggleDIY Slew_R_SoftLimit;
    /// <summary>
    /// 悬臂右转限位集合
    /// </summary>
    public ToggleDIY Slew_R_LimitStatus;
    /// <summary>
    /// 悬臂左转软限位
    /// </summary>
    public ToggleDIY Slew_L_SoftLimit;
    /// <summary>
    /// 悬臂左转限位集合
    /// </summary>
    public ToggleDIY Slew_L_LimitStatus;
    /// <summary>
    /// 俯仰上极限软限位
    /// </summary>
    public ToggleDIY Luff_Up_SoftLimit;
    /// <summary>
    /// 俯仰上限位集合
    /// </summary>
    public ToggleDIY LuffUp_LimitStatus;
    /// <summary>
    /// 俯仰下极限软限位
    /// </summary>
    public ToggleDIY Luff_Down_SoftLimit;

    /// <summary>
    /// 俯仰下限位集合
    /// </summary>
    public ToggleDIY LuffDown_LimitStatus;
    /// <summary>
    /// 悬臂左侧防撞保护动作
    /// </summary>
    public ToggleDIY Slew_SAS_L_Alarm;
    /// <summary>
    /// 悬臂右侧防撞保护动作
    /// </summary>
    public ToggleDIY Slew_SAS_R_Alarm;
    /// <summary>
    /// 大车前进防撞保护动作
    /// </summary>
    public ToggleDIY DC_SAS_F_Alarm;
    /// <summary>
    /// 大车后退防撞保护动作
    /// </summary>
    public ToggleDIY DC_SAS_B_Alarm;
    /// <summary>
    /// 悬臂右雷达前方有煤垛碰撞警告
    /// </summary>
    public ToggleDIY Slew_SAS_RR_Alarm;
    /// <summary>
    /// 悬臂左雷达前方有煤垛碰撞警告
    /// </summary>
    public ToggleDIY Slew_SAS_LR_Alarm;
    /// <summary>
    /// 悬臂右超声波前方有煤垛碰撞警告
    /// </summary>
    public ToggleDIY Slew_SAS_RU_Alarm;
    /// <summary>
    /// 悬臂左超声波前方有煤垛碰撞警告
    /// </summary>
    public ToggleDIY Slew_SAS_LU_Alarm;
    /// <summary>
    /// 大车右前方有障碍
    /// </summary>
    public ToggleDIY DC_SAS_RF_Alrm;
    /// <summary>
    /// 大车右后方有障碍
    /// </summary>
    public ToggleDIY DC_SAS_RB_Alrm;
    /// <summary>
    /// 大车左前方有障碍
    /// </summary>
    public ToggleDIY DC_SAS_LF_Alrm;
    /// <summary>
    /// 大车左后方有障碍
    /// </summary>
    public ToggleDIY DC_SAS_LB_Alrm;
    /// <summary>
    /// 大车前进限位两米预警
    /// </summary>
    public ToggleDIY FWD_Limit_Waring;
    /// <summary>
    /// 大车后退限位两米预警
    /// </summary>
    public ToggleDIY REV_Limit_Waring;
    /// <summary>
    /// 回转右转限位两度预警
    /// </summary>
    public ToggleDIY Slew_R_Limit_Waring;
    /// <summary>
    ///  <summary>
    /// 回转左转限位两度预警
    /// </summary>
    public ToggleDIY Slew_L_Limit_Waring;
    /// <summary>
    /// 俯仰上仰限位两度预警
    /// </summary>
    public ToggleDIY Luff_U_Limit_Waring;
    /// <summary>
    /// 俯仰下俯限位两度预警
    /// </summary>
    public ToggleDIY Luff_D_Limit_Waring;
    /// <summary>
    /// 回转电流过大暂停回转
    /// </summary>
    public ToggleDIY Slew_Current_Pause_Sle;
    /// <summary>
    /// 斗轮电流过大暂停回转
    /// </summary>
    public ToggleDIY BUCKET_Current_Pause_SLEW;
    public override void UpdateData(AntiCollisionLimitData data, bool isConnect = false)
    {
        SetToggleState(OverBelt_R_Limit,data.OverBelt_R_Limit,true,isConnect);
        SetToggleState(OverBelt_L_Limit,data.OverBelt_L_Limit,true,isConnect);
        SetToggleState(OverBelt_D_Limit,data.OverBelt_D_Limit,true,isConnect);
        SetToggleState(OverBelt_R_SoftLimit,data.OverBelt_R_SoftLimit,true,isConnect);
        SetToggleState(OverBelt_L_SoftLimit,data.OverBelt_L_SoftLimit,true,isConnect);
        SetToggleState(OverBelt_D_SoftLimit,data.OverBelt_D_SoftLimit,true,isConnect);
        SetToggleState(DC_FWD_SoftLimit,data.DC_FWD_SoftLimit,true,isConnect);
        SetToggleState(DcFWD_LimitStatus,data.DcFWD_LimitStatus,true,isConnect);
        SetToggleState(DC_REV_SoftLimit,data.DC_REV_SoftLimit,true,isConnect);
        SetToggleState(DcREV_LimitStatus,data.DcREV_LimitStatus,true,isConnect);
        SetToggleState(Slew_R_SoftLimit,data.Slew_R_SoftLimit,true,isConnect);
        SetToggleState(Slew_R_LimitStatus,data.Slew_R_LimitStatus,true,isConnect);
        SetToggleState(Slew_L_SoftLimit,data.Slew_L_SoftLimit,true,isConnect);
        SetToggleState(Slew_L_LimitStatus,data.Slew_L_LimitStatus,true,isConnect);
        SetToggleState(Luff_Up_SoftLimit,data.Luff_Up_SoftLimit,true,isConnect);
        SetToggleState(LuffUp_LimitStatus,data.LuffUp_LimitStatus,true,isConnect);
        SetToggleState(Luff_Down_SoftLimit,data.Luff_Down_SoftLimit,true,isConnect);
        SetToggleState(LuffDown_LimitStatus,data.LuffDown_LimitStatus,true,isConnect);
        SetToggleState(Slew_SAS_L_Alarm,data.Slew_SAS_L_Alarm,true,isConnect);
        SetToggleState(Slew_SAS_R_Alarm,data.Slew_SAS_R_Alarm,true,isConnect);
        SetToggleState(DC_SAS_F_Alarm,data.DC_SAS_F_Alarm,true,isConnect);
        SetToggleState(DC_SAS_B_Alarm,data.DC_SAS_B_Alarm,true,isConnect);
        SetToggleState(Slew_SAS_RR_Alarm,data.Slew_SAS_RR_Alarm,true,isConnect);
        SetToggleState(Slew_SAS_LR_Alarm,data.Slew_SAS_LR_Alarm,true,isConnect);
        SetToggleState(Slew_SAS_RU_Alarm,data.Slew_SAS_RU_Alarm,true,isConnect);
        SetToggleState(Slew_SAS_LU_Alarm,data.Slew_SAS_LU_Alarm,true,isConnect);
        SetToggleState(DC_SAS_RF_Alrm,data.DC_SAS_RF_Alrm,true,isConnect);
        SetToggleState(DC_SAS_RB_Alrm,data.DC_SAS_RB_Alrm,true,isConnect);
        SetToggleState(DC_SAS_LF_Alrm,data.DC_SAS_LF_Alrm,true,isConnect);
        SetToggleState(DC_SAS_LB_Alrm,data.DC_SAS_LB_Alrm,true,isConnect);
        SetToggleState(FWD_Limit_Waring,data.FWD_Limit_Waring,true,isConnect);
        SetToggleState(REV_Limit_Waring,data.REV_Limit_Waring,true,isConnect);
        SetToggleState(Slew_R_Limit_Waring,data.Slew_R_Limit_Waring,true,isConnect);
        SetToggleState(Slew_L_Limit_Waring,data.Slew_L_Limit_Waring,true,isConnect);
        SetToggleState(Luff_U_Limit_Waring,data.Luff_U_Limit_Waring,true,isConnect);
        SetToggleState(Luff_D_Limit_Waring,data.Luff_D_Limit_Waring,true,isConnect);
        SetToggleState(Slew_Current_Pause_Sle,data.Slew_Current_Pause_Sle,true,isConnect);
        SetToggleState(BUCKET_Current_Pause_SLEW,data.BUCKET_Current_Pause_SLEW,true,isConnect);
        
        
    }
}

