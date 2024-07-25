using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

public class MainPanelView :UIView<MainPanelCtr>
{
   private BucketWheelStateBase _bucketWheelState2;
   private BucketWheelStackerReclaimerState _bucketWheelState1;
   private BucketWheelCtrMoveBase _bucketWheelCtrMove2;
   private BucketWheelStackerReclaimerCtrMove _bucketWheelCtrMove1;
   private BucketWheelTaskBase _bucketWheelTask2;
   private BucketWheelStackerReclaimerTask _bucketWheelTask1;
   private HideButtonCtrBase _bucketWheelHideBtnCtr2;
   private BucketWheelStackerReclaimerHideBtnCtr _bucketWheelHideBtnCtr1;
   public override void InitUIElements(UIArgs uiArgs)
   {
      _bucketWheelState2 = RootObj.transform.FindComponent<BucketWheelStateBase>("machine_2/Bg_1");
      _bucketWheelCtrMove2=RootObj.transform.FindComponent<BucketWheelCtrMoveBase>("machine_2/Bg_2");
      _bucketWheelTask2 = RootObj.transform.FindComponent<BucketWheelTaskBase>("machine_2/Bg_3");
      _bucketWheelHideBtnCtr2 = RootObj.transform.FindComponent<HideButtonCtrBase>("machine_2/hideCtrBtns");
      
      _bucketWheelState1 = RootObj.transform.FindComponent<BucketWheelStackerReclaimerState>("machine_1/Bg_1");
      _bucketWheelCtrMove1=RootObj.transform.FindComponent<BucketWheelStackerReclaimerCtrMove>("machine_1/Bg_2");
      _bucketWheelTask1 = RootObj.transform.FindComponent<BucketWheelStackerReclaimerTask>("machine_1/Bg_3");
      _bucketWheelHideBtnCtr1 = RootObj.transform.FindComponent<BucketWheelStackerReclaimerHideBtnCtr>("machine_1/hideCtrBtns");

      _bucketWheelCtrMove2.hideBtn.onClick.AddListener(ActiveHideBtnCtr2);
      // AddOnClickListener(_bucketWheelCtrMove2.takeResetBtn, (() => _ctr.SendMessage("归零")));
      // AddOnClickListener(_bucketWheelCtrMove2.aloneBtn, (() => _ctr.SendMessage("控制方式单动")));
      // AddOnClickListener(_bucketWheelCtrMove2.togetherBtn, (() => _ctr.SendMessage("控制方式联动")));
      // AddOnClickListener(_bucketWheelCtrMove2.automaticBtn, (() => _ctr.SendMessage("控制方式自动")));
      // AddOnClickListener(_bucketWheelCtrMove2.takeMaterBtn, (() => _ctr.SendMessage("堆、取料控制 取料")));
      // AddOnClickListener(_bucketWheelCtrMove2.stopTakeMaterBtn, (() => _ctr.SendMessage("堆、取料控制 停止")));
      // AddOnClickListener(_bucketWheelCtrMove2.carFastBtn, (() => _ctr.SendMessage("堆、取料控制 停止")));
   
      _bucketWheelCtrMove1.hideBtn.onClick.AddListener(ActiveHideBtnCtr1);
      UpdateDdata(uiArgs);
   }

   private void ActiveHideBtnCtr2()
   {
      _bucketWheelHideBtnCtr2.gameObject.SetActive(!_bucketWheelHideBtnCtr2.gameObject.activeSelf);
      _bucketWheelCtrMove2.show.SetActive(!_bucketWheelHideBtnCtr2.gameObject.activeSelf);
      _bucketWheelCtrMove2.hide.SetActive(_bucketWheelHideBtnCtr2.gameObject.activeSelf);
   }
   private void ActiveHideBtnCtr1()
   {
      _bucketWheelHideBtnCtr1.gameObject.SetActive(!_bucketWheelHideBtnCtr1.gameObject.activeSelf);
      _bucketWheelCtrMove1.show.SetActive(!_bucketWheelHideBtnCtr1.gameObject.activeSelf);
      _bucketWheelCtrMove1.hide.SetActive(_bucketWheelHideBtnCtr1.gameObject.activeSelf);
   }

   public void UpdateDdata<T>(T data)
   {
      _bucketWheelState2.UpdateData(data);
      _bucketWheelState1.UpdateData(data);
      _bucketWheelCtrMove2.UpdateData(data);
      _bucketWheelCtrMove1.UpdateData(data);
   }

   public void UpdateDdata(object o, EventArgs eventArgs)
   {
      UpdateDdata(eventArgs);
   }

   public void AddOnClickListener(Button btn, UnityAction action)
   {
      btn.onClick.AddListener(action);
   }
   public void AddOnClickListener(ButtonCell btn, UnityAction action)
   {
      btn.AddListener(action);
   }
}
