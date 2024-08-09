using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using RemoteControl.Event;
using UnityEngine;

public class HistoryPanelCtr :UIPresenter<HistoryPanelView>
{
    private MySqlDataReader _dataReader = null;
    private List<HistoryData> _historyDatas = new List<HistoryData>();
   public override void ShowView(UIArgs uiArgs = null)
   {
      base.ShowView(uiArgs);
      
   }

   public void SearchRecord(string startTime,string endTime, MechanicalType type,string OperatorPerson="")
   {
      Debug.Log($"搜索开始日期{startTime}  结束日期{endTime} 机器类型{type}");
      string Tables = "";
      bool useDate = true, useOperator =OperatorPerson != string.Empty;
      if (startTime=="" || endTime=="")
      {
          useDate = false;
      }
      Debug.Log("useOperator: "+useOperator);
      if (view.curPanelType==PanelType.AlarmPanel)
      {
          Tables = "history_warning";
      }
      else
      {
          Tables = "history_logs";
      }
      string sql = $"SELECT * FROM {Tables} WHERE ";
      
      if (!useDate && !useOperator)
      {
          sql = "Select * from " + Tables + " ORDER BY id DESC LIMIT 50;";
      }
      else if (useDate && useOperator)
      {
          sql += $"`operator` = '{OperatorPerson}' AND `time` BETWEEN '{startTime}' AND '{endTime}' ORDER BY `id` DESC;";
      }
      else if (useDate)
      {
          sql += $"`time` BETWEEN '{startTime}' AND '{endTime}' ORDER BY `id` DESC;";
      }
      else
      {
          sql += $"`operator` = '{OperatorPerson}' ORDER BY `id` DESC;";
      }
      
      if (_dataReader != null)
      {
          _dataReader.Close();
      }
      Debug.LogError($"sql={sql}");
      RequestData(sql,type,view.curPanelType);
   }

   public void RequestData(string sql,MechanicalType mechanicalType,PanelType panelType)
   {
       // sql = "Select * from " + Tables + " ORDER BY id DESC LIMIT 50;";
       _dataReader = MySqlHelper.ExecuteReader(sql);
       Reader(mechanicalType,panelType);
   }
   private void Reader(MechanicalType mechanicalType,PanelType panelType)
   {
       int counter = 0;
       _historyDatas.Clear();
       while(_dataReader.Read())
       {
           HistoryData data = new HistoryData();
           data.id = _dataReader[0].ToString();
           data.time = _dataReader[1].ToString();
           data.info = _dataReader[2].ToString();
           data.user = _dataReader[3].ToString();
          
           _historyDatas.Add(data);
           ++counter;
           if (counter == 1000)
           {
               counter = 0;
               break;
           }
       }
       _dataReader.Close();
       view.RefreshList(_historyDatas,mechanicalType,panelType);
   }
}
