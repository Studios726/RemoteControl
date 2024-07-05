using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanelCtr :UIPresenter<LoginPanelView>
{
    public void Login(string account, string password)
    {
        Debugger.LogError("账号"+account+"  "+password);
    }
}
