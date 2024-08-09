using Knivt.Tools.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryTaskList : UICyclicScrollList<HistoryTaskCell, HistoryTaskData>
{
    private List<HistoryTaskData> datas = new List<HistoryTaskData>();
    //private void Start()
    //{
    //    // dataList1 = new List<TestData>();
    //    for (int i = 0; i < 20; i++)
    //    {
    //        HistoryTaskData data = new HistoryTaskData();
    //        data.id = DateTime.Now.ToString("yyMMddHHmmss");
    //        data.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
    //        data.machine = "���ֻ���ȡ�ϻ�";
    //        data.taskType = "ȡ��";
    //        data.thingRange = "100-100";
    //        data.leftRightRange = "100-100"; ;
    //        data.leftRightSelect = "��"; ;
    //        data.takePileLength = "100";
    //        data.layerHigh = "100"; ;
    //        data.timeAt = "10000"; ;
    //        data.quantity = "10555"; ;
    //        data.operationName = "������";
    //        data.state = "�����";
    //        datas.Add(data);
    //    }
    //    Initlize(datas);
    //}
    public void RefreshList(List<HistoryTaskData> historyDatas)
    {
        datas.Clear();
        for (int i = 0; i < historyDatas.Count; i++)
        {
            datas.Add(historyDatas[i]);
        }
        // datas = historyDatas;
        Initlize(datas);
    }
    protected override void ResetCellData(HistoryTaskCell cell, HistoryTaskData data, int dataIndex)
    {
        cell.gameObject.SetActive(true);
        cell.UpdateDisplay(data);
    }
}
