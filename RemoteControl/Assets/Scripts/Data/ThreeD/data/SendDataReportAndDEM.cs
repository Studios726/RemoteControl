using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShangHaiPro
{
    public class SendDataReportAndDEM
    {
        public CoalHeapDEM SendCoalHeapDEM;
        public Report SendReport;
        public int code; //code  0 表示模型为初始化模型暂未更新 1表示模型为更新模型 2壁上雷达距离
        public BucketLidarDis[] bucketLidarDisList;

        public SendDataReportAndDEM()
        {
            SendCoalHeapDEM = new CoalHeapDEM();

            SendReport = new Report();
            bucketLidarDisList = new BucketLidarDis[2];
        }
    }
    public class FlowMeter_data
    {
        public int id { get; set; }
        public string equipment_name { get; set; }
        public DateTime start_time { get; set; }//只在单次有用
        public DateTime end_time { get; set; }  //只在单次有用
        public double FlowRealtime { get; set; }  //实时流量  吨每小时
        public double Once_piling_weight { get; set; }//单次
        public double Once_extra_weight { get; set; }//单次
        public double Oneday_piling_weight { get; set; }
        public double Oneday_extra_weight { get; set; }
        public double Month_piling_weight { get; set; }
        public double Month_extra_weight { get; set; }
        public DateTime date { get; set; }
    }
    public class BucketLidarDis
    {
        public string BucketName;
        public int LidarPlace; //0是左 1是右
        public double Dis;
        public double MinDisMean;

        public BucketLidarDis()
        {
            BucketName = "堆取料机";
            LidarPlace = 0;
            Dis = 0.5;
        }

    }
    public class FlowMeterData
    {
        public FlowMeter_data[] curFlowMeter_data;
    }
}