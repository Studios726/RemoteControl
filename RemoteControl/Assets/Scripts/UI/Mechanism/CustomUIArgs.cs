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
    private int _duration;
    private int _duration2;
    public string Describe{
        get => _describe;
        set => _describe = value;
    }
    public int Duration{
        get => _duration;
        set => _duration = value;
    }
    public int Duration2{
        get => _duration2;
        set => _duration2 = value;
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
    public ConfirmPanelArgs(string des, Action cancleAction=null, Action confirmAction=null,int time=0,int time2=0)
    {
        Describe = des;
        CancleAction = cancleAction;
        ConfirmAction = confirmAction;
        Duration = time;
        Duration2 = time2;
    }
}
