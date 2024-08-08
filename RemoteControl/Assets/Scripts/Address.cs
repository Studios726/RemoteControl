using System.Net;

public static class Address
{

    public static string serviceIP = "192.168.1.8";//本地 数据库
    //public static string serviceIP = "localhost";//本地 数据库
    public static string serviceTaoIP = "192.168.1.101:11000";//本地 远程驱动
    public static string serviceYuanIP = "192.168.1.120:12000";// "192.168.1.5";//本地 三维扫描
    public static string serviceTaskIP = "192.168.1.109:12500";//任务

    public static string taoUrl = "ws://" + serviceTaoIP;
    public static string taskUrl = "ws://" + serviceTaskIP;
    public static string yuanUrl = "ws://" + serviceYuanIP;
}
