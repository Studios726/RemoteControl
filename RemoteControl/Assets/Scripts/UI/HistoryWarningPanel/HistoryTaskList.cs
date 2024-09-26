using Knivt.Tools.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryTaskList : UICyclicScrollList<HistoryTaskCell, HistoryTaskData>
{
    private List<HistoryTaskData> datas = new List<HistoryTaskData>();
    
    public void RefreshList(List<HistoryTaskData> historyDatas)
    {
        datas.Clear();
        for (int i = 0; i < historyDatas.Count; i++)
        {
            datas.Add(historyDatas[i]);
        }
        Initlize(datas);
    }
    protected override void ResetCellData(HistoryTaskCell cell, HistoryTaskData data, int dataIndex)
    {
        cell.gameObject.SetActive(true);
        cell.UpdateDisplay(data);
    }
}
