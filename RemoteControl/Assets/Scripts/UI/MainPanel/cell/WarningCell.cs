using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningCellData
{
    public  string Key;
    public string Des;
    public bool IsConfirm;
    public string TriggerTime;
    public bool IsSelect;
    public string ConfirmTime;
    public DateTime TriggerDateTime;
    public double Timestamp;
    public Machine Machine;

    public WarningCellData(string key,string des,DateTime dateTime, Machine machine, bool isConfirm = false, bool isSelect = false,
        string confirmTime = "")
    {
        this.Key = key;
        this.Des = des;
        this.IsConfirm = isConfirm;
        this.ConfirmTime = confirmTime;
        this.Machine = machine;
        this.TriggerTime = dateTime.ToString("HH:mm:ss");
        this.IsSelect = isSelect;
        this.TriggerDateTime = dateTime;
        this.Timestamp =(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)).TotalSeconds;
    }
}

public class WarningCell : MonoBehaviour
{
    public Text des;
    public Button confirmBtn;
    public Toggle isConfirmToggle;
    public Text confirmBtnText;
    public Text time;
    public Text triggerTime;
    public WarningCellData data;
    private void Start()
    {
        confirmBtn.onClick.AddListener(() =>
        {
            time.text = DateTime.Now.ToString("HH:mm:ss");
            confirmBtnText.text = "已确认";
        });
        isConfirmToggle.onValueChanged.AddListener((arg0 =>
        {
            GameDataManager.Instance.ChangeWarningDesDict(data.Key,arg0,data.ConfirmTime);
        }));
    }

    public void UpdateDisplay(WarningCellData warningCellData)
    {
        data = warningCellData;
        des.text = warningCellData.Des;
        triggerTime.text = warningCellData.TriggerTime;
        des.color = warningCellData.IsConfirm ? Color.white : Color.red;
        time.text = warningCellData.IsConfirm == true ? warningCellData.ConfirmTime : "";
        isConfirmToggle.isOn = warningCellData.IsSelect;
    }
}