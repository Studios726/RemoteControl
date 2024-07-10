using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanelView :UIView<MainPanelCtr>
{
   string str="你好，我是儿子";
   public override void InitUIElements(UIArgs uiArgs)
   {
      Debug.LogError("打开mainpanel");
      RootObj.transform.Find("open").GetComponent<Button>().onClick.AddListener((() =>
      {
         // UIManager.Instance.OpenUI(UIID.LoginPanel);
         Debug.LogError("openUI");
         UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("你好，我是你爸爸",(() => Debugger.LogError("取消")),
            () => { Debugger.LogError("确认");}));
      }));
      RootObj.transform.Find("close").GetComponent<Button>().onClick.AddListener((() =>
      {
         Debug.LogError("close");
         UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs(str,(() => Debugger.LogError("取消")),
            () => { Debugger.LogError("确认");}));
         str = "你好，我是你爸爸";
      }));
   }
}
