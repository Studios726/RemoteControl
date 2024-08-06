using System;
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
    //任务ID
    public int TaskID {  get; set; }
    //操作系統
    public string OperatorSystem { get; set; }
    public Machine Machine { get; set; }
    public TaskType TaskType { get; set; }
    //启动0 暂停1 换向2 结束3
    public OperationType OperationCommand { get; set; }
    // 取料范围，可以是一个区间
    public TaskRange MaterialRange { get; set; }

    // 左右侧选择，例如 Left 或 Right
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


