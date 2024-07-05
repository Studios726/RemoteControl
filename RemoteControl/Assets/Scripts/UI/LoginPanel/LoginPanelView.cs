using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelView : UIView<LoginPanelCtr>
{
   private InputField _accountInput;
   private InputField _passwordInput;
   private Button _loginBtn;
   public override void InitUIElements()
   {
      _accountInput = RootObj.transform.Find("Bg/accountInput").GetComponent<InputField>();
      _passwordInput = RootObj.transform.Find("Bg/passwordInput").GetComponent<InputField>();
      _loginBtn = RootObj.transform.Find("Bg/loginBtn").GetComponent<Button>();
      _loginBtn.onClick.AddListener(Login);
   }
   private void Login()
   {
      presenter.Login(_accountInput.text, _passwordInput.text);
   }
}
