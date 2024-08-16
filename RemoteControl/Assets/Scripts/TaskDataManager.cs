using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using RemoteControl.Event;
using UnityEngine;

public class TaskDataManager : Singleton<TaskDataManager>
{
    private TaskVariables _taskVariables;
    public TaskVariables TaskVariables
    {
        get => _taskVariables;
    }
    
    private Dictionary<string, TaskData> nearestTaskDataDic = new Dictionary<string, TaskData>();
    private Dictionary<string, TaskData> curTaskDic = new Dictionary<string,TaskData>();
    public Queue<string> BucketWheelQueue = new Queue<string>();
    public Queue<string> BucketWheelStackerReclaimer = new Queue<string>();
    public void SendTaskCommand(TaskCommand taskCommand)
    {
        MessageCenter.Instance.SendMessage(MessageType.PC, taskCommand);
    }

    public void SendChangeTaskStateCommand(Machine machine,OperationType operationType)
    {
        TaskCommand command = new TaskCommand();
        command.QuerySystem = "MC";
        command.Machine = machine;
        command.Command_Type = 2;
        command.OperationCommand = operationType;
        SendTaskCommand(command);
    }
    /// <summary>
    /// 获取任务当前状态
    /// </summary>
    public void UpdatePcData()
    {
        TaskCommand taskCommand = new TaskCommand();
        taskCommand.QuerySystem = "MC";
        taskCommand.Command_Type = 1;
        MessageCenter.Instance.SendMessage(MessageType.PC, taskCommand);
        Debug.Log("获取当前任务状态");
    }
    
    public void SetTaskVariables(TaskVariables taskVariables)
    {
        _taskVariables = taskVariables;
        if (taskVariables.Error != 0)
        {
            string tips = "";
            if (taskVariables.Error==1)
            {
                tips = "当前任务已经结束或者不存在";
            }else if(taskVariables.Error==2)
            {
                tips = "当前机器正在执行任务";
            }
            else
            {
                tips = "任务异常稍后重试";
            }
         
            UIManager.Instance.OpenUI(UIID.ConfirmPanel, new ConfirmPanelArgs(tips, null, null));
            return;
        }

        if (taskVariables.McData.Count > 0)
        {
            if (nearestTaskDataDic.Count <= 0)
            {
                GetNearestTaskDataDic();
            }

            for (int i = 0; i < taskVariables.McData.Count; i++)
            {
                AddOrUpdateTaskData(taskVariables.McData[i]);
                if (nearestTaskDataDic.ContainsKey(taskVariables.McData[i].TaskID))
                {
                    TaskData taskData = nearestTaskDataDic[taskVariables.McData[i].TaskID];
                    if (taskVariables.McData[i].AllData.Code == 0)
                    {
                        if (taskVariables.McData[i].AllData.ProcessingProgress == 1 && taskData.TaskState != "1")
                        {
                            taskData.TaskState = "1";
                            DataManager.Instance.UpdateHistoryTaskMc(taskData.TaskID, "1");
                            if (taskVariables.McData[i].Machine==Machine.BucketWheelStackerReclaimer)
                            {
                                AddOrUpdateTaskDesQueue(BucketWheelStackerReclaimer,0,Machine.BucketWheelStackerReclaimer);
                            }
                            else
                            {
                                AddOrUpdateTaskDesQueue(BucketWheelQueue,0,Machine.BucketWheel);
                            }
                         
                        }
                    }
                    else if (taskData.TaskState != taskVariables.McData[i].AllData.Code.ToString())
                    {
                        taskData.TaskState = taskVariables.McData[i].AllData.Code.ToString();
                        DataManager.Instance.UpdateHistoryTaskMc(taskData.TaskID,
                            taskData.TaskState);
                        if (taskVariables.McData[i].Machine==Machine.BucketWheelStackerReclaimer)
                        {
                            AddOrUpdateTaskDesQueue(BucketWheelStackerReclaimer,taskVariables.McData[i].AllData.Code,Machine.BucketWheelStackerReclaimer);
                        }
                        else
                        {
                            AddOrUpdateTaskDesQueue(BucketWheelQueue,taskVariables.McData[i].AllData.Code,Machine.BucketWheel);
                        }
                    }
                }
                else
                {
                    TaskCommand taskCommand = taskVariables.McData[i];
                    nearestTaskDataDic.Add(taskVariables.McData[i].TaskID,
                        new TaskData(taskVariables.McData[i].TaskID,
                            taskVariables.McData[i].AllData.ProcessingProgress.ToString()));
                    DataManager.Instance.InsertHistoryTaskMc(taskCommand, taskCommand.OperatorName,
                        taskVariables.McData[i].AllData.ProcessingProgress.ToString());
                    
                    if (taskCommand.Machine==Machine.BucketWheelStackerReclaimer)
                    {
                        AddOrUpdateTaskDesQueue(BucketWheelStackerReclaimer,-1,Machine.BucketWheelStackerReclaimer);//hard code 
                    }
                    else
                    {
                        AddOrUpdateTaskDesQueue(BucketWheelQueue,-1,Machine.BucketWheel);
                    }
                }
            }
        }

        EventManager.Instance.TriggerEvent(EventName.UpdatePcData, null);
    }

    public void AddOrUpdateTaskData(TaskCommand taskCommand)
    {
        if (curTaskDic.ContainsKey(taskCommand.TaskID))
        {
            TaskData taskData = curTaskDic[taskCommand.TaskID];
            if (taskCommand.AllData.ProcessingProgress==1||taskCommand.AllData.Code!=0)
            {
                taskData.Cancle();
                curTaskDic.Remove(taskCommand.TaskID);
            }
        }
        else
        {
            if (taskCommand.AllData.ProcessingProgress==1||taskCommand.AllData.Code!=0)
            {
                return;
            }
            
            if (curTaskDic.Count>0)
            {
                string key = "";
                foreach (KeyValuePair<string,TaskData> data in curTaskDic)
                {
                    if (data.Value.Machine==taskCommand.Machine)
                    {
                        key=data.Key;
                        data.Value.Cancle();
                    }

                }

                if (key!="")
                {
                    curTaskDic.Remove(key);
                }

            }
          
            TaskData taskData = new TaskData(taskCommand.TaskID,taskCommand.AllData.Code.ToString());
            taskData.Machine = taskCommand.Machine;
            if (taskCommand.IsTimed==true&&taskCommand.TaskType==TaskType.TAKEMATER)
            {
                DateTime parsedDateTime;
                long timestamp=0;
                if (DateTime.TryParse(taskCommand.TaskCreateTime, out parsedDateTime))
                {
                    timestamp =taskCommand.TimedAt*60 - (long)(DateTime.Now - parsedDateTime).TotalSeconds ;
                    Debug.LogError("字符串转换为 timestamp: " + timestamp);
                }

                if (timestamp>0)
                {
                    taskData.AddTimer(() =>
                    {
                        SendChangeTaskStateCommand(taskCommand.Machine, OperationType.END);
                        Debug.LogError("结束任务");
                    }, timestamp*60);
                }
                else
                {
                    SendChangeTaskStateCommand(taskCommand.Machine, OperationType.END);
                    Debug.LogError("结束任务22");
                }
               
            }
            curTaskDic.Add(taskCommand.TaskID,taskData);
           
        }
        Debug.LogError($"当前任务列表 { curTaskDic.Count}");
    }
    public Dictionary<string, TaskData> GetNearestTaskDataDic()
    {
        if (nearestTaskDataDic.Count <= 0)
        {
            MySqlDataReader mySqlDataReader = DataManager.Instance.GetHistoryTaskMc(5);
            while (mySqlDataReader.Read())
            {
                string taskID = mySqlDataReader["TaskID"].ToString();
                if (nearestTaskDataDic.ContainsKey(taskID) == false)
                {
                    nearestTaskDataDic.Add(taskID, new TaskData(taskID, mySqlDataReader["TaskState"].ToString()));
                }
            }
        }

        return nearestTaskDataDic;
    }

    public void AddOrUpdateTaskDesQueue(Queue<string> queue,int code,Machine machine)
    {
        if (queue.Count>=3)
        {
            queue.Dequeue();
        }

        string des = "";
        if (code==-1)
        {
            des = $"开始任务 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
        }else if (code == 0)
        {
            des = $"任务完成 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
        }
        else
        {
            des = $"任务异常中断 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
        }
        queue.Enqueue(des);
        if (machine==Machine.BucketWheelStackerReclaimer)
        {
            EventManager.Instance.TriggerEvent(EventName.RefreshTaskDes1,null);
        }
        else
        {
            EventManager.Instance.TriggerEvent(EventName.RefreshTaskDes2,null);
        }
      
    }
}