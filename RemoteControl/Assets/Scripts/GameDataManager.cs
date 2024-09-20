using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RemoteControl;
using RemoteControl.Event;
using ShangHaiPro;
using ShenYangRemoteSystem.Subclass;
using UnityEngine;
using UnityEngine.Rendering;

public struct WarningData
{
    public string Des;
    public DateTime Time;

    public WarningData(string des,int rank=3)
    {
        string time = DateTime.Now.ToString("HH:mm:ss");
        if (rank==0)
        {
            Des = $"<color=#FF0000>{des} {time}</color>";
        }else if (rank==1)
        {
            Des = $"<color=#FFFF00>{des} {time}</color>";
        }
        else
        {
            Des = des + " " + time;
        }
       
        Time = DateTime.Now;
    }
}

public class GameDataManager : Singleton<GameDataManager>
{
    private bool _rcConnectionState;
    private SystemVariables _systemVariables;
    private SendDataReportAndDEM _sendDataReportAndDem;
    private MachineMove machineMove_1; //堆取斗轮机
    private MachineMove machineMove_2; //取斗轮机
    public AccountInfo curAccountInfo;
    public GameObject machineRoot;
    public GameMain GameMain;
    private string _taoIP;

    private IpConfig _ipConfig;

    // private int count;
    public Queue<WarningData> BucketWheelQueue = new Queue<WarningData>();
    public Queue<WarningData> BucketWheelStackerReclaimerQueue = new Queue<WarningData>();

    public SystemVariables SystemVariables
    {
        get => _systemVariables;
    }

    public bool IsAdmin()
    {
        if (curAccountInfo!=null&&curAccountInfo.isAdmin)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public string TaoIP
    {
        get => _taoIP;
    }

    public IpConfig IpConfig
    {
        get => _ipConfig;
    }

    public void SetIpConfig(IpConfig ipConfig)
    {
        _ipConfig = ipConfig;
    }

    public string GetUserName()
    {
        if (curAccountInfo != null)
        {
            return curAccountInfo.name;
        }

        return "管理者";
    }

    public void SetIp(string ip)
    {
        _taoIP = "ws://" + ip;
    }

    public SendDataReportAndDEM SendDataReportAndDEM
    {
        get => _sendDataReportAndDem;
    }

    public void SetSystemVariables(SystemVariables systemVariables)
    {
        RecordWarning(systemVariables);
        _systemVariables = systemVariables;
        _rcConnectionState = _systemVariables.D1PLC1CommunicationState;

        UpdateMachine();
    
        EventManager.Instance.TriggerEvent(EventName.UpdateRcData, null);
    }

    public bool GetPlcConnection(Machine machine)
    {
        if (_systemVariables == null)
        {
            return false;
        }

        if (machine == Machine.BucketWheelStackerReclaimer)
        {
            if (_systemVariables.D1PLC1CommunicationState == false ||
                _systemVariables.D1PLC2CommunicationState == false || GameMain.connectionRC.isConnect == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (_systemVariables.D2PLC1CommunicationState == false ||
                _systemVariables.D2PLC2CommunicationState == false || GameMain.connectionRC.isConnect == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public void SetScaReportAndDem(SendDataReportAndDEM sendDataReportAndDEM)
    {
        _sendDataReportAndDem = sendDataReportAndDEM;
        Debug.Log($"模型数据状态 {_sendDataReportAndDem.code}");
        EventManager.Instance.TriggerEvent(EventName.RefreshModel, null);
    }

    public void RecordLocalSCAData(string jsonData)
    {
        PlayerPrefs.SetString("LocalSCAData", jsonData);
    }

    public void GetLocalSCAData()
    {
        string json = PlayerPrefs.GetString("LocalSCAData", "");
        // Debug.LogError($" 获取本地数据sca {json}");
        if (json != "")
        {
            try
            {
                SetScaReportAndDem(JsonMgr.DeSerialize<SendDataReportAndDEM>(json));
            }
            catch (Exception e)
            {
                Debug.LogError("本地读取SCA数据失败");
            }
        }
    }

    public void SetMachine(MachineMove machine1, MachineMove machine2)
    {
        machineMove_1 = machine1;
        machineMove_2 = machine2;
    }

    public void SetMachineActive(bool active)
    {
        if (machineRoot == null)
        {
            machineRoot = GameObject.Find("ModelRoot");
        }

        if (machineRoot == null)
        {
            return;
        }

        machineRoot.SetActive(active);
    }

    public void UpdateMachine()
    {
        UpdateMachinePosAndRot();
        UpdateDirectionBucketWheel();
        UpdateDirectionBucketWheelStackerReclaimer();
        UpdateMachineWarning();
        UpdateWheelAnimation();
    }

    public void UpdateMachinePosAndRot()
    {
        if (machineMove_1)
        {
            machineMove_1.UpdatePosAndRotaionByMeter(SystemVariables.DC_Pos, SystemVariables.SLEW_Angle,
                -SystemVariables.Luff_Angle); //
        }

        if (machineMove_2)
        {
            machineMove_2.UpdatePosAndRotaionByMeter(SystemVariables.DC_Pos_2 + 64.34f, SystemVariables.SLEW_Angle_2,
                -SystemVariables.Luff_Angle_2); //
        }
    }

    public void UpdateMachineWarning()
    {
        if (machineMove_1)
        {
            string error = "";
            error = _systemVariables.LargeCarForwardLimit ? error + "大车前进限位\n" : error;
            error = _systemVariables.LargeCarReverseLimit ? error + "大车后退限位\n" : error;
            error = _systemVariables.LargeCarForwardExtremeLimit ? error + "大车前进极限\n" : error;
            error = _systemVariables.LargeCarReverseExtremeLimit ? error + "大车后退极限\n" : error;

            error = _systemVariables.RotaryLeftTurnLimit ? error + "回转左转限位\n" : error;
            error = _systemVariables.RotaryRightTurnLimit ? error + "回转右转限位\n" : error;
            error = _systemVariables.RotaryLeftTurnExtremeLimit ? error + "回转左转极限\n" : error;
            error = _systemVariables.RotaryRightTurnExtremeLimit ? error + "回转右转极限\n" : error;

            error = _systemVariables.VariableAmplitudeUpperLimit ? error + "变幅上仰限位\n" : error;
            error = _systemVariables.VariableAmplitudeLowerLimit ? error + "变幅下俯限位\n" : error;
            error = _systemVariables.VariableAmplitudeUpperExtremeLimit ? error + "变幅上仰极限\n" : error;
            error = _systemVariables.VariableAmplitudeLowerExtremeLimit ? error + "变幅下俯极限\n" : error;

            machineMove_1.UpdateErrorText(error);
        }

        if (machineMove_2)
        {
            string error = "";
            error = _systemVariables.LargeCarForwardLimit_2 ? error + "大车前进限位\n" : error;
            error = _systemVariables.LargeCarReverseLimit_2 ? error + "大车后退限位\n" : error;
            error = _systemVariables.LargeCarForwardExtremeLimit_2 ? error + "大车前进极限\n" : error;
            error = _systemVariables.LargeCarReverseExtremeLimit_2 ? error + "大车后退极限\n" : error;

            error = _systemVariables.RotaryLeftTurnLimit_2 ? error + "回转左转限位\n" : error;
            error = _systemVariables.RotaryRightTurnLimit_2 ? error + "回转右转限位\n" : error;
            error = _systemVariables.RotaryLeftTurnExtremeLimit_2 ? error + "回转左转极限\n" : error;
            error = _systemVariables.RotaryRightTurnExtremeLimit_2 ? error + "回转右转极限\n" : error;

            error = _systemVariables.VariableAmplitudeUpperLimit_2 ? error + "变幅上仰限位\n" : error;
            error = _systemVariables.VariableAmplitudeLowerLimit_2 ? error + "变幅下俯限位\n" : error;
            error = _systemVariables.VariableAmplitudeUpperExtremeLimit_2 ? error + "变幅上仰极限\n" : error;
            error = _systemVariables.VariableAmplitudeLowerExtremeLimit_2 ? error + "变幅下俯极限\n" : error;

            machineMove_2.UpdateErrorText(error);
        }
    }

    public void UpdateDirectionBucketWheel()
    {
        ModelDirection[] direction_2 = new ModelDirection[3];
        int index = 0;
        if (_systemVariables.LargeCarForwardCommand_2)
        {
            //大车前进
            direction_2[index++] = ModelDirection.Forward;
        }
        else if (_systemVariables.LargeCarReverseCommand_2)
        {
            //大车后退
            direction_2[index++] = ModelDirection.Backward;
        }
        else
        {
            direction_2[index++] = ModelDirection.FbStop;
        }

        if (_systemVariables.RotaryLeftTurnCommand_2)
        {
            //大车左转
            direction_2[index++] = ModelDirection.Left;
        }
        else if (_systemVariables.RotaryRightTurnCommand_2)
        {
            //大车右转
            direction_2[index++] = ModelDirection.Right;
        }
        else
        {
            direction_2[index++] = ModelDirection.LrStop;
        }

        if (_systemVariables.VariableAmplitudeUpperElectromagneticValveOpen_2)
        {
            //大车上仰
            direction_2[index++] = ModelDirection.Up;
        }
        else if (_systemVariables.VariableAmplitudeLowerElectromagneticValveOpen_2)
        {
            //大车下俯
            direction_2[index++] = ModelDirection.Down;
        }
        else
        {
            direction_2[index++] = ModelDirection.UdStop;
        }

        EventManager.Instance.TriggerEvent(EventName.UpdateModelDirection, null,
            new UpdateModelDirectionEventArgs(direction_2, Machine.BucketWheel));
    }

    public void UpdateDirectionBucketWheelStackerReclaimer()
    {
        ModelDirection[] direction = new ModelDirection[3];
        int index = 0;
        if (_systemVariables.LargeCarForwardCommand)
        {
            //大车前进
            direction[index++] = ModelDirection.Forward;
        }
        else if (_systemVariables.LargeCarReverseCommand)
        {
            //大车后退
            direction[index++] = ModelDirection.Backward;
        }
        else
        {
            direction[index++] = ModelDirection.FbStop;
        }

        if (_systemVariables.RotaryLeftTurnCommand)
        {
            //大车左转
            direction[index++] = ModelDirection.Left;
        }
        else if (_systemVariables.RotaryRightTurnCommand)
        {
            //大车右转
            direction[index++] = ModelDirection.Right;
        }
        else
        {
            direction[index++] = ModelDirection.LrStop;
        }

        if (_systemVariables.VariableAmplitudeUpperElectromagneticValveOpen)
        {
            //大车上仰
            direction[index++] = ModelDirection.Up;
        }
        else if (_systemVariables.VariableAmplitudeLowerElectromagneticValveOpen)
        {
            //大车下俯
            direction[index++] = ModelDirection.Down;
        }
        else
        {
            direction[index++] = ModelDirection.UdStop;
        }

        EventManager.Instance.TriggerEvent(EventName.UpdateModelDirection, null,
            new UpdateModelDirectionEventArgs(direction, Machine.BucketWheelStackerReclaimer));
    }

    public void UpdateWheelAnimation()
    {
        machineMove_1.PlayRotationClip(_systemVariables.BucketWheelMotorRunning);
        machineMove_2.PlayRotationClip(_systemVariables.BucketWheelMotorRunning_2);
    }

    public async Task DeSerializeScaJson(string json)
    {
        SendDataReportAndDEM cursendDataReportAndDem = new SendDataReportAndDEM();
        await Task.Run((() =>
        {
            SystemCommand command = JsonMgr.DeSerialize<SystemCommand>(json);
            cursendDataReportAndDem = command.SendAllData.DEM_DATA;
        }));
        SetScaReportAndDem(cursendDataReportAndDem);
    }

    public async Task SpawnCoalModel(Transform parent, Material material, SendDataReportAndDEM sendDataReportAndDem,
        GameObject model = null)
    {
        if (sendDataReportAndDem == null || sendDataReportAndDem.SendCoalHeapDEM == null)
        {
            Debug.LogError("模型数据是空");
            return;
        }

        if (model == null)
        {
            Debug.LogError("模型对象为空");
            return;
        }

        CoalHeapDEM demData = sendDataReportAndDem.SendCoalHeapDEM;

        // 预先分配数组空间，避免频繁扩容
        Vector3[] vertices = new Vector3[demData.NZ * demData.NX];
        int[] triangles = new int[(demData.NZ - 1) * (demData.NX - 1) * 6];
        Color[] colorList = new Color[demData.NZ * demData.NX];
        try
        {
            await Task.Run(() =>
            {
                int vertexIndex = 0;
                // 第一步处理，解析数据格式
                for (int i = 0; i < demData.NZ; i++)
                {
                    for (int j = 0; j < demData.NX; j++)
                    {
                        float X = demData.Z0 + demData.DZ * i - (demData.NZ - 1) * demData.DZ / 2;
                        float Z = -(demData.X0 + demData.DX * j) + (demData.NX - 1) * demData.DX / 2;
                        float Y = -demData.DEM[i, j];
                        vertices[vertexIndex++] = new Vector3(X, Y, Z);
                    }
                }

                int triangleIndex = 0;
                // 第二步处理，例如生成顶点数据
                for (int m = 0; m < demData.NZ - 1; m++)
                {
                    for (int n = 0; n < demData.NX - 1; n++)
                    {
                        int[] face = new int[6];

                        face[0] = m * demData.NX + n;
                        face[1] = (m + 1) * demData.NX + n;
                        face[2] = m * demData.NX + (n + 1);

                        face[3] = m * demData.NX + (n + 1);
                        face[4] = (m + 1) * demData.NX + n;
                        face[5] = (m + 1) * demData.NX + (n + 1);

                        for (int q = 0; q < 6; q++)
                        {
                            triangles[triangleIndex++] = face[q];
                        }
                    }
                }

                int colorIndex = 0;
                REGION[] regionList = demData.REGION_LIST;
                float xLength = demData.LENGTH / 2;
                foreach (Vector3 ve3 in vertices)
                {
                    Color pointColor = Color.gray;
                    bool foundColor = false;
                    for (int regIndex = 0; regIndex < regionList.Length && !foundColor; regIndex++)
                    {
                        REGION reg = regionList[regIndex];
                        int side;
                        if (ve3.z > 0)
                        {
                            side = 1;
                        }
                        else
                        {
                            side = 0;
                        }

                        if (ve3.x + xLength >= reg.BEGIN && ve3.x + xLength < reg.END && side == reg.SIDE)
                        {
                            if (reg.IsUseLayer == 0)
                            {
                                pointColor = new Color(reg.ColorR / 255.0f, reg.ColorG / 255.0f, reg.ColorB / 255.0f,
                                    1);
                                foundColor = true;
                            }
                            else
                            {
                                for (int i = 0; i < reg.layerArray.Count && !foundColor; i++)
                                {
                                    if (ve3.y >= Mathf.Abs(reg.layerArray[i].hBEGIN) &&
                                        ve3.y < Mathf.Abs(reg.layerArray[i].hEND))
                                    {
                                        pointColor = new Color(reg.layerArray[i].ColorR / 255.0f,
                                            reg.layerArray[i].ColorG / 255.0f, reg.layerArray[i].ColorB / 255.0f, 1);
                                        foundColor = true;
                                    }
                                }
                            }
                        }
                    }

                    colorList[colorIndex++] = pointColor;
                }
            });
        }
        catch (Exception ex)
        {
            Debug.LogError($"异步任务执行失败: {ex.Message}");
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colorList;
        model.transform.localRotation = Quaternion.identity;
        MeshFilter meshFilter = model.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = model.GetComponent<MeshRenderer>();
        MeshCollider meshCollider = model.GetComponent<MeshCollider>();
        meshFilter.mesh = mesh;
        meshRenderer.sharedMaterial = material;
        meshFilter.mesh.RecalculateNormals();
        // meshCollider.sharedMesh = mesh;
    }

    /// <summary>
    /// 获取PLC没帧数据
    /// </summary>
    public void UpdatePlcData()
    {
        ServerCommand serverCommand = new ServerCommand();
        serverCommand.QUERY_SYSTEM = "MC";
        serverCommand.DATA_TYPE = 6;
        serverCommand.QUERY_TYPE = 1;
        MessageCenter.Instance.SendMessage(MessageType.RC, serverCommand);
    }

    public void SendServerCommandByName(string commandName, int dataInt = 0)
    {
        ServerCommand serverCommand = new ServerCommand();
        serverCommand.QUERY_SYSTEM = "MC";
        serverCommand.DATA_TYPE = 6;
        serverCommand.QUERY_TYPE = 2;
        serverCommand.COMMAND_NAME = commandName;
        serverCommand.DATA_INT = dataInt;
        MessageCenter.Instance.SendMessage(MessageType.RC, serverCommand);
    }

    public void UpdateSCAData(int query_type)
    {
        Debug.Log("堆料模型更新");
        SystemCommand serverCommand = new SystemCommand();
        serverCommand.QUERY_SYSTEM = "MC";
        serverCommand.DATA_TYPE = 3;
        serverCommand.QUERY_TYPE = query_type;
        MessageCenter.Instance.SendMessage(MessageType.SCA, serverCommand);
    }

    public void RecordChart()
    {
     
        if (_systemVariables == null)
        {
            return;
        }

        if (curAccountInfo != null && curAccountInfo.isAdmin)
        {
            DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC,
                _systemVariables.BucketWheelElectricCurrent, "斗轮电流",
                Machine.BucketWheelStackerReclaimer);

            DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC,
                _systemVariables.LargeCarElectricCurrent, "大车电流",
                Machine.BucketWheelStackerReclaimer);

            DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC,
                _systemVariables.RotaryElectricCurrent, "回转电流",
                Machine.BucketWheelStackerReclaimer);

            DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC,
                _systemVariables.SuspensionBeltElectricCurrent, "悬胶电流",
                Machine.BucketWheelStackerReclaimer);

            // DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC, _systemVariables, "悬臂流量",
            //     Machine.BucketWheelStackerReclaimer);

            DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC,
                _systemVariables.BucketWheelElectricCurrent_2, "斗轮电流",
                Machine.BucketWheel);

            DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC,
                _systemVariables.LargeCarElectricCurrent_2, "大车电流",
                Machine.BucketWheel);

            DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC,
                _systemVariables.RotaryElectricCurrent_2, "回转电流",
                Machine.BucketWheel);

            DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC,
                _systemVariables.SuspensionBeltElectricCurrent_2, "悬胶电流",
                Machine.BucketWheel);

            // DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC, _systemVariables, "悬臂流量",
            //     Machine.BucketWheelStackerReclaimer);
        }
    }

    public void DeleteThreeMonthData() //
    {
        if (curAccountInfo == null || curAccountInfo.isAdmin == false)
        {
            return;
        }

        string time = PlayerPrefs.GetString("Time", "");
        if (time == "")
        {
            PlayerPrefs.SetString("Time", DateTime.Now.ToString());
        }

        DateTime anotherTime = time == "" ? DateTime.Now : DateTime.Parse(time);
        DateTime currentTime = DateTime.Now;

        TimeSpan timeDifference = currentTime - anotherTime;

        double monthsDifference = timeDifference.TotalDays / 30.44; // 平均每个月的天数约为 30.44 天

        if (Math.Abs(monthsDifference) >= 6)
        {
            PlayerPrefs.SetString("Time", DateTime.Now.ToString());
            DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_LOG1_MC);
            DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_LOG2_MC);
            DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_WARNING1_MC);
            DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_WARNING2_MC);

            DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC);
            DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC);

            DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC);
            DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC);

            DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC);
        }
        else
        {
            // Debug.Log("相差不足三个月");
        }
    }

    public void RefreshWarningDesQueue()
    {
        if (BucketWheelStackerReclaimerQueue.Count>0)
        {
            WarningData warningData= BucketWheelStackerReclaimerQueue.Peek();
            if ((DateTime.Now - warningData.Time).TotalSeconds>=8)
            {
                BucketWheelStackerReclaimerQueue.Dequeue();
                EventManager.Instance.TriggerEvent(EventName.RefreshTaskDes1, null);
            }
        }
        
        if (BucketWheelQueue.Count>0)
        {
            WarningData warningData= BucketWheelQueue.Peek();
            if ((DateTime.Now - warningData.Time).TotalSeconds>=8)
            {
                BucketWheelQueue.Dequeue();
                EventManager.Instance.TriggerEvent(EventName.RefreshTaskDes2, null);
            }
        }
    }
    public void AddOrUpdateWarningDesQueue(string des, Machine machine,int rank=0)
    {
        WarningData warningData = new WarningData(des,rank);
        if (machine == Machine.BucketWheelStackerReclaimer)
        {
            if (BucketWheelStackerReclaimerQueue.Count >= 3)
            {
                BucketWheelStackerReclaimerQueue.Dequeue();
            }

            BucketWheelStackerReclaimerQueue.Enqueue(warningData);
            EventManager.Instance.TriggerEvent(EventName.RefreshTaskDes1, null);
        }
        else
        {
            if (BucketWheelQueue.Count >= 3)
            {
                BucketWheelQueue.Dequeue();
            }

            BucketWheelQueue.Enqueue(warningData);
            EventManager.Instance.TriggerEvent(EventName.RefreshTaskDes2, null);
        }
    }

    public void RecordWarning(SystemVariables newSystemVariables)
    {
        if (_systemVariables != null)
        {
            // 存儲警告信息
            if (newSystemVariables.DriverRoomEmergencyStopButton &&
                _systemVariables.DriverRoomEmergencyStopButton == false)
            {
                //司机室急停
                DataManager.Instance.InsertHistoryWarningMc("司机室急停", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("司机室急停", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.DriverRoomEmergencyStopButton == false &&
                     _systemVariables.DriverRoomEmergencyStopButton == true)
            {
                //司机室急停解除
                DataManager.Instance.InsertHistoryWarningMc("司机室急停解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.ElectricalRoomEmergencyStopButton &&
                _systemVariables.ElectricalRoomEmergencyStopButton == false)
            {
                //电气室急停
                DataManager.Instance.InsertHistoryWarningMc("电气室急停", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("电气室急停", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.ElectricalRoomEmergencyStopButton == false &&
                     _systemVariables.ElectricalRoomEmergencyStopButton == true)
            {
                //电气室急停解除
                DataManager.Instance.InsertHistoryWarningMc("电气室急停解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.EmergencyStopRelay && _systemVariables.EmergencyStopRelay == false)
            {
                //急停继电器
                DataManager.Instance.InsertHistoryWarningMc("急停继电器", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("急停继电器", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.EmergencyStopRelay == false && _systemVariables.EmergencyStopRelay == true)
            {
                //急停继电器解除
                DataManager.Instance.InsertHistoryWarningMc("急停继电器解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RemoteEmergencyStop && _systemVariables.RemoteEmergencyStop == false)
            {
                //远程急停
                DataManager.Instance.InsertHistoryWarningMc("远程急停", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("远程急停", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RemoteEmergencyStop == false && _systemVariables.RemoteEmergencyStop == true)
            {
                //远程急停解除
                DataManager.Instance.InsertHistoryWarningMc("远程急停解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.BucketWheelFault && _systemVariables.BucketWheelFault == false)
            {
                //斗轮机故障
                DataManager.Instance.InsertHistoryWarningMc("斗轮机故障", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("斗轮机故障", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.BucketWheelFault == false && _systemVariables.BucketWheelFault == true)
            {
                //斗轮机故障解除
                DataManager.Instance.InsertHistoryWarningMc("斗轮机故障解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.LargeCarFault && _systemVariables.LargeCarFault == false)
            {
                //大车-大车故障
                DataManager.Instance.InsertHistoryWarningMc("大车-大车故障", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("大车-大车故障", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.LargeCarFault == false && _systemVariables.LargeCarFault == true)
            {
                //大车-大车故障解除
                DataManager.Instance.InsertHistoryWarningMc("大车-大车故障解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.LargeCarFrequencyConverterFault &&
                _systemVariables.LargeCarFrequencyConverterFault == false)
            {
                //大车-变频器故障
                DataManager.Instance.InsertHistoryWarningMc("大车-变频器故障", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("大车-变频器故障", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.LargeCarFrequencyConverterFault == false &&
                     _systemVariables.LargeCarFrequencyConverterFault == true)
            {
                //大车-变频器故障解除
                DataManager.Instance.InsertHistoryWarningMc("大车-变频器故障解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.LargeCarBrakeResistorOverheatSwitch &&
                _systemVariables.LargeCarBrakeResistorOverheatSwitch == false)
            {
                //大车-制动电阻超温
                DataManager.Instance.InsertHistoryWarningMc("大车-制动电阻超温", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("大车-制动电阻超温", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.LargeCarBrakeResistorOverheatSwitch == false &&
                     _systemVariables.LargeCarBrakeResistorOverheatSwitch == true)
            {
                //大车-制动电阻超温解除
                DataManager.Instance.InsertHistoryWarningMc("大车-制动电阻超温解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.LargeCarCentralizedLubricationLowOilLevel &&
                _systemVariables.LargeCarCentralizedLubricationLowOilLevel == false)
            {
                //大车-大车集中润滑低油位
                DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑低油位", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("大车-大车集中润滑低油位", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.LargeCarCentralizedLubricationLowOilLevel == false &&
                     _systemVariables.LargeCarCentralizedLubricationLowOilLevel == true)
            {
                //大车-大车集中润滑低油位解除
                DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑低油位解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.LargeCarCentralizedLubricationOilBlockage &&
                _systemVariables.LargeCarCentralizedLubricationOilBlockage == false)
            {
                //大车-大车集中润滑堵油
                DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑堵油", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("大车-大车集中润滑堵油", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.LargeCarCentralizedLubricationOilBlockage == false &&
                     _systemVariables.LargeCarCentralizedLubricationOilBlockage == true)
            {
                //大车-大车集中润滑堵油解除
                DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑堵油解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.LargeCarForwardLimit && _systemVariables.LargeCarForwardLimit == false)
            {
                //大车-前进限位
                DataManager.Instance.InsertHistoryWarningMc("大车-前进限位", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("大车-前进限位", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.LargeCarForwardLimit == false && _systemVariables.LargeCarForwardLimit == true)
            {
                //大车-前进限位解除
                DataManager.Instance.InsertHistoryWarningMc("大车-前进限位解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.LargeCarForwardExtremeLimit && _systemVariables.LargeCarForwardExtremeLimit == false)
            {
                //大车-前进极限
                DataManager.Instance.InsertHistoryWarningMc("大车-前进极限", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("大车-前进极限", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.LargeCarForwardExtremeLimit == false &&
                     _systemVariables.LargeCarForwardExtremeLimit == true)
            {
                //大车-前进极限解除
                DataManager.Instance.InsertHistoryWarningMc("大车-前进极限解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.LargeCarReverseLimit && _systemVariables.LargeCarReverseLimit == false)
            {
                //大车-后退限位
                DataManager.Instance.InsertHistoryWarningMc("大车-后退限位", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("大车-后退限位", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.LargeCarReverseLimit == false && _systemVariables.LargeCarReverseLimit == true)
            {
                //大车-后退限位解除
                DataManager.Instance.InsertHistoryWarningMc("大车-后退限位解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.LargeCarReverseExtremeLimit && _systemVariables.LargeCarReverseExtremeLimit == false)
            {
                //大车-后退极限
                DataManager.Instance.InsertHistoryWarningMc("大车-后退极限", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("大车-后退极限", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.LargeCarReverseExtremeLimit == false &&
                     _systemVariables.LargeCarReverseExtremeLimit == true)
            {
                //大车-后退极限解除
                DataManager.Instance.InsertHistoryWarningMc("大车-后退极限解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.TwoMachineCollisionAlarm && _systemVariables.TwoMachineCollisionAlarm == false)
            {
                //大车-两车碰撞报警
                DataManager.Instance.InsertHistoryWarningMc("大车-两车碰撞报警", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("大车-两车碰撞报警", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.TwoMachineCollisionAlarm == false &&
                     _systemVariables.TwoMachineCollisionAlarm == true)
            {
                //大车-两车碰撞报警解除
                DataManager.Instance.InsertHistoryWarningMc("大车-两车碰撞报警解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.VariableAmplitudeMotorOverload &&
                _systemVariables.VariableAmplitudeMotorOverload == false)
            {
                //变幅-主电机过载
                DataManager.Instance.InsertHistoryWarningMc("变幅-主电机过载", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("变幅-主电机过载", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.VariableAmplitudeMotorOverload == false &&
                     _systemVariables.VariableAmplitudeMotorOverload == true)
            {
                //变幅-主电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-主电机过载解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.VariableAmplitudeUpperLimit && _systemVariables.VariableAmplitudeUpperLimit == false)
            {
                //变幅-上仰限位
                DataManager.Instance.InsertHistoryWarningMc("变幅-上仰限位", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("变幅-上仰限位", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.VariableAmplitudeUpperLimit == false &&
                     _systemVariables.VariableAmplitudeUpperLimit == true)
            {
                //变幅-上仰限位解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-上仰限位解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.VariableAmplitudeUpperExtremeLimit &&
                _systemVariables.VariableAmplitudeUpperExtremeLimit == false)
            {
                //变幅-上仰极限
                DataManager.Instance.InsertHistoryWarningMc("变幅-上仰极限", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("变幅-上仰极限", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.VariableAmplitudeUpperExtremeLimit == false &&
                     _systemVariables.VariableAmplitudeUpperExtremeLimit == true)
            {
                //变幅-上仰极限解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-上仰极限解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.VariableAmplitudeLowerLimit && _systemVariables.VariableAmplitudeLowerLimit == false)
            {
                //变幅-下俯限位
                DataManager.Instance.InsertHistoryWarningMc("变幅-下俯限位", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("变幅-下俯限位", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.VariableAmplitudeLowerLimit == false &&
                     _systemVariables.VariableAmplitudeLowerLimit == true)
            {
                //变幅-下俯限位解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-下俯限位解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.VariableAmplitudeLowerExtremeLimit &&
                _systemVariables.VariableAmplitudeLowerExtremeLimit == false)
            {
                //变幅-下俯极限
                DataManager.Instance.InsertHistoryWarningMc("变幅-下俯极限", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("变幅-下俯极限", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.VariableAmplitudeLowerExtremeLimit == false &&
                     _systemVariables.VariableAmplitudeLowerExtremeLimit == true)
            {
                //变幅-下俯极限解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-下俯极限解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.VariableAmplitudeLowerForbiddenZoneLimit &&
                _systemVariables.VariableAmplitudeLowerForbiddenZoneLimit == false)
            {
                //变幅-下俯禁区
                DataManager.Instance.InsertHistoryWarningMc("变幅-下俯禁区", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("变幅-下俯禁区", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.VariableAmplitudeLowerForbiddenZoneLimit == false &&
                     _systemVariables.VariableAmplitudeLowerForbiddenZoneLimit == true)
            {
                //变幅-下俯禁区解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-下俯禁区解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.VariableAmplitudePumpStationOverheatAlarm &&
                _systemVariables.VariableAmplitudePumpStationOverheatAlarm == false)
            {
                //变幅-泵站高温报警
                DataManager.Instance.InsertHistoryWarningMc("变幅-泵站高温报警", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("变幅-泵站高温报警", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.VariableAmplitudePumpStationOverheatAlarm == false &&
                     _systemVariables.VariableAmplitudePumpStationOverheatAlarm == true)
            {
                //变幅-泵站高温报警解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-泵站高温报警解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.VariableAmplitudeOilLevelVeryLowSignal &&
                _systemVariables.VariableAmplitudeOilLevelVeryLowSignal == false)
            {
                //变幅-油液位低信号
                DataManager.Instance.InsertHistoryWarningMc("变幅-油液位低信号", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("变幅-油液位低信号", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.VariableAmplitudeOilLevelVeryLowSignal == false &&
                     _systemVariables.VariableAmplitudeOilLevelVeryLowSignal == true)
            {
                //变幅-油液位低信号解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-油液位低信号解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.VariableAmplitudeOilLevelLowSignal &&
                _systemVariables.VariableAmplitudeOilLevelLowSignal == false)
            {
                //变幅-液位超低信号
                DataManager.Instance.InsertHistoryWarningMc("变幅-液位超低信号", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("变幅-液位超低信号", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.VariableAmplitudeOilLevelLowSignal == false &&
                     _systemVariables.VariableAmplitudeOilLevelLowSignal == true)
            {
                //变幅-液位超低信号解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-液位超低信号解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.VariableFrequencyOilBlockageSignal &&
                _systemVariables.VariableFrequencyOilBlockageSignal == false)
            {
                //变幅-泵站堵油信号
                DataManager.Instance.InsertHistoryWarningMc("变幅-泵站堵油信号", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("变幅-泵站堵油信号", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.VariableFrequencyOilBlockageSignal == false &&
                     _systemVariables.VariableFrequencyOilBlockageSignal == true)
            {
                //变幅-泵站堵油信号解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-泵站堵油信号解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RotaryFrequencyConverterFault &&
                _systemVariables.RotaryFrequencyConverterFault == false)
            {
                //回转-变频器故障
                DataManager.Instance.InsertHistoryWarningMc("回转-变频器故障", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("回转-变频器故障", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RotaryFrequencyConverterFault == false &&
                     _systemVariables.RotaryFrequencyConverterFault == true)
            {
                //回转-变频器故障解除
                DataManager.Instance.InsertHistoryWarningMc("回转-变频器故障解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RotaryBrakeOverload && _systemVariables.RotaryBrakeOverload == false)
            {
                //回转-制动器过载
                DataManager.Instance.InsertHistoryWarningMc("回转-制动器过载", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("回转-制动器过载", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RotaryBrakeOverload == false && _systemVariables.RotaryBrakeOverload == true)
            {
                //回转-制动器过载解除
                DataManager.Instance.InsertHistoryWarningMc("回转-制动器过载解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RotaryFanOverload && _systemVariables.RotaryFanOverload == false)
            {
                //回转-风机过载
                DataManager.Instance.InsertHistoryWarningMc("回转-风机过载", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("回转-风机过载", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RotaryFanOverload == false && _systemVariables.RotaryFanOverload == true)
            {
                //回转-风机过载解除
                DataManager.Instance.InsertHistoryWarningMc("回转-风机过载解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RotaryBrakeResistorOverheatSwitch &&
                _systemVariables.RotaryBrakeResistorOverheatSwitch == false)
            {
                //回转-制动电阻超温
                DataManager.Instance.InsertHistoryWarningMc("回转-制动电阻超温", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("回转-制动电阻超温", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RotaryBrakeResistorOverheatSwitch == false &&
                     _systemVariables.RotaryBrakeResistorOverheatSwitch == true)
            {
                //回转-制动电阻超温解除
                DataManager.Instance.InsertHistoryWarningMc("回转-制动电阻超温解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RotaryFault && _systemVariables.RotaryFault == false)
            {
                //回转-回转故障
                DataManager.Instance.InsertHistoryWarningMc("回转-回转故障", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("回转-回转故障", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RotaryFault == false && _systemVariables.RotaryFault == true)
            {
                //回转-回转故障解除
                DataManager.Instance.InsertHistoryWarningMc("回转-回转故障解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RotaryLeftTurnLimit && _systemVariables.RotaryLeftTurnLimit == false)
            {
                //回转-左转限位
                DataManager.Instance.InsertHistoryWarningMc("回转-左转限位", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("回转-左转限位", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RotaryLeftTurnLimit == false && _systemVariables.RotaryLeftTurnLimit == true)
            {
                //回转-左转限位解除
                DataManager.Instance.InsertHistoryWarningMc("回转-左转限位解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RotaryLeftTurnExtremeLimit && _systemVariables.RotaryLeftTurnExtremeLimit == false)
            {
                //回转-左转极限
                DataManager.Instance.InsertHistoryWarningMc("回转-左转极限", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("回转-左转极限", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RotaryLeftTurnExtremeLimit == false &&
                     _systemVariables.RotaryLeftTurnExtremeLimit == true)
            {
                //回转-左转极限解除
                DataManager.Instance.InsertHistoryWarningMc("回转-左转极限解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RotaryLeftTurnForbiddenZoneLimit &&
                _systemVariables.RotaryLeftTurnForbiddenZoneLimit == false)
            {
                //回转-左转禁区限位
                DataManager.Instance.InsertHistoryWarningMc("回转-左转禁区限位", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("回转-左转禁区限位", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RotaryLeftTurnForbiddenZoneLimit == false &&
                     _systemVariables.RotaryLeftTurnForbiddenZoneLimit == true)
            {
                //回转-左转禁区限位解除
                DataManager.Instance.InsertHistoryWarningMc("回转-左转禁区限位解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RotaryRightTurnLimit && _systemVariables.RotaryRightTurnLimit == false)
            {
                //回转-右转限位
                DataManager.Instance.InsertHistoryWarningMc("回转-右转限位", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("回转-右转限位", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RotaryRightTurnLimit == false && _systemVariables.RotaryRightTurnLimit == true)
            {
                //回转-右转限位解除
                DataManager.Instance.InsertHistoryWarningMc("回转-右转限位解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RotaryRightTurnExtremeLimit && _systemVariables.RotaryRightTurnExtremeLimit == false)
            {
                //回转-右转极限
                DataManager.Instance.InsertHistoryWarningMc("回转-右转极限", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("回转-右转极限", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RotaryRightTurnExtremeLimit == false &&
                     _systemVariables.RotaryRightTurnExtremeLimit == true)
            {
                //回转-右转极限解除
                DataManager.Instance.InsertHistoryWarningMc("回转-右转极限解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RotaryRightTurnForbiddenZoneLimit &&
                _systemVariables.RotaryRightTurnForbiddenZoneLimit == false)
            {
                //回转-右转禁区限位
                DataManager.Instance.InsertHistoryWarningMc("回转-右转禁区限位", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("回转-右转禁区限位", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RotaryRightTurnForbiddenZoneLimit == false &&
                     _systemVariables.RotaryRightTurnForbiddenZoneLimit == true)
            {
                //回转-右转禁区限位解除
                DataManager.Instance.InsertHistoryWarningMc("回转-右转禁区限位解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RotaryRightTurnForbiddenLimit &&
                _systemVariables.RotaryRightTurnForbiddenLimit == false)
            {
                //回转-右转防撞限位
                DataManager.Instance.InsertHistoryWarningMc("回转-右转防撞限位", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("回转-右转防撞限位", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RotaryRightTurnForbiddenLimit == false &&
                     _systemVariables.RotaryRightTurnForbiddenLimit == true)
            {
                //回转-右转防撞限位解除
                DataManager.Instance.InsertHistoryWarningMc("回转-右转防撞限位解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RotaryOverTorque && _systemVariables.RotaryOverTorque == false)
            {
                //回转-回转过力矩
                DataManager.Instance.InsertHistoryWarningMc("回转-回转过力矩", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("回转-回转过力矩", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RotaryOverTorque == false && _systemVariables.RotaryOverTorque == true)
            {
                //回转-回转过力矩解除
                DataManager.Instance.InsertHistoryWarningMc("回转-回转过力矩解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RotaryCentralizedLubricationOilBlockageFault &&
                _systemVariables.RotaryCentralizedLubricationOilBlockageFault == false)
            {
                //回转-回转集中润滑堵油
                DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑堵油", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("回转-回转集中润滑堵油", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RotaryCentralizedLubricationOilBlockageFault == false &&
                     _systemVariables.RotaryCentralizedLubricationOilBlockageFault == true)
            {
                //回转-回转集中润滑堵油解除
                DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑堵油解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RotaryCentralizedLubricationLowOilLevelFault &&
                _systemVariables.RotaryCentralizedLubricationLowOilLevelFault == false)
            {
                //回转-回转集中润滑低油位
                DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑低油位", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("回转-回转集中润滑低油位", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RotaryCentralizedLubricationLowOilLevelFault == false &&
                     _systemVariables.RotaryCentralizedLubricationLowOilLevelFault == true)
            {
                //回转-回转集中润滑低油位解除
                DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑低油位解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.BucketWheelMotorOverload && _systemVariables.BucketWheelMotorOverload == false)
            {
                //斗轮/槽-电机过载
                DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-电机过载", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("斗轮/槽-电机过载", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.BucketWheelMotorOverload == false &&
                     _systemVariables.BucketWheelMotorOverload == true)
            {
                //斗轮/槽-电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-电机过载解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.BucketWheelOverTorqueSwitch && _systemVariables.BucketWheelOverTorqueSwitch == false)
            {
                //斗轮/槽-斗轮过力矩开关
                DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-斗轮过力矩开关", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("斗轮/槽-斗轮过力矩开关", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.BucketWheelOverTorqueSwitch == false &&
                     _systemVariables.BucketWheelOverTorqueSwitch == true)
            {
                //斗轮/槽-斗轮过力矩开关解除
                DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-斗轮过力矩开关解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.BucketWheelSlotMotorOverload &&
                _systemVariables.BucketWheelSlotMotorOverload == false)
            {
                //斗轮导料槽-电机过载
                DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽-电机过载", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("斗轮/槽-电机过载", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.BucketWheelSlotMotorOverload == false &&
                     _systemVariables.BucketWheelSlotMotorOverload == true)
            {
                //斗轮导料槽-电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽-电机过载解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.SuspensionBeltMotorOverload && _systemVariables.SuspensionBeltMotorOverload == false)
            {
                //悬胶/挡板-电机过载
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-电机过载", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("悬胶/挡板-电机过载", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.SuspensionBeltMotorOverload == false &&
                     _systemVariables.SuspensionBeltMotorOverload == true)
            {
                //悬胶/挡板-电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-电机过载解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.SuspensionBeltFirstLevelDeviationSwitch &&
                _systemVariables.SuspensionBeltFirstLevelDeviationSwitch == false)
            {
                //悬胶/挡板-一级跑偏开关
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-一级跑偏开关", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("悬胶/挡板-一级跑偏开关", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.SuspensionBeltFirstLevelDeviationSwitch == false &&
                     _systemVariables.SuspensionBeltFirstLevelDeviationSwitch == true)
            {
                //悬胶/挡板-一级跑偏开关解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-一级跑偏开关解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.SuspensionBeltSecondLevelDeviationSwitch &&
                _systemVariables.SuspensionBeltSecondLevelDeviationSwitch == false)
            {
                //悬胶/挡板-二级跑偏开关
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-二级跑偏开关", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("悬胶/挡板-二级跑偏开关", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.SuspensionBeltSecondLevelDeviationSwitch == false &&
                     _systemVariables.SuspensionBeltSecondLevelDeviationSwitch == true)
            {
                //悬胶/挡板-二级跑偏开关解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-二级跑偏开关解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.SuspendedBeltSlip && _systemVariables.SuspendedBeltSlip == false)
            {
                //悬胶/挡板-打滑检测开关
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-打滑检测开关", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("悬胶/挡板-打滑检测开关", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.SuspendedBeltSlip == false && _systemVariables.SuspendedBeltSlip == true)
            {
                //悬胶/挡板-打滑检测开关解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-打滑检测开关解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.SuspensionBeltLongitudinalTearSwitch &&
                _systemVariables.SuspensionBeltLongitudinalTearSwitch == false)
            {
                //悬胶/挡板-纵向撕裂开关
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-纵向撕裂开关", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("悬胶/挡板-纵向撕裂开关", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.SuspensionBeltLongitudinalTearSwitch == false &&
                     _systemVariables.SuspensionBeltLongitudinalTearSwitch == true)
            {
                //悬胶/挡板-纵向撕裂开关解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-纵向撕裂开关解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.SuspensionBeltEmergencyStopSwitch &&
                _systemVariables.SuspensionBeltEmergencyStopSwitch == false)
            {
                //悬胶/挡板-急停拉线开关
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-急停拉线开关", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("悬胶/挡板-急停拉线开关", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.SuspensionBeltEmergencyStopSwitch == false &&
                     _systemVariables.SuspensionBeltEmergencyStopSwitch == true)
            {
                //悬胶/挡板-急停拉线开关解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-急停拉线开关解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.SuspensionBeltMaterialFlowDetectionSwitch &&
                _systemVariables.SuspensionBeltMaterialFlowDetectionSwitch == false)
            {
                //悬胶/挡板-料流检测开关
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-料流检测开关", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("悬胶/挡板-料流检测开关", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.SuspensionBeltMaterialFlowDetectionSwitch == false &&
                     _systemVariables.SuspensionBeltMaterialFlowDetectionSwitch == true)
            {
                //悬胶/挡板-料流检测开关解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-料流检测开关解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.CentralMaterialDustDetectionSwitch &&
                _systemVariables.CentralMaterialDustDetectionSwitch == false)
            {
                //悬胶/挡板-中部料斗堵煤
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-中部料斗堵煤", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("悬胶/挡板-中部料斗堵煤", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.CentralMaterialDustDetectionSwitch == false &&
                     _systemVariables.CentralMaterialDustDetectionSwitch == true)
            {
                //悬胶/挡板-中部料斗堵煤解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-中部料斗堵煤解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.DiversionBaffleMotorOverload &&
                _systemVariables.DiversionBaffleMotorOverload == false)
            {
                //分流挡板-电机过载
                DataManager.Instance.InsertHistoryWarningMc("分流挡板-电机过载", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("分流挡板-电机过载", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.DiversionBaffleMotorOverload == false &&
                     _systemVariables.DiversionBaffleMotorOverload == true)
            {
                //分流挡板-电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("分流挡板-电机过载解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }


            if (newSystemVariables.CableReelMotorOverload && _systemVariables.CableReelMotorOverload == false)
            {
                //夹轨/卷筒-电缆卷筒-卷筒电机过载
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒电机过载", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("电缆卷筒-卷筒电机过载", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.CableReelMotorOverload == false &&
                     _systemVariables.CableReelMotorOverload == true)
            {
                //夹轨/卷筒-电缆卷筒-卷筒电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒电机过载解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.ReelOverTensionLimit1 && _systemVariables.ReelOverTensionLimit1 == false)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过紧限位1
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧限位1", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("电缆卷筒-卷筒过紧限位1", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.ReelOverTensionLimit1 == false &&
                     _systemVariables.ReelOverTensionLimit1 == true)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过紧限位1解除
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧限位1解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.ReelOverLooseLimit1 && _systemVariables.ReelOverLooseLimit1 == false)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过松限位1
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松限位1", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("电缆卷筒-卷筒过松限位1", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.ReelOverLooseLimit1 == false && _systemVariables.ReelOverLooseLimit1 == true)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过松限位1解除
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松限位1解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RollerOverTightLimit2 && _systemVariables.RollerOverTightLimit2 == false)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过紧限位2
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧限位2", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("电缆卷筒-卷筒过紧限位2", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RollerOverTightLimit2 == false &&
                     _systemVariables.RollerOverTightLimit2 == true)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过紧限位2解除
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧限位2解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.RollerOverLooseLimit2 && _systemVariables.RollerOverLooseLimit2 == false)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过松限位2
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松限位2", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("电缆卷筒-卷筒过松限位2", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.RollerOverLooseLimit2 == false &&
                     _systemVariables.RollerOverLooseLimit2 == true)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过松限位2解除
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松限位2解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.ReelEmptySwitch && _systemVariables.ReelEmptySwitch == false)
            {
                //夹轨/卷筒-电缆卷筒-卷筒空盘开关
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒空盘开关", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("电缆卷筒-卷筒空盘开关", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.ReelEmptySwitch == false && _systemVariables.ReelEmptySwitch == true)
            {
                //夹轨/卷筒-电缆卷筒-卷筒空盘开关解除
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒空盘开关解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.DryFogSystemLowAirPressure && _systemVariables.DryFogSystemLowAirPressure == false)
            {
                //抑尘振打-洒水抑尘-干雾系统气压低
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统气压低", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("洒水抑尘-干雾系统气压低", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.DryFogSystemLowAirPressure == false &&
                     _systemVariables.DryFogSystemLowAirPressure == true)
            {
                //抑尘振打-洒水抑尘-干雾系统气压低解除
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统气压低解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.DryFogSystemLowWaterPressure &&
                _systemVariables.DryFogSystemLowWaterPressure == false)
            {
                //抑尘振打-洒水抑尘-干雾系统水压低
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统水压低", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("洒水抑尘-干雾系统水压低", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.DryFogSystemLowWaterPressure == false &&
                     _systemVariables.DryFogSystemLowWaterPressure == true)
            {
                //抑尘振打-洒水抑尘-干雾系统水压低解除
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统水压低解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.DryFogSystemFilterClogged && _systemVariables.DryFogSystemFilterClogged == false)
            {
                //抑尘振打-洒水抑尘-干雾系统过滤器堵塞
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统过滤器堵塞", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("洒水抑尘-干雾系统过滤器堵塞", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.DryFogSystemFilterClogged == false &&
                     _systemVariables.DryFogSystemFilterClogged == true)
            {
                //抑尘振打-洒水抑尘-干雾系统过滤器堵塞解除
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统过滤器堵塞解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.WaterTankLowLevelSwitch && _systemVariables.WaterTankLowLevelSwitch == false)
            {
                //抑尘振打-洒水抑尘-水箱液位低开关
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-水箱液位低开关", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("洒水抑尘-水箱液位低开关", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.WaterTankLowLevelSwitch == false &&
                     _systemVariables.WaterTankLowLevelSwitch == true)
            {
                //抑尘振打-洒水抑尘-水箱液位低开关解除
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-水箱液位低开关解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.VibrationMotorOverload && _systemVariables.VibrationMotorOverload == false)
            {
                //抑尘振打-振打电机-振打电机过载
                DataManager.Instance.InsertHistoryWarningMc("振打电机-振打电机过载", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("振打电机-振打电机过载", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.VibrationMotorOverload == false &&
                     _systemVariables.VibrationMotorOverload == true)
            {
                //抑尘振打-振打电机-振打电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("振打电机-振打电机过载解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.ClampingDeviceMotorOverload && _systemVariables.ClampingDeviceMotorOverload == false)
            {
                //夹轨/卷筒-夹轨器-电机过载
                DataManager.Instance.InsertHistoryWarningMc("夹轨器-电机过载", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("夹轨器-电机过载", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.ClampingDeviceMotorOverload == false &&
                     _systemVariables.ClampingDeviceMotorOverload == true)
            {
                //夹轨/卷筒-夹轨器-电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("夹轨器-电机过载解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.ClampFault && _systemVariables.ClampFault == false)
            {
                //夹轨/卷筒-夹轨器故障
                DataManager.Instance.InsertHistoryWarningMc("夹轨器故障", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("夹轨器故障", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.ClampFault == false && _systemVariables.ClampFault == true)
            {
                //夹轨/卷筒-夹轨器故障解除
                DataManager.Instance.InsertHistoryWarningMc("夹轨器故障解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.TailCarDrivenRollerBearingUpperLimitAlarm &&
                _systemVariables.TailCarDrivenRollerBearingUpperLimitAlarm == false)
            {
                //尾车胶带-尾车从动滚筒轴承上限报警
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承上限报警", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("尾车胶带-尾车从动滚筒轴承上限报警", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.TailCarDrivenRollerBearingUpperLimitAlarm == false &&
                     _systemVariables.TailCarDrivenRollerBearingUpperLimitAlarm == true)
            {
                //尾车胶带-尾车从动滚筒轴承上限报警解除
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承上限报警解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.TailCarDrivenRollerBearingLowerLimitAlarm &&
                _systemVariables.TailCarDrivenRollerBearingLowerLimitAlarm == false)
            {
                //尾车胶带-尾车从动滚筒轴承下限报警
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承下限报警", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("尾车胶带-尾车从动滚筒轴承下限报警", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.TailCarDrivenRollerBearingLowerLimitAlarm == false &&
                     _systemVariables.TailCarDrivenRollerBearingLowerLimitAlarm == true)
            {
                //尾车胶带-尾车从动滚筒轴承下限报警解除
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承下限报警解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.TailCarFirstLevelDeviationSwitch &&
                _systemVariables.TailCarFirstLevelDeviationSwitch == false)
            {
                //尾车胶带-尾车一级跑偏开关
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车一级跑偏开关", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("尾车胶带-尾车一级跑偏开关", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.TailCarFirstLevelDeviationSwitch == false &&
                     _systemVariables.TailCarFirstLevelDeviationSwitch == true)
            {
                //尾车胶带-尾车一级跑偏开关解除
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车一级跑偏开关解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.TailCarSecondLevelDeviationSwitch &&
                _systemVariables.TailCarSecondLevelDeviationSwitch == false)
            {
                //尾车胶带-尾车二级跑偏开关
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车二级跑偏开关", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("尾车胶带-尾车二级跑偏开关", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.TailCarSecondLevelDeviationSwitch == false &&
                     _systemVariables.TailCarSecondLevelDeviationSwitch == true)
            {
                //尾车胶带-尾车二级跑偏开关解除
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车二级跑偏开关解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.TailCarEmergencyStopSwitch && _systemVariables.TailCarEmergencyStopSwitch == false)
            {
                //尾车胶带-尾车急停拉线开关
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车急停拉线开关", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("尾车胶带-尾车急停拉线开关", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.TailCarEmergencyStopSwitch == false &&
                     _systemVariables.TailCarEmergencyStopSwitch == true)
            {
                //尾车胶带-尾车急停拉线开关解除
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车急停拉线开关解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }

            if (newSystemVariables.TailCarBeltLongitudinalTearing &&
                _systemVariables.TailCarBeltLongitudinalTearing == false)
            {
                //尾车胶带-尾车胶带纵向撕裂
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车胶带纵向撕裂", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
                AddOrUpdateWarningDesQueue("尾车胶带-尾车胶带纵向撕裂", Machine.BucketWheelStackerReclaimer);
            }
            else if (newSystemVariables.TailCarBeltLongitudinalTearing == false &&
                     _systemVariables.TailCarBeltLongitudinalTearing == true)
            {
                //尾车胶带-尾车胶带纵向撕裂解除
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车胶带纵向撕裂解除", GetUserName(),
                    Machine.BucketWheelStackerReclaimer);
            }
            //333333333333333333

            //取料机
            if (newSystemVariables.DriverRoomEmergencyStopButton_2 &&
                _systemVariables.DriverRoomEmergencyStopButton_2 == false)
            {
                //司机室急停
                DataManager.Instance.InsertHistoryWarningMc("司机室急停", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("司机室急停", Machine.BucketWheel);
            }
            else if (newSystemVariables.DriverRoomEmergencyStopButton_2 == false &&
                     _systemVariables.DriverRoomEmergencyStopButton_2 == true)
            {
                //司机室急停解除
                DataManager.Instance.InsertHistoryWarningMc("司机室急停解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.ElectricalRoomEmergencyStopButton_2 &&
                _systemVariables.ElectricalRoomEmergencyStopButton_2 == false)
            {
                //电气室急停
                DataManager.Instance.InsertHistoryWarningMc("电气室急停", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("电气室急停", Machine.BucketWheel);
            }
            else if (newSystemVariables.ElectricalRoomEmergencyStopButton_2 == false &&
                     _systemVariables.ElectricalRoomEmergencyStopButton_2 == true)
            {
                //电气室急停解除
                DataManager.Instance.InsertHistoryWarningMc("电气室急停解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.EmergencyStopRelay_2 && _systemVariables.EmergencyStopRelay_2 == false)
            {
                //急停继电器
                DataManager.Instance.InsertHistoryWarningMc("急停继电器", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("急停继电器", Machine.BucketWheel);
            }
            else if (newSystemVariables.EmergencyStopRelay_2 == false && _systemVariables.EmergencyStopRelay_2 == true)
            {
                //急停继电器解除
                DataManager.Instance.InsertHistoryWarningMc("急停继电器解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RemoteEmergencyStop_2 && _systemVariables.RemoteEmergencyStop_2 == false)
            {
                //远程急停
                DataManager.Instance.InsertHistoryWarningMc("远程急停", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("远程急停", Machine.BucketWheel);
            }
            else if (newSystemVariables.RemoteEmergencyStop_2 == false &&
                     _systemVariables.RemoteEmergencyStop_2 == true)
            {
                //远程急停解除
                DataManager.Instance.InsertHistoryWarningMc("远程急停解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.BucketWheelFault_2 && _systemVariables.BucketWheelFault_2 == false)
            {
                //斗轮机故障
                DataManager.Instance.InsertHistoryWarningMc("斗轮机故障", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("斗轮机故障", Machine.BucketWheel);
            }
            else if (newSystemVariables.BucketWheelFault_2 == false && _systemVariables.BucketWheelFault_2 == true)
            {
                //斗轮机故障解除
                DataManager.Instance.InsertHistoryWarningMc("斗轮机故障解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.LargeCarFault_2 && _systemVariables.LargeCarFault_2 == false)
            {
                //大车-大车故障
                DataManager.Instance.InsertHistoryWarningMc("大车-大车故障", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("大车-大车故障", Machine.BucketWheel);
            }
            else if (newSystemVariables.LargeCarFault_2 == false && _systemVariables.LargeCarFault_2 == true)
            {
                //大车-大车故障解除
                DataManager.Instance.InsertHistoryWarningMc("大车-大车故障解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.LargeCarFrequencyConverterFault_2 &&
                _systemVariables.LargeCarFrequencyConverterFault_2 == false)
            {
                //大车-变频器故障
                DataManager.Instance.InsertHistoryWarningMc("大车-变频器故障", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("大车-变频器故障", Machine.BucketWheel);
            }
            else if (newSystemVariables.LargeCarFrequencyConverterFault_2 == false &&
                     _systemVariables.LargeCarFrequencyConverterFault_2 == true)
            {
                //大车-变频器故障解除
                DataManager.Instance.InsertHistoryWarningMc("大车-变频器故障解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.LargeCarBrakeResistorOverheatSwitch_2 &&
                _systemVariables.LargeCarBrakeResistorOverheatSwitch_2 == false)
            {
                //大车-制动电阻超温
                DataManager.Instance.InsertHistoryWarningMc("大车-制动电阻超温", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("大车-制动电阻超温", Machine.BucketWheel);
            }
            else if (newSystemVariables.LargeCarBrakeResistorOverheatSwitch_2 == false &&
                     _systemVariables.LargeCarBrakeResistorOverheatSwitch_2 == true)
            {
                //大车-制动电阻超温解除
                DataManager.Instance.InsertHistoryWarningMc("大车-制动电阻超温解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.LargeCarCentralizedLubricationLowOilLevel_2 &&
                _systemVariables.LargeCarCentralizedLubricationLowOilLevel_2 == false)
            {
                //大车-大车集中润滑低油位
                DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑低油位", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("大车-大车集中润滑低油位", Machine.BucketWheel);
            }
            else if (newSystemVariables.LargeCarCentralizedLubricationLowOilLevel_2 == false &&
                     _systemVariables.LargeCarCentralizedLubricationLowOilLevel_2 == true)
            {
                //大车-大车集中润滑低油位解除
                DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑低油位解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.LargeCarCentralizedLubricationOilBlockage_2 &&
                _systemVariables.LargeCarCentralizedLubricationOilBlockage_2 == false)
            {
                //大车-大车集中润滑堵油
                DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑堵油", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("大车-大车集中润滑堵油", Machine.BucketWheel);
            }
            else if (newSystemVariables.LargeCarCentralizedLubricationOilBlockage_2 == false &&
                     _systemVariables.LargeCarCentralizedLubricationOilBlockage_2 == true)
            {
                //大车-大车集中润滑堵油解除
                DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑堵油解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.LargeCarForwardLimit_2 && _systemVariables.LargeCarForwardLimit_2 == false)
            {
                //大车-前进限位
                DataManager.Instance.InsertHistoryWarningMc("大车-前进限位", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("大车-前进限位", Machine.BucketWheel);
            }
            else if (newSystemVariables.LargeCarForwardLimit_2 == false &&
                     _systemVariables.LargeCarForwardLimit_2 == true)
            {
                //大车-前进限位解除
                DataManager.Instance.InsertHistoryWarningMc("大车-前进限位解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.LargeCarForwardExtremeLimit_2 &&
                _systemVariables.LargeCarForwardExtremeLimit_2 == false)
            {
                //大车-前进极限
                DataManager.Instance.InsertHistoryWarningMc("大车-前进极限", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("大车-前进极限", Machine.BucketWheel);
            }
            else if (newSystemVariables.LargeCarForwardExtremeLimit_2 == false &&
                     _systemVariables.LargeCarForwardExtremeLimit_2 == true)
            {
                //大车-前进极限解除
                DataManager.Instance.InsertHistoryWarningMc("大车-前进极限解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.LargeCarReverseLimit_2 && _systemVariables.LargeCarReverseLimit_2 == false)
            {
                //大车-后退限位
                DataManager.Instance.InsertHistoryWarningMc("大车-后退限位", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("大车-后退限位", Machine.BucketWheel);
            }
            else if (newSystemVariables.LargeCarReverseLimit_2 == false &&
                     _systemVariables.LargeCarReverseLimit_2 == true)
            {
                //大车-后退限位解除
                DataManager.Instance.InsertHistoryWarningMc("大车-后退限位解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.LargeCarReverseExtremeLimit_2 &&
                _systemVariables.LargeCarReverseExtremeLimit_2 == false)
            {
                //大车-后退极限
                DataManager.Instance.InsertHistoryWarningMc("大车-后退极限", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("大车-后退极限", Machine.BucketWheel);
            }
            else if (newSystemVariables.LargeCarReverseExtremeLimit_2 == false &&
                     _systemVariables.LargeCarReverseExtremeLimit_2 == true)
            {
                //大车-后退极限解除
                DataManager.Instance.InsertHistoryWarningMc("大车-后退极限解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.TwoMachineCollisionAlarm_2 && _systemVariables.TwoMachineCollisionAlarm_2 == false)
            {
                //大车-两车碰撞报警
                DataManager.Instance.InsertHistoryWarningMc("大车-两车碰撞报警", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("大车-两车碰撞报警", Machine.BucketWheel);
            }
            else if (newSystemVariables.TwoMachineCollisionAlarm_2 == false &&
                     _systemVariables.TwoMachineCollisionAlarm_2 == true)
            {
                //大车-两车碰撞报警解除
                DataManager.Instance.InsertHistoryWarningMc("大车-两车碰撞报警解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.VariableAmplitudeMotorOverload_2 &&
                _systemVariables.VariableAmplitudeMotorOverload_2 == false)
            {
                //变幅-主电机过载
                DataManager.Instance.InsertHistoryWarningMc("变幅-主电机过载", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("变幅-主电机过载", Machine.BucketWheel);
            }
            else if (newSystemVariables.VariableAmplitudeMotorOverload_2 == false &&
                     _systemVariables.VariableAmplitudeMotorOverload_2 == true)
            {
                //变幅-主电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-主电机过载解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.VariableAmplitudeUpperLimit_2 &&
                _systemVariables.VariableAmplitudeUpperLimit_2 == false)
            {
                //变幅-上仰限位
                DataManager.Instance.InsertHistoryWarningMc("变幅-上仰限位", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("变幅-上仰限位", Machine.BucketWheel);
            }
            else if (newSystemVariables.VariableAmplitudeUpperLimit_2 == false &&
                     _systemVariables.VariableAmplitudeUpperLimit_2 == true)
            {
                //变幅-上仰限位解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-上仰限位解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.VariableAmplitudeUpperExtremeLimit_2 &&
                _systemVariables.VariableAmplitudeUpperExtremeLimit_2 == false)
            {
                //变幅-上仰极限
                DataManager.Instance.InsertHistoryWarningMc("变幅-上仰极限", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("变幅-上仰极限", Machine.BucketWheel);
            }
            else if (newSystemVariables.VariableAmplitudeUpperExtremeLimit_2 == false &&
                     _systemVariables.VariableAmplitudeUpperExtremeLimit_2 == true)
            {
                //变幅-上仰极限解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-上仰极限解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.VariableAmplitudeLowerLimit_2 &&
                _systemVariables.VariableAmplitudeLowerLimit_2 == false)
            {
                //变幅-下俯限位
                DataManager.Instance.InsertHistoryWarningMc("变幅-下俯限位", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("变幅-下俯限位", Machine.BucketWheel);
            }
            else if (newSystemVariables.VariableAmplitudeLowerLimit_2 == false &&
                     _systemVariables.VariableAmplitudeLowerLimit_2 == true)
            {
                //变幅-下俯限位解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-下俯限位解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.VariableAmplitudeLowerExtremeLimit_2 &&
                _systemVariables.VariableAmplitudeLowerExtremeLimit_2 == false)
            {
                //变幅-下俯极限
                DataManager.Instance.InsertHistoryWarningMc("变幅-下俯极限", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("变幅-下俯极限", Machine.BucketWheel);
            }
            else if (newSystemVariables.VariableAmplitudeLowerExtremeLimit_2 == false &&
                     _systemVariables.VariableAmplitudeLowerExtremeLimit_2 == true)
            {
                //变幅-下俯极限解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-下俯极限解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.VariableAmplitudeLowerForbiddenZoneLimit_2 &&
                _systemVariables.VariableAmplitudeLowerForbiddenZoneLimit_2 == false)
            {
                //变幅-下俯禁区
                DataManager.Instance.InsertHistoryWarningMc("变幅-下俯禁区", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("变幅-下俯禁区", Machine.BucketWheel);
            }
            else if (newSystemVariables.VariableAmplitudeLowerForbiddenZoneLimit_2 == false &&
                     _systemVariables.VariableAmplitudeLowerForbiddenZoneLimit_2 == true)
            {
                //变幅-下俯禁区解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-下俯禁区解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.VariableAmplitudePumpStationOverheatAlarm_2 &&
                _systemVariables.VariableAmplitudePumpStationOverheatAlarm_2 == false)
            {
                //变幅-泵站高温报警
                DataManager.Instance.InsertHistoryWarningMc("变幅-泵站高温报警", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("变幅-泵站高温报警", Machine.BucketWheel);
            }
            else if (newSystemVariables.VariableAmplitudePumpStationOverheatAlarm_2 == false &&
                     _systemVariables.VariableAmplitudePumpStationOverheatAlarm_2 == true)
            {
                //变幅-泵站高温报警解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-泵站高温报警解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.VariableAmplitudeOilLevelVeryLowSignal_2 &&
                _systemVariables.VariableAmplitudeOilLevelVeryLowSignal_2 == false)
            {
                //变幅-油液位低信号
                DataManager.Instance.InsertHistoryWarningMc("变幅-油液位低信号", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("变幅-油液位低信号", Machine.BucketWheel);
            }
            else if (newSystemVariables.VariableAmplitudeOilLevelVeryLowSignal_2 == false &&
                     _systemVariables.VariableAmplitudeOilLevelVeryLowSignal_2 == true)
            {
                //变幅-油液位低信号解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-油液位低信号解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.VariableAmplitudeOilLevelLowSignal_2 &&
                _systemVariables.VariableAmplitudeOilLevelLowSignal_2 == false)
            {
                //变幅-液位超低信号
                DataManager.Instance.InsertHistoryWarningMc("变幅-液位超低信号", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("变幅-液位超低信号", Machine.BucketWheel);
            }
            else if (newSystemVariables.VariableAmplitudeOilLevelLowSignal_2 == false &&
                     _systemVariables.VariableAmplitudeOilLevelLowSignal_2 == true)
            {
                //变幅-液位超低信号解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-液位超低信号解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.VariableFrequencyOilBlockageSignal_2 &&
                _systemVariables.VariableFrequencyOilBlockageSignal_2 == false)
            {
                //变幅-泵站堵油信号
                DataManager.Instance.InsertHistoryWarningMc("变幅-泵站堵油信号", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("变幅-泵站堵油信号", Machine.BucketWheel);
            }
            else if (newSystemVariables.VariableFrequencyOilBlockageSignal_2 == false &&
                     _systemVariables.VariableFrequencyOilBlockageSignal_2 == true)
            {
                //变幅-泵站堵油信号解除
                DataManager.Instance.InsertHistoryWarningMc("变幅-泵站堵油信号解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RotaryFrequencyConverterFault_2 &&
                _systemVariables.RotaryFrequencyConverterFault_2 == false)
            {
                //回转-变频器故障
                DataManager.Instance.InsertHistoryWarningMc("回转-变频器故障", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("回转-变频器故障", Machine.BucketWheel);
            }
            else if (newSystemVariables.RotaryFrequencyConverterFault_2 == false &&
                     _systemVariables.RotaryFrequencyConverterFault_2 == true)
            {
                //回转-变频器故障解除
                DataManager.Instance.InsertHistoryWarningMc("回转-变频器故障解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RotaryBrakeOverload_2 && _systemVariables.RotaryBrakeOverload_2 == false)
            {
                //回转-制动器过载
                DataManager.Instance.InsertHistoryWarningMc("回转-制动器过载", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("回转-制动器过载", Machine.BucketWheel);
            }
            else if (newSystemVariables.RotaryBrakeOverload_2 == false &&
                     _systemVariables.RotaryBrakeOverload_2 == true)
            {
                //回转-制动器过载解除
                DataManager.Instance.InsertHistoryWarningMc("回转-制动器过载解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RotaryFanOverload_2 && _systemVariables.RotaryFanOverload_2 == false)
            {
                //回转-风机过载
                DataManager.Instance.InsertHistoryWarningMc("回转-风机过载", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("回转-风机过载", Machine.BucketWheel);
            }
            else if (newSystemVariables.RotaryFanOverload_2 == false && _systemVariables.RotaryFanOverload_2 == true)
            {
                //回转-风机过载解除
                DataManager.Instance.InsertHistoryWarningMc("回转-风机过载解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RotaryBrakeResistorOverheatSwitch_2 &&
                _systemVariables.RotaryBrakeResistorOverheatSwitch_2 == false)
            {
                //回转-制动电阻超温
                DataManager.Instance.InsertHistoryWarningMc("回转-制动电阻超温", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("回转-制动电阻超温", Machine.BucketWheel);
            }
            else if (newSystemVariables.RotaryBrakeResistorOverheatSwitch_2 == false &&
                     _systemVariables.RotaryBrakeResistorOverheatSwitch_2 == true)
            {
                //回转-制动电阻超温解除
                DataManager.Instance.InsertHistoryWarningMc("回转-制动电阻超温解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RotaryFault_2 && _systemVariables.RotaryFault_2 == false)
            {
                //回转-回转故障
                DataManager.Instance.InsertHistoryWarningMc("回转-回转故障", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("回转-回转故障", Machine.BucketWheel);
            }
            else if (newSystemVariables.RotaryFault_2 == false && _systemVariables.RotaryFault_2 == true)
            {
                //回转-回转故障解除
                DataManager.Instance.InsertHistoryWarningMc("回转-回转故障解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RotaryLeftTurnLimit_2 && _systemVariables.RotaryLeftTurnLimit_2 == false)
            {
                //回转-左转限位
                DataManager.Instance.InsertHistoryWarningMc("回转-左转限位", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("回转-左转限位", Machine.BucketWheel);
            }
            else if (newSystemVariables.RotaryLeftTurnLimit_2 == false &&
                     _systemVariables.RotaryLeftTurnLimit_2 == true)
            {
                //回转-左转限位解除
                DataManager.Instance.InsertHistoryWarningMc("回转-左转限位解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RotaryLeftTurnExtremeLimit_2 &&
                _systemVariables.RotaryLeftTurnExtremeLimit_2 == false)
            {
                //回转-左转极限
                DataManager.Instance.InsertHistoryWarningMc("回转-左转极限", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("回转-左转极限", Machine.BucketWheel);
            }
            else if (newSystemVariables.RotaryLeftTurnExtremeLimit_2 == false &&
                     _systemVariables.RotaryLeftTurnExtremeLimit_2 == true)
            {
                //回转-左转极限解除
                DataManager.Instance.InsertHistoryWarningMc("回转-左转极限解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RotaryLeftTurnForbiddenZoneLimit_2 &&
                _systemVariables.RotaryLeftTurnForbiddenZoneLimit_2 == false)
            {
                //回转-左转禁区限位
                DataManager.Instance.InsertHistoryWarningMc("回转-左转禁区限位", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("回转-左转禁区限位", Machine.BucketWheel);
            }
            else if (newSystemVariables.RotaryLeftTurnForbiddenZoneLimit_2 == false &&
                     _systemVariables.RotaryLeftTurnForbiddenZoneLimit_2 == true)
            {
                //回转-左转禁区限位解除
                DataManager.Instance.InsertHistoryWarningMc("回转-左转禁区限位解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RotaryRightTurnLimit_2 && _systemVariables.RotaryRightTurnLimit_2 == false)
            {
                //回转-右转限位
                DataManager.Instance.InsertHistoryWarningMc("回转-右转限位", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("回转-右转限位", Machine.BucketWheel);
            }
            else if (newSystemVariables.RotaryRightTurnLimit_2 == false &&
                     _systemVariables.RotaryRightTurnLimit_2 == true)
            {
                //回转-右转限位解除
                DataManager.Instance.InsertHistoryWarningMc("回转-右转限位解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RotaryRightTurnExtremeLimit_2 &&
                _systemVariables.RotaryRightTurnExtremeLimit_2 == false)
            {
                //回转-右转极限
                DataManager.Instance.InsertHistoryWarningMc("回转-右转极限", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("回转-右转极限", Machine.BucketWheel);
            }
            else if (newSystemVariables.RotaryRightTurnExtremeLimit_2 == false &&
                     _systemVariables.RotaryRightTurnExtremeLimit_2 == true)
            {
                //回转-右转极限解除
                DataManager.Instance.InsertHistoryWarningMc("回转-右转极限解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RotaryRightTurnForbiddenZoneLimit_2 &&
                _systemVariables.RotaryRightTurnForbiddenZoneLimit_2 == false)
            {
                //回转-右转禁区限位
                DataManager.Instance.InsertHistoryWarningMc("回转-右转禁区限位", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("回转-右转禁区限位", Machine.BucketWheel);
            }
            else if (newSystemVariables.RotaryRightTurnForbiddenZoneLimit_2 == false &&
                     _systemVariables.RotaryRightTurnForbiddenZoneLimit_2 == true)
            {
                //回转-右转禁区限位解除
                DataManager.Instance.InsertHistoryWarningMc("回转-右转禁区限位解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RotaryRightTurnForbiddenLimit_2 &&
                _systemVariables.RotaryRightTurnForbiddenLimit_2 == false)
            {
                //回转-右转防撞限位
                DataManager.Instance.InsertHistoryWarningMc("回转-右转防撞限位", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("回转-右转防撞限位", Machine.BucketWheel);
            }
            else if (newSystemVariables.RotaryRightTurnForbiddenLimit_2 == false &&
                     _systemVariables.RotaryRightTurnForbiddenLimit_2 == true)
            {
                //回转-右转防撞限位解除
                DataManager.Instance.InsertHistoryWarningMc("回转-右转防撞限位解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RotaryOverTorque_2 && _systemVariables.RotaryOverTorque_2 == false)
            {
                //回转-回转过力矩
                DataManager.Instance.InsertHistoryWarningMc("回转-回转过力矩", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("回转-回转过力矩", Machine.BucketWheel);
            }
            else if (newSystemVariables.RotaryOverTorque_2 == false && _systemVariables.RotaryOverTorque_2 == true)
            {
                //回转-回转过力矩解除
                DataManager.Instance.InsertHistoryWarningMc("回转-回转过力矩解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RotaryCentralizedLubricationOilBlockageFault_2 &&
                _systemVariables.RotaryCentralizedLubricationOilBlockageFault_2 == false)
            {
                //回转-回转集中润滑堵油
                DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑堵油", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("回转-回转集中润滑堵油", Machine.BucketWheel);
            }
            else if (newSystemVariables.RotaryCentralizedLubricationOilBlockageFault_2 == false &&
                     _systemVariables.RotaryCentralizedLubricationOilBlockageFault_2 == true)
            {
                //回转-回转集中润滑堵油解除
                DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑堵油解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RotaryCentralizedLubricationLowOilLevelFault_2 &&
                _systemVariables.RotaryCentralizedLubricationLowOilLevelFault_2 == false)
            {
                //回转-回转集中润滑低油位
                DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑低油位", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("回转-回转集中润滑低油位", Machine.BucketWheel);
            }
            else if (newSystemVariables.RotaryCentralizedLubricationLowOilLevelFault_2 == false &&
                     _systemVariables.RotaryCentralizedLubricationLowOilLevelFault_2 == true)
            {
                //回转-回转集中润滑低油位解除
                DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑低油位解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.BucketWheelMotorOverload_2 && _systemVariables.BucketWheelMotorOverload_2 == false)
            {
                //斗轮/槽-电机过载
                DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-电机过载", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("斗轮/槽-电机过载", Machine.BucketWheel);
            }
            else if (newSystemVariables.BucketWheelMotorOverload_2 == false &&
                     _systemVariables.BucketWheelMotorOverload_2 == true)
            {
                //斗轮/槽-电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-电机过载解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.BucketWheelOverTorqueSwitch_2 &&
                _systemVariables.BucketWheelOverTorqueSwitch_2 == false)
            {
                //斗轮/槽-斗轮过力矩开关
                DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-斗轮过力矩开关", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("斗轮/槽-斗轮过力矩开关", Machine.BucketWheel);
            }
            else if (newSystemVariables.BucketWheelOverTorqueSwitch_2 == false &&
                     _systemVariables.BucketWheelOverTorqueSwitch_2 == true)
            {
                //斗轮/槽-斗轮过力矩开关解除
                DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-斗轮过力矩开关解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.BucketWheelSlotMotorOverload_2 &&
                _systemVariables.BucketWheelSlotMotorOverload_2 == false)
            {
                //斗轮导料槽-电机过载
                DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽-电机过载", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("斗轮导料槽-电机过载", Machine.BucketWheel);
            }
            else if (newSystemVariables.BucketWheelSlotMotorOverload_2 == false &&
                     _systemVariables.BucketWheelSlotMotorOverload_2 == true)
            {
                //斗轮导料槽-电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽-电机过载解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.SuspensionBeltMotorOverload_2 &&
                _systemVariables.SuspensionBeltMotorOverload_2 == false)
            {
                //悬胶/挡板-电机过载
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-电机过载", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("悬胶/挡板-电机过载", Machine.BucketWheel);
            }
            else if (newSystemVariables.SuspensionBeltMotorOverload_2 == false &&
                     _systemVariables.SuspensionBeltMotorOverload_2 == true)
            {
                //悬胶/挡板-电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-电机过载解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.SuspensionBeltFirstLevelDeviationSwitch_2 &&
                _systemVariables.SuspensionBeltFirstLevelDeviationSwitch_2 == false)
            {
                //悬胶/挡板-一级跑偏开关
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-一级跑偏开关", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("悬胶/挡板-一级跑偏开关", Machine.BucketWheel);
            }
            else if (newSystemVariables.SuspensionBeltFirstLevelDeviationSwitch_2 == false &&
                     _systemVariables.SuspensionBeltFirstLevelDeviationSwitch_2 == true)
            {
                //悬胶/挡板-一级跑偏开关解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-一级跑偏开关解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.SuspensionBeltSecondLevelDeviationSwitch_2 &&
                _systemVariables.SuspensionBeltSecondLevelDeviationSwitch_2 == false)
            {
                //悬胶/挡板-二级跑偏开关
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-二级跑偏开关", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("悬胶/挡板-二级跑偏开关", Machine.BucketWheel);
            }
            else if (newSystemVariables.SuspensionBeltSecondLevelDeviationSwitch_2 == false &&
                     _systemVariables.SuspensionBeltSecondLevelDeviationSwitch_2 == true)
            {
                //悬胶/挡板-二级跑偏开关解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-二级跑偏开关解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.SuspendedBeltSlip_2 && _systemVariables.SuspendedBeltSlip_2 == false)
            {
                //悬胶/挡板-打滑检测开关
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-打滑检测开关", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("悬胶/挡板-打滑检测开关", Machine.BucketWheel);
            }
            else if (newSystemVariables.SuspendedBeltSlip_2 == false && _systemVariables.SuspendedBeltSlip_2 == true)
            {
                //悬胶/挡板-打滑检测开关解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-打滑检测开关解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.SuspensionBeltLongitudinalTearSwitch_2 &&
                _systemVariables.SuspensionBeltLongitudinalTearSwitch_2 == false)
            {
                //悬胶/挡板-纵向撕裂开关
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-纵向撕裂开关", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("悬胶/挡板-纵向撕裂开关", Machine.BucketWheel);
            }
            else if (newSystemVariables.SuspensionBeltLongitudinalTearSwitch_2 == false &&
                     _systemVariables.SuspensionBeltLongitudinalTearSwitch_2 == true)
            {
                //悬胶/挡板-纵向撕裂开关解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-纵向撕裂开关解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.SuspensionBeltEmergencyStopSwitch_2 &&
                _systemVariables.SuspensionBeltEmergencyStopSwitch_2 == false)
            {
                //悬胶/挡板-急停拉线开关
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-急停拉线开关", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("悬胶/挡板-急停拉线开关", Machine.BucketWheel);
            }
            else if (newSystemVariables.SuspensionBeltEmergencyStopSwitch_2 == false &&
                     _systemVariables.SuspensionBeltEmergencyStopSwitch_2 == true)
            {
                //悬胶/挡板-急停拉线开关解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-急停拉线开关解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.SuspensionBeltMaterialFlowDetectionSwitch_2 &&
                _systemVariables.SuspensionBeltMaterialFlowDetectionSwitch_2 == false)
            {
                //悬胶/挡板-料流检测开关
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-料流检测开关", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("悬胶/挡板-料流检测开关", Machine.BucketWheel);
            }
            else if (newSystemVariables.SuspensionBeltMaterialFlowDetectionSwitch_2 == false &&
                     _systemVariables.SuspensionBeltMaterialFlowDetectionSwitch_2 == true)
            {
                //悬胶/挡板-料流检测开关解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-料流检测开关解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.CentralMaterialDustDetectionSwitch_2 &&
                _systemVariables.CentralMaterialDustDetectionSwitch_2 == false)
            {
                //悬胶/挡板-中部料斗堵煤
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-中部料斗堵煤", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("悬胶/挡板-中部料斗堵煤", Machine.BucketWheel);
            }
            else if (newSystemVariables.CentralMaterialDustDetectionSwitch_2 == false &&
                     _systemVariables.CentralMaterialDustDetectionSwitch_2 == true)
            {
                //悬胶/挡板-中部料斗堵煤解除
                DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-中部料斗堵煤解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.DiversionBaffleMotorOverload_2 &&
                _systemVariables.DiversionBaffleMotorOverload_2 == false)
            {
                //分流挡板-电机过载
                DataManager.Instance.InsertHistoryWarningMc("分流挡板-电机过载", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("分流挡板-电机过载", Machine.BucketWheel);
            }
            else if (newSystemVariables.DiversionBaffleMotorOverload_2 == false &&
                     _systemVariables.DiversionBaffleMotorOverload_2 == true)
            {
                //分流挡板-电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("分流挡板-电机过载解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.CableReelMotorOverload_2 && _systemVariables.CableReelMotorOverload_2 == false)
            {
                //夹轨/卷筒-电缆卷筒-卷筒电机过载
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒电机过载", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("电缆卷筒-卷筒电机过载", Machine.BucketWheel);
            }
            else if (newSystemVariables.CableReelMotorOverload_2 == false &&
                     _systemVariables.CableReelMotorOverload_2 == true)
            {
                //夹轨/卷筒-电缆卷筒-卷筒电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒电机过载解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.ReelOverTensionLimit1_2 && _systemVariables.ReelOverTensionLimit1_2 == false)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过紧限位1
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧限位1", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("电缆卷筒-卷筒过紧限位1", Machine.BucketWheel);
            }
            else if (newSystemVariables.ReelOverTensionLimit1_2 == false &&
                     _systemVariables.ReelOverTensionLimit1_2 == true)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过紧限位1解除
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧限位1解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.ReelOverLooseLimit1_2 && _systemVariables.ReelOverLooseLimit1_2 == false)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过松限位1
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松限位1", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("电缆卷筒-卷筒过松限位1", Machine.BucketWheel);
            }
            else if (newSystemVariables.ReelOverLooseLimit1_2 == false &&
                     _systemVariables.ReelOverLooseLimit1_2 == true)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过松限位1解除
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松限位1解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RollerOverTightLimit2_2 && _systemVariables.RollerOverTightLimit2_2 == false)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过紧限位2
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧限位2", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("电缆卷筒-卷筒过紧限位2", Machine.BucketWheel);
            }
            else if (newSystemVariables.RollerOverTightLimit2_2 == false &&
                     _systemVariables.RollerOverTightLimit2_2 == true)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过紧限位2解除
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧限位2解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.RollerOverLooseLimit2_2 && _systemVariables.RollerOverLooseLimit2_2 == false)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过松限位2
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松限位2", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("电缆卷筒-卷筒过松限位2", Machine.BucketWheel);
            }
            else if (newSystemVariables.RollerOverLooseLimit2_2 == false &&
                     _systemVariables.RollerOverLooseLimit2_2 == true)
            {
                //夹轨/卷筒-电缆卷筒-卷筒过松限位2解除
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松限位2解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.ReelEmptySwitch_2 && _systemVariables.ReelEmptySwitch_2 == false)
            {
                //夹轨/卷筒-电缆卷筒-卷筒空盘开关
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒空盘开关", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("电缆卷筒-卷筒空盘开关", Machine.BucketWheel);
            }
            else if (newSystemVariables.ReelEmptySwitch_2 == false && _systemVariables.ReelEmptySwitch_2 == true)
            {
                //夹轨/卷筒-电缆卷筒-卷筒空盘开关解除
                DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒空盘开关解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.DryFogSystemLowAirPressure_2 &&
                _systemVariables.DryFogSystemLowAirPressure_2 == false)
            {
                //抑尘振打-洒水抑尘-干雾系统气压低
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统气压低", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("洒水抑尘-干雾系统气压低", Machine.BucketWheel);
            }
            else if (newSystemVariables.DryFogSystemLowAirPressure_2 == false &&
                     _systemVariables.DryFogSystemLowAirPressure_2 == true)
            {
                //抑尘振打-洒水抑尘-干雾系统气压低解除
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统气压低解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.DryFogSystemLowWaterPressure_2 &&
                _systemVariables.DryFogSystemLowWaterPressure_2 == false)
            {
                //抑尘振打-洒水抑尘-干雾系统水压低
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统水压低", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("洒水抑尘-干雾系统水压低", Machine.BucketWheel);
            }
            else if (newSystemVariables.DryFogSystemLowWaterPressure_2 == false &&
                     _systemVariables.DryFogSystemLowWaterPressure_2 == true)
            {
                //抑尘振打-洒水抑尘-干雾系统水压低解除
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统水压低解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.DryFogSystemFilterClogged_2 && _systemVariables.DryFogSystemFilterClogged_2 == false)
            {
                //抑尘振打-洒水抑尘-干雾系统过滤器堵塞
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统过滤器堵塞", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("洒水抑尘-干雾系统过滤器堵塞", Machine.BucketWheel);
            }
            else if (newSystemVariables.DryFogSystemFilterClogged_2 == false &&
                     _systemVariables.DryFogSystemFilterClogged_2 == true)
            {
                //抑尘振打-洒水抑尘-干雾系统过滤器堵塞解除
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统过滤器堵塞解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.WaterTankLowLevelSwitch_2 && _systemVariables.WaterTankLowLevelSwitch_2 == false)
            {
                //抑尘振打-洒水抑尘-水箱液位低开关
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-水箱液位低开关", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("洒水抑尘-水箱液位低开关", Machine.BucketWheel);
            }
            else if (newSystemVariables.WaterTankLowLevelSwitch_2 == false &&
                     _systemVariables.WaterTankLowLevelSwitch_2 == true)
            {
                //抑尘振打-洒水抑尘-水箱液位低开关解除
                DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-水箱液位低开关解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.VibrationMotorOverload_2 && _systemVariables.VibrationMotorOverload_2 == false)
            {
                //抑尘振打-振打电机-振打电机过载
                DataManager.Instance.InsertHistoryWarningMc("振打电机-振打电机过载", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("振打电机-振打电机过载", Machine.BucketWheel);
            }
            else if (newSystemVariables.VibrationMotorOverload_2 == false &&
                     _systemVariables.VibrationMotorOverload_2 == true)
            {
                //抑尘振打-振打电机-振打电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("振打电机-振打电机过载解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.ClampingDeviceMotorOverload_2 &&
                _systemVariables.ClampingDeviceMotorOverload_2 == false)
            {
                //夹轨/卷筒-夹轨器-电机过载
                DataManager.Instance.InsertHistoryWarningMc("夹轨器-电机过载", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("夹轨器-电机过载", Machine.BucketWheel);
            }
            else if (newSystemVariables.ClampingDeviceMotorOverload_2 == false &&
                     _systemVariables.ClampingDeviceMotorOverload_2 == true)
            {
                //夹轨/卷筒-夹轨器-电机过载解除
                DataManager.Instance.InsertHistoryWarningMc("夹轨器-电机过载解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.ClampFault_2 && _systemVariables.ClampFault_2 == false)
            {
                //夹轨/卷筒-夹轨器故障
                DataManager.Instance.InsertHistoryWarningMc("夹轨器故障", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("夹轨器故障", Machine.BucketWheel);
            }
            else if (newSystemVariables.ClampFault_2 == false && _systemVariables.ClampFault_2 == true)
            {
                //夹轨/卷筒-夹轨器故障解除
                DataManager.Instance.InsertHistoryWarningMc("夹轨器故障解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.TailCarDrivenRollerBearingUpperLimitAlarm_2 &&
                _systemVariables.TailCarDrivenRollerBearingUpperLimitAlarm_2 == false)
            {
                //尾车胶带-尾车从动滚筒轴承上限报警
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承上限报警", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("尾车胶带-尾车从动滚筒轴承上限报警", Machine.BucketWheel);
            }
            else if (newSystemVariables.TailCarDrivenRollerBearingUpperLimitAlarm_2 == false &&
                     _systemVariables.TailCarDrivenRollerBearingUpperLimitAlarm_2 == true)
            {
                //尾车胶带-尾车从动滚筒轴承上限报警解除
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承上限报警解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.TailCarDrivenRollerBearingLowerLimitAlarm_2 &&
                _systemVariables.TailCarDrivenRollerBearingLowerLimitAlarm_2 == false)
            {
                //尾车胶带-尾车从动滚筒轴承下限报警
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承下限报警", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("尾车胶带-尾车从动滚筒轴承下限报警", Machine.BucketWheel);
            }
            else if (newSystemVariables.TailCarDrivenRollerBearingLowerLimitAlarm_2 == false &&
                     _systemVariables.TailCarDrivenRollerBearingLowerLimitAlarm_2 == true)
            {
                //尾车胶带-尾车从动滚筒轴承下限报警解除
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承下限报警解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.TailCarFirstLevelDeviationSwitch_2 &&
                _systemVariables.TailCarFirstLevelDeviationSwitch_2 == false)
            {
                //尾车胶带-尾车一级跑偏开关
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车一级跑偏开关", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("尾车胶带-尾车一级跑偏开关", Machine.BucketWheel);
            }
            else if (newSystemVariables.TailCarFirstLevelDeviationSwitch_2 == false &&
                     _systemVariables.TailCarFirstLevelDeviationSwitch_2 == true)
            {
                //尾车胶带-尾车一级跑偏开关解除
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车一级跑偏开关解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.TailCarSecondLevelDeviationSwitch_2 &&
                _systemVariables.TailCarSecondLevelDeviationSwitch_2 == false)
            {
                //尾车胶带-尾车二级跑偏开关
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车二级跑偏开关", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("尾车胶带-尾车二级跑偏开关", Machine.BucketWheel);
            }
            else if (newSystemVariables.TailCarSecondLevelDeviationSwitch_2 == false &&
                     _systemVariables.TailCarSecondLevelDeviationSwitch_2 == true)
            {
                //尾车胶带-尾车二级跑偏开关解除
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车二级跑偏开关解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.TailCarEmergencyStopSwitch_2 &&
                _systemVariables.TailCarEmergencyStopSwitch_2 == false)
            {
                //尾车胶带-尾车急停拉线开关
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车急停拉线开关", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("尾车胶带-尾车急停拉线开关", Machine.BucketWheel);
            }
            else if (newSystemVariables.TailCarEmergencyStopSwitch_2 == false &&
                     _systemVariables.TailCarEmergencyStopSwitch_2 == true)
            {
                //尾车胶带-尾车急停拉线开关解除
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车急停拉线开关解除", GetUserName(),
                    Machine.BucketWheel);
            }

            if (newSystemVariables.TailCarBeltLongitudinalTearing_2 &&
                _systemVariables.TailCarBeltLongitudinalTearing_2 == false)
            {
                //尾车胶带-尾车胶带纵向撕裂
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车胶带纵向撕裂", GetUserName(),
                    Machine.BucketWheel);
                AddOrUpdateWarningDesQueue("尾车胶带-尾车胶带纵向撕裂", Machine.BucketWheel);
            }
            else if (newSystemVariables.TailCarBeltLongitudinalTearing_2 == false &&
                     _systemVariables.TailCarBeltLongitudinalTearing_2 == true)
            {
                //尾车胶带-尾车胶带纵向撕裂解除
                DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车胶带纵向撕裂解除", GetUserName(),
                    Machine.BucketWheel);
            }
            //333333333333333333
        }
    }
}