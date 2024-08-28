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
    private MachineMove machineMove_1;//堆取斗轮机
    private MachineMove machineMove_2;//取斗轮机
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
        if (curAccountInfo!=null)
        {
            return curAccountInfo.name;
        }

        return "";
    }

    public void SetIp(string ip)
    {
        _taoIP ="ws://"+ip;
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
        string json=PlayerPrefs.GetString("LocalSCAData", "");
        // Debug.LogError($" 获取本地数据sca {json}");
        if (json!="")
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
        if (machineRoot == null) {

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
            machineMove_1.UpdatePosAndRotaionByMeter(SystemVariables.DC_Pos, SystemVariables.SLEW_Angle, SystemVariables.Luff_Angle);//
        }
        if (machineMove_2)
        {
            machineMove_2.UpdatePosAndRotaionByMeter(SystemVariables.DC_Pos_2, SystemVariables.SLEW_Angle_2, SystemVariables.Luff_Angle_2);//
        }
    }
    // public GameObject SpawnCoalModel(Transform parent, Material material, SendDataReportAndDEM sendDataReportAndDem)
    // {
    //     List<Vector3> vertices = new List<Vector3>();
    //     List<int> triangles = new List<int>();
    //     //初始顶点颜色列表
    //     List<Color> colorList = new List<Color>();
    //
    //     // SendDataReportAndDEM sendDataReportAndDem = JsonConvert.DeserializeObject<SendDataReportAndDEM>(jsonData);
    //     Debug.Log("data " + sendDataReportAndDem.SendCoalHeapDEM.QUERY_SYSTEM);
    //     CoalHeapDEM demData = sendDataReportAndDem.SendCoalHeapDEM;
    //     for (int i = 0; i < sendDataReportAndDem.SendCoalHeapDEM.NZ; i++) //1003
    //     {
    //         for (int j = 0; j < demData.NX; j++) //336
    //         {
    //             float X = demData.Z0 + demData.DZ * i - (demData.NZ - 1) * demData.DZ / 2;
    //             float Z = -(demData.X0 + demData.DX * j) + (demData.NX - 1) * demData.DX / 2;
    //             float Y = -demData.DEM[i, j];
    //             vertices.Add(new Vector3(X, Y, Z)); //循环获得顶点列表224784
    //         }
    //     }
    //
    //     for (int m = 0; m < demData.NZ - 1; m++)
    //     {
    //         for (int n = 0; n < demData.NX - 1; n++)
    //         {
    //             int[] face = new int[6]; //循环获得面列表
    //
    //             face[0] = m * demData.NX + n; //0
    //             face[1] = (m + 1) * demData.NX + n; //336
    //             face[2] = m * demData.NX + (n + 1); //1
    //
    //             face[3] = m * demData.NX + (n + 1); //1
    //             face[4] = (m + 1) * demData.NX + n; //336
    //             face[5] = (m + 1) * demData.NX + (n + 1); //337
    //
    //             for (int q = 0; q < 6; q++)
    //             {
    //                 triangles.Add(face[q]); //存入顶点索引数据 
    //             }
    //         }
    //     }
    //
    //     //取分区数据
    //     REGION[] regionList = demData.REGION_LIST;
    //     float xLength = demData.LENGTH / 2;
    //     foreach (Vector3 ve3 in vertices)
    //     {
    //         Color pointColor = Color.gray;
    //         foreach (REGION reg in regionList)
    //         {
    //             int side;
    //             if (ve3.z > 0)
    //             {
    //                 side = 1;
    //             }
    //             else
    //             {
    //                 side = 0;
    //             }
    //
    //             if (ve3.x + xLength >= reg.BEGIN && ve3.x + xLength < reg.END && side == reg.SIDE)
    //             {
    //                 if (reg.IsUseLayer==0)
    //                 {
    //                     pointColor = new Color(reg.ColorR / 255.0f, reg.ColorG / 255.0f, reg.ColorB / 255.0f, 1);
    //                     break;
    //                 }
    //                 else
    //                 {
    //                     for (int i = 0; i < reg.layerArray.Count; i++)
    //                     {
    //                         if (ve3.y>=Mathf.Abs(reg.layerArray[i].hBEGIN)&&ve3.y<Mathf.Abs(reg.layerArray[i].hEND))
    //                         {
    //                             pointColor = new Color(reg.layerArray[i].ColorR / 255.0f, reg.layerArray[i].ColorG / 255.0f, reg.layerArray[i].ColorB / 255.0f, 1);
    //                             break;
    //                         }
    //                     }
    //                 }
    //                 break;
    //             }
    //         }
    //
    //         colorList.Add(pointColor);
    //     }
    //
    //     Mesh mesh = new Mesh();
    //     mesh.indexFormat = IndexFormat.UInt32;
    //     mesh.vertices = vertices.ToArray();
    //     mesh.triangles = triangles.ToArray();
    //     mesh.colors = colorList.ToArray();
    //     GameObject go = new GameObject("Model");
    //     go.transform.SetParent(parent);
    //     go.transform.localRotation = Quaternion.identity;
    //     MeshFilter meshFilter = go.AddComponent<MeshFilter>();
    //     MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
    //     MeshCollider meshCollider = go.AddComponent<MeshCollider>();
    //     meshFilter.mesh = mesh;
    //     meshRenderer.sharedMaterial = material;
    //     meshFilter.mesh.RecalculateNormals(); //更新法线
    //     meshCollider.sharedMesh = mesh;
    //     return go;
    // }
    public async Task SpawnCoalModel(Transform parent, Material material, SendDataReportAndDEM sendDataReportAndDem,GameObject model=null)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        //初始顶点颜色列表
        List<Color> colorList = new List<Color>();
        CoalHeapDEM demData = sendDataReportAndDem.SendCoalHeapDEM;
        // GameObject go = new GameObject("Model");
        // Debug.Log($"开始获取顶点 {DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}");
        await Task.Run(() => 
        {
            // 第一步处理，例如解析数据格式
            for (int i = 0; i < sendDataReportAndDem.SendCoalHeapDEM.NZ; i++) //1003
            {
                for (int j = 0; j < demData.NX; j++) //336
                {
                    float X = demData.Z0 + demData.DZ * i - (demData.NZ - 1) * demData.DZ / 2;
                    float Z = -(demData.X0 + demData.DX * j) + (demData.NX - 1) * demData.DX / 2;
                    float Y = -demData.DEM[i, j];
                    vertices.Add(new Vector3(X, Y, Z)); //循环获得顶点列表224784
                }
            }
        });
        // Debug.Log($"开始获取面数 {DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}");
        await Task.Run(() => 
        {
            // 第二步处理，例如生成顶点数据
            for (int m = 0; m < demData.NZ - 1; m++)
            {
                for (int n = 0; n < demData.NX - 1; n++)
                {
                    int[] face = new int[6]; //循环获得面列表

                    face[0] = m * demData.NX + n; //0
                    face[1] = (m + 1) * demData.NX + n; //336
                    face[2] = m * demData.NX + (n + 1); //1

                    face[3] = m * demData.NX + (n + 1); //1
                    face[4] = (m + 1) * demData.NX + n; //336
                    face[5] = (m + 1) * demData.NX + (n + 1); //337

                    for (int q = 0; q < 6; q++)
                    {
                        triangles.Add(face[q]); //存入顶点索引数据 
                    }
                }
            }

        });
        await Task.Run(() => 
        {
            // 第三步处理，例如构建 Mesh
            REGION[] regionList = demData.REGION_LIST;
            float xLength = demData.LENGTH / 2;
            foreach (Vector3 ve3 in vertices)
            {
                Color pointColor = Color.gray;
                foreach (REGION reg in regionList)
                {
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
                        if (reg.IsUseLayer==0)
                        {
                            pointColor = new Color(reg.ColorR / 255.0f, reg.ColorG / 255.0f, reg.ColorB / 255.0f, 1);
                            break;
                        }
                        else
                        {
                            for (int i = 0; i < reg.layerArray.Count; i++)
                            {
                                if (ve3.y>=Mathf.Abs(reg.layerArray[i].hBEGIN)&&ve3.y<Mathf.Abs(reg.layerArray[i].hEND))
                                {
                                    pointColor = new Color(reg.layerArray[i].ColorR / 255.0f, reg.layerArray[i].ColorG / 255.0f, reg.layerArray[i].ColorB / 255.0f, 1);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }

                colorList.Add(pointColor);
            }

        });
        Mesh mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32;
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colorList.ToArray();
        // go.transform.SetParent(parent);
        model.transform.localRotation = Quaternion.identity;
        MeshFilter meshFilter = model.GetComponent<MeshFilter>();// go.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = model.GetComponent<MeshRenderer>();
        MeshCollider meshCollider = model.GetComponent<MeshCollider>();
        meshFilter.mesh = mesh;
        meshRenderer.sharedMaterial = material;
        meshFilter.mesh.RecalculateNormals(); //更新法线
        meshCollider.sharedMesh = mesh;
        // Debug.Log("模型处理全部完成");
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

    public void SendServerCommandByName(string commandName,int dataInt=0)
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

