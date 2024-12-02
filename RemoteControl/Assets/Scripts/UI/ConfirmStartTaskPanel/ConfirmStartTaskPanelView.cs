using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class ConfirmStartTaskPanelView : UIView<ConfirmStartTaskPanelCtr>
{
    private Button _confirmBtn;
    private Button _cancelBtn;
    private Toggle _restoreTask;//归零
    private Toggle _startNewTask;//关闭设备
    public override void InitUIElements(UIArgs uiArgs = null)
    {
        _confirmBtn = RootObj.transform.FindComponent<Button>("bg/confirmBtn");
        _cancelBtn = RootObj.transform.FindComponent<Button>("bg/cancelBtn");
        _restoreTask = RootObj.transform.FindComponent<Toggle>("bg/RestoreTask");
        _startNewTask = RootObj.transform.FindComponent<Toggle>("bg/StartNewTask");
        _restoreTask.onValueChanged.AddListener(value =>
        {
            if (value)
            {
                _startNewTask.isOn = false;
            }
        });
        _startNewTask.onValueChanged.AddListener(value =>
        {
            if (value)
            {
                _restoreTask.isOn = false;
            }
        });
        UpdateUI(uiArgs);
    }
    
    public void UpdateUI(UIArgs uiArgs)
    {
        if (uiArgs == null)
        {
            return;
        }
        // 类型安全检查
        if (uiArgs is ConfirmTaskPanelArgs args)
        {
            _startNewTask.isOn = false;
            _restoreTask.isOn = false;
            if (_confirmBtn != null)
            {
                _confirmBtn.onClick.RemoveAllListeners();
                _confirmBtn.onClick.AddListener(() =>
                {
                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.CloseUI(UIID.ConfirmStartTaskPanel);
                    }

                    if (_restoreTask.isOn==true ||_startNewTask.isOn==true)
                    {
                        TaskDataManager.Instance.SendTaskCommand(args.TaskCommand);
                    }
                    args.ConfirmAction?.Invoke();
                });
            }

            if (_cancelBtn != null)
            {
                _cancelBtn.onClick.RemoveAllListeners();
                _cancelBtn.onClick.AddListener(() =>
                {
                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.CloseUI(UIID.ConfirmStartTaskPanel);
                    }
                    args.CancleAction?.Invoke();
                    if (_ctr != null)
                    {
                        _ctr.HideView();
                    }
                });
            }
        }
    }
}
