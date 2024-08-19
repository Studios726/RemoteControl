using System.Collections;
using System.Collections.Generic;
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
}
