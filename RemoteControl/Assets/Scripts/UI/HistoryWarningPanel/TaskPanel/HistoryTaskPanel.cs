using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class HistoryTaskPanel : MonoBehaviour
{
    public SearchPanel searchPanel;
    List<HistoryTaskData> historyTaskDatas = new List<HistoryTaskData>();
    public HistoryTaskList historyTaskList;
    MySqlDataReader mySqlDataReader;
    private void Start()
    {
        searchPanel.SetSearchAction(SearchRecord);
        InitRecord();
    }
    public void InitRecord()
    {
         mySqlDataReader = DataManager.Instance.GetHistoryTaskMc(10);
         ReadRecord();
    }

    public void ReadRecord()
    {
        int counter = 0;
        historyTaskDatas.Clear();
        while (mySqlDataReader.Read())
        {
            HistoryTaskData data = new HistoryTaskData();
            data.id = mySqlDataReader[ConstStr.DATA_TASK_ID].ToString(); //DateTime.Now.ToString("yyMMddHHmmss");
            data.time = mySqlDataReader[ConstStr.DATA_TASK_CREATE_TIME].ToString();
            data.machine = mySqlDataReader[ConstStr.DATA_MACHINE].ToString() == "0" ? "1#" : "2#";
            data.taskType = mySqlDataReader[ConstStr.DATA_TASK_TYPE].ToString() == "0" ? "∂—¡œ" : "»°¡œ";
            data.thingRange = mySqlDataReader[ConstStr.DATA_MATERIAL_RANGE_START].ToString() + "-" + mySqlDataReader[ConstStr.DATA_MATERIAL_RANGE_END].ToString();
            data.leftRightRange = mySqlDataReader[ConstStr.DATA_LEFT_RIGHT_RANGE_START].ToString() + "-" + mySqlDataReader[ConstStr.DATA_LEFT_RIGHT_RANGE_END].ToString();
            data.leftRightSelect = mySqlDataReader[ConstStr.DATA_LEFT_RIGHT_RANGE_START].ToString();
            data.takePileLength = mySqlDataReader[ConstStr.DATA_TASK_TAKE_MATE_HIGH].ToString();
            data.layerHigh = mySqlDataReader[ConstStr.DATA_TASK_LAYER_HIGH].ToString();
            data.timeAt = mySqlDataReader[ConstStr.DATA_TIMEDAT].ToString();
            data.quantity = mySqlDataReader[ConstStr.DATA_QUANTITY].ToString();
            data.operationName = mySqlDataReader[ConstStr.DATA_OPERATOR].ToString();
            data.state = mySqlDataReader[ConstStr.DATA_TASK_STATE].ToString();
            historyTaskDatas.Add(data);
            ++counter;
            if (counter == 1000)
            {
                counter = 0;
                break;
            }
        }
        RefreshRecord(historyTaskDatas);
    }
    public void SearchRecord(string startTime, string endTime, MechanicalType mechanicalType,string user)
    {
        string sql = $"SELECT * FROM {ConstStr.DATABASE_HISTORY_TASK_MC} WHERE"+$"`TaskCreateTime` BETWEEN '{startTime}' AND '{endTime}' ORDER BY `id` DESC;";
        mySqlDataReader =DataManager.Instance.GetHistoryTaskMcBySql(sql);
        ReadRecord();
    }
    public void RefreshRecord(List<HistoryTaskData> datas)
    {
        historyTaskList.RefreshList(datas);
    }
}
