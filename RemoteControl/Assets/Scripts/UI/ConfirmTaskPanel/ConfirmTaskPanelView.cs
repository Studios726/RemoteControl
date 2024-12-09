using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class ConfirmTaskPanelView: UIView<ConfirmTaskPanelCtr>
{
    private Button _confirmBtn;
    private Button _cancelBtn;
    private Toggle _zeroAdjustmentDevice;//归零
    private Toggle _closingDeviceToggle;//关闭设备
    private GameObject _title;
    private Text _name;
    public override void InitUIElements(UIArgs uiArgs = null)
    {
        _name = RootObj.transform.FindComponent<Text>("bg/title/name");
        _title = RootObj.transform.Find("bg/title").gameObject;
        _confirmBtn = RootObj.transform.FindComponent<Button>("bg/confirmBtn");
        _cancelBtn = RootObj.transform.FindComponent<Button>("bg/cancelBtn");
        _zeroAdjustmentDevice = RootObj.transform.FindComponent<Toggle>("bg/ZeroAdjustmentDevice");
        _closingDeviceToggle = RootObj.transform.FindComponent<Toggle>("bg/ClosingDevice");
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
            if (_confirmBtn != null)
            {
                _title.SetActive(args.TitleName!="");
                _name.text=args.TitleName;
                _confirmBtn.onClick.RemoveAllListeners();
                _confirmBtn.onClick.AddListener(() =>
                {
                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.CloseUI(UIID.ConfirmTaskPanel);
                    }

                    args.TaskCommand.FinishMethod = new List<int>(){0,0};
                    args.TaskCommand.FinishMethod[0]= _zeroAdjustmentDevice.isOn ? 1 : 0;
                    args.TaskCommand.FinishMethod[1] = _closingDeviceToggle.isOn ? 1 : 0;
                    Debug.Log($"{ args.TaskCommand.FinishMethod[0]} { args.TaskCommand.FinishMethod[1] }");
                    TaskDataManager.Instance.SendTaskCommand(args.TaskCommand);
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
                        UIManager.Instance.CloseUI(UIID.ConfirmTaskPanel);
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
