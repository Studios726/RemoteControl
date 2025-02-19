using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using RemoteControl.Event;
using ShenYangRemoteSystem.Subclass;
using Unity.VisualScripting;
using Debug = UnityEngine.Debug;

public class TaskCodeDes
{
    public int Code { get; set; }
    public string Time { get; set; }
    public string Des { get; set; }
    public Machine Machine { get; set; }
    public List<float> NextPositionList { get; set; }
    public string Pos { get; set; }

    public TaskCodeDes(int code, string time, string des, Machine machine, List<float> list)
    {
        Code = code;
        Time =time;
        Des = des;
        Machine = machine;
        if (list == null || list.Count <= 0 || list.Count > 3)
        {
            list = new List<float> { 0, 0, 0 };
        }
        NextPositionList = new List<float>(list); // 或者 NextPositionList = list.ToList();
        StringBuilder sb = new StringBuilder("下一目标点 ");
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        sb.Append($"回转: {list[i].ToString("F1")}° ");
                        break;
                    // case 1:
                    //     sb.Append($"俯仰: {list[i].ToString("F1")}° ");
                    //     break;
                    case 2:
                        sb.Append($"前进: {list[i].ToString("F1")}m");
                        break;
                    default:
                        break;
                }
            }
        }

        Pos = sb.ToString();
    }

    public void UpdateNextPositionList(List<float> list)
    {
        NextPositionList = new List<float>(list); 
        StringBuilder sb = new StringBuilder("下一目标点 ");
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        sb.Append($"回转: {list[i].ToString("F1")}° ");
                        break;
                    // case 1:
                    //     sb.Append($"俯仰: {list[i].ToString("F1")}° ");
                    //     break;
                    case 2:
                        sb.Append($"前进: {list[i].ToString("F1")}m");
                        break;
                    default:
                        break;
                }
            }
        }
        Pos = sb.ToString();
    }
}

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
        { 598, "取料参数重设成功" },
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
        { 621, "导料槽堆料抬起需要时间" },
        { 622, "导料槽堆料停止需要时间" },
        { 623, "悬臂皮带堆料启动需要时间" },
        { 624, "悬臂皮带堆料停止需要时间" },
        { 625, "夹轨器放松需要时间" },
        { 626, "夹轨器夹紧需要时间" },
        { 627, "取料参数重设成功" },
        { 628, "中部落料斗堵煤" },
        { 629, "回转电流异常" },
        { 630, "斗轮电流异常" },
        { 631, "任务开始，启动相关设备中" },
        { 632, "相关设备启动完成" },
        { 633, "手动换向，到下一个位置继续堆料" },
        { 634, "手动换向，不能使用" },
        { 635, "俯仰油泵启动需要时间" },
        { 636, "俯仰油泵关闭需要时间" },
        { 637, "变幅升压阀打开，已经进行俯仰" },
        { 638, "变幅升压阀打开，不能进行俯仰" },
        { 639, "堆料结束调零回转完成" },
        { 640, "堆料结束调零回转异常" },
        { 641, "堆料结束调零俯仰完成" },
        { 642, "堆料结束调零俯仰异常" },
        { 643, "堆料结束调零正常移动" },
        { 644, "堆料结束调零正常回转" },
        { 645, "堆料结束调零正常俯仰" },
        { 646, "堆料结束调车开始" },
        { 647, "堆料结束调车完成" },
        { 648, "堆料结束调零异常完成" },
        { 649, "堆料调车准备开始" },
        { 650, "堆料调车完成，准备堆料" },
        { 701, "启动警铃指令已发送" },
        { 702, "关闭警铃指令已发送" },
        { 703, "启动警铃需要时间" },
        { 704, "关闭警铃需要时间" },
        { 705, "启动警铃完成" },
        { 706, "关闭警铃完成" },
        { 801, "左右范围重设要大于12°" },
        { 802, "左右范围重设前后相同" },
        { 803, "收尾阶段，只可修改步长" },
        { 804, "步长重设前后相同" },
        { 805, "重设的取料范围超限" },
        { 806, "取料范围重设前后相同" },
        { 807, "步长重设成功" },
        { 808, "取料范围重设成功" },
        { 809, "左右范围重设成功" },
        { 810, "获取取料远端角度成功" },
        { 811, "当前可确认远端边界" },
        { 812, "当前不可确认远端边界" },
        { 813, "未确认边界，默认为远端角度" },
        { 814, "臂与煤可能碰撞，大车停止" },
        { 815, "臂与煤可能碰撞，停止回转" },
        { 816, "臂与煤可能碰撞，停止俯仰" },
        { 817, "调车完成，结束任务" },
        { 818, "直接执行取料" },
        { 819, "等待用户选择是否继续执行取料" },
        { 820, "主动换层" },
        { 821, "臂在取料范围内不需要调零" },
        { 822, "调车完成" },
        { 823, "斗轮在取料范围前" },
        { 824, "斗轮在取料范围中" },
        { 825, "斗轮在取料范围后" },
        { 826, "定位完成，可以取料" },
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
        { 1017, "斗轮机故障" },
        { 1018, "远程切本地" },
        { 1019, "远程状态下自动切单动" },
        { 1020, "堆料或取料变为停止" },
        { 1021, "与系统解锁" },
        { 1022, "允许取料信号消失" },
        { 1023, "剩余取料高度比煤堆高度高" },
        { 1024, "堆料中大臂不在设置范围内" },
        { 1025, "当前任务未设置初始方向" },
        { 1026, "上次取料任务未确认边界" },
        { 1027, "与上次取料任务不匹配" },
        { 1028, "斗轮不在上次任务范围内" },
        { 1029, "允许堆料信号消失" },
        { 1030, "取料没有三维数据，任务结束" },
        { 1031, "堆料没有三维数据，任务结束" },
        { 1032, "取料没有臂上雷达数据，任务结束" },
        { 1500, "任务规划与远程驱动断开连接，任务结束" },
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
        { 2019, "变幅油泵故障解除" },
        { 2020, "plc满足取料前提条件" },
        { 2021, "斗轮机故障解除" }
    };
    private static object o = new object();
    public ConcurrentDictionary<string, List<TaskCodeDes>> taskCodeDesDictionary =
        new ConcurrentDictionary<string, List<TaskCodeDes>>();

    private TaskVariables _taskVariables;
    public string TestStr;
    public List<string> TaskMessageList = new List<string>();
    public Queue<List<TaskLogCellData>> PileTakeLogQueue = new Queue<List<TaskLogCellData>>();
    public Queue<List<TaskLogCellData>> TakeLogQueue = new Queue<List<TaskLogCellData>>();
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
    private Dictionary<string, TaskData> nearestPileTaskDataDic = new Dictionary<string, TaskData>();
    private Dictionary<string, TaskData> curTaskDic = new Dictionary<string, TaskData>();

    // private Dictionary<string,List<int>>
    public void SendTaskCommand(TaskCommand taskCommand)
    {
        taskCommand.CommonTaskParameters = GetCommonTaskParameters(taskCommand.Machine);
        taskCommand.ReversingValueList  = new List<float>()
        {
            taskCommand.CommonTaskParameters.BucketLidarDisLeft, taskCommand.CommonTaskParameters.BucketLidarDisRight
        };
        MessageCenter.Instance.SendMessage(MessageType.PC, taskCommand);
    }

    public void SendTaskLidarDis(float left, float right,Machine machine)
    {
        TaskCommand taskCommand = new TaskCommand();
        taskCommand.Command_Type= 7;
        taskCommand.Machine= machine;
        taskCommand.ReversingValueList = new List<float>() { left, right };
        MessageCenter.Instance.SendMessage(MessageType.PC, taskCommand);
        Debug.Log($"SendTaskLidarDis:{left},{right} {machine}");
    }
    public CommonTaskParameters GetCommonTaskParameters(Machine machine)
    {
        CommonTaskParameters commonTaskParameters = new CommonTaskParameters();
        DataSet dataSet=DataManager.Instance.GetTaskConfigMcData(machine);
        if (dataSet!=null)
        {
            DataRowCollection dataRowCollection = dataSet.Tables[0].Rows;
            for (int i = 0; i < dataRowCollection.Count; i++)
            {
                commonTaskParameters.HeapDis = float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_HEAPDOS].ToString());
                commonTaskParameters.MoveModel = int.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_MOVEMODEL].ToString());
                commonTaskParameters.FetchPileDepth =
                    float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_FETCHPILEDEPTH].ToString());
                commonTaskParameters.FetchVerticalRangeAdd =
                    float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_FETCHVERTICALRANGEADD].ToString());
                commonTaskParameters.FetchHorizontalRangeSub =
                    float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_FETCHORIZONTALTANGESUB].ToString());
                commonTaskParameters.BucketLidarDisLeft =
                    float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_REVERSALSETLEFT].ToString());
                commonTaskParameters.BucketLidarDisRight =
                    float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_REVERSALSETRIGHT].ToString());
            }
        }
        return commonTaskParameters;
    }

    public void UpdateCommonTaskParameters(string name, string value, Machine machine)
    {
        DataManager.Instance.UpdateTaskConfig(name, value, machine);
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
    public void UpdateTaskData(int type = 1)
    {
        TaskCommand taskCommand = new TaskCommand();
        taskCommand.QuerySystem = "MC";
        taskCommand.Command_Type = type;
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

            UIManager.Instance.OpenUI(UIID.ConfirmPanel, new ConfirmPanelArgs(tips,"任务规划", null, null));
            return;
        }
        EventManager.Instance.TriggerEvent(EventName.UpdatePcData, null);
        if (nearestTaskDataDic.Count <=0)
        {
            GetNearestTaskDataDic();
        }

        try
        {
            CheckTaskDesDesDictionary(taskVariables);
            // 触发事件
        }
        catch (Exception e)
        {
            taskCodeDesDictionary?.Clear();
            Debug.LogError($"数据更新失败 {e.Message} ");
            // 创建栈跟踪对象
            StackTrace stackTrace = new StackTrace(e, true);
            foreach (var frame in stackTrace.GetFrames())
            {
                Debug.LogError($"Method: {frame.GetMethod().Name}, Line: {frame.GetFileLineNumber()}>>>");
            }
        }
        EventManager.Instance.TriggerEvent(EventName.RefreshTaskDes1, this, null);
        EventManager.Instance.TriggerEvent(EventName.RefreshTaskDes2, this, null);
        if (taskVariables.McData.Count > 0)
        {
            for (int i = 0; i < taskVariables.McData.Count; i++)
            {
               
                if (nearestTaskDataDic.ContainsKey(taskVariables.McData[i].TaskID))
                {
                    TaskData taskData = nearestTaskDataDic[taskVariables.McData[i].TaskID];
                    // Debug.LogError($">>TaskEndTime: {taskVariables.McData[i].AllData.TaskEndTime}  结束状态：{taskVariables.McData[i].AllData.OperationCommandList[3]}");
                    if (taskVariables.McData[i].AllData.Code == 0)
                    {
                        if (taskVariables.McData[i].AllData.ProcessingProgress == 1 && taskData.TaskState != "1")
                        {
                            taskData.TaskState = "1";
                            DataManager.Instance.UpdateHistoryTaskMc(taskData.TaskID, "1");
                            DataManager.Instance.UpdateHistoryTaskMcCompleteState(taskVariables.McData[i].TaskID,
                                TaskStatus.Completed,
                                taskVariables.McData[i].AllData.TaskEndTime);
                            // GameDataManager.Instance.UpdateSCAData(1);
                        }
                    }
                    else if (taskData.TaskState != taskVariables.McData[i].AllData.Code.ToString())
                    {
                        taskData.TaskState = taskVariables.McData[i].AllData.Code.ToString();
                        DataManager.Instance.UpdateHistoryTaskMc(taskData.TaskID,
                            taskData.TaskState);
                    }

                    if (taskData.State != TaskStatus.Completed &&
                        taskVariables.McData[i].AllData.OperationCommandList[3] == 1&&taskVariables.McData[i].AllData.TaskEndTime!="") //任务结束更新数据库
                    {
                        taskData.State = TaskStatus.Completed;
                        DataManager.Instance.UpdateHistoryTaskMcCompleteState(taskVariables.McData[i].TaskID,
                            TaskStatus.Completed,
                            taskVariables.McData[i].AllData.TaskEndTime);
                    }
                }
                else
                {
                    TaskCommand taskCommand = taskVariables.McData[i];
                    nearestTaskDataDic.Add(taskVariables.McData[i].TaskID,
                        new TaskData(taskVariables.McData[i].TaskID,
                            taskVariables.McData[i].AllData.ProcessingProgress.ToString(), TaskStatus.InProgress));
                   
                    if (taskVariables.McData[i].AllData.OperationCommandList[3] == 1) //任务结束更新数据库
                    {
                        nearestTaskDataDic[taskVariables.McData[i].TaskID].State = TaskStatus.Completed;
                        DataManager.Instance.InsertHistoryTaskMc(taskCommand, taskCommand.OperatorName,
                            taskVariables.McData[i].AllData.ProcessingProgress.ToString(),"2");
                    }
                    else
                    {
                        nearestTaskDataDic[taskVariables.McData[i].TaskID].State = TaskStatus.InProgress;
                        DataManager.Instance.InsertHistoryTaskMc(taskCommand, taskCommand.OperatorName,
                            taskVariables.McData[i].AllData.ProcessingProgress.ToString());
                    }
                }
                PopConfirmPanelByTaskCode(taskVariables.McData[i].AllData.Code);
            }
        }
        else
        {
            foreach (var data in nearestTaskDataDic) //任务规划崩溃处理最近任务完成状态
            {
                if (data.Value.State != TaskStatus.Completed)
                {
                    data.Value.State = TaskStatus.Completed;
                    DataManager.Instance.UpdateHistoryTaskMcCompleteState(data.Value.TaskID, TaskStatus.Completed,
                        "");
                }
            }
        }

    }

    public void PopConfirmPanelByTaskCode(int code)
    {
        //1002，1015，1032，826
        if (code==1002||code==1015||code==1032||code==826)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel, new ConfirmPanelArgs(GetDesByTaskCode(code),"任务规划", null, null));
        }
    }
    //处理定时任务
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
            // if (taskCommand.IsTimed == true && taskCommand.TaskType == TaskType.TAKEMATER)
            // {
            //     long timestamp = 0;
            //     timestamp = taskCommand.TimedAt * 60 - (long)(DateTime.Now - taskCommand.TaskCreateTime).TotalSeconds;
            //     if (timestamp > 0)
            //     {
            //         taskData.AddTimer(
            //             () =>
            //             {
            //                 SendChangeTaskStateCommand(taskCommand.Machine, OperationType.END, taskCommand.TaskType);
            //             }, timestamp);
            //     }
            //     else
            //     {
            //         SendChangeTaskStateCommand(taskCommand.Machine, OperationType.END, taskCommand.TaskType);
            //     }
            // }

            curTaskDic.Add(taskCommand.TaskID, taskData);
        }
    }

    public Dictionary<string, TaskData> GetNearestTaskDataDic()
    {
        if (nearestTaskDataDic.Count <=0)
        {
            DataSet dataSet = DataManager.Instance.GetHistoryTaskMcByTaskType(TaskType.TAKEMATER,5);
            if (dataSet!=null)
            {
                DataRowCollection dataRowCollection = dataSet.Tables[0].Rows;
                for (int i = 0; i < dataRowCollection.Count; i++)
                {
                    string taskID = dataRowCollection[i][ConstStr.DATA_TASK_ID].ToString();
                    if (nearestTaskDataDic.ContainsKey(taskID) == false)
                    {
                        nearestTaskDataDic.Add(taskID,
                            new TaskData(taskID, dataRowCollection[i][ConstStr.DATA_TASK_STATE].ToString(),
                                (TaskStatus)Enum.Parse(typeof(TaskStatus),
                                    dataRowCollection[i][ConstStr.DATA_TASK_STATE2].ToString())));
                    }
                }
            }
        }

        return nearestTaskDataDic;
    }

    public void CheckTaskDesDesDictionary(TaskVariables taskVariables)
    {
        lock (o)
        {
            if (taskVariables.McData.Count > 0) // 把旧数据删除
            {
                if (taskCodeDesDictionary != null && taskCodeDesDictionary.Count > 0)
                {
                    HashSet<string> taskIDsToRemove = new HashSet<string>();
            
                    foreach (var data in taskCodeDesDictionary)
                    {
                        if (!taskVariables.McData.Any(mcData => mcData.TaskID == data.Key))
                        {
                            taskIDsToRemove.Add(data.Key);
                        }
                    }
            
                    foreach (var taskId in taskIDsToRemove)
                    {
                        taskCodeDesDictionary.TryRemove(taskId, out _);
                    }
                }

                for (int i = 0; i < taskVariables.McData.Count; i++) // 刷新code
                {
                    if (taskVariables.McData[i].AllData.Code==598)//任务重设成功，修改数据库数据
                    {
                        DataManager.Instance.UpdateResetTaskParams(taskVariables.McData[i]);
                    }
                    AddOrUpdateTaskDesDictionary(GetDesByTaskCode(taskVariables.McData[i].AllData.Code),
                        taskVariables.McData[i]);
                }

                AddTakeLogQueue();
            }
            else
            {
                taskCodeDesDictionary?.Clear();
                AddTakeLogQueue();
                EventManager.Instance.TriggerEvent(EventName.RefreshTaskDes1, this, null);
                EventManager.Instance.TriggerEvent(EventName.RefreshTaskDes2, this, null);
            }
        }
    }

    public void AddTakeLogQueue()
    {
        List<TaskLogCellData> pileTakeData = new List<TaskLogCellData>();
        List<TaskLogCellData> takeData = new List<TaskLogCellData>();
        ConcurrentDictionary<string, List<TaskCodeDes>> taskCodeDesDictionary = TaskDataManager.Instance?.taskCodeDesDictionary;
        if (taskCodeDesDictionary != null&&taskCodeDesDictionary.Count > 0)
        {
            foreach (var data in taskCodeDesDictionary)
            {
                for (int i = 0; i < data.Value.Count; i++)
                {
                    if (data.Value[i].Machine == Machine.BucketWheelStackerReclaimer)
                    {
                        pileTakeData.Add(new TaskLogCellData("", data.Value[i].Des, data.Value[i].Time,
                            Machine.BucketWheelStackerReclaimer, data.Value[i].Pos));
                    }else if (data.Value[i].Machine == Machine.BucketWheel)
                    {
                        takeData.Add(new TaskLogCellData("", data.Value[i].Des, data.Value[i].Time,
                            Machine.BucketWheelStackerReclaimer, data.Value[i].Pos));
                    }
                }
            }

            pileTakeData.Reverse();
            takeData.Reverse();
            PileTakeLogQueue.Enqueue(pileTakeData);
            TakeLogQueue.Enqueue(takeData);
        }
    }
    private void AddTaskCodeDesToList(List<TaskCodeDes> taskList, int code, string codeTime, string des,
        Machine machine, List<float> nextPositionList)
    {
        if (codeTime == null)
        {
            codeTime = "";
        }

        if (des==null)
        {
            des = "";
        }
        taskList.Add(new TaskCodeDes(code, codeTime, des, machine, nextPositionList));
    }

    public void AddOrUpdateTaskDesDictionary(string des, TaskCommand taskCommand)
    {
        if (taskCommand == null || taskCommand.AllData == null)
        {
            Debug.LogError("TaskCommand or its AllData property cannot be null.");
            return;
        }
        
        var taskList = taskCodeDesDictionary.GetOrAdd(taskCommand.TaskID, _ => new List<TaskCodeDes>());
        // 检查是否存在相同的任务代码和机器
        if (taskList.Any(tcd =>
                tcd.Code == taskCommand.AllData.Code && tcd.Time == taskCommand.AllData.CodeTime &&
                tcd.Machine == taskCommand.Machine))
        {
            for (int i = 0; i < taskCodeDesDictionary[taskCommand.TaskID].Count; i++)
            {
                if (taskCodeDesDictionary[taskCommand.TaskID][i].Code==taskCommand.AllData.Code&&taskCodeDesDictionary[taskCommand.TaskID][i].Machine==taskCommand.Machine&&taskCodeDesDictionary[taskCommand.TaskID][i].Time==taskCommand.AllData.CodeTime)
                {
                    taskCodeDesDictionary[taskCommand.TaskID][i].UpdateNextPositionList(taskCommand.AllData.NextPositionList);
                }
            }
        }
        else
        {
            // 添加新的任务代码描述
            AddTaskCodeDesToList(taskList, taskCommand.AllData.Code, taskCommand.AllData.CodeTime, des, taskCommand.Machine,
                taskCommand.AllData.NextPositionList);
        }
    }

    public string GetDesByTaskCode(int code)
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

        return des;
    }

    public void f(int code, Machine machine, TaskCommand taskCommand)
    {
        // TODO 待完善
        // string des = "";
        // if (_codeDescriptions.TryGetValue(code, out string description))
        // {
        //     des = description;
        // }
        // else
        // {
        //     des = $"错误码 {code}";
        // }
        // if (machine == Machine.BucketWheelStackerReclaimer)
        // {
        //     EventManager.Instance.TriggerEvent(EventName.RefreshTaskDes1, this, new TaskLogArgs(des, taskCommand));
        // }
        // else
        // {
        //     EventManager.Instance.TriggerEvent(EventName.RefreshTaskDes2, this, new TaskLogArgs(des, taskCommand));
        // }
    }

    public bool IsCanSet(Machine machine)
    {
        if (_taskVariables==null||_taskVariables.McData.Count<=0)
        {
            return true;
        }
        else
        {
            for (int i = 0; i < _taskVariables.McData.Count; i++)
            {
                if (_taskVariables.McData[i].Machine==machine)
                {
                    if (_taskVariables.McData[i].AllData.OperationCommandList[3]==1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        return true;
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
    /// <summary>
    /// 添加堆料任务
    /// </summary>
    /// <param name="isPile"></param>
    public void AddPileMaterialTask(bool isPile,SystemVariables systemVariables )
    {
        Debug.Log("开始堆料任务");
        GetNearestPileTaskDataDic();
        string taskId = DateTime.Now.ToString("yyMMddHHmmss");
        if (nearestPileTaskDataDic.ContainsKey(taskId)==false)
        {
            nearestPileTaskDataDic.Add(taskId,
                new TaskData(taskId, "0"));
        }
        AutoMode autoMode=systemVariables.SR1_AutoBorder_Enable?AutoMode.AUTOMAX:AutoMode.SemiAuto;
        string pileMode = systemVariables.SR1_SlewStack_SEL ? "回转堆料" : "定点堆料";
        string sideSelection=systemVariables.SR1_SEL_WorkArea.ToString()=="1"?"RIGHT" : "LEFT";
        DataManager.Instance.InsertHistoryTaskPileMc(DateTime.Now, Machine.BucketWheelStackerReclaimer, TaskType.PILEMATER,systemVariables.SR1_Stack_Start_Pos,systemVariables.SR1_Stack_End_Pos,systemVariables.SR1_Stack_LeftBorder_SP,systemVariables.SR1_Stack_RightBorder_SP,systemVariables.SR1_Stack_DcRevSize,taskId,systemVariables.SR1_Stack_HighSet,autoMode,pileMode,sideSelection);
    }
    /// <summary>
    /// 更新堆料任务
    /// </summary>
    /// <param name="isEnd"></param>
    public void UpdatePileTakeMaterialTask(bool isEnd, SystemVariables systemVariables)
    {
        if (isEnd)
        {
            GetNearestPileTaskDataDic();
            Debug.Log("结束堆料任务");
            foreach (var data in nearestPileTaskDataDic)
            {
                if (data.Value.State != TaskStatus.Completed)
                {
                    data.Value.State = TaskStatus.Completed;
                    DataManager.Instance.UpdateHistoryTaskPileMc(data.Value.TaskID,
                        ((int)TaskStatus.Completed).ToString(),
                        DateTime.Now);
                }
            }
        }
    }
    
    public void GetNearestPileTaskDataDic()
    {
        if (nearestPileTaskDataDic.Count <=0)
        {
            DataSet dataSet = DataManager.Instance.GetHistoryTaskMcByTaskType(TaskType.PILEMATER,5);
            if (dataSet!=null)
            {
                DataRowCollection dataRowCollection = dataSet.Tables[0].Rows;
                for (int i = 0; i < dataRowCollection.Count; i++)
                {
                    string taskID = dataRowCollection[i][ConstStr.DATA_TASK_ID].ToString();
                    if (nearestPileTaskDataDic.ContainsKey(taskID) == false)
                    {
                        nearestPileTaskDataDic.Add(taskID,
                            new TaskData(taskID, dataRowCollection[i][ConstStr.DATA_TASK_STATE].ToString(),
                                (TaskStatus)Enum.Parse(typeof(TaskStatus),
                                    dataRowCollection[i][ConstStr.DATA_TASK_STATE2].ToString())));
                    }
                }
            }
        }
    }
}