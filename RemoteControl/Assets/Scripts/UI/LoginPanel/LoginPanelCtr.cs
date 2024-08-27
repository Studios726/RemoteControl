using System;
using RemoteControl.Event;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ShenYangRemoteSystem.Subclass;
using UnityEngine;

public class LoginPanelCtr :UIPresenter<LoginPanelView>
{
    public override void ShowView(UIArgs uiArgs = null)
    {
        base.ShowView(uiArgs);
        GameDataManager.Instance.SetMachineActive(false);
        ReadConfig();
    }

    public void Login(string account, string password)
    {
        if (true)//DataManager.Instance.CheckLoginInfo(account, password)
        {
            EventManager.Instance.TriggerEvent(EventName.LoginSuccess, null);
            view.SetAccountAndPassword();
            UIManager.Instance.OpenUI(UIID.MainPanel);
            UIManager.Instance.OpenUI(UIID.TopPanel);
            UIManager.Instance.CloseUI(UIID.LoginPanel);
        }
        else
        {
            view.ShowError("账号或密码输入错误，请重新输入");
        }
    }

    public void KeyCodeTab(object o, EventArgs eventArgs)
    {
        view.Focus();
    }
    public override void SetPanelData(UIArgs uiArgs)
    {
        EventManager.Instance.AddListener(EventName.KeyCodeTab,KeyCodeTab);
    }

    public void ReadConfig()
    {
        if (GameDataManager.Instance.IpConfig!=null)
        {
               return;
        }
        string exeRootPath = Application.dataPath;
        string parentPath = Directory.GetParent(exeRootPath).FullName;
        string filePath =parentPath+ "\\IpConfig.txt";
        if (File.Exists(filePath))
        {
            string content = File.ReadAllText(filePath);
            IpConfig ipConfig = JsonMgr.DeSerialize<IpConfig>(content);
            GameDataManager.Instance.SetIpConfig(ipConfig);
          
        }
        else
        {
            IpConfig config = new IpConfig();
            config.TaoIP = Address.serviceTaoIP;
            config.YuanIP= Address.serviceYuanIP;
            config.TaskIP= Address.serviceTaskIP;
            config.DataIP= Address.serviceIP;
            GameDataManager.Instance.SetIpConfig(config);
           
        }
        Debug.LogError(GameDataManager.Instance.IpConfig.DataIP);
    }
    public override void Dispose()
    {
        EventManager.Instance.RemoveListener(EventName.KeyCodeTab, KeyCodeTab);
    }
}
