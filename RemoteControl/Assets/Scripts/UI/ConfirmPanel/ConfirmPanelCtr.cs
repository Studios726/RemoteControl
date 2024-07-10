using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPanelCtr :UIPresenter<ConfirmPanelView>
{
  public override void ShowView(UIArgs uiArgs = null)
  {
    base.ShowView(uiArgs);
    view.UpdateUI(uiArgs);
  }
}
