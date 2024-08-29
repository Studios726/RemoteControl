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
