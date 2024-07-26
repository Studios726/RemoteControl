using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class ConfirmPanelView : UIView<ConfirmPanelCtr>
{
    private Text _des;
    private Button _confirmBtn;
    private Button _cancelBtn; // 修改按钮命名以保持一致性
    
    public override void InitUIElements(UIArgs uiArgs = null)
    {
        _des = RootObj.transform.FindComponent<Text>("bg/des");
        _confirmBtn = RootObj.transform.FindComponent<Button>("bg/confirmBtn");
        _cancelBtn = RootObj.transform.FindComponent<Button>("bg/cancelBtn");
        // 保证UI元素初始化后再调用UpdateUI，避免NullReferenceException
        if (_des != null && _confirmBtn != null && _cancelBtn != null) 
        {
            UpdateUI(uiArgs);
        }
    }

    // private T GetUIComponent<T>(string path) where T : UnityEngine.Component
    // {
    //     UnityEngine.Component component = RootObj.transform.FindComponent<T>(path);
    //     if (component == null)
    //     {
    //         Debug.LogError($"Component {path} not found!");
    //         return null;
    //     }
    //     return component.GetComponent<T>();
    // }

    public void UpdateUI(UIArgs uiArgs)
    {
        if (uiArgs == null || _confirmBtn == null || _cancelBtn == null) 
        {
            return;
        }
        
        // 类型安全检查
        if (uiArgs is ConfirmPanelArgs args)
        {
            _confirmBtn.onClick.RemoveAllListeners();
            _cancelBtn.onClick.RemoveAllListeners();
            
            _des.text = args.Describe;
            
            _confirmBtn.onClick.AddListener(() =>
            {
                args.ConfirmAction?.Invoke();
                if (UIManager.Instance != null) 
                {
                    UIManager.Instance.CloseUI(UIID.ConfirmPanel);
                }
            });
            
            _cancelBtn.onClick.AddListener(() =>
            {
                args.CancleAction?.Invoke();
                if (_ctr != null) 
                {
                    _ctr.HideView();
                }
                if (UIManager.Instance != null) 
                {
                    UIManager.Instance.CloseUI(UIID.ConfirmPanel);
                }
            });
        }
    }
}
