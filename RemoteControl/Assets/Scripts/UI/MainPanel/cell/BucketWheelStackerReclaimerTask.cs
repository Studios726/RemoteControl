using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BucketWheelStackerReclaimerTask : BucketWheelTaskBase
{
    public InputField startPileMaterText;
    public InputField endPileMaterText;
    public Toggle leftPileMaterToggle;
    public Toggle rightPileMaterToggle;
    public InputField startLeftPileMaterText;
    public InputField endLeftPileMaterText;
    public InputField pileMaterHeightText;
    public ButtonCell pileMaterStartBtn;
    public ButtonCell pileMaterStopBtn;
    public ButtonCell pileMaterEndBtn;
    public override void Start()
    {
        base.Start();
        InputFieldValueRange(startLeftPileMaterText, 0, 350);
        InputFieldValueRange(endLeftPileMaterText, 0, 350);
        AddOnClickListener(pileMaterStartBtn,(() => { SendPileMaterCommand("启动");}));
        AddOnClickListener(pileMaterStopBtn,(() => {SendPileMaterCommand("停止");}));
        AddOnClickListener(pileMaterEndBtn,(() => {SendPileMaterCommand("结束");}));
    }

    public void SendPileMaterCommand(string message)
    {
        Debug.Log($"message {machine} {message}");
    }
}
