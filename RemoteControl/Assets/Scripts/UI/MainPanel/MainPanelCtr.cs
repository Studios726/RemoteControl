using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using RemoteControl.Event;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class MainPanelCtr : UIPresenter<MainPanelView>
{
    private readonly static object lockObject = new object();
    private readonly static object lockObject2 = new object();
    private readonly static object lockWarningObject = new object();
    public Timer TaskLogTimer;
    public override void ShowView(UIArgs uiArgs = null)
    {
        base.ShowView(uiArgs);
        Addlistener();
        Timer.Register(0.1f, () =>
            {
                try
                {
                    GameDataManager.Instance.SetMachineActive(true);
                    view.UpdateData(null,null);
                    view.UpdatePcData(null, null);
                    UpdateWarningDes1(null, null);
                    UpdateWarningDes2(null, null);
                  
                }
                catch (Exception e)
                {
                    Debug.LogError($"主界面打开逻辑处理{e.Message}");
                }
              
            }
        );
        if (TaskLogTimer==null)
        {
            TaskLogTimer = Timer.Register(1, true, false, (() =>
            {
                // List<TaskLogCellData> data = new List<TaskLogCellData>();
                // int count = Random.Range(0, 100);
                // for (int i = 0; i < count; i++)
                // {
                //     data.Add(new TaskLogCellData("",i.ToString(),DateTime.Now.ToString("h:mm:ss"),Machine.BucketWheelStackerReclaimer,"888888888"));
                // }
                // view._bucketWheelTask1.UpdateTaskLog(data);
                UpdateTaskLog1(null,null);
                UpdateTaskLog2(null,null);
            }));
        }
    }

    public override void HideView()
    {
        base.HideView();
        GameDataManager.Instance.SetMachineActive(false);
    }

    public override void SetPanelData(UIArgs uiArgs)
    {
   
    }

    public void UpdateWarningDes1(object o, EventArgs eventArgs)
    {
        lock (lockWarningObject)
        {
            try
            {
                List<WarningCellData> datas = new List<WarningCellData>();
                if (GameDataManager.Instance.WarningCellDataDict.Count > 0)
                {
                    foreach (var keyCellData in GameDataManager.Instance.WarningCellDataDict)
                    {
                        if (keyCellData.Value.Machine == Machine.BucketWheelStackerReclaimer)
                        {
                            datas.Add(keyCellData.Value);
                        }
                    }
                }

                int n = datas.Count;
                if (n>=1)
                {
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = 0; j < n - i - 1; j++)
                        {
                            if (datas[j].Timestamp < datas[j + 1].Timestamp)
                            {
                                WarningCellData temp = datas[j];
                                datas[j] = datas[j + 1];
                                datas[j + 1] = temp;
                            }
                        }
                    }
                }
                view._bucketWheelTask1.UpdateDes(datas);
            }
            catch (Exception e)
            {
                Debug.LogError("数据更新失败");
            }
        }
    }

    public void UpdateWarningDes2(object o, EventArgs eventArgs)
    {
        lock (lockWarningObject)
        {
            try
            {
                List<WarningCellData> datas = new List<WarningCellData>();
                if (GameDataManager.Instance.WarningCellDataDict.Count > 0)
                {
                    foreach (var keyCellData in GameDataManager.Instance.WarningCellDataDict)
                    {
                        if (keyCellData.Value.Machine == Machine.BucketWheel)
                        {
                            datas.Add(keyCellData.Value);
                        }
                    }
                }

                int n = datas.Count;
                if (n>=1)
                {
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = 0; j < n - i - 1; j++)
                        {
                            if (datas[j].Timestamp < datas[j + 1].Timestamp)
                            {
                                WarningCellData temp = datas[j];
                                datas[j] = datas[j + 1];
                                datas[j + 1] = temp;
                            }
                        }
                    }
                }
                view._bucketWheelTask2.UpdateDes(datas);
            }
            catch (Exception e)
            {
                Debug.LogError("数据更新失败");
            }
        }
    }

    public void UpdateTaskLog1(object o, EventArgs eventArgs)
    {
        try
        {
            if (TaskDataManager.Instance.PileTakeLogQueue!=null&&TaskDataManager.Instance.PileTakeLogQueue.Count>0)
            {
                List<TaskLogCellData> data = TaskDataManager.Instance.PileTakeLogQueue.Dequeue();
                view._bucketWheelTask1.UpdateTaskLog(data);
            }
        }
        catch (Exception e)
        {
            // 创建栈跟踪对象
            StackTrace stackTrace = new StackTrace(e, true);
            string test = "";
            foreach (var frame in stackTrace.GetFrames())
            {
                test = test + $"Method: {frame.GetMethod().Name}, Line: {frame.GetFileLineNumber()}>>>";
            }
            TaskDataManager.Instance.TestStr =$"<1> {DateTime.Now.ToString("HH:mm:ss")} {e.Message} {test}";
            EventManager.Instance.TriggerEvent(EventName.TestEvent);
            // TaskDataManager.Instance.UpdateTaskData(4);
            Debug.LogError($"MainPanel UpdateTaskLog {e.Message} ");
        }
      
        // lock (lockObject)
        // {
        //     try
        //     {
        //         List<TaskLogCellData> datas = new List<TaskLogCellData>();
        //         ConcurrentDictionary<string, List<TaskCodeDes>> taskCodeDesDictionary = TaskDataManager.Instance?.taskCodeDesDictionary;
        //         if (taskCodeDesDictionary != null&&taskCodeDesDictionary.Count > 0)
        //         {
        //             foreach (var data in taskCodeDesDictionary)
        //             {
        //                 for (int i = 0; i < data.Value.Count; i++)
        //                 {
        //                     if (data.Value[i].Machine == Machine.BucketWheelStackerReclaimer)
        //                     {
        //                         datas.Add(new TaskLogCellData("", data.Value[i].Des, data.Value[i].Time,
        //                             Machine.BucketWheelStackerReclaimer, data.Value[i].Pos));
        //                     }
        //                 }
        //             }
        //
        //             datas.Reverse();
        //         }
        //         view._bucketWheelTask1.UpdateTaskLog(datas);
        //     }
        //     catch (Exception e)
        //     {
        //         StackTrace stackTrace = new StackTrace(e, true);
        //         string test = "";
        //         foreach (var frame in stackTrace.GetFrames())
        //         {
        //             test = test + $"Method: {frame.GetMethod().Name}, Line: {frame.GetFileLineNumber()}>>>";
        //         }
        //         TaskDataManager.Instance.TestStr =$"<2>{e.Message} {test}";
        //         EventManager.Instance.TriggerEvent(EventName.TestEvent);
        //         TaskDataManager.Instance.UpdateTaskData(4);
        //         Debug.LogError($"MainPanel UpdateTaskLog1 {e.Message} ");
        //     }
        // }
       
        // TaskLogArgs args = (TaskLogArgs)eventArgs;
       
    }

    public void UpdateTaskLog2(object o, EventArgs eventArgs)
    {
        try
        {
            if (TaskDataManager.Instance.TakeLogQueue!=null&&TaskDataManager.Instance.TakeLogQueue.Count>0)
            {
                List<TaskLogCellData> data = TaskDataManager.Instance.TakeLogQueue.Dequeue();
                view._bucketWheelTask2.UpdateTaskLog(data);
            }
        }
        catch (Exception e)
        {
            // // 创建栈跟踪对象
            StackTrace stackTrace = new StackTrace(e, true);
            string test = "";
            foreach (var frame in stackTrace.GetFrames())
            {
                test = test + $"Method: {frame.GetMethod().Name}, Line: {frame.GetFileLineNumber()}>>>";
            }
            // string str = JsonMgr.Serialize(TaskDataManager.Instance.taskCodeDesDictionary);
            TaskDataManager.Instance.TestStr =$"<1> {DateTime.Now.ToString("HH:mm:ss")} {e.Message} {test}";
            EventManager.Instance.TriggerEvent(EventName.TestEvent);
            // TaskDataManager.Instance.UpdateTaskData(4);
            Debug.LogError($"MainPanel UpdateTaskLog {e.Message} ");
        }
       
        // lock (lockObject2)
        // {
        //     try
        //     {
        //         List<TaskLogCellData> datas = new List<TaskLogCellData>();
        //         ConcurrentDictionary<string, List<TaskCodeDes>> taskCodeDesDictionary = TaskDataManager.Instance?.taskCodeDesDictionary;
        //         if (taskCodeDesDictionary != null&&taskCodeDesDictionary.Count>0)
        //         {
        //             foreach (var data in taskCodeDesDictionary)
        //             {
        //                 for (int i = 0; i < data.Value.Count; i++)
        //                 {
        //                     if (data.Value[i].Machine == Machine.BucketWheel)
        //                     {
        //                         datas.Add(new TaskLogCellData("", data.Value[i].Des, data.Value[i].Time, Machine.BucketWheel,
        //                             data.Value[i].Pos));
        //                     }
        //                 }
        //             }
        //             datas.Reverse();
        //         }
        //         view._bucketWheelTask2.UpdateTaskLog(datas);
        //     }
        //     catch (Exception e)
        //     {
        //         // 创建栈跟踪对象
        //         StackTrace stackTrace = new StackTrace(e, true);
        //         string test = "";
        //         foreach (var frame in stackTrace.GetFrames())
        //         {
        //             test = test + $"Method: {frame.GetMethod().Name}, Line: {frame.GetFileLineNumber()}>>>";
        //         }
        //         // string str = JsonMgr.Serialize(TaskDataManager.Instance.taskCodeDesDictionary);
        //         TaskDataManager.Instance.TestStr =$"<1>{e.Message} {test}";
        //         EventManager.Instance.TriggerEvent(EventName.TestEvent);
        //         TaskDataManager.Instance.UpdateTaskData(4);
        //         Debug.LogError($"MainPanel UpdateTaskLog2 {e.Message} ");
        //     }
        // }
    }

    public override void Dispose()
    {
        EventManager.Instance.RemoveListener(EventName.UpdateRcData, view.UpdateData);
        EventManager.Instance.RemoveListener(EventName.UpdatePcData, view.UpdatePcData);
        EventManager.Instance.RemoveListener(EventName.RefreshWarningDes1, UpdateWarningDes1);
        EventManager.Instance.RemoveListener(EventName.RefreshWarningDes2, UpdateWarningDes2);
        EventManager.Instance.RemoveListener(EventName.RefreshTaskDes1, UpdateTaskLog1);
        EventManager.Instance.RemoveListener(EventName.RefreshTaskDes2, UpdateTaskLog2);
        EventManager.Instance.RemoveListener(EventName.TestEvent, view.SetTestInputField);
        if (TaskLogTimer!=null)
        {
            TaskLogTimer.Cancel();
            TaskLogTimer = null;
        }
    }

    public void Addlistener()
    {
        EventManager.Instance.AddListener(EventName.UpdateRcData, view.UpdateData);
        EventManager.Instance.AddListener(EventName.UpdatePcData, view.UpdatePcData);
        EventManager.Instance.AddListener(EventName.RefreshWarningDes1, UpdateWarningDes1);
        EventManager.Instance.AddListener(EventName.RefreshWarningDes2, UpdateWarningDes2);
        EventManager.Instance.AddListener(EventName.RefreshTaskDes1, UpdateTaskLog1);
        EventManager.Instance.AddListener(EventName.RefreshTaskDes2, UpdateTaskLog2);
        EventManager.Instance.AddListener(EventName.TestEvent, view.SetTestInputField);
        EventManager.Instance.AddListener(EventName.ShowTestEvent,view.ShowTestInput);
    }

    public void SendMessage(string message)
    {
        Debug.Log($"SendMessage  {message}");
    }
}