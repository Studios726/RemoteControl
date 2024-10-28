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
    public ButtonCell resetBtn;
    public string commandName;
    public Action<string> Action;

    private void Start()
    {
        cell.AddListener((() =>
        {
            //
            //
            // GameDataManager.Instance.SendServerCommandByName(commandName,0, float.Parse(inputField.text));
        }));
        inputField.onEndEdit.AddListener((arg0 =>
        {
            Debug.Log($"启用 {commandName} {inputField.text}");
            GameDataManager.Instance.SendServerCommandByName(commandName,0, float.Parse(inputField.text));
        }));
    }

    public void SetButtonColor(bool isRun)
    {
        cell.SetSystemState(isRun,true);
        resetBtn.SetSystemState(isRun==false,true);
    }
    public void SetText(float str)
    {
        text.text = str.ToString("F2");
        // inputField.text
    }
    public void SetInputField(float str)
    {
        inputField.text =str.ToString("F2");
    }
    // public k
    public void SetCommandName(string command)
    {
        commandName = command;
    }
}
