using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Utility;
using XCharts.Runtime;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class ElectricityData
{
    public string Name { get;  set; }
    public DateTime Time { get;  set; }

    public int ElectricityValue {  get;  set; }
}
public class ImportantParamsGraphPanel : MonoBehaviour
{
    LineChart cartElectricityChart;
    private MySqlDataReader _dataReader = null;
    public ButtonCell bucketWheelCurrent_1;//斗轮电流
    public ButtonCell bucketWheelCurrent_2;
    public ButtonCell trolleyCurrent_1;//大车电流
    public ButtonCell trolleyCurrent_2;
    public ButtonCell slewingCurrent_1;//回转电流
    public ButtonCell slewingCurrent_2;
    public ButtonCell suspendedGelCurrent_1;//悬胶电流
    public ButtonCell suspendedGelCurrent_2;
    public ButtonCell cantileverCurrent_1;//悬臂电流
    public ButtonCell cantileverCurrent_2;
    public SearchPanel searchPanel;
    public ButtonCell lastButton;
    public void Start()
    {
        AddOnClickListener(bucketWheelCurrent_1, () =>
        {
            RestLastButtonState(bucketWheelCurrent_1);
            UpdateCurElectricity(nameof(bucketWheelCurrent_1));
        });

        AddOnClickListener(bucketWheelCurrent_2, () =>
        {
            RestLastButtonState(bucketWheelCurrent_2);
            UpdateCurElectricity(nameof(bucketWheelCurrent_2));
        });
        AddOnClickListener(trolleyCurrent_1, () =>
        {
            RestLastButtonState(trolleyCurrent_1);
            UpdateCurElectricity(nameof(trolleyCurrent_1));
        });
        AddOnClickListener(trolleyCurrent_2, () =>
        {
            RestLastButtonState(trolleyCurrent_2);
            UpdateCurElectricity(nameof(trolleyCurrent_2));
        });
        AddOnClickListener(slewingCurrent_1, () =>
        {
            RestLastButtonState(slewingCurrent_1);
            UpdateCurElectricity(nameof(slewingCurrent_1));
        });
        AddOnClickListener(slewingCurrent_2, () =>
        {
            RestLastButtonState(slewingCurrent_2);
            UpdateCurElectricity(nameof(slewingCurrent_2));
        });
        AddOnClickListener(suspendedGelCurrent_1, () =>
        {
            RestLastButtonState(suspendedGelCurrent_1);
            UpdateCurElectricity(nameof(suspendedGelCurrent_1));
        });
        AddOnClickListener(suspendedGelCurrent_2, () =>
        {
            RestLastButtonState(suspendedGelCurrent_2);
            UpdateCurElectricity(nameof(suspendedGelCurrent_2));
        });
        AddOnClickListener(cantileverCurrent_1, () =>
        {
            RestLastButtonState(cantileverCurrent_1);
            UpdateCurElectricity(nameof(cantileverCurrent_1));
        });
        AddOnClickListener(cantileverCurrent_2, () =>
        {
            RestLastButtonState(cantileverCurrent_2);
            UpdateCurElectricity(nameof(cantileverCurrent_2));
        });
        cartElectricityChart = transform.FindComponent<LineChart>("LineChart");
        cartElectricityChart.EnsureChartComponent<XAxis>().axisLabel.textStyle.fontSize = 14;
        lastButton = bucketWheelCurrent_1;
        bucketWheelCurrent_1.Invoke();
    }
    //public void OnEnable()
    //{
        
    //}
    public void RestLastButtonState(ButtonCell btn)
    {
        if (lastButton != null)
        {
            lastButton.SetSelectState(false);
        }
        lastButton = btn;
        lastButton.SetSelectState(true);
    }
    public void SetHistoryPanel(HistoryPanelCtr historyPanelCtr)
    {
        searchPanel.SetHistoryPanel(historyPanelCtr);
    }
    public void UpdateCurElectricity(string str) { 
    
        if (str== nameof(bucketWheelCurrent_1))
        {
            string history_cartelectricitySql = "Select * from history_cartelectricity ORDER BY id DESC LIMIT 4;";
            GetSqlData(history_cartelectricitySql);
        }
    }
    public void GetSqlData(string sql)
    {
        cartElectricityChart.series[0].data.Clear();
        //cartElectricityChart.RemoveData();
        _dataReader = MySqlHelper.ExecuteReader(sql);
        List<ElectricityData> electricityDataList = new List<ElectricityData>();
        int counter = 0;
        while (_dataReader.Read())
        {
            ElectricityData data = new ElectricityData();
            data.Name = _dataReader[1].ToString();
            data.Time = DateTime.Parse( _dataReader[2].ToString());
            data.ElectricityValue = int.Parse(_dataReader[3].ToString());
            electricityDataList.Add(data);
            //cartElectricityChart.AddData(0, data.Time, data.ElectricityValue);
            ++counter;
            if (counter == 100)
            {
                counter = 0;
                break;
            }
        }
    
        for (int i = 0; i < electricityDataList.Count; i++)
        {
            //cartElectricityChart.AddXAxisData(electricityDataList[i].Time.ToString("yyyy-MM-dd \n HH:mm:ss"));//\n
            cartElectricityChart.AddData(0, electricityDataList[i].Time, electricityDataList[i].ElectricityValue);
        }
        _dataReader.Close();
    }
    public void UpdateCartElectricity()
    {
        //cartElectricityChart.AddData(0, new DateTime(2024, 8, i + 1), a);
    }
    public void AddOnClickListener(ButtonCell btn, UnityAction action)
    {
        btn.AddListener(action);
    }
}
