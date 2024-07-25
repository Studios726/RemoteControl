using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using UnityEngine;

public class MainPanelCtr : UIPresenter<MainPanelView>
{
     public override void SetPanelData(UIArgs uiArgs)
     {
          Debug.Log("MainPanelCtr ");
          Addlistener();
     }
     public override void Dispose()
     {
          Debug.Log("MainPanelCtr ");
          EventManager.Instance.RemoveListener(EventName.MainPanelUpdateArgs,view.UpdateDdata);
     }
     public void Addlistener()
     {
          EventManager.Instance.AddListener(EventName.MainPanelUpdateArgs,view.UpdateDdata);
     }

     public void SendMessage(string message)
     {
          Debug.Log($"SendMessage  {message}");
     }
}
