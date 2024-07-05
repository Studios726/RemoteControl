using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanelView :UIView<MainPanelCtr>
{
   public override void InitUIElements()
   {
      Debug.LogError("打开mainpanel");
      RootObj.transform.Find("open").GetComponent<Button>().onClick.AddListener((() =>
      {
         UIManager.Instance.OpenUI(UIID.LoginPanel);
         Debug.LogError("openUI");
      }));
      RootObj.transform.Find("close").GetComponent<Button>().onClick.AddListener((() =>
      {
         Debug.LogError("close");
      }));
   }
}
