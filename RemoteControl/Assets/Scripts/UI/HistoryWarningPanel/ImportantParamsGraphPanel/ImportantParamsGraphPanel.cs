using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using RemoteControl.Event;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Utility;
using XCharts.Runtime;
using Random = UnityEngine.Random;

public class ElectricityData
{
    public string Name { get;  set; }
    public DateTime Time { get;  set; }

    public int Value {  get;  set; }
}

public struct CData
{
   public DateTime date{ get;  set; }
   public float Value { get; set; }

   public CData(DateTime d,float v)
   {
       date = d;
       Value = v;
   }
}
public enum ChartName
{
    None,
    BucketWheelCurrent_1,
    BucketWheelCurrent_2,
    TrolleyCurrent_1,
    TrolleyCurrent_2,
    SlewingCurrent_1,
    SlewingCurrent_2,
    SuspendedGelCurrent_1,
    SuspendedGelCurrent_2,
    CantileverCurrent_1,
    CantileverCurrent_2,
 
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
    private DataSet dataSet;
    public ButtonCell bucketWheelCurrent_1;//斗轮电流
    public ButtonCell bucketWheelCurrent_2;
    public ButtonCell trolleyCurrent_1;//大车电流
    public ButtonCell trolleyCurrent_2;
    public ButtonCell slewingCurrent_1;//回转电流
    public ButtonCell slewingCurrent_2;
    public ButtonCell suspendedGelCurrent_1;//悬胶电流
    public ButtonCell suspendedGelCurrent_2;
    public ButtonCell cantileverCurrent_1;//悬臂流量
    public ButtonCell cantileverCurrent_2;
    public SearchPanel searchPanel;
    private ButtonCell lastButton;
    private LineChart lastChart;
    private HistoryChartData historyChartData;
    private bool dynamicUpdateData;
    private ChartName curChartName;
    private Queue<CData> tempChartData= new Queue<CData>();
    private float tempChartValue;
    public void Start()
    {
        AddOnClickListener(bucketWheelCurrent_1, () =>
        {
            ResetLastButtonState(bucketWheelCurrent_1);
            UpdateCurChart(nameof(bucketWheelCurrent_1));
            curChartName = ChartName.BucketWheelCurrent_1;
        });

        AddOnClickListener(bucketWheelCurrent_2, () =>
        {
            ResetLastButtonState(bucketWheelCurrent_2);
            UpdateCurChart(nameof(bucketWheelCurrent_2));
            curChartName = ChartName.BucketWheelCurrent_2;
        });
        AddOnClickListener(trolleyCurrent_1, () =>
        {
            ResetLastButtonState(trolleyCurrent_1);
            UpdateCurChart(nameof(trolleyCurrent_1));
            curChartName = ChartName.TrolleyCurrent_1;
        });
        AddOnClickListener(trolleyCurrent_2, () =>
        {
            ResetLastButtonState(trolleyCurrent_2);
            UpdateCurChart(nameof(trolleyCurrent_2));
            curChartName = ChartName.TrolleyCurrent_2;
        });
        AddOnClickListener(slewingCurrent_1, () =>
        {
            ResetLastButtonState(slewingCurrent_1);
            UpdateCurChart(nameof(slewingCurrent_1));
            curChartName = ChartName.SlewingCurrent_1;
        });
        AddOnClickListener(slewingCurrent_2, () =>
        {
            ResetLastButtonState(slewingCurrent_2);
            UpdateCurChart(nameof(slewingCurrent_2));
            curChartName = ChartName.SlewingCurrent_2;
        });
        AddOnClickListener(suspendedGelCurrent_1, () =>
        {
            ResetLastButtonState(suspendedGelCurrent_1);
            UpdateCurChart(nameof(suspendedGelCurrent_1));
            curChartName = ChartName.SuspendedGelCurrent_1;
        });
        AddOnClickListener(suspendedGelCurrent_2, () =>
        {
            ResetLastButtonState(suspendedGelCurrent_2);
            UpdateCurChart(nameof(suspendedGelCurrent_2));
            curChartName = ChartName.SuspendedGelCurrent_2;
        });
        AddOnClickListener(cantileverCurrent_1, () =>
        {
            ResetLastButtonState(cantileverCurrent_1);
            UpdateCurChart(nameof(cantileverCurrent_1));
            curChartName = ChartName.CantileverCurrent_1;
        });
        AddOnClickListener(cantileverCurrent_2, () =>
        {
            ResetLastButtonState(cantileverCurrent_2);
            UpdateCurChart(nameof(cantileverCurrent_2));
            curChartName = ChartName.CantileverCurrent_2;
        });
        InitChart();
        lastButton = bucketWheelCurrent_1;
        historyChartData=new HistoryChartData();
        historyChartData.SetData(bucketWheelChart_1, ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC,Machine.BucketWheelStackerReclaimer);
        bucketWheelCurrent_1.Invoke();
        searchPanel.SetSearchAction(UpdateCurChartByTime);
    }

    private void OnEnable()
    {
        tempChartData.Clear();
        EventManager.Instance.AddListener(EventName.UpdateChartData, DynamicUpdateData);
    }

    private void OnDisable()
    {
        searchPanel.Reset();
        tempChartData.Clear();
        EventManager.Instance.RemoveListener(EventName.UpdateChartData, DynamicUpdateData);
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
 
    public void ResetLastButtonState(ButtonCell btn)
    {
        
        if (lastButton != null)
        {
            lastButton.SetSelectState(false);
        }
        lastButton = btn;
        lastButton.SetSelectState(true,false);
        dynamicUpdateData = true;
        tempChartData.Clear();
        searchPanel.Reset();
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
            // if (bucketWheelChart_1.series[0].data.Count <= 0)
            // {
            //     GetSqlData(bucketWheelChart_1, ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC, Machine.BucketWheelStackerReclaimer,false,"","",true);
            // }
        }
        else if (str == nameof(bucketWheelCurrent_2))
        {

            ResetLastChart(bucketWheelChart_2);
            historyChartData.SetData(bucketWheelChart_2, ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC, Machine.BucketWheel);
            // if (bucketWheelChart_2.series[0].data.Count <= 0)
            // {
            //     GetSqlData(bucketWheelChart_2, ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC, Machine.BucketWheel,false,"","",true);
            // }
        }
        else if (str == nameof(trolleyCurrent_1))
        {
            ResetLastChart(trolleyElectricityChart_1);
            historyChartData.SetData(trolleyElectricityChart_1, ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC, Machine.BucketWheelStackerReclaimer);
            // if (trolleyElectricityChart_1.series[0].data.Count <= 0)
            // {
            //     GetSqlData(trolleyElectricityChart_1, ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC, Machine.BucketWheelStackerReclaimer,false,"","",true);
            // }
        }
        else if (str == nameof(trolleyCurrent_2)) {
            ResetLastChart(trolleyElectricityChart_2);
            historyChartData.SetData(trolleyElectricityChart_2, ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC, Machine.BucketWheel);
            // if (trolleyElectricityChart_2.series[0].data.Count <= 0)
            // {
            //     GetSqlData(trolleyElectricityChart_2, ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC, Machine.BucketWheel,false,"","",true);
            // }
        }
        else if(str== nameof(suspendedGelCurrent_1))
        {
         
            ResetLastChart(suspensoidChart_1);
            historyChartData.SetData(suspensoidChart_1, ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC, Machine.BucketWheelStackerReclaimer);
            // if (suspensoidChart_1.series[0].data.Count <= 0)
            // {
            //     GetSqlData(suspensoidChart_1, ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC, Machine.BucketWheelStackerReclaimer,false,"","",true);
            // }
        }
        else if (str == nameof(suspendedGelCurrent_2))
        {
        
            ResetLastChart(suspensoidChart_2);
            historyChartData.SetData(suspensoidChart_2, ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC, Machine.BucketWheel);
            // if (suspensoidChart_2.series[0].data.Count <= 0)
            // {
            //     GetSqlData(suspensoidChart_2, ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC, Machine.BucketWheel,false,"","",true);
            // }
        }
        
        else if (str == nameof(cantileverCurrent_1))
        {
            ResetLastChart(cantileverChart_1);
            historyChartData.SetData(cantileverChart_1, ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC, Machine.BucketWheelStackerReclaimer);
            // if (cantileverChart_1.series[0].data.Count <= 0)
            // {
            //     GetSqlData(cantileverChart_1, ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC, Machine.BucketWheelStackerReclaimer,false,"","",true);
            // }
        }
        else if (str == nameof(cantileverCurrent_2))
        {
            ResetLastChart(cantileverChart_2);
            historyChartData.SetData(cantileverChart_2, ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC, Machine.BucketWheel);
            // if (cantileverChart_2.series[0].data.Count <= 0)
            // {
            //     GetSqlData(cantileverChart_2, ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC,Machine.BucketWheel,false,"","",true);
            // }
        }
        else if (str == nameof(slewingCurrent_1))
        {
            ResetLastChart(slewingChart_1);
            historyChartData.SetData(slewingChart_1, ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC, Machine.BucketWheelStackerReclaimer);
            // if (slewingChart_1.series[0].data.Count <= 0)
            // {
            //     GetSqlData(slewingChart_1, ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC, Machine.BucketWheelStackerReclaimer,false,"","",true);
            // }
        }
        else if (str == nameof(slewingCurrent_2))
        {
        
            ResetLastChart(slewingChart_2);
            historyChartData.SetData(slewingChart_2, ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC, Machine.BucketWheel);
            // if (slewingChart_2.series[0].data.Count <= 0)
            // {
            //     GetSqlData(slewingChart_2, ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC,Machine.BucketWheel,false,"","",true);
            // }
        }
    } 
    public async void GetSqlData(LineChart lineChart,string chartName, Machine machine,bool isUseTime =false,string startTime="",string endTime="",bool isLimit=false,int limit=1000)
    {
        if (isLimit==false)
        {
            await Task.Run(() =>
            {
                dataSet = DataManager.Instance.GetHistoryChartDataSet(chartName, ((int)machine).ToString(),
                    isUseTime, startTime, endTime);
            });
        }
        else
        {
            await Task.Run(() =>
            {
               dataSet = DataManager.Instance.GetHistoryChartDataSet(chartName, ((int)machine).ToString(),
                    limit, isUseTime, startTime, endTime);
            });
        }
        dynamicUpdateData = false;
        lineChart.series[0].data.Clear();
        if (dataSet==null)
        {
            return;
        }
        int addNum=dataSet.Tables[0].Rows.Count / 1000;
        addNum = addNum == 0 ? 1 : addNum;
        DataRowCollection dataRowCollection = dataSet.Tables[0].Rows;
        for (int i = dataRowCollection.Count -1; i >= 0; i-=addNum)
        {
            lineChart.AddData(0, DateTime.Parse(dataRowCollection[i][ConstStr.DATA_HISTORY_CARTELECTRICITY_TIME].ToString()), float.Parse(dataRowCollection[i][ConstStr.DATA_HISTORY_CARTELECTRICITY_VALUE].ToString()));
        }
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

    private void UpdateChartData(float value)
    {
        lastChart.ClearData();
        // if (tempChartData.Count==0&& value<=1)
        // {
        //     value = 0.1f;
        // }
        tempChartData.Enqueue(new CData(DateTime.Now,value));
        if (tempChartData.Count>200)
        {
            tempChartData.Dequeue();
        }
        
        foreach (var data in tempChartData)
        {
            lastChart.AddData(0, data.date, data.Value);
        }
    }
    public void DynamicUpdateData(object sender, EventArgs e)
    {
        
        if (dynamicUpdateData&&GameDataManager.Instance.SystemVariables!=null)
        {
            if (curChartName==ChartName.BucketWheelCurrent_1)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.BucketWheelElectricCurrent;
            }else if (curChartName== ChartName.BucketWheelCurrent_2)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.BucketWheelElectricCurrent_2;
            }else if (curChartName==ChartName.TrolleyCurrent_1)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.LargeCarElectricCurrent;
            }else if (curChartName==ChartName.TrolleyCurrent_2)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.LargeCarElectricCurrent_2;
            }else if (curChartName==ChartName.SlewingCurrent_1)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.RotaryElectricCurrent;
            }else if (curChartName==ChartName.SlewingCurrent_2)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.RotaryElectricCurrent_2;
            }else if (curChartName==ChartName.SuspendedGelCurrent_1)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.SuspensionBeltElectricCurrent;
            }else if (curChartName==ChartName.SuspendedGelCurrent_2)
            {
               tempChartValue = GameDataManager.Instance.SystemVariables.SuspensionBeltElectricCurrent_2;
            }else if (curChartName==ChartName.CantileverCurrent_1)
            {
                // tempChartValue = GameDataManager.Instance.SystemVariables.SuspensionBeltElectricCurrent_2;
                tempChartValue = Random.Range(45, 50);
            }else if (curChartName==ChartName.CantileverCurrent_2)
            {
                // tempChartValue = GameDataManager.Instance.SystemVariables.SuspensionBeltElectricCurrent_2;
                tempChartValue = Random.Range(45, 50);
            }
            UpdateChartData(tempChartValue);
        }
    }
}
