using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public struct AngleCurrentValueData
{
    /// <summary>
    /// ˾���ҽǶ�
    /// </summary>
    public string cabAngleStr;
    /// <summary>
    /// ��ת�Ƕ� 
    /// </summary>
    public string slewingAngleStr;
    /// <summary>
    /// �����Ƕ�
    /// </summary>
    public string pitchAngleStr;
    /// <summary>
    ///  ��λ��  
    /// </summary>
    public string trolleyPositionStr;
    /// <summary>
    /// �������� 
    /// </summary>
    public string twoMachineDistanceStr;
    /// <summary>
    /// ��������
    /// </summary>
    public string diversionBaffleStr;
    /// <summary>
    ///  ��ת����
    /// </summary>
    public string slewingCurrentStr;
    /// <summary>
    ///  ��������
    /// </summary>
    public string suspendedGelCurrentStr;
    /// <summary>
    ///  �󳵵���
    /// </summary>
    public string trolleyCurrentStr;
    /// <summary>
    ///  ���ֵ���
    /// </summary>
    public string bucketWheelCurrentStr;
}
public class AngleCurrentValueItem : MonoBehaviour
{
    /// <summary>
    /// ˾���ҽǶ�
    /// </summary>
    public Text CabAngleText;
    /// <summary>
    /// ��ת�Ƕ� 
    /// </summary>
    public Text SlewingAngleText;
    /// <summary>
    /// �����Ƕ�
    /// </summary>
    public Text PitchAngleText;
    /// <summary>
    ///  ��λ��  
    /// </summary>
    public Text TrolleyPositionText;
    /// <summary>
    /// �������� 
    /// </summary>
    public Text TwoMachineDistanceText;
    /// <summary>
    /// ��������
    /// </summary>
    public Text DiversionBaffleText;
    /// <summary>
    ///  ��ת����
    /// </summary>
    public Text SlewingCurrentText;
    /// <summary>
    ///  ��������
    /// </summary>
    public Text SuspendedGelCurrentText;
    /// <summary>
    ///  �󳵵���
    /// </summary>
    public Text TrolleyCurrentText;
    /// <summary>
    ///  ���ֵ���
    /// </summary>
    public Text BucketWheelCurrentText;
    public void UpdateData(AngleCurrentValueData data)
    {
        SetText(CabAngleText, data.cabAngleStr, TextType.Angle);
        SetText(SlewingAngleText, data.slewingAngleStr, TextType.Angle);
        SetText(PitchAngleText, data.pitchAngleStr, TextType.Angle);
        SetText(TrolleyPositionText, data.trolleyPositionStr, TextType.Meter);
        SetText(TwoMachineDistanceText, data.twoMachineDistanceStr, TextType.Meter);
        SetText(DiversionBaffleText, data.diversionBaffleStr, TextType.None);
        SetText(SlewingCurrentText, data.slewingCurrentStr, TextType.Electricity);
        SetText(SuspendedGelCurrentText, data.suspendedGelCurrentStr, TextType.Electricity);
        SetText(TrolleyCurrentText, data.trolleyCurrentStr, TextType.Electricity);
        SetText(BucketWheelCurrentText, data.bucketWheelCurrentStr, TextType.Electricity);
    }
    void SetText(Text text,string str,TextType type)
    {
        if (text == null)
        {
            Debug.LogError(" Text is null ���� AngleCurrentValueItem");
            return;
        }
        if (text.text == str) {
            return;
        }
        text.SetTextSymbol(str, type);
    }
}
