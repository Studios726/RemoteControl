using Knivt.Tools.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningList :UICyclicScrollList<WarningCell, WarningCellData>
{
    private List<WarningCellData> datas = new List<WarningCellData>();

    // private void Start()
    // {
    //     datas.Add((new WarningCellData("888888")));
    //     datas.Add((new WarningCellData("9999")));
    //     datas.Add((new WarningCellData("7777")));
    //     datas.Add((new WarningCellData("999")));
    //     datas.Add((new WarningCellData("22222")));
    //     Initlize(datas);
    // }

    public void RefreshList(List<WarningCellData> historyDatas)
    {
        datas.Clear();
        for (int i = 0; i < historyDatas.Count; i++)
        {
            datas.Add(historyDatas[i]);
        }
        Initlize(datas);
    }
    protected override void ResetCellData(WarningCell cell, WarningCellData data, int dataIndex)
    {
        cell.gameObject.SetActive(true);
        cell.UpdateDisplay(data);
    }
}
