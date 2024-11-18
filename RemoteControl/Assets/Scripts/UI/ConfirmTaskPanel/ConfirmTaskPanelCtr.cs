using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmTaskPanelCtr :UIPresenter<ConfirmTaskPanelView>
{
    public override void ShowView(UIArgs uiArgs = null)
    {
        base.ShowView(uiArgs);
        view.UpdateUI(uiArgs);
    }
}
