using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskParmItem : MonoBehaviour
{
   /// <summary>
   /// 修改
   /// </summary>
   public ButtonCell buttonCell;
   public InputField inputField;
   private string name;
   private Machine Machine;
   private string des;
   public int min;
   public int max;

   private void Start()
   {
      buttonCell.AddListener((() =>
      {
         UIManager.Instance.OpenUI(UIID.ConfirmPanel,
            new ConfirmPanelArgs($"{des}修改？",GameDataManager.Instance.GetMachineName(Machine), null, () =>
            {
               DataManager.Instance.InsertHistoryLogMc($"{des}修改", GameDataManager.Instance.GetUserName(), Machine);
               TaskDataManager.Instance.UpdateCommonTaskParameters(name,inputField.text,Machine);
            }));
       
      }));
      InputFieldValueRange(inputField, min, max);
   }

   public void SetCurValue(float value)
   {
      inputField.text = value.ToString();
   }

   public void InitName(string str,string des,Machine machine)
   {
      name = str;
      Machine = machine;
      this.des = des;
   }
   public void InputFieldValueRange(InputField inputField, float min, float max)
   {
      inputField.onEndEdit.AddListener(((string value) =>
      {
         float num = 0;
         if (float.TryParse(value, out num))
         {
            num = num < min ? min : num;
            num = num > max ? max : num;
         }

         inputField.text = num.ToString();
      }));
   }
   
}
