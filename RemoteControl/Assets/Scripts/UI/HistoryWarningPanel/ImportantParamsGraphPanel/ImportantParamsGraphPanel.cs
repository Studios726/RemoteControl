using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO;
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

public class ElectricityData
{
    public string Name { get;  set; }
    public DateTime Time { get;  set; }

    public int Value {  get;  set; }
}
public class HistoryChartData
{
    public LineChart linechart { get; set; }
    public string TableName {  get; set; }

    public Machine Machine { get; set; }

    public void SetData(LineChart _lineChart,string _name,Machine _machine)
    {
        linechart = _lineChart;
        TableName = _name;
        Machine= _machine;
    }
}
public class ImportantParamsGraphPanel : MonoBehaviour
{
    public LineChart trolleyElectricityChart_1;
    public LineChart slewingChart_1;
    public LineChart suspensoidChart_1;
    public LineChart cantileverChart_1;
    public LineChart bucketWheelChart_1;
    public LineChart trolleyElectricityChart_2;
    public LineChart slewingChart_2;
    public LineChart suspensoidChart_2;
    public LineChart cantileverChart_2;
    public LineChart bucketWheelChart_2;
    private MySqlDataReader _dataReader = null;
    public ButtonCell bucketWheelCurrent_1;//���ֵ���
    public ButtonCell bucketWheelCurrent_2;
    public ButtonCell trolleyCurrent_1;//�󳵵���
    public ButtonCell trolleyCurrent_2;
    public ButtonCell slewingCurrent_1;//��ת����
    public ButtonCell slewingCurrent_2;
    public ButtonCell suspendedGelCurrent_1;//��������
    public ButtonCell suspendedGelCurrent_2;
    public ButtonCell cantileverCurrent_1;//���۵���
    public ButtonCell cantileverCurrent_2;
    public SearchPanel searchPanel;
    private ButtonCell lastButton;
    private LineChart lastChart;
    private HistoryChartData historyChartData;
    public void Start()
    {
        AddOnClickListener(bucketWheelCurrent_1, () =>
        {
            ResetLastButtonState(bucketWheelCurrent_1);
            UpdateCurChart(nameof(bucketWheelCurrent_1));
        });

        AddOnClickListener(bucketWheelCurrent_2, () =>
        {
            ResetLastButtonState(bucketWheelCurrent_2);
            UpdateCurChart(nameof(bucketWheelCurrent_2));
        });
        AddOnClickListener(trolleyCurrent_1, () =>
        {
            ResetLastButtonState(trolleyCurrent_1);
            UpdateCurChart(nameof(trolleyCurrent_1));
        });
        AddOnClickListener(trolleyCurrent_2, () =>
        {
            ResetLastButtonState(trolleyCurrent_2);
            UpdateCurChart(nameof(trolleyCurrent_2));
        });
        AddOnClickListener(slewingCurrent_1, () =>
        {
            ResetLastButtonState(slewingCurrent_1);
            UpdateCurChart(nameof(slewingCurrent_1));
        });
        AddOnClickListener(slewingCurrent_2, () =>
        {
            ResetLastButtonState(slewingCurrent_2);
            UpdateCurChart(nameof(slewingCurrent_2));
        });
        AddOnClickListener(suspendedGelCurrent_1, () =>
        {
            ResetLastButtonState(suspendedGelCurrent_1);
            UpdateCurChart(nameof(suspendedGelCurrent_1));
        });
        AddOnClickListener(suspendedGelCurrent_2, () =>
        {
            ResetLastButtonState(suspendedGelCurrent_2);
            UpdateCurChart(nameof(suspendedGelCurrent_2));
        });
        AddOnClickListener(cantileverCurrent_1, () =>
        {
            ResetLastButtonState(cantileverCurrent_1);
            UpdateCurChart(nameof(cantileverCurrent_1));
        });
        AddOnClickListener(cantileverCurrent_2, () =>
        {
            ResetLastButtonState(cantileverCurrent_2);
            UpdateCurChart(nameof(cantileverCurrent_2));
        });
        InitChart();
        lastButton = bucketWheelCurrent_1;
        historyChartData=new HistoryChartData();
        historyChartData.SetData(bucketWheelChart_1, ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC,Machine.BucketWheelStackerReclaimer);
        bucketWheelCurrent_1.Invoke();
        searchPanel.SetSearchAction(UpdateCurChartByTime);
    }
    public void InitChart()
    {
        trolleyElectricityChart_1.EnsureChartComponent<XAxis>().axisLabel.textStyle.fontSize = 14;
        trolleyElectricityChart_1.series[0].data.Clear();
        slewingChart_1.EnsureChartComponent<XAxis>().axisLabel.textStyle.fontSize = 14;
        slewingChart_1.series[0].data.Clear();
        suspensoidChart_1.EnsureChartComponent<XAxis>().axisLabel.textStyle.fontSize = 14;
        suspensoidChart_1.series[0].data.Clear();
        cantileverChart_1.EnsureChartComponent<XAxis>().axisLabel.textStyle.fontSize = 14;
        cantileverChart_1.series[0].data.Clear();
        bucketWheelChart_1.EnsureChartComponent<XAxis>().axisLabel.textStyle.fontSize = 14;
        bucketWheelChart_1.series[0].data.Clear();

        trolleyElectricityChart_2.EnsureChartComponent<XAxis>().axisLabel.textStyle.fontSize = 14;
        trolleyElectricityChart_2.series[0].data.Clear();
        slewingChart_2.EnsureChartComponent<XAxis>().axisLabel.textStyle.fontSize = 14;
        slewingChart_2.series[0].data.Clear();
        suspensoidChart_2.EnsureChartComponent<XAxis>().axisLabel.textStyle.fontSize = 14;
        suspensoidChart_2.series[0].data.Clear();
        cantileverChart_2.EnsureChartComponent<XAxis>().axisLabel.textStyle.fontSize = 14;
        cantileverChart_2.series[0].data.Clear();
        bucketWheelChart_2.EnsureChartComponent<XAxis>().axisLabel.textStyle.fontSize = 14;
        bucketWheelChart_2.series[0].data.Clear();
    }
    //public void OnEnable()
    //{
        
    //}
    public void ResetLastButtonState(ButtonCell btn)
    {
        if (lastButton != null)
        {
            lastButton.SetSelectState(false);
        }
        lastButton = btn;
        lastButton.SetSelectState(true);
    }
    public void ResetLastChart(LineChart go)
    {
        if(lastChart != null)
        {
            lastChart.gameObject.SetActive(false);
        }
        lastChart= go;
        lastChart.gameObject.SetActive(true);
    }
    public void SetHistoryPanel(HistoryPanelCtr historyPanelCtr)
    {
        searchPanel.SetHistoryPanel(historyPanelCtr);
    }
    public void UpdateCurChart(string str) {

        if (str == nameof(bucketWheelCurrent_1))
        {
            ResetLastChart(bucketWheelChart_1);
            historyChartData.SetData(bucketWheelChart_1, ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC, Machine.BucketWheelStackerReclaimer);
            if (bucketWheelChart_1.series[0].data.Count <= 0)
            {
                GetSqlData(bucketWheelChart_1, ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC, Machine.BucketWheelStackerReclaimer);
            }
            else
            {

            }
            //bucketWheelChart_1.AnimationReset();

        }
        else if (str == nameof(bucketWheelCurrent_2))
        {

            ResetLastChart(bucketWheelChart_2);
            historyChartData.SetData(bucketWheelChart_2, ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC, Machine.BucketWheel);
            if (bucketWheelChart_2.series[0].data.Count <= 0)
            {
                GetSqlData(bucketWheelChart_2, ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC, Machine.BucketWheel);
            }
            else
            {

            }
        }
        else if (str == nameof(trolleyCurrent_1))
        {
            ResetLastChart(trolleyElectricityChart_1);
            historyChartData.SetData(trolleyElectricityChart_1, ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC, Machine.BucketWheelStackerReclaimer);
            if (trolleyElectricityChart_1.series[0].data.Count <= 0)
            {
                GetSqlData(trolleyElectricityChart_1, ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC, Machine.BucketWheelStackerReclaimer);
            }
            else
            {

            }
        }
        else if (str == nameof(trolleyCurrent_2)) {
            ResetLastChart(trolleyElectricityChart_2);
            historyChartData.SetData(trolleyElectricityChart_2, ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC, Machine.BucketWheel);
            if (trolleyElectricityChart_2.series[0].data.Count <= 0)
            {
                GetSqlData(trolleyElectricityChart_2, ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC, Machine.BucketWheel);
            }
            else
            {

            }
        }
        else if(str== nameof(suspendedGelCurrent_1))
        {
         
            ResetLastChart(suspensoidChart_1);
            historyChartData.SetData(suspensoidChart_1, ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC, Machine.BucketWheelStackerReclaimer);
            if (suspensoidChart_1.series[0].data.Count <= 0)
            {
                GetSqlData(suspensoidChart_1, ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC, Machine.BucketWheelStackerReclaimer);
            }
            else
            {

            }

        }
        else if (str == nameof(suspendedGelCurrent_2))
        {
        
            ResetLastChart(suspensoidChart_2);
            historyChartData.SetData(suspensoidChart_2, ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC, Machine.BucketWheel);
            if (suspensoidChart_2.series[0].data.Count <= 0)
            {
                GetSqlData(suspensoidChart_2, ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC, Machine.BucketWheel);
            }
            else
            {

            }
        }
        
        else if (str == nameof(cantileverCurrent_1))
        {
            ResetLastChart(cantileverChart_1);
            historyChartData.SetData(cantileverChart_1, ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC, Machine.BucketWheelStackerReclaimer);
            if (cantileverChart_1.series[0].data.Count <= 0)
            {
                GetSqlData(cantileverChart_1, ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC, Machine.BucketWheelStackerReclaimer);
            }
            else
            {

            }
        }
        else if (str == nameof(cantileverCurrent_2))
        {
            ResetLastChart(cantileverChart_2);
            historyChartData.SetData(cantileverChart_2, ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC, Machine.BucketWheel);
            if (cantileverChart_2.series[0].data.Count <= 0)
            {
                GetSqlData(cantileverChart_2, ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC,Machine.BucketWheel);
            }
            else
            {

            }
        }
        else if (str == nameof(slewingCurrent_1))
        {
            ResetLastChart(slewingChart_1);
            historyChartData.SetData(slewingChart_1, ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC, Machine.BucketWheelStackerReclaimer);
            if (slewingChart_1.series[0].data.Count <= 0)
            {
                GetSqlData(slewingChart_1, ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC, Machine.BucketWheelStackerReclaimer);
            }
            else
            {

            }
        }
        else if (str == nameof(slewingCurrent_2))
        {
        
            ResetLastChart(slewingChart_2);
            historyChartData.SetData(slewingChart_2, ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC, Machine.BucketWheel);
            if (slewingChart_2.series[0].data.Count <= 0)
            {
                GetSqlData(slewingChart_2, ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC,Machine.BucketWheel);
            }
            else
            {

            }
        }
    }
    public void GetSqlData(LineChart lineChart,string chartName, Machine machine,bool isUseTime =false,string startTime="",string endTime="")
    {
        lineChart.series[0].data.Clear();
        _dataReader = DataManager.Instance.GetHistoryChartData(chartName, ((int)machine).ToString(), 3000, isUseTime, startTime, endTime);

        List<ElectricityData> electricityDataList = new List<ElectricityData>();
        int counter = 0;
        while (_dataReader.Read())
        {
            ElectricityData data = new ElectricityData();
            data.Name = _dataReader[1].ToString();
            data.Time = DateTime.Parse( _dataReader[2].ToString());
            data.Value = int.Parse(_dataReader[3].ToString());
            electricityDataList.Add(data);
            ++counter;
            if (counter == 3000)
            {
                counter = 0;
                break;
            }
        }
    
        for (int i = 0; i < electricityDataList.Count; i++)
        {
            lineChart.AddData(0, electricityDataList[i].Time, electricityDataList[i].Value);
        }
        _dataReader.Close();
    }

    public void UpdateCurChartByTime(string startTime, string endTime, MechanicalType mechanicalType,string other)
    {
        if (lastChart == null || historyChartData==null)
        {
            return;
        }
        GetSqlData(historyChartData.linechart, historyChartData.TableName, historyChartData.Machine,true, startTime, endTime);
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
