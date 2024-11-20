using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskLogCellData
{
    public string Key;
    public string Des;
    public string pos;
    public string TriggerTime;
    public DateTime TriggerDateTime;
    public double Timestamp;
    public Machine Machine;

    public TaskLogCellData(string key,string des,DateTime dateTime, Machine machine,string pos
       )
    {
        this.Key = key;
        this.Des = des;
        this.Machine = machine;
        this.TriggerTime = dateTime.ToString("HH:mm:ss");
        this.TriggerDateTime = dateTime;
        this.pos = pos;
        this.Timestamp =(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)).TotalSeconds;
    }
}
public class TaskLogCell : MonoBehaviour
{
    public Text des;
    public Text pos;
    public Text time;
    public void UpdateDisplay(TaskLogCellData taskLogCellData)
    {
        des.text = taskLogCellData.Des;
        pos.text = taskLogCellData.pos;
        time.text = taskLogCellData.TriggerTime;
    }
}
