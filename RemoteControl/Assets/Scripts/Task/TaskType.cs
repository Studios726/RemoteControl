public enum TaskType
{
    PILEMATER,
    TAKEMATER
}
public enum OperationType
{
    START=0,
    PAUSE=1,
    REVERSING=2,
    END=3,
    RECOVER=4
}
public class TaskData
{
    public string TaskID { get; set; }
    public string TaskState { get; set; }
    public TaskData(string taskID, string taskState)
    {
        TaskID=taskID;
        TaskState=taskState;    
    }
}