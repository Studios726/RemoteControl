using System;

namespace ShangHaiPro
{
    public class Report_Data
    {
        public string NAME;//分区名字
        public string COAL_TYPE;//种类
        public int SIDE;//相对位置 左侧为1，右侧为0

        public float BEGIN;//起始位置 
        public float END;//结束位置

        public float FLOOR_SPACE;//占地面积
        public float VOLUME;//体积
        public float DENSITY;//密度
        public float WEIGHT;//质量

        public DateTime Time;//时间
    }
}
