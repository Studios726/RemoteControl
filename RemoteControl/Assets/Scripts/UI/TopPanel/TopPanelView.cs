using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class TopPanelView : UIView<TopPanelCtr>
{
    private Button _controlBtn;
    private Button _superviseBtn;
    private Button _alarmBtn;
    public TMP_Text _controlText;
    public TMP_Text _superviseText;
    public TMP_Text _alarmText;
    private Button _userBtn;
    private TMP_Text _userTxt;
    private Transform _userPnl;
    private Button _settingBtn;
    private Button _logoutBtn;
    private Button _logoutBtn2;
    private Button _closeUserBtn;
    private GameObject StatePanel;
    private TMP_Text _title;
    public GameObject WorkScreenPanel;
    private Color selectColor = new Color(0, 0.98f, 1,1);
    private Color normalColor = Color.white;
    public TMP_Text _lastText;

    public override void InitUIElements(UIArgs uiArgs)
    {
        _controlBtn = RootObj.transform.FindComponent<Button>("BG/Control");
        _controlText=RootObj.transform.FindComponent<TMP_Text>("BG/Control/Text");
        _superviseBtn = RootObj.transform.FindComponent<Button>("BG/Supervise");
        _superviseText = RootObj.transform.FindComponent<TMP_Text>("BG/Supervise/Text");
        _alarmBtn = RootObj.transform.FindComponent<Button>("BG/Alarm");
        _alarmText = RootObj.transform.FindComponent<TMP_Text>("BG/Alarm/Text");
        _userBtn = RootObj.transform.FindComponent<Button>("BG/User");
        _closeUserBtn = RootObj.transform.FindComponent<Button>("BG/close");
        _userTxt = RootObj.transform.FindComponent<TMP_Text>("BG/User/Text (TMP)");
        _userPnl = RootObj.transform.Find("BG/UserPnl");
        _settingBtn = _userPnl.FindComponent<Button>("SettingBtn");
        _logoutBtn = _userPnl.FindComponent<Button>("LogoutBtn");
        _logoutBtn2 = _userPnl.FindComponent<Button>("LogoutBtn_2");
        _title = RootObj.transform.FindComponent<TMP_Text>("BG/Title/Text (TMP)");
        _title.text = ConstStr.PROJECT_NAME;
      
        _lastText = _controlText;
        _userTxt.text =GameDataManager.Instance.GetUserName();

        _controlBtn.onClick.AddListener(() => //打开远程操作界面
        {
            UIManager.Instance.OpenUI(UIID.MainPanel);
            UIManager.Instance.CloseUI(UIID.HistoryPanel);
            UIManager.Instance.CloseUI(UIID.StatusParaeterPanel);
            UIManager.Instance.CloseUI(UIID.SettingPanel);
            SetSelectState(_controlText);
        });
        _superviseBtn.onClick.AddListener(() =>
        {
          
            UIManager.Instance.OpenUI(UIID.StatusParaeterPanel);
            UIManager.Instance.CloseUI(UIID.SettingPanel);
            UIManager.Instance.CloseUI(UIID.MainPanel);
            UIManager.Instance.CloseUI(UIID.HistoryPanel);
            SetSelectState(_superviseText);
        });
        _alarmBtn.onClick.AddListener(() => //打开报警处理界面
        {
            UIManager.Instance.OpenUI(UIID.HistoryPanel);
            UIManager.Instance.CloseUI(UIID.SettingPanel);
            UIManager.Instance.CloseUI(UIID.MainPanel);
            UIManager.Instance.CloseUI(UIID.StatusParaeterPanel);
            SetSelectState(_alarmText);

        });
        _userBtn.onClick.AddListener(() =>
        {
            _userPnl.gameObject.SetActive(!_userPnl.gameObject.activeSelf);
            _closeUserBtn.gameObject.SetActive(!_closeUserBtn.gameObject.activeSelf);
        });
        _closeUserBtn.onClick.AddListener((() =>
        {
            _closeUserBtn.gameObject.SetActive(false);
            _userPnl.gameObject.SetActive(false);
        }));
        _settingBtn.onClick.AddListener(() =>
        {
            _userPnl.gameObject.SetActive(false);
            _closeUserBtn.gameObject.SetActive(false);
            UIManager.Instance.OpenUI(UIID.SettingPanel);
            UIManager.Instance.CloseUI(UIID.HistoryPanel);
            UIManager.Instance.CloseUI(UIID.MainPanel);
            UIManager.Instance.CloseUI(UIID.StatusParaeterPanel);
            SetSelectState(_userTxt);
          
        });
        _logoutBtn.onClick.AddListener(() =>
        {
            _userPnl.gameObject.SetActive(false);
            _closeUserBtn.gameObject.SetActive(false);
            _ctr.Logout();
        });
        _logoutBtn2.onClick.AddListener(() =>
        {
            EventManager.Instance.TriggerEvent(EventName.ExitGame,null,null);
            // _userPnl.gameObject.SetActive(false);
            
        });
        _userPnl.gameObject.SetActive(false);
        _closeUserBtn.gameObject.SetActive(false);
    }

    public void SetSelectState(TMP_Text tmpText )
    {
        if (_lastText!=null)
        {
            _lastText.color = normalColor;
        }

        _lastText = tmpText;
        _lastText.color = selectColor;
    }
    public void UpdateCurrentAccount(AccountInfo account)
    {
        _userTxt.text = account.name;
    }
}