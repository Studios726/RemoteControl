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
    public void Start()
    {
        AddOnClickListener(bucketWheelCurrent_1, () =>
        {
            UpdateCurElectricity(nameof(bucketWheelCurrent_1));
        });
        cartElectricityChart = transform.FindComponent<LineChart>("LineChart");
        cartElectricityChart.EnsureChartComponent<XAxis>().axisLabel.textStyle.fontSize = 14;
    }
    public void UpdateCurElectricity(string str) { 
    
        if (str== nameof(bucketWheelCurrent_1))
        {
            string history_cartelectricitySql = "Select * from history_cartelectricity ORDER BY id DESC LIMIT 10;";
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
            cartElectricityChart.AddXAxisData(electricityDataList[i].Time.ToString("yyyy-MM-dd \n HH:mm:ss"));//\n
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
