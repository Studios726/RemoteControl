using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShangHaiPro
{
    public class SystemCommand
    {
        public string QUERY_SYSTEM { get; set; }
        //发起命令的系统名称
        public int DATA_TYPE { get; set; }
        //数据类型：1-流量，2-运动参数，3-三维料堆，4-安全信息，5-任务规划信息，6-远程驱动系统交互
        public int QUERY_TYPE { get; set; }
        //查询类型：1-三维料堆，2-系统运行状态，3-三维料堆和系统运行状态，4-立即执行扫描，生成新的料堆模型，5-激活功能，6-停止功能，其他数字-三维料堆

        public string COMMAND_NAME { get; set; }
        //三维扫描无意义
        public int COMMAND_TYPE { get; set; }
        //三维扫描无意义

        public SendAllData SendAllData { get; set; }
        //传输的数据大类
    }
}
