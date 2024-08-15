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

    public void SendTaskCommand(TaskCommand taskCommand)
    {
        MessageCenter.Instance.SendMessage<TaskCommand>(MessageType.PC, taskCommand);
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
                if (nearestTaskDataDic.ContainsKey(taskVariables.McData[i].TaskID))
                {
                    TaskData taskData = nearestTaskDataDic[taskVariables.McData[i].TaskID];
                    if (taskVariables.McData[i].AllData.Code == 0)
                    {
                        if (taskVariables.McData[i].AllData.ProcessingProgress == 1 && taskData.TaskState != "1")
                        {
                            taskData.TaskState = "1";
                            DataManager.Instance.UpdateHistoryTaskMc(taskData.TaskID, "1");
                        }
                    }
                    else if (taskData.TaskState != taskVariables.McData[i].AllData.Code.ToString())
                    {
                        DataManager.Instance.UpdateHistoryTaskMc(taskData.TaskID,
                            taskVariables.McData[i].AllData.Code.ToString());
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
                }
            }
        }

        EventManager.Instance.TriggerEvent(EventName.UpdatePcData, null);
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
}