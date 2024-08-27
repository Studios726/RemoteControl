using System;

public enum TaskType
{
    PILEMATER,
    TAKEMATER
}

public enum OperationType
{
    START = 0,//启动
    PAUSE = 1,//暂定、和恢复
    REVERSING = 2,//换向
    END = 3,//结束
    RECOVER = 4//恢复
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