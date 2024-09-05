using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShangHaiPro
{
    public class SystemCommand
    {
        public string QUERY_SYSTEM { get; set; }
        //发起命令的系统名称  MC郭  Pc茂
        public int DATA_TYPE { get; set; }
        //数据类型：1-流量，2-运动参数，3-三维料堆 SCAN，4-安全信息，5-任务规划信息，6-远程驱动系统交互 

        public int QUERY_TYPE { get; set; }
        //查询类型： 30 更新dem

        public string COMMAND_NAME { get; set; }
        //三维扫描无意义

        public int COMMAND_TYPE { get; set; }
        //三维扫描无意义

        public SendAllData SendAllData { get; set; }
        //传输的数据大类

        //陶client
        public string DATA_STRING { get; set; }
        //数据字符串：用于存放向其他系统转发的内容（暂无实现）
        public int DATA_INT { get; set; }
        //数据整型：0-False，1-True
        public float DATA_FLOAT { get; set; }
        //数据浮点型：度数，深度
    }
}
