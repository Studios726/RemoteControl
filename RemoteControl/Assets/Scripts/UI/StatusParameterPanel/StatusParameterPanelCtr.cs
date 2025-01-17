using System;
using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using UnityEngine;

public class StatusParameterPanelCtr : UIPresenter<StatusParameterPanelView>
{
   //获取悬臂雷达距离
   public Timer timer;
   public override void SetPanelData(UIArgs uiArgs)
   {
      if (timer==null)
      {
         timer=Timer.Register(1, true, true, (() =>
         {
            GameDataManager.Instance.UpdateSCAData(32); 
         }));
      }

      if (timer.IsPaused)
      {
         timer?.Resume();
      }
      Addlistener();
   }
   public override void Dispose()
   {
      timer?.Pause();
      EventManager.Instance.RemoveListener(EventName.UpdateRcData, UpdateData);
   }
   
   public void Addlistener()
   {
      EventManager.Instance.AddListener(EventName.UpdateRcData, UpdateData);
   }
   public void UpdateData(object o, EventArgs eventArgs)
   {
        view.UpdateData(GameDataManager.Instance.SystemVariables);
   }
}
