using System;

public enum TaskType
{
    PILEMATER,
    TAKEMATER
}

public enum OperationType
{
    START = 0,
    PAUSE = 1,
    REVERSING = 2,
    END = 3,
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
        _timer = Timer.Register(5, action);
    }
    public void Cancle()
    {
        _timer?.Cancel();
        _timer=null;
    }
}