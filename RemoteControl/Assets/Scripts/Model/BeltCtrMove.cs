using System;
using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using UnityEngine;

public class BeltCtrMove : MonoBehaviour
{
   public Animation beltAnim;
   public Animation beltAnim2;
   private Material[] beltMaterials =new Material[11];
   private Material[] beltMaterials2 =new Material[11];

   private void Start()
   {
      AddListenerEvent();
      AddBeltMaterials(transform.Find("belt1#"), beltMaterials);
      AddBeltMaterials(transform.Find("belt2#"), beltMaterials2);
   }

   public void AddBeltMaterials(Transform transform,Material[]materials)
   {
      for (int i = 0; i < transform.childCount; i++)
      {
         // Debug.Log(transform.GetChild(i).name);
         // Debug.Log($"AddBeltMaterials {transform.GetChild(i).name} {transform.GetChild(i).GetChild(0).name}");
         materials[i]=transform.GetChild(i).GetChild(0).GetComponent<MeshRenderer>().materials[0];
         materials[i].SetFloat("_FlowSpeed", 0);
      }
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

   public void SetFlowSpeed(Material[] materials, float speed)
   {
      for (int i = 0; i < materials.Length; i++)
      {
         if (materials[i].GetFloat("_FlowSpeed")!=speed)
         {
            materials[i].SetFloat("_FlowSpeed", speed);
         }
      }
   }
   public void PlayBeltClip(bool isPlay,bool isPlay2)
   {
      SetFlowSpeed(beltMaterials, isPlay?1:0);
      SetFlowSpeed(beltMaterials2, isPlay2?1:0);
      // TODO: 暂时关闭 beltAnim
      // if (beltAnim == null||beltAnim2==null)
      // {
      //    Debug.Log(" rotClip is null");
      //    return;
      // }
      // if (isPlay && beltAnim.isPlaying==false)
      // {
      //    beltAnim.Play("brltClip");
      // }else if (isPlay==false && beltAnim.isPlaying==true)
      // {
      //    beltAnim.Stop("brltClip");
      // }
      //
      // if (isPlay2 && beltAnim2.isPlaying==false)
      // {
      //    beltAnim2.Play("brltClip2");
      // }else if (isPlay2==false && beltAnim2.isPlaying==true)
      // {
      //    beltAnim2.Stop("brltClip2");
      // }
      
   }
}
