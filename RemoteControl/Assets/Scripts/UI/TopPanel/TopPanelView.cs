using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class TopPanelView : UIView<TopPanelCtr>
{
    private Button _controlBtn;
    private Button _superviseBtn;
    private Button _alarmBtn;
    private Button _userBtn;
    private TMP_Text _userTxt;
    private Transform _userPnl;
    private Button _settingBtn;
    private Button _logoutBtn;
    private GameObject StatePanel;
    private TMP_Text _title;
    public GameObject WorkScreenPanel;

    public override void InitUIElements(UIArgs uiArgs)
    {
        _controlBtn = RootObj.transform.FindComponent<Button>("BG/Control");
        _superviseBtn = RootObj.transform.FindComponent<Button>("BG/Supervise");
        _alarmBtn = RootObj.transform.FindComponent<Button>("BG/Alarm");
        _userBtn = RootObj.transform.FindComponent<Button>("BG/User");
        _userTxt = RootObj.transform.FindComponent<TMP_Text>("BG/User/Text (TMP)");
        _userPnl = RootObj.transform.Find("BG/UserPnl");
        _settingBtn = _userPnl.FindComponent<Button>("SettingBtn");
        _logoutBtn = _userPnl.FindComponent<Button>("LogoutBtn");
        _title = RootObj.transform.FindComponent<TMP_Text>("BG/Title/Text (TMP)");
        _title.text = ConstStr.PROJECT_NAME;
        // StatePanel = GameObject.Find("StatePanel");
        // StatePanel.SetActive(false);
        // WorkScreenPanel = GameObject.Find("WorkScreenPanel");

        _userTxt.text ="test";

        _controlBtn.onClick.AddListener(() => //��Զ�̲�������
        {
            Debugger.Log("��Զ�̲�������");
        });
        _superviseBtn.onClick.AddListener(() =>
        {
            // //presenter.CurrentActive = UIID.RuntimeMonitor;
            // //SceneManager.LoadScene("Monitor");
            // StatePanel.SetActive(true);
            // UISystem.Instance.HideCanvasParent();
            // Debugger.Log("��Զ�̲�������");
        });
        _alarmBtn.onClick.AddListener(() => //�򿪱����������
        {
            // UISystem.Instance.SetActive(UIID.HistoryWarning, true);
            // UISystem.Instance.SetActive(UIID.Settings, false);
            // WorkScreenPanel.gameObject.SetActive(false);
        });
        _userBtn.onClick.AddListener(() => { _userPnl.gameObject.SetActive(!_userPnl.gameObject.activeSelf); });
        _settingBtn.onClick.AddListener(() =>
        {
            _userPnl.gameObject.SetActive(false);
            UIManager.Instance.OpenUI(UIID.SettingPanel);
            Debugger.LogError("��settingҳ��");
            // UISystem.Instance.SetActive(UIID.Settings, true);
            // UISystem.Instance.SetActive(UIID.HistoryWarning, false);
        });
        _logoutBtn.onClick.AddListener(() =>
        {
            _userPnl.gameObject.SetActive(false);
            _ctr.Logout();
        });

        _userPnl.gameObject.SetActive(false);
    }

    public void UpdateCurrentAccount(AccountInfo account)
    {
        _userTxt.text = account.name;
    }
}