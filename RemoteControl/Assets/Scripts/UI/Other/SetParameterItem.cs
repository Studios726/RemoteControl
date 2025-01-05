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
    public string useCommand;
    public Action<string> Action;
    public Machine machine;
    public string des;

    private void Start()
    {
        cell.AddListener((() =>
        {    
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs(des+"启用?",GameDataManager.Instance.GetMachineName(machine), null, () =>
                {
                    DataManager.Instance.InsertHistoryLogMc(des+"启用", GameDataManager.Instance.GetUserName(), machine);
                    Debug.Log($"启用 {des} {machine} {useCommand}");
                    GameDataManager.Instance.SendServerCommandByName(useCommand,0);
                }));
          
        }));
        resetBtn.AddListener((() =>
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs(des+"切除?",GameDataManager.Instance.GetMachineName(machine), null, () =>
                {
                    DataManager.Instance.InsertHistoryLogMc(des+"切除", GameDataManager.Instance.GetUserName(), machine);
                    Debug.Log($"切除 {des} {machine} {useCommand}");
                    GameDataManager.Instance.SendServerCommandByName(useCommand,1);
                }));

         
        }));
        inputField.onEndEdit.AddListener((arg0 =>
        {
            Debug.Log($"设定修改 {des} {machine} {commandName}");
            DataManager.Instance.InsertHistoryLogMc(des+"设定修改", GameDataManager.Instance.GetUserName(), machine);
            GameDataManager.Instance.SendServerCommandByName(commandName,0, float.Parse(inputField.text));
        }));
        inputField.text = "0";
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
    public void SetCommandName(Machine machine,string command,string useCommand="",string des="")
    {
        this.machine = machine;
        this.des = des;
        commandName = command;
        this.useCommand = useCommand;
    }
}
