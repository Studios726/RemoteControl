using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using RemoteControl.Event;
using UnityEngine;

public class TaskDataManager : Singleton<TaskDataManager>
{
    private static readonly Dictionary<int, string> _codeDescriptions = new Dictionary<int, string>
    {
        { -1, "任务进行中" },
        { 0, "作业完成" },
        { 1, "堆料完成，区间不可堆料" },
        { 2, "取料完成，区间无料可取" },
        { 3, "作业已被结束" },
        { 4, "控制模式由自动切换为手动，任务结束" },
        { 101, "调臂区间煤堆高度超限" },
        { 102, "设备限位/故障触发" },
        { 103, "设备碰撞预警" },
        { 104, "设备预超限位" },
        { 105, "异常完成，回转超极限" },
        { 106, "异常完成，俯仰超极限" },
        { 150, "堆料作业暂停中" },
        { 151, "取料作业暂停中" },
        { 201, "取料中，已换向" },
        { 301, "堆料暂停自动解除" },
        { 302, "堆料中，暂停不可用" },
        { 303, "取料中，暂停不可用" },
        { 304, "取料中，暂停已解除" },
        { 401, "取料中，换向不可用" },
        { 402, "取料中，已换向" },
        { 501, "取料任务规划中" },
        { 502, "取料任务规划完成" },
        { 503, "夹轨器放松指令已发送" },
        { 504, "夹轨器放松完成" },
        { 505, "夹轨器夹紧指令已发送" },
        { 506, "夹轨器夹紧完成" },
        { 507, "收到允许取料信号，开始取料" },
        { 508, "未收到允许取料信号，等待中" },
        { 509, "俯仰油泵启动指令已发送" },
        { 510, "俯仰油泵启动完成" },
        { 511, "俯仰油泵停止指令已发送" },
        { 512, "俯仰油泵停止完成" },
        { 513, "导料槽取料落下指令已发送" },
        { 514, "导料槽取料落下完成" },
        { 515, "导料槽停止指令已发送" },
        { 516, "导料槽停止完成" },
        { 517, "悬臂皮带取料启动指令已发送" },
        { 518, "悬臂皮带取料启动完成" },
        { 519, "悬臂皮带停止指令已发送" },
        { 520, "悬臂皮带停止完成" },
        { 521, "斗轮启动指令已发送" },
        { 522, "斗轮启动完成" },
        { 523, "斗轮停止指令已发送" },
        { 524, "斗轮停止完成" },
        { 525, "振打器启动指令已发送" },
        { 526, "振打器启动完成" },
        { 527, "振打器停止指令已发送" },
        { 528, "振打器停止完成" },
        { 529, "油泵升压中，暂停和换向不可用" },
        { 530, "俯仰油泵升压完成" },
        { 531, "导料槽取料落下中" },
        { 532, "导料槽停止中" },
        { 533, "回转电流异常" },
        { 534, "斗轮电流异常" },
        { 535, "大车前进指令已发送" },
        { 536, "大车前进运行中" },
        { 537, "大车后退指令已发送" },
        { 538, "大车后退运行中" },
        { 539, "回转左转指令已发送" },
        { 540, "回转左转运行中" },
        { 541, "回转右转指令已发送" },
        { 542, "回转右转运行中" },
        { 543, "回转停止指令已发送" },
        { 544, "回转停止运行" },
        { 545, "俯仰上仰指令已发送" },
        { 546, "俯仰上仰运行中" },
        { 547, "俯仰下俯指令已发送" },
        { 548, "俯仰下俯运行中" },
        { 549, "俯仰停止指令已发送" },
        { 550, "俯仰停止运行" },
        { 551, "夹轨器放松中" },
        { 552, "夹轨器夹紧中" },
        { 553, "俯仰油泵启动中" },
        { 554, "俯仰油泵停止中" },
        { 555, "悬臂皮带取料启动中" },
        { 556, "悬臂皮带停止中" },
        { 557, "斗轮启动中" },
        { 558, "斗轮停止中" },
        { 559, "振打器启动中" },
        { 560, "振打器停止中" },
        { 561, "大车前进启动中" },
        { 562, "大车停止中" },
        { 563, "大车后退启动中" },
        { 564, "回转左转启动中" },
        { 565, "回转停止中" },
        { 566, "回转右转启动中" },
        { 567, "俯仰上仰启动中" },
        { 568, "俯仰停止中" },
        { 569, "俯仰下俯启动中" },
        { 570, "边界已自动换向" },
        { 571, "任务开始，启动相关设备中" },
        { 572, "相关设备启动完成" },
        { 573, "任务结束，停止相关设备中" },
        { 574, "相关设备停止完成" },
        { 575, "俯仰油泵压力到位，俯仰可用" },
        { 576, "俯仰油泵压力未到位，俯仰不可用" },
        { 577, "导料槽取料位已到达" },
        { 578, "导料槽取料位未到达" },
        { 579, "大车停止指令已发送" },
        { 580, "大车停止运行" },
        { 581, "取料结束，大车位置调整中" },
        { 582, "取料结束，回转位置调整中" },
        { 583, "取料结束，俯仰位置调整中" },
        { 584, "取料调车准备开始" },
        { 585, "取料调车完成，准备取料" },
        { 586, "取料结束调车开始" },
        { 587, "取料结束调车完成" },
        { 588, "取料结束调零大车后退开始" },
        { 589, "取料结束调零大车后退完成" },
        { 590, "取料结束调零大车后退异常" },
        { 591, "取料结束调零回转完成" },
        { 592, "取料结束调零回转异常" },
        { 593, "取料结束调零俯仰完成" },
        { 594, "取料结束调零俯仰异常" },
        { 595, "取料结束调零异常完成" },
        { 596, "升压阀已打开可俯仰" },
        { 597, "升压阀未打开不能俯仰" },
        { 598, "参数重置成功" },
        { 599, "目前正在获取三维数据" },
        { 600, "成功获取三维数据" },
        { 601, "堆料任务规划中" },
        { 602, "堆料任务规划已完成" },
        { 603, "夹轨器放松指令已发送" },
        { 604, "夹轨器放松完成" },
        { 605, "夹轨器夹紧指令已发送" },
        { 606, "夹轨器夹紧完成" },
        { 607, "收到允许堆料信号，开始堆料" },
        { 608, "未收到允许堆料信号，等待中" },
        { 609, "俯仰油泵启动指令已发送" },
        { 610, "俯仰油泵启动完成" },
        { 611, "俯仰油泵停止指令已发送" },
        { 612, "俯仰油泵停止运行" },
        { 613, "导料槽堆料抬起指令已发送" },
        { 614, "导料槽堆料抬起完成" },
        { 615, "导料槽停止指令已发送" },
        { 616, "导料槽停止运行" },
        { 617, "悬臂皮带堆料启动指令已发送" },
        { 618, "悬臂皮带堆料启动完成" },
        { 619, "悬臂皮带停止指令已发送" },
        { 620, "悬臂皮带停止运行" },
        { 621, "分流挡板堆料落下指令已发送" },
        { 622, "分流挡板堆料落下完成" },
        { 623, "分流挡板分流抬起指令已发送" },
        { 624, "分流挡板分流抬起完成" },
        { 625, "分流挡板停止指令已发送" },
        { 626, "分流挡板停止运行" },
        { 627, "允许分流信号已收到" },
        { 628, "中部落料斗堵煤" },
        { 629, "回转电流异常" },
        { 630, "斗轮电流异常" },
        { 1000, "与远程驱动通信中断" },
        { 1001, "堆料范围不恰当" },
        { 1002, "取料范围不恰当" },
        { 1003, "悬臂皮带故障" },
        { 1004, "斗轮故障" },
        { 1005, "大车故障" },
        { 1006, "回转故障" },
        { 1007, "变幅故障" },
        { 1008, "夹轨器故障" },
        { 1009, "导料槽故障" },
        { 1010, "分流挡板故障" },
        { 1011, "振打器故障" },
        { 1012, "保护故障" },
        { 1013, "变幅油泵电机故障" },
        { 1014, "取料中出现设备故障" },
        { 1015, "斗轮机未到达取料范围" },
        { 1016, "斗轮机故障，详情查看综合监控" },
        { 2001, "取料回转电流异常解除" },
        { 2002, "堆料回转电流异常解除" },
        { 2003, "取料斗轮电流异常解除" },
        { 2004, "堆料斗轮电流异常解除" },
        { 2005, "设备碰撞异常解除" },
        { 2006, "大车行走极限异常解除" },
        { 2007, "回转极限异常解除" },
        { 2008, "俯仰极限异常解除" },
        { 2009, "悬臂皮带故障解除" },
        { 2010, "斗轮故障解除" },
        { 2011, "大车故障解除" },
        { 2012, "回转故障解除" },
        { 2013, "变幅故障解除" },
        { 2014, "夹轨器故障解除" },
        { 2015, "导料槽故障解除" },
        { 2016, "分流挡板故障解除" },
        { 2017, "振打器故障解除" },
        { 2018, "保护故障解除" },
        { 2019, "变幅油泵故障解除" }
    };

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
    private Dictionary<string, TaskData> curTaskDic = new Dictionary<string, TaskData>();
    public Queue<string> BucketWheelQueue = new Queue<string>();
    public Queue<string> BucketWheelStackerReclaimerQueue = new Queue<string>();

    public void SendTaskCommand(TaskCommand taskCommand)
    {
        taskCommand.CommonTaskParameters = GetCommonTaskParameters();
        MessageCenter.Instance.SendMessage(MessageType.PC, taskCommand);
    }

    public CommonTaskParameters GetCommonTaskParameters()
    {
        if (_taskConfig == null)
        {
            _taskConfig = new CommonTaskParameters();
            MySqlDataReader reader = DataManager.Instance.GetTaskConfigMc();
            while (reader.Read())
            {
                _taskConfig.HeapDis = float.Parse(reader[ConstStr.DATA_TASK_CONFIG_HEAPDOS].ToString());
                _taskConfig.MoveModel = int.Parse(reader[ConstStr.DATA_TASK_CONFIG_MOVEMODEL].ToString());
                _taskConfig.FetchPileDepth = float.Parse(reader[ConstStr.DATA_TASK_CONFIG_FETCHPILEDEPTH].ToString());
                ;
                _taskConfig.FetchVerticalRangeAdd =
                    float.Parse(reader[ConstStr.DATA_TASK_CONFIG_FETCHVERTICALRANGEADD].ToString());
                _taskConfig.FetchHorizontalRangeSub =
                    float.Parse(reader[ConstStr.DATA_TASK_CONFIG_FETCHORIZONTALTANGESUB].ToString());
            }
        }

        return TaskConfig;
    }

    public void UpdateCommonTaskParameters(string name, string value)
    {
        if (_taskConfig == null)
        {
            GetCommonTaskParameters();
        }

        if (ConstStr.DATA_TASK_CONFIG_HEAPDOS == name)
        {
            _taskConfig.HeapDis = float.Parse(value);
        }
        else if (ConstStr.DATA_TASK_CONFIG_MOVEMODEL == name)
        {
            _taskConfig.MoveModel = int.Parse(value);
        }
        else if (ConstStr.DATA_TASK_CONFIG_FETCHPILEDEPTH == name)
        {
            _taskConfig.FetchPileDepth = float.Parse(value);
        }
        else if (ConstStr.DATA_TASK_CONFIG_FETCHVERTICALRANGEADD == name)
        {
            _taskConfig.FetchVerticalRangeAdd = float.Parse(value);
        }
        else if (ConstStr.DATA_TASK_CONFIG_FETCHORIZONTALTANGESUB == name)
        {
            _taskConfig.FetchHorizontalRangeSub = float.Parse(value);
        }

        DataManager.Instance.UpdateTaskConfig(name, value);
    }

    public void SendChangeTaskStateCommand(Machine machine, OperationType operationType, TaskType taskType)
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
    public void UpdateTaskData()
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
            if (taskVariables.Error == 1)
            {
                tips = "任务不存在";
            }
            else if (taskVariables.Error == 2)
            {
                tips = "当前机器正在执行任务";
            }
            else if (taskVariables.Error == 3)
            {
                tips = $"当前任务无法执行该操作，请检查相关作业条件";
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
                            if (taskVariables.McData[i].Machine == Machine.BucketWheelStackerReclaimer)
                            {
                                AddOrUpdateTaskDesQueue(0, Machine.BucketWheelStackerReclaimer);
                            }
                            else
                            {
                                AddOrUpdateTaskDesQueue(0, Machine.BucketWheel);
                            }
                        }
                    }
                    else if (taskData.TaskState != taskVariables.McData[i].AllData.Code.ToString())
                    {
                        taskData.TaskState = taskVariables.McData[i].AllData.Code.ToString();
                        DataManager.Instance.UpdateHistoryTaskMc(taskData.TaskID,
                            taskData.TaskState);
                        GameDataManager.Instance.UpdateSCAData(1);
                        if (taskVariables.McData[i].Machine == Machine.BucketWheelStackerReclaimer)
                        {
                            AddOrUpdateTaskDesQueue(taskVariables.McData[i].AllData.Code,
                                Machine.BucketWheelStackerReclaimer);
                        }
                        else
                        {
                            AddOrUpdateTaskDesQueue(taskVariables.McData[i].AllData.Code, Machine.BucketWheel);
                        }
                    }
                    else
                    {
                        if (taskVariables.McData[i].Machine == Machine.BucketWheelStackerReclaimer)
                        {
                            AddOrUpdateTaskDesQueue(taskVariables.McData[i].AllData.Code,
                                Machine.BucketWheelStackerReclaimer);
                        }
                        else
                        {
                            AddOrUpdateTaskDesQueue(taskVariables.McData[i].AllData.Code, Machine.BucketWheel);
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

                    if (taskCommand.Machine == Machine.BucketWheelStackerReclaimer)
                    {
                        AddOrUpdateTaskDesQueue(-1, Machine.BucketWheelStackerReclaimer); //hard code 
                    }
                    else
                    {
                        AddOrUpdateTaskDesQueue(-1, Machine.BucketWheel);
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
            if (taskCommand.AllData.ProcessingProgress == 1 || taskCommand.AllData.Code != 0)
            {
                taskData.Cancle();
                curTaskDic.Remove(taskCommand.TaskID);
            }
        }
        else
        {
            if (taskCommand.AllData.ProcessingProgress == 1 || taskCommand.AllData.Code != 0)
            {
                return;
            }

            if (curTaskDic.Count > 0)
            {
                string key = "";
                foreach (KeyValuePair<string, TaskData> data in curTaskDic)
                {
                    if (data.Value.Machine == taskCommand.Machine)
                    {
                        key = data.Key;
                        data.Value.Cancle();
                    }
                }

                if (key != "")
                {
                    curTaskDic.Remove(key);
                }
            }

            TaskData taskData = new TaskData(taskCommand.TaskID, taskCommand.AllData.Code.ToString());
            taskData.Machine = taskCommand.Machine;
            if (taskCommand.IsTimed == true && taskCommand.TaskType == TaskType.TAKEMATER)
            {
                long timestamp = 0;
                timestamp = taskCommand.TimedAt * 60 - (long)(DateTime.Now - taskCommand.TaskCreateTime).TotalSeconds;
                Debug.LogError($"timestamp == {timestamp}");
                if (timestamp > 0)
                {
                    taskData.AddTimer(
                        () =>
                        {
                            SendChangeTaskStateCommand(taskCommand.Machine, OperationType.END, taskCommand.TaskType);
                        }, timestamp);
                }
                else
                {
                    SendChangeTaskStateCommand(taskCommand.Machine, OperationType.END, taskCommand.TaskType);
                }
            }

            curTaskDic.Add(taskCommand.TaskID, taskData);
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

    public void AddOrUpdateTaskDesQueue(int code, Machine machine)
    {
        string des = "";
        if (_codeDescriptions.TryGetValue(code, out string description))
        {
            des = description;
        }
        else
        {
            des = $"错误码 {code}";
        }

        GameDataManager.Instance.AddOrUpdateWarningDesDict(code.ToString(), des,
            machine, false, "");
    }

    public int IsCanSendTaskCommond(Machine machine, TaskType taskType, OperationType operationType)
    {
        if (_taskVariables != null)
        {
            if (_taskVariables.McData.Count > 0)
            {
                for (int i = 0; i < _taskVariables.McData.Count; i++)
                {
                    if (_taskVariables.McData[i].Machine == machine)
                    {
                        if (machine == Machine.BucketWheelStackerReclaimer)
                        {
                            if (_taskVariables.McData[i].TaskType == taskType ||
                                _taskVariables.McData[i].AllData.OperationCommandList[3] == 1)
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