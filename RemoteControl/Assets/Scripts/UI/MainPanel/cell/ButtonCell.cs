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
    private GameObject red;
    private Text text;
    public GameObject select;
    private UnityAction lastAction;
    private void Awake()
    {
        btn = GetComponent<Button>();
        red = transform.Find("Image").gameObject;
        text = transform.FindComponent<Text>("Text");
        select = transform.Find("select").gameObject;
    }
    public void SetSelectState(bool state)
    {
        select.SetActive(state);
    }
    public void AddListener(UnityAction action)
    {
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
