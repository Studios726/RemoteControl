using System.Collections.Generic;
using RemoteControl.Event;
using UnityEngine;


public enum MessageType
{
    Plc=1000,
}

public class MessageCenter : Singleton<MessageCenter>
{

    // 消息委托
    public delegate void messageDelHandle(string message);
    // 消息字典
    private Dictionary<int, messageDelHandle> messageMap = new Dictionary<int, messageDelHandle>();

    /// <summary>
    /// 注册监听
    /// </summary>
    public void RegisterListener(int messageType, messageDelHandle handle)
    {
        if (handle == null) return;
 
        // 把事件添加到对应的委托中
        messageDelHandle myHandle = null;
        messageMap.TryGetValue(messageType, out myHandle);
        messageMap[messageType] = myHandle + handle;

    }
    /// <summary>
    /// 移除监听
    /// </summary>
    public void RemoveListener(int messageType, messageDelHandle handle)
    {
        if (handle == null) return;
        messageMap[messageType] =messageMap[messageType]-handle;
    }
 
    public void RegisterListener(MessageType messageType, messageDelHandle handle) {
        RegisterListener((int)messageType, handle);
    }
    public void RemoveListener(MessageType messageType, messageDelHandle handle)
    {
        RemoveListener((int) messageType, handle);
    }
    /// <summary>
    /// 清空消息
    /// </summary>
    public void Clear()
    {
        messageMap.Clear();
    }

    public void ReadJson(string json, SocketType type)
    {
        // if (type==SocketType.Socket1)
        // {
        //     SystemVariables systemVariables = JsonMgr.DeSerialize<SystemVariables>(json);
        // }
        EventManager.Instance.TriggerEvent(EventName.Message,this,new MessageEventArgs(json,type));
    }
    public void SendMessage<T>(int messageType, T body)
    {
 
        messageDelHandle handle;
        if (messageMap.TryGetValue(messageType, out handle))
        {
            string json = JsonMgr.Serialize<T>(body);
            try
            {
                if (handle != null)
                {
                    handle(json);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("发送失败");
            }
        }
 
    }
}
