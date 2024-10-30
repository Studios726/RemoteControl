using System;

public enum TaskType
{
    PILEMATER,//堆料
    TAKEMATER,//取料
    None
}

public enum AutoMode
{
    AUTOMAX,//全自动
    SemiAuto//半自动
}

public  enum AngleEntryMode
{
    /// <summary>
    /// 直角
    /// </summary>
    RIGHTANGLE,
    /// <summary>
    /// 斜角
    /// </summary>
    OBLIQUEANGLE
}
public enum OperationType
{
    /// <summary>
    /// 启动
    /// </summary>
    START = 0,
    /// <summary>
    /// 暂定、和恢复
    /// </summary>
    PAUSE = 1,
    /// <summary>
    /// 换向
    /// </summary>
    REVERSING = 2,
    /// <summary>
    /// 结束
    /// </summary>
    END = 3,
    /// <summary>
    /// 恢复
    /// </summary>
    RECOVER = 4,
    /// <summary>
    /// 重置
    /// </summary>
    RESET
}

public class TaskData
{
    public string TaskID { get; set; }
    public string TaskState { get; set; }

    public TaskData(string taskID, string taskState)
    {
        TaskID = taskID;
        TaskState = taskState;
    }

    public Machine Machine { get; set; }
    private Timer _timer;

    public void AddTimer(Action action,float duration)
    {
        Cancle();
        _timer = Timer.Register(duration, action);
    }
    public void Cancle()
    {
        _timer?.Cancel();
        _timer=null;
    }
}