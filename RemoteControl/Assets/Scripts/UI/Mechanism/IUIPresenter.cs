using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIPresenter
{
    void SetPanelData(UIArgs uiArgs);
    void ShowView(UIArgs uiArgs);
    void HideView();
    void Dispose();
}