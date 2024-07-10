using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIView<T> : IUIView where T : IUIPresenter
{
    public T _ctr;

    protected GameObject _rootObj;
    public GameObject RootObj { get => _rootObj; }

    public abstract void InitUIElements(UIArgs uiArgs=null);
}