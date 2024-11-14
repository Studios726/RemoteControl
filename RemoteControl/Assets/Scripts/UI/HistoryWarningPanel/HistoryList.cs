using System.Collections;
using System.Collections.Generic;
using Knivt.Tools.UI;
using UnityEngine;

public class HistoryList: UICyclicScrollList<HistoryCell, HistoryData>
{
    private List<HistoryData> datas=new List<HistoryData>();
    public PanelType PanelType;
    public void RefreshList(List<HistoryData> historyDatas,PanelType panelType)
    {
        PanelType = panelType;
        datas.Clear();
        for (int i = 0; i < historyDatas.Count; i++)
        {
            datas.Add(historyDatas[i]);
        }
        Initlize(datas);
    }
    protected override void ResetCellData(HistoryCell cell, HistoryData data, int dataIndex)
    {
        cell.gameObject.SetActive(true);
        cell.UpdateDisplay(data.id, data.time,data.info,data.user,PanelType==PanelType.AlarmPanel);
    }
}
