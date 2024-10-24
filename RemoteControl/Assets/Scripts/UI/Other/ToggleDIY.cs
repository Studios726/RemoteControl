using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image=UnityEngine.UI.Image;

public class ToggleDIY : MonoBehaviour
{
    public Image state1;
    public Image state2;
    public Image state3;
    public Color color1;
    public Color color2;
    public Color color3;
    public  int curState;
    public Timer _timer;
    private void Start()
    {
        state1.color = color1;
        state2.color = color2;
        state3.color = color3;
    }
    public void SetState(int state)
    {
        curState = state;
        state1.gameObject.SetActive(state == 0);
        state2.gameObject.SetActive(state == 1);
        state3.gameObject.SetActive(state == 2);
    }
    //特殊处理闪烁
    public void SetState(int state,bool isFlicker)
    {
        curState = state;
        if (isFlicker)
        {
            if (_timer==null)
            {
                _timer = Timer.Register(1, true, true, (() =>
                {
                    state3.gameObject.SetActive(!state3.gameObject.activeSelf);
                }));
            }
        }
        else
        {
            if (_timer!=null)
            {
                _timer.Cancel();
                _timer = null;
            }

            if (state3.gameObject.activeSelf==true)
            {
                state3.gameObject.SetActive(false);
            }
         
        }
        state1.gameObject.SetActive(state == 0);
        state2.gameObject.SetActive(state == 1);
        // state3.gameObject.SetActive(state == 2);
    }
}
