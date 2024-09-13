
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class LoginPanelView : UIView<LoginPanelCtr>
{
    private TMP_InputField _accountInput;
    private TMP_InputField _passwordInput;
    private UnityEngine.UI.Button _loginBtn;
    private UnityEngine.UI.Toggle _toggle;
    public GameObject _error;
    public Timer timer;
    public Text _IP;
    public override void InitUIElements(UIArgs uiArgs)
    {
        _accountInput = RootObj.transform.Find("Bg/accountInput").GetComponent<TMP_InputField>();
        _passwordInput = RootObj.transform.Find("Bg/passwordInput").GetComponent<TMP_InputField>();
        _loginBtn = RootObj.transform.Find("Bg/loginBtn").GetComponent<UnityEngine.UI.Button>();
        _toggle = RootObj.transform.Find("Bg/Toggle").GetComponent<UnityEngine.UI.Toggle>();
        _IP = RootObj.transform.FindComponent<Text>("IP");
        _error = RootObj.transform.Find("error").gameObject;
        _loginBtn.onClick.AddListener(Login);
        _passwordInput.onSubmit.AddListener(OnSubmit);
        _accountInput.text = PlayerPrefs.GetString("Account");
        _passwordInput.text = PlayerPrefs.GetString("Password");
        _toggle.isOn = PlayerPrefs.GetInt("Remember",0) == 1;
        _accountInput.caretPosition=_accountInput.text.Length;
        _accountInput.onFocusSelectAll = false;
        _passwordInput.onFocusSelectAll = false;
        _accountInput.ActivateInputField();
        // _IP.text ="数据库IP:"+ GameDataManager.Instance.IpConfig.DataIP + "\n 三维扫描IP:" + GameDataManager.Instance.IpConfig.YuanIP + "\n 任务IP " +
        //            GameDataManager.Instance.IpConfig.TaskIP +"\n RC IP "+
        //            GameDataManager.Instance.IpConfig.TaoIP;
        _IP.text = "";
       
    }

    public void OnSubmit(string str)
    {
        Login();
    }
    public void ShowError(string error)
    {
        if (timer != null) {
            timer.Cancel();
         
        }
        timer = Timer.Register(1, false, true, () => {
            timer=null;
            _error.SetActive(false);
        });
        _error.transform.FindComponent<Text>("Text").text = error;
        _error.SetActive(true);
    }
    public void SetAccountAndPassword()
    {
        if (_toggle != null && _toggle.isOn)
        {
            PlayerPrefs.SetString("Account", _accountInput.text);
            PlayerPrefs.SetString("Password", _passwordInput.text);
            PlayerPrefs.SetInt("Remember",1);
        }
        else
        {
            PlayerPrefs.SetString("Account", "");
            PlayerPrefs.SetString("Password", "");
            PlayerPrefs.SetInt("Remember",0);
        }
    }

    public void Focus()
    {
        if (_accountInput.isFocused)
        {
            _passwordInput.caretPosition=_passwordInput.text.Length;
            _passwordInput.ActivateInputField();
        }
        else
        {
            _passwordInput.DeactivateInputField();
        }
    }
    private void Login()
    {
        if (_accountInput.text!=""&&_passwordInput.text!="")
        {
            _ctr.Login(_accountInput.text, _passwordInput.text);
        }
        else if (_accountInput.text=="")
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("账号不能为空"));
        }else if (_passwordInput.text=="")
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("密码不能为空"));
        }

        
    }
}
