namespace ShangHaiPro
{
    public class MP_Status_Single
    {
        public string MP_InSpace;//煤场E F
        public int MP_ID;//1~8
        public MP_Status MP_Lidar_Status;
        public int MP_Lider_Once_PointNums;//雷达单次扫描点数
        public MP_Status MP_Platform_Status;
        public double MP_Platform_Rotation_Angle;//云台回转角度

        public bool MP_Status_Isnormal;
    }
}