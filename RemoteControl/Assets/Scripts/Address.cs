using System.Net;

public static class Address
{

    public static string serviceIP = "192.168.13.60";//本地 数据库
    //public static string serviceIP = "localhost";//本地 数据库
    public static string serviceTaoIP ="192.168.13.60:11000";//本地 远程驱动 ;"192.168.13.57:11000"  "192.168.1.101:11000";
    public static string serviceYuanIP = "192.168.13.60:12000";// "192.168.1.5";//本地 三维扫描
    public static string serviceTaskIP ="192.168.13.60:12500";// "192.168.1.109:12500";//任务
    public static string serviceFmIP = "192.168.13.60:13000";
    public static string taoUrl = "ws://" + serviceTaoIP;
    public static string taskUrl = "ws://" + serviceTaskIP;
    public static string yuanUrl = "ws://" + serviceYuanIP;
}
