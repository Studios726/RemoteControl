namespace ShangHaiPro
{
    public class SendAllData
    {
        //发送 3 3时 的状态数据类
        public MP_Status_List STATUS_DATA_LIST { get; set; }

        //发送 3 4时候 的高层数据类
        public SendDataReportAndDEM DEM_DATA { get; set; }

        //发送 3 5时 的截图数据
        public byte[] SCREENSHOT_IMG { get; set; }

        //发送 3 7时候 的报表名字
        public string REPORT_FILE_NAME { get; set; }

        //发送 3 7时候 的报表数据类
        public Send_Report_Data REPORT_DATA { get; set; }

        //发送 3 8时候 的相机图像
        public byte[] MONITOR_IMG { get; set; }

        //发送 3 10时候 的相机图像
        public string IsAutoType { get; set; }

        //发送 主动传输时候的标识位
        public string ICON { get; set; }
    }
}
