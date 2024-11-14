using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunStateTip : MonoBehaviour
{
    public Text des;
    public Text des2;
    public Text des3;
    public string curTip;
    private int count;
    public Timer Timer;
    public void SetText(string tip)
    {
        if (curTip==tip)
        {
            AddTimer();
            return;
        }

        if (gameObject.activeSelf==false)
        {
            gameObject.SetActive(true);
        }
        count=0;
        curTip = tip;
        des.text=tip+".";
        des2.text=tip+"..";
        des3.text=tip+"...";
        AddTimer();
    }

    public void AddTimer()
    {
        if (Timer==null)
        {
            Timer=Timer.Register(0.2f, true, true, (() =>
            {
                if (count>2)
                {
                    count = 0;
                }
                des.gameObject.SetActive(count==0);
                des2.gameObject.SetActive(count==1);
                des3.gameObject.SetActive(count==2);
                count++;
            }));
        }
    }
    public void Hide()
    {
        if (gameObject.activeSelf==true)
        {
            gameObject.SetActive(false);
        }
        if (Timer!=null)
        {
            Timer?.Cancel();
            Timer = null;
            curTip="";
            des.text="";
            des2.text="";
            des3.text="";
        }
      
    }
}
