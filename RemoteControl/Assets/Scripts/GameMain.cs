using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using RemoteControl.Event;
using ShenYangRemoteSystem.Subclass;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RemoteControl
{
    public class GameMain : MonoBehaviour
    {
        // Start is called before the first frame update
        private ClientConnection connectionRC;
        private ClientConnection connectionPC;
        private ClientConnection connectionSCA;
        private MachineMove machineMove_1;
        private MachineMove machineMove_2;
        private Timer timerRc;
        private Timer timerPc;
        private bool isConnect=false;
        public void EnterGame()
        {
            AddListener();
            InitMode();
            //CreatConnect();
            UIManager.Instance.OpenUI(UIID.LoginPanel);
        }

        public void InitMode()
        {
            machineMove_1=GameObject.Find("ModelRoot/Model/machine1").GetComponent<MachineMove>();
            machineMove_2 = GameObject.Find("ModelRoot/Model/machine2").GetComponent<MachineMove>();
            GameDataManager.Instance.SetMachine(machineMove_1, machineMove_2);
        }
        public void CreatConnect(object o, EventArgs eventArgs)
        {
            if (isConnect) {
                return;//避免重复登录
            }
            isConnect = true;
            connectionRC = new GameObject().AddComponent<ClientConnection>();
            connectionRC.Init(Address.taoUrl, SocketType.TaoRC);
            connectionPC = new GameObject().AddComponent<ClientConnection>();
            connectionPC.Init(Address.taskUrl, SocketType.TaskPC);
            connectionSCA = new GameObject().AddComponent<ClientConnection>();
            connectionSCA.Init(Address.yuanUrl, SocketType.SCA);

            MessageCenter.Instance.RegisterListener(MessageType.RC, connectionRC.WebSend);
            MessageCenter.Instance.RegisterListener(MessageType.PC, connectionPC.WebSend);
            MessageCenter.Instance.RegisterListener(MessageType.SCA, connectionSCA.WebSend);
        }
        public void AddListener()
        {
            EventManager.Instance.AddListener(EventName.ConnectionSuccess,ConnectionSuccess);
            EventManager.Instance.AddListener(EventName.ConnectionFail,ConnectionFail); 
            EventManager.Instance.AddListener(EventName.ConnectionClose, ConnectionFail);
            EventManager.Instance.AddListener(EventName.ConnectionError, ConnectionFail);
            EventManager.Instance.AddListener(EventName.Message,MessageReveive);
            EventManager.Instance.AddListener(EventName.LoginSuccess, CreatConnect);
        }

        public void ConnectionSuccess(object o,EventArgs eventArgs)
        {
            ConnectEventArgs connectEventArgs = (ConnectEventArgs)eventArgs;
            if (connectEventArgs.type == SocketType.TaoRC)
            {
                if (timerRc != null)
                {
                    timerRc.Cancel();
                    timerRc=null;
                }
                timerRc = Timer.Register(1, true, true, () => {
                    GameDataManager.Instance.UpdatePlcData();
                });
            }else if(connectEventArgs.type == SocketType.TaskPC)
            {
                if (timerPc != null)
                {
                    timerPc.Cancel();
                    timerPc=null;
                }
                timerPc = Timer.Register(1, true, true, () => {
                    GameDataManager.Instance.UpdatePcData();
                });
            }
          
            Debug.Log("----------------------Success "+ connectEventArgs.type);
        }
        public void ConnectionFail(object o,EventArgs eventArgs)
        {
            
            ConnectEventArgs connectEventArgs = (ConnectEventArgs)eventArgs;
            if (connectEventArgs.type == SocketType.TaoRC)
            {
                if (timerRc != null)
                {
                    timerRc.Cancel();
                    timerRc=null;
                }
                GameDataManager.Instance.RcConnectionState = false;
            }else if (connectEventArgs.type == SocketType.TaskPC)
            {
                if (timerPc != null)
                {
                    timerPc.Cancel();
                    timerPc=null;
                }
            }
            Debug.Log("----------------------Fail "+ connectEventArgs.type);
        }

        public void MessageReveive(object o,EventArgs eventArgs)
        {
            MessageEventArgs messageEventArgs = (MessageEventArgs)eventArgs;
            if (messageEventArgs.socketTpe==SocketType.TaoRC)
            {
                //Debug.Log("消息接受"+messageEventArgs.message);
                //// SystemVariables systemVariables= JsonMgr.DeSerialize<SystemVariables>(messageEventArgs.message);
                //Debug.Log("消息反序列号完成");
            }
            else if (messageEventArgs.socketTpe == SocketType.TaskPC)
            {
                //
            }

        }
    }
}