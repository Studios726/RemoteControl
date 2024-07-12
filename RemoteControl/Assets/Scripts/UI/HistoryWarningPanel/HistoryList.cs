using System.Collections;
using System.Collections.Generic;
using Knivt.Tools.UI;
using UnityEngine;

public class HistoryList: UICyclicScrollList<HistoryCell, HistoryData>
{
    private List<HistoryData> datas=new List<HistoryData>();
    // private void Start()
    // {
    //     // dataList1 = new List<TestData>();
    //      datas = new HistoryData[20];
    //     for (int i = 0; i < datas.Length; i++)
    //     {
    //
    //         datas[i].id = "1234567";
    //         datas[i].time = "2024/06/30 18:52:30";
    //         datas[i].info = "Hello Word";
    //         datas[i].user = "管理者";
    //     }
    //     Initlize(datas);
    // }

    public void RefreshList(List<HistoryData> historyDatas)
    {
        datas.Clear();
        for (int i = 0; i < historyDatas.Count; i++)
        {
            datas.Add(historyDatas[i]);
        }
        // datas = historyDatas;
        Initlize(datas);
    }
    protected override void ResetCellData(HistoryCell cell, HistoryData data, int dataIndex)
    {
        cell.gameObject.SetActive(true);
        cell.UpdateDisplay(data.id, data.time,data.info,data.user);
    }
}
