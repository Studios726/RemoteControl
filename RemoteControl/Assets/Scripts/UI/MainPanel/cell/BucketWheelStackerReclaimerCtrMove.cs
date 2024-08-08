using ShenYangRemoteSystem.Subclass;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BucketWheelStackerReclaimerCtrMove : BucketWheelCtrMoveBase
{
    /// <summary>
    /// 当次取料量
    /// </summary>
    public Text thisPileMater;
    /// <summary>
    /// 当天取料量
    /// </summary>
    public Text dayPileMater;
    /// <summary>
    /// d堆料重置
    /// </summary>
    public Button pileMaterResetBtn;
    public Button pileMaterTakeBtn;

    public override void Start()
    {
        base.Start();
        AddOnClickListener(pileMaterResetBtn,(() => SendMessage("堆料重置")));
        AddOnClickListener(pileMaterTakeBtn,(() => SendMessage("堆料")));
        
    }
    public override void UpdateData(SystemVariables data)
    {
        //Debug.Log("更新堆取料机碰撞信息");
    }
}
