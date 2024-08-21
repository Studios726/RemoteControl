using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatusParmItemBase<T> : MonoBehaviour
{
    public virtual void UpdateData(T data,bool isConnect = false )
    {
    }
    public virtual void SetToggleState(ToggleDIY toggle, bool ison,bool isFault=true, bool isConnect=true) {
        if (isConnect) {
            if (ison)
            {
                if (isFault)
                {
                    toggle?.SetState(2);
                }
                else
                {
                    toggle?.SetState(1);
                }
                
            }
            else
            {
                if (isFault)
                {
                    toggle?.SetState(1);
                }
                else
                {
                    toggle?.SetState(2);
                }
            }
        }
        else
        {
            toggle?.SetState(0);
        }
       
    }
    public virtual void SetToggleState(Toggle toggle, bool ison)
    {
        if (toggle.isOn == ison)
        {
            return;
        }
        else if (toggle)
        {
            toggle.isOn = ison;
        }
        else
        {
            Debug.LogError("Toggle is null");
        }

    }
}
