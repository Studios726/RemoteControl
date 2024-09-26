using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

public class ButtonCell : MonoBehaviour
{
    private Button btn;
    public GameObject red;
    private Text text;
    public GameObject select;
    private UnityAction lastAction;
    private Timer _timer;
    private void Awake()
    {
        btn = GetComponent<Button>();
        red = transform.Find("Image").gameObject;
        text = transform.FindComponent<Text>("Text");
        select = transform.Find("select").gameObject;
    }

    public void SetSelectState(bool state,float duration)
    {
        if (_timer!=null)
        {
            _timer.Cancel();
            _timer = null;
        }
        _timer=Timer.Register(duration,(() =>
        {
            SetSelectState(false);
        }));
        SetSelectState(state);
    }
    public void SetSelectState(bool state)
    {
        if (select==null)
        {
            select = transform.Find("select").gameObject;
        }
        if (select==null)
        {
            Debug.Log($"  red is null {gameObject.name}");
            return;
        }
        if (select.activeSelf==state)
        {
            return;
        }
        select.SetActive(state);
    }
    public void SetSystemState(bool state)
    {
        if (red==null)
        {
            red = transform.Find("Image").gameObject;
        }

        if (red==null)
        {
            Debug.Log($" red is null  {gameObject.name}");
            return;
        }
        if (red.activeSelf==state)
        {
            return;
        }
        red.SetActive(state);
    }
    public void AddListener(UnityAction action)
    {
        if (btn==null)
        {
            Debug.Log("按钮没有初始化");
            return;
        }
        if (lastAction!=null)
        {
            btn.onClick.RemoveListener(lastAction);
        }
        lastAction = action;
        btn.onClick.AddListener(lastAction);
       
    }
    public void Invoke()
    {
        btn.onClick?.Invoke();
    }
}
