using System;

public enum TaskType
{
    PILEMATER,
    TAKEMATER
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
    RECOVER = 4
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