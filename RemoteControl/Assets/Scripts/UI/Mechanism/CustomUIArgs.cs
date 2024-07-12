using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUIArgs:UIArgs
{
    
}

public class ConfirmPanelArgs : UIArgs
{
    private string _describe;
    private Action _cancleAction;
    private Action _confirmAction;
    public string Describe{
        get => _describe;
        set => _describe = value;
    }

    public Action CancleAction
    {
        get  =>_cancleAction;
        set => _cancleAction = value;
    }
    public Action ConfirmAction
    {
        get  =>_confirmAction;
        set => _confirmAction = value;
    }
    public ConfirmPanelArgs(string des, Action cancleAction=null, Action confirmAction=null)
    {
        Describe = des;
        CancleAction = cancleAction;
        ConfirmAction = confirmAction;
    }
}
