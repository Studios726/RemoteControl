using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class ConfirmPanelView :UIView<ConfirmPanelCtr>
{
    private Text _des;
    private Button _confirmBtn;
    private Button _cancleBtn;
    
    
    public override void InitUIElements(UIArgs uiArgs = null)
    {
        
        _des = RootObj.transform.FindComponent<Text>("bg/des");
        _confirmBtn = RootObj.transform.FindComponent<Button>("bg/confirmBtn");
        _cancleBtn = RootObj.transform.FindComponent<Button>("bg/cancleBtn");
        UpdateUI(uiArgs);

    }

    public void UpdateUI(UIArgs uiArgs)
    {
        if (uiArgs==null)
        {
            return;
        }
        _confirmBtn.onClick.RemoveAllListeners();
        _cancleBtn.onClick.RemoveAllListeners();
        ConfirmPanelArgs args = (ConfirmPanelArgs)uiArgs;
        _des.text = args.Describe;
        _confirmBtn.onClick.AddListener(() =>
        {
            args.ConfirmAction?.Invoke();
            UIManager.Instance.CloseUI(UIID.ConfirmPanel);
        });
        _cancleBtn.onClick.AddListener(() =>
        {
            args.CancleAction?.Invoke();
            _ctr.HideView();
            UIManager.Instance.CloseUI(UIID.ConfirmPanel);
        });
    }
}
