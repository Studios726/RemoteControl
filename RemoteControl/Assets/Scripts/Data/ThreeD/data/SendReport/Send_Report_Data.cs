using System;
using System.Collections.Generic;

namespace ShangHaiPro
{
    public class Send_Report_Data
    {
        public string fileName;//报表文件名字
        public int fileType;//报表文件类型 0是分开统计分区 1是合并统计分区
        public DateTime Time;//生成报表时间

        public string reportName;//报表标题名字
        public List<Report_Data> reportDataList;//数据列表

        public float totalVolume;//总体积
        public float totalWeight;//总重量

        public List<string> peopleList;//参与人员
        public string supervisionAuditDepartment;//监审部
        public string financeDepartment;//财务部
        public string operationDepartment;//运行部
        public string operationDepartment2;//运行2部
        public string maintenanceDepartment;//检修部
        public string combustionPipesDepartment;//燃管部
        public List<string> reviewersList;//审核人员

        public string remark;//备注
    }
}
