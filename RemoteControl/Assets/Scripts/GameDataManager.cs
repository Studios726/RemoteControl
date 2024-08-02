using System.Collections;
using System.Collections.Generic;
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

    private MachineMove machineMove_1;//堆取斗轮机
    private MachineMove machineMove_2;//取斗轮机
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
    public void SetSystemVariables(SystemVariables systemVariables)
    {
        _systemVariables= systemVariables;
        _RcConnectionState = _systemVariables.PLCCommunicationState;
        UpdateMachinePosAndRot();
        EventManager.Instance.TriggerEvent(EventName.UpdateRcData, null);
        Debug.Log($"Plc 链接状态 {_systemVariables.PLCCommunicationState}");
    }

    public void SetMachine(MachineMove machine1, MachineMove machine2) {
        machineMove_1= machine1;
        machineMove_2= machine2;
    }
    public void UpdateMachinePosAndRot()
    {
        if (machineMove_1) {
            machineMove_1.UpdatePosAndRotaionByMeter(SystemVariables.LargeCarTravelDistance, 0, 0);
        }
        if (machineMove_2) {
            machineMove_2.UpdatePosAndRotaionByMeter(SystemVariables.LargeCarTravelDistance, 0, 0);
        }
    }
    public GameObject SpawnCoalModel(Transform parent,Material material,SendDataReportAndDEM sendDataReportAndDem)
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
}