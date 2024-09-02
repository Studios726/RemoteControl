using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
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
            machineMove_2.UpdatePosAndRotaionByMeter(SystemVariables.DC_Pos_2, SystemVariables.SLEW_Angle_2,
                -SystemVariables.Luff_Angle_2); //
        }
    }

    public void UpdateDirectionBucketWheel()
    {
        ModelDirection[] direction_2 = new ModelDirection[2];
        int index = 0;
        if (_systemVariables.LargeCarForwardCommand_2)
        {
            //大车前进
            direction_2[index++] = ModelDirection.Forward;
        }else if (_systemVariables.LargeCarReverseCommand_2)
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
        }else if (_systemVariables.RotaryRightTurnCommand_2)
        {
            //大车右转
            direction_2[index++] = ModelDirection.Right;
        }
        else
        {
            direction_2[index++] = ModelDirection.LrStop;
        }
        EventManager.Instance.TriggerEvent(EventName.UpdateModelDirection,null,new UpdateModelDirectionEventArgs(direction_2,Machine.BucketWheel));
    }
    public void UpdateDirectionBucketWheelStackerReclaimer()
    {
        ModelDirection[] direction = new ModelDirection[2];
        int index = 0;
        if (_systemVariables.LargeCarForwardCommand)
        {
            //大车前进
            direction[index++] = ModelDirection.Forward;
        }else if (_systemVariables.LargeCarReverseCommand)
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
        }else if (_systemVariables.RotaryRightTurnCommand)
        {
            //大车右转
            direction[index++] = ModelDirection.Right;
        }
        else
        {
            direction[index++] = ModelDirection.LrStop;
        }
        EventManager.Instance.TriggerEvent(EventName.UpdateModelDirection,null,new UpdateModelDirectionEventArgs(direction,Machine.BucketWheelStackerReclaimer));
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
        meshCollider.sharedMesh = mesh;
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
        ServerCommand serverCommand = new ServerCommand();
        serverCommand.QUERY_SYSTEM = "MC";
        serverCommand.DATA_TYPE = 3;
        serverCommand.QUERY_TYPE = query_type;
        MessageCenter.Instance.SendMessage(MessageType.SCA, serverCommand);
    }
}