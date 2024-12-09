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
    private Timer _timer;
    private float _duration;
    private float _duration2;
    private string _content;
    private GameObject _title;
    private Text _name;

    public override void InitUIElements(UIArgs uiArgs = null)
    {
        _des = RootObj.transform.FindComponent<Text>("bg/des");
        _name = RootObj.transform.FindComponent<Text>("bg/title/name");
        _title = RootObj.transform.Find("bg/title").gameObject;
        _confirmBtn = RootObj.transform.FindComponent<Button>("bg/confirmBtn");
        _cancelBtn = RootObj.transform.FindComponent<Button>("bg/cancelBtn");
        // 保证UI元素初始化后再调用UpdateUI，避免NullReferenceException
        UpdateUI(uiArgs);
    }

    public void UpdateUI(UIArgs uiArgs)
    {
        if (uiArgs == null)
        {
            return;
        }

        // 类型安全检查
        if (uiArgs is ConfirmPanelArgs args)
        {
            _content = args.Describe;
            _des.text = args.Describe;
            _title.SetActive(args.TitleName!="");
            _name.text=args.TitleName;
            if (_confirmBtn != null)
            {
                _confirmBtn.onClick.RemoveAllListeners();
                _confirmBtn.onClick.AddListener(() =>
                {
                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.CloseUI(UIID.ConfirmPanel);
                    }

                    if (_timer != null)
                    {
                        _timer.Cancel();
                        _timer = null;
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
                        UIManager.Instance.CloseUI(UIID.ConfirmPanel);
                    }

                    if (_timer != null)
                    {
                        _timer.Cancel();
                        _timer = null;
                    }

                    args.CancleAction?.Invoke();
                    if (_ctr != null)
                    {
                        _ctr.HideView();
                    }
                });
            }

            if (args.Duration != 0 || args.Duration2 != 0)
            {
                if (_timer != null)
                {
                    _duration = 0;
                    _duration2 = 0;
                    _timer.Cancel();
                    _timer = null;
                }
                _duration = args.Duration;
                _duration2 = args.Duration2;
                _des.text = string.Format(_content, _duration, _duration2);
                
                // _timer=Timer.Register(args.Duration,(() =>
                // {
                //     if (UIManager.Instance != null) 
                //     {
                //         UIManager.Instance.CloseUI(UIID.ConfirmPanel);
                //     }
                // }));
                _timer = Timer.Register(1, () =>
                {
                    _duration = _duration - 1;
                    _duration2 = _duration2 - 1;
                    _duration = _duration >= 0 ? _duration : 0;
                    _duration2 = _duration2 >= 0 ? _duration2 : 0;
                    _des.text = string.Format(_content, _duration, _duration2);
                    if (_duration <= 0 && _duration2 <= 0)
                    {
                        _timer?.Cancel();
                        _timer = null;
                        if (UIManager.Instance != null)
                        {
                            UIManager.Instance.CloseUI(UIID.ConfirmPanel);
                        }
                    }
                }, null, true);
            }
        }
    }
}