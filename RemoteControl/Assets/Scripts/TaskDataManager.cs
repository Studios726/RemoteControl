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

    private CommonTaskParameters _taskConfig;

    public CommonTaskParameters TaskConfig
    {
        get => _taskConfig;
    }
    private Dictionary<string, TaskData> nearestTaskDataDic = new Dictionary<string, TaskData>();
    private Dictionary<string, TaskData> curTaskDic = new Dictionary<string,TaskData>();
    public Queue<string> BucketWheelQueue = new Queue<string>();
    public Queue<string> BucketWheelStackerReclaimerQueue = new Queue<string>();
    public void SendTaskCommand(TaskCommand taskCommand)
    {
        taskCommand.CommonTaskParameters = GetCommonTaskParameters();
        MessageCenter.Instance.SendMessage(MessageType.PC, taskCommand);
    }

    public CommonTaskParameters GetCommonTaskParameters()
    {
        if (_taskConfig==null)
        {
            _taskConfig = new CommonTaskParameters();
            MySqlDataReader reader= DataManager.Instance.GetTaskConfigMc();
            while (reader.Read())
            {
                _taskConfig.HeapDis = float.Parse(reader[ConstStr.DATA_TASK_CONFIG_HEAPDOS].ToString());
                _taskConfig.MoveModel =int.Parse(reader[ConstStr.DATA_TASK_CONFIG_MOVEMODEL].ToString());
                _taskConfig.FetchPileDepth =float.Parse(reader[ConstStr.DATA_TASK_CONFIG_FETCHPILEDEPTH].ToString());;
                _taskConfig.FetchVerticalRangeAdd = float.Parse(reader[ConstStr.DATA_TASK_CONFIG_FETCHVERTICALRANGEADD].ToString());
                _taskConfig.FetchHorizontalRangeSub = float.Parse(reader[ConstStr.DATA_TASK_CONFIG_FETCHORIZONTALTANGESUB].ToString());
            }
        }
        return TaskConfig;
    }

    public void UpdateCommonTaskParameters(string name,string value)
    {
        if (_taskConfig==null)
        {
            GetCommonTaskParameters();
        }
        if (ConstStr.DATA_TASK_CONFIG_HEAPDOS==name)
        {
            _taskConfig.HeapDis=float.Parse(value);
        }else if (ConstStr.DATA_TASK_CONFIG_MOVEMODEL==name)
        {
            _taskConfig.MoveModel=int.Parse(value);
        }else if (ConstStr.DATA_TASK_CONFIG_FETCHPILEDEPTH==name)
        {
            _taskConfig.FetchPileDepth=float.Parse(value);
        }else if (ConstStr.DATA_TASK_CONFIG_FETCHVERTICALRANGEADD==name)
        {
            _taskConfig.FetchVerticalRangeAdd=float.Parse(value);
        }else if (ConstStr.DATA_TASK_CONFIG_FETCHORIZONTALTANGESUB==name)
        {
            _taskConfig.FetchHorizontalRangeSub=float.Parse(value);
        }
        DataManager.Instance.UpdateTaskConfig(name, value);
    }
    public void SendChangeTaskStateCommand(Machine machine,OperationType operationType,TaskType taskType)
    {
        TaskCommand command = new TaskCommand();
        command.QuerySystem = "MC";
        command.Machine = machine;
        command.TaskType = taskType;
        command.Command_Type = 2;
        command.OperatorSystem = "MC";
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
        taskCommand.OperatorSystem = "MC";
        MessageCenter.Instance.SendMessage(MessageType.PC, taskCommand);
        // Debug.Log("获取当前任务状态");
    }
    
    public void SetTaskVariables(TaskVariables taskVariables)
    {
        _taskVariables = taskVariables;
        if (taskVariables.Error != 0)
        {
            string tips = "";
            if (taskVariables.Error==1)
            {
                tips = "任务不存在";
            }else if(taskVariables.Error==2)
            {
                tips = "当前机器正在执行任务";
            }
            else
            {
                tips = $"任务异常稍后重试{taskVariables.Error}";
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
                // Debug.LogError($" code {taskVariables.McData[i].AllData.Code}  {taskVariables.McData[i].Machine} {taskVariables.McData[i].AllData.ProcessingProgress} ");
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
                            GameDataManager.Instance.UpdateSCAData(1);
                            if (taskVariables.McData[i].Machine==Machine.BucketWheelStackerReclaimer)
                            {
                                AddOrUpdateTaskDesQueue(0,Machine.BucketWheelStackerReclaimer);
                            }
                            else
                            {
                                AddOrUpdateTaskDesQueue(0,Machine.BucketWheel);
                            }
                         
                        }
                    }
                    else if (taskData.TaskState != taskVariables.McData[i].AllData.Code.ToString())
                    {
                        taskData.TaskState = taskVariables.McData[i].AllData.Code.ToString();
                        DataManager.Instance.UpdateHistoryTaskMc(taskData.TaskID,
                            taskData.TaskState);
                        GameDataManager.Instance.UpdateSCAData(1);
                        if (taskVariables.McData[i].Machine==Machine.BucketWheelStackerReclaimer)
                        {
                            AddOrUpdateTaskDesQueue(taskVariables.McData[i].AllData.Code,Machine.BucketWheelStackerReclaimer);
                        }
                        else
                        {
                            AddOrUpdateTaskDesQueue(taskVariables.McData[i].AllData.Code,Machine.BucketWheel);
                        }
                    }
                    else
                    {
                        if (taskVariables.McData[i].Machine==Machine.BucketWheelStackerReclaimer)
                        {
                            AddOrUpdateTaskDesQueue(taskVariables.McData[i].AllData.Code,Machine.BucketWheelStackerReclaimer);
                        }
                        else
                        {
                            AddOrUpdateTaskDesQueue(taskVariables.McData[i].AllData.Code,Machine.BucketWheel);
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
                        AddOrUpdateTaskDesQueue(-1,Machine.BucketWheelStackerReclaimer);//hard code 
                    }
                    else
                    {
                        AddOrUpdateTaskDesQueue(-1,Machine.BucketWheel);
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
                long timestamp=0;
                timestamp =taskCommand.TimedAt*60 - (long)(DateTime.Now - taskCommand.TaskCreateTime).TotalSeconds ;
                Debug.LogError($"timestamp == {timestamp}");
                if (timestamp>0)
                {
                    taskData.AddTimer(() =>
                    {
                        SendChangeTaskStateCommand(taskCommand.Machine, OperationType.END,taskCommand.TaskType);
                    }, timestamp);
                }
                else
                {
                    SendChangeTaskStateCommand(taskCommand.Machine, OperationType.END,taskCommand.TaskType);
                }
               
            }
            curTaskDic.Add(taskCommand.TaskID,taskData);
           
        }
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

    public void AddOrUpdateTaskDesQueue(int code,Machine machine)
    {
        Debug.Log($">>>>>>>code {code}");
        string des = "";
        int rank = 3;
        if (code==-1)
        {
            des ="任务进行中";
        }else if (code == 0)
        {
            des = "作业完成";
        }else if (code == 1)
        {
            des = "堆料完成，区间不可堆料";
            rank = 1;
        }else if (code == 2)
        {
            des = "取料完成，区间无料可取";
            rank = 1;
        }else if (code == 3)
        {
            des = "作业已被结束";
        }else if (code == 101)
        {
            des = "调臂区间煤堆高度超限";
            rank = 1;
        }else if (code == 102)
        {
            des = "设备限位/故障触发";
            rank = 1;
        }else if (code == 103)
        {
            des = "设备碰撞预警";
            rank = 1;
        }else if (code == 104)
        {
            des = "设备预超限位";
            rank = 1;
        }else if (code == 105)
        {
            des = "异常完成，回转超极限";
            rank = 1;
        }else if (code == 106)
        {
            des = "异常完成，俯仰超极限";
            rank = 1;
        }else if (code == 150)
        {
            des = "堆料作业暂停中";
        }
        else if (code == 151)
        {
            des = "取料作业暂停中";
        } else if (code == 201)
        {
            des = "取料中，已换向";
        }
        else if (code == 301)
        {
            des = "堆料暂停自动解除";
            rank = 1;
        }
        else if (code == 302)
        {
            des = "堆料中，暂停不可用";
            rank = 1;
        }
        else if (code == 303)
        {
            des = "取料中，暂停不可用";
            rank = 1;
        }
        else if (code == 304)
        {
            des = "取料解除暂停";
            rank = 1;
        }
        else if (code == 401)
        {
            des = "取料中，换向不可用";
            rank = 1;
        }else if (code == 402)
        {
            des = "取料换向成功";
            rank = 1;
        }
        else if (code == 1000)
        {
            des = "与远程驱动通信中断";
            rank = 1;
        }
        else if (code == 1001)
        {
            des = "堆料范围不恰当";
            rank = 1;
        }
        else if (code == 1002)
        {
            des = "取料范围不恰当";
            rank = 1;
        }
        else
        {
            des = $"错误码 {code}";
            rank = 0;
        }
        if (machine==Machine.BucketWheelStackerReclaimer)
        {
            GameDataManager.Instance.AddOrUpdateWarningDesQueue(des,Machine.BucketWheelStackerReclaimer,rank);
            
        }
        else
        {
            GameDataManager.Instance.AddOrUpdateWarningDesQueue(des,Machine.BucketWheelStackerReclaimer,rank);
        }
      
    }

    public int IsCanSendTaskCommond(Machine machine,TaskType taskType,OperationType operationType)
    {
        if (_taskVariables != null)
        {
            if ( _taskVariables.McData.Count > 0)
            {
                for (int i = 0; i < _taskVariables.McData.Count; i++)
                {
                    if (_taskVariables.McData[i].Machine==machine)
                    {
                        if (machine==Machine.BucketWheelStackerReclaimer)
                        {
                            if (_taskVariables.McData[i].TaskType==taskType||_taskVariables.McData[i].AllData.OperationCommandList[3]==1)
                            {
                                return -1;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
                return -1;
            }
            else
            {
                return -1;
            }
            
        }
        else
        {
            return -1;
        }
    
    }
}