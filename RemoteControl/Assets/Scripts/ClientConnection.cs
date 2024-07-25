using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.WebSocket;
using RemoteControl.Event;
using UnityEngine;

public enum SocketType
{
    Socket1=0,
    Socket2=1,
}
public class ClientConnection:MonoBehaviour
{
    public string address;
    public SocketType socketType;
    WebSocket webSocket;
    private bool lockReconnect = false;
    private Coroutine _pingCor, _clientPing, _serverPing;
    public bool isConnect;
    public void Init(string _address,SocketType type)
    {
        socketType = type;
        address = _address;
        CreateWebSocket();
    }
    
    void CreateWebSocket()
    {
        try
        {
            webSocket = new WebSocket(new Uri(address));
// #if !UNITY_WEBGL
//             webSocket.StartPingThread = true;
// #endif
            InitHandle();
            webSocket.Open();
        }
        catch (Exception e)
        {
            Debug.Log("websocket连接异常:" + e.Message);
            ReConnect();
        }
 
    }
 
    private void Update()
    {
        // Debug.Log(lockReconnect);
    }
 
    void InitHandle()
    {
        RemoveHandle();
        webSocket.OnOpen += OnOpen;
        webSocket.OnMessage += OnMessageReceived;
        webSocket.OnClosed += OnClosed;
        webSocket.OnError += OnError;
    }
 
    void RemoveHandle()
    {
        webSocket.OnOpen -= OnOpen;
        webSocket.OnMessage -= OnMessageReceived;
        webSocket.OnClosed -= OnClosed;
        webSocket.OnError -= OnError;
    }
 
    void OnOpen(WebSocket ws)
    {
        isConnect = true;
        EventManager.Instance.TriggerEvent(EventName.ConnectionSuccess,this,new ConnectEventArgs(socketType));
        // Debug.Log("websocket连接成功");
        // if (_pingCor != null)
        // {
        //     StopCoroutine(_pingCor);
        //     _pingCor = null;
        // }
        // _pingCor = StartCoroutine(HeartPing());
        // // 心跳检测重置
        // HeartCheck();
    }
 
    void OnMessageReceived(WebSocket ws, string message)
    {
        // 如果获取到消息，心跳检测重置
        // 拿到任何消息都说明当前连接是正常的
        MessageCenter.Instance.ReadJson(message,socketType);
        // HeartCheck();
        Debug.Log(message);
    }
 
    void OnClosed(WebSocket ws, UInt16 code, string message)
    {
        isConnect = false;
        Debug.LogFormat("OnClosed: code={0}, msg={1}", code, message);
        webSocket = null;
        ReConnect();
    }
 
    void OnError(WebSocket ws, string ex)
    {
        string errorMsg = string.Empty;
#if !UNITY_WEBGL || UNITY_EDITOR
        if (ws.InternalRequest.Response != null)
        {
            errorMsg = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
        }
#endif
        Debug.LogFormat("OnError: error occured: {0}\n", (ex != null ? ex : "Unknown Error " + errorMsg));
        webSocket = null;
        ReConnect();
 
    }
 
    void ReConnect()
    {
        if (this.lockReconnect)
            return;
        this.lockReconnect = true;
        StartCoroutine(SetReConnect());
    }
 
    private IEnumerator SetReConnect()
    {
        Debug.Log("正在重连websocket");
        yield return new WaitForSeconds(5);
        CreateWebSocket();
        lockReconnect = false;
    }
 
    //心跳检测
    private void HeartCheck()
    {
        if (_clientPing != null)
        {
            StopCoroutine(_clientPing);
            _clientPing = null;
        }
        if (_serverPing != null)
        {
            StopCoroutine(_serverPing);
            _serverPing = null;
        }
        _clientPing = StartCoroutine(ClientPing());
    }
 
    // 这里发送一个心跳，后端收到后，返回一个心跳消息
    // onmessage拿到返回的心跳就说明连接正常
    private IEnumerator ClientPing()
    {
        yield return new WaitForSeconds(5);
        WebSend("heartbeat");
        _serverPing = StartCoroutine(ServerPing());
    }
    // 如果超过一定时间还没重置，说明后端主动断开了
    // 如果onclose会执行reconnect，我们执行ws.close()就行了.如果直接执行reconnect 会触发onclose导致重连两次
    private IEnumerator ServerPing()
    {
        yield return new WaitForSeconds(5);
        if (webSocket != null)
        {
            webSocket.Close();
        }
      
    }
 
    //发送心跳
    private IEnumerator HeartPing()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            this.WebSend("Heartbeat");
        }
    }
    //向服务端发送信息
    public void WebSend(string msg)
    {
        if (webSocket == null || string.IsNullOrEmpty(msg)) return;
        if (webSocket.IsOpen)
        {
           
            try
            {
                webSocket.Send(msg);
                Debug.Log("发送数据成功" + msg);
            }
            catch (Exception ex)
            {

                Debug.Log("发送数据失败" + ex.ToString()); 
            }
        }
    }
 
    public void OnClose()
    {
        // 关闭连接
        webSocket.Close(1000, "Bye!");
    }
}
