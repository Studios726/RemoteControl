using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoValueChange : MonoBehaviour
{
    public InputField FirstInputField;
    public InputField SecondInputField;
    private int firsrMaxNum;
    private int firsrMinNum;
    private Action firsrOnEndEdit;
    private string firsrCommondName;
    private Machine machine;
    private int secondMaxNum;
    private int secondMinNum;
    private Action secondOnEndEdit;
    private string secondCommondName;
    private string firstDes;
    private string secondDes;
    public void Awake()
    {
        FirstInputField.onEndEdit.AddListener(((string value) =>
        {
            float num = 0;
            if (float.TryParse(value,out num))
            {
                num = Mathf.Clamp(num, firsrMinNum, firsrMaxNum);
                
                FirstInputField.text=num.ToString();
                DataManager.Instance.InsertHistoryLogMc($"{firstDes}修改", GameDataManager.Instance.GetUserName(), machine);
                TaskDataManager.Instance.UpdateCommonTaskParameters(this.firsrCommondName,num.ToString(),machine);
                secondOnEndEdit?.Invoke();
            }
           
        }));
        SecondInputField.onEndEdit.AddListener(((string value) =>
        {
            float num = 0;
            if (float.TryParse(value,out num))
            {
                num = Mathf.Clamp(num, secondMinNum, secondMaxNum);
                SecondInputField.text=num.ToString();
                DataManager.Instance.InsertHistoryLogMc($"{secondDes}修改", GameDataManager.Instance.GetUserName(), machine);
                TaskDataManager.Instance.UpdateCommonTaskParameters(this.secondCommondName,num.ToString(),machine);
                firsrOnEndEdit?.Invoke();
            }
           
        }));
    }
    public void Init(string firsrCommondName,string secondCommondName,string firstDes,string secondDes, Machine machine,int firsrMaxNum,int firsrMinNum,int secondMaxNum,int secondMinNum,Action firsrOnEndEdit,Action secondOnEndEdit)
    {
        this.firsrCommondName = firsrCommondName;
        this.secondCommondName = secondCommondName;
        this.machine = machine;
        this.firsrMaxNum = firsrMaxNum;
        this.firsrMinNum = firsrMinNum;
        this.secondMaxNum = secondMaxNum;
        this.secondMinNum = secondMinNum;
        this.firsrOnEndEdit = firsrOnEndEdit;
        this.secondOnEndEdit = secondOnEndEdit;
        this.firstDes = firstDes;
        this.secondDes = secondDes;
    }
    public void SetValue(string firsrValue,string secondValue)
    {
        FirstInputField.text = firsrValue;
        SecondInputField.text = secondValue;
    }
}
