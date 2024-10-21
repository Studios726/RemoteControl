using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;
using Random = UnityEngine.Random;

public class TestCharts : MonoBehaviour
{ public LineChart CurLineChart;

    private void Start()
    {
        CurLineChart.EnsureChartComponent<XAxis>().axisLabel.textStyle.fontSize = 14;
        CurLineChart.series[0].data.Clear();
        for (int i = 0; i < 100; i++)
        {
            CurLineChart.AddData(0, new DateTime(2024,1,15,15,i,0), Random.Range(1, 30));
        }
    }
}
