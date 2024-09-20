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
        Debug.Log("MainPanelCtr ");
        Addlistener();
        view.UpdateData(null,null);
        view.UpdatePcData(null, null);
        UpdateTaskDes1(null, null);
        UpdateTaskDes2(null, null);
    }

    public void UpdateTaskDes1(object o, EventArgs eventArgs)
    {
        // if (GameDataManager.Instance.BucketWheelStackerReclaimerQueue.Count>0)
        // {
        //     
        // }
        view._bucketWheelTask1.UpdateDes(GameDataManager.Instance.BucketWheelStackerReclaimerQueue);
    }
    
    public void UpdateTaskDes2(object o, EventArgs eventArgs)
    {
        // if (GameDataManager.Instance.BucketWheelQueue.Count>0)
        // {
        //     view._bucketWheelTask2.UpdateDes(GameDataManager.Instance.BucketWheelQueue);
        // }
        view._bucketWheelTask2.UpdateDes(GameDataManager.Instance.BucketWheelQueue);
    }
    public override void Dispose()
    {
        EventManager.Instance.RemoveListener(EventName.UpdateRcData, view.UpdateData);
        EventManager.Instance.RemoveListener(EventName.UpdatePcData, view.UpdatePcData);
        EventManager.Instance.RemoveListener(EventName.RefreshTaskDes1,UpdateTaskDes1);
        EventManager.Instance.RemoveListener(EventName.RefreshTaskDes2,UpdateTaskDes2);
    }
   
    public void Addlistener()
    {
        EventManager.Instance.AddListener(EventName.UpdateRcData, view.UpdateData);
        EventManager.Instance.AddListener(EventName.UpdatePcData, view.UpdatePcData);
        EventManager.Instance.AddListener(EventName.RefreshTaskDes1,UpdateTaskDes1);
        EventManager.Instance.AddListener(EventName.RefreshTaskDes2,UpdateTaskDes2);
    }

    public void SendMessage(string message)
    {
        Debug.Log($"SendMessage  {message}");
    }
}