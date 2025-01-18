using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using RemoteControl;
using RemoteControl.Event;
using ShangHaiPro;
using ShenYangRemoteSystem.Subclass;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Random = System.Random;

public struct WarningData
{
    public string Des;
    public DateTime Time;

    public WarningData(string des, int rank = 3)
    {
        string time = DateTime.Now.ToString("HH:mm:ss");
        if (rank == 0)
        {
            Des = $"<color=#FF0000>{des} {time}</color>";
        }
        else if (rank == 1)
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

[DataContract]
public class McWarningRecord
{
    [DataMember] public DateTime updateTime;

    [DataMember]
    public Dictionary<string, WarningCellData> WarningCellDataDict = new Dictionary<string, WarningCellData>();

    public McWarningRecord(Dictionary<string, WarningCellData> warningCellDatas, DateTime dateTime)
    {
        WarningCellDataDict = warningCellDatas;
        updateTime = dateTime;
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

    public List<FlowMeter_data> flowMeterDataList = new List<FlowMeter_data>();
    public bool IsCanPop;
    public bool IsCanPopTakeMater;
    public BucketLidarDis[] BucketLidarDisList;
    public SystemVariables SystemVariables
    {
        get => _systemVariables;
    }

    public bool IsAdmin()
    {
        if (curAccountInfo != null && curAccountInfo.isAdmin&&IpConfig.IsRecordData)
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

        return "";
    }

    public string GetUserID()
    {
        if (curAccountInfo != null)
        {
            return curAccountInfo.ID;
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
        if (_systemVariables == null)
        {
            IsCanPop = true;
            IsCanPopTakeMater = true;
        }

        if (systemVariables.MCString != null)
        {
            try
            {
                AlarmDataManager.Instance.McWarningRecord(systemVariables.MCString);
            }
            catch (Exception e)
            {
                Debug.LogError("解析失败");
            }
        }

        AlarmDataManager.Instance.RecordWarning(systemVariables, _systemVariables);
        _systemVariables = systemVariables;
        _rcConnectionState = _systemVariables.D1PLC1CommunicationState;
        if (_systemVariables.SuspensionGlueRunCommand && IsCanPop && _systemVariables.BeltRealyDis > 0 &&
            curAccountInfo != null)
        {
            IsCanPop = false;
            Timer.Register(_systemVariables.BeltRealyDis, false, false, (() => { IsCanPop = true; }));
            PileTakeMaterPop(TaskType.PILEMATER, _systemVariables.BeltRealyDis, Machine.BucketWheelStackerReclaimer);
        }


        if (_systemVariables.SuspensionGlueRunCommand_2 && IsCanPopTakeMater && _systemVariables.BeltRealyDis_2 > 0 &&
            curAccountInfo != null)
        {
            IsCanPopTakeMater = false;
            Timer.Register(_systemVariables.BeltRealyDis_2, false, false, (() => { IsCanPopTakeMater = true; }));
            PileTakeMaterPop(TaskType.TAKEMATER, _systemVariables.BeltRealyDis_2, Machine.BucketWheel);
        }

        UpdateMachine();

        EventManager.Instance.TriggerEvent(EventName.UpdateRcData, null);
    }

    public void SetFlowMeterData(List<FlowMeter_data> datas)
    {
        flowMeterDataList = datas;
        EventManager.Instance.TriggerEvent(EventName.UpdateFlowMeterData, null);
    }

    public FlowMeter_data GetFlowMeterData(Machine machine)
    {
        if (flowMeterDataList.Count > 0) //更新流量
        {
            for (int i = 0; i < flowMeterDataList.Count; i++)
            {
                if (flowMeterDataList[i].id == (int)machine)
                {
                    return flowMeterDataList[i];
                }
            }
        }

        return null;
    }

    //悬胶皮带运行提示
    public void PileTakeMaterPop(TaskType taskType, int time, Machine machine)
    {
        string title = machine == Machine.BucketWheelStackerReclaimer
            ? ConstStr.BucketWheelStackerReclaimerName
            : ConstStr.BucketWheelName;
        UIID uiID = machine == Machine.BucketWheelStackerReclaimer ? UIID.ConfirmPanel_1 : UIID.ConfirmPanel_2;
        if (taskType == TaskType.PILEMATER)
        {
            UIManager.Instance.OpenUI(uiID, new ConfirmPanelArgs("悬胶堆料运行倒计时 {0}s", title, null, null, time, 0, uiID));
        }
        else if (taskType == TaskType.TAKEMATER)
        {
            UIManager.Instance.OpenUI(uiID,
                new ConfirmPanelArgs("悬胶取料运行倒计时 {0}s", title, null, null, time, 0, uiID));
        }
        else
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                new ConfirmPanelArgs("悬胶运行倒计时 {0}s", "斗轮机", null, null, time));
        }
    }

    public string GetMachineName(Machine machine)
    {
        string title = machine == Machine.BucketWheelStackerReclaimer
            ? ConstStr.BucketWheelStackerReclaimerName
            : ConstStr.BucketWheelName;
        return title;
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

    //油泵是否启动判断
    public string IsOilPumpStarted(Machine machine)
    {
        if (_systemVariables == null)
        {
            return "油泵未启动，是否继续执行？";
        }
        else
        {
            string des;
            bool isRelaxed = machine == Machine.BucketWheelStackerReclaimer
                ? _systemVariables.VariableAmplitudeOilPumpMotorRunning
                : _systemVariables.VariableAmplitudeOilPumpMotorRunning_2;
            if (isRelaxed)
            {
                des = "油泵已启动，是否继续执行？";
            }
            else
            {
                des = "油泵未启动，是否继续执行？";
            }

            return des;
        }
    }

    // 夹轨器是否放松判断
    public string IsRailClampRelaxed(Machine machine)
    {
        if (_systemVariables == null)
        {
            return "夹轨器未放松，是否继续执行?";
        }
        else
        {
            string des;
            bool isRelaxed = machine == Machine.BucketWheelStackerReclaimer
                ? (_systemVariables.LeftClampRelaxLimit && _systemVariables.RightClampRelaxLimit)
                : (_systemVariables.LeftClampRelaxLimit_2 && _systemVariables.RightClampRelaxLimit_2);
            if (isRelaxed)
            {
                des = "夹轨器已放松，是否继续执行?";
            }
            else
            {
                des = "夹轨器未放松，是否继续执行?";
            }

            return des;
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
        UpdateBucketWheelPosText();
        UpdateWheelAnimation();
        UpdateBeltAnimation();
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
            machineMove_2.UpdatePosAndRotaionByMeter(SystemVariables.DC_Pos_2 + ConstStr.InitDistance,
                SystemVariables.SLEW_Angle_2,
                -SystemVariables.Luff_Angle_2); //
        }
    }

    /// <summary>
    /// 更新模型斗輪數據
    /// </summary>
    public void UpdateBucketWheelPosText()
    {
        if (machineMove_1 && machineMove_2)
        {
            machineMove_1.SetFogfallVfxActive(SystemVariables.BucketWheelMaterialLoadingRunning);
            machineMove_1.UpdateBucketWheelPosText(
                $"{(SystemVariables.DC_Pos + ConstStr.InitPosition_1).ToString("F2")} m");
            machineMove_2.UpdateBucketWheelPosText(
                $"{(SystemVariables.DC_Pos_2 + ConstStr.InitPosition_2).ToString("F2")} m");
            machineMove_1.UpdateBucketWheelHeighText(SystemVariables.Luff_Angle);
            machineMove_2.UpdateBucketWheelHeighText(SystemVariables.Luff_Angle_2);
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
            
            error = _systemVariables.LuffUp_LimitStatus ? error + "俯仰上限位集合\n" : error;
            error = _systemVariables.LuffDown_LimitStatus ? error + "俯仰下限位集合\n" : error;
            error = _systemVariables.DcFWD_LimitStatus ? error + "大车前进限位集合\n" : error;
            error = _systemVariables.DcREV_LimitStatus ? error + "大车后退限位集合\n" : error;
            error = _systemVariables.Slew_R_LimitStatus ? error + "悬臂右转限位集合\n" : error;
            error = _systemVariables.Slew_L_LimitStatus ? error + "悬臂左转限位集合\n" : error;
         

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
            
            error = _systemVariables.LuffUp_LimitStatus_2 ? error + "俯仰上限位集合\n" : error;
            error = _systemVariables.LuffDown_LimitStatus_2 ? error + "俯仰下限位集合\n" : error;
            error = _systemVariables.DcFWD_LimitStatus_2 ? error + "大车前进限位集合\n" : error;
            error = _systemVariables.DcREV_LimitStatus_2 ? error + "大车后退限位集合\n" : error;
            error = _systemVariables.Slew_R_LimitStatus_2 ? error + "悬臂右转限位集合\n" : error;
            error = _systemVariables.Slew_L_LimitStatus_2 ? error + "悬臂左转限位集合\n" : error;
          

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
        machineMove_1.PlayCantileverClipTake(_systemVariables.BucketWheelMaterialUnloadingRunning);
        machineMove_1.PlayCantileverClipPile(_systemVariables.BucketWheelMaterialLoadingRunning);
        machineMove_2.PlayRotationClip(_systemVariables.BucketWheelMotorRunning_2);
        machineMove_2.PlayCantileverClipTake(_systemVariables.BucketWheelMaterialUnloadingRunning_2);
        machineMove_2.PlayCantileverClipPile(_systemVariables.BucketWheelMaterialLoadingRunning_2);
    }

    public void UpdateBeltAnimation()
    {
        EventManager.Instance.TriggerEvent(EventName.PlayBeltAnim, null,
            new BeltRunArgs(
                _systemVariables.AllowBucketWheelMaterialUnloading || _systemVariables.AllowBucketWheelMaterialLoading,
                _systemVariables.AllowBucketWheelMaterialUnloading_2 ||
                _systemVariables.AllowBucketWheelMaterialLoading_2));
    }

    public async Task DeSerializeScaJson(string json)
    {
        SendDataReportAndDEM cursendDataReportAndDem = new SendDataReportAndDEM();
        await Task.Run((() =>
        {
            SystemCommand command = JsonMgr.DeSerialize<SystemCommand>(json);
            cursendDataReportAndDem = command.SendAllData.DEM_DATA;
        }));
       
        if (cursendDataReportAndDem==null)
        {
            Debug.Log($"获取三维数据 cursendDataReportAndDem is null");
            return;
        }
        if (cursendDataReportAndDem.code==1)
        {
            SetScaReportAndDem(cursendDataReportAndDem);
        }else if (cursendDataReportAndDem.code==2)
        {
            BucketLidarDisList = cursendDataReportAndDem.bucketLidarDisList;
            // Debug.LogError($">>>>>{cursendDataReportAndDem.bucketLidarDisList[0].Dis}");
        }
        else
        {
            Debug.Log($"获取三维数据 code: {cursendDataReportAndDem.code}");
        }
     
    }

    public async Task SpawnCoalModel(Transform parent, Material material, SendDataReportAndDEM sendDataReportAndDem,
        GameObject model = null, MeshFilter meshFilter = null, MeshRenderer meshRenderer = null,
        MeshCollider meshCollider = null, Mesh cachedMesh = null)
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
            return;
        }

        try
        {
            cachedMesh.indexFormat = IndexFormat.UInt32;
            cachedMesh.vertices = vertices;
            cachedMesh.triangles = triangles;
            cachedMesh.colors = colorList;
            model.transform.localRotation = Quaternion.identity;
            meshFilter.mesh = cachedMesh;
            meshRenderer.sharedMaterial = material;
            meshFilter.mesh.RecalculateNormals();
            meshCollider.sharedMesh = cachedMesh;
        }
        catch (Exception e)
        {
            Debug.LogError("加载模型失败");
        }
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
    
    public void SendServerCommandByName(string commandName, int dataInt = 0, float dataFloat = 0)
    {
        ServerCommand serverCommand = new ServerCommand();
        serverCommand.QUERY_SYSTEM = "MC";
        serverCommand.DATA_TYPE = 6;
        serverCommand.QUERY_TYPE = 2;
        serverCommand.COMMAND_NAME = commandName;
        serverCommand.DATA_INT = dataInt;
        serverCommand.DATA_FLOAT = dataFloat;
        MessageCenter.Instance.SendMessage(MessageType.RC, serverCommand);
        Debug.Log($"命令{DateTime.Now.ToString("G")} commandName: {commandName} dataInt: {dataInt} dataFloat: {dataFloat}");
    }

    public void SendServerCommandRC(string commandName, int dataType, int queryType, int dataInt = 0,
        float dataFloat = 0)
    {
        ServerCommand serverCommand = new ServerCommand();
        serverCommand.QUERY_SYSTEM = "MC";
        serverCommand.DATA_TYPE = dataType;
        serverCommand.QUERY_TYPE = queryType;
        serverCommand.COMMAND_NAME = commandName;
        serverCommand.DATA_INT = dataInt;
        serverCommand.DATA_FLOAT = dataFloat;
        MessageCenter.Instance.SendMessage(MessageType.RC, serverCommand);
    }

    /// <summary>
    /// 获取三维数据
    /// </summary>
    /// <param name="query_type"></param>
    public void UpdateSCAData(int query_type)
    {
        SystemCommand serverCommand = new SystemCommand();
        serverCommand.QUERY_SYSTEM = "MC";
        serverCommand.DATA_TYPE = 3;
        serverCommand.QUERY_TYPE = query_type;
        MessageCenter.Instance.SendMessage(MessageType.SCA, serverCommand);
    }

    /// <summary>
    /// 获取流量计数据
    /// </summary>
    /// <param name="query_type"></param>
    public void UpdateFMData(int query_type)
    {
        SystemCommand serverCommand = new SystemCommand();
        serverCommand.QUERY_SYSTEM = "MC";
        serverCommand.DATA_TYPE = 3;
        serverCommand.QUERY_TYPE = query_type;
        MessageCenter.Instance.SendMessage(MessageType.FM, serverCommand);
    }

    public void RecordChart()
    {
        if (_systemVariables == null)
        {
            return;
        }
        if (curAccountInfo != null && curAccountInfo.isAdmin&&IpConfig.IsRecordData)
        {
            string sql = "";
            if (GameMain!=null&&GameMain.connectionRC.isConnect == true)
            {
                string a=DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC,
                    _systemVariables.BucketWheelElectricCurrent, "斗轮电流",
                    Machine.BucketWheelStackerReclaimer);

                string b=DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC,
                    _systemVariables.LargeCarElectricCurrent, "大车电流",
                    Machine.BucketWheelStackerReclaimer);

                string c=DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC,
                    _systemVariables.RotaryElectricCurrent, "回转电流",
                    Machine.BucketWheelStackerReclaimer);

                string d=DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC,
                    _systemVariables.SuspensionBeltElectricCurrent, "悬胶电流",
                    Machine.BucketWheelStackerReclaimer);

                string e=DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC,
                    _systemVariables.BucketWheelElectricCurrent_2, "斗轮电流",
                    Machine.BucketWheel);

                string f=DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC,
                    _systemVariables.LargeCarElectricCurrent_2, "大车电流",
                    Machine.BucketWheel);

                string g=DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_ROTELECTRICITY_MC,
                    _systemVariables.RotaryElectricCurrent_2, "回转电流",
                    Machine.BucketWheel);

                string h=DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC,
                    _systemVariables.SuspensionBeltElectricCurrent_2, "悬胶电流",
                    Machine.BucketWheel);
                sql = $"{a};{b};{c};{d};{e};{f};{g};{h}";
            }

            if (GameMain!=null&&GameMain.connectionFM.isConnect == true)
            {
                if (flowMeterDataList.Count > 0) //更新流量
                {
                    for (int i = 0; i < flowMeterDataList.Count; i++)
                    {
                        sql=sql+";"+DataManager.Instance.InsertHistoryChartData(ConstStr.DATABASE_HISTORY_CANTILEVER_Flow_MC,
                            (float)flowMeterDataList[i].FlowRealtime, "悬臂流量",
                            (Machine)flowMeterDataList[i].id);
                    }
                 
                }
            }
            MySqlHelper.ExecuteSql(@sql);
        }
    }
    /// <summary>
    /// 获取雷达距离
    /// </summary>
    /// <param name="machine">堆取料机   取料机</param>
    /// <param name="LidarPlace">0 左 1右   取料机</param>
    /// <returns></returns>
    public string GetBucketLidarDisByMachine(string machine,int LidarPlace)
    {
        if (BucketLidarDisList!=null&&BucketLidarDisList.Length>0)
        {
            for (int i = 0; i < BucketLidarDisList.Length; i++)
            {
                if (BucketLidarDisList[i].BucketName==machine&&BucketLidarDisList[i].LidarPlace==LidarPlace)
                {
                    return BucketLidarDisList[i].Dis.ToString("F2");
                }
            }
        }

        return "0";
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

        if (Math.Abs(monthsDifference) >= 3)
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
}