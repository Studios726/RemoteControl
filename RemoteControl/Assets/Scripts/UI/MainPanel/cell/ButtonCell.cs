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

    private void Awake()
    {
        btn = GetComponent<Button>();
        red = transform.Find("Image").gameObject;
        text = transform.FindComponent<Text>("Text");
    }

    public void AddListener(UnityAction action)
    {
        btn.onClick.AddListener(action);
    }
}