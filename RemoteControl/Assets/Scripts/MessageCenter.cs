using System;
using System.Collections.Generic;
using UnityEngine;
using RemoteControl.Event;
using ShenYangRemoteSystem.Subclass;
using UnityEngine.Networking.PlayerConnection;
using System.IO;
using System.Text;
using System.IO.Compression;

public enum MessageType
{
    RC, // 由于是示例，这里省略了枚举值的赋值。
    PC,
}

public class MessageCenter : Singleton<MessageCenter>
{
    public delegate void MessageDelHandle(string message);

    private Dictionary<int, List<MessageDelHandle>> messageMap = new Dictionary<int, List<MessageDelHandle>>();

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
            try
            {
                string json = Decompress(message);
                SystemVariables systemVariables = JsonMgr.DeSerialize<SystemVariables>(json);
                Debug.Log($"数据解析成功 socketType {nameof(SocketType.TaoRC)}");
                GameDataManager.Instance.SetSystemVariables(systemVariables);
            }
            catch (Exception)
            {

                Debug.LogError($"数据解析失败 socketType {nameof(SocketType.TaoRC)}");
            }
           
        }else if(socketType== SocketType.TaskPC)
        {
            Debug.Log($"收到数据 {SocketType.TaoRC} {message}");
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
                //Debug.Log($"json  {json}");
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
                Debug.LogError($"发送失败: {e.Message}");
            }
        }
    }
    #region 压缩方法
    //解压字符串
    public static string Decompress(string compressedText)
    {
        byte[] compressedBuffer = Convert.FromBase64String(compressedText); using (MemoryStream compressedStream = new MemoryStream(compressedBuffer))
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
