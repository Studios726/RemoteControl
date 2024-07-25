using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelBase : MonoBehaviour
{
    public virtual void AddOnClickListener(ButtonCell btn, UnityAction action)
    {
        btn.AddListener(action);
    }
    public virtual void AddOnClickListener(Button btn, UnityAction action)
    {
        btn.onClick.AddListener(action);
    }
}
