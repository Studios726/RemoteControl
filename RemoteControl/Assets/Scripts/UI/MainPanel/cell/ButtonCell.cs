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
    private Image bg;
    private Color bgColor1=new Color(1,1,1,1);
    private Color bgColor2 = new Color(1, 1, 1, 0);
    private Color textColor1 = new Color(0.1411765f, 1, 1, 1);
    private Color textColor2 = new Color(1, 0, 0.1803922f, 1);
    private void Awake()
    {
        bg = GetComponent<Image>();
        btn = GetComponent<Button>();
        red = transform.Find("Image").gameObject;
        text = transform.FindComponent<Text>("Text");
        select = transform.Find("select").gameObject;
    }

    public void SetTextColor(Color color)
    {
        if (text !=null&&text.color!=color)
        {
            text.color = color;
        }
    }

    public void SetBgAndTextColor(Color bgColor,Color textColor)
    {
        if (bg!=null&&bg.color!=bgColor)
        {
            bg.color = bgColor;
        }

        SetTextColor(textColor);
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
            if (state&&select!=null&&select.activeSelf==true)
            {
                select.SetActive(false);
            }
            return;
        }
        red.SetActive(state);
    }
    public void SetSystemState(bool state,Color bgColor,Color textColor)
    {
        SetBgAndTextColor(bgColor, textColor);
        SetSystemState(state);
    }
    public void SetSystemState(bool state,bool updateColor)
    {
        if (updateColor)
        {
            if (state)
            {
                SetBgAndTextColor(bgColor2, textColor2);
            }
            else
            {
                SetBgAndTextColor(bgColor1, textColor1);
            }
          
        }
        SetSystemState(state);
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
