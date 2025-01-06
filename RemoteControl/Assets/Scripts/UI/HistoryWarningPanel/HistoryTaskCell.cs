using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct HistoryTaskData
{
    public string id;
    public string time;
    public string machine;
    public string taskType;
    public string thingRange;
    public string leftRightRange;
    public string leftRightSelect;
    public string takePileLength;
    public string takeStepLength;
    public string pileHigh;
    public string layerHigh;
    public string timeAt;
    public string quantity;
    public string angleExpansionFactor;//扩角系数
    public string operationName;
    public string state;
    public string autoMode;
}
public class HistoryTaskCell : MonoBehaviour
{
    public Text idText;
    public Text timeText;
    public Text machineText;
    public Text taskTypeText;
    public Text thingRangeText;
    public Text leftRightRangeText;
    public Text leftRightSelectText;
    public Text takeStepLengthText;
    public Text pileHighText;
    public Text autoModeText;
    public Text angleExpansionFactor;
    public Text operationNameText;
    public Text stateText;
    public void UpdateDisplay(HistoryTaskData historyTaskData)
    {
        idText.text = historyTaskData.id;
        timeText.text = historyTaskData.time;
        machineText.text = historyTaskData.machine;
        taskTypeText.text = historyTaskData.taskType;
        thingRangeText.text = historyTaskData.thingRange;
        leftRightSelectText.text = historyTaskData.leftRightSelect;
        leftRightRangeText.text = historyTaskData.leftRightRange;
        takeStepLengthText.text = historyTaskData.takeStepLength;
        // takePileLengthText.text = historyTaskData.takePileLength;
        pileHighText.text = historyTaskData.pileHigh;
        autoModeText.text = historyTaskData.autoMode;
        angleExpansionFactor.text = historyTaskData.angleExpansionFactor;
        operationNameText.text = historyTaskData.operationName;
        stateText.text =historyTaskData.state=="1"? "进行中":"完成";
        
        
    }
}
