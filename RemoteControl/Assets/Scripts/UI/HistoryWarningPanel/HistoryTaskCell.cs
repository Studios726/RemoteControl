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
    public string layerHigh;
    public string timeAt;
    public string quantity;
    public string operationName;
    public string state;
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
    public Text takePileLengthText;
    public Text layerHighText;
    public Text timeAtText;
    public Text quantityText;
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
        takePileLengthText.text = historyTaskData.takePileLength;
        takePileLengthText.text = historyTaskData.takePileLength;
        layerHighText.text = historyTaskData.layerHigh;
        timeAtText.text = historyTaskData.timeAt;
        quantityText.text = historyTaskData.quantity;
        operationNameText.text = historyTaskData.operationName;
        stateText.text = historyTaskData.state;
    }
}
