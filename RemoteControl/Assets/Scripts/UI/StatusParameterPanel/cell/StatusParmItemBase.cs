using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatusParmItemBase<T> : MonoBehaviour
{
    public virtual void UpdateData(T data)
    {
    }
    public virtual void SetToggleState(Toggle toggle, bool ison) {
        if (toggle)
        {
            toggle.isOn = ison;
        }
        else
        {
            Debug.LogError("Toggle is null");
        }
    }
}
