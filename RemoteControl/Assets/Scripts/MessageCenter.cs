using System;
using System.Collections.Generic;
using System.Diagnostics;
using RemoteControl.Event;
using ShenYangRemoteSystem.Subclass;
using UnityEngine.Networking.PlayerConnection;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Threading;
using Newtonsoft.Json;
using ShangHaiPro;
using Debug = UnityEngine.Debug;

public enum MessageType
{
    RC, // 由于是示例，这里省略了枚举值的赋值。
    PC,
    SCA,
    FM
}

public class MessageCenter : Singleton<MessageCenter>
{
    public delegate void MessageDelHandle(string message);

    private Dictionary<int, List<MessageDelHandle>> messageMap = new Dictionary<int, List<MessageDelHandle>>();
    private static object TaskObj = new object();
    private static object RcObj = new object();
    public void RegisterListener(int messageType, MessageDelHandle handle)
    {
        if (handle == null) return;

        if (!messageMap.ContainsKey(messageType))
        {
            messageMap[messageType] = new List<MessageDelHandle>();
        }

        messageMap[messageType].Add(handle);
    }

    public void RemoveListener(int messageType, MessageDelHandle handle)
    {
        if (handle == null || !messageMap.ContainsKey(messageType)) return;

        List<MessageDelHandle> handlers = messageMap[messageType];
        if (handlers.Contains(handle))
        {
            handlers.Remove(handle);
            // 如果移除后列表为空，考虑从messageMap中移除该键
            if (handlers.Count == 0)
            {
                messageMap.Remove(messageType);
            }
        }
    }

    public void RegisterListener(MessageType messageType, MessageDelHandle handle)
    {
        RegisterListener((int)messageType, handle);
    }

    public void RemoveListener(MessageType messageType, MessageDelHandle handle)
    {
        RemoveListener((int)messageType, handle);
    }

    public void Clear()
    {
        messageMap.Clear();
    }

    public void ReadJson(string message, SocketType socketType)
    {
        // 此处省略具体实现，根据type来处理不同的json数据
        if (message == null)
            return;
        if(socketType== SocketType.TaoRC)
        {
            lock (RcObj)
            {
                try
                {
                    string json = Decompress(message);
                    SystemVariables systemVariables = JsonMgr.DeSerialize<SystemVariables>(json);
                    GameDataManager.Instance.SetSystemVariables(systemVariables);
                }
                catch (Exception e)
                {
                    Debug.Log($"数据解析失败 socketType {socketType} message {message} error {e.Message}");
                    // 创建栈跟踪对象
                    StackTrace stackTrace = new StackTrace(e, true);
                    foreach (var frame in stackTrace.GetFrames())
                    {
                        Debug.LogError($"Method: {frame.GetMethod().Name}, Line: {frame.GetFileLineNumber()}>>>");
                    }
                }
            }
        }else if(socketType== SocketType.TaskPC)
        {
            lock (TaskObj)
            {
                try
                {
                    // TaskDataManager.Instance.TaskMessageList.Add(message);
                    TaskVariables taskVariables = JsonMgr.DeSerialize<TaskVariables>(message);
                    TaskDataManager.Instance.SetTaskVariables(taskVariables);
                }
                catch (Exception e)
                {
                    TaskDataManager.Instance.UpdateTaskData(4);
                    Debug.Log($"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")} 数据解析失败 socketType {socketType} message {message} error {e.Message}");
                    // 创建栈跟踪对象
                    StackTrace stackTrace = new StackTrace(e, true);
                    foreach (var frame in stackTrace.GetFrames())
                    {
                        Debug.LogError($"Method: {frame.GetMethod().Name}, Line: {frame.GetFileLineNumber()}>>>");
                    }
                }
            }
           
        }else if (socketType == SocketType.SCA)
        {
            try
            {
                // Debug.LogError(">>>>>>>>>>..模型更新");
                string json = Decompress(message);
                GameDataManager.Instance.DeSerializeScaJson(json);
            }
            catch (Exception e)
            {
                Debug.Log($"数据解析失败 socketType {socketType} message {message} error {e.Message}");
                // 创建栈跟踪对象
                StackTrace stackTrace = new StackTrace(e, true);
                foreach (var frame in stackTrace.GetFrames())
                {
                    Debug.LogError($"Method: {frame.GetMethod().Name}, Line: {frame.GetFileLineNumber()}>>>");
                }
            }
        }else if (socketType == SocketType.FM)
        {
            try
            {
                string json =message;
                List<FlowMeter_data> data= JsonMgr.DeSerialize<List<FlowMeter_data>>(json);
                GameDataManager.Instance.SetFlowMeterData(data);
            } 
            catch (Exception e)
            {
                Debug.LogError($"数据解析失败 socketType {socketType} {e.Message}");
            }
        }
        EventManager.Instance.TriggerEvent(EventName.Message, this, new MessageEventArgs(message, socketType));
    }

    public void SendMessage<T>(MessageType messageType, T body)
    {
        List<MessageDelHandle> handlers;
        if (messageMap.TryGetValue((int)messageType, out handlers))
        {
            string json;
            try
            {
                json = JsonMgr.Serialize<T>(body);
            }
            catch (Exception e)
            {
                Debug.LogError($"序列化失败: {e.Message}");
                return; // 序列化失败，直接返回不再发送
            }

            try
            {
                foreach (var handle in handlers)
                {
                    if (handle != null)
                    {
                        handle(json);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"{messageType}发送失败: {e.Message}");
            }
        }
    }
    #region 压缩方法
    //解压字符串
    public static string Decompress(string compressedText)
    {
        byte[] compressedBuffer = Convert.FromBase64String(compressedText);
        using (MemoryStream compressedStream = new MemoryStream(compressedBuffer))
        {
            using (GZipStream gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            {
                using (MemoryStream resultStream = new MemoryStream())
                {
                    gzipStream.CopyTo(resultStream);
                    return Encoding.UTF8.GetString(resultStream.ToArray());
                }
            }
        }
    }
    //压缩字符串
    public static string Compress(string text)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(text);
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
            {
                gzipStream.Write(buffer, 0, buffer.Length);
            }
            return Convert.ToBase64String(memoryStream.ToArray());
        }
    }

    #endregion
}
