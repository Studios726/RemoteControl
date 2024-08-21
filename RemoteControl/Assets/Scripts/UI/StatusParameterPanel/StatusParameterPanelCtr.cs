using System;
using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using UnityEngine;

public class StatusParameterPanelCtr : UIPresenter<StatusParameterPanelView>
{
   public override void SetPanelData(UIArgs uiArgs)
   {
      Addlistener();
   }
   public override void Dispose()
   {
      Debug.Log("MainPanelCtr ");
      EventManager.Instance.RemoveListener(EventName.UpdateRcData, UpdateData);
   }
   
   public void Addlistener()
   {
      EventManager.Instance.AddListener(EventName.UpdateRcData, UpdateData);
   }
   public void UpdateData(object o, EventArgs eventArgs)
   {
        Debug.LogError("状态参数面板更新数据");
        view.UpdateData(GameDataManager.Instance.SystemVariables);
   }
}
