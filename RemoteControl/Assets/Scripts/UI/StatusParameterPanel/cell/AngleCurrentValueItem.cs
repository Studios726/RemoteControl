using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public struct AngleCurrentValueData
{
    /// <summary>
    /// 司机室角度
    /// </summary>
    public string cabAngleStr;
    /// <summary>
    /// 回转角度 
    /// </summary>
    public string slewingAngleStr;
    /// <summary>
    /// 俯仰角度
    /// </summary>
    public string pitchAngleStr;
    /// <summary>
    ///  大车位置  
    /// </summary>
    public string trolleyPositionStr;
    /// <summary>
    /// 两机距离 
    /// </summary>
    public string twoMachineDistanceStr;
    /// <summary>
    /// 分流挡板
    /// </summary>
    public string diversionBaffleStr;
    /// <summary>
    ///  回转电流
    /// </summary>
    public string slewingCurrentStr;
    /// <summary>
    ///  悬胶电流
    /// </summary>
    public string suspendedGelCurrentStr;
    /// <summary>
    ///  大车电流
    /// </summary>
    public string trolleyCurrentStr;
    /// <summary>
    ///  斗轮电流
    /// </summary>
    public string bucketWheelCurrentStr;
}
public class AngleCurrentValueItem : MonoBehaviour
{
    /// <summary>
    /// 司机室角度
    /// </summary>
    public Text CabAngleText;
    /// <summary>
    /// 回转角度 
    /// </summary>
    public Text SlewingAngleText;
    /// <summary>
    /// 俯仰角度
    /// </summary>
    public Text PitchAngleText;
    /// <summary>
    ///  大车位置  
    /// </summary>
    public Text TrolleyPositionText;
    /// <summary>
    /// 两机距离 
    /// </summary>
    public Text TwoMachineDistanceText;
    /// <summary>
    /// 分流挡板
    /// </summary>
    public Text DiversionBaffleText;
    /// <summary>
    ///  回转电流
    /// </summary>
    public Text SlewingCurrentText;
    /// <summary>
    ///  悬胶电流
    /// </summary>
    public Text SuspendedGelCurrentText;
    /// <summary>
    ///  大车电流
    /// </summary>
    public Text TrolleyCurrentText;
    /// <summary>
    ///  斗轮电流
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
            Debug.LogError(" Text is null 请检查 AngleCurrentValueItem");
            return;
        }
        if (text.text == str) {
            return;
        }
        text.SetTextSymbol(str, type);
    }
}
