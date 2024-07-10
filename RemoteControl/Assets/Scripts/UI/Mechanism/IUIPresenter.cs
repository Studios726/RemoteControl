using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIPresenter
{
    void ShowView(UIArgs uiArgs);
    void HideView();
}