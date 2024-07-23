using System;

namespace ShangHaiPro
{
    public class CoalHeapDEM
    {
        public string QUERY_SYSTEM;
        public int DATA_TYPE;
        public int QUERY_TYPE;

        public DateTime TIME;
        public string COALHOUSE_NO;
        public float LENGTH;//煤场长度，米
        public float WIDTH;  //煤场宽度
        public float TRACK_WIDTH; //煤场轨道宽

        public float VOLUME_TOTAL; //m3
        public float WEIGHT_TOTAL; //t

        public int REGION_NUM;
        public int LEFT_REGION_NUM;
        public int RIGHT_REGION_NUM;
        public REGION[] REGION_LIST;
        /// <summary>
        /// X0 初始
        /// </summary>
        public float X0; // m
        /// <summary>
        /// Z0 间距
        /// </summary>
        public float Z0; // m
        /// <summary>
        /// X 间距
        /// </summary>
        public float DX; // m
        /// <summary>
        /// z 间距
        /// </summary>
        public float DZ; // m
        /// <summary>
        /// X0  数量
        /// </summary>
        public int NX;
        /// <summary>
        /// z 数量
        /// </summary>
        public int NZ;

        public float[,] DEM;  // m

        public int SYS_STATUS; //三维扫描子系统的工作状态，-2-内存不足，-1-CPU占用太大，0-功能未激活，1-正常

        public CoalHeapDEM()
        {

            QUERY_SYSTEM = "MC";
            DATA_TYPE = 3;
            QUERY_TYPE = 3;

            TIME = DateTime.Now;
            COALHOUSE_NO = "HT-Circular-001";

            LENGTH = 300f;
            WIDTH = 128f;
            TRACK_WIDTH = 20f;

            VOLUME_TOTAL = 110000;
            WEIGHT_TOTAL = 120000;

            X0 = 0;
            Z0 = 0;
            DX = 0.6f;
            DZ = 0.6f;
            NX = 169;
            NZ = 502;


            REGION_NUM = 6;
            LEFT_REGION_NUM = 3;
            RIGHT_REGION_NUM = 3;
            REGION_LIST = new REGION[REGION_NUM];
            float REG_INTERVAL = 300.0f / LEFT_REGION_NUM;
            //列表的右侧区域，再存的左侧区域
            for (int j = 0; j < 2; j++)
                for (int i = 0; i < LEFT_REGION_NUM; i++)
                {
                    REGION_LIST[i + j * LEFT_REGION_NUM] = new REGION();
                    REGION_LIST[i + j * LEFT_REGION_NUM].SIDE = j;
                    REGION_LIST[i + j * LEFT_REGION_NUM].REG_ID = i;
                    REGION_LIST[i + j * LEFT_REGION_NUM].BEGIN = i * REG_INTERVAL;
                    REGION_LIST[i + j * LEFT_REGION_NUM].END = (i + 1) * REG_INTERVAL;
                    REGION_LIST[i + j * LEFT_REGION_NUM].COAL_TYPE = i.ToString();
                    REGION_LIST[i + j * LEFT_REGION_NUM].DENSITY = 1100;   // kg/m3
                    REGION_LIST[i + j * LEFT_REGION_NUM].VOLUME = 5000;    // m3
                    REGION_LIST[i + j * LEFT_REGION_NUM].WEIGHT = 5500;      //t
                }

            DEM = new float[NZ, NX];

            for (int row = 0; row < NZ; row++)
                for (int col = 0; col < NX; col++)

                    DEM[row, col] = -10;

            SYS_STATUS = 1;

        }

        
    }

}