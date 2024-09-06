using System;
using System.Text;
using System.Threading.Tasks;
using RemoteControl;
using RemoteControl.Event;
using ShangHaiPro;
using ShenYangRemoteSystem.Subclass;
using UnityEngine;
using UnityEngine.Rendering;

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
    private int count;

    public bool RcConnectionState
    {
        get => _rcConnectionState;
        set => _rcConnectionState = value;
    }

    public SystemVariables SystemVariables
    {
        get => _systemVariables;
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

        return "";
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
        _systemVariables = systemVariables;
        _rcConnectionState = _systemVariables.D1PLC1CommunicationState;
        UpdateMachinePosAndRot();
        EventManager.Instance.TriggerEvent(EventName.UpdateRcData, null);
        UpdateDirectionBucketWheel();
        UpdateDirectionBucketWheelStackerReclaimer();
        UpdateMachineWarning();
        ++count;
        if (++count>5)
        {
            count = 0;
            RecordChart();
        }
    }

    public void SetScaReportAndDem(SendDataReportAndDEM sendDataReportAndDEM)
    {
        _sendDataReportAndDem = sendDataReportAndDEM;
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

    public void UpdateMachinePosAndRot()
    {
        if (machineMove_1)
        {
            machineMove_1.UpdatePosAndRotaionByMeter(SystemVariables.DC_Pos, SystemVariables.SLEW_Angle,
                -SystemVariables.Luff_Angle); //
        }

        if (machineMove_2)
        {
            machineMove_2.UpdatePosAndRotaionByMeter(SystemVariables.DC_Pos_2+64.34f, SystemVariables.SLEW_Angle_2,
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
            error = _systemVariables.VariableAmplitudeLowerLimit ? error + "变幅下附限位\n" : error;
            error = _systemVariables.VariableAmplitudeUpperExtremeLimit_2 ? error + "变幅上仰极限\n" : error;
            error = _systemVariables.VariableAmplitudeLowerExtremeLimit_2 ? error + "变幅下附极限\n" : error;

            if (error!="")
            {
                machineMove_1.UpdateErrorText(error);
            }
          
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
            error = _systemVariables.VariableAmplitudeLowerLimit_2 ? error + "变幅下附限位\n" : error;
            error = _systemVariables.VariableAmplitudeUpperExtremeLimit_2 ? error + "变幅上仰极限\n" : error;
            error = _systemVariables.VariableAmplitudeLowerExtremeLimit_2 ? error + "变幅下附极限\n" : error;

            if (error!="")
            {
                machineMove_2.UpdateErrorText(error);
            }
            
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

    public async Task DeSerializeScaJson(string json)
    {
        SendDataReportAndDEM cursendDataReportAndDem=new SendDataReportAndDEM();
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

    /// <summary>
    /// 获取任务当前状态
    /// </summary>
    public void UpdatePcData()
    {
        TaskCommand taskCommand = new TaskCommand();
        taskCommand.QuerySystem = "MC";
        taskCommand.Command_Type = 1;
        MessageCenter.Instance.SendMessage(MessageType.PC, taskCommand);
        Debug.Log("获取当前任务状态");
    }

    public void UpdateSCAData(int query_type)
    {
        Debug.LogError("堆料模型更新");
        SystemCommand serverCommand = new SystemCommand();
        serverCommand.QUERY_SYSTEM = "MC";
        serverCommand.DATA_TYPE = 3;
        serverCommand.QUERY_TYPE = query_type;
        MessageCenter.Instance.SendMessage(MessageType.SCA, serverCommand);
    }

    public void RecordChart()
    {
        DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC,_systemVariables.BucketWheelElectricCurrent, "斗轮电流",
            Machine.BucketWheelStackerReclaimer);
        
        DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC, _systemVariables.LargeCarElectricCurrent, "大车电流",
            Machine.BucketWheelStackerReclaimer);
        
        DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC, _systemVariables.RotaryElectricCurrent, "回转电流",
            Machine.BucketWheelStackerReclaimer);
        
        DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC, _systemVariables.SuspensionBeltElectricCurrent, "悬胶电流",
            Machine.BucketWheelStackerReclaimer);
        
        // DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC, _systemVariables, "悬臂流量",
        //     Machine.BucketWheelStackerReclaimer);
        
        DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC,_systemVariables.BucketWheelElectricCurrent_2, "斗轮电流",
            Machine.BucketWheel);
        
        DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC, _systemVariables.LargeCarElectricCurrent_2, "大车电流",
            Machine.BucketWheel);
        
        DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC, _systemVariables.RotaryElectricCurrent_2, "回转电流",
            Machine.BucketWheel);
        
        DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC, _systemVariables.SuspensionBeltElectricCurrent_2, "悬胶电流",
            Machine.BucketWheel);
        
        // DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC, _systemVariables, "悬臂流量",
        //     Machine.BucketWheelStackerReclaimer);
        
    }
    
    public void DeleteThreeMonthData() //删除电流表 日志表 和告警表三个月前的数据 -ljz
    {
        bool A=DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_LOG1_MC);
        bool B=DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_LOG2_MC);
        Debug.LogError($"删除成功{A} {B}");
        DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_WARNING1_MC);
        DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_WARNING2_MC);
        
        DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC);
        DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC);
        
        DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC);
        DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC);
        
        DataManager.Instance.DeleTabData(ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC);
    }
}