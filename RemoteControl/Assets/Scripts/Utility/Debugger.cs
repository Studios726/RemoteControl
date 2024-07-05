using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger
{
    public static void Log(string log)
    {
#if UNITY_EDITOR
        Debug.Log(log);  
#endif
    }
    public static void LogError(string log)
    {
#if UNITY_EDITOR
        Debug.LogError(log);
#endif
    }
    public static void LogWarning(string log)
    {
#if UNITY_EDITOR
        Debug.LogWarning(log);
#endif
    }
}
