using UnityEngine;
using UnityEngine.UI;
//数据结构体
[System.Serializable]
public struct HistoryData
{
    public string id;
    public string time;
    public string info;
    public string user;
}
public class HistoryCell : MonoBehaviour
{
    public Text idText;
    public Text timeText;
    public Text infoText;
    public Text operatorUserText;
    public void UpdateDisplay(string userId, string time,string info,string name,bool isColor=false)
    {
        idText.text = userId;
        timeText.text = time;
        if (isColor)
        {
            if (info.Contains("解除")||info.Contains("成功"))
            {
                infoText.color = Color.white;
            }
            else
            {
                infoText.color = Color.red;
            }
        }
        infoText.text = info;
        operatorUserText.text = name;
    }
}
