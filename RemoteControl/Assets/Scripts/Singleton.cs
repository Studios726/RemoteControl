using System;
using System.Reflection;
 
/// <summary>
/// 单例
/// 继承需要一个非公有的无参构造函数
/// </summary>
/// <typeparam name="T">类名的泛型</typeparam>
public abstract class Singleton<T> where T : new()
{
    private static T instance;
 
    // 多线程安全机制
    private static readonly object locker = new object();
    
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                //lock写第一个if里是因为只有该类的实例还没创建时，才需要加锁，这样可以节省性能
                lock (locker)
                {
                    if (instance == null)
                        instance = new T();
                }
            }
            return instance;
        }
    }
}