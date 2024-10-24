using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using UnityEngine;

public class TopPanelCtr : UIPresenter<TopPanelView>
{
    public void Logout()
    {
        UIManager.Instance.CloseUI(UIID.TopPanel);
        UIManager.Instance.CloseUI(UIID.SettingPanel);
        UIManager.Instance.CloseUI(UIID.HistoryPanel);
        UIManager.Instance.CloseUI(UIID.MainPanel);
        UIManager.Instance.CloseUI(UIID.StatusParaeterPanel);
        UIManager.Instance.OpenUI(UIID.LoginPanel);
    }

    public override void ShowView(UIArgs uiArgs = null)
    {
        base.ShowView(uiArgs);
        view.SetSelectState(view._controlText);
    }

    public override void SetPanelData(UIArgs uiArgs)
    {
        Addlistener();
    }
    
    public override void Dispose()
    {
        EventManager.Instance.RemoveListener(EventName.UpdateRcData, view.UpdateData);
    }
    public void Addlistener()
    {
        EventManager.Instance.AddListener(EventName.UpdateRcData, view.UpdateData);
    }
}
