using System;
using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using ShenYangRemoteSystem.Subclass;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RemoteControl
{
    public class GameMain : MonoBehaviour
    {
        // Start is called before the first frame update
        private ClientConnection connection;

        void Start()
        {
            AddListener();
            CreatConnect();
        }

        public void CreatConnect()
        {
            connection = new GameObject().AddComponent<ClientConnection>();
            connection.Init(Address.yuanUrl, SocketType.Socket1);
            MessageCenter.Instance.RegisterListener(MessageType.Plc, connection.WebSend);
        }
        public void AddListener()
        {
            EventManager.Instance.AddListener(EventName.ConnectionSuccess,ConnectionSuccess);
            EventManager.Instance.AddListener(EventName.ConnectionFail,ConnectionFail);
            EventManager.Instance.AddListener(EventName.Message,MessageReveive);
        }

        public void ConnectionSuccess(object o,EventArgs eventArgs)
        {
            ConnectEventArgs a = (ConnectEventArgs)eventArgs;
            Debug.Log("----------------------Success "+a.type);
        }
        public void ConnectionFail(object o,EventArgs eventArgs)
        {
            ConnectEventArgs a = (ConnectEventArgs)eventArgs;
            Debug.Log("----------------------Fail "+a.type);
        }

        public void MessageReveive(object o,EventArgs eventArgs)
        {
            MessageEventArgs messageEventArgs = (MessageEventArgs)eventArgs;
            if (messageEventArgs.socketTpe==SocketType.Socket1)
            {
                Debug.Log("消息接受"+messageEventArgs.message);
                // SystemVariables systemVariables= JsonMgr.DeSerialize<SystemVariables>(messageEventArgs.message);
                Debug.Log("消息反序列号完成");
            }
           
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}