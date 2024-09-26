using System;
using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using UnityEngine;

public class BeltCtrMove : MonoBehaviour
{
   public Animation beltAnim;
   public Animation beltAnim2;

   private void Start()
   {
      AddListenerEvent();
   }

   public void AddListenerEvent()
   {
      EventManager.Instance.AddListener(EventName.PlayBeltAnim,((sender, args) =>
      {
         if (args is BeltRunArgs beltRunArgs)
         {
            PlayBeltClip(beltRunArgs.isPlayPileBeltAnim,beltRunArgs.isPlayTakeBeltAnim);
         }
       
      } ));
      
      EventManager.Instance.AddListener(EventName.StopPlayBeltAnim,((sender, args) =>
      {
         PlayBeltClip(false,false);
      } ));
   }
   
   public void PlayBeltClip(bool isPlay,bool isPlay2)
   {
      if (beltAnim == null||beltAnim2==null)
      {
         Debug.Log(" rotClip is null");
         return;
      }
      if (isPlay && beltAnim.isPlaying==false)
      {
         beltAnim.Play("brltClip");
      }else if (isPlay==false && beltAnim.isPlaying==true)
      {
         beltAnim.Stop("brltClip");
      }
      
      if (isPlay2 && beltAnim2.isPlaying==false)
      {
         beltAnim2.Play("brltClip2");
      }else if (isPlay2==false && beltAnim2.isPlaying==true)
      {
         beltAnim2.Stop("brltClip2");
      }
   }
}
