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
    LrStop,
    Up,
    Down,
    UdStop
}
public class DirectionIndicator : MonoBehaviour
{
    private GameObject Root;
    private GameObject MoveForward;
    private GameObject MoveBackward;
    private GameObject TurnLeft;
    private GameObject TurnRight;
    private GameObject Up;
    private GameObject Down;
    public Machine Machine;
    private Timer timer;
    private void Start()
    {
        Root= transform.Find("root").gameObject;
        MoveForward = transform.Find("root/前进").gameObject;
        MoveBackward = transform.Find("root/后退").gameObject;
        TurnLeft = transform.Find("root/左转").gameObject;
        TurnRight = transform.Find("root/右转").gameObject;
        Up = transform.Find("root/上").gameObject;
        Down = transform.Find("root/下").gameObject;
        timer= Timer.Register(0.5F, true, true, () => {
            Root.SetActive(!Root.activeSelf);
            });
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
    
    public void SetUD(ModelDirection ud)
    {
        if (ud==ModelDirection.Up)
        {
            SetActive(Up, true);
            SetActive(Down,false);
        }else if (ud==ModelDirection.Down)
        {
            SetActive(Up, false);
            SetActive(Down,true);
        }
        else
        {
            SetActive(Up, false);
            SetActive(Down,false);
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
                else if (data.Direction[i] == ModelDirection.Right ||data.Direction[i] == ModelDirection.Left ||data.Direction[i] == ModelDirection.LrStop)
                {
                    SetLR(data.Direction[i]);
                }else
                {
                    SetUD(data.Direction[i]);
                }
            }
        }
       
    }
    public void AddListener()
    {
        EventManager.Instance.AddListener(EventName.UpdateModelDirection, OnUpdateModelDirection);
    }

    private void OnDestroy()
    {
        if (timer!=null)
        {
            timer.Cancel();
            timer = null;
        }
    }
}
