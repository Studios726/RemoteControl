using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using RemoteControl.Event;
using ShangHaiPro;
using ShenYangRemoteSystem.Subclass;
using UnityEngine;
using UnityEngine.Rendering;

public class GameDataManager : Singleton<GameDataManager>
{
    private bool _RcConnectionState;
    private SystemVariables _systemVariables;
    private SendDataReportAndDEM _sendDataReportAndDEM;
    private TaskVariables _taskVariables;

    private MachineMove machineMove_1;//堆取斗轮机
    private MachineMove machineMove_2;//取斗轮机
    private Dictionary<string, TaskData> nearestTaskDataDic = new Dictionary<string, TaskData>();
    public AccountInfo curAccountInfo;
    public bool RcConnectionState
    {
        get => _RcConnectionState;
        set => _RcConnectionState = value;
    }
    public SystemVariables SystemVariables
    {
        get => _systemVariables;
        set => _systemVariables = value;
    }

    public TaskVariables TaskVariables { get => _taskVariables; }
    public SendDataReportAndDEM SendDataReportAndDEM
    {
        get => _sendDataReportAndDEM;
    }
    public void SetSystemVariables(SystemVariables systemVariables)
    {
        _systemVariables = systemVariables;
        _RcConnectionState = _systemVariables.PLCCommunicationState;
        UpdateMachinePosAndRot();
        EventManager.Instance.TriggerEvent(EventName.UpdateRcData, null);
    }

    public void SetScaReportAndDEM(SendDataReportAndDEM sendDataReportAndDEM)
    {

        _sendDataReportAndDEM = sendDataReportAndDEM;
        EventManager.Instance.TriggerEvent(EventName.RefreshModel, null);
    }
    public void SetTaskVariables(TaskVariables taskVariables)
    {

        _taskVariables = taskVariables;
        if (taskVariables.McData.Count > 0)
        {

            if (nearestTaskDataDic.Count <= 0)
            {
                GetNearestTaskDataDic();
            }
            for (int i = 0; i < taskVariables.McData.Count - 1; i++)
            {
                if (nearestTaskDataDic.ContainsKey(taskVariables.McData[i].TaskID))
                {
                    TaskData taskData = nearestTaskDataDic[taskVariables.McData[i].TaskID];
                    if (taskVariables.McData[i].AllData.ProcessingProgress == 1 && taskData.TaskState == "0")
                    {
                        taskData.TaskState = "1";
                        DataManager.Instance.UpdateHistoryTaskMc(taskData.TaskID, "1");
                    }
                }
                else
                {
                    TaskCommand taskCommand = taskVariables.McData[i];
                    nearestTaskDataDic.Add(taskVariables.McData[i].TaskID, new TaskData(taskVariables.McData[i].TaskID, taskVariables.McData[i].AllData.ProcessingProgress.ToString()));
                    DataManager.Instance.InsertHistoryTaskMc(taskCommand,"test", taskVariables.McData[i].AllData.ProcessingProgress.ToString());
                }

            }

        }
        EventManager.Instance.TriggerEvent(EventName.UpdatePcData, null);
    }
    public void SetMachine(MachineMove machine1, MachineMove machine2)
    {
        machineMove_1 = machine1;
        machineMove_2 = machine2;
    }
    public void UpdateMachinePosAndRot()
    {
        if (machineMove_1)
        {
            machineMove_1.UpdatePosAndRotaionByMeter(SystemVariables.LargeCarTravelDistance, SystemVariables.RotaryAngle, SystemVariables.VariableAmplitudeAngle);//
        }
        if (machineMove_2)
        {
            machineMove_2.UpdatePosAndRotaionByMeter(SystemVariables.LargeCarTravelDistance_2, SystemVariables.RotaryAngle_2, SystemVariables.VariableAmplitudeAngle_2);//
        }
    }
    public GameObject SpawnCoalModel(Transform parent, Material material, SendDataReportAndDEM sendDataReportAndDem)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        //初始顶点颜色列表
        List<Color> colorList = new List<Color>();

        // SendDataReportAndDEM sendDataReportAndDem = JsonConvert.DeserializeObject<SendDataReportAndDEM>(jsonData);
        Debug.Log("data " + sendDataReportAndDem.SendCoalFeapDEM.QUERY_SYSTEM);
        CoalHeapDEM demData = sendDataReportAndDem.SendCoalFeapDEM;
        for (int i = 0; i < sendDataReportAndDem.SendCoalFeapDEM.NZ; i++) //1003
        {
            for (int j = 0; j < demData.NX; j++) //336
            {
                float X = demData.Z0 + demData.DZ * i - (demData.NZ - 1) * demData.DZ / 2;
                float Z = -(demData.X0 + demData.DX * j) + (demData.NX - 1) * demData.DX / 2;
                float Y = -demData.DEM[i, j];
                vertices.Add(new Vector3(X, Y, Z)); //循环获得顶点列表224784
            }
        }

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

        //取分区数据
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
                    pointColor = new Color(reg.ColorR / 255.0f, reg.ColorG / 255.0f, reg.ColorB / 255.0f, 1);
                    break;
                }
            }

            colorList.Add(pointColor);
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32;
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colorList.ToArray();
        GameObject go = new GameObject("Model");
        go.transform.SetParent(parent);
        go.transform.localRotation = Quaternion.identity;
        MeshFilter meshFilter = go.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = go.AddComponent<MeshCollider>();
        meshFilter.mesh = mesh;
        meshRenderer.sharedMaterial = material;
        meshFilter.mesh.RecalculateNormals(); //更新法线
        meshCollider.sharedMesh = mesh;
        return go;
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
    /// <summary>
    /// 获取任务当前状态
    /// </summary>
    public void UpdatePcData()
    {
        //TaskCommand taskCommand = new TaskCommand();
        //taskCommand.QuerySystem = "MC";
        //taskCommand.Command_Type = 1;
        //MessageCenter.Instance.SendMessage(MessageType.PC, taskCommand);
    }

    public void UpdateSCAData()
    {
        Debug.LogError("堆料模型更新");
        ServerCommand serverCommand = new ServerCommand();
        serverCommand.QUERY_SYSTEM = "MC";
        serverCommand.DATA_TYPE = 3;
        serverCommand.QUERY_TYPE = 30;
        MessageCenter.Instance.SendMessage(MessageType.SCA, serverCommand);
    }
    public void SendTaskCommand(TaskCommand taskCommand)
    {
        MessageCenter.Instance.SendMessage<TaskCommand>(MessageType.PC, taskCommand);
    }

    public Dictionary<string, TaskData> GetNearestTaskDataDic()
    {
        if (nearestTaskDataDic.Count <= 0)
        {
            //string sql = "Select * from history_task_mc ORDER BY id DESC LIMIT 2;";
            //MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(sql);
            MySqlDataReader mySqlDataReader=DataManager.Instance.GetHistoryTaskMc(5);
            while (mySqlDataReader.Read())
            {
                string taskID = mySqlDataReader["TaskID"].ToString();
                nearestTaskDataDic.Add(taskID, new TaskData(taskID, mySqlDataReader["TaskState"].ToString()));
            }
        }
        return nearestTaskDataDic;
    }
}

