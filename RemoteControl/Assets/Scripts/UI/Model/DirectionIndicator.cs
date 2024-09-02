using System;
using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using Unity.VisualScripting;
using UnityEngine;

public enum ModelDirection
{
    Forward,
    Backward,
    FbStop,
    Left,
    Right,
    LrStop
}
public class DirectionIndicator : MonoBehaviour
{
    private GameObject MoveForward;
    private GameObject MoveBackward;
    private GameObject TurnLeft;
    private GameObject TurnRight;
    public Machine Machine;

    private void Start()
    {
        MoveForward = transform.Find("前进").gameObject;
        MoveBackward = transform.Find("后退").gameObject;
        TurnLeft = transform.Find("左转").gameObject;
        TurnRight = transform.Find("右转").gameObject;
        AddListener();
    }

    public void SetActive(GameObject go, bool active)
    {
        if (go.activeSelf!=active)
        {
            go.SetActive(active);
        }
    }
    public void SetFB(ModelDirection fb)
    {
        if (fb==ModelDirection.Forward)
        {
            SetActive(MoveForward, true);
            SetActive(MoveBackward, false);
            
        }else if (fb==ModelDirection.Backward)
        {
            SetActive(MoveForward, false);
            SetActive(MoveBackward, true);

        }
        else
        {
            SetActive(MoveForward, false);
            SetActive(MoveBackward, false);

        }
    }
    public void SetLR(ModelDirection lr)
    {
        if (lr==ModelDirection.Left)
        {
            SetActive(TurnLeft, true);
            SetActive(TurnRight,false);
        }else if (lr==ModelDirection.Right)
        {
            SetActive(TurnLeft, false);
            SetActive(TurnRight,true);
        }
        else
        {
            SetActive(TurnLeft, false);
            SetActive(TurnRight,false);
        }
    }
    public void OnUpdateModelDirection(object sender, EventArgs args)
    {
        UpdateModelDirectionEventArgs data = args as UpdateModelDirectionEventArgs;
        if (Machine==data.Machine)
        {
            for (int i = 0; i < data.Direction.Length; i++)
            {
                if (data.Direction[i] == ModelDirection.Forward ||data.Direction[i] == ModelDirection.Backward||data.Direction[i] == ModelDirection.FbStop)
                {
                    SetFB(data.Direction[i]);
                }
                else
                {
                    SetLR(data.Direction[i]);
                }
            }
        }
       
    }
    public void AddListener()
    {
        EventManager.Instance.AddListener(EventName.UpdateModelDirection, OnUpdateModelDirection);
    } 
}
