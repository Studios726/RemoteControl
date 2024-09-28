using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetParameterItem : MonoBehaviour
{
    public  Text text;
    public InputField inputField;
    public ButtonCell cell;
    public string commandName;
    public Action<string> Action;

    private void Start()
    {
        cell.AddListener((() =>
        {
            GameDataManager.Instance.SendServerCommandByName(commandName,0, float.Parse(inputField.text));
        }));
    }

    public void SetText(float str)
    {
        text.text = str.ToString("F2");
    }

    public void SetCommandName(string command)
    {
        commandName = command;
    }
}
