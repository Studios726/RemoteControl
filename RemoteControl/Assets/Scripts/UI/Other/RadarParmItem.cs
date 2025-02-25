using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarParmItem : MonoBehaviour
{
    public Text curValue;
    public InputField setValueInputField;
    public Machine machine;
    private string commondName;
    public Action onEndEdit;
    private int maxNum;
    private string des;
    private void Start()
    {
        setValueInputField.onEndEdit.AddListener(((string value) =>
        {
            float num = 0;
            if (float.TryParse(value,out num))
            {
                if (maxNum!=-1)
                {
                    if (num>maxNum)
                    {
                        num=maxNum;
                    }
                }
                setValueInputField.text=num.ToString();
                DataManager.Instance.InsertHistoryLogMc($"{des}修改", GameDataManager.Instance.GetUserName(), machine);
                TaskDataManager.Instance.UpdateCommonTaskParameters(this.commondName,num.ToString(),machine);
                onEndEdit?.Invoke();
            }
           
        }));
    }

    public void InitName(string commondName,Machine machine,int max=-1,string des="")
    {
        maxNum = max;
        this.commondName = commondName;
        this.machine = machine;
        this.des = des;
    }
    public void SetTextValue(string cur)
    {
        curValue.text = cur;
   
    }

    public void SetTaskValue(float value)
    {
        setValueInputField.text = value.ToString();
    }
}
