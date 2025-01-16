using System;
using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImportantAlarmTips : MonoBehaviour
{
    public Text Text;
    public Timer Timer;
    private int index;//当前索引
    private int count;//闪烁次数
    private Machine Machine;
    private List<WarningCellData> ImportantAlarmMessageList = new List<WarningCellData>();
    private void Awake()
    {
        AddTimer();
        EventManager.Instance.AddListener(EventName.RefreshImportantAlarm,RefreshImportantAlarm);
    }
    public void SetText(string text)
    {
        Text.text = text;
    }
    public void Init(List<WarningCellData> alarmList,Machine _machine)
    {
        Machine = _machine;
        index = 0;
        ImportantAlarmMessageList = alarmList;
        if (ImportantAlarmMessageList.Count>0)
        {
            SetText(ImportantAlarmMessageList[index].Des);
        }
        else
        {
            SetText("");
        }
    }

    public void RestImportantAlarmListIndex()
    {
        index = 0;
        if (ImportantAlarmMessageList.Count>0)
        {
            SetText(ImportantAlarmMessageList[index].Des);
        }
        else
        {
            SetText("");
        }
    }
    public void RefreshShow()
    {
        count = 0;
        
    }
    public void AddTimer()
    {
        if (Timer==null)
        {
            Timer=Timer.Register(1f, true, true, (() =>
            {
                count++;
                Text.color = Text.color != Color.red ? Color.red : Color.yellow;
                if (count>3)
                {
                    index++;
                    if (index>=ImportantAlarmMessageList.Count)
                    {
                        index = 0;
                    }
                    if (ImportantAlarmMessageList.Count>0)
                    {
                        SetText(ImportantAlarmMessageList[index].Des);
                    }
                    else
                    {
                        SetText("");
                    }
                  
                    count = 0;
                }
            }));
        }
    }

    public void RefreshImportantAlarm(object o, EventArgs eventArgs)
    {
        if (eventArgs is UpdateImportantAlarmArgs args)
        {
            if (Machine==args.WarningCellData.Machine)
            {
                if (args.IsAdd)
                {
                    if (Text.text=="")
                    {
                        count = 0;
                        RestImportantAlarmListIndex();
                    }
                }
                else
                {
                    if (Text.text ==args.WarningCellData.Des)
                    {
                        if (index<ImportantAlarmMessageList.Count)
                        {
                            count = 0;
                            SetText(ImportantAlarmMessageList[index].Des);
                        }
                        else if (ImportantAlarmMessageList.Count>0)
                        {
                            count = 0;
                            index = 0;
                            SetText(ImportantAlarmMessageList[0].Des);
                        }else
                        {
                            SetText("");
                        }
                    }
                }
            }
            
        }
    }
}
