using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineMove : MonoBehaviour
{
   public float distance;
   public Vector3 initialPoint;
   public Transform rotationGo;
   public float curPercentage;
   public void UpdatePosAndRotaion(float percentage,float rotAngleY,float rotAngleZ)
   {
      percentage = percentage > 1 ? 1 :percentage;
      curPercentage = percentage;
      Vector3 curPos = initialPoint;
      curPos.x=curPos.x - distance * percentage;
      transform.localPosition = curPos;
      rotationGo.localRotation=Quaternion.Euler(new Vector3(0,rotAngleY,rotAngleZ));
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.U))
      {
         UpdatePosAndRotaion(0.3f, 20, 0);
      }else if (Input.GetKeyDown(KeyCode.I))
      {
         UpdatePosAndRotaion(0.5f, 40, 0);
      }
      else if (Input.GetKeyDown(KeyCode.O))
      {
         UpdatePosAndRotaion(0.7f, 10, 10);
      }
      else if (Input.GetKeyDown(KeyCode.P))
      {
         UpdatePosAndRotaion(1, 0, 0);
      }
      //
      // curPercentage = curPercentage + Time.deltaTime;
      // UpdatePosAndRotaion(curPercentage, 0, 0);
   }
}
