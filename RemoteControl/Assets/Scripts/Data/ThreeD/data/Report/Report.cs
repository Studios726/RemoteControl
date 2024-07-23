using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShangHaiPro
{
    public class Report
    {
        public int coalHouseID;
        public float coalHouseLength;
        public float coalHouseWidth;
        public float coalHouseArea;
        public double coalLeftVolume;
        public double coalLeftWeight;

        public double coalRightVolume;
        public double coalRightWeight;

        public double coalToTalVolume;
        public double coalTotalWeight;

        public string measureTimeStr;
        public string measureName;
        public string checkName;
        public int pcNumber;

        //仅用于生成区域Region报表
        public int RegionCounts;
        public RegionReport[] RegionReport_LIST;

        public Report()
        {
            coalHouseID = 1;
            coalHouseLength = 300;//长度m
            coalHouseWidth = 100;
            coalHouseArea = 30000;//面积m*m
            coalLeftVolume = 12000;
            coalLeftWeight = 12000;
            coalRightVolume = 12000;
            coalRightWeight = 12000;
            coalToTalVolume = 24000;
            coalTotalWeight = 24000;
            measureTimeStr = "20240408";
            measureName = "admin";
            checkName = "user";
            pcNumber = 66666;

            RegionCounts = 6;
            RegionReport_LIST = new RegionReport[RegionCounts];
            for (int i = 0; i < RegionCounts; i++)
            {
                RegionReport_LIST[i] = new RegionReport();
                RegionReport_LIST[i].id = i;
                RegionReport_LIST[i].side = 1;
                RegionReport_LIST[i].coalName = "Place 1";
                RegionReport_LIST[i].densityValue = 1100;  // kg/m3
                RegionReport_LIST[i].volume = 5000;          // m3
                RegionReport_LIST[i].weight = 5500;           //t
            }

        }
    }

}

