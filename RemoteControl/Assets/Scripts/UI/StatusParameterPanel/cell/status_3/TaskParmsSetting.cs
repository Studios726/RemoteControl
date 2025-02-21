using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Unity.VisualScripting;
using UnityEngine;

public class TaskParmsSetting : MonoBehaviour
{
   /// <summary>
   /// 定点堆的距离（堆料间隔）
   /// </summary>
   public TaskParmItem HeapDis;
   /// <summary>
   /// 斗轮机根据工作范围按照就近原则还是工作范围中第一个数据，默认就近原则，数值为0（就近堆料   起始点堆料）
   /// </summary>
   public TaskParmItem MoveModel;
   /// <summary>
   /// 斗轮机取料时每层下降的深度（取料分层高度）
   /// </summary>
   public TaskParmItem FetchPileDepth;
   /// <summary>
   ///  根据第一次三维的数据情况，获取第一次要刮取的范围后，往两侧增加（左右范围增加的长度）
   /// </summary>
   public TaskParmItem FetchVerticalRangeAdd;
   /// <summary>
   /// 斗轮机取料时沿着轨道的工作范围每取一层左右缩减的距离（沿着轨道方向的取料范围缩减）
   /// </summary>
   public TaskParmItem FetchHorizontalRangeSub;
   /// <summary>
   /// 左侧臂上雷达
   /// </summary>
   public RadarParmItem LeftRadarPos;
   /// <summary>
   /// 右侧臂上雷达
   /// </summary>
   public RadarParmItem RightRadarPos;
   
   /// <summary>
   /// 左侧臂上雷达防撞
   /// </summary>
   public RadarParmItem LeftRadarCollision;
   /// <summary>
   /// 右侧臂上雷达防撞
   /// </summary>
   public RadarParmItem RightRadarCollision;

   public Machine Machine;
   private string machineName;
   private Timer Timer;
   public void Awake()
   {
      HeapDis.InitName(ConstStr.DATA_TASK_CONFIG_HEAPDOS,$"定点堆的距离（堆料间隔）",Machine);
      FetchPileDepth.InitName(ConstStr.DATA_TASK_CONFIG_FETCHPILEDEPTH,"取料分层高度",Machine);
      FetchVerticalRangeAdd.InitName(ConstStr.DATA_TASK_CONFIG_FETCHVERTICALRANGEADD,"左右范围增加的长度",Machine);
      FetchHorizontalRangeSub.InitName(ConstStr.DATA_TASK_CONFIG_FETCHORIZONTALTANGESUB,"沿着轨道方向的取料范围缩减",Machine);
      LeftRadarPos.InitName(ConstStr.DATA_TASK_CONFIG_REVERSALSETLEFT,Machine,10);
      LeftRadarPos.onEndEdit = (() =>
      {
         TaskDataManager.Instance.SendTaskLidarDis(float.Parse(LeftRadarPos.setValueInputField.text),float.Parse(RightRadarPos.setValueInputField.text),Machine);
      });
      RightRadarPos.InitName(ConstStr.DATA_TASK_CONFIG_REVERSALSETRIGHT,Machine,10);
      RightRadarPos.onEndEdit = (() =>
      {
         TaskDataManager.Instance.SendTaskLidarDis(float.Parse(LeftRadarPos.setValueInputField.text),float.Parse(RightRadarPos.setValueInputField.text),Machine);
      });
      
      //防撞
      
      LeftRadarCollision.InitName(ConstStr.DATA_TASK_CONFIG_COLLISIONLEFT,Machine,10);
      LeftRadarCollision.onEndEdit = (() =>
      {
         TaskDataManager.Instance.SendTaskLidarCollisionDis(float.Parse(LeftRadarCollision.setValueInputField.text),float.Parse(RightRadarCollision.setValueInputField.text),Machine);
      });
      RightRadarCollision.InitName(ConstStr.DATA_TASK_CONFIG_COLLISIONRIGHT,Machine,10);
      RightRadarCollision.onEndEdit = (() =>
      {
         TaskDataManager.Instance.SendTaskLidarCollisionDis(float.Parse(LeftRadarCollision.setValueInputField.text),float.Parse(RightRadarCollision.setValueInputField.text),Machine);
      });
      machineName = Machine == Machine.BucketWheelStackerReclaimer ? "堆取料机" : "取料机";
   }

   private void OnEnable()
   {
      if (Timer==null)
      {
         Timer=Timer.Register(1, true, true, (() =>
         {
            LeftRadarPos.SetTextValue(GameDataManager.Instance.GetBucketLidarDisByMachine(machineName,0));
            RightRadarPos.SetTextValue(GameDataManager.Instance.GetBucketLidarDisByMachine(machineName,1));
            LeftRadarCollision.SetTextValue(GameDataManager.Instance.GetBucketLidarCollisionValueByMachine(machineName,0));
            RightRadarCollision.SetTextValue(GameDataManager.Instance.GetBucketLidarCollisionValueByMachine(machineName,1));
            
         }));
      }
      if (Timer.IsPaused)
      {
         Timer?.Resume();
      }
      
      DataSet dataSet=DataManager.Instance.GetTaskConfigMcData(Machine);
      if (dataSet!=null)
      {
         DataRowCollection dataRowCollection = dataSet.Tables[0].Rows;
         for (int i = 0; i < dataRowCollection.Count; i++)
         {
            HeapDis.SetCurValue(float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_HEAPDOS].ToString()));
            FetchPileDepth.SetCurValue(float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_FETCHPILEDEPTH].ToString()));
           
            FetchVerticalRangeAdd.SetCurValue(float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_FETCHVERTICALRANGEADD].ToString()));
            FetchHorizontalRangeSub.SetCurValue( float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_FETCHORIZONTALTANGESUB].ToString()));
            LeftRadarPos.SetTaskValue(float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_REVERSALSETLEFT].ToString()));
            RightRadarPos.SetTaskValue(float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_REVERSALSETRIGHT].ToString()));
            LeftRadarCollision.SetTaskValue(float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_COLLISIONLEFT].ToString()));
            RightRadarCollision.SetTaskValue(float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_COLLISIONRIGHT].ToString()));
         }
      }
   }

   private void OnDisable()
   {
      Timer?.Pause();
   }
}
