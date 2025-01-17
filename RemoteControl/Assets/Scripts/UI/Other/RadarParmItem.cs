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

    private void Start()
    {
        setValueInputField.onEndEdit.AddListener(((string value) =>
        {
            float num = 0;
            if (float.TryParse(value,out num))
            {
                TaskDataManager.Instance.UpdateCommonTaskParameters(this.commondName,num.ToString(),machine);
            }
           
        }));
    }

    public void InitName(string commondName,Machine machine)
    {
        this.commondName = commondName;
        this.machine = machine;
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
