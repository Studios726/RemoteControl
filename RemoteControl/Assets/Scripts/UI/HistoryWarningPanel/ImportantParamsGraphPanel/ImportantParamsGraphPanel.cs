using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using RemoteControl.Event;
using ShangHaiPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;
using XCharts.Runtime;
using Random = UnityEngine.Random;

public class ElectricityData
{
    public string Name { get; set; }
    public DateTime Time { get; set; }

    public int Value { get; set; }
}

public struct CData
{
    public DateTime date { get; set; }
    public float Value { get; set; }

    public CData(DateTime d, float v)
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
    Position_1,
    RotationAngle_1,
    PitchAngle_1,
    SlewingCurrent_2,
    SuspendedGelCurrent_1,
    SuspendedGelCurrent_2,
    CantileverCurrent_1,
    CantileverCurrent_2,
    allElectricity_1,
    allElectricity_2,
    Position_2,
    RotationAngle_2,
    PitchAngle_2,
}

public class HistoryChartData
{
    public LineChart linechart { get; set; }
    public string TableName { get; set; }

    public Machine Machine { get; set; }

    public void SetData(LineChart _lineChart, string _name, Machine _machine)
    {
        linechart = _lineChart;
        TableName = _name;
        Machine = _machine;
    }
}

public class DateCell
{
    public string StartTime;
    public string EndTime;
    public bool IsDynamic;

    public DateCell(string startTime, string endTime, bool isDynamic = true)
    {
        StartTime = startTime;
        EndTime = endTime;
        IsDynamic = isDynamic;
    }
}

public class ImportantParamsGraphPanel : MonoBehaviour
{
    public LineChart trolleyElectricityChart_1;
    public LineChart slewingChart_1;
    public LineChart suspensoidChart_1;
    public LineChart cantileverChart_1;
    public LineChart bucketWheelChart_1;
    public LineChart allChart_1;
    public LineChart positionChart_1;
    public LineChart rotationAngleChart_1;
    public LineChart pitchAngleChart_1;
    public LineChart trolleyElectricityChart_2;
    public LineChart slewingChart_2;
    public LineChart suspensoidChart_2;
    public LineChart cantileverChart_2;
    public LineChart bucketWheelChart_2;
    public LineChart allChart_2;
    public LineChart positionChart_2;
    public LineChart rotationAngleChart_2;
    public LineChart pitchAngleChart_2;
    private MySqlDataReader _dataReader = null;
    private DataSet dataSet;
    public ButtonCell bucketWheelCurrent_1; //斗轮电流
    public ButtonCell bucketWheelCurrent_2;
    public ButtonCell trolleyCurrent_1; //大车电流
    public ButtonCell trolleyCurrent_2;
    public ButtonCell slewingCurrent_1; //回转电流
    public ButtonCell slewingCurrent_2;
    public ButtonCell suspendedGelCurrent_1; //悬胶电流
    public ButtonCell suspendedGelCurrent_2;
    public ButtonCell cantileverCurrent_1; //悬臂流量
    public ButtonCell cantileverCurrent_2;
    public ButtonCell allElectricity_1; //全部电流
    public ButtonCell allElectricity_2;
    
    public ButtonCell position_1; //大车位置
    public ButtonCell position_2;
    
    public ButtonCell rotationAngel_1; //回转角度
    public ButtonCell rotationAngel_2;
    
    public ButtonCell pitchAngle_1; //俯仰角度
    public ButtonCell pitchAngle_2;
    
    public SelectElectricityShowItem selectElectricityShowItem_1;
    public SelectElectricityShowItem selectElectricityShowItem_2;
    public SearchPanel searchPanel;

    public Button historyBtn; //历史记录
    public Button dynamicBtn; //实时数据
    private ButtonCell lastButton;
    private LineChart lastChart;
    private HistoryChartData historyChartData;
    private bool dynamicUpdateData;
    private ChartName curChartName;
    private Queue<CData> tempChartData = new Queue<CData>();
    private Dictionary<int, Queue<CData>> allTempChartData = new Dictionary<int, Queue<CData>>();
    private float tempChartValue;
    private Dictionary<string, DateCell> dateDic = new Dictionary<string, DateCell>();

    public List<string> ElectricityTableNameList = new List<string>()
    {
        ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC,
        ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC,
        ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC,
        ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC
    };

    public void Start()
    {
        InitDateDic();
        AddOnClickListener(bucketWheelCurrent_1, () =>
        {
            ResetLastButtonState(bucketWheelCurrent_1);
            curChartName = ChartName.BucketWheelCurrent_1;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(bucketWheelCurrent_1));
        });

        AddOnClickListener(bucketWheelCurrent_2, () =>
        {
            ResetLastButtonState(bucketWheelCurrent_2);
            curChartName = ChartName.BucketWheelCurrent_2;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(bucketWheelCurrent_2));
        });
        AddOnClickListener(trolleyCurrent_1, () =>
        {
            ResetLastButtonState(trolleyCurrent_1);
            curChartName = ChartName.TrolleyCurrent_1;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(trolleyCurrent_1));
        });
        AddOnClickListener(trolleyCurrent_2, () =>
        {
            ResetLastButtonState(trolleyCurrent_2);
            curChartName = ChartName.TrolleyCurrent_2;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(trolleyCurrent_2));
        });
        AddOnClickListener(slewingCurrent_1, () =>
        {
            ResetLastButtonState(slewingCurrent_1);
            curChartName = ChartName.SlewingCurrent_1;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(slewingCurrent_1));
        });
        AddOnClickListener(slewingCurrent_2, () =>
        {
            ResetLastButtonState(slewingCurrent_2);
            curChartName = ChartName.SlewingCurrent_2;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(slewingCurrent_2));
        });
        AddOnClickListener(suspendedGelCurrent_1, () =>
        {
            ResetLastButtonState(suspendedGelCurrent_1);
            curChartName = ChartName.SuspendedGelCurrent_1;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(suspendedGelCurrent_1));
        });
        AddOnClickListener(suspendedGelCurrent_2, () =>
        {
            ResetLastButtonState(suspendedGelCurrent_2);
            curChartName = ChartName.SuspendedGelCurrent_2;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(suspendedGelCurrent_2));
        });
        AddOnClickListener(cantileverCurrent_1, () =>
        {
            ResetLastButtonState(cantileverCurrent_1);
            curChartName = ChartName.CantileverCurrent_1;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(cantileverCurrent_1));
        });

        AddOnClickListener(cantileverCurrent_2, () =>
        {
            ResetLastButtonState(cantileverCurrent_2);
            curChartName = ChartName.CantileverCurrent_2;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(cantileverCurrent_2));
        });

        AddOnClickListener(allElectricity_1, (() =>
        {
            ResetLastButtonState(allElectricity_1);
            curChartName = ChartName.allElectricity_1;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(allElectricity_1));
        }));
        AddOnClickListener(allElectricity_2, (() =>
        {
            ResetLastButtonState(allElectricity_2);
            curChartName = ChartName.allElectricity_2;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(allElectricity_2));
        }));
        
        AddOnClickListener(position_1, () =>
        {
            ResetLastButtonState(position_1);
            curChartName = ChartName.Position_1;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(position_1));
        });

        AddOnClickListener(position_2, () =>
        {
            ResetLastButtonState(position_2);
            curChartName = ChartName.Position_2;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(position_2));
        });
        
        AddOnClickListener(rotationAngel_1, () =>
        {
            ResetLastButtonState(rotationAngel_1);
            curChartName = ChartName.RotationAngle_1;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(rotationAngel_1));
        });

        AddOnClickListener(rotationAngel_2, () =>
        {
            ResetLastButtonState(rotationAngel_2);
            curChartName = ChartName.RotationAngle_2;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(rotationAngel_2));
        });
        
        AddOnClickListener(pitchAngle_1, () =>
        {
            ResetLastButtonState(pitchAngle_1);
            curChartName = ChartName.PitchAngle_1;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(pitchAngle_1));
        });

        AddOnClickListener(pitchAngle_2, () =>
        {
            ResetLastButtonState(pitchAngle_2);
            curChartName = ChartName.PitchAngle_2;
            SetSearchPanelDate(curChartName);
            UpdateCurChart(nameof(pitchAngle_2));
        });

        historyBtn.onClick.AddListener((() =>
        {
            historyBtn.gameObject.SetActive(false);
            dynamicBtn.gameObject.SetActive(true);
            if (dateDic.ContainsKey(curChartName.ToString()))
            {
                dateDic[curChartName.ToString()].IsDynamic = true;
            }

            dynamicUpdateData = true;
            searchPanel.gameObject.SetActive(false);
        }));
        dynamicBtn.onClick.AddListener((() =>
        {
            historyBtn.gameObject.SetActive(true);
            dynamicBtn.gameObject.SetActive(false);
            if (dateDic.ContainsKey(curChartName.ToString()))
            {
                dateDic[curChartName.ToString()].IsDynamic = false;
            }

            searchPanel.gameObject.SetActive(true);
            dynamicUpdateData = false;
            searchPanel.searchBtn.onClick.Invoke();
        }));
        InitChart();
        InitSelectElectricityShowItem();
        lastButton = bucketWheelCurrent_1;
        historyChartData = new HistoryChartData();
        historyChartData.SetData(bucketWheelChart_1, ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC,
            Machine.BucketWheelStackerReclaimer);
        searchPanel.SetSearchAction(UpdateCurChartByTime);
        bucketWheelCurrent_1.Invoke();
    }

    private void InitDateDic()
    {
        DateTime now = DateTime.Now;
        string startDate = $"{now.Year}-{now.Month}-{now.Day}" + "-0-0";
        string endDate = $"{now.Year}-{now.Month}-{now.Day}" + "-23-59";
        dateDic.Add(ChartName.BucketWheelCurrent_1.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.BucketWheelCurrent_2.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.TrolleyCurrent_1.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.TrolleyCurrent_2.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.SlewingCurrent_1.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.SlewingCurrent_2.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.SuspendedGelCurrent_1.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.SuspendedGelCurrent_2.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.CantileverCurrent_1.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.CantileverCurrent_2.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.allElectricity_1.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.allElectricity_2.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.Position_1.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.Position_2.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.RotationAngle_1.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.RotationAngle_2.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.PitchAngle_1.ToString(), new DateCell(startDate, endDate));
        dateDic.Add(ChartName.PitchAngle_2.ToString(), new DateCell(startDate, endDate));
    }

    private void UpdateDateDic(ChartName chartName)
    {
        if (dateDic.ContainsKey(chartName.ToString()))
        {
            dateDic[chartName.ToString()].StartTime = searchPanel?.GetStartDateText();
            dateDic[chartName.ToString()].EndTime = searchPanel?.GetEndDateText();
        }
    }

    private void SetSearchPanelDate(ChartName chartName)
    {
        if (searchPanel != null)
        {
            if (dateDic.ContainsKey(chartName.ToString()))
            {
                string[] startTime = dateDic[chartName.ToString()].StartTime.Split("-");
                string[] endTime = dateDic[chartName.ToString()].EndTime.Split("-");
                if (startTime.Length >= 4 && endTime.Length >= 4)
                {
                    searchPanel.SetDateText(startTime, endTime);
                    dynamicUpdateData = dateDic[chartName.ToString()].IsDynamic;
                }

                historyBtn.gameObject.SetActive(dateDic[chartName.ToString()].IsDynamic == false);
                dynamicBtn.gameObject.SetActive(dateDic[chartName.ToString()].IsDynamic);
                searchPanel.gameObject.SetActive(dateDic[chartName.ToString()].IsDynamic == false);
            }
        }
    }

    private void OnEnable()
    {
        SetSearchPanelDate(curChartName);
        tempChartData.Clear();
        allTempChartData.Clear();
        EventManager.Instance.AddListener(EventName.UpdateChartData, DynamicUpdateData);
    }

    private void OnDisable()
    {
        dynamicUpdateData = false;
        searchPanel.Reset();
        tempChartData.Clear();
        allTempChartData.Clear();
        EventManager.Instance.RemoveListener(EventName.UpdateChartData, DynamicUpdateData);
    }

    public void InitChart()
    {
        SetLineChartParms(trolleyElectricityChart_1);
        SetLineChartParms(slewingChart_1);
        SetLineChartParms(suspensoidChart_1);
        SetLineChartParms(cantileverChart_1);
        SetLineChartParms(bucketWheelChart_1);
        SetLineChartParms(bucketWheelChart_1);
        SetLineChartParms(positionChart_1);
        SetLineChartParms(rotationAngleChart_1);
        SetLineChartParms(pitchAngleChart_1);
        // SetLineChartParms(allChart_1);

        SetLineChartParms(trolleyElectricityChart_2);
        SetLineChartParms(slewingChart_2);
        SetLineChartParms(suspensoidChart_2);
        SetLineChartParms(cantileverChart_2);
        SetLineChartParms(bucketWheelChart_2);
        SetLineChartParms(trolleyElectricityChart_2);
        SetLineChartParms(positionChart_2);
        SetLineChartParms(rotationAngleChart_2);
        SetLineChartParms(pitchAngleChart_2);
        // SetLineChartParms(allChart_2);
        SetAllChartSerie(allChart_2);
        SetAllChartSerie(allChart_1);
    }

    public void InitSelectElectricityShowItem()
    {
        selectElectricityShowItem_1.BucketWheelToggleAction = (b =>
        {
            allChart_1.series[0].show=b;
        });
        selectElectricityShowItem_1.TrolleyToggleAction = (b =>
        {
            allChart_1.series[1].show=b;
        });
        selectElectricityShowItem_1.SlewingToggleAction = (b =>
        {
            allChart_1.series[2].show=b;
        });
        selectElectricityShowItem_1.SuspendedGelToggleAction = (b =>
        {
            allChart_1.series[3].show=b;
        });
        
        selectElectricityShowItem_2.BucketWheelToggleAction = (b =>
        {
            allChart_2.series[0].show=b;
        });
        selectElectricityShowItem_2.TrolleyToggleAction = (b =>
        {
            allChart_2.series[1].show=b;
        });
        selectElectricityShowItem_2.SlewingToggleAction = (b =>
        {
            allChart_2.series[2].show=b;
        });
        selectElectricityShowItem_2.SuspendedGelToggleAction = (b =>
        {
            allChart_2.series[3].show=b;
        });
    }
    //设置所有电流名字和字体大小
    public void SetAllChartSerie(LineChart lineChart)
    {
        lineChart.RemoveData();
        lineChart.AddSerie<Line>().serieName = "斗轮电流";
        lineChart.AddSerie<Line>().serieName = "大车电流";
        lineChart.AddSerie<Line>().serieName = "回转电流";
        lineChart.AddSerie<Line>().serieName = "悬胶电流";
        lineChart.EnsureChartComponent<XAxis>().axisLabel.textStyle.fontSize = 14;
        lineChart.EnsureChartComponent<Tooltip>().titleFormatter = "{j}";
        ClearLineChartSeries(lineChart);
    }

    public void SetLineChartParms(LineChart lineChart)
    {
        lineChart.EnsureChartComponent<XAxis>().axisLabel.textStyle.fontSize = 14;
        ClearLineChartSeries(lineChart);
        SetLineChartTooltip(lineChart);
    }

    //清理所有series数据
    public void ClearLineChartSeries(LineChart lineChart)
    {
        for (int i = 0; i < lineChart.series.Count; i++)
        {
            lineChart.series[i].data.Clear();
            lineChart.series[i].AnimationEnable(false);
        }
    }

    public void SetLineChartTooltip(LineChart lineChart)
    {
        lineChart.EnsureChartComponent<Tooltip>().itemFormatter = "{c1}\n{c0}";
        lineChart.EnsureChartComponent<Tooltip>().numericFormatter = "o";
    }

    public void ResetLastButtonState(ButtonCell btn)
    {
        if (lastButton != null)
        {
            lastButton.SetSelectState(false);
        }

        lastButton = btn;
        lastButton.SetSelectState(true, false);
        // dynamicUpdateData = true;
        tempChartData.Clear();
        allTempChartData.Clear();
        searchPanel.Reset();
    }

    public void ResetLastChart(LineChart go)
    {
        if (lastChart != null)
        {
            lastChart.gameObject.SetActive(false);
        }

        lastChart = go;
        lastChart.gameObject.SetActive(true);
    }

    public void SetHistoryPanel(HistoryPanelCtr historyPanelCtr)
    {
        searchPanel.SetHistoryPanel(historyPanelCtr);
    }

    public void UpdateCurChart(string str)
    {
        selectElectricityShowItem_1.gameObject.SetActive(false);
        selectElectricityShowItem_2.gameObject.SetActive(false);
        if (str == nameof(bucketWheelCurrent_1))
        {
            ResetLastChart(bucketWheelChart_1);
            historyChartData.SetData(bucketWheelChart_1, ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC,
                Machine.BucketWheelStackerReclaimer);
            if (bucketWheelChart_1.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
                // GetSqlData(bucketWheelChart_1, ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC, Machine.BucketWheelStackerReclaimer,false,"","",true);
            }
        }
        else if (str == nameof(bucketWheelCurrent_2))
        {
            ResetLastChart(bucketWheelChart_2);
            historyChartData.SetData(bucketWheelChart_2, ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC,
                Machine.BucketWheel);
            if (bucketWheelChart_2.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
                // GetSqlData(bucketWheelChart_2, ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC, Machine.BucketWheel,false,"","",true);
            }
        }
        else if (str == nameof(trolleyCurrent_1))
        {
            ResetLastChart(trolleyElectricityChart_1);
            historyChartData.SetData(trolleyElectricityChart_1, ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC,
                Machine.BucketWheelStackerReclaimer);
            if (trolleyElectricityChart_1.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
                // GetSqlData(trolleyElectricityChart_1, ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC, Machine.BucketWheelStackerReclaimer,false,"","",true);
            }
        }
        else if (str == nameof(trolleyCurrent_2))
        {
            ResetLastChart(trolleyElectricityChart_2);
            historyChartData.SetData(trolleyElectricityChart_2, ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC,
                Machine.BucketWheel);
            if (trolleyElectricityChart_2.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
                // GetSqlData(trolleyElectricityChart_2, ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC, Machine.BucketWheel,false,"","",true);
            }
        }
        else if (str == nameof(suspendedGelCurrent_1))
        {
            ResetLastChart(suspensoidChart_1);
            historyChartData.SetData(suspensoidChart_1, ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC,
                Machine.BucketWheelStackerReclaimer);
            if (suspensoidChart_1.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
                // GetSqlData(suspensoidChart_1, ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC, Machine.BucketWheelStackerReclaimer,false,"","",true);
            }
        }
        else if (str == nameof(suspendedGelCurrent_2))
        {
            ResetLastChart(suspensoidChart_2);
            historyChartData.SetData(suspensoidChart_2, ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC,
                Machine.BucketWheel);
            if (suspensoidChart_2.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
                // GetSqlData(suspensoidChart_2, ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC, Machine.BucketWheel,false,"","",true);
            }
        }

        else if (str == nameof(cantileverCurrent_1))
        {
            ResetLastChart(cantileverChart_1);
            historyChartData.SetData(cantileverChart_1, ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC,
                Machine.BucketWheelStackerReclaimer);
            if (cantileverChart_1.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
                // GetSqlData(cantileverChart_1, ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC, Machine.BucketWheelStackerReclaimer,false,"","",true);
            }
        }
        else if (str == nameof(cantileverCurrent_2))
        {
            ResetLastChart(cantileverChart_2);
            historyChartData.SetData(cantileverChart_2, ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC,
                Machine.BucketWheel);
            if (cantileverChart_2.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
                // GetSqlData(cantileverChart_2, ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC,Machine.BucketWheel,false,"","",true);
            }
        }
        else if (str == nameof(slewingCurrent_1))
        {
          
            ResetLastChart(slewingChart_1);
            historyChartData.SetData(slewingChart_1, ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC,
                Machine.BucketWheelStackerReclaimer);
            if (slewingChart_1.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
                // GetSqlData(slewingChart_1, ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC, Machine.BucketWheelStackerReclaimer,false,"","",true);
            }
        }
        else if (str == nameof(slewingCurrent_2))
        {
            ResetLastChart(slewingChart_2);
            historyChartData.SetData(slewingChart_2, ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC, Machine.BucketWheel);
            if (slewingChart_2.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
                // GetSqlData(slewingChart_2, ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC,Machine.BucketWheel,false,"","",true);
            }
        }
        else if (str == nameof(allElectricity_1))
        {
            selectElectricityShowItem_1.gameObject.SetActive(true);
            ResetLastChart(allChart_1);
            historyChartData.SetData(allChart_1, "ALL", Machine.BucketWheelStackerReclaimer);
            searchPanel.searchBtn.onClick?.Invoke();
        }
        else if (str == nameof(allElectricity_2))
        {
            selectElectricityShowItem_2.gameObject.SetActive(true);
            ResetLastChart(allChart_2);
            historyChartData.SetData(allChart_2, "ALL", Machine.BucketWheel);
            searchPanel.searchBtn.onClick?.Invoke();
        }else if (str == nameof(position_1))
        {
            ResetLastChart(positionChart_1);
            historyChartData.SetData(positionChart_1, ConstStr.DATABASE_HISTORY_MACHINE_POSITION,
                Machine.BucketWheelStackerReclaimer);
            if (positionChart_1.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
            }
        }
        else if (str == nameof(position_2))
        {
            ResetLastChart(positionChart_2);
            historyChartData.SetData(positionChart_2, ConstStr.DATABASE_HISTORY_MACHINE_POSITION,
                Machine.BucketWheel);
            if (positionChart_2.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
            }
        }else if (str == nameof(rotationAngel_1))
        {
            ResetLastChart(rotationAngleChart_1);
            historyChartData.SetData(rotationAngleChart_1, ConstStr.DATABASE_HISTORY_MACHINE_ROTATION_ANGLE,
                Machine.BucketWheelStackerReclaimer);
            if (rotationAngleChart_1.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
            }
        }
        else if (str == nameof(rotationAngel_2))
        {
            ResetLastChart(rotationAngleChart_2);
            historyChartData.SetData(rotationAngleChart_2, ConstStr.DATABASE_HISTORY_MACHINE_ROTATION_ANGLE,
                Machine.BucketWheel);
            if (rotationAngleChart_2.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
            }
        }
        else if (str == nameof(pitchAngle_1))
        {
            ResetLastChart(pitchAngleChart_1);
            historyChartData.SetData(pitchAngleChart_1, ConstStr.DATABASE_HISTORY_MACHINE_PITCH_ANGLE,
                Machine.BucketWheelStackerReclaimer);
            if (pitchAngleChart_1.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
            }
        }
        else if (str == nameof(pitchAngle_2))
        {
            ResetLastChart(pitchAngleChart_2);
            historyChartData.SetData(pitchAngleChart_2, ConstStr.DATABASE_HISTORY_MACHINE_PITCH_ANGLE,
                Machine.BucketWheel);
            if (pitchAngleChart_2.series[0].data.Count <= 0)
            {
                searchPanel.searchBtn.onClick?.Invoke();
            }
        }
    }

    public async void GetSqlData(LineChart lineChart, string chartName, Machine machine, bool isUseTime = false,
        string startTime = "", string endTime = "", bool isLimit = false, int limit = 1000, int serieIndex = 0)
    {
        if (isLimit == false)
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

        // dynamicUpdateData = false;
        if (dataSet != null)
        {
            lineChart.series[serieIndex].data.Clear();
        }

        if (dataSet == null || dataSet.Tables.Count <= 0 || dataSet.Tables[0].Rows.Count <= 0)
        {
            return;
        }

        UpdateDateDic(curChartName);
        int addNum = dataSet.Tables[0].Rows.Count / 1000;
        addNum = addNum == 0 ? 1 : addNum;
        DataRowCollection dataRowCollection = dataSet.Tables[0].Rows;
        for (int i = dataRowCollection.Count - 1; i >= 0; i -= addNum)
        {
            lineChart.AddData(serieIndex,
                DateTime.Parse(dataRowCollection[i][ConstStr.DATA_HISTORY_CARTELECTRICITY_TIME].ToString()),
                float.Parse(dataRowCollection[i][ConstStr.DATA_HISTORY_CARTELECTRICITY_VALUE].ToString()));
        }
    }

    public void UpdateCurChartByTime(string startTime, string endTime, MechanicalType mechanicalType, string other)
    {
        if (lastChart == null || historyChartData == null)
        {
            return;
        }

        if (historyChartData.TableName == "ALL") //选中电流展示数据处理
        {
            Debug.Log($">>>>>>>>>>>>{startTime}  {endTime} {mechanicalType}");
            for (int i = 0; i < historyChartData.linechart.series.Count; i++)
            {
                GetSqlData(historyChartData.linechart, ElectricityTableNameList[i], historyChartData.Machine, true,
                    startTime,
                    endTime, false, 0, i);
            }
        }
        else
        {
            GetSqlData(historyChartData.linechart, historyChartData.TableName, historyChartData.Machine, true,
                startTime,
                endTime);
        }
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
        if (lastChart == null)
        {
            return;
        }
        lastChart.ClearData();
        tempChartData.Enqueue(new CData(DateTime.Now, value));
        if (tempChartData.Count > 200)
        {
            tempChartData.Dequeue();
        }

        foreach (var data in tempChartData)
        {
            double num = Math.Floor(data.Value * 100) / 100;
            lastChart.AddData(0, data.date,num);
        }
    }
    private void UpdateChartData(float value,int serieIndex=0)
    {
        if (lastChart == null)
        {
            return;
        }
        lastChart.series[serieIndex].ClearData();
        if (!allTempChartData.ContainsKey(serieIndex))
        {
            allTempChartData.Add(serieIndex,new Queue<CData>());
        }
        allTempChartData[serieIndex].Enqueue(new CData(DateTime.Now, value));
        if (allTempChartData[serieIndex].Count > 200)
        {
            allTempChartData[serieIndex].Dequeue();
        }
        
        foreach (var data in allTempChartData[serieIndex])
        {
            double num = Math.Floor(data.Value * 100) / 100;
            lastChart.AddData(serieIndex, data.date,num );
        }
    }
    public void DynamicUpdateData(object sender, EventArgs e)
    {
        if (dynamicUpdateData && GameDataManager.Instance.SystemVariables != null)
        {
            if (curChartName == ChartName.BucketWheelCurrent_1)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.BucketWheelElectricCurrent;
            }
            else if (curChartName == ChartName.BucketWheelCurrent_2)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.BucketWheelElectricCurrent_2;
            }
            else if (curChartName == ChartName.TrolleyCurrent_1)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.LargeCarElectricCurrent;
            }
            else if (curChartName == ChartName.TrolleyCurrent_2)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.LargeCarElectricCurrent_2;
            }
            else if (curChartName == ChartName.SlewingCurrent_1)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.RotaryElectricCurrent;
            }
            else if (curChartName == ChartName.SlewingCurrent_2)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.RotaryElectricCurrent_2;
            }
            else if (curChartName == ChartName.SuspendedGelCurrent_1)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.SuspensionBeltElectricCurrent;
            }
            else if (curChartName == ChartName.SuspendedGelCurrent_2)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.SuspensionBeltElectricCurrent_2;
            }
            else if (curChartName == ChartName.CantileverCurrent_1)
            {
                // tempChartValue = GameDataManager.Instance.SystemVariables.SuspensionBeltElectricCurrent_2;
                FlowMeter_data data = GameDataManager.Instance.GetFlowMeterData(Machine.BucketWheelStackerReclaimer);
                tempChartValue = data == null ? 0 : (float)data.FlowRealtime;
            }
            else if (curChartName == ChartName.CantileverCurrent_2)
            {
                FlowMeter_data data = GameDataManager.Instance.GetFlowMeterData(Machine.BucketWheel);
                tempChartValue = data == null ? 0 : (float)data.FlowRealtime;
            }else if (curChartName== ChartName.allElectricity_1)
            {
                UpdateChartData(GameDataManager.Instance.SystemVariables.BucketWheelElectricCurrent, 0);
                UpdateChartData(GameDataManager.Instance.SystemVariables.LargeCarElectricCurrent, 1);
                UpdateChartData(GameDataManager.Instance.SystemVariables.RotaryElectricCurrent, 2);
                UpdateChartData(GameDataManager.Instance.SystemVariables.SuspensionBeltElectricCurrent, 3);
               return;
            }else if (curChartName== ChartName.allElectricity_2)
            {
                UpdateChartData(GameDataManager.Instance.SystemVariables.BucketWheelElectricCurrent_2, 0);
                UpdateChartData(GameDataManager.Instance.SystemVariables.LargeCarElectricCurrent_2, 1);
                UpdateChartData(GameDataManager.Instance.SystemVariables.RotaryElectricCurrent_2, 2);
                UpdateChartData(GameDataManager.Instance.SystemVariables.SuspensionBeltElectricCurrent_2, 3);
                return;
            } else if (curChartName == ChartName.Position_1)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.DC_Pos;
            }
            else if (curChartName == ChartName.Position_2)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.DC_Pos_2;
            }else if (curChartName == ChartName.RotationAngle_1)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.SLEW_Angle;
            }
            else if (curChartName == ChartName.RotationAngle_2)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.SLEW_Angle_2;
            }else if (curChartName == ChartName.PitchAngle_1)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.Luff_Angle;
            }
            else if (curChartName == ChartName.PitchAngle_2)
            {
                tempChartValue = GameDataManager.Instance.SystemVariables.Luff_Angle_2;
            }

            UpdateChartData(tempChartValue);
        }
    }
}