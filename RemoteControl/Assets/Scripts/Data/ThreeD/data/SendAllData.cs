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

        //发送 3 7 的报表文件名
        public string REPORT_FILE_NAME { get; set; }

        //发送 3 7 的报表数据类
        public Send_Report_Data REPORT_DATA { get; set; }

        //发送 3 8 的监控截图数据类
        public byte[] MONITOR_IMG { get; set; }

        //接收扫描类型 在发送3的时候 获取
        public string IsAutoType { get; set; }

        //主动传输时候的标识位
        public string ICON { get; set; }
        
        

    }
}
