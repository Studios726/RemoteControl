
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


[DataContract]
//远程驱动命令类
public class ServerCommand
{
    [DataMember]
    public string QUERY_SYSTEM { get; set; }
    //正在控制/查询的子系统的ID
    [DataMember]
    public int DATA_TYPE { get; set; }
    //数据类型：1-流量，2-运动参数，3-三维料堆，4-安全信息，5-任务规划信息，6-远程驱动系统交互
    [DataMember]
    public int QUERY_TYPE { get; set; }
    //命令类型：0-提供给本地服务器端以初始化的数据，1-PLC的一帧数据，2-命令模式--30三维料堆模型  31报表
    [DataMember]
    public string COMMAND_NAME { get; set; }
    //命令名：该名字即为“基本指令表.xlsx”中的“英文指令名”
    [DataMember]
    public string DATA_STRING { get; set; }
    //数据字符串：用于存放向其他系统转发的内容（暂无实现）
    [DataMember]
    public int DATA_INT { get; set; }
    //数据整型：0-False，1-True
    [DataMember]
    public float DATA_FLOAT { get; set; }
    //数据浮点型：度数，深度
}
public enum COMMAND_NAME
{
    /// <summary>
    /// 回转左转
    /// </summary>
    ROTATE_LEFT,
    /// <summary>
    /// 回转右转
    /// </summary>
    ROTATE_RIGHT,
    /// <summary>
    /// 回转停止
    /// </summary>
    ROTATE_STOP,
    /// <summary>
    /// 俯仰上仰
    /// </summary>
    ELEVATE_UP,
    /// <summary>
    /// 俯仰下附
    /// </summary>
    ELEVATE_DOWN,
    /// <summary>
    /// 俯仰停止
    /// </summary>
    ELEVATE_STOP,
    /// <summary>
    /// 大车前进
    /// </summary>
    MOVE_FORWARD,
    /// <summary>
    /// 大车后退
    /// </summary>
    MOVE_BACKWARD,
    /// <summary>
    /// 大车停止
    /// </summary>
    MOVE_STOP,
    /// <summary>
    /// 夹轨器放松
    /// </summary>
    RAIL_RELAX,
    /// <summary>
    /// 夹轨器夹紧
    /// </summary>
    RAIL_CLAMP,
    /// <summary>
    /// 动力电源合闸
    /// </summary>
    SUPPLYPOWER_ON,
    /// <summary>
    /// 动力电源分闸
    /// </summary>
    SUPPLYPOWER_OFF,
    /// <summary>
    /// 控制电源合闸
    /// </summary>
    CONTROLPOWER_ON,
    /// <summary>
    /// 控制电源分闸
    /// </summary>
    CONTROLPOWER_OFF,
    /// <summary>
    /// 悬臂皮带取料
    /// </summary>
    BELT_TAKE,
    /// <summary>
    /// 悬臂皮带堆料
    /// </summary>
    BELT_STACK,
    /// <summary>
    /// 悬臂皮带停止
    /// </summary>
    BELT_STOP,
    /// <summary>
    /// 斗轮启动
    /// </summary>
    BUCKET_START,
    /// <summary>
    /// 斗轮停止
    /// </summary>
    BUCKET_STOP,
    /// <summary>
    /// 照明合闸
    /// </summary>
    LIGHTPOWER_ON,
    /// <summary>
    /// 照明分闸
    /// </summary>
    LIGHTPOWER_OFF,
    /// <summary>
    /// 主车油泵启动
    /// </summary>
    OILBUMP_ON,
    /// <summary>
    /// 主车油泵关闭
    /// </summary>
    OILBUMP_OFF,
    /// <summary>
    /// #取料开关
    /// </summary>
    BELTTAKE_BUTTON,
    /// <summary>
    /// 堆料开关
    /// </summary>
    BELTSTACK_BUTTON,
    /// <summary>
    /// 堆取料停止开关
    /// </summary>
    BELTSSTOP_BUTTON,
    /// <summary>
    /// 与系统连锁解锁
    /// </summary>
    SYSTEM_UNLOCK,
    /// <summary>
    /// 与系统连锁连锁
    /// </summary>
    SYSTEM_LOCK,
    /// <summary>
    /// 悬臂头部导料槽抬起（堆料）
    /// </summary>
    XBTB_UP_BUTTON,
    /// <summary>
    /// 悬臂头部导料槽落下（取料）
    /// </summary>
    XBTB_DOWN_BUTTON,
    /// <summary>
    /// 悬臂头部导料槽停止
    /// </summary>
    XBTB_STOP_BUTTON,
    /// <summary>
    /// 振打器启动
    /// </summary>
    VIBRATOR_START,
    /// <summary>
    /// 振打器停止
    /// </summary>
    VIBRATOR_STOP,
    /// <summary>
    /// 上位急停
    /// </summary>
    EMERGENCY_STOP,
    /// <summary>
    /// 上位机故障复位
    /// </summary>
    ERR_RESET,
    /// <summary>
    /// 上位机旁路
    /// </summary>
    BYPASS_BUTTON,
    /// <summary>
    /// 取料大车步长递增按钮
    /// </summary>
    STEP_SIZE_INC_1,
    /// <summary>
    /// 取料大车步长递减按钮
    /// </summary>
    STEP_SIZE_DES_1,
    /// <summary>
    /// 启车报警
    /// </summary>
    STARTUP_ALARM,
    /// <summary>
    /// 挡板取料变换启动
    /// </summary>
    SKRIT_TAKE_START,
    /// <summary>
    /// 挡板分流变换停止
    /// </summary>
    SKRIT_TAKE_STOP,
    /// <summary>
    /// 挡板堆料变换启动
    /// </summary>
    SKRIT_STACK_START,
    /// <summary>
    /// 变幅油加热器启动
    /// </summary>
    LUFF_HART_START,
    /// <summary>
    /// 变幅油加热器停止
    /// </summary>
    LUFF_HART_STOP,
    /// <summary>
    /// 变幅风机启动
    /// </summary>
    LUFF_FAN_START,
    /// <summary>
    /// 变幅风机停止
    /// </summary>
    LUFF_FAN_STOP,
    /// <summary>
    /// 大车行走慢速
    /// </summary>
    TRAVEL_SPEED_SLOW,
    /// <summary>
    /// 大车行走快速
    /// </summary>
    TRAVEL_SPEED_FAST,
    /// <summary>
    /// 单动
    /// </summary>
    MODE_A,
    /// <summary>
    /// 联动
    /// </summary>
    MODE_B,
    /// <summary>
    /// 自动
    /// </summary>
    MODE_C,
    /// <summary>
    /// 大车右前防碰撞设定
    /// </summary>
    CAR_RIGHT_FRONT_SET,
    /// <summary>
    /// 大车左前防碰撞设定
    /// </summary>
    CAR_LEFT_FRONT_SET,
    /// <summary>
    /// 大车右后防碰撞设定
    /// </summary>
    CAR_RIGHT_BACK_SET,
    /// <summary>
    /// 大车左后防碰撞设定
    /// </summary>
    CAR_LEFT_BACK_SET,
    /// <summary>
    /// 大车前进极限设定
    /// </summary>
    FORWARD_LIMIT_SET,
    /// <summary>
    /// 大车后退极限设定
    /// </summary>
    BACKWARD_LIMIT_SET,
    /// <summary>
    /// 右转极限设定
    /// </summary>
    RIGHT_LIMIT_SET,
    /// <summary>
    /// 左转极限设定
    /// </summary>
    LEFT_LIMIT_SET,
    /// <summary>
    /// 上仰极限设定
    /// </summary>
    UP_LIMIT_SET,
    /// <summary>
    /// 下附极限设定
    /// </summary>
    DOWN_LIMIT_SET,
    /// <summary>
    /// 大车防撞保护旁路
    /// </summary>
    BTCP_BYPASS,
    /// <summary>
    /// 悬臂防撞保护旁路
    /// </summary>
    ABP_BYPASS,
    /// <summary>
    /// 大车位置软保护旁路
    /// </summary>
    BTPP_BYPASS,
    /// <summary>
    /// 回转角度软保护旁路
    /// </summary>
    RAP_BYPASS,
    /// <summary>
    /// 俯仰角度软保护旁路
    /// </summary>
    PAP_BYPASS,
    /// <summary>
    /// 大车右前防撞保护旁路
    /// </summary>
    BTRFCP_BYPASS,
    /// <summary>
    /// 大车左前防撞保护旁路
    /// </summary>
    BTLFCP_BYPASS,
    /// <summary>
    /// 大车右后防撞保护旁路
    /// </summary>
    BTRRCP_BYPASS,
    /// <summary>
    /// 大车左后防撞保护旁路
    /// </summary>
    BTLRCP_BYPASS,
    /// <summary>
    /// 悬臂右前超声波防撞保护旁路
    /// </summary>
    ARFRUCP_BYPASS,
    /// <summary>
    /// 悬臂右中超声波防撞保护旁路
    /// </summary>
    ARMUCP_BYPASS,
    /// <summary>
    /// 悬臂右后超声波防撞保护旁路
    /// </summary>
    ARREUCP_BYPASS,
    /// <summary>
    /// 悬臂右前防撞超声波保护距离设定
    /// </summary>
    ULT_RIGHT_FRONT_SET,
    /// <summary>
    /// 悬臂右中防撞超声波保护距离设定
    /// </summary>
    ULT_RIGHT_MIDDLE_SET,
    /// <summary>
    /// 悬臂右后防撞超声波保护距离设定
    /// </summary>
    ULT_RIGHT_BACK_SET,
    /// <summary>
    /// 悬臂左前防撞超声波保护距离设定
    /// </summary>
    ULT_LEFT_FRONT_SET,
    /// <summary>
    /// 悬臂左中防撞超声波保护距离设定
    /// </summary>
    ULT_LEFT_MIDDLE_SET,
    /// <summary>
    /// 悬臂左后防撞超声波保护距离设定
    /// </summary>
    ULT_LEFT_BACK_SET,
    /// <summary>
    /// 悬臂左前超声波防撞保护旁路
    /// </summary>
    SASLF_BYPASS,
    /// <summary>
    /// 悬臂左中超声波防撞保护旁路
    /// </summary>
    SASLM_BYPASS,
    /// <summary>
    /// 悬臂左后超声波防撞保护旁路
    /// </summary>
    SASLB_BYPASS,
    /// <summary>
    /// 取料任务暂停
    /// </summary>
    TAKE_PAUSE,
    /// <summary>
    /// 堆料任务暂停
    /// </summary>
    STACK_PAUSE,
    /// <summary>
    /// 步进前进
    /// </summary>
    CAR_FORWARD,
    /// <summary>
    /// 步进后退
    /// </summary>
    CAR_REVERSE,
    /// <summary>
    /// 步进数值设定
    /// </summary>
    CAR_FIXSIZE_SET
    
}
public static class ServerCommandDataType
{
    public const int FLOW = 1;
    public const int MOTION = 2;
    public const int SCAN = 3;
    public const int SECURITY = 4;
    public const int TASK = 5;
    public const int REMOTE = 6;
    
}

public class TaskCommand
{
    public int ID { get; set; }
    public string QuerySystem { get; set; }
    //任务ID 0发任务 1获取任务当前状态 2修改当前任务状态 3边界确认
    public int Command_Type { get; set; }
    //0新任务 1恢复上次任务
    public int IsTaskContinued{ get; set; }
    //任务ID
    public string TaskID {  get; set; }
    //任务创建时间
    public DateTime TaskCreateTime { get; set; }
    //web 专属
    public string nowState { get; set; }
    // public 
    //发布任务者
    public string OperatorName { get; set; }
    //操作系統 MC WEB
    public string OperatorSystem { get; set; }
    //0 堆取料机 1 取料机
    public Machine Machine { get; set; }
    //0 堆料 1 取料
    public TaskType TaskType { get; set; }
    //自动模式
    public AutoMode AutoMode{ get; set;}
    public AngleEntryMode AngleEntryMode{ get; set; }
    //启动0 暂停1 换向2 结束3,边界确认 6
    public OperationType OperationCommand { get; set; }
    //任务参数重置 点击发送 1
    public int ResetState { get; set; }
    //左转右转边界确认按钮 点击发送 1
    public int TurnConfirmState{ get; set;}
    // 取料范围，可以是一个区间
    public TaskRange MaterialRange { get; set; }

    // 左右侧选择，例如 LEFT 或 RIGHT
    public string SideSelection { get; set; }

    // 左右范围，分别对应左侧和右侧的范围
    public TaskRange LeftRightRange { get; set; }

    // 取料步长
    public float StepLength { get; set; }

    // public string
    // 是否定时
    public bool IsTimed { get; set; }

    // 定时时间，如果IsTimed为true，则此字段有效
    public int TimedAt { get; set; }

    // 是否定量
    // public bool IsQuantified { get; set; }

    // 定量多少，如果IsQuantified为true，则此字段有效
    // public int Quantity { get; set; }
    //取料高度
    // public float TakeMateHigh {  get; set; }1
    //堆料高度
    public float PileMateHigh {  get; set; }
    //层高
    // public float LayerHigh {  get; set; }  
    //1 左转 2 右转
    public TurnMode TurnMode{  get; set; }
    public AllData  AllData { get; set; }
    public CommonTaskParameters CommonTaskParameters{ get; set; }
    //任务结束执行方式[调零，关设备] 0 否 1 是
    public List<int> FinishMethod {  get; set; }
}
public class AllData
{
    //public string InfoIcon { get; set; }   
    public int Code {  get; set; }
    //code 触发code时间
     public DateTime CodeTime{ get; set; }
     //任务结束时间
     public DateTime TaskEndTime{ get; set; }
    // [回转，俯仰，前进]
    public List<float> NextPositionList {  get; set; }
    public int ProcessingProgress {  get; set; }
    public List<int> OperationCommandList {  get; set; }
}

public class CommonTaskParameters
{
    /// <summary>
    /// 定点堆的距离（堆料间隔）
    /// </summary>
    public float HeapDis {  get; set; }
    /// <summary>
    /// 斗轮机根据工作范围按照就近原则还是工作范围中第一个数据，默认就近原则，数值为0（就近堆料   起始点堆料）
    /// </summary>
    public int MoveModel{ get; set; }
    /// <summary>
    /// 斗轮机取料时每层下降的深度（取料分层高度）
    /// </summary>
    public float FetchPileDepth{ get; set; }
    /// <summary>
    ///  根据第一次三维的数据情况，获取第一次要刮取的范围后，往两侧增加（左右范围增加的长度）
    /// </summary>
    public float FetchVerticalRangeAdd{ get; set; }
    /// <summary>
    /// 斗轮机取料时沿着轨道的工作范围每取一层左右缩减的距离（沿着轨道方向的取料范围缩减）
    /// </summary>
    public float FetchHorizontalRangeSub{ get; set; }
}
public class TaskRange
{
    public float startValue;
    public  float endValue;

    public TaskRange(float startValue, float endValue)
    {
        this.startValue = startValue;
        this.endValue = endValue;
    }
}


