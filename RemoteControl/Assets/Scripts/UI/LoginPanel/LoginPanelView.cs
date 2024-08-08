using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Utility;

public class LoginPanelView : UIView<LoginPanelCtr>
{
    private InputField _accountInput;
    private InputField _passwordInput;
    private UnityEngine.UI.Button _loginBtn;
    private UnityEngine.UI.Toggle _toggle;
    public GameObject _error;
    public Timer timer;
    public override void InitUIElements(UIArgs uiArgs)
    {
        _accountInput = RootObj.transform.Find("Bg/accountInput").GetComponent<InputField>();
        _passwordInput = RootObj.transform.Find("Bg/passwordInput").GetComponent<InputField>();
        _loginBtn = RootObj.transform.Find("Bg/loginBtn").GetComponent<UnityEngine.UI.Button>();
        _toggle = RootObj.transform.Find("Bg/Toggle").GetComponent<UnityEngine.UI.Toggle>();
        _error = RootObj.transform.Find("error").gameObject;
        _loginBtn.onClick.AddListener(Login);
        //_accountInput.ActivateInputField();
        Debug.Log($"Account {PlayerPrefs.GetString("Account")}");
        Debug.Log($"Password {PlayerPrefs.GetString("Password")}");
        _accountInput.text = PlayerPrefs.GetString("Account");
        _passwordInput.text = PlayerPrefs.GetString("Password");

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
        _error.SetActive(true);
    }
    public void SetAccountAndPassword()
    {
        if (_toggle != null && _toggle.isOn)
        {
            PlayerPrefs.SetString("Account", _accountInput.text);
            PlayerPrefs.SetString("Password", _passwordInput.text);
        }
        else
        {
            PlayerPrefs.SetString("Account", "");
            PlayerPrefs.SetString("Password", "");
        }
    }
    private void Login()
    {
       
        _ctr.Login(_accountInput.text, _passwordInput.text);
    }
}
