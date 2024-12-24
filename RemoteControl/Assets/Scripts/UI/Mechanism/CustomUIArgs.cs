using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUIArgs:UIArgs
{
    
}

public  class ConfirmTaskPanelArgs : UIArgs
{
    private Action _cancleAction;
    private Action _confirmAction;
    private string _title;
    private  TaskCommand _taskCommand;
    public   TaskCommand TaskCommand{
        get => _taskCommand;
        set => _taskCommand = value;
    }
    public string TitleName{
        get => _title;
        set => _title = value;
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
    public  ConfirmTaskPanelArgs(TaskCommand taskCommand,string title, Action cancleAction=null, Action confirmAction=null)
    {
        TitleName = title;
        CancleAction = cancleAction;
        ConfirmAction = confirmAction;
        TaskCommand = taskCommand;
    }
}
public class ConfirmPanelArgs : UIArgs
{
    private string _describe;
    private string _title;
    private Action _cancleAction;
    private Action _confirmAction;
    private int _duration;
    private int _duration2;
    private UIID _uiID;
    public string Describe{
        get => _describe;
        set => _describe = value;
    }
    public string TitleName{
        get => _title;
        set => _title = value;
    }
    public int Duration{
        get => _duration;
        set => _duration = value;
    }
    public int Duration2{
        get => _duration2;
        set => _duration2 = value;
    }
    public UIID UIID{
        get => _uiID;
        set => _uiID = value;
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
    public ConfirmPanelArgs(string des,string title, Action cancleAction=null, Action confirmAction=null,int time=0,int time2=0,UIID uiID=null)
    {
        Describe = des;
        TitleName = title;
        CancleAction = cancleAction;
        ConfirmAction = confirmAction;
        Duration = time;
        Duration2 = time2;
        if (uiID==null)
        {
            UIID=UIID.ConfirmPanel;
        }
        else
        {
            UIID=uiID;
        }
     
    }
}
