namespace ShangHaiPro
{
    public class Layer
    {
        public int layerID;
        public float hBEGIN; //m
        public float hEND;    //m
        public string COAL_TYPE;
        public float DENSITY;    // kg/m3
        public float VOLUME;    // m3
        public float WEIGHT;      //t
        public int IsUse; //是否启用 1启用
        public float ColorR; //该区域颜色
        public float ColorG;
        public float ColorB;

    }
    public class REGION
    {
        public string Name;
        public int SIDE;//左侧为1，右侧为0
        public int REG_ID;
        public float BEGIN; //距离m
        public float END;    //距离m
        public string COAL_TYPE;
        public float DENSITY;    // 密度 kg/m3
        public float VOLUME;    // 体积 m3
        public float WEIGHT;      //质量 t
        public int IsUse; //是否启用 1启用 0 不启用
        public float ColorR;
        public float ColorG;
        public float ColorB;
        public int layerNumber;
        public Layer[] layerArray;

    }
}