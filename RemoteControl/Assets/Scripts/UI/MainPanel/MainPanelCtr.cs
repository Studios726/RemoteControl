using System;
using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using UnityEngine;

public class MainPanelCtr : UIPresenter<MainPanelView>
{
    private static object lockObject = new object();
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
    }

    public override void HideView()
    {
        base.HideView();
        GameDataManager.Instance.SetMachineActive(false);
    }

    public override void SetPanelData(UIArgs uiArgs)
    {
        // view.UpdateData(null,null);
        // view.UpdatePcData(null, null);
        // UpdateWarningDes1(null, null);
        // UpdateWarningDes2(null, null);
    }

    public void UpdateWarningDes1(object o, EventArgs eventArgs)
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

    public void UpdateWarningDes2(object o, EventArgs eventArgs)
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

    public void UpdateTaskLog1(object o, EventArgs eventArgs)
    {
        lock (lockObject)
        {
            try
            {
                List<TaskLogCellData> datas = new List<TaskLogCellData>();
                if (TaskDataManager.Instance.taskCodeDesDictionary.Count > 0)
                {
                    foreach (var data in TaskDataManager.Instance.taskCodeDesDictionary)
                    {
                        for (int i = 0; i < data.Value.Count; i++)
                        {
                            if (data.Value[i].Machine == Machine.BucketWheelStackerReclaimer)
                            {
                                datas.Add(new TaskLogCellData("", data.Value[i].Des, data.Value[i].Time,
                                    Machine.BucketWheelStackerReclaimer, data.Value[i].Pos));
                            }
                        }
                    }

                    datas.Reverse();
                }
                view._bucketWheelTask1.UpdateTaskLog(datas);
            }
            catch (Exception e)
            {
                // string str = JsonMgr.Serialize(TaskDataManager.Instance.taskCodeDesDictionary);
                TaskDataManager.Instance.TestStr =$"<2>{e.Message}";
                EventManager.Instance.TriggerEvent(EventName.TestEvent);
                TaskDataManager.Instance.UpdateTaskData(4);
                Debug.LogError($"MainPanel UpdateTaskLog1 {e.Message} ");
            }
        }
       
        // TaskLogArgs args = (TaskLogArgs)eventArgs;
       
    }

    public void UpdateTaskLog2(object o, EventArgs eventArgs)
    {
        lock (lockObject)
        {
            try
            {
                List<TaskLogCellData> datas = new List<TaskLogCellData>();
                if (TaskDataManager.Instance.taskCodeDesDictionary.Count>0)
                {
                    foreach (var data in TaskDataManager.Instance.taskCodeDesDictionary)
                    {
                        for (int i = 0; i < data.Value.Count; i++)
                        {
                            if (data.Value[i].Machine == Machine.BucketWheel)
                            {
                                datas.Add(new TaskLogCellData("", data.Value[i].Des, data.Value[i].Time, Machine.BucketWheel,
                                    data.Value[i].Pos));
                            }
                        }
                    }
                    datas.Reverse();
                }
                view._bucketWheelTask2.UpdateTaskLog(datas);
            }
            catch (Exception e)
            {
                // string str = JsonMgr.Serialize(TaskDataManager.Instance.taskCodeDesDictionary);
                TaskDataManager.Instance.TestStr =$"<1>{e.Message}";
                EventManager.Instance.TriggerEvent(EventName.TestEvent);
                TaskDataManager.Instance.UpdateTaskData(4);
                Debug.LogError($"MainPanel UpdateTaskLog2 {e.Message} ");
            }
        }
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
    }

    public void SendMessage(string message)
    {
        Debug.Log($"SendMessage  {message}");
    }
}