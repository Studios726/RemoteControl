using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using ShangHaiPro;
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
   /// <summary>
   /// 左侧臂上雷达垂直距离
   /// </summary>
   public RadarParmItem LeftRadarVerticalDis;
   /// <summary>
   /// 右侧臂上雷达垂直距离
   /// </summary>
   public RadarParmItem RightRadarVerticalDis;
   /// <summary>
   /// 两长一短设置
   /// </summary>
   public TwoValueChange TwoShortOneLong;
   /// <summary>
   /// 振打电机开始时间和循环时间设置
   /// </summary>
   public TwoValueChange VibrationMotor;

   public Machine Machine;
   private string machineName;
   private Timer Timer;
   public void Awake()
   {
      HeapDis.InitName(ConstStr.DATA_TASK_CONFIG_HEAPDOS,$"定点堆的距离（堆料间隔）",Machine);
      FetchPileDepth.InitName(ConstStr.DATA_TASK_CONFIG_FETCHPILEDEPTH,"取料分层高度",Machine);
      FetchVerticalRangeAdd.InitName(ConstStr.DATA_TASK_CONFIG_FETCHVERTICALRANGEADD,"左右范围增加的长度",Machine);
      FetchHorizontalRangeSub.InitName(ConstStr.DATA_TASK_CONFIG_FETCHORIZONTALTANGESUB,"沿着轨道方向的取料范围缩减",Machine);
      LeftRadarPos.InitName(ConstStr.DATA_TASK_CONFIG_REVERSALSETLEFT,ConstStr.DATA_TASK_CONFIG_REVERSALSETLEFT_RIGHT,Machine,10,"左侧臂上雷达-换向设定");
      LeftRadarPos.onEndEdit = (() =>
      {
         TaskDataManager.Instance.SendTaskLidarDis(float.Parse(LeftRadarPos.setValueInputField.text),float.Parse(LeftRadarPos.setRightValueInputField.text),float.Parse(RightRadarPos.setValueInputField.text),float.Parse(RightRadarPos.setRightValueInputField.text),Machine);
      });
      RightRadarPos.InitName(ConstStr.DATA_TASK_CONFIG_REVERSALSETRIGHT,ConstStr.DATA_TASK_CONFIG_REVERSALSETRIGHT_RIGHT,Machine,10,"右侧臂上雷达-换向设定");
      RightRadarPos.onEndEdit = (() =>
      {
         TaskDataManager.Instance.SendTaskLidarDis(float.Parse(LeftRadarPos.setValueInputField.text),float.Parse(LeftRadarPos.setRightValueInputField.text),float.Parse(RightRadarPos.setValueInputField.text),float.Parse(RightRadarPos.setRightValueInputField.text),Machine);
      });
      
      //防撞
      
      LeftRadarCollision.InitName(ConstStr.DATA_TASK_CONFIG_COLLISIONLEFT,ConstStr.DATA_TASK_CONFIG_COLLISIONLEFT_RIGHT,Machine,10,"左侧臂上雷达-防撞设定");
      LeftRadarCollision.onEndEdit = (() =>
      {
         TaskDataManager.Instance.SendTaskLidarCollisionDis(float.Parse(LeftRadarCollision.setValueInputField.text),float.Parse(LeftRadarCollision.setRightValueInputField.text),float.Parse(RightRadarCollision.setValueInputField.text),float.Parse(RightRadarCollision.setRightValueInputField.text),Machine);
      });
      RightRadarCollision.InitName(ConstStr.DATA_TASK_CONFIG_COLLISIONRIGHT,ConstStr.DATA_TASK_CONFIG_COLLISIONRIGHT_RIGHT,Machine,10,"右侧臂上雷达-防撞设定");
      RightRadarCollision.onEndEdit = (() =>
      {
         TaskDataManager.Instance.SendTaskLidarCollisionDis(float.Parse(LeftRadarCollision.setValueInputField.text),float.Parse(LeftRadarCollision.setRightValueInputField.text),float.Parse(RightRadarCollision.setValueInputField.text),float.Parse(RightRadarCollision.setRightValueInputField.text),Machine);
      });
      //垂直距离
      LeftRadarVerticalDis.InitName(ConstStr.DATA_TASK_CONFIG_VERTICALLEFT,ConstStr.DATA_TASK_CONFIG_VERTICALLEFT_RIGHT,Machine,10,"左侧臂上雷达-换层设定");
      LeftRadarVerticalDis.onEndEdit = (() =>
      {
         TaskDataManager.Instance.SendTaskLidarVerticalDis(float.Parse(LeftRadarVerticalDis.setValueInputField.text),float.Parse(LeftRadarVerticalDis.setRightValueInputField.text),float.Parse(RightRadarVerticalDis.setValueInputField.text),float.Parse(RightRadarVerticalDis.setRightValueInputField.text),Machine);
      });
      RightRadarVerticalDis.InitName(ConstStr.DATA_TASK_CONFIG_VERTICALRIGHT,ConstStr.DATA_TASK_CONFIG_VERTICALRIGHT_RIGHT,Machine,10,"右侧臂上雷达-换层设定");
      RightRadarVerticalDis.onEndEdit = (() =>
      {
         TaskDataManager.Instance.SendTaskLidarVerticalDis(float.Parse(LeftRadarVerticalDis.setValueInputField.text),float.Parse(LeftRadarVerticalDis.setRightValueInputField.text),float.Parse(RightRadarVerticalDis.setValueInputField.text),float.Parse(RightRadarVerticalDis.setRightValueInputField.text),Machine);
      });
      machineName = Machine == Machine.BucketWheelStackerReclaimer ? "堆取料机" : "取料机";

      TwoShortOneLong.Init(ConstStr.DATA_TASK_CONFIG_TWO_SHORT_ONE_LONG_FIRST,
         ConstStr.DATA_TASK_CONFIG_TWO_SHORT_ONE_LONG_SECOND,"两短一长缩短度数1","两短一长缩短度数2", Machine, 100, 0, 100, 0, (
            () =>
            {
               TaskDataManager.Instance.SendTaskTwoShortOneLongList(float.Parse(TwoShortOneLong.FirstInputField.text),float.Parse(TwoShortOneLong.SecondInputField.text),Machine);
            }), () =>
         {
            TaskDataManager.Instance.SendTaskTwoShortOneLongList(float.Parse(TwoShortOneLong.FirstInputField.text),float.Parse(TwoShortOneLong.SecondInputField.text),Machine);
         });
      VibrationMotor.Init(ConstStr.DATA_TASK_CONFIG_VIBRATION_MOTOR_START,ConstStr.DATA_TASK_CONFIG_VIBRATION_MOTOR_LOOP,"振打电机首次启动时间","振打电机循环间隔",Machine,100,1,100,1,
         (() =>
         {
            TaskDataManager.Instance.SendTaskVibrationMotorList(float.Parse(VibrationMotor.FirstInputField.text),float.Parse(VibrationMotor.SecondInputField.text),Machine);
         }), () =>
         {
            TaskDataManager.Instance.SendTaskVibrationMotorList(float.Parse(VibrationMotor.FirstInputField.text),float.Parse(VibrationMotor.SecondInputField.text),Machine);
         });
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
            
            LeftRadarVerticalDis.SetTextValue(GameDataManager.Instance.GetBucketLidarVerticalDis(machineName,0));
            RightRadarVerticalDis.SetTextValue(GameDataManager.Instance.GetBucketLidarVerticalDis(machineName,1));
            
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
            LeftRadarPos.SetTaskValue(float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_REVERSALSETLEFT].ToString()),float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_REVERSALSETLEFT_RIGHT].ToString()));
            RightRadarPos.SetTaskValue(float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_REVERSALSETRIGHT].ToString()),float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_REVERSALSETRIGHT_RIGHT].ToString()));
            LeftRadarCollision.SetTaskValue(float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_COLLISIONLEFT].ToString()),float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_COLLISIONLEFT_RIGHT].ToString()));
            RightRadarCollision.SetTaskValue(float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_COLLISIONRIGHT].ToString()),float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_COLLISIONRIGHT_RIGHT].ToString()));
            LeftRadarVerticalDis.SetTaskValue(float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_VERTICALLEFT].ToString()),float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_VERTICALLEFT_RIGHT].ToString()));
            RightRadarVerticalDis.SetTaskValue(float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_VERTICALRIGHT].ToString()),float.Parse(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_VERTICALRIGHT_RIGHT].ToString()));
            TwoShortOneLong.SetValue(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_TWO_SHORT_ONE_LONG_FIRST].ToString(),dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_TWO_SHORT_ONE_LONG_SECOND].ToString());
            VibrationMotor.SetValue(dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_VIBRATION_MOTOR_START].ToString(),dataRowCollection[i][ConstStr.DATA_TASK_CONFIG_VIBRATION_MOTOR_LOOP].ToString());
         }
      }
   }

   private void OnDisable()
   {
      Timer?.Pause();
   }
}
