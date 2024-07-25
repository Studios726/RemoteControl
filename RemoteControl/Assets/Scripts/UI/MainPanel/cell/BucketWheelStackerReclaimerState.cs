using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BucketWheelStackerReclaimerState : BucketWheelStateBase
{
   /// <summary>
   /// 堆料信号
   /// </summary>
   public Toggle storeSpaceSignal;
   /// <summary>
   /// 堆料运行
   /// </summary>
   public Toggle storeSpaceRun;
   /// <summary>
   /// 分流信号
   /// </summary>
   public Toggle shuntSignal;
   /// <summary>
   /// 分流运行
   /// </summary>
   public Toggle shuntRun;

   public override void UpdateData<T>(T data)
   {
      base.UpdateData(data);
      Debug.Log("更新状态");
   }
}
