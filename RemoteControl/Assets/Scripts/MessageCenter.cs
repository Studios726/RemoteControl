using System;
using System.Collections.Generic;
using UnityEngine;
using RemoteControl.Event;

public enum MessageType
{
    Plc // 由于是示例，这里省略了枚举值的赋值。
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

    public void ReadJson(string json, SocketType type)
    {
        // 此处省略具体实现，根据type来处理不同的json数据
        EventManager.Instance.TriggerEvent(EventName.Message, this, new MessageEventArgs(json, type));
    }

    public void SendMessage<T>(int messageType, T body)
    {
        List<MessageDelHandle> handlers;
        if (messageMap.TryGetValue(messageType, out handlers))
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
                Debug.LogError($"发送失败: {e.Message}");
            }
        }
    }
}
