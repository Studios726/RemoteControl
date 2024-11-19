using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using UnityEngine;
using UnityEngine.UIElements;

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
        view.UpdateCurrentAccount(null,null);
    }

    public override void SetPanelData(UIArgs uiArgs)
    {
        Addlistener();
    }
    
    public override void Dispose()
    {
        EventManager.Instance.RemoveListener(EventName.UpdateRcData, view.UpdateData);
        EventManager.Instance.RemoveListener(EventName.UpdateAccountData,view.UpdateCurrentAccount);
    }
    public void Addlistener()
    {
        EventManager.Instance.AddListener(EventName.UpdateRcData, view.UpdateData);
        EventManager.Instance.AddListener(EventName.UpdateAccountData,view.UpdateCurrentAccount);
    }
}
