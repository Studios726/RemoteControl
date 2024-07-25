using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BucketWheelStateBase : MonoBehaviour
{
    /// <summary>
    /// 远程控制
    /// </summary>
    public Toggle remoteControl;
    /// <summary>
    /// 电源合闸
    /// </summary>
    public Toggle powerSupplyClose;
    /// <summary>
    /// 与系统连锁
    /// </summary>
    public Toggle systemChain;
    /// <summary>
    /// 通訊状态
    /// </summary>
    public Toggle communicationStatus;
    /// <summary>
    /// 通訊状态 红色灯
    /// </summary>
    public GameObject red;
    /// <summary>
    /// 通訊状态 黄色灯
    /// </summary>
    public GameObject yellow;
    /// <summary>
    /// 检修
    /// </summary>
    public Toggle recondition;
    /// <summary>
    /// 斗轮机故障
    /// </summary>
    public Toggle bucketWheelMalfunction;
    /// <summary>
    /// 蜂鸣报警
    /// </summary>
    public Toggle buzzerAlarm;
    /// <summary>
    /// 斗轮运行
    /// </summary>
    public Toggle bucketWheelRun;
    /// <summary>
    /// 允许取料信号
    /// </summary>
    public Toggle reclaimerSignal;
    /// <summary>
    /// 斗轮机取料运行
    /// </summary>
    public Toggle reclaimerRun;
    /// <summary>
    /// 俯仰上俯
    /// </summary>
    public Toggle pitchingUp;
    /// <summary>
    /// 俯仰下俯
    /// </summary>
    public  Toggle pitchingDown;
    /// <summary>
    /// 左转运行
    /// </summary>
    public Toggle leftTurnRun;
    /// <summary>
    /// 右转运行
    /// </summary>
    public  Toggle rightTurnRun;
    /// <summary>
    /// 后退运行
    /// </summary>
    public Toggle backTurnRun;
    /// <summary>
    /// 前进运行
    /// </summary>
    public Toggle fowardTurnRun;
    
    public float pastTime = 0;
    public virtual void UpdateData<T>(T data)
    {
        Debug.Log("更新参数状态");
    }

    public void Update()
    {
        if (true)//GameDataManager.Instance.Connection.isConnect==false
        {
            if (communicationStatus.isOn==true)
            {
                ConnectionStatus(false);
            }
        }
        else
        {
            if (communicationStatus.isOn==false)
            {
                ConnectionStatus(true);
            }
        }
        if (communicationStatus==null||communicationStatus.isOn)
        {
            return;
        }
        pastTime=pastTime+Time.deltaTime;
     
        if (pastTime >=1)
        {
            pastTime = 0;
            red.SetActive(!red.activeSelf);
            if (yellow.activeSelf==false)
            {
                yellow.SetActive(true);
            }
        }
    }

    public void ConnectionStatus(bool isSucc)
    {
        communicationStatus.isOn=isSucc;
        red.SetActive(!isSucc);
        yellow.SetActive(!isSucc);
    }
}
