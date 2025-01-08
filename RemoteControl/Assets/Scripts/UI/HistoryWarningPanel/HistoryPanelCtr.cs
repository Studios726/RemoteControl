using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
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

   public override void SetPanelData(UIArgs uiArgs)
   {
       view.InitPanelUI();
   }

   public void SearchRecord(string startTime,string endTime, MechanicalType type,string OperatorPerson="")
   {
      // Debug.Log($"搜索开始日期{startTime}  结束日期{endTime} 机器类型{type}");
      string Tables = "";
      bool useDate = true, useOperator =OperatorPerson != string.Empty;
      if (startTime=="" || endTime=="")
      {
          useDate = false;
      }
      // Debug.Log("useOperator: "+useOperator);
      if (view.curPanelType==PanelType.AlarmPanel)
      {
          Tables =type==MechanicalType.StackerReclaimer? ConstStr.DATABASE_HISTORY_WARNING1_MC:ConstStr.DATABASE_HISTORY_WARNING2_MC;
      }
      else
      {
          Tables =type==MechanicalType.StackerReclaimer? ConstStr.DATABASE_HISTORY_LOG1_MC:ConstStr.DATABASE_HISTORY_LOG2_MC;
      }
      string sql = $"SELECT * FROM {Tables} WHERE ";
      
      if (!useDate && !useOperator)
      {
          sql = "Select * from " + Tables + " ORDER BY id DESC LIMIT 50;";
      }
      else if (useDate && useOperator)
      {
          sql += $"`operator` = '{OperatorPerson}' AND `time` BETWEEN '{startTime}' AND '{endTime}' ORDER BY `time` DESC;";
      }
      else if (useDate)
      {
          sql += $"`time` BETWEEN '{startTime}' AND '{endTime}' ORDER BY `time` DESC;";
      }
      else
      {
          sql += $"`operator` = '{OperatorPerson}' ORDER BY `time` DESC;";
      }
      
      if (_dataReader != null)
      {
          _dataReader.Close();
      }
      RequestData(sql,type,view.curPanelType);
   }

   public async void RequestData(string sql,MechanicalType mechanicalType,PanelType panelType)
   {
       // sql = "Select * from " + Tables + " ORDER BY id DESC LIMIT 50;";
       // _dataReader = MySqlHelper.ExecuteReader(sql);
       // Debug.LogError($">>>>>>>>>>>{sql}");
       // if (_dataReader!=null)
       // {
       //     Reader(mechanicalType,panelType);
       // }
       DataSet dataSet = null;
       await Task.Run((() =>
       { 
           dataSet = DataManager.Instance.GetHistoryLog(sql);
       }));
       if (dataSet!=null)
       {
           ReaderLog(dataSet, mechanicalType, panelType);
       }
   
   }

   public void ReaderLog(DataSet dataSet,MechanicalType mechanicalType,PanelType panelType)
   {
       List<HistoryData> _historyDatas = new List<HistoryData>();
       DataRowCollection dataRowCollection = dataSet.Tables[0].Rows;
       int counter = 0;
       for (int i = 0; i < dataRowCollection.Count; i++)
       {
           HistoryData data = new HistoryData();
           data.id = dataRowCollection[i][ConstStr.DATA_HISTORY_LOGS_ID].ToString();
           data.time =  dataRowCollection[i][ConstStr.DATA_HISTORY_LOGS_TIME].ToString();
           data.info =  dataRowCollection[i][ConstStr.DATA_HISTORY_LOGS_INFO].ToString();
           data.user = dataRowCollection[i][ConstStr.DATA_HISTORY_LOGS_OPERATOR].ToString();
           _historyDatas.Add(data);
           ++counter;
           if (counter == 1000)
           {
               break;
           }
           
       }
       view.RefreshList(_historyDatas,mechanicalType,panelType);
   }
   private  void Reader(MechanicalType mechanicalType,PanelType panelType)
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
