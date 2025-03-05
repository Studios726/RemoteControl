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
    public Image redImage;
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
        redImage = transform.FindComponent<Image>("Image");
        redImage.color= new Color(1, 1, 1, 0.6f);
        text = transform.FindComponent<Text>("Text");
        select = transform.Find("select").gameObject;
    }

    public void SetRunImageColor(Color color)
    {
        if (redImage&&redImage.color!=color)
        {
            redImage.color = color;
        }
    }
    public void SetRedAlpha(float alpha)
    {
        if (redImage.color.a==alpha)
        {
            return;
        }
        redImage.color= new Color(0.3f, 0, 0, alpha);
    }
    public void SetTextColor(Color color)
    {
        if (text !=null&&text.color!=color)
        {
            text.color = color;
        }
    }
    public void SetText(string str)
    {
        if (text!=null&&text.text!=str)
        {
            text.text = str;
        }
    }
    public void SetBgColor(Color color)
    {
        if (bg!=null&&bg.color!=color)
        {
            bg.color = color;
        }

    }
    public void SetBgAndTextColor(Color bgColor,Color textColor)
    {
        SetBgColor(bgColor);

        SetTextColor(textColor);
    }

    public void SetSelectTimerEvent(float duration)
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
    }
    public void SetSelectState(bool state,float duration)
    {
        SetSelectTimerEvent(duration);
        SetSelectState(state);
    }
    public void SetSelectState(bool state,bool isReset=true)
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
        if (isReset&&state)
        {
            SetSelectTimerEvent(1.3f);
        }
   
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
        if (state&&select!=null&&select.activeSelf==true)
        {
            select.SetActive(false);
        }
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

    public void SetSystemState(bool state, bool isUpdateTextColor, bool isUpdateBgColor)
    {
        if (isUpdateTextColor)
        {
            SetTextColor(state?textColor2:textColor1);
        }

        if (isUpdateBgColor)
        {
            SetBgColor(state?bgColor2:bgColor1);
        }
        SetSystemState(state);
    }
    public void AddListener(UnityAction action)
    {
        if (btn==null)
        {
            // Debug.Log("按钮没有初始化");
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
