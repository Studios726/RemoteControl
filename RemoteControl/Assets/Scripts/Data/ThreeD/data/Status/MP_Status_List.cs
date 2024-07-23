//测量点状态类，发送的是这个

namespace ShangHaiPro
{
    public class MP_Status_List
    {
        public int MP_Numbers;
        public int MP_AreaNums;//分区个数
        public int MP_AllPointNums;//总点数
        public float PlatformHorizontalAngle;
        public MP_Status_Single[] MPStatusList;

        public MP_Status_List()
        {
            MP_Numbers = 8;
            MP_AllPointNums = 0;
            PlatformHorizontalAngle = 0f;
            MP_AreaNums = 6;
            MPStatusList = new MP_Status_Single[8];
            for (int i = 0; i < MP_Numbers; i++)
            {
                MPStatusList[i] = new MP_Status_Single();
                MPStatusList[i].MP_InSpace = "E";
                MPStatusList[i].MP_ID = i + 1;
                MPStatusList[i].MP_Lidar_Status = MP_Status.LIDAR_NOT_CONNECTED;
                MPStatusList[i].MP_Lider_Once_PointNums = 0;
                MPStatusList[i].MP_Platform_Status = MP_Status.PLATFORM_NOT_CONNECTED;
                MPStatusList[i].MP_Platform_Rotation_Angle = 0.0;
                MPStatusList[i].MP_Status_Isnormal = false;
            }
        }

    }
}