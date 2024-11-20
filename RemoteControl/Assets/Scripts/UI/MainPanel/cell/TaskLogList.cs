using System.Collections;
using System.Collections.Generic;
using Knivt.Tools.UI;
using UnityEngine;

public class TaskLogList :UICyclicScrollList<TaskLogCell, TaskLogCellData>
{
    private List<TaskLogCellData> datas = new List<TaskLogCellData>();
    public void RefreshList(List<TaskLogCellData> historyDatas)
    {
        datas.Clear();
        for (int i = 0; i < historyDatas.Count; i++)
        {
            datas.Add(historyDatas[i]);
        }
        Initlize(datas);
    }
    protected override void ResetCellData(TaskLogCell cell, TaskLogCellData data, int dataIndex)
    {
        cell.gameObject.SetActive(true);
        cell.UpdateDisplay(data);
    }
}

