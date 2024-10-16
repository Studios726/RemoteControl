using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        public ClientConnection connectionRC;
        public ClientConnection connectionPC;
        public ClientConnection connectionSCA;
        private MachineMove machineMove_1;
        private MachineMove machineMove_2;
        private Timer timerRc;
        private Timer timerPc;
        private Timer chartTimer;
        private Timer warningTimer;
        private bool isConnect = false;

        public void EnterGame()
        {
            ReadConfig();
            AddListener();
            InitMode();
            CreatConnect(null, null);
            UIManager.Instance.OpenUI(UIID.LoginPanel);
            chartTimer = Timer.Register(5, true, true, (() => { GameDataManager.Instance.RecordChart(); }));
            // warningTimer=Timer.Register(1,true,true,(() =>
            // {
            //     GameDataManager.Instance.RefreshWarningDesQueue();
            // }));
            // GameDataManager.Instance.GetLocalSCAData();
        }

        public void OnExitGame()
        {
            Dispose();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void InitMode()
        {
            machineMove_1 = GameObject.Find("ModelRoot/Model/machine1").GetComponent<MachineMove>();
            machineMove_2 = GameObject.Find("ModelRoot/Model/machine2").GetComponent<MachineMove>();
            GameDataManager.Instance.SetMachine(machineMove_1, machineMove_2);
        }

        public void CreatConnect(object o, EventArgs eventArgs)
        {
            if (isConnect)
            {
                return; //避免重复登录
            }

            isConnect = true;
            connectionRC = new GameObject().AddComponent<ClientConnection>();
            connectionRC.Init("ws://" + GameDataManager.Instance.IpConfig.TaoIP, SocketType.TaoRC);
            connectionPC = new GameObject().AddComponent<ClientConnection>();
            connectionPC.Init("ws://" + GameDataManager.Instance.IpConfig.TaskIP, SocketType.TaskPC);
            connectionSCA = new GameObject().AddComponent<ClientConnection>();
            connectionSCA.Init("ws://" + GameDataManager.Instance.IpConfig.YuanIP, SocketType.SCA);

            MessageCenter.Instance.RegisterListener(MessageType.RC, connectionRC.WebSend);
            MessageCenter.Instance.RegisterListener(MessageType.PC, connectionPC.WebSend);
            MessageCenter.Instance.RegisterListener(MessageType.SCA, connectionSCA.WebSend);
        }

        public void ReadConfig()
        {
            if (GameDataManager.Instance.IpConfig != null)
            {
                return;
            }

            string exeRootPath = Application.dataPath;
            string parentPath = Directory.GetParent(exeRootPath).FullName;
            string filePath = parentPath + "\\IpConfig.txt";
            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                IpConfig ipConfig = JsonMgr.DeSerialize<IpConfig>(content);
                GameDataManager.Instance.SetIpConfig(ipConfig);
            }
            else
            {
                IpConfig config = new IpConfig();
                config.TaoIP = Address.serviceTaoIP;
                config.YuanIP = Address.serviceYuanIP;
                config.TaskIP = Address.serviceTaskIP;
                config.DataIP = Address.serviceIP;
                GameDataManager.Instance.SetIpConfig(config);
            }
        }

        public void AddListener()
        {
            EventManager.Instance.AddListener(EventName.ConnectionSuccess, ConnectionSuccess);
            EventManager.Instance.AddListener(EventName.ConnectionFail, ConnectionFail);
            EventManager.Instance.AddListener(EventName.ConnectionClose, ConnectionFail);
            EventManager.Instance.AddListener(EventName.ConnectionError, ConnectionFail);
            EventManager.Instance.AddListener(EventName.ReConnect, ReConnect);
            EventManager.Instance.AddListener(EventName.Message, MessageReveive);
            EventManager.Instance.AddListener(EventName.LoginSuccess, CreatConnect);
        }

        private void OnDestroy()
        {
            Dispose();
            Debug.LogError("销毁GameMain");
        }

        public void Dispose()
        {
            if (connectionRC != null && connectionRC.isConnect)
            {
                connectionRC.OnClose();
                connectionRC = null;
            }

            if (connectionPC != null && connectionPC.isConnect)
            {
                connectionPC.OnClose();
                connectionPC = null;
            }

            if (connectionSCA != null && connectionSCA.isConnect)
            {
                connectionSCA.OnClose();
                connectionSCA = null;
            }

            if (chartTimer != null)
            {
                chartTimer.Cancel();
                chartTimer = null;
            }

            EventManager.Instance.RemoveListener(EventName.ConnectionSuccess, ConnectionSuccess);
            EventManager.Instance.RemoveListener(EventName.ConnectionFail, ConnectionFail);
            EventManager.Instance.RemoveListener(EventName.ConnectionClose, ConnectionFail);
            EventManager.Instance.RemoveListener(EventName.ConnectionError, ConnectionFail);
            EventManager.Instance.RemoveListener(EventName.ReConnect, ReConnect);
            EventManager.Instance.RemoveListener(EventName.Message, MessageReveive);
            EventManager.Instance.RemoveListener(EventName.LoginSuccess, CreatConnect);
        }

        public void ConnectionSuccess(object o, EventArgs eventArgs)
        {
            ConnectEventArgs connectEventArgs = (ConnectEventArgs)eventArgs;
            string des = "";
            if (connectEventArgs.type == SocketType.TaoRC)
            {
                if (timerRc != null)
                {
                    timerRc.Cancel();
                    timerRc = null;
                }

                des = "远程驱动连接成功";
                timerRc = Timer.Register(0.2f, true, true, () => { GameDataManager.Instance.UpdatePlcData(); });
            }
            else if (connectEventArgs.type == SocketType.TaskPC)
            {
                if (timerPc != null)
                {
                    timerPc.Cancel();
                    timerPc = null;
                }

                TaskDataManager.Instance.UpdatePcData();
                timerPc = Timer.Register(3600f, true, true, () => { TaskDataManager.Instance.UpdatePcData(); });
                des = "任务规划连接成功";
            }
            else if (connectEventArgs.type == SocketType.SCA)
            {
                GameDataManager.Instance.UpdateSCAData(4);
                des = "三维扫描连接成功";
            }

            Debug.Log("----------------------Success " + connectEventArgs.type);
            DataManager.Instance.InsertHistoryWarningMc(des, GameDataManager.Instance.GetUserName(),
                Machine.BucketWheelStackerReclaimer);
            DataManager.Instance.InsertHistoryWarningMc(des, GameDataManager.Instance.GetUserName(),
                Machine.BucketWheel);
        }

        public void ConnectionFail(object o, EventArgs eventArgs)
        {
            ConnectEventArgs connectEventArgs = (ConnectEventArgs)eventArgs;
            string des = "";
            bool isInsert = false;
            if (connectEventArgs.type == SocketType.TaoRC)
            {
                if (timerRc != null)
                {
                    timerRc.Cancel();
                    timerRc = null;
                }

                des = "远程驱动连接失败";
                isInsert = connectionRC.ReconnectCount == 0;
            }
            else if (connectEventArgs.type == SocketType.TaskPC)
            {
                if (timerPc != null)
                {
                    timerPc.Cancel();
                    timerPc = null;
                }

                des = "任务规划连接失败";
                isInsert = connectionPC.ReconnectCount == 0;
            }
            else if (connectEventArgs.type == SocketType.SCA)
            {
                des = "三维扫描连接失败";  
                isInsert = connectionSCA.ReconnectCount == 0;
            }

            Debug.Log("----------------------Fail " + connectEventArgs.type);
            if (isInsert == true)
            {
                DataManager.Instance.InsertHistoryWarningMc(des, GameDataManager.Instance.GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                DataManager.Instance.InsertHistoryWarningMc(des, GameDataManager.Instance.GetUserName(),
                    Machine.BucketWheel);
            }
        }

        public void ReConnect(object o, EventArgs eventArgs)
        {
            ConnectEventArgs connectEventArgs = (ConnectEventArgs)eventArgs;
            string des = "";
            bool isInsert = false;
            if (connectEventArgs.type == SocketType.TaoRC)
            {
                des = "RC 重连";
                isInsert = connectionRC.ReconnectCount == 0;
            }
            else if (connectEventArgs.type == SocketType.TaskPC)
            {
                des = "PC 重连";
                isInsert = connectionPC.ReconnectCount == 0;
            }
            else if (connectEventArgs.type == SocketType.SCA)
            {
                des = "SCA 重连";
                isInsert = connectionSCA.ReconnectCount == 0;
            }

            if (isInsert)
            {
                DataManager.Instance.InsertHistoryWarningMc(des, GameDataManager.Instance.GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                DataManager.Instance.InsertHistoryWarningMc(des, GameDataManager.Instance.GetUserName(),
                    Machine.BucketWheel);
            }
            
        }

        public void MessageReveive(object o, EventArgs eventArgs)
        {
            MessageEventArgs messageEventArgs = (MessageEventArgs)eventArgs;
            if (messageEventArgs.socketTpe == SocketType.TaoRC)
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