using ShangHaiPro;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;

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
    public string QuerySystem { get; set; }
    //任务ID 0发任务 1获取任务当前状态 2修改当前任务状态
    public int Command_Type { get; set; }
    //任务ID
    public string TaskID {  get; set; }
    //发布任务者
    public string OperatorName { get; set; }
    //操作系統 MC WEB
    public string OperatorSystem { get; set; }
    //0 堆取料机 1 取料机
    public Machine Machine { get; set; }
    //0 堆料 1 取料
    public TaskType TaskType { get; set; }
    //启动0 暂停1 换向2 结束3
    public OperationType OperationCommand { get; set; }
    // 取料范围，可以是一个区间
    public TaskRange MaterialRange { get; set; }

    // 左右侧选择，例如 LEFT 或 RIGHT
    public string SideSelection { get; set; }

    // 左右范围，分别对应左侧和右侧的范围
    public TaskRange LeftRightRange { get; set; }

    // 取料步长
    public float StepLength { get; set; }

    // 是否定时
    public bool IsTimed { get; set; }

    // 定时时间，如果IsTimed为true，则此字段有效
    public int TimedAt { get; set; }

    // 是否定量
    public bool IsQuantified { get; set; }

    // 定量多少，如果IsQuantified为true，则此字段有效
    public int Quantity { get; set; }
    //堆料高度
    public int TakeMateHigh {  get; set; }
    //层高
    public float LayerHigh {  get; set; }
    public AllData  AllData { get; set; }
    public CommonTaskParameters CommonTaskParameters{ get; set; }
}
public class AllData
{
    //public string InfoIcon { get; set; }   
    public int Code {  get; set; }
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


