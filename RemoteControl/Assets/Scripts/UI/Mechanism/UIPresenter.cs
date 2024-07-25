using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPresenter<T> : IUIPresenter where T : IUIView
{
    public T view;

    public virtual void SetPanelData(UIArgs uiArgs)
    {
        
    }

    public virtual void ShowView(UIArgs uiArgs=null)
    {
        view.RootObj.SetActive(true);
    }
    public virtual void HideView()
    {
        view.RootObj.SetActive(false);
    }

    public virtual void Dispose()
    {
        
    }
}