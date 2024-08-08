using RemoteControl.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanelCtr :UIPresenter<LoginPanelView>
{
    public void Login(string account, string password)
    {
        if (DataManager.Instance.CheckLoginInfo(account, password))
        {
            EventManager.Instance.TriggerEvent(EventName.LoginSuccess, null);
            view.SetAccountAndPassword();
            UIManager.Instance.OpenUI(UIID.MainPanel);
            UIManager.Instance.OpenUI(UIID.TopPanel);
        }
        else
        {
            view.ShowError("");
        }
    }
}
