using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class BucketWheelCtrMoveBase : MonoBehaviour
{
    public Button hideBtn;
    public GameObject show;
    public GameObject hide;
    /// <summary>
    /// 大车左前防碰撞
    /// </summary>
    public Text carLeftForwardDistance;
    /// <summary>
    /// 大车右前防碰撞
    /// </summary>
    public Text carRightForwardDistance;
    /// <summary>
    /// 大车左后防碰撞
    /// </summary>
    public Text carLeftBackDistance;
    /// <summary>
    /// 大车右后防碰撞
    /// </summary>
    public Text carRightBackDistance;
    /// <summary>
    /// 悬臂左前防碰撞
    /// </summary>
    public Text cantileverLeftForwardDistance;
    /// <summary>
    /// 悬臂右前防碰撞
    /// </summary>
    public Text cantileverRightForwardDistance;
    /// <summary>
    /// 悬臂左中方防碰撞
    /// </summary>
    public Text cantileverLeftCenterDistance;
    /// <summary>
    /// 悬臂右中方防碰撞
    /// </summary>
    public Text cantileverRightCenterDistance;
    /// <summary>
    /// 悬臂左后防碰撞
    /// </summary>
    public Text cantileverLeftBackDistance;
    /// <summary>
    /// 悬臂右后防碰撞
    /// </summary>
    public Text cantileverRightBackDistance;
    /// <summary>
    /// 悬臂流量
    /// </summary>
    public Text cantileverFlow;
    /// <summary>
    /// 悬臂料位高度
    /// </summary>
    public Text cantileverHeight;
    /// <summary>
    /// 当次取料量
    /// </summary>
    public Text thisTakeMater;
    /// <summary>
    /// 当天取料量
    /// </summary>
    public Text dayTakeMater;
    /// <summary>
    /// 尾车电流
    /// </summary>
    public Text carTailElectricity;
    /// <summary>
    /// 悬臂皮带电流
    /// </summary>
    public Text cantileverBeltElectricity;
    /// <summary>
    /// 重置
    /// </summary>
    public Button takeResetBtn;
    /// <summary>
    /// 单动
    /// </summary>
    public ButtonCell aloneBtn;
    /// <summary>
    /// 联动
    /// </summary>
    public ButtonCell togetherBtn;
    /// <summary>
    /// 自动
    /// </summary>
    public ButtonCell automaticBtn;
    /// <summary>
    /// 取料
    /// </summary>
    public ButtonCell takeMaterBtn;
    /// <summary>
    /// 停止
    /// </summary>
    public ButtonCell stopTakeMaterBtn;
    /// <summary>
    /// 大车电流
    /// </summary>
    public Text carElectricity;
    /// <summary>
    /// 大车位置
    /// </summary>
    public Text carPos;
    /// <summary>
    /// 车快速
    /// </summary>
    public ButtonCell carFastBtn;
    /// <summary>
    /// 车慢速
    /// </summary>
    public ButtonCell carSlowBtn;
    /// <summary>
    /// 车停止
    /// </summary>
    public ButtonCell carStopBtn;
    /// <summary>
    /// 车后退
    /// </summary>
    public ButtonCell carBackBtn;
    /// <summary>
    /// 车前进
    /// </summary>
    public ButtonCell carForwardBtn;
    /// <summary>
    /// 斗轮电流
    /// </summary>
    public Text bucketWheelElectricity;

    /// <summary>
    /// 斗轮位置
    /// </summary>
    public Text bucketWheelPos;
    /// <summary>
    /// 回转电流
    /// </summary>
    public Text rotationElectricity;

    /// <summary>
    /// 回转角度
    /// </summary>
    public Text rotationAngle;
    /// <summary>
    /// 俯仰电流
    /// </summary>
    public Text upElectricity;
    /// <summary>
    /// 俯仰角度
    /// </summary>
    public Text upAngle;
    /// <summary>
    /// 上仰
    /// </summary>
    public ButtonCell upBtn;
    /// <summary>
    /// 下附
    /// </summary>
    public ButtonCell downBtn;
    /// <summary>
    /// 左转
    /// </summary>
    public  ButtonCell leftBtn;
    /// <summary>
    /// 右转
    /// </summary>
    public  ButtonCell rightBtn;
    
    /// <summary>
    /// 停止
    /// </summary>
    public  ButtonCell stopBtn;

    public Machine Machine;

    public virtual void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        AddOnClickListener(takeResetBtn, (() =>SendMessageToServer("归零")));
        AddOnClickListener(aloneBtn, (() => SendMessageToServer("控制方式单动")));
        AddOnClickListener(togetherBtn, (() => SendMessageToServer("控制方式联动")));
        AddOnClickListener(automaticBtn, (() => SendMessageToServer("控制方式自动")));
        AddOnClickListener(takeMaterBtn, (() => SendMessageToServer("堆、取料控制 取料")));
        AddOnClickListener(stopTakeMaterBtn, (() =>SendMessageToServer("堆、取料控制 停止")));
        AddOnClickListener(carFastBtn, (() => SendMessageToServer("大车快速")));
        AddOnClickListener(carSlowBtn, (() => SendMessageToServer("大车慢速")));
        AddOnClickListener(carBackBtn, (() => SendMessageToServer("大车后退")));
        AddOnClickListener(carStopBtn, (() => SendMessageToServer("大车停止")));
        AddOnClickListener(carForwardBtn, (() => SendMessageToServer("大车前进")));
        AddOnClickListener(upBtn,(() => SendMessageToServer("大车上仰")));
        AddOnClickListener(downBtn,(() => SendMessageToServer("大车下附")));
        AddOnClickListener(leftBtn,(() => SendMessageToServer("大车左转")));
        AddOnClickListener(rightBtn,(() => SendMessageToServer("大车右转")));
        AddOnClickListener(stopBtn,(() => SendMessageToServer("大车停止")));
    }
    public virtual void UpdateData<T>(T data)
    {
        Debug.Log("更新move  大车碰撞信息 ");
    }
    public  void AddOnClickListener(Button btn, UnityAction action)
    {
        btn.onClick.AddListener(action);
    }
    public void AddOnClickListener(ButtonCell btn, UnityAction action)
    {
        btn.AddListener(action);
    }

    public virtual void SendMessageToServer(string str)
    {
        Debug.Log($"sendMessage { Machine } {str}");
    }
}
