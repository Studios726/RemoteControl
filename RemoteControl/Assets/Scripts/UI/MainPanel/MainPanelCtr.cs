using System;
using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using UnityEngine;

public class MainPanelCtr : UIPresenter<MainPanelView>
{
    public override void ShowView(UIArgs uiArgs = null)
    {
        base.ShowView(uiArgs);
        Timer.Register(0.1f, () => GameDataManager.Instance.SetMachineActive(true));

    }

    public override void HideView()
    {
        base.HideView();
        GameDataManager.Instance.SetMachineActive(false);
    }

    public override void SetPanelData(UIArgs uiArgs)
    {
        Addlistener();
        view.UpdateData(null,null);
        view.UpdatePcData(null, null);
        UpdateWarningDes1(null, null);
        UpdateWarningDes2(null, null);
    }

    public void UpdateWarningDes1(object o, EventArgs eventArgs)
    {
        List<WarningCellData> datas = new List<WarningCellData>();
        if (GameDataManager.Instance.WarningCellDataDict.Count>0)
        {
            foreach (var keyCellData in GameDataManager.Instance.WarningCellDataDict)
            {
                if (keyCellData.Value.Machine==Machine.BucketWheelStackerReclaimer)
                {
                    datas.Add(keyCellData.Value);
                }
            }
        }
        
        int n = datas.Count;

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
        
        view._bucketWheelTask1.UpdateDes(datas);
      
    }
    
    public void UpdateWarningDes2(object o, EventArgs eventArgs)
    {
        List<WarningCellData> datas = new List<WarningCellData>();
        if (GameDataManager.Instance.WarningCellDataDict.Count>0)
        {
            foreach (var keyCellData in GameDataManager.Instance.WarningCellDataDict)
            {
                if (keyCellData.Value.Machine==Machine.BucketWheel)
                {
                    datas.Add(keyCellData.Value);
                }
            }
        }
        
        int n = datas.Count;

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
        view._bucketWheelTask2.UpdateDes(datas);
    }

    public void UpdateTaskLog1(object o, EventArgs eventArgs)
    {
        TaskLogArgs args = (TaskLogArgs)eventArgs;
        List<TaskLogCellData> datas = new List<TaskLogCellData>();
        datas.Add(new TaskLogCellData("",args.des,DateTime.Now,Machine.BucketWheelStackerReclaimer,args.pos));
        view._bucketWheelTask1.UpdateTaskLog(datas);
       
    }
    public void UpdateTaskLog2(object o, EventArgs eventArgs)
    {
        TaskLogArgs args = (TaskLogArgs)eventArgs;
        List<TaskLogCellData> datas = new List<TaskLogCellData>();
        datas.Add(new TaskLogCellData("",args.des,DateTime.Now,Machine.BucketWheel,args.pos));
        view._bucketWheelTask2.UpdateTaskLog(datas);
    }
    public override void Dispose()
    {
        EventManager.Instance.RemoveListener(EventName.UpdateRcData, view.UpdateData);
        EventManager.Instance.RemoveListener(EventName.UpdatePcData, view.UpdatePcData);
        EventManager.Instance.RemoveListener(EventName.RefreshWarningDes1,UpdateWarningDes1);
        EventManager.Instance.RemoveListener(EventName.RefreshWarningDes2,UpdateWarningDes2);
        EventManager.Instance.RemoveListener(EventName.RefreshTaskDes1,UpdateTaskLog1);
        EventManager.Instance.RemoveListener(EventName.RefreshTaskDes2,UpdateTaskLog2);
    }
   
    public void Addlistener()
    {
        EventManager.Instance.AddListener(EventName.UpdateRcData, view.UpdateData);
        EventManager.Instance.AddListener(EventName.UpdatePcData, view.UpdatePcData);
        EventManager.Instance.AddListener(EventName.RefreshWarningDes1,UpdateWarningDes1);
        EventManager.Instance.AddListener(EventName.RefreshWarningDes2,UpdateWarningDes2);
        EventManager.Instance.AddListener(EventName.RefreshTaskDes1,UpdateTaskLog1);
        EventManager.Instance.AddListener(EventName.RefreshTaskDes2,UpdateTaskLog2);
        
    }

    public void SendMessage(string message)
    {
        Debug.Log($"SendMessage  {message}");
    }
}