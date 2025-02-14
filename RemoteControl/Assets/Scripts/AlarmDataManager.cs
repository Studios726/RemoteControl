using System;
using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using ShenYangRemoteSystem.Subclass;
using UnityEngine;

public class AlarmDataManager : Singleton<AlarmDataManager>
{
    public Dictionary<string, WarningCellData> WarningCellDataDict = new Dictionary<string, WarningCellData>();
    public bool IsUpdatePlcWarningRecord;
    public McWarningRecord LastMcWarningRecord;
    public List<WarningCellData> ImportantAlarmMessageList_1 = new List<WarningCellData>();
    public List<WarningCellData> ImportantAlarmMessageList_2 =  new List<WarningCellData>();
    public List<string> ImportantAlarmNameList = new List<string>()
    {
        "TaoRC",
        "TaoRC_2",
        "TaskPC",
        "TaskPC_2",
        "StartAlarmStatus",
        "StartAlarmStatus_2",
        "D1PLC1CommunicationState",
        "D1PLC2CommunicationState",
        "D2PLC1CommunicationState",
        "D2PLC2CommunicationState",
        "BucketWheelFault",
        "BucketWheelFault_2",
        "LinkedBucketWheelNotRunning",
        "LinkedBucketWheelNotRunning_2",
        "RotaryFault",
        "RotaryFault_2",
        "SuspensionBeltFault",
        "SuspensionBeltFault_2",
        "BucketWheelOverTorqueSwitch",
        "BucketWheelOverTorqueSwitch_2",
        "LargeCarFault",
        "LargeCarFault_2",
        "SR1_REMOTE_PLANT_COMM_FAULT_0",
        "SR1_REMOTE_PLANT_COMM_FAULT_0_2"
    };
    public string GetUserName()
    {
        if (GameDataManager.Instance.curAccountInfo != null)
        {
            return GameDataManager.Instance.curAccountInfo.name;
        }

        return "";
    }

    public void McWarningRecord(string MCString)
    {
        if (MCString != null && MCString != "")
        {
            try
            {
                McWarningRecord mcWarningRecord = JsonMgr.DeSerialize<McWarningRecord>(MCString);
                if (LastMcWarningRecord == null)
                {
                    LastMcWarningRecord = mcWarningRecord;
                    UpdateWarningByLastMcWarningRecord();
                }
                else
                {
                    if ((mcWarningRecord.updateTime - LastMcWarningRecord.updateTime).TotalSeconds > 0)
                    {
                        LastMcWarningRecord = mcWarningRecord;
                        UpdateWarningByLastMcWarningRecord();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("解析失败");
            }
        }
    }

    public void UpdatePlcWarningRecordData()
        {
            McWarningRecord mcWarningRecord = new McWarningRecord(WarningCellDataDict, DateTime.Now);
            ServerCommand serverCommand = new ServerCommand();
            serverCommand.QUERY_SYSTEM = "MC";
            serverCommand.DATA_TYPE = 6;
            serverCommand.QUERY_TYPE = 3;
            serverCommand.DATA_STRING = JsonMgr.Serialize(mcWarningRecord);
            MessageCenter.Instance.SendMessage(MessageType.RC, serverCommand);
        }

        public void UpdateWarningConfirmTime(Machine machine)
        {
            if (WarningCellDataDict.Count > 0)
            {
                foreach (var keyCellData in WarningCellDataDict)
                {
                    if (machine == keyCellData.Value.Machine && keyCellData.Value.IsSelect &&
                        keyCellData.Value.IsConfirm == false)
                    {
                        //更新确认时间
                        keyCellData.Value.ConfirmTime = DateTime.Now.ToString("HH:mm:ss");
                        keyCellData.Value.IsConfirm = true;
                    }
                }

                if (machine == Machine.BucketWheelStackerReclaimer)
                {
                    EventManager.Instance.TriggerEvent(EventName.RefreshWarningDes1, null);
                }
                else
                {
                    EventManager.Instance.TriggerEvent(EventName.RefreshWarningDes2, null);
                }
            }
        }

        public void ChangeWarningDesDict(string key, bool isSelect, string confirmTime)
        {
            if (WarningCellDataDict.ContainsKey(key))
            {
                WarningCellDataDict[key].IsSelect = isSelect;
                WarningCellDataDict[key].ConfirmTime = confirmTime;
            }
        }

        public void AddOrUpdateWarningDesQueue(string des, Machine machine, int rank = 0)
        {
            // des = des + DateTime.Now.ToString("HH:mm:ss");
            // if (machine==Machine.BucketWheelStackerReclaimer)
            // {
            //     BucketWheelStackerReclaimerWarningCellDataList.Add(new WarningCellData(des,machine,false,""));
            //     EventManager.Instance.TriggerEvent(EventName.RefreshTaskDes1, null);
            // }
            // else
            // {
            //     BucketWheelWarningCellDataList.Add(new WarningCellData(des,machine,false,""));
            //     EventManager.Instance.TriggerEvent(EventName.RefreshTaskDes2, null);
            // }
        }

        public void AddOrUpdateWarningDesDict(string key, string des, Machine machine, bool isSelect, string time,
            bool isConfirm = false, string confirmTime = "", bool isDataSynchronized = true)
        {
            if (WarningCellDataDict.ContainsKey(key))
            {
                WarningCellDataDict.Remove(key);
            }

            WarningCellDataDict.Add(key,
                new WarningCellData(key, des, DateTime.Now, machine, isConfirm, isSelect, confirmTime,
                    isDataSynchronized));
            AddImportantAlarm(machine,key,des);
            if (machine == Machine.BucketWheelStackerReclaimer)
            {
                EventManager.Instance.TriggerEvent(EventName.RefreshWarningDes1, null);
            }
            else
            {
                EventManager.Instance.TriggerEvent(EventName.RefreshWarningDes2, null);
            }

            IsUpdatePlcWarningRecord = true;
        }

        public void AddOrUpdateWarningDesDict(string key, string des, Machine machine, bool isSelect,
            DateTime TriggerTime,
            bool isConfirm = false, string confirmTime = "")
        {
            if (WarningCellDataDict.ContainsKey(key))
            {
                WarningCellDataDict.Remove(key);
                WarningCellDataDict.Add(key,
                    new WarningCellData(key, des, TriggerTime, machine, isConfirm, isSelect, confirmTime));
                AddImportantAlarm(machine,key,des);
                if (machine == Machine.BucketWheelStackerReclaimer)
                {
                    EventManager.Instance.TriggerEvent(EventName.RefreshWarningDes1, null);
                }
                else
                {
                    EventManager.Instance.TriggerEvent(EventName.RefreshWarningDes2, null);
                }
            }
        }

        public void RemoveWarningDesDict(string key)
        {
            if (WarningCellDataDict.ContainsKey(key))
            {
                RemoveImportantAlarm(WarningCellDataDict[key].Machine,key);
                WarningCellDataDict.Remove(key);
              
                UpdatePlcWarningRecordData();
            }

            EventManager.Instance.TriggerEvent(EventName.RefreshWarningDes1, null);
            EventManager.Instance.TriggerEvent(EventName.RefreshWarningDes2, null);
        }

        public void AddImportantAlarm(Machine machine, string name, string des)
        {
            if (ImportantAlarmNameList.Contains(name))
            {
                if (machine == Machine.BucketWheelStackerReclaimer)
                {
                    bool isAdd = true;
                    for (int i = 0; i < ImportantAlarmMessageList_1.Count; i++)
                    {
                        if (ImportantAlarmMessageList_1[i].Des == des)
                        {
                            ImportantAlarmMessageList_1[i].TriggerTime = DateTime.Now.ToString("HH:mm:ss");
                            ImportantAlarmMessageList_1[i].TriggerDateTime = DateTime.Now;
                            isAdd = false;
                        }
                    }

                    if (isAdd)
                    {
                        WarningCellData warningCellData = new WarningCellData(name, des, DateTime.Now, machine);
                        ImportantAlarmMessageList_1.Add(warningCellData);
                        EventManager.Instance.TriggerEvent(EventName.RefreshImportantAlarm,null,
                            new UpdateImportantAlarmArgs(true, warningCellData));
                    }
                }
                else
                {
                    bool isAdd = true;
                    for (int i = 0; i < ImportantAlarmMessageList_2.Count; i++)
                    {
                        if (ImportantAlarmMessageList_2[i].Des == des)
                        {
                            ImportantAlarmMessageList_2[i].TriggerTime = DateTime.Now.ToString("HH:mm:ss");
                            ImportantAlarmMessageList_2[i].TriggerDateTime = DateTime.Now;
                            isAdd = false;
                        }
                    }

                    if (isAdd)
                    {
                        WarningCellData warningCellData = new WarningCellData(name, des, DateTime.Now, machine);
                        ImportantAlarmMessageList_2.Add(warningCellData);
                        EventManager.Instance.TriggerEvent(EventName.RefreshImportantAlarm,null,
                            new UpdateImportantAlarmArgs(true, warningCellData));
                    }
                }
            }
        }

        public void RemoveImportantAlarm(Machine machine, string name)
        {
            if (ImportantAlarmNameList.Contains(name))
            {
                if (machine == Machine.BucketWheelStackerReclaimer)
                {
                    for (int i = 0; i < ImportantAlarmMessageList_1.Count; i++)
                    {
                        if (ImportantAlarmMessageList_1[i].Key == name)
                        {
                            WarningCellData warningCellData = ImportantAlarmMessageList_1[i];
                            ImportantAlarmMessageList_1.RemoveAt(i);
                            EventManager.Instance.TriggerEvent(EventName.RefreshImportantAlarm,null,
                                new UpdateImportantAlarmArgs(false,warningCellData));
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ImportantAlarmMessageList_2.Count; i++)
                    {
                        if (ImportantAlarmMessageList_2[i].Key == name)
                        {
                            WarningCellData warningCellData = ImportantAlarmMessageList_2[i];
                            ImportantAlarmMessageList_2.RemoveAt(i);
                            EventManager.Instance.TriggerEvent(EventName.RefreshImportantAlarm,null,
                                new UpdateImportantAlarmArgs(false, warningCellData));
                            break;
                        }
                    }
                }
            }
        }
        public void RecordWarning(SystemVariables newSystemVariables, SystemVariables _systemVariables)
        {
            bool isUpdate = false;
            IsUpdatePlcWarningRecord = false;
            if (_systemVariables == null)
            {
                _systemVariables = new SystemVariables();
                _systemVariables.D1PLC1CommunicationState = true;
                _systemVariables.D1PLC2CommunicationState = true;
                _systemVariables.D2PLC1CommunicationState = true;
                _systemVariables.D2PLC2CommunicationState = true;
                _systemVariables.ReelOverTensionLimit1_2 = true;
                _systemVariables.ReelOverTensionLimit1 = true;
                isUpdate = true;
            }

            if (_systemVariables != null)
            {
                if (newSystemVariables.D1PLC1CommunicationState == false && _systemVariables.D1PLC1CommunicationState)
                {
                    //堆取料机PLC断线
                    DataManager.Instance.InsertHistoryWarningMc("斗轮机PLC1断线", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.D1PLC1CommunicationState), "斗轮机PLC1断线",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.D1PLC1CommunicationState &&
                         _systemVariables.D1PLC1CommunicationState == false)
                {
                    //堆取料机PLC1断线解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮机PLC1断线解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.D1PLC1CommunicationState));
                }

                if (newSystemVariables.D1PLC2CommunicationState == false && _systemVariables.D1PLC2CommunicationState)
                {
                    //堆取料机PLC断线
                    DataManager.Instance.InsertHistoryWarningMc("无人值守PLC2断线", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.D1PLC2CommunicationState), "无人值守PLC2断线",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.D1PLC2CommunicationState &&
                         _systemVariables.D1PLC2CommunicationState == false)
                {
                    //堆取料机PLC2断线解除
                    DataManager.Instance.InsertHistoryWarningMc("无人值守PLC2断线解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.D1PLC2CommunicationState));
                }

                // 存儲警告信息
                if (newSystemVariables.DriverRoomEmergencyStopButton &&
                    _systemVariables.DriverRoomEmergencyStopButton == false)
                {
                    //司机室急停
                    DataManager.Instance.InsertHistoryWarningMc("司机室急停", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DriverRoomEmergencyStopButton), "司机室急停",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DriverRoomEmergencyStopButton == false &&
                         _systemVariables.DriverRoomEmergencyStopButton == true)
                {
                    //司机室急停解除
                    DataManager.Instance.InsertHistoryWarningMc("司机室急停解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DriverRoomEmergencyStopButton));
                }

                if (newSystemVariables.ElectricalRoomEmergencyStopButton &&
                    _systemVariables.ElectricalRoomEmergencyStopButton == false)
                {
                    //电气室急停
                    DataManager.Instance.InsertHistoryWarningMc("电气室急停", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ElectricalRoomEmergencyStopButton), "电气室急停",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.ElectricalRoomEmergencyStopButton == false &&
                         _systemVariables.ElectricalRoomEmergencyStopButton == true)
                {
                    //电气室急停解除
                    DataManager.Instance.InsertHistoryWarningMc("电气室急停解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.ElectricalRoomEmergencyStopButton));
                }

                if (newSystemVariables.EmergencyStopRelay == false && _systemVariables.EmergencyStopRelay == true)
                {
                    //急停继电器
                    DataManager.Instance.InsertHistoryWarningMc("急停继电器", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.EmergencyStopRelay), "急停继电器",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.EmergencyStopRelay == true && _systemVariables.EmergencyStopRelay == false)
                {
                    //急停继电器解除
                    DataManager.Instance.InsertHistoryWarningMc("急停继电器解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.EmergencyStopRelay));
                }

                if (newSystemVariables.RemoteEmergencyStop && _systemVariables.RemoteEmergencyStop == false)
                {
                    //远程急停
                    DataManager.Instance.InsertHistoryWarningMc("远程急停", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RemoteEmergencyStop), "远程急停",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RemoteEmergencyStop == false &&
                         _systemVariables.RemoteEmergencyStop == true)
                {
                    //远程急停解除
                    DataManager.Instance.InsertHistoryWarningMc("远程急停解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RemoteEmergencyStop));
                }

                if (newSystemVariables.BucketWheelFault && _systemVariables.BucketWheelFault == false)
                {
                    //斗轮机故障
                    DataManager.Instance.InsertHistoryWarningMc("斗轮机故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelFault), "斗轮机故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelFault == false && _systemVariables.BucketWheelFault == true)
                {
                    //斗轮机故障解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮机故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelFault));
                }

                if (newSystemVariables.CentralControlRoomNoStackingOrDiversionCommand &&
                    _systemVariables.CentralControlRoomNoStackingOrDiversionCommand == false)
                {
                    //中控室没有允许堆料或分流命令
                    DataManager.Instance.InsertHistoryWarningMc("中控室没有允许堆取料或分流命令", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CentralControlRoomNoStackingOrDiversionCommand),
                        "中控室没有允许堆取料或分流命令",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.CentralControlRoomNoStackingOrDiversionCommand == false &&
                         _systemVariables.CentralControlRoomNoStackingOrDiversionCommand == true)
                {
                    //中控室没有允许堆取料或分流命令解除
                    DataManager.Instance.InsertHistoryWarningMc("中控室没有允许堆取料或分流命令解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.CentralControlRoomNoStackingOrDiversionCommand));
                }

                if (newSystemVariables.LargeCarFault && _systemVariables.LargeCarFault == false)
                {
                    //大车-大车故障
                    DataManager.Instance.InsertHistoryWarningMc("大车-大车故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarFault), "大车-大车故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarFault == false && _systemVariables.LargeCarFault == true)
                {
                    //大车-大车故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-大车故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarFault));
                }

                if (newSystemVariables.LargeCarFrequencyConverterFault &&
                    _systemVariables.LargeCarFrequencyConverterFault == false)
                {
                    //大车-变频器故障
                    DataManager.Instance.InsertHistoryWarningMc("大车-变频器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarFrequencyConverterFault), "大车-变频器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarFrequencyConverterFault == false &&
                         _systemVariables.LargeCarFrequencyConverterFault == true)
                {
                    //大车-变频器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-变频器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarFrequencyConverterFault));
                }

                if (newSystemVariables.LargeCarBrakeResistorOverheatSwitch == false &&
                    _systemVariables.LargeCarBrakeResistorOverheatSwitch == true)
                {
                    //大车-制动电阻超温
                    DataManager.Instance.InsertHistoryWarningMc("大车-制动电阻超温", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarBrakeResistorOverheatSwitch),
                        "大车-制动电阻超温",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarBrakeResistorOverheatSwitch == true &&
                         _systemVariables.LargeCarBrakeResistorOverheatSwitch == false)
                {
                    //大车-制动电阻超温解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-制动电阻超温解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarBrakeResistorOverheatSwitch));
                }

                // if (newSystemVariables.LargeCarCentralizedLubricationLowOilLevel &&
                //     _systemVariables.LargeCarCentralizedLubricationLowOilLevel == false)
                // {
                //     //大车-大车集中润滑低油位
                //     DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑低油位", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarCentralizedLubricationLowOilLevel),
                //         "大车-大车集中润滑低油位",
                //         Machine.BucketWheelStackerReclaimer, false, "");
                // }
                // else if (newSystemVariables.LargeCarCentralizedLubricationLowOilLevel == false &&
                //          _systemVariables.LargeCarCentralizedLubricationLowOilLevel == true)
                // {
                //     //大车-大车集中润滑低油位解除
                //     DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑低油位解除", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     RemoveWarningDesDict(nameof(newSystemVariables.LargeCarCentralizedLubricationLowOilLevel));
                // }

                // if (newSystemVariables.LargeCarCentralizedLubricationOilBlockage &&
                //     _systemVariables.LargeCarCentralizedLubricationOilBlockage == false)
                // {
                //     //大车-大车集中润滑堵油
                //     DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑堵油", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarCentralizedLubricationOilBlockage),
                //         "大车-大车集中润滑堵油",
                //         Machine.BucketWheelStackerReclaimer, false, "");
                // }
                // else if (newSystemVariables.LargeCarCentralizedLubricationOilBlockage == false &&
                //          _systemVariables.LargeCarCentralizedLubricationOilBlockage == true)
                // {
                //     //大车-大车集中润滑堵油解除
                //     DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑堵油解除", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     RemoveWarningDesDict(nameof(newSystemVariables.LargeCarCentralizedLubricationOilBlockage));
                // }

                if (newSystemVariables.LargeCarForwardLimit && _systemVariables.LargeCarForwardLimit == false)
                {
                    //大车-前进限位
                    DataManager.Instance.InsertHistoryWarningMc("大车-前进限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarForwardLimit), "大车-前进限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarForwardLimit == false &&
                         _systemVariables.LargeCarForwardLimit == true)
                {
                    //大车-前进限位解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-前进限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarForwardLimit));
                }

                if (newSystemVariables.LargeCarForwardExtremeLimit &&
                    _systemVariables.LargeCarForwardExtremeLimit == false)
                {
                    //大车-前进极限
                    DataManager.Instance.InsertHistoryWarningMc("大车-前进极限", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarForwardExtremeLimit), "大车-前进极限",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarForwardExtremeLimit == false &&
                         _systemVariables.LargeCarForwardExtremeLimit == true)
                {
                    //大车-前进极限解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-前进极限解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarForwardExtremeLimit));
                }

                if (newSystemVariables.LargeCarReverseLimit && _systemVariables.LargeCarReverseLimit == false)
                {
                    //大车-后退限位
                    DataManager.Instance.InsertHistoryWarningMc("大车-后退限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarReverseLimit), "大车-后退限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarReverseLimit == false &&
                         _systemVariables.LargeCarReverseLimit == true)
                {
                    //大车-后退限位解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-后退限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarReverseLimit));
                }

                if (newSystemVariables.LargeCarReverseExtremeLimit &&
                    _systemVariables.LargeCarReverseExtremeLimit == false)
                {
                    //大车-后退极限
                    DataManager.Instance.InsertHistoryWarningMc("大车-后退极限", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarReverseExtremeLimit), "大车-后退极限",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarReverseExtremeLimit == false &&
                         _systemVariables.LargeCarReverseExtremeLimit == true)
                {
                    //大车-后退极限解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-后退极限解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarReverseExtremeLimit));
                }

                if (newSystemVariables.TwoMachineCollisionAlarm && _systemVariables.TwoMachineCollisionAlarm == false)
                {
                    //大车-两车碰撞报警
                    DataManager.Instance.InsertHistoryWarningMc("大车-两车碰撞报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TwoMachineCollisionAlarm), "大车-两车碰撞报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.TwoMachineCollisionAlarm == false &&
                         _systemVariables.TwoMachineCollisionAlarm == true)
                {
                    //大车-两车碰撞报警解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-两车碰撞报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.TwoMachineCollisionAlarm));
                }

                if (newSystemVariables.VariableAmplitudeMotorOverload &&
                    _systemVariables.VariableAmplitudeMotorOverload == false)
                {
                    //变幅-主电机过载
                    DataManager.Instance.InsertHistoryWarningMc("变幅-主电机过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeMotorOverload), "变幅-主电机过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeMotorOverload == false &&
                         _systemVariables.VariableAmplitudeMotorOverload == true)
                {
                    //变幅-主电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-主电机过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeMotorOverload));
                }

                if (newSystemVariables.VariableAmplitudeUpperLimit &&
                    _systemVariables.VariableAmplitudeUpperLimit == false)
                {
                    //变幅-上仰限位
                    DataManager.Instance.InsertHistoryWarningMc("变幅-上仰限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeUpperLimit), "变幅-上仰限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeUpperLimit == false &&
                         _systemVariables.VariableAmplitudeUpperLimit == true)
                {
                    //变幅-上仰限位解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-上仰限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeUpperLimit));
                }

                if (newSystemVariables.VariableAmplitudeUpperExtremeLimit &&
                    _systemVariables.VariableAmplitudeUpperExtremeLimit == false)
                {
                    //变幅-上仰极限
                    DataManager.Instance.InsertHistoryWarningMc("变幅-上仰极限", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeUpperExtremeLimit), "变幅-上仰极限",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeUpperExtremeLimit == false &&
                         _systemVariables.VariableAmplitudeUpperExtremeLimit == true)
                {
                    //变幅-上仰极限解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-上仰极限解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeUpperExtremeLimit));
                }

                if (newSystemVariables.VariableAmplitudeLowerLimit &&
                    _systemVariables.VariableAmplitudeLowerLimit == false)
                {
                    //变幅-下俯限位
                    DataManager.Instance.InsertHistoryWarningMc("变幅-下俯限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLowerLimit), "变幅-下俯限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeLowerLimit == false &&
                         _systemVariables.VariableAmplitudeLowerLimit == true)
                {
                    //变幅-下俯限位解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-下俯限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLowerLimit));
                }

                if (newSystemVariables.VariableAmplitudeLowerExtremeLimit &&
                    _systemVariables.VariableAmplitudeLowerExtremeLimit == false)
                {
                    //变幅-下俯极限
                    DataManager.Instance.InsertHistoryWarningMc("变幅-下俯极限", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLowerExtremeLimit), "变幅-下俯极限",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeLowerExtremeLimit == false &&
                         _systemVariables.VariableAmplitudeLowerExtremeLimit == true)
                {
                    //变幅-下俯极限解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-下俯极限解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLowerExtremeLimit));
                }

                if (newSystemVariables.VariableAmplitudeLowerForbiddenZoneLimit &&
                    _systemVariables.VariableAmplitudeLowerForbiddenZoneLimit == false)
                {
                    //变幅-下俯禁区
                    DataManager.Instance.InsertHistoryWarningMc("变幅-下俯禁区", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLowerForbiddenZoneLimit),
                        "变幅-下俯禁区",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeLowerForbiddenZoneLimit == false &&
                         _systemVariables.VariableAmplitudeLowerForbiddenZoneLimit == true)
                {
                    //变幅-下俯禁区解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-下俯禁区解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLowerForbiddenZoneLimit));
                }

                if (newSystemVariables.VariableAmplitudePumpStationOverheatAlarm &&
                    _systemVariables.VariableAmplitudePumpStationOverheatAlarm == false)
                {
                    //变幅-泵站高温报警
                    DataManager.Instance.InsertHistoryWarningMc("变幅-泵站高温报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudePumpStationOverheatAlarm),
                        "变幅-泵站高温报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableAmplitudePumpStationOverheatAlarm == false &&
                         _systemVariables.VariableAmplitudePumpStationOverheatAlarm == true)
                {
                    //变幅-泵站高温报警解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-泵站高温报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudePumpStationOverheatAlarm));
                }

                if (newSystemVariables.VariableAmplitudeOilLevelVeryLowSignal &&
                    _systemVariables.VariableAmplitudeOilLevelVeryLowSignal == false)
                {
                    //变幅-油液位低信号
                    DataManager.Instance.InsertHistoryWarningMc("变幅-油液位低信号", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilLevelVeryLowSignal),
                        "变幅-油液位低信号",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeOilLevelVeryLowSignal == false &&
                         _systemVariables.VariableAmplitudeOilLevelVeryLowSignal == true)
                {
                    //变幅-油液位低信号解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-油液位低信号解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilLevelVeryLowSignal));
                }

                if (newSystemVariables.VariableAmplitudeOilLevelLowSignal &&
                    _systemVariables.VariableAmplitudeOilLevelLowSignal == false)
                {
                    //变幅-液位超低信号
                    DataManager.Instance.InsertHistoryWarningMc("变幅-液位超低信号", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilLevelLowSignal),
                        "变幅-液位超低信号",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeOilLevelLowSignal == false &&
                         _systemVariables.VariableAmplitudeOilLevelLowSignal == true)
                {
                    //变幅-液位超低信号解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-液位超低信号解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilLevelLowSignal));
                }

                if (newSystemVariables.VariableFrequencyOilBlockageSignal &&
                    _systemVariables.VariableFrequencyOilBlockageSignal == false)
                {
                    //变幅-泵站堵油信号
                    DataManager.Instance.InsertHistoryWarningMc("变幅-泵站堵油信号", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableFrequencyOilBlockageSignal),
                        "变幅-泵站堵油信号",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableFrequencyOilBlockageSignal == false &&
                         _systemVariables.VariableFrequencyOilBlockageSignal == true)
                {
                    //变幅-泵站堵油信号解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-泵站堵油信号解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableFrequencyOilBlockageSignal));
                }

                if (newSystemVariables.RotaryFrequencyConverterFault &&
                    _systemVariables.RotaryFrequencyConverterFault == false)
                {
                    //回转-变频器故障
                    DataManager.Instance.InsertHistoryWarningMc("回转-变频器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryFrequencyConverterFault), "回转-变频器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryFrequencyConverterFault == false &&
                         _systemVariables.RotaryFrequencyConverterFault == true)
                {
                    //回转-变频器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-变频器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryFrequencyConverterFault));
                }

                if (newSystemVariables.RotaryBrakeOverload && _systemVariables.RotaryBrakeOverload == false)
                {
                    //回转-制动器过载
                    DataManager.Instance.InsertHistoryWarningMc("回转-制动器过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryBrakeOverload), "回转-制动器过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryBrakeOverload == false &&
                         _systemVariables.RotaryBrakeOverload == true)
                {
                    //回转-制动器过载解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-制动器过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryBrakeOverload));
                }

                if (newSystemVariables.RotaryFanOverload && _systemVariables.RotaryFanOverload == false)
                {
                    //回转-风机过载
                    DataManager.Instance.InsertHistoryWarningMc("回转-风机过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryFanOverload), "回转-风机过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryFanOverload == false && _systemVariables.RotaryFanOverload == true)
                {
                    //回转-风机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-风机过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryFanOverload));
                }

                if (newSystemVariables.RotaryBrakeResistorOverheatSwitch == false &&
                    _systemVariables.RotaryBrakeResistorOverheatSwitch == true)
                {
                    //回转-制动电阻超温
                    DataManager.Instance.InsertHistoryWarningMc("回转-制动电阻超温", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryBrakeResistorOverheatSwitch), "回转-制动电阻超温",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryBrakeResistorOverheatSwitch == true &&
                         _systemVariables.RotaryBrakeResistorOverheatSwitch == false)
                {
                    //回转-制动电阻超温解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-制动电阻超温解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryBrakeResistorOverheatSwitch));
                }

                if (newSystemVariables.RotaryFault && _systemVariables.RotaryFault == false)
                {
                    //回转-回转故障
                    DataManager.Instance.InsertHistoryWarningMc("回转-回转故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryFault), "回转-回转故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryFault == false && _systemVariables.RotaryFault == true)
                {
                    //回转-回转故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-回转故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryFault));
                }

                if (newSystemVariables.RotaryLeftTurnLimit && _systemVariables.RotaryLeftTurnLimit == false)
                {
                    //回转-左转限位
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnLimit), "回转-左转限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryLeftTurnLimit == false &&
                         _systemVariables.RotaryLeftTurnLimit == true)
                {
                    //回转-左转限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnLimit));
                }

                if (newSystemVariables.RotaryLeftTurnExtremeLimit &&
                    _systemVariables.RotaryLeftTurnExtremeLimit == false)
                {
                    //回转-左转极限
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转极限", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnExtremeLimit), "回转-左转极限",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryLeftTurnExtremeLimit == false &&
                         _systemVariables.RotaryLeftTurnExtremeLimit == true)
                {
                    //回转-左转极限解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转极限解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnExtremeLimit));
                }

                if (newSystemVariables.RotaryLeftTurnForbiddenZoneLimit &&
                    _systemVariables.RotaryLeftTurnForbiddenZoneLimit == false)
                {
                    //回转-左转禁区限位
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转禁区限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnForbiddenZoneLimit), "回转-左转禁区限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryLeftTurnForbiddenZoneLimit == false &&
                         _systemVariables.RotaryLeftTurnForbiddenZoneLimit == true)
                {
                    //回转-左转禁区限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转禁区限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnForbiddenZoneLimit));
                }

                if (newSystemVariables.RotaryRightTurnLimit && _systemVariables.RotaryRightTurnLimit == false)
                {
                    //回转-右转限位
                    DataManager.Instance.InsertHistoryWarningMc("回转-右转限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryRightTurnLimit), "回转-右转限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryRightTurnLimit == false &&
                         _systemVariables.RotaryRightTurnLimit == true)
                {
                    //回转-右转限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-右转限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryRightTurnLimit));
                }

                if (newSystemVariables.RotaryRightTurnExtremeLimit &&
                    _systemVariables.RotaryRightTurnExtremeLimit == false)
                {
                    //回转-右转极限
                    DataManager.Instance.InsertHistoryWarningMc("回转-右转极限", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryRightTurnExtremeLimit), "回转-右转极限",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryRightTurnExtremeLimit == false &&
                         _systemVariables.RotaryRightTurnExtremeLimit == true)
                {
                    //回转-右转极限解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-右转极限解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryRightTurnExtremeLimit));
                }
                //要求删除
                // if (newSystemVariables.RotaryRightTurnForbiddenZoneLimit &&
                //     _systemVariables.RotaryRightTurnForbiddenZoneLimit == false)
                // {
                //     //回转-右转禁区限位
                //     DataManager.Instance.InsertHistoryWarningMc("回转-右转禁区限位", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryRightTurnForbiddenZoneLimit), "回转-右转禁区限位",
                //         Machine.BucketWheelStackerReclaimer, false, "");
                // }
                // else if (newSystemVariables.RotaryRightTurnForbiddenZoneLimit == false &&
                //          _systemVariables.RotaryRightTurnForbiddenZoneLimit == true)
                // {
                //     //回转-右转禁区限位解除
                //     DataManager.Instance.InsertHistoryWarningMc("回转-右转禁区限位解除", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     RemoveWarningDesDict(nameof(newSystemVariables.RotaryRightTurnForbiddenZoneLimit));
                // }

                if (newSystemVariables.RotaryRightTurnForbiddenLimit &&
                    _systemVariables.RotaryRightTurnForbiddenLimit == false)
                {
                    //回转-右转防撞限位
                    DataManager.Instance.InsertHistoryWarningMc("回转-右转防撞限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryRightTurnForbiddenLimit), "回转-右转防撞限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryRightTurnForbiddenLimit == false &&
                         _systemVariables.RotaryRightTurnForbiddenLimit == true)
                {
                    //回转-右转防撞限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-右转防撞限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryRightTurnForbiddenLimit));
                }

                if (newSystemVariables.RotaryOverTorque && _systemVariables.RotaryOverTorque == false)
                {
                    //回转-回转过力矩
                    DataManager.Instance.InsertHistoryWarningMc("回转-回转过力矩", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryOverTorque), "回转-回转过力矩",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryOverTorque == false && _systemVariables.RotaryOverTorque == true)
                {
                    //回转-回转过力矩解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-回转过力矩解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryOverTorque));
                }

                // if (newSystemVariables.RotaryCentralizedLubricationOilBlockageFault &&
                //     _systemVariables.RotaryCentralizedLubricationOilBlockageFault == false)
                // {
                //     //回转-回转集中润滑堵油
                //     DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑堵油", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationOilBlockageFault),
                //         "回转-回转集中润滑堵油",
                //         Machine.BucketWheelStackerReclaimer, false, "");
                // }
                // else if (newSystemVariables.RotaryCentralizedLubricationOilBlockageFault == false &&
                //          _systemVariables.RotaryCentralizedLubricationOilBlockageFault == true)
                // {
                //     //回转-回转集中润滑堵油解除
                //     DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑堵油解除", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     RemoveWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationOilBlockageFault));
                // }

                // if (newSystemVariables.RotaryCentralizedLubricationLowOilLevelFault &&
                //     _systemVariables.RotaryCentralizedLubricationLowOilLevelFault == false)
                // {
                //     //回转-回转集中润滑低油位
                //     DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑低油位", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationLowOilLevelFault),
                //         "回转-回转集中润滑低油位",
                //         Machine.BucketWheelStackerReclaimer, false, "");
                // }
                // else if (newSystemVariables.RotaryCentralizedLubricationLowOilLevelFault == false &&
                //          _systemVariables.RotaryCentralizedLubricationLowOilLevelFault == true)
                // {
                //     //回转-回转集中润滑低油位解除
                //     DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑低油位解除", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     RemoveWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationLowOilLevelFault));
                // }

                if (newSystemVariables.BucketWheelMotorOverload && _systemVariables.BucketWheelMotorOverload == false)
                {
                    //斗轮/槽-电机过载
                    DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-电机过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelMotorOverload), "斗轮/槽-电机过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelMotorOverload == false &&
                         _systemVariables.BucketWheelMotorOverload == true)
                {
                    //斗轮/槽-电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-电机过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelMotorOverload));
                }

                if (newSystemVariables.BucketWheelOverTorqueSwitch &&
                    _systemVariables.BucketWheelOverTorqueSwitch == false)
                {
                    //斗轮/槽-斗轮过力矩开关
                    DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-斗轮过力矩开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelOverTorqueSwitch), "斗轮/槽-斗轮过力矩开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelOverTorqueSwitch == false &&
                         _systemVariables.BucketWheelOverTorqueSwitch == true)
                {
                    //斗轮/槽-斗轮过力矩开关解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-斗轮过力矩开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelOverTorqueSwitch));
                }

                if (newSystemVariables.BucketWheelSlotMotorOverload &&
                    _systemVariables.BucketWheelSlotMotorOverload == false)
                {
                    //斗轮导料槽-电机过载
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽-电机过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelSlotMotorOverload), "斗轮/槽-电机过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelSlotMotorOverload == false &&
                         _systemVariables.BucketWheelSlotMotorOverload == true)
                {
                    //斗轮导料槽-电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽-电机过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelSlotMotorOverload));
                }

                if (newSystemVariables.SuspensionBeltMotorOverload &&
                    _systemVariables.SuspensionBeltMotorOverload == false)
                {
                    //悬胶/挡板-电机过载
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-电机过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltMotorOverload), "悬胶/挡板-电机过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspensionBeltMotorOverload == false &&
                         _systemVariables.SuspensionBeltMotorOverload == true)
                {
                    //悬胶/挡板-电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-电机过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltMotorOverload));
                }

                if (newSystemVariables.SuspensionBeltFirstLevelDeviationSwitch &&
                    _systemVariables.SuspensionBeltFirstLevelDeviationSwitch == false)
                {
                    //悬胶/挡板-一级跑偏开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-一级跑偏开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltFirstLevelDeviationSwitch),
                        "悬胶/挡板-一级跑偏开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspensionBeltFirstLevelDeviationSwitch == false &&
                         _systemVariables.SuspensionBeltFirstLevelDeviationSwitch == true)
                {
                    //悬胶/挡板-一级跑偏开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-一级跑偏开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltFirstLevelDeviationSwitch));
                }

                if (newSystemVariables.SuspensionBeltSecondLevelDeviationSwitch &&
                    _systemVariables.SuspensionBeltSecondLevelDeviationSwitch == false)
                {
                    //悬胶/挡板-二级跑偏开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-二级跑偏开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltSecondLevelDeviationSwitch),
                        "悬胶/挡板-二级跑偏开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspensionBeltSecondLevelDeviationSwitch == false &&
                         _systemVariables.SuspensionBeltSecondLevelDeviationSwitch == true)
                {
                    //悬胶/挡板-二级跑偏开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-二级跑偏开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltSecondLevelDeviationSwitch));
                }

                if (newSystemVariables.SuspendedBeltSlip && _systemVariables.SuspendedBeltSlip == false)
                {
                    //悬胶/挡板-打滑检测开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-打滑检测开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltSlip), "悬胶/挡板-打滑检测开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspendedBeltSlip == false && _systemVariables.SuspendedBeltSlip == true)
                {
                    //悬胶/挡板-打滑检测开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-打滑检测开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltSlip));
                }

                if (newSystemVariables.SuspensionBeltLongitudinalTearSwitch &&
                    _systemVariables.SuspensionBeltLongitudinalTearSwitch == false)
                {
                    //悬胶/挡板-纵向撕裂开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-纵向撕裂开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltLongitudinalTearSwitch),
                        "悬胶/挡板-纵向撕裂开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspensionBeltLongitudinalTearSwitch == false &&
                         _systemVariables.SuspensionBeltLongitudinalTearSwitch == true)
                {
                    //悬胶/挡板-纵向撕裂开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-纵向撕裂开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltLongitudinalTearSwitch));
                }

                if (newSystemVariables.SuspensionBeltEmergencyStopSwitch &&
                    _systemVariables.SuspensionBeltEmergencyStopSwitch == false)
                {
                    //悬胶/挡板-急停拉线开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-急停拉线开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltEmergencyStopSwitch),
                        "悬胶/挡板-急停拉线开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspensionBeltEmergencyStopSwitch == false &&
                         _systemVariables.SuspensionBeltEmergencyStopSwitch == true)
                {
                    //悬胶/挡板-急停拉线开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-急停拉线开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltEmergencyStopSwitch));
                }

                if (newSystemVariables.SuspensionBeltMaterialFlowDetectionSwitch &&
                    _systemVariables.SuspensionBeltMaterialFlowDetectionSwitch == false)
                {
                    //悬胶/挡板-料流检测开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-料流检测开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltMaterialFlowDetectionSwitch),
                        "悬胶/挡板-料流检测开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspensionBeltMaterialFlowDetectionSwitch == false &&
                         _systemVariables.SuspensionBeltMaterialFlowDetectionSwitch == true)
                {
                    //悬胶/挡板-料流检测开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-料流检测开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltMaterialFlowDetectionSwitch));
                }

                if (newSystemVariables.CentralMaterialDustDetectionSwitch == false &&
                    _systemVariables.CentralMaterialDustDetectionSwitch == true)
                {
                    //悬胶/挡板-中部料斗堵煤
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-中部料斗堵煤", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CentralMaterialDustDetectionSwitch),
                        "悬胶/挡板-中部料斗堵煤",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.CentralMaterialDustDetectionSwitch == true &&
                         _systemVariables.CentralMaterialDustDetectionSwitch == false)
                {
                    //悬胶/挡板-中部料斗堵煤解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-中部料斗堵煤解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.CentralMaterialDustDetectionSwitch));
                }

                if (newSystemVariables.DiversionBaffleMotorOverload &&
                    _systemVariables.DiversionBaffleMotorOverload == false)
                {
                    //分流挡板-电机过载
                    DataManager.Instance.InsertHistoryWarningMc("分流挡板-电机过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DiversionBaffleMotorOverload), "分流挡板-电机过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DiversionBaffleMotorOverload == false &&
                         _systemVariables.DiversionBaffleMotorOverload == true)
                {
                    //分流挡板-电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("分流挡板-电机过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DiversionBaffleMotorOverload));
                }

                if (newSystemVariables.DiversionPlateTimeout &&
                    _systemVariables.DiversionPlateTimeout == false)
                {
                    //分流挡板运行超时
                    DataManager.Instance.InsertHistoryWarningMc("分流挡板运行超时", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DiversionPlateTimeout), "分流挡板运行超时",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DiversionPlateTimeout == false &&
                         _systemVariables.DiversionPlateTimeout == true)
                {
                    //分流挡板运行超时
                    DataManager.Instance.InsertHistoryWarningMc("分流挡板运行超时解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DiversionPlateTimeout));
                }

                if (newSystemVariables.CableReelMotorOverload && _systemVariables.CableReelMotorOverload == false)
                {
                    //夹轨/卷筒-电缆卷筒-卷筒电机过载
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒电机过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CableReelMotorOverload), "电缆卷筒-卷筒电机过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.CableReelMotorOverload == false &&
                         _systemVariables.CableReelMotorOverload == true)
                {
                    //夹轨/卷筒-电缆卷筒-卷筒电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒电机过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.CableReelMotorOverload));
                }

                if (newSystemVariables.ReelOverTensionLimit1 == false && newSystemVariables.RollerOverTightLimit2 &&
                    (_systemVariables.ReelOverTensionLimit1 == true || _systemVariables.RollerOverTightLimit2 == false))
                {
                    //夹轨/卷筒-电缆卷筒-卷筒过紧限位1
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧1过紧2限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ReelOverTensionLimit1), "电缆卷筒-卷筒过紧1过紧2限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if ((newSystemVariables.ReelOverTensionLimit1 == true &&
                          _systemVariables.ReelOverTensionLimit1 == false) ||
                         (newSystemVariables.RollerOverTightLimit2 == false &&
                          _systemVariables.RollerOverTightLimit2 == true))
                {
                    if (WarningCellDataDict.ContainsKey(nameof(newSystemVariables.ReelOverTensionLimit1)))
                    {
                        //夹轨/卷筒-电缆卷筒-卷筒过紧限位1解除
                        DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧1过紧2限位解除", GetUserName(),
                            Machine.BucketWheelStackerReclaimer);
                    }

                    RemoveWarningDesDict(nameof(newSystemVariables.ReelOverTensionLimit1));
                }

                if (newSystemVariables.ReelOverLooseLimit1 && newSystemVariables.RollerOverLooseLimit2 &&
                    (_systemVariables.ReelOverLooseLimit1 == false || _systemVariables.RollerOverLooseLimit2 == false))
                {
                    //夹轨/卷筒-电缆卷筒-卷筒过松限位1
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松1过松2限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ReelOverLooseLimit1), "电缆卷筒-卷筒过松1过松2限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if ((newSystemVariables.ReelOverLooseLimit1 == false &&
                          _systemVariables.ReelOverLooseLimit1 == true) ||
                         (newSystemVariables.RollerOverLooseLimit2 == false &&
                          _systemVariables.RollerOverLooseLimit2 == true))
                {
                    if (WarningCellDataDict.ContainsKey(nameof(newSystemVariables.ReelOverLooseLimit1)))
                    {
                        //夹轨/卷筒-电缆卷筒-卷筒过松限位1解除
                        DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松1过松2限位解除", GetUserName(),
                            Machine.BucketWheelStackerReclaimer);
                    }

                    RemoveWarningDesDict(nameof(newSystemVariables.ReelOverLooseLimit1));
                }

                // if (newSystemVariables.RollerOverTightLimit2 && _systemVariables.RollerOverTightLimit2 == false)
                // {
                //     //夹轨/卷筒-电缆卷筒-卷筒过紧限位2
                //     DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧限位2", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.RollerOverTightLimit2), "电缆卷筒-卷筒过紧限位2",
                //         Machine.BucketWheelStackerReclaimer, false, "");
                // }
                // else if (newSystemVariables.RollerOverTightLimit2 == false &&
                //          _systemVariables.RollerOverTightLimit2 == true)
                // {
                //     //夹轨/卷筒-电缆卷筒-卷筒过紧限位2解除
                //     DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧限位2解除", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     RemoveWarningDesDict(nameof(newSystemVariables.RollerOverTightLimit2));
                // }
                //
                // if (newSystemVariables.RollerOverLooseLimit2 && _systemVariables.RollerOverLooseLimit2 == false)
                // {
                //     //夹轨/卷筒-电缆卷筒-卷筒过松限位2
                //     DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松限位2", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.RollerOverLooseLimit2), "电缆卷筒-卷筒过松限位2",
                //         Machine.BucketWheelStackerReclaimer, false, "");
                // }
                // else if (newSystemVariables.RollerOverLooseLimit2 == false &&
                //          _systemVariables.RollerOverLooseLimit2 == true)
                // {
                //     //夹轨/卷筒-电缆卷筒-卷筒过松限位2解除
                //     DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松限位2解除", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     RemoveWarningDesDict(nameof(newSystemVariables.RollerOverLooseLimit2));
                // }

                if (newSystemVariables.ReelEmptySwitch && _systemVariables.ReelEmptySwitch == false)
                {
                    //夹轨/卷筒-电缆卷筒-卷筒空盘开关
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒空盘开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ReelEmptySwitch), "电缆卷筒-卷筒空盘开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.ReelEmptySwitch == false && _systemVariables.ReelEmptySwitch == true)
                {
                    //夹轨/卷筒-电缆卷筒-卷筒空盘开关解除
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒空盘开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.ReelEmptySwitch));
                }

                if (newSystemVariables.DryFogSystemLowAirPressure &&
                    _systemVariables.DryFogSystemLowAirPressure == false)
                {
                    //抑尘振打-洒水抑尘-干雾系统气压低
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统气压低", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DryFogSystemLowAirPressure), "洒水抑尘-干雾系统气压低",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DryFogSystemLowAirPressure == false &&
                         _systemVariables.DryFogSystemLowAirPressure == true)
                {
                    //抑尘振打-洒水抑尘-干雾系统气压低解除
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统气压低解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DryFogSystemLowAirPressure));
                }

                if (newSystemVariables.DryFogSystemLowWaterPressure &&
                    _systemVariables.DryFogSystemLowWaterPressure == false)
                {
                    //抑尘振打-洒水抑尘-干雾系统水压低
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统水压低", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DryFogSystemLowWaterPressure), "洒水抑尘-干雾系统水压低",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DryFogSystemLowWaterPressure == false &&
                         _systemVariables.DryFogSystemLowWaterPressure == true)
                {
                    //抑尘振打-洒水抑尘-干雾系统水压低解除
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统水压低解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DryFogSystemLowWaterPressure));
                }

                if (newSystemVariables.DryFogSystemFilterClogged && _systemVariables.DryFogSystemFilterClogged == false)
                {
                    //抑尘振打-洒水抑尘-干雾系统过滤器堵塞
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统过滤器堵塞", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DryFogSystemFilterClogged), "洒水抑尘-干雾系统过滤器堵塞",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DryFogSystemFilterClogged == false &&
                         _systemVariables.DryFogSystemFilterClogged == true)
                {
                    //抑尘振打-洒水抑尘-干雾系统过滤器堵塞解除
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统过滤器堵塞解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DryFogSystemFilterClogged));
                }

                if (newSystemVariables.WaterTankLowLevelSwitch && _systemVariables.WaterTankLowLevelSwitch == false)
                {
                    //抑尘振打-洒水抑尘-水箱液位低开关
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-水箱液位低开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.WaterTankLowLevelSwitch), "洒水抑尘-水箱液位低开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.WaterTankLowLevelSwitch == false &&
                         _systemVariables.WaterTankLowLevelSwitch == true)
                {
                    //抑尘振打-洒水抑尘-水箱液位低开关解除
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-水箱液位低开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.WaterTankLowLevelSwitch));
                }

                if (newSystemVariables.VibrationMotorOverload && _systemVariables.VibrationMotorOverload == false)
                {
                    //抑尘振打-振打电机-振打电机过载
                    DataManager.Instance.InsertHistoryWarningMc("振打电机-振打电机过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VibrationMotorOverload), "振打电机-振打电机过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VibrationMotorOverload == false &&
                         _systemVariables.VibrationMotorOverload == true)
                {
                    //抑尘振打-振打电机-振打电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("振打电机-振打电机过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VibrationMotorOverload));
                }

                if (newSystemVariables.ClampingDeviceMotorOverload &&
                    _systemVariables.ClampingDeviceMotorOverload == false)
                {
                    //夹轨/卷筒-夹轨器-电机过载
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器-电机过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ClampingDeviceMotorOverload), "夹轨器-电机过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.ClampingDeviceMotorOverload == false &&
                         _systemVariables.ClampingDeviceMotorOverload == true)
                {
                    //夹轨/卷筒-夹轨器-电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器-电机过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.ClampingDeviceMotorOverload));
                }

                if (newSystemVariables.ClampFault && _systemVariables.ClampFault == false)
                {
                    //夹轨/卷筒-夹轨器故障
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ClampFault), "夹轨器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.ClampFault == false && _systemVariables.ClampFault == true)
                {
                    //夹轨/卷筒-夹轨器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.ClampFault));
                }

                if (newSystemVariables.TailCarDrivenRollerBearingUpperLimitAlarm &&
                    _systemVariables.TailCarDrivenRollerBearingUpperLimitAlarm == false)
                {
                    //尾车胶带-尾车从动滚筒轴承上限报警
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承上限报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarDrivenRollerBearingUpperLimitAlarm),
                        "尾车胶带-尾车从动滚筒轴承上限报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.TailCarDrivenRollerBearingUpperLimitAlarm == false &&
                         _systemVariables.TailCarDrivenRollerBearingUpperLimitAlarm == true)
                {
                    //尾车胶带-尾车从动滚筒轴承上限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承上限报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarDrivenRollerBearingUpperLimitAlarm));
                }

                if (newSystemVariables.TailCarDrivenRollerBearingLowerLimitAlarm &&
                    _systemVariables.TailCarDrivenRollerBearingLowerLimitAlarm == false)
                {
                    //尾车胶带-尾车从动滚筒轴承下限报警
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承下限报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarDrivenRollerBearingLowerLimitAlarm),
                        "尾车胶带-尾车从动滚筒轴承下限报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.TailCarDrivenRollerBearingLowerLimitAlarm == false &&
                         _systemVariables.TailCarDrivenRollerBearingLowerLimitAlarm == true)
                {
                    //尾车胶带-尾车从动滚筒轴承下限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承下限报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarDrivenRollerBearingLowerLimitAlarm));
                }

                if (newSystemVariables.TailCarFirstLevelDeviationSwitch &&
                    _systemVariables.TailCarFirstLevelDeviationSwitch == false)
                {
                    //尾车胶带-尾车一级跑偏开关
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车一级跑偏开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarFirstLevelDeviationSwitch),
                        "尾车胶带-尾车一级跑偏开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.TailCarFirstLevelDeviationSwitch == false &&
                         _systemVariables.TailCarFirstLevelDeviationSwitch == true)
                {
                    //尾车胶带-尾车一级跑偏开关解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车一级跑偏开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarFirstLevelDeviationSwitch));
                }

                if (newSystemVariables.TailCarSecondLevelDeviationSwitch &&
                    _systemVariables.TailCarSecondLevelDeviationSwitch == false)
                {
                    //尾车胶带-尾车二级跑偏开关
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车二级跑偏开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarSecondLevelDeviationSwitch),
                        "尾车胶带-尾车二级跑偏开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.TailCarSecondLevelDeviationSwitch == false &&
                         _systemVariables.TailCarSecondLevelDeviationSwitch == true)
                {
                    //尾车胶带-尾车二级跑偏开关解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车二级跑偏开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarSecondLevelDeviationSwitch));
                }

                if (newSystemVariables.TailCarEmergencyStopSwitch &&
                    _systemVariables.TailCarEmergencyStopSwitch == false)
                {
                    //尾车胶带-尾车急停拉线开关
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车急停拉线开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarEmergencyStopSwitch), "尾车胶带-尾车急停拉线开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.TailCarEmergencyStopSwitch == false &&
                         _systemVariables.TailCarEmergencyStopSwitch == true)
                {
                    //尾车胶带-尾车急停拉线开关解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车急停拉线开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarEmergencyStopSwitch));
                }

                if (newSystemVariables.TailCarBeltLongitudinalTearing &&
                    _systemVariables.TailCarBeltLongitudinalTearing == false)
                {
                    //尾车胶带-尾车胶带纵向撕裂
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车胶带纵向撕裂", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarBeltLongitudinalTearing),
                        "尾车胶带-尾车胶带纵向撕裂",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.TailCarBeltLongitudinalTearing == false &&
                         _systemVariables.TailCarBeltLongitudinalTearing == true)
                {
                    //尾车胶带-尾车胶带纵向撕裂解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车胶带纵向撕裂解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarBeltLongitudinalTearing));
                }

                if (newSystemVariables.OverBelt_R_Limit && _systemVariables.OverBelt_R_Limit == false)
                {
                    // 回转右转过皮带保护限位
                    DataManager.Instance.InsertHistoryWarningMc("回转右转过皮带保护限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.OverBelt_R_Limit), "回转右转过皮带保护限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.OverBelt_R_Limit == false && _systemVariables.OverBelt_R_Limit == true)
                {
                    // 回转右转过皮带保护限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转右转过皮带保护限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.OverBelt_R_Limit));
                }

                if (newSystemVariables.OverBelt_L_Limit && _systemVariables.OverBelt_L_Limit == false)
                {
                    // 回转左转过皮带保护限位
                    DataManager.Instance.InsertHistoryWarningMc("回转左转过皮带保护限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.OverBelt_L_Limit), "回转左转过皮带保护限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.OverBelt_L_Limit == false && _systemVariables.OverBelt_L_Limit == true)
                {
                    // 回转左转过皮带保护限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转左转过皮带保护限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.OverBelt_L_Limit));
                }

                if (newSystemVariables.OverBelt_D_Limit && _systemVariables.OverBelt_D_Limit == false)
                {
                    // 回转过皮带俯仰下限位保护
                    DataManager.Instance.InsertHistoryWarningMc("回转过皮带俯仰下限位保护", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.OverBelt_D_Limit), "回转过皮带俯仰下限位保护",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.OverBelt_D_Limit == false && _systemVariables.OverBelt_D_Limit == true)
                {
                    // 回转过皮带俯仰下限位保护解除
                    DataManager.Instance.InsertHistoryWarningMc("回转过皮带俯仰下限位保护解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.OverBelt_D_Limit));
                }

                if (newSystemVariables.OverBelt_R_SoftLimit && _systemVariables.OverBelt_R_SoftLimit == false)
                {
                    // 回转右转过皮带保护软限位
                    DataManager.Instance.InsertHistoryWarningMc("回转右转过皮带保护软限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.OverBelt_R_SoftLimit), "回转右转过皮带保护软限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.OverBelt_R_SoftLimit == false &&
                         _systemVariables.OverBelt_R_SoftLimit == true)
                {
                    // 回转右转过皮带保护软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转右转过皮带保护软限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.OverBelt_R_SoftLimit));
                }

                if (newSystemVariables.OverBelt_L_SoftLimit && _systemVariables.OverBelt_L_SoftLimit == false)
                {
                    // 回转左转过皮带保护软限位
                    DataManager.Instance.InsertHistoryWarningMc("回转左转过皮带保护软限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.OverBelt_L_SoftLimit), "回转左转过皮带保护软限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.OverBelt_L_SoftLimit == false &&
                         _systemVariables.OverBelt_L_SoftLimit == true)
                {
                    // 回转左转过皮带保护软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转左转过皮带保护软限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.OverBelt_L_SoftLimit));
                }

                if (newSystemVariables.OverBelt_D_SoftLimit && _systemVariables.OverBelt_D_SoftLimit == false)
                {
                    // 回转过皮带俯仰下限软限位
                    DataManager.Instance.InsertHistoryWarningMc("回转过皮带俯仰下限软限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.OverBelt_D_SoftLimit), "回转过皮带俯仰下限软限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.OverBelt_D_SoftLimit == false &&
                         _systemVariables.OverBelt_D_SoftLimit == true)
                {
                    // 回转过皮带俯仰下限软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转过皮带俯仰下限软限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.OverBelt_D_SoftLimit));
                }

                if (newSystemVariables.DC_FWD_SoftLimit && _systemVariables.DC_FWD_SoftLimit == false)
                {
                    // 大车前进停止软限位
                    DataManager.Instance.InsertHistoryWarningMc("大车前进停止软限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_FWD_SoftLimit), "大车前进停止软限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DC_FWD_SoftLimit == false && _systemVariables.DC_FWD_SoftLimit == true)
                {
                    // 大车前进停止软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("大车前进停止软限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_FWD_SoftLimit));
                }

                if (newSystemVariables.DcFWD_LimitStatus && _systemVariables.DcFWD_LimitStatus == false)
                {
                    // 大车前进限位集合
                    DataManager.Instance.InsertHistoryWarningMc("大车前进限位集合", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DcFWD_LimitStatus), "大车前进限位集合",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DcFWD_LimitStatus == false && _systemVariables.DcFWD_LimitStatus == true)
                {
                    // 大车前进限位集合解除
                    DataManager.Instance.InsertHistoryWarningMc("大车前进限位集合解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DcFWD_LimitStatus));
                }

                if (newSystemVariables.DC_REV_SoftLimit && _systemVariables.DC_REV_SoftLimit == false)
                {
                    // 大车后退停止软限位
                    DataManager.Instance.InsertHistoryWarningMc("大车后退停止软限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_REV_SoftLimit), "大车后退停止软限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DC_REV_SoftLimit == false && _systemVariables.DC_REV_SoftLimit == true)
                {
                    // 大车后退停止软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("大车后退停止软限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_REV_SoftLimit));
                }

                if (newSystemVariables.DcREV_LimitStatus && _systemVariables.DcREV_LimitStatus == false)
                {
                    // 大车后退限位集合
                    DataManager.Instance.InsertHistoryWarningMc("大车后退限位集合", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DcREV_LimitStatus), "大车后退限位集合",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DcREV_LimitStatus == false && _systemVariables.DcREV_LimitStatus == true)
                {
                    // 大车后退限位集合解除
                    DataManager.Instance.InsertHistoryWarningMc("大车后退限位集合解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DcREV_LimitStatus));
                }

                if (newSystemVariables.Slew_R_SoftLimit && _systemVariables.Slew_R_SoftLimit == false)
                {
                    // 悬臂右转软限位
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右转软限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_R_SoftLimit), "悬臂右转软限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Slew_R_SoftLimit == false && _systemVariables.Slew_R_SoftLimit == true)
                {
                    // 悬臂右转软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右转软限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_R_SoftLimit));
                }

                if (newSystemVariables.Slew_R_LimitStatus && _systemVariables.Slew_R_LimitStatus == false)
                {
                    // 悬臂右转限位集合
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右转限位集合", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_R_LimitStatus), "悬臂右转限位集合",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Slew_R_LimitStatus == false && _systemVariables.Slew_R_LimitStatus == true)
                {
                    // 悬臂右转限位集合解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右转限位集合解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_R_LimitStatus));
                }

                if (newSystemVariables.Slew_L_SoftLimit && _systemVariables.Slew_L_SoftLimit == false)
                {
                    // 悬臂左转软限位
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左转软限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_L_SoftLimit), "悬臂左转软限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Slew_L_SoftLimit == false && _systemVariables.Slew_L_SoftLimit == true)
                {
                    // 悬臂左转软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左转软限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_L_SoftLimit));
                }

                if (newSystemVariables.Slew_L_LimitStatus && _systemVariables.Slew_L_LimitStatus == false)
                {
                    // 悬臂左转限位集合
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左转限位集合", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_L_LimitStatus), "悬臂左转限位集合",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Slew_L_LimitStatus == false && _systemVariables.Slew_L_LimitStatus == true)
                {
                    // 悬臂左转限位集合解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左转限位集合解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_L_LimitStatus));
                }

                if (newSystemVariables.Luff_Up_SoftLimit && _systemVariables.Luff_Up_SoftLimit == false)
                {
                    // 俯仰上极限软限位
                    DataManager.Instance.InsertHistoryWarningMc("俯仰上极限软限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Luff_Up_SoftLimit), "俯仰上极限软限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Luff_Up_SoftLimit == false && _systemVariables.Luff_Up_SoftLimit == true)
                {
                    // 俯仰上极限软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("俯仰上极限软限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Luff_Up_SoftLimit));
                }

                if (newSystemVariables.LuffUp_LimitStatus && _systemVariables.LuffUp_LimitStatus == false)
                {
                    // 俯仰上极限限位集合
                    DataManager.Instance.InsertHistoryWarningMc("俯仰上极限限位集合", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LuffUp_LimitStatus), "俯仰上极限限位集合",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LuffUp_LimitStatus == false && _systemVariables.LuffUp_LimitStatus == true)
                {
                    // 俯仰上极限限位集合解除
                    DataManager.Instance.InsertHistoryWarningMc("俯仰上极限限位集合解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LuffUp_LimitStatus));
                }

                if (newSystemVariables.Luff_Down_SoftLimit && _systemVariables.Luff_Down_SoftLimit == false)
                {
                    // 俯仰下极限软限位
                    DataManager.Instance.InsertHistoryWarningMc("俯仰下极限软限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Luff_Down_SoftLimit), "俯仰下极限软限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Luff_Down_SoftLimit == false &&
                         _systemVariables.Luff_Down_SoftLimit == true)
                {
                    // 俯仰下极限软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("俯仰下极限软限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Luff_Down_SoftLimit));
                }

                if (newSystemVariables.LuffDown_LimitStatus && _systemVariables.LuffDown_LimitStatus == false)
                {
                    // 俯仰下极限限位集合
                    DataManager.Instance.InsertHistoryWarningMc("俯仰下极限限位集合", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LuffDown_LimitStatus), "俯仰下极限限位集合",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LuffDown_LimitStatus == false &&
                         _systemVariables.LuffDown_LimitStatus == true)
                {
                    // 俯仰下极限限位集合解除
                    DataManager.Instance.InsertHistoryWarningMc("俯仰下极限限位集合解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LuffDown_LimitStatus));
                }

                if (newSystemVariables.Slew_SAS_L_Alarm && _systemVariables.Slew_SAS_L_Alarm == false)
                {
                    // 悬臂左侧防撞保护动作
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左侧防撞保护动作", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_SAS_L_Alarm), "悬臂左侧防撞保护动作",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Slew_SAS_L_Alarm == false && _systemVariables.Slew_SAS_L_Alarm == true)
                {
                    // 悬臂左侧防撞保护动作解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左侧防撞保护动作解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_SAS_L_Alarm));
                }

                if (newSystemVariables.Slew_SAS_R_Alarm && _systemVariables.Slew_SAS_R_Alarm == false)
                {
                    // 悬臂右侧防撞保护动作
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右侧防撞保护动作", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_SAS_R_Alarm), "悬臂右侧防撞保护动作",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Slew_SAS_R_Alarm == false && _systemVariables.Slew_SAS_R_Alarm == true)
                {
                    // 悬臂右侧防撞保护动作解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右侧防撞保护动作解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_SAS_R_Alarm));
                }

                if (newSystemVariables.DC_SAS_F_Alarm && _systemVariables.DC_SAS_F_Alarm == false)
                {
                    // 大车前进防撞保护动作
                    DataManager.Instance.InsertHistoryWarningMc("大车前进防撞保护动作", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_SAS_F_Alarm), "大车前进防撞保护动作",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DC_SAS_F_Alarm == false && _systemVariables.DC_SAS_F_Alarm == true)
                {
                    // 大车前进防撞保护动作解除
                    DataManager.Instance.InsertHistoryWarningMc("大车前进防撞保护动作解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_SAS_F_Alarm));
                }

                if (newSystemVariables.DC_SAS_B_Alarm && _systemVariables.DC_SAS_B_Alarm == false)
                {
                    // 大车后退防撞保护动作
                    DataManager.Instance.InsertHistoryWarningMc("大车后退防撞保护动作", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_SAS_B_Alarm), "大车后退防撞保护动作",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DC_SAS_B_Alarm == false && _systemVariables.DC_SAS_B_Alarm == true)
                {
                    // 大车后退防撞保护动作解除
                    DataManager.Instance.InsertHistoryWarningMc("大车后退防撞保护动作解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_SAS_B_Alarm));
                }

                if (newSystemVariables.Slew_SAS_RR_Alarm && _systemVariables.Slew_SAS_RR_Alarm == false)
                {
                    // 悬臂右雷达前方有煤垛碰撞警告
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右雷达前方有煤垛碰撞警告", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_SAS_RR_Alarm), "悬臂右雷达前方有煤垛碰撞警告",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Slew_SAS_RR_Alarm == false && _systemVariables.Slew_SAS_RR_Alarm == true)
                {
                    // 悬臂右雷达前方有煤垛碰撞警告解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右雷达前方有煤垛碰撞警告解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_SAS_RR_Alarm));
                }

                if (newSystemVariables.Slew_SAS_LR_Alarm && _systemVariables.Slew_SAS_LR_Alarm == false)
                {
                    // 悬臂左雷达前方有煤垛碰撞警告
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左雷达前方有煤垛碰撞警告", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_SAS_LR_Alarm), "悬臂左雷达前方有煤垛碰撞警告",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Slew_SAS_LR_Alarm == false && _systemVariables.Slew_SAS_LR_Alarm == true)
                {
                    // 悬臂左雷达前方有煤垛碰撞警告解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左雷达前方有煤垛碰撞警告解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_SAS_LR_Alarm));
                }

                if (newSystemVariables.Slew_SAS_RU_Alarm && _systemVariables.Slew_SAS_RU_Alarm == false)
                {
                    // 悬臂右超声波前方有煤垛碰撞警告
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右超声波前方有煤垛碰撞警告", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_SAS_RU_Alarm), "悬臂右超声波前方有煤垛碰撞警告",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Slew_SAS_RU_Alarm == false && _systemVariables.Slew_SAS_RU_Alarm == true)
                {
                    // 悬臂右超声波前方有煤垛碰撞警告解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右超声波前方有煤垛碰撞警告解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_SAS_RU_Alarm));
                }

                if (newSystemVariables.Slew_SAS_LU_Alarm && _systemVariables.Slew_SAS_LU_Alarm == false)
                {
                    // 悬臂左超声波前方有煤垛碰撞警告
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左超声波前方有煤垛碰撞警告", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_SAS_LU_Alarm), "悬臂左超声波前方有煤垛碰撞警告",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Slew_SAS_LU_Alarm == false && _systemVariables.Slew_SAS_LU_Alarm == true)
                {
                    // 悬臂左超声波前方有煤垛碰撞警告解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左超声波前方有煤垛碰撞警告解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_SAS_LU_Alarm));
                }

                if (newSystemVariables.DC_SAS_RF_Alrm && _systemVariables.DC_SAS_RF_Alrm == false)
                {
                    // 大车右前方有障碍
                    DataManager.Instance.InsertHistoryWarningMc("大车右前方有障碍", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_SAS_RF_Alrm), "大车右前方有障碍",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DC_SAS_RF_Alrm == false && _systemVariables.DC_SAS_RF_Alrm == true)
                {
                    // 大车右前方有障碍解除
                    DataManager.Instance.InsertHistoryWarningMc("大车右前方有障碍解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_SAS_RF_Alrm));
                }

                if (newSystemVariables.DC_SAS_RB_Alrm && _systemVariables.DC_SAS_RB_Alrm == false)
                {
                    // 大车右后方有障碍
                    DataManager.Instance.InsertHistoryWarningMc("大车右后方有障碍", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_SAS_RB_Alrm), "大车右后方有障碍",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DC_SAS_RB_Alrm == false && _systemVariables.DC_SAS_RB_Alrm == true)
                {
                    // 大车右后方有障碍解除
                    DataManager.Instance.InsertHistoryWarningMc("大车右后方有障碍解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_SAS_RB_Alrm));
                }

                if (newSystemVariables.DC_SAS_LF_Alrm && _systemVariables.DC_SAS_LF_Alrm == false)
                {
                    // 大车左前方有障碍
                    DataManager.Instance.InsertHistoryWarningMc("大车左前方有障碍", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_SAS_LF_Alrm), "大车左前方有障碍",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DC_SAS_LF_Alrm == false && _systemVariables.DC_SAS_LF_Alrm == true)
                {
                    // 大车左前方有障碍解除
                    DataManager.Instance.InsertHistoryWarningMc("大车左前方有障碍解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_SAS_LF_Alrm));
                }

                if (newSystemVariables.DC_SAS_LB_Alrm && _systemVariables.DC_SAS_LB_Alrm == false)
                {
                    // 大车左后方有障碍
                    DataManager.Instance.InsertHistoryWarningMc("大车左后方有障碍", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_SAS_LB_Alrm), "大车左后方有障碍",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DC_SAS_LB_Alrm == false && _systemVariables.DC_SAS_LB_Alrm == true)
                {
                    // 大车左后方有障碍解除
                    DataManager.Instance.InsertHistoryWarningMc("大车左后方有障碍解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_SAS_LB_Alrm));
                }

                if (newSystemVariables.FWD_Limit_Waring && _systemVariables.FWD_Limit_Waring == false)
                {
                    // 大车前进限位两米预警
                    DataManager.Instance.InsertHistoryWarningMc("大车前进限位两米预警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.FWD_Limit_Waring), "大车前进限位两米预警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.FWD_Limit_Waring == false && _systemVariables.FWD_Limit_Waring == true)
                {
                    // 大车前进限位两米预警解除
                    DataManager.Instance.InsertHistoryWarningMc("大车前进限位两米预警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.FWD_Limit_Waring));
                }

                if (newSystemVariables.REV_Limit_Waring && _systemVariables.REV_Limit_Waring == false)
                {
                    // 大车后退限位两米预警
                    DataManager.Instance.InsertHistoryWarningMc("大车后退限位两米预警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.REV_Limit_Waring), "大车后退限位两米预警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.REV_Limit_Waring == false && _systemVariables.REV_Limit_Waring == true)
                {
                    // 大车后退限位两米预警解除
                    DataManager.Instance.InsertHistoryWarningMc("大车后退限位两米预警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.REV_Limit_Waring));
                }

                if (newSystemVariables.Slew_R_Limit_Waring && _systemVariables.Slew_R_Limit_Waring == false)
                {
                    // 回转右转限位两度预警
                    DataManager.Instance.InsertHistoryWarningMc("回转右转限位两度预警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_R_Limit_Waring), "回转右转限位两度预警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Slew_R_Limit_Waring == false &&
                         _systemVariables.Slew_R_Limit_Waring == true)
                {
                    // 回转右转限位两度预警解除
                    DataManager.Instance.InsertHistoryWarningMc("回转右转限位两度预警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_R_Limit_Waring));
                }

                if (newSystemVariables.Slew_L_Limit_Waring && _systemVariables.Slew_L_Limit_Waring == false)
                {
                    // 回转左转限位两度预警
                    DataManager.Instance.InsertHistoryWarningMc("回转左转限位两度预警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_L_Limit_Waring), "回转左转限位两度预警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Slew_L_Limit_Waring == false &&
                         _systemVariables.Slew_L_Limit_Waring == true)
                {
                    // 回转左转限位两度预警解除
                    DataManager.Instance.InsertHistoryWarningMc("回转左转限位两度预警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_L_Limit_Waring));
                }

                if (newSystemVariables.Luff_U_Limit_Waring && _systemVariables.Luff_U_Limit_Waring == false)
                {
                    // 俯仰上仰限位两度预警
                    DataManager.Instance.InsertHistoryWarningMc("俯仰上仰限位两度预警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Luff_U_Limit_Waring), "俯仰上仰限位两度预警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Luff_U_Limit_Waring == false &&
                         _systemVariables.Luff_U_Limit_Waring == true)
                {
                    // 俯仰上仰限位两度预警解除
                    DataManager.Instance.InsertHistoryWarningMc("俯仰上仰限位两度预警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Luff_U_Limit_Waring));
                }

                if (newSystemVariables.Luff_D_Limit_Waring && _systemVariables.Luff_D_Limit_Waring == false)
                {
                    // 俯仰下俯限位两度预警
                    DataManager.Instance.InsertHistoryWarningMc("俯仰下俯限位两度预警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Luff_D_Limit_Waring), "俯仰下俯限位两度预警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Luff_D_Limit_Waring == false &&
                         _systemVariables.Luff_D_Limit_Waring == true)
                {
                    // 俯仰下俯限位两度预警解除
                    DataManager.Instance.InsertHistoryWarningMc("俯仰下俯限位两度预警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Luff_D_Limit_Waring));
                }

                if (newSystemVariables.DC_Encoder_ERR == true && _systemVariables.DC_Encoder_ERR == false)
                {
                    // 行走编码器异常
                    DataManager.Instance.InsertHistoryWarningMc("行走编码器异常", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_Encoder_ERR), "行走编码器异常",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DC_Encoder_ERR == false && _systemVariables.DC_Encoder_ERR == true)
                {
                    // 行走编码器异常解除
                    DataManager.Instance.InsertHistoryWarningMc("行走编码器异常解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_Encoder_ERR));
                }

                if (newSystemVariables.QJY_CH_FAULT == true && _systemVariables.QJY_CH_FAULT == false)
                {
                    // 倾角仪异常
                    DataManager.Instance.InsertHistoryWarningMc("倾角仪异常", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.QJY_CH_FAULT), "倾角仪异常",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.QJY_CH_FAULT == false && _systemVariables.QJY_CH_FAULT == true)
                {
                    //倾角仪异常解除
                    DataManager.Instance.InsertHistoryWarningMc("倾角仪异常解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.QJY_CH_FAULT));
                }

                if (newSystemVariables.XBTB_LWJ_CH_FAULT == true && _systemVariables.XBTB_LWJ_CH_FAULT == false)
                {
                    // 垂直料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("垂直料位计异常", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.XBTB_LWJ_CH_FAULT), "垂直料位计异常",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.XBTB_LWJ_CH_FAULT == false && _systemVariables.XBTB_LWJ_CH_FAULT == true)
                {
                    // 垂直料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("垂直料位计异常解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.XBTB_LWJ_CH_FAULT));
                }

                if (newSystemVariables.DCZQ_FZ_CH_FAULT == true && _systemVariables.DCZQ_FZ_CH_FAULT == false)
                {
                    // 大车左前料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("大车左前料位计异常", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DCZQ_FZ_CH_FAULT), "大车左前料位计异常",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DCZQ_FZ_CH_FAULT == false && _systemVariables.DCZQ_FZ_CH_FAULT == true)
                {
                    // 大车左前料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("大车左前料位计异常解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DCZQ_FZ_CH_FAULT));
                }

                if (newSystemVariables.DCZH_FZ_CH_FAULT == true && _systemVariables.DCZH_FZ_CH_FAULT == false)
                {
                    // 大车左后料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("大车左后料位计异常", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DCZH_FZ_CH_FAULT), "大车左后料位计异常",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DCZH_FZ_CH_FAULT == false && _systemVariables.DCZH_FZ_CH_FAULT == true)
                {
                    // 大车左后料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("大车左后料位计异常解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DCZH_FZ_CH_FAULT));
                }

                if (newSystemVariables.DCYQ_FZ_CH_FAULT == true && _systemVariables.DCYQ_FZ_CH_FAULT == false)
                {
                    // 大车右前料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("大车右前料位计异常", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DCYQ_FZ_CH_FAULT), "大车右前料位计异常",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DCYQ_FZ_CH_FAULT == false && _systemVariables.DCYQ_FZ_CH_FAULT == true)
                {
                    // 大车右前料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("大车右前料位计异常解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DCYQ_FZ_CH_FAULT));
                }

                if (newSystemVariables.DCYH_FZ_CH_FAULT == true && _systemVariables.DCYH_FZ_CH_FAULT == false)
                {
                    // 大车右后料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("大车右后料位计异常", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DCYH_FZ_CH_FAULT), "大车右后料位计异常",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DCYH_FZ_CH_FAULT == false && _systemVariables.DCYH_FZ_CH_FAULT == true)
                {
                    // 大车右后料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("大车右后料位计异常解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DCYH_FZ_CH_FAULT));
                }

                if (newSystemVariables.XBZQ_FZ_CH_FAULT == true && _systemVariables.XBZQ_FZ_CH_FAULT == false)
                {
                    // 悬臂左前料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左前料位计异常", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.XBZQ_FZ_CH_FAULT), "悬臂左前料位计异常",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.XBZQ_FZ_CH_FAULT == false && _systemVariables.XBZQ_FZ_CH_FAULT == true)
                {
                    // 悬臂左前料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左前料位计异常解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.XBZQ_FZ_CH_FAULT));
                }

                if (newSystemVariables.XBZZ_FZ_CH_FAULT == true && _systemVariables.XBZZ_FZ_CH_FAULT == false)
                {
                    // 悬臂左中料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左中料位计异常", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.XBZZ_FZ_CH_FAULT), "悬臂左中料位计异常",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.XBZZ_FZ_CH_FAULT == false && _systemVariables.XBZZ_FZ_CH_FAULT == true)
                {
                    // 悬臂左中料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左中料位计异常解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.XBZZ_FZ_CH_FAULT));
                }

                if (newSystemVariables.XBZH_FZ_CH_FAULT == true && _systemVariables.XBZH_FZ_CH_FAULT == false)
                {
                    // 悬臂左后料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左后料位计异常", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.XBZH_FZ_CH_FAULT), "悬臂左后料位计异常",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.XBZH_FZ_CH_FAULT == false && _systemVariables.XBZH_FZ_CH_FAULT == true)
                {
                    // 悬臂左后料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左后料位计异常解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.XBZH_FZ_CH_FAULT));
                }

                if (newSystemVariables.XBYQ_FZ_CH_FAULT == true && _systemVariables.XBYQ_FZ_CH_FAULT == false)
                {
                    // 悬臂右前料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右前料位计异常", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.XBYQ_FZ_CH_FAULT), "悬臂右前料位计异常",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.XBYQ_FZ_CH_FAULT == false && _systemVariables.XBYQ_FZ_CH_FAULT == true)
                {
                    // 悬臂右前料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右前料位计异常解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.XBYQ_FZ_CH_FAULT));
                }

                if (newSystemVariables.XBYZ_FZ_CH_FAULT == true && _systemVariables.XBYZ_FZ_CH_FAULT == false)
                {
                    // 悬臂右中料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右中料位计异常", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.XBYZ_FZ_CH_FAULT), "悬臂右中料位计异常",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.XBYZ_FZ_CH_FAULT == false && _systemVariables.XBYZ_FZ_CH_FAULT == true)
                {
                    // 悬臂右中料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右中料位计异常解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.XBYZ_FZ_CH_FAULT));
                }

                if (newSystemVariables.XBYH_FZ_CH_FAULT == true && _systemVariables.XBYH_FZ_CH_FAULT == false)
                {
                    // 悬臂右后料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右后料位计异常", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.XBYH_FZ_CH_FAULT), "悬臂右后料位计异常",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.XBYH_FZ_CH_FAULT == false && _systemVariables.XBYH_FZ_CH_FAULT == true)
                {
                    // 悬臂右后料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右后料位计异常解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.XBYH_FZ_CH_FAULT));
                }

                if (newSystemVariables.Slew_Encoder_ERR == true && _systemVariables.Slew_Encoder_ERR == false)
                {
                    // 回转编码器异常
                    DataManager.Instance.InsertHistoryWarningMc("回转编码器异常", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_Encoder_ERR), "回转编码器异常",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Slew_Encoder_ERR == false && _systemVariables.Slew_Encoder_ERR == true)
                {
                    // 回转编码器异常解除
                    DataManager.Instance.InsertHistoryWarningMc("回转编码器异常解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_Encoder_ERR));
                }

                if (newSystemVariables.LeftAnchorNotLifted == true && _systemVariables.LeftAnchorNotLifted == false)
                {
                    // 左侧锚锭没有抬起
                    DataManager.Instance.InsertHistoryWarningMc("左侧锚锭没有抬起", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LeftAnchorNotLifted), "左侧锚锭没有抬起",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LeftAnchorNotLifted == false &&
                         _systemVariables.LeftAnchorNotLifted == true)
                {
                    // 左侧锚锭没有抬起解除
                    DataManager.Instance.InsertHistoryWarningMc("左侧锚锭没有抬起解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LeftAnchorNotLifted));
                }

                if (newSystemVariables.RightAnchorNotLifted == true && _systemVariables.RightAnchorNotLifted == false)
                {
                    // 右侧锚锭没有抬起
                    DataManager.Instance.InsertHistoryWarningMc("右侧锚锭没有抬起", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RightAnchorNotLifted), "右侧锚锭没有抬起",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RightAnchorNotLifted == false &&
                         _systemVariables.RightAnchorNotLifted == true)
                {
                    // 右侧锚锭没有抬起解除
                    DataManager.Instance.InsertHistoryWarningMc("右侧锚锭没有抬起解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RightAnchorNotLifted));
                }

                if (newSystemVariables.ClampNotRelaxed == true && _systemVariables.ClampNotRelaxed == false)
                {
                    // 夹轨器没有放松
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器没有放松", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ClampNotRelaxed), "夹轨器没有放松",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.ClampNotRelaxed == false && _systemVariables.ClampNotRelaxed == true)
                {
                    // 夹轨器没有放松解除
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器没有放松解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.ClampNotRelaxed));
                }

                if (newSystemVariables.LargeCarBrakeNotOpen == true && _systemVariables.LargeCarBrakeNotOpen == false)
                {
                    // 大车制动器没有打开
                    DataManager.Instance.InsertHistoryWarningMc("大车制动器没有打开", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarBrakeNotOpen), "大车制动器没有打开",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarBrakeNotOpen == false &&
                         _systemVariables.LargeCarBrakeNotOpen == true)
                {
                    // 大车制动器没有打开解除
                    DataManager.Instance.InsertHistoryWarningMc("大车制动器没有打开解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarBrakeNotOpen));
                }

                if (newSystemVariables.LargeCarFrequencyConverterNotPowered == true &&
                    _systemVariables.LargeCarFrequencyConverterNotPowered == false)
                {
                    // 大车变频器没有投入
                    DataManager.Instance.InsertHistoryWarningMc("大车变频器没有投入", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarFrequencyConverterNotPowered),
                        "大车变频器没有投入",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarFrequencyConverterNotPowered == false &&
                         _systemVariables.LargeCarFrequencyConverterNotPowered == true)
                {
                    // 大车变频器没有投入解除
                    DataManager.Instance.InsertHistoryWarningMc("大车变频器没有投入解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarFrequencyConverterNotPowered));
                }

                if (newSystemVariables.LargeCarBrakeContactAuxiliaryFault == true &&
                    _systemVariables.LargeCarBrakeContactAuxiliaryFault == false)
                {
                    // 大车制动器接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("大车制动器接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarBrakeContactAuxiliaryFault),
                        "大车制动器接触器辅助触点故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarBrakeContactAuxiliaryFault == false &&
                         _systemVariables.LargeCarBrakeContactAuxiliaryFault == true)
                {
                    // 大车制动器接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车制动器接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarBrakeContactAuxiliaryFault));
                }

                if (newSystemVariables.LargeCarFrequencyConverterContactAuxiliaryFault == true &&
                    _systemVariables.LargeCarFrequencyConverterContactAuxiliaryFault == false)
                {
                    // 大车变频器接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("大车变频器接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.LargeCarFrequencyConverterContactAuxiliaryFault),
                        "大车变频器接触器辅助触点故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarFrequencyConverterContactAuxiliaryFault == false &&
                         _systemVariables.LargeCarFrequencyConverterContactAuxiliaryFault == true)
                {
                    // 大车变频器接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车变频器接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarFrequencyConverterContactAuxiliaryFault));
                }

                if (newSystemVariables.RotaryFrequencyConverterNotPowered == true &&
                    _systemVariables.RotaryFrequencyConverterNotPowered == false)
                {
                    // 回转变频器没有投入
                    DataManager.Instance.InsertHistoryWarningMc("回转变频器没有投入", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryFrequencyConverterNotPowered),
                        "回转变频器没有投入",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryFrequencyConverterNotPowered == false &&
                         _systemVariables.RotaryFrequencyConverterNotPowered == true)
                {
                    // 回转变频器没有投入解除
                    DataManager.Instance.InsertHistoryWarningMc("回转变频器没有投入解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryFrequencyConverterNotPowered));
                }

                if (newSystemVariables.RotaryFrequencyConverterContactAuxiliaryFault == true &&
                    _systemVariables.RotaryFrequencyConverterContactAuxiliaryFault == false)
                {
                    // 回转变频器接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("回转变频器接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryFrequencyConverterContactAuxiliaryFault),
                        "回转变频器接触器辅助触点故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryFrequencyConverterContactAuxiliaryFault == false &&
                         _systemVariables.RotaryFrequencyConverterContactAuxiliaryFault == true)
                {
                    // 回转变频器接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转变频器接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryFrequencyConverterContactAuxiliaryFault));
                }

                if (newSystemVariables.RotaryBrakeContactAuxiliaryFault == true &&
                    _systemVariables.RotaryBrakeContactAuxiliaryFault == false)
                {
                    // 回转制动器接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("回转制动器接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryBrakeContactAuxiliaryFault),
                        "回转制动器接触器辅助触点故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryBrakeContactAuxiliaryFault == false &&
                         _systemVariables.RotaryBrakeContactAuxiliaryFault == true)
                {
                    // 回转制动器接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转制动器接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryBrakeContactAuxiliaryFault));
                }

                if (newSystemVariables.VariableAmplitudeOilPumpMotorNotRunning == true &&
                    _systemVariables.VariableAmplitudeOilPumpMotorNotRunning == false)
                {
                    // 变幅油泵电机没有运行
                    DataManager.Instance.InsertHistoryWarningMc("变幅油泵电机没有运行", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilPumpMotorNotRunning),
                        "变幅油泵电机没有运行",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeOilPumpMotorNotRunning == false &&
                         _systemVariables.VariableAmplitudeOilPumpMotorNotRunning == true)
                {
                    // 变幅油泵电机没有运行解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅油泵电机没有运行解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilPumpMotorNotRunning));
                }

                if (newSystemVariables.SuspensionBeltBrakeContactAuxiliaryFault == true &&
                    _systemVariables.SuspensionBeltBrakeContactAuxiliaryFault == false)
                {
                    // 悬臂胶带制动器接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带制动器接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltBrakeContactAuxiliaryFault),
                        "悬臂胶带制动器接触器辅助触点故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspensionBeltBrakeContactAuxiliaryFault == false &&
                         _systemVariables.SuspensionBeltBrakeContactAuxiliaryFault == true)
                {
                    // 悬臂胶带制动器接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带制动器接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltBrakeContactAuxiliaryFault));
                }

                if (newSystemVariables.SuspensionBeltLoadingContactAuxiliaryFault == true &&
                    _systemVariables.SuspensionBeltLoadingContactAuxiliaryFault == false)
                {
                    // 悬臂胶带堆料接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带堆料接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltLoadingContactAuxiliaryFault),
                        "悬臂胶带堆料接触器辅助触点故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspensionBeltLoadingContactAuxiliaryFault == false &&
                         _systemVariables.SuspensionBeltLoadingContactAuxiliaryFault == true)
                {
                    // 悬臂胶带堆料接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带堆料接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltLoadingContactAuxiliaryFault));
                }

                if (newSystemVariables.SuspensionBeltUnloadingContactAuxiliaryFault == true &&
                    _systemVariables.SuspensionBeltUnloadingContactAuxiliaryFault == false)
                {
                    // 悬臂胶带取料接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带取料接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltUnloadingContactAuxiliaryFault),
                        "悬臂胶带取料接触器辅助触点故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspensionBeltUnloadingContactAuxiliaryFault == false &&
                         _systemVariables.SuspensionBeltUnloadingContactAuxiliaryFault == true)
                {
                    // 悬臂胶带取料接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带取料接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltUnloadingContactAuxiliaryFault));
                }

                if (newSystemVariables.SuspensionBeltFirstLevelDeviation == true &&
                    _systemVariables.SuspensionBeltFirstLevelDeviation == false)
                {
                    // 悬臂胶带一级跑偏
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带一级跑偏", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltFirstLevelDeviation), "悬臂胶带一级跑偏",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspensionBeltFirstLevelDeviation == false &&
                         _systemVariables.SuspensionBeltFirstLevelDeviation == true)
                {
                    // 悬臂胶带一级跑偏解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带一级跑偏解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltFirstLevelDeviation));
                }

                if (newSystemVariables.BucketWheelLubricationPumpContactAuxiliaryFault == true &&
                    _systemVariables.BucketWheelLubricationPumpContactAuxiliaryFault == false)
                {
                    // 斗轮润滑油泵接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("斗轮润滑油泵接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.BucketWheelLubricationPumpContactAuxiliaryFault),
                        "斗轮润滑油泵接触器辅助触点故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelLubricationPumpContactAuxiliaryFault == false &&
                         _systemVariables.BucketWheelLubricationPumpContactAuxiliaryFault == true)
                {
                    // 斗轮润滑油泵接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮润滑油泵接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelLubricationPumpContactAuxiliaryFault));
                }

                if (newSystemVariables.WindproofSystemCableLimit1 == true &&
                    _systemVariables.WindproofSystemCableLimit1 == false)
                {
                    // 防风系缆限位 1
                    DataManager.Instance.InsertHistoryWarningMc("防风系缆限位 1", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.WindproofSystemCableLimit1), "防风系缆限位 1",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.WindproofSystemCableLimit1 == false &&
                         _systemVariables.WindproofSystemCableLimit1 == true)
                {
                    // 防风系缆限位 1 解除
                    DataManager.Instance.InsertHistoryWarningMc("防风系缆限位 1 解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.WindproofSystemCableLimit1));
                }

                if (newSystemVariables.VariableAmplitudeOilPumpMotorContactFault == true &&
                    _systemVariables.VariableAmplitudeOilPumpMotorContactFault == false)
                {
                    // 变幅油泵电机接触器故障
                    DataManager.Instance.InsertHistoryWarningMc("变幅油泵电机接触器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilPumpMotorContactFault),
                        "变幅油泵电机接触器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeOilPumpMotorContactFault == false &&
                         _systemVariables.VariableAmplitudeOilPumpMotorContactFault == true)
                {
                    // 变幅油泵电机接触器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅油泵电机接触器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilPumpMotorContactFault));
                }

                if (newSystemVariables.BucketWheelMotorContactAuxiliaryFault == true &&
                    _systemVariables.BucketWheelMotorContactAuxiliaryFault == false)
                {
                    // 斗轮电机接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电机接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelMotorContactAuxiliaryFault),
                        "斗轮电机接触器辅助触点故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelMotorContactAuxiliaryFault == false &&
                         _systemVariables.BucketWheelMotorContactAuxiliaryFault == true)
                {
                    // 斗轮电机接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电机接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelMotorContactAuxiliaryFault));
                }

                if (newSystemVariables.TailCarOilPumpMotorContactAuxiliaryFault == true &&
                    _systemVariables.TailCarOilPumpMotorContactAuxiliaryFault == false)
                {
                    // 尾车油泵电机接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("尾车油泵电机接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarOilPumpMotorContactAuxiliaryFault),
                        "尾车油泵电机接触器辅助触点故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.TailCarOilPumpMotorContactAuxiliaryFault == false &&
                         _systemVariables.TailCarOilPumpMotorContactAuxiliaryFault == true)
                {
                    // 尾车油泵电机接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车油泵电机接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarOilPumpMotorContactAuxiliaryFault));
                }

                if (newSystemVariables.VibrationMotorFault == true && _systemVariables.VibrationMotorFault == false)
                {
                    // 振打电机故障
                    DataManager.Instance.InsertHistoryWarningMc("振打电机故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VibrationMotorFault), "振打电机故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VibrationMotorFault == false &&
                         _systemVariables.VibrationMotorFault == true)
                {
                    // 振打电机故障解除
                    DataManager.Instance.InsertHistoryWarningMc("振打电机故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VibrationMotorFault));
                }

                if (newSystemVariables.WindproofSystemCableNotOpen == true &&
                    _systemVariables.WindproofSystemCableNotOpen == false)
                {
                    // 防风系缆没有打开
                    DataManager.Instance.InsertHistoryWarningMc("防风系缆没有打开", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.WindproofSystemCableNotOpen), "防风系缆没有打开",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.WindproofSystemCableNotOpen == false &&
                         _systemVariables.WindproofSystemCableNotOpen == true)
                {
                    // 防风系缆没有打开解除
                    DataManager.Instance.InsertHistoryWarningMc("防风系缆没有打开解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.WindproofSystemCableNotOpen));
                }

                if (newSystemVariables.LargeCarLimitAction == true && _systemVariables.LargeCarLimitAction == false)
                {
                    // 大车限位动作
                    DataManager.Instance.InsertHistoryWarningMc("大车限位动作", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarLimitAction), "大车限位动作",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarLimitAction == false &&
                         _systemVariables.LargeCarLimitAction == true)
                {
                    // 大车限位动作解除
                    DataManager.Instance.InsertHistoryWarningMc("大车限位动作解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarLimitAction));
                }

                if (newSystemVariables.RotaryLimitAction == true && _systemVariables.RotaryLimitAction == false)
                {
                    // 回转限位动作
                    DataManager.Instance.InsertHistoryWarningMc("回转限位动作", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryLimitAction), "回转限位动作",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryLimitAction == false && _systemVariables.RotaryLimitAction == true)
                {
                    // 回转限位动作解除
                    DataManager.Instance.InsertHistoryWarningMc("回转限位动作解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryLimitAction));
                }

                if (newSystemVariables.VariableAmplitudeLimitAction == true &&
                    _systemVariables.VariableAmplitudeLimitAction == false)
                {
                    // 变幅限位动作
                    DataManager.Instance.InsertHistoryWarningMc("变幅限位动作", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLimitAction), "变幅限位动作",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeLimitAction == false &&
                         _systemVariables.VariableAmplitudeLimitAction == true)
                {
                    // 变幅限位动作解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅限位动作解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLimitAction));
                }

                if (newSystemVariables.ForbiddenZoneLimitAction == true &&
                    _systemVariables.ForbiddenZoneLimitAction == false)
                {
                    // 禁区限位动作
                    DataManager.Instance.InsertHistoryWarningMc("禁区限位动作", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ForbiddenZoneLimitAction), "禁区限位动作",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.ForbiddenZoneLimitAction == false &&
                         _systemVariables.ForbiddenZoneLimitAction == true)
                {
                    // 禁区限位动作解除
                    DataManager.Instance.InsertHistoryWarningMc("禁区限位动作解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.ForbiddenZoneLimitAction));
                }

                if (newSystemVariables.RotaryCrashSwitchAction == true &&
                    _systemVariables.RotaryCrashSwitchAction == false)
                {
                    // 回转防撞开关动作
                    DataManager.Instance.InsertHistoryWarningMc("回转防撞开关动作", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryCrashSwitchAction), "回转防撞开关动作",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryCrashSwitchAction == false &&
                         _systemVariables.RotaryCrashSwitchAction == true)
                {
                    // 回转防撞开关动作解除
                    DataManager.Instance.InsertHistoryWarningMc("回转防撞开关动作解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryCrashSwitchAction));
                }

                if (newSystemVariables.LargeCarCentralizedLubricationLowOilLevelAlarm == true &&
                    _systemVariables.LargeCarCentralizedLubricationLowOilLevelAlarm == false)
                {
                    // 大车集中润滑低油位报警
                    DataManager.Instance.InsertHistoryWarningMc("大车集中润滑低油位报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarCentralizedLubricationLowOilLevelAlarm),
                        "大车集中润滑低油位报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarCentralizedLubricationLowOilLevelAlarm == false &&
                         _systemVariables.LargeCarCentralizedLubricationLowOilLevelAlarm == true)
                {
                    // 大车集中润滑低油位报警解除
                    DataManager.Instance.InsertHistoryWarningMc("大车集中润滑低油位报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarCentralizedLubricationLowOilLevelAlarm));
                }

                if (newSystemVariables.LargeCarCentralizedLubricationOilBlockageAlarm == true &&
                    _systemVariables.LargeCarCentralizedLubricationOilBlockageAlarm == false)
                {
                    // 大车集中润滑堵油报警
                    DataManager.Instance.InsertHistoryWarningMc("大车集中润滑堵油报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarCentralizedLubricationOilBlockageAlarm),
                        "大车集中润滑堵油报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarCentralizedLubricationOilBlockageAlarm == false &&
                         _systemVariables.LargeCarCentralizedLubricationOilBlockageAlarm == true)
                {
                    // 大车集中润滑堵油报警解除
                    DataManager.Instance.InsertHistoryWarningMc("大车集中润滑堵油报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarCentralizedLubricationOilBlockageAlarm));
                }

                if (newSystemVariables.RotaryCentralizedLubricationLowOilLevelAlarm == true &&
                    _systemVariables.RotaryCentralizedLubricationLowOilLevelAlarm == false)
                {
                    // 回转集中润滑低油位报警
                    DataManager.Instance.InsertHistoryWarningMc("回转集中润滑低油位报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationLowOilLevelAlarm),
                        "回转集中润滑低油位报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryCentralizedLubricationLowOilLevelAlarm == false &&
                         _systemVariables.RotaryCentralizedLubricationLowOilLevelAlarm == true)
                {
                    // 回转集中润滑低油位报警解除
                    DataManager.Instance.InsertHistoryWarningMc("回转集中润滑低油位报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationLowOilLevelAlarm));
                }

                if (newSystemVariables.RotaryCentralizedLubricationOilBlockageAlarm == true &&
                    _systemVariables.RotaryCentralizedLubricationOilBlockageAlarm == false)
                {
                    // 回转集中润滑堵油报警
                    DataManager.Instance.InsertHistoryWarningMc("回转集中润滑堵油报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationOilBlockageAlarm),
                        "回转集中润滑堵油报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryCentralizedLubricationOilBlockageAlarm == false &&
                         _systemVariables.RotaryCentralizedLubricationOilBlockageAlarm == true)
                {
                    // 回转集中润滑堵油报警解除
                    DataManager.Instance.InsertHistoryWarningMc("回转集中润滑堵油报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationOilBlockageAlarm));
                }

                if (newSystemVariables.StrongWindPreAlarm == true && _systemVariables.StrongWindPreAlarm == false)
                {
                    // 大风预报警
                    DataManager.Instance.InsertHistoryWarningMc("大风预报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.StrongWindPreAlarm), "大风预报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.StrongWindPreAlarm == false && _systemVariables.StrongWindPreAlarm == true)
                {
                    // 大风预报警解除
                    DataManager.Instance.InsertHistoryWarningMc("大风预报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.StrongWindPreAlarm));
                }

                if (newSystemVariables.BucketWheelCentralizedLubricationLowOilLevelAlarm == true &&
                    _systemVariables.BucketWheelCentralizedLubricationLowOilLevelAlarm == false)
                {
                    // 斗轮集中润滑低油位报警
                    DataManager.Instance.InsertHistoryWarningMc("斗轮集中润滑低油位报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.BucketWheelCentralizedLubricationLowOilLevelAlarm),
                        "斗轮集中润滑低油位报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelCentralizedLubricationLowOilLevelAlarm == false &&
                         _systemVariables.BucketWheelCentralizedLubricationLowOilLevelAlarm == true)
                {
                    // 斗轮集中润滑低油位报警解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮集中润滑低油位报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelCentralizedLubricationLowOilLevelAlarm));
                }

                if (newSystemVariables.BucketWheelCentralizedLubricationOilBlockageAlarm == true &&
                    _systemVariables.BucketWheelCentralizedLubricationOilBlockageAlarm == false)
                {
                    // 斗轮集中润滑堵油报警
                    DataManager.Instance.InsertHistoryWarningMc("斗轮集中润滑堵油报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.BucketWheelCentralizedLubricationOilBlockageAlarm),
                        "斗轮集中润滑堵油报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelCentralizedLubricationOilBlockageAlarm == false &&
                         _systemVariables.BucketWheelCentralizedLubricationOilBlockageAlarm == true)
                {
                    // 斗轮集中润滑堵油报警解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮集中润滑堵油报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelCentralizedLubricationOilBlockageAlarm));
                }

                if (newSystemVariables.ElectricRoomEmergencyStopButtonAction == true &&
                    _systemVariables.ElectricRoomEmergencyStopButtonAction == false)
                {
                    // 电气室急停按钮动作
                    DataManager.Instance.InsertHistoryWarningMc("电气室急停按钮动作", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ElectricRoomEmergencyStopButtonAction),
                        "电气室急停按钮动作",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.ElectricRoomEmergencyStopButtonAction == false &&
                         _systemVariables.ElectricRoomEmergencyStopButtonAction == true)
                {
                    // 电气室急停按钮动作解除
                    DataManager.Instance.InsertHistoryWarningMc("电气室急停按钮动作解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.ElectricRoomEmergencyStopButtonAction));
                }

                if (newSystemVariables.CabinEmergencyStopButtonAction == true &&
                    _systemVariables.CabinEmergencyStopButtonAction == false)
                {
                    // 司机室急停按钮动作
                    DataManager.Instance.InsertHistoryWarningMc("司机室急停按钮动作", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CabinEmergencyStopButtonAction), "司机室急停按钮动作",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.CabinEmergencyStopButtonAction == false &&
                         _systemVariables.CabinEmergencyStopButtonAction == true)
                {
                    // 司机室急停按钮动作解除
                    DataManager.Instance.InsertHistoryWarningMc("司机室急停按钮动作解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.CabinEmergencyStopButtonAction));
                }

                if (newSystemVariables.EmergencyStopRelayNot == true && _systemVariables.EmergencyStopRelayNot == false)
                {
                    // 急停继电器没有吸合
                    DataManager.Instance.InsertHistoryWarningMc("急停继电器没有吸合", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.EmergencyStopRelayNot), "急停继电器没有吸合",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.EmergencyStopRelayNot == false &&
                         _systemVariables.EmergencyStopRelayNot == true)
                {
                    // 急停继电器没有吸合解除
                    DataManager.Instance.InsertHistoryWarningMc("急停继电器没有吸合解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.EmergencyStopRelayNot));
                }

                if (newSystemVariables.TransformerOverheatAlarm == true &&
                    _systemVariables.TransformerOverheatAlarm == false)
                {
                    // 变压器超温报警
                    DataManager.Instance.InsertHistoryWarningMc("变压器超温报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TransformerOverheatAlarm), "变压器超温报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.TransformerOverheatAlarm == false &&
                         _systemVariables.TransformerOverheatAlarm == true)
                {
                    // 变压器超温报警解除
                    DataManager.Instance.InsertHistoryWarningMc("变压器超温报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.TransformerOverheatAlarm));
                }

                if (newSystemVariables.ElectricRoomPLCModulePowerFault == true &&
                    _systemVariables.ElectricRoomPLCModulePowerFault == false)
                {
                    // 电气室PLC模块电源故障
                    DataManager.Instance.InsertHistoryWarningMc("电气室PLC模块电源故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ElectricRoomPLCModulePowerFault),
                        "电气室PLC模块电源故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.ElectricRoomPLCModulePowerFault == false &&
                         _systemVariables.ElectricRoomPLCModulePowerFault == true)
                {
                    // 电气室 PLC 模块电源故障解除
                    DataManager.Instance.InsertHistoryWarningMc("电气室PLC模块电源故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.ElectricRoomPLCModulePowerFault));
                }

                if (newSystemVariables.CabinPLCModulePowerFault == true &&
                    _systemVariables.CabinPLCModulePowerFault == false)
                {
                    // 司机室 PLC 模块电源故障
                    DataManager.Instance.InsertHistoryWarningMc("司机室PLC模块电源故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CabinPLCModulePowerFault), "司机室PLC模块电源故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.CabinPLCModulePowerFault == false &&
                         _systemVariables.CabinPLCModulePowerFault == true)
                {
                    // 司机室 PLC 模块电源故障解除
                    DataManager.Instance.InsertHistoryWarningMc("司机室PLC模块电源故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.CabinPLCModulePowerFault));
                }

                if (newSystemVariables.ElectricRoomFireAlarm == true && _systemVariables.ElectricRoomFireAlarm == false)
                {
                    // 电气室火灾报警
                    DataManager.Instance.InsertHistoryWarningMc("电气室火灾报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ElectricRoomFireAlarm), "电气室火灾报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.ElectricRoomFireAlarm == false &&
                         _systemVariables.ElectricRoomFireAlarm == true)
                {
                    // 电气室火灾报警解除
                    DataManager.Instance.InsertHistoryWarningMc("电气室火灾报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.ElectricRoomFireAlarm));
                }

                if (newSystemVariables.CabinFireAlarm == true && _systemVariables.CabinFireAlarm == false)
                {
                    // 司机室火灾报警
                    DataManager.Instance.InsertHistoryWarningMc("司机室火灾报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CabinFireAlarm), "司机室火灾报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.CabinFireAlarm == false && _systemVariables.CabinFireAlarm == true)
                {
                    // 司机室火灾报警解除
                    DataManager.Instance.InsertHistoryWarningMc("司机室火灾报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.CabinFireAlarm));
                }

                if (newSystemVariables.SuspensionBeltEmergencyStop == true &&
                    _systemVariables.SuspensionBeltEmergencyStop == false)
                {
                    // 悬臂胶带急停拉线
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带急停拉线", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltEmergencyStop), "悬臂胶带急停拉线",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspensionBeltEmergencyStop == false &&
                         _systemVariables.SuspensionBeltEmergencyStop == true)
                {
                    // 悬臂胶带急停拉线解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带急停拉线解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltEmergencyStop));
                }

                if (newSystemVariables.TailCarBeltEmergencyStopSwitch == true &&
                    _systemVariables.TailCarBeltEmergencyStopSwitch == false)
                {
                    // 尾车胶带急停拉线
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带急停拉线", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarBeltEmergencyStopSwitch), "尾车胶带急停拉线",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.TailCarBeltEmergencyStopSwitch == false &&
                         _systemVariables.TailCarBeltEmergencyStopSwitch == true)
                {
                    // 尾车胶带急停拉线解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带急停拉线解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarBeltEmergencyStopSwitch));
                }

                if (newSystemVariables.LargeCarMainCircuitBreakerFault == true &&
                    _systemVariables.LargeCarMainCircuitBreakerFault == false)
                {
                    // 大车主断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("大车主断路器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarMainCircuitBreakerFault), "大车主断路器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarMainCircuitBreakerFault == false &&
                         _systemVariables.LargeCarMainCircuitBreakerFault == true)
                {
                    // 大车主断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车主断路器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarMainCircuitBreakerFault));
                }

                if (newSystemVariables.LargeCarMotorCircuitBreakerFault == true &&
                    _systemVariables.LargeCarMotorCircuitBreakerFault == false)
                {
                    // 大车电机断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("大车电机断路器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarMotorCircuitBreakerFault), "大车电机断路器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarMotorCircuitBreakerFault == false &&
                         _systemVariables.LargeCarMotorCircuitBreakerFault == true)
                {
                    // 大车电机断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车电机断路器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarMotorCircuitBreakerFault));
                }

                if (newSystemVariables.LargeCarBrakeCircuitBreakerFault == true &&
                    _systemVariables.LargeCarBrakeCircuitBreakerFault == false)
                {
                    // 大车制动器断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("大车制动器断路器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarBrakeCircuitBreakerFault), "大车制动器断路器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarBrakeCircuitBreakerFault == false &&
                         _systemVariables.LargeCarBrakeCircuitBreakerFault == true)
                {
                    // 大车制动器断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车制动器断路器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarBrakeCircuitBreakerFault));
                }

                if (newSystemVariables.CarFrequencyConverterFault == true &&
                    _systemVariables.CarFrequencyConverterFault == false)
                {
                    // 大车变频器故障
                    DataManager.Instance.InsertHistoryWarningMc("大车变频器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CarFrequencyConverterFault), "大车变频器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.CarFrequencyConverterFault == false &&
                         _systemVariables.CarFrequencyConverterFault == true)
                {
                    // 大车变频器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车变频器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.CarFrequencyConverterFault));
                }

                if (newSystemVariables.LargeCarBrakeResistorOverheatJump == true &&
                    _systemVariables.LargeCarBrakeResistorOverheatJump == false)
                {
                    // 大车制动电阻超温跳闸
                    DataManager.Instance.InsertHistoryWarningMc("大车制动电阻超温跳闸", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarBrakeResistorOverheatJump),
                        "大车制动电阻超温跳闸",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarBrakeResistorOverheatJump == false &&
                         _systemVariables.LargeCarBrakeResistorOverheatJump == true)
                {
                    // 大车制动电阻超温跳闸解除
                    DataManager.Instance.InsertHistoryWarningMc("大车制动电阻超温跳闸解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarBrakeResistorOverheatJump));
                }

                if (newSystemVariables.CableReelMainCircuitBreakerFault == true &&
                    _systemVariables.CableReelMainCircuitBreakerFault == false)
                {
                    // 电缆卷筒主断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒主断路器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CableReelMainCircuitBreakerFault), "电缆卷筒主断路器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.CableReelMainCircuitBreakerFault == false &&
                         _systemVariables.CableReelMainCircuitBreakerFault == true)
                {
                    // 电缆卷筒主断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒主断路器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.CableReelMainCircuitBreakerFault));
                }

                if (newSystemVariables.CableReelMotorOverloading == true &&
                    _systemVariables.CableReelMotorOverloading == false)
                {
                    // 电缆卷筒电机过载
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒电机过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CableReelMotorOverloading), "电缆卷筒电机过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.CableReelMotorOverloading == false &&
                         _systemVariables.CableReelMotorOverloading == true)
                {
                    // 电缆卷筒电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒电机过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.CableReelMotorOverloading));
                }

                if (newSystemVariables.PowerReelCableOverLooseAlarm == true &&
                    _systemVariables.PowerReelCableOverLooseAlarm == false)
                {
                    // 动力卷筒电缆过松报警
                    DataManager.Instance.InsertHistoryWarningMc("动力卷筒电缆过松报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.PowerReelCableOverLooseAlarm), "动力卷筒电缆过松报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.PowerReelCableOverLooseAlarm == false &&
                         _systemVariables.PowerReelCableOverLooseAlarm == true)
                {
                    // 动力卷筒电缆过松报警解除
                    DataManager.Instance.InsertHistoryWarningMc("动力卷筒电缆过松报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.PowerReelCableOverLooseAlarm));
                }
                
                if (newSystemVariables.PowerReelCableOverTightAlarm == true &&
                    _systemVariables.PowerReelCableOverTightAlarm == false)
                {
                    //动力卷筒电缆过紧报警报警
                    DataManager.Instance.InsertHistoryWarningMc("动力卷筒电缆过紧报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.PowerReelCableOverTightAlarm), "动力卷筒电缆过紧报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.PowerReelCableOverTightAlarm == false &&
                         _systemVariables.PowerReelCableOverTightAlarm == true)
                {
                    // 动力卷筒电缆过紧报警解除
                    DataManager.Instance.InsertHistoryWarningMc("动力卷筒电缆过紧报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.PowerReelCableOverTightAlarm));
                }

                if (newSystemVariables.PowerReelFullDiskAlarm == true &&
                    _systemVariables.PowerReelFullDiskAlarm == false)
                {
                    // 动力电缆卷筒满盘报警
                    DataManager.Instance.InsertHistoryWarningMc("动力电缆卷筒满盘报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.PowerReelFullDiskAlarm), "动力电缆卷筒满盘报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.PowerReelFullDiskAlarm == false &&
                         _systemVariables.PowerReelFullDiskAlarm == true)
                {
                    // 动力电缆卷筒满盘报警解除
                    DataManager.Instance.InsertHistoryWarningMc("动力电缆卷筒满盘报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.PowerReelFullDiskAlarm));
                }

                if (newSystemVariables.PowerReelEmptyDiskAlarm == true &&
                    _systemVariables.PowerReelEmptyDiskAlarm == false)
                {
                    // 动力电缆卷筒空盘报警
                    DataManager.Instance.InsertHistoryWarningMc("动力电缆卷筒空盘报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.PowerReelEmptyDiskAlarm), "动力电缆卷筒空盘报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.PowerReelEmptyDiskAlarm == false &&
                         _systemVariables.PowerReelEmptyDiskAlarm == true)
                {
                    // 动力电缆卷筒空盘报警解除
                    DataManager.Instance.InsertHistoryWarningMc("动力电缆卷筒空盘报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.PowerReelEmptyDiskAlarm));
                }

                if (newSystemVariables.LargeCarOperationHandleFault == true &&
                    _systemVariables.LargeCarOperationHandleFault == false)
                {
                    // 大车操作手柄故障
                    DataManager.Instance.InsertHistoryWarningMc("大车操作手柄故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarOperationHandleFault), "大车操作手柄故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeCarOperationHandleFault == false &&
                         _systemVariables.LargeCarOperationHandleFault == true)
                {
                    // 大车操作手柄故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车操作手柄故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarOperationHandleFault));
                }

                if (newSystemVariables.RotaryMainCircuitBreakerFault == true &&
                    _systemVariables.RotaryMainCircuitBreakerFault == false)
                {
                    // 回转主断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("回转主断路器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryMainCircuitBreakerFault), "回转主断路器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryMainCircuitBreakerFault == false &&
                         _systemVariables.RotaryMainCircuitBreakerFault == true)
                {
                    // 回转主断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转主断路器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryMainCircuitBreakerFault));
                }

                if (newSystemVariables.RotaryBrakeOverloadAlarm == true &&
                    _systemVariables.RotaryBrakeOverloadAlarm == false)
                {
                    // 回转制动器过载报警
                    DataManager.Instance.InsertHistoryWarningMc("回转制动器过载报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryBrakeOverloadAlarm), "回转制动器过载报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryBrakeOverloadAlarm == false &&
                         _systemVariables.RotaryBrakeOverloadAlarm == true)
                {
                    // 回转制动器过载报警解除
                    DataManager.Instance.InsertHistoryWarningMc("回转制动器过载报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryBrakeOverloadAlarm));
                }

                if (newSystemVariables.RotaryFanOverloadAlarm == true &&
                    _systemVariables.RotaryFanOverloadAlarm == false)
                {
                    // 回转风机过载报警
                    DataManager.Instance.InsertHistoryWarningMc("回转风机过载报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryFanOverloadAlarm), "回转风机过载报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryFanOverloadAlarm == false &&
                         _systemVariables.RotaryFanOverloadAlarm == true)
                {
                    // 回转风机过载报警解除
                    DataManager.Instance.InsertHistoryWarningMc("回转风机过载报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryFanOverloadAlarm));
                }

                if (newSystemVariables.RotaryFrequencyConverterFaulting == true &&
                    _systemVariables.RotaryFrequencyConverterFaulting == false)
                {
                    // 回转变频器故障
                    DataManager.Instance.InsertHistoryWarningMc("回转变频器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryFrequencyConverterFaulting), "回转变频器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryFrequencyConverterFaulting == false &&
                         _systemVariables.RotaryFrequencyConverterFaulting == true)
                {
                    // 回转变频器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转变频器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryFrequencyConverterFaulting));
                }

                if (newSystemVariables.RotaryBrakeResistorOverheatSwitching == true &&
                    _systemVariables.RotaryBrakeResistorOverheatSwitching == false)
                {
                    // 回转制动电阻超温开关
                    DataManager.Instance.InsertHistoryWarningMc("回转制动电阻超温开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryBrakeResistorOverheatSwitching),
                        "回转制动电阻超温开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryBrakeResistorOverheatSwitching == false &&
                         _systemVariables.RotaryBrakeResistorOverheatSwitching == true)
                {
                    // 回转制动电阻超温开关解除
                    DataManager.Instance.InsertHistoryWarningMc("回转制动电阻超温开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryBrakeResistorOverheatSwitching));
                }

                if (newSystemVariables.RotaryOverTorqueSwitch == true &&
                    _systemVariables.RotaryOverTorqueSwitch == false)
                {
                    // 回转过力矩开关
                    DataManager.Instance.InsertHistoryWarningMc("回转过力矩开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryOverTorqueSwitch), "回转过力矩开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryOverTorqueSwitch == false &&
                         _systemVariables.RotaryOverTorqueSwitch == true)
                {
                    // 回转过力矩开关解除
                    DataManager.Instance.InsertHistoryWarningMc("回转过力矩开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryOverTorqueSwitch));
                }

                if (newSystemVariables.ReversalHandleFault == true && _systemVariables.ReversalHandleFault == false)
                {
                    // 回转操作手柄故障
                    DataManager.Instance.InsertHistoryWarningMc("回转操作手柄故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ReversalHandleFault), "回转操作手柄故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.ReversalHandleFault == false &&
                         _systemVariables.ReversalHandleFault == true)
                {
                    // 回转操作手柄故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转操作手柄故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.ReversalHandleFault));
                }

                if (newSystemVariables.LinkedBucketWheelNotRunning == true &&
                    _systemVariables.LinkedBucketWheelNotRunning == false)
                {
                    // 联动斗轮未运行禁止回转
                    DataManager.Instance.InsertHistoryWarningMc("联动斗轮未运行禁止回转", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LinkedBucketWheelNotRunning), "联动斗轮未运行禁止回转",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LinkedBucketWheelNotRunning == false &&
                         _systemVariables.LinkedBucketWheelNotRunning == true)
                {
                    // 联动斗轮未运行禁止回转解除
                    DataManager.Instance.InsertHistoryWarningMc("联动斗轮未运行禁止回转解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LinkedBucketWheelNotRunning));
                }

                if (newSystemVariables.VariableFrequencyMainCircuitBreakerFault == true &&
                    _systemVariables.VariableFrequencyMainCircuitBreakerFault == false)
                {
                    // 变幅主断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("变幅主断路器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableFrequencyMainCircuitBreakerFault),
                        "变幅主断路器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableFrequencyMainCircuitBreakerFault == false &&
                         _systemVariables.VariableFrequencyMainCircuitBreakerFault == true)
                {
                    // 变幅主断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅主断路器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableFrequencyMainCircuitBreakerFault));
                }

                if (newSystemVariables.VariableFrequencyMotorOverload == true &&
                    _systemVariables.VariableFrequencyMotorOverload == false)
                {
                    // 变幅主电机过载
                    DataManager.Instance.InsertHistoryWarningMc("变幅主电机过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableFrequencyMotorOverload), "变幅主电机过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableFrequencyMotorOverload == false &&
                         _systemVariables.VariableFrequencyMotorOverload == true)
                {
                    // 变幅主电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅主电机过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableFrequencyMotorOverload));
                }

                if (newSystemVariables.VariableFrequencyPumpClogged == true &&
                    _systemVariables.VariableFrequencyPumpClogged == false)
                {
                    // 变幅油泵堵油
                    DataManager.Instance.InsertHistoryWarningMc("变幅油泵堵油", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableFrequencyPumpClogged), "变幅油泵堵油",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableFrequencyPumpClogged == false &&
                         _systemVariables.VariableFrequencyPumpClogged == true)
                {
                    // 变幅油泵堵油解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅油泵堵油解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableFrequencyPumpClogged));
                }

                if (newSystemVariables.VariableFrequencyPumpStationHighTemperatureAlarm == true &&
                    _systemVariables.VariableFrequencyPumpStationHighTemperatureAlarm == false)
                {
                    // 变幅泵站高温报警信号
                    DataManager.Instance.InsertHistoryWarningMc("变幅泵站高温报警信号", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.VariableFrequencyPumpStationHighTemperatureAlarm),
                        "变幅泵站高温报警信号",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableFrequencyPumpStationHighTemperatureAlarm == false &&
                         _systemVariables.VariableFrequencyPumpStationHighTemperatureAlarm == true)
                {
                    // 变幅泵站高温报警信号解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅泵站高温报警信号解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableFrequencyPumpStationHighTemperatureAlarm));
                }

                if (newSystemVariables.VariableFrequencyOilTankLowLevelAlarm == true &&
                    _systemVariables.VariableFrequencyOilTankLowLevelAlarm == false)
                {
                    // 变幅油箱油位超低报警信号
                    DataManager.Instance.InsertHistoryWarningMc("变幅油箱油位超低报警信号", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableFrequencyOilTankLowLevelAlarm),
                        "变幅油箱油位超低报警信号",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableFrequencyOilTankLowLevelAlarm == false &&
                         _systemVariables.VariableFrequencyOilTankLowLevelAlarm == true)
                {
                    // 变幅油箱油位超低报警信号解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅油箱油位超低报警信号解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableFrequencyOilTankLowLevelAlarm));
                }

                if (newSystemVariables.VariableFrequencyHandleFault == true &&
                    _systemVariables.VariableFrequencyHandleFault == false)
                {
                    // 变幅操作手柄故障
                    DataManager.Instance.InsertHistoryWarningMc("变幅操作手柄故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableFrequencyHandleFault), "变幅操作手柄故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VariableFrequencyHandleFault == false &&
                         _systemVariables.VariableFrequencyHandleFault == true)
                {
                    // 变幅操作手柄故障解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅操作手柄故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableFrequencyHandleFault));
                }

                if (newSystemVariables.SuspendedBeltCircuitBreakerFault == true &&
                    _systemVariables.SuspendedBeltCircuitBreakerFault == false)
                {
                    // 悬臂胶带断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带断路器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltCircuitBreakerFault), "悬臂胶带断路器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspendedBeltCircuitBreakerFault == false &&
                         _systemVariables.SuspendedBeltCircuitBreakerFault == true)
                {
                    // 悬臂胶带断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带断路器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltCircuitBreakerFault));
                }

                if (newSystemVariables.SuspendedBeltMotorOverload == true &&
                    _systemVariables.SuspendedBeltMotorOverload == false)
                {
                    // 悬臂胶带电机过载
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带电机过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltMotorOverload), "悬臂胶带电机过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspendedBeltMotorOverload == false &&
                         _systemVariables.SuspendedBeltMotorOverload == true)
                {
                    // 悬臂胶带电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带电机过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltMotorOverload));
                }

                if (newSystemVariables.SuspendedBeltSecondLevelDeviationSwitch == true &&
                    _systemVariables.SuspendedBeltSecondLevelDeviationSwitch == false)
                {
                    // 悬胶二级跑偏开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶二级跑偏开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltSecondLevelDeviationSwitch),
                        "悬胶二级跑偏开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspendedBeltSecondLevelDeviationSwitch == false &&
                         _systemVariables.SuspendedBeltSecondLevelDeviationSwitch == true)
                {
                    // 悬胶二级跑偏开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶二级跑偏开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltSecondLevelDeviationSwitch));
                }

                if (newSystemVariables.SuspendedBeltEmergencyStop == true &&
                    _systemVariables.SuspendedBeltEmergencyStop == false)
                {
                    // 悬臂胶带急停拉线
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带急停拉线", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltEmergencyStop), "悬臂胶带急停拉线",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspendedBeltEmergencyStop == false &&
                         _systemVariables.SuspendedBeltEmergencyStop == true)
                {
                    // 悬臂胶带急停拉线解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带急停拉线解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltEmergencyStop));
                }

                if (newSystemVariables.SuspendedBeltLongitudinalTearSwitch == true &&
                    _systemVariables.SuspendedBeltLongitudinalTearSwitch == false)
                {
                    // 悬胶纵向撕裂开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶纵向撕裂开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltLongitudinalTearSwitch),
                        "悬胶纵向撕裂开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspendedBeltLongitudinalTearSwitch == false &&
                         _systemVariables.SuspendedBeltLongitudinalTearSwitch == true)
                {
                    // 悬胶纵向撕裂开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶纵向撕裂开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltLongitudinalTearSwitch));
                }

                if (newSystemVariables.CentralHopperCloggedDetectionSwitch == true &&
                    _systemVariables.CentralHopperCloggedDetectionSwitch == false)
                {
                    // 中部料斗堵煤检测开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-中部料斗堵煤检测开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CentralHopperCloggedDetectionSwitch),
                        "悬胶/挡板-中部料斗堵煤检测开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.CentralHopperCloggedDetectionSwitch == false &&
                         _systemVariables.CentralHopperCloggedDetectionSwitch == true)
                {
                    // 中部料斗堵煤检测开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-中部料斗堵煤检测开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.CentralHopperCloggedDetectionSwitch));
                }

                if (newSystemVariables.StackingSwitchFault == true && _systemVariables.StackingSwitchFault == false)
                {
                    // 堆取料开关故障
                    DataManager.Instance.InsertHistoryWarningMc("堆取料开关故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.StackingSwitchFault), "堆取料开关故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.StackingSwitchFault == false &&
                         _systemVariables.StackingSwitchFault == true)
                {
                    // 堆取料开关故障解除
                    DataManager.Instance.InsertHistoryWarningMc("堆取料开关故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.StackingSwitchFault));
                }

                // if (newSystemVariables.CentralControlRoomNoStackingCommand == true &&
                //     _systemVariables.CentralControlRoomNoStackingCommand == false)
                // {
                //     // 中控室没有允许堆取料命令
                //     DataManager.Instance.InsertHistoryWarningMc("中控室没有允许堆取料命令", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.CentralControlRoomNoStackingCommand),
                //         "中控室没有允许堆取料命令",
                //         Machine.BucketWheelStackerReclaimer, false, "");
                // }
                // else if (newSystemVariables.CentralControlRoomNoStackingCommand == false &&
                //          _systemVariables.CentralControlRoomNoStackingCommand == true)
                // {
                //     // 中控室没有允许堆取料命令解除
                //     DataManager.Instance.InsertHistoryWarningMc("中控室没有允许堆取料命令解除", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     RemoveWarningDesDict(nameof(newSystemVariables.CentralControlRoomNoStackingCommand));
                // }

                if (newSystemVariables.BucketWheelMotorMainCircuitBreakerFault == true &&
                    _systemVariables.BucketWheelMotorMainCircuitBreakerFault == false)
                {
                    // 斗轮电机主断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电机主断路器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelMotorMainCircuitBreakerFault),
                        "斗轮电机主断路器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelMotorMainCircuitBreakerFault == false &&
                         _systemVariables.BucketWheelMotorMainCircuitBreakerFault == true)
                {
                    // 斗轮电机主断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电机主断路器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelMotorMainCircuitBreakerFault));
                }

                if (newSystemVariables.BucketWheelMotorOverloading == true &&
                    _systemVariables.BucketWheelMotorOverloading == false)
                {
                    // 斗轮电机过载
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电机过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelMotorOverloading), "斗轮电机过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelMotorOverloading == false &&
                         _systemVariables.BucketWheelMotorOverloading == true)
                {
                    // 斗轮电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电机过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelMotorOverloading));
                }

                if (newSystemVariables.BucketWheelOverTorqueSwitching == true &&
                    _systemVariables.BucketWheelOverTorqueSwitching == false)
                {
                    // 斗轮过力矩开关
                    DataManager.Instance.InsertHistoryWarningMc("斗轮过力矩开关", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelOverTorqueSwitching), "斗轮过力矩开关",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelOverTorqueSwitching == false &&
                         _systemVariables.BucketWheelOverTorqueSwitching == true)
                {
                    // 斗轮过力矩开关解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮过力矩开关解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelOverTorqueSwitching));
                }

                if (newSystemVariables.BucketWheelTemperatureUpperLimitAlarm == true &&
                    _systemVariables.BucketWheelTemperatureUpperLimitAlarm == false)
                {
                    // 斗轮测温上限报警
                    DataManager.Instance.InsertHistoryWarningMc("斗轮测温上限报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelTemperatureUpperLimitAlarm),
                        "斗轮测温上限报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelTemperatureUpperLimitAlarm == false &&
                         _systemVariables.BucketWheelTemperatureUpperLimitAlarm == true)
                {
                    // 斗轮测温上限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮测温上限报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelTemperatureUpperLimitAlarm));
                }

                if (newSystemVariables.ClampingDeviceMainCircuitBreakerFault == true &&
                    _systemVariables.ClampingDeviceMainCircuitBreakerFault == false)
                {
                    // 夹轨器主断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器主断路器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ClampingDeviceMainCircuitBreakerFault),
                        "夹轨器主断路器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.ClampingDeviceMainCircuitBreakerFault == false &&
                         _systemVariables.ClampingDeviceMainCircuitBreakerFault == true)
                {
                    // 夹轨器主断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器主断路器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.ClampingDeviceMainCircuitBreakerFault));
                }

                if (newSystemVariables.LeftClampingDeviceTimeout == true &&
                    _systemVariables.LeftClampingDeviceTimeout == false)
                {
                    // 左夹轨器运行超时
                    DataManager.Instance.InsertHistoryWarningMc("左夹轨器运行超时", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LeftClampingDeviceTimeout), "左夹轨器运行超时",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LeftClampingDeviceTimeout == false &&
                         _systemVariables.LeftClampingDeviceTimeout == true)
                {
                    // 左夹轨器运行超时解除
                    DataManager.Instance.InsertHistoryWarningMc("左夹轨器运行超时解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LeftClampingDeviceTimeout));
                }

                if (newSystemVariables.RightClampingDeviceTimeout == true &&
                    _systemVariables.RightClampingDeviceTimeout == false)
                {
                    // 右夹轨器运行超时
                    DataManager.Instance.InsertHistoryWarningMc("右夹轨器运行超时", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RightClampingDeviceTimeout), "右夹轨器运行超时",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RightClampingDeviceTimeout == false &&
                         _systemVariables.RightClampingDeviceTimeout == true)
                {
                    // 右夹轨器运行超时解除
                    DataManager.Instance.InsertHistoryWarningMc("右夹轨器运行超时解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RightClampingDeviceTimeout));
                }

                // if (newSystemVariables.StrongWindAlarm == true && _systemVariables.StrongWindAlarm == false)
                // {
                //     // 大风报警信号
                //     DataManager.Instance.InsertHistoryWarningMc("大风报警信号", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.StrongWindAlarm), "大风报警信号",
                //         Machine.BucketWheelStackerReclaimer, false, "");
                // }
                // else if (newSystemVariables.StrongWindAlarm == false && _systemVariables.StrongWindAlarm == true)
                // {
                //     // 大风报警信号解除
                //     DataManager.Instance.InsertHistoryWarningMc("大风报警信号解除", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     RemoveWarningDesDict(nameof(newSystemVariables.StrongWindAlarm));
                // }

                if (newSystemVariables.DryFogSystemWaterTankLowLevel == true &&
                    _systemVariables.DryFogSystemWaterTankLowLevel == false)
                {
                    // 干雾系统水箱液位低
                    DataManager.Instance.InsertHistoryWarningMc("干雾系统水箱液位低", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DryFogSystemWaterTankLowLevel), "干雾系统水箱液位低",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DryFogSystemWaterTankLowLevel == false &&
                         _systemVariables.DryFogSystemWaterTankLowLevel == true)
                {
                    // 干雾系统水箱液位低解除
                    DataManager.Instance.InsertHistoryWarningMc("干雾系统水箱液位低解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DryFogSystemWaterTankLowLevel));
                }

                if (newSystemVariables.DiversionPlateCircuitBreakerFault == true &&
                    _systemVariables.DiversionPlateCircuitBreakerFault == false)
                {
                    // 分流挡板断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("分流挡板断路器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DiversionPlateCircuitBreakerFault), "分流挡板断路器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DiversionPlateCircuitBreakerFault == false &&
                         _systemVariables.DiversionPlateCircuitBreakerFault == true)
                {
                    // 分流挡板断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("分流挡板断路器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DiversionPlateCircuitBreakerFault));
                }

                if (newSystemVariables.BucketWheelFeederCircuitBreakerFault == true &&
                    _systemVariables.BucketWheelFeederCircuitBreakerFault == false)
                {
                    // 斗轮导料槽断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽断路器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelFeederCircuitBreakerFault),
                        "斗轮导料槽断路器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelFeederCircuitBreakerFault == false &&
                         _systemVariables.BucketWheelFeederCircuitBreakerFault == true)
                {
                    // 斗轮导料槽断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽断路器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelFeederCircuitBreakerFault));
                }

                if (newSystemVariables.BucketWheelFeederMotorOverload == true &&
                    _systemVariables.BucketWheelFeederMotorOverload == false)
                {
                    // 斗轮导料槽电机过载
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽电机过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelFeederMotorOverload), "斗轮导料槽电机过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelFeederMotorOverload == false &&
                         _systemVariables.BucketWheelFeederMotorOverload == true)
                {
                    // 斗轮导料槽电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽电机过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelFeederMotorOverload));
                }

                if (newSystemVariables.BucketWheelFeederTimeout == true &&
                    _systemVariables.BucketWheelFeederTimeout == false)
                {
                    // 斗轮导料槽运行超时
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽运行超时", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelFeederTimeout), "斗轮导料槽运行超时",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelFeederTimeout == false &&
                         _systemVariables.BucketWheelFeederTimeout == true)
                {
                    // 斗轮导料槽运行超时解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽运行超时解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelFeederTimeout));
                }

                // if (newSystemVariables.CentralControlRoomNoStackingUnloadingCommand == true &&
                //     _systemVariables.CentralControlRoomNoStackingUnloadingCommand == false)
                // {
                //     // 中控室没有允许堆取料命令
                //     DataManager.Instance.InsertHistoryWarningMc("中控室没有允许堆取料命令", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.CentralControlRoomNoStackingUnloadingCommand),
                //         "中控室没有允许堆取料命令",
                //         Machine.BucketWheelStackerReclaimer, false, "");
                // }
                // else if (newSystemVariables.CentralControlRoomNoStackingUnloadingCommand == false &&
                //          _systemVariables.CentralControlRoomNoStackingUnloadingCommand == true)
                // {
                //     // 中控室没有允许堆取料命令解除
                //     DataManager.Instance.InsertHistoryWarningMc("中控室没有允许堆取料命令解除", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     RemoveWarningDesDict(nameof(newSystemVariables.CentralControlRoomNoStackingUnloadingCommand));
                // }

                if (newSystemVariables.TailCarBeltFirstLevelDeviation == true &&
                    _systemVariables.TailCarBeltFirstLevelDeviation == false)
                {
                    // 尾车胶带一级跑偏
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带一级跑偏", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarBeltFirstLevelDeviation), "尾车胶带一级跑偏",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.TailCarBeltFirstLevelDeviation == false &&
                         _systemVariables.TailCarBeltFirstLevelDeviation == true)
                {
                    // 尾车胶带一级跑偏解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带一级跑偏解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarBeltFirstLevelDeviation));
                }

                if (newSystemVariables.TailCarBeltSecondLevelDeviation == true &&
                    _systemVariables.TailCarBeltSecondLevelDeviation == false)
                {
                    // 尾车胶带二级跑偏
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带二级跑偏", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarBeltSecondLevelDeviation), "尾车胶带二级跑偏",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.TailCarBeltSecondLevelDeviation == false &&
                         _systemVariables.TailCarBeltSecondLevelDeviation == true)
                {
                    // 尾车胶带二级跑偏解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带二级跑偏解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarBeltSecondLevelDeviation));
                }

                if (newSystemVariables.VibrationMotorCircuitBreakerFault == true &&
                    _systemVariables.VibrationMotorCircuitBreakerFault == false)
                {
                    // 振打电机断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("振打电机断路器故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VibrationMotorCircuitBreakerFault), "振打电机断路器故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VibrationMotorCircuitBreakerFault == false &&
                         _systemVariables.VibrationMotorCircuitBreakerFault == true)
                {
                    // 振打电机断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("振打电机断路器故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VibrationMotorCircuitBreakerFault));
                }

                if (newSystemVariables.VibrationMotorOverloading == true &&
                    _systemVariables.VibrationMotorOverloading == false)
                {
                    // 振打电机过载
                    DataManager.Instance.InsertHistoryWarningMc("振打电机过载", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VibrationMotorOverloading), "振打电机过载",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.VibrationMotorOverloading == false &&
                         _systemVariables.VibrationMotorOverloading == true)
                {
                    // 振打电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("振打电机过载解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.VibrationMotorOverloading));
                }

                // if (newSystemVariables.BucketWheelMotorContactor == true &&
                //     _systemVariables.BucketWheelMotorContactor == false)
                // {
                //     // 斗轮电机接触器
                //     DataManager.Instance.InsertHistoryWarningMc("斗轮电机接触器", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelMotorContactor), "斗轮电机接触器",
                //         Machine.BucketWheelStackerReclaimer, false, "");
                // }
                // else if (newSystemVariables.BucketWheelMotorContactor == false &&
                //          _systemVariables.BucketWheelMotorContactor == true)
                // {
                //     // 斗轮电机接触器解除
                //     DataManager.Instance.InsertHistoryWarningMc("斗轮电机接触器解除", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelMotorContactor));
                // }

                if (newSystemVariables.PowerCableRollerNotRunning == true &&
                    _systemVariables.PowerCableRollerNotRunning == false)
                {
                    // 动力电缆卷筒没有运行
                    DataManager.Instance.InsertHistoryWarningMc("动力电缆卷筒没有运行", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.PowerCableRollerNotRunning), "动力电缆卷筒没有运行",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.PowerCableRollerNotRunning == false &&
                         _systemVariables.PowerCableRollerNotRunning == true)
                {
                    // 动力电缆卷筒没有运行解除
                    DataManager.Instance.InsertHistoryWarningMc("动力电缆卷筒没有运行解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.PowerCableRollerNotRunning));
                }

                // if (newSystemVariables.TailCarDrivenRollerBearingTemperatureUpperLimitAlarm == true &&
                //     _systemVariables.TailCarDrivenRollerBearingTemperatureUpperLimitAlarm == false)
                // {
                //     // 尾车从动滚筒轴承测温上限报警
                //     DataManager.Instance.InsertHistoryWarningMc("尾车从动滚筒轴承测温上限报警", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     AddOrUpdateWarningDesDict(
                //         nameof(newSystemVariables.TailCarDrivenRollerBearingTemperatureUpperLimitAlarm),
                //         "尾车从动滚筒轴承测温上限报警",
                //         Machine.BucketWheelStackerReclaimer, false, "");
                // }
                // else if (newSystemVariables.TailCarDrivenRollerBearingTemperatureUpperLimitAlarm == false &&
                //          _systemVariables.TailCarDrivenRollerBearingTemperatureUpperLimitAlarm == true)
                // {
                //     // 尾车从动滚筒轴承测温上限报警解除
                //     DataManager.Instance.InsertHistoryWarningMc("尾车从动滚筒轴承测温上限报警解除", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     RemoveWarningDesDict(
                //         nameof(newSystemVariables.TailCarDrivenRollerBearingTemperatureUpperLimitAlarm));
                // }

                // if (newSystemVariables.TailCarDrivenRollerBearingTemperatureLowerLimitAlarm == true &&
                //     _systemVariables.TailCarDrivenRollerBearingTemperatureLowerLimitAlarm == false)
                // {
                //     // 尾车从动滚筒轴承测温下限报警
                //     DataManager.Instance.InsertHistoryWarningMc("尾车从动滚筒轴承测温下限报警", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     AddOrUpdateWarningDesDict(
                //         nameof(newSystemVariables.TailCarDrivenRollerBearingTemperatureLowerLimitAlarm),
                //         "尾车从动滚筒轴承测温下限报警",
                //         Machine.BucketWheelStackerReclaimer, false, "");
                // }
                // else if (newSystemVariables.TailCarDrivenRollerBearingTemperatureLowerLimitAlarm == false &&
                //          _systemVariables.TailCarDrivenRollerBearingTemperatureLowerLimitAlarm == true)
                // {
                //     // 尾车从动滚筒轴承测温下限报警解除
                //     DataManager.Instance.InsertHistoryWarningMc("尾车从动滚筒轴承测温下限报警解除", GetUserName(),
                //         Machine.BucketWheelStackerReclaimer);
                //     RemoveWarningDesDict(
                //         nameof(newSystemVariables.TailCarDrivenRollerBearingTemperatureLowerLimitAlarm));
                // }

                if (newSystemVariables.LargeVehicleMotor1OvertemperatureAlarm == true &&
                    _systemVariables.LargeVehicleMotor1OvertemperatureAlarm == false)
                {
                    // 大车电机 1 超温报警
                    DataManager.Instance.InsertHistoryWarningMc("大车电机 1 超温报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeVehicleMotor1OvertemperatureAlarm),
                        "大车电机 1 超温报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.LargeVehicleMotor1OvertemperatureAlarm == false &&
                         _systemVariables.LargeVehicleMotor1OvertemperatureAlarm == true)
                {
                    // 大车电机 1 超温报警解除
                    DataManager.Instance.InsertHistoryWarningMc("大车电机 1 超温报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeVehicleMotor1OvertemperatureAlarm));
                }

                if (newSystemVariables.DriverRoomBalancePumpMotorNotRunning == true &&
                    _systemVariables.DriverRoomBalancePumpMotorNotRunning == false)
                {
                    // 司机室平衡油泵电机没有运行
                    DataManager.Instance.InsertHistoryWarningMc("司机室平衡油泵电机没有运行", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DriverRoomBalancePumpMotorNotRunning),
                        "司机室平衡油泵电机没有运行",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DriverRoomBalancePumpMotorNotRunning == false &&
                         _systemVariables.DriverRoomBalancePumpMotorNotRunning == true)
                {
                    // 司机室平衡油泵电机没有运行解除
                    DataManager.Instance.InsertHistoryWarningMc("司机室平衡油泵电机没有运行解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DriverRoomBalancePumpMotorNotRunning));
                }

                if (newSystemVariables.DriverRoomBalancePumpMotorAuxiliaryContactFault == true &&
                    _systemVariables.DriverRoomBalancePumpMotorAuxiliaryContactFault == false)
                {
                    // 司机室平衡油泵电机辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("司机室平衡油泵电机辅助触点故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.DriverRoomBalancePumpMotorAuxiliaryContactFault),
                        "司机室平衡油泵电机辅助触点故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.DriverRoomBalancePumpMotorAuxiliaryContactFault == false &&
                         _systemVariables.DriverRoomBalancePumpMotorAuxiliaryContactFault == true)
                {
                    // 司机室平衡油泵电机辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("司机室平衡油泵电机辅助触点故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.DriverRoomBalancePumpMotorAuxiliaryContactFault));
                }

                if (newSystemVariables.SR1_REMOTE_PLANT_COMM_FAULT_0 == true &&
                    _systemVariables.SR1_REMOTE_PLANT_COMM_FAULT_0 == false)
                {
                    // 远程任务规划子系统通讯异常
                    DataManager.Instance.InsertHistoryWarningMc("远程任务规划子系统通讯异常", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SR1_REMOTE_PLANT_COMM_FAULT_0),
                        "远程任务规划子系统通讯异常",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SR1_REMOTE_PLANT_COMM_FAULT_0 == false &&
                         _systemVariables.SR1_REMOTE_PLANT_COMM_FAULT_0 == true)
                {
                    // 远程任务规划子系统通讯异常解除
                    DataManager.Instance.InsertHistoryWarningMc("远程任务规划子系统通讯异常解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SR1_REMOTE_PLANT_COMM_FAULT_0));
                }

                if (newSystemVariables.SuspensionBeltFault == true &&
                    _systemVariables.SuspensionBeltFault == false)
                {
                    // 悬臂胶带故障
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltFault),
                        "悬臂胶带故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspensionBeltFault == false &&
                         _systemVariables.SuspensionBeltFault == true)
                {
                    //悬臂胶带故障解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltFault));
                }

                if (newSystemVariables.BUCKET_Current_Pause_SLEW == true &&
                    _systemVariables.BUCKET_Current_Pause_SLEW == false)
                {
                    // 斗轮电流过大暂停回转
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电流过大暂停回转", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BUCKET_Current_Pause_SLEW),
                        "斗轮电流过大暂停回转",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BUCKET_Current_Pause_SLEW == false &&
                         _systemVariables.BUCKET_Current_Pause_SLEW == true)
                {
                    //悬臂胶带故障解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电流过大暂停回转解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BUCKET_Current_Pause_SLEW));
                }

                if (newSystemVariables.Slew_Current_Pause_Slew == true &&
                    _systemVariables.Slew_Current_Pause_Slew == false)
                {
                    // 回转电流过大暂停回转
                    DataManager.Instance.InsertHistoryWarningMc("回转电流过大暂停回转", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_Current_Pause_Slew),
                        "回转电流过大暂停回转",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.Slew_Current_Pause_Slew == false &&
                         _systemVariables.Slew_Current_Pause_Slew == true)
                {
                    //悬臂胶带故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转电流过大暂停回转解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_Current_Pause_Slew));
                }
                
                if (newSystemVariables.RotaryLeftTurnForbiddenLimit == true &&
                    _systemVariables.RotaryLeftTurnForbiddenLimit == false)
                {
                    // 回转左转防撞限位
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转防撞限位", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnForbiddenLimit),
                        "回转-左转防撞限位",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.RotaryLeftTurnForbiddenLimit == false &&
                         _systemVariables.RotaryLeftTurnForbiddenLimit == true)
                {
                    //回转左转防撞限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转防撞限位解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnForbiddenLimit));
                }
                
                if (newSystemVariables.CableRollerContactorAuxiliaryContactFault == true &&
                    _systemVariables.CableRollerContactorAuxiliaryContactFault == false)
                {
                    // 电缆卷筒接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CableRollerContactorAuxiliaryContactFault),
                        "电缆卷筒接触器辅助触点故障",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.CableRollerContactorAuxiliaryContactFault == false &&
                         _systemVariables.CableRollerContactorAuxiliaryContactFault == true)
                {
                    //回转左转防撞限位解除
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.CableRollerContactorAuxiliaryContactFault));
                }
                
                if (newSystemVariables.SR1_3D_Unusable == true &&
                    _systemVariables.SR1_3D_Unusable == false)
                {
                    // 三维正在盘煤，自动堆料功能不可用
                    DataManager.Instance.InsertHistoryWarningMc("三维正在盘煤，自动堆料功能不可用", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SR1_3D_Unusable),
                        "三维正在盘煤，自动堆料功能不可用",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SR1_3D_Unusable == false &&
                         _systemVariables.SR1_3D_Unusable == true)
                {
                    //三维正在盘煤，自动堆料功能不可用解除
                    DataManager.Instance.InsertHistoryWarningMc("三维正在盘煤，自动堆料功能不可用解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SR1_3D_Unusable));
                }
                //1.22
                if (newSystemVariables.SuspendedBeltTemperatureUpperLimitAlarm == true &&
                    _systemVariables.SuspendedBeltTemperatureUpperLimitAlarm == false)
                {
                    // 悬胶测温上限报警
                    DataManager.Instance.InsertHistoryWarningMc("悬胶测温上限报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltTemperatureUpperLimitAlarm),
                        "悬胶测温上限报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspendedBeltTemperatureUpperLimitAlarm == false &&
                         _systemVariables.SuspendedBeltTemperatureUpperLimitAlarm == true)
                {
                    //悬胶测温上限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶测温上限报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltTemperatureUpperLimitAlarm));
                }
                
                if (newSystemVariables.SuspendedBeltTemperatureLowerLimitAlarm == true &&
                    _systemVariables.SuspendedBeltTemperatureLowerLimitAlarm == false)
                {
                    // 悬胶测温下限报警
                    DataManager.Instance.InsertHistoryWarningMc("悬胶测温下限报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltTemperatureLowerLimitAlarm),
                        "悬胶测温下限报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspendedBeltTemperatureLowerLimitAlarm == false &&
                         _systemVariables.SuspendedBeltTemperatureLowerLimitAlarm == true)
                {
                    //悬胶测温下限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶测温下限报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltTemperatureLowerLimitAlarm));
                }
                
                if (newSystemVariables.SuspendedBeltRollerBearingTemperatureUpperLimitAlarm == true &&
                    _systemVariables.SuspendedBeltRollerBearingTemperatureUpperLimitAlarm == false)
                {
                    // 悬胶滚筒轴承测温上限报警
                    DataManager.Instance.InsertHistoryWarningMc("悬胶滚筒轴承测温上限报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltRollerBearingTemperatureUpperLimitAlarm),
                        "悬胶滚筒轴承测温上限报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspendedBeltRollerBearingTemperatureUpperLimitAlarm == false &&
                         _systemVariables.SuspendedBeltRollerBearingTemperatureUpperLimitAlarm == true)
                {
                    //悬胶滚筒轴承测温上限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶滚筒轴承测温上限报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltRollerBearingTemperatureUpperLimitAlarm));
                }
                
                if (newSystemVariables.SuspendedBeltRollerBearingTemperatureLowerLimitAlarm == true &&
                    _systemVariables.SuspendedBeltRollerBearingTemperatureLowerLimitAlarm == false)
                {
                    // 悬胶滚筒轴承测温下限报警
                    DataManager.Instance.InsertHistoryWarningMc("悬胶滚筒轴承测温下限报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltRollerBearingTemperatureLowerLimitAlarm),
                        "悬胶滚筒轴承测温下限报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.SuspendedBeltRollerBearingTemperatureLowerLimitAlarm == false &&
                         _systemVariables.SuspendedBeltRollerBearingTemperatureLowerLimitAlarm == true)
                {
                    //悬胶滚筒轴承测温下限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶滚筒轴承测温下限报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltRollerBearingTemperatureLowerLimitAlarm));
                }
                
                if (newSystemVariables.BucketWheelTemperatureLowerLimitAlarm == true &&
                    _systemVariables.BucketWheelTemperatureLowerLimitAlarm == false)
                {
                    // 斗轮测温下限报警
                    DataManager.Instance.InsertHistoryWarningMc("斗轮测温下限报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelTemperatureLowerLimitAlarm),
                        "斗轮测温下限报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BucketWheelTemperatureLowerLimitAlarm == false &&
                         _systemVariables.BucketWheelTemperatureLowerLimitAlarm == true)
                {
                    //斗轮测温下限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮测温下限报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelTemperatureLowerLimitAlarm));
                }
                
                 
                if (newSystemVariables.BrokenBeltCaptureAlarming == true &&
                    _systemVariables.BrokenBeltCaptureAlarming == false)
                {
                    // 断带抓捕报警
                    DataManager.Instance.InsertHistoryWarningMc("断带抓捕报警", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BrokenBeltCaptureAlarming),
                        "断带抓捕报警",
                        Machine.BucketWheelStackerReclaimer, false, "");
                }
                else if (newSystemVariables.BrokenBeltCaptureAlarming == false &&
                         _systemVariables.BrokenBeltCaptureAlarming == true)
                {
                    //斗断带抓捕报警解除
                    DataManager.Instance.InsertHistoryWarningMc("断带抓捕报警解除", GetUserName(),
                        Machine.BucketWheelStackerReclaimer);
                    RemoveWarningDesDict(nameof(newSystemVariables.BrokenBeltCaptureAlarming));
                }
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>取料机报错信息<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                //取料机
                if (newSystemVariables.D2PLC1CommunicationState == false && _systemVariables.D2PLC1CommunicationState)
                {
                    //堆取料机PLC断线
                    DataManager.Instance.InsertHistoryWarningMc("斗轮机PLC1断线", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.D2PLC1CommunicationState), "斗轮机PLC1断线",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.D2PLC1CommunicationState &&
                         _systemVariables.D2PLC1CommunicationState == false)
                {
                    //堆取料机PLC1断线解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮机PLC1断线解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.D2PLC1CommunicationState));
                }

                if (newSystemVariables.D2PLC2CommunicationState == false && _systemVariables.D2PLC2CommunicationState)
                {
                    //堆取料机PLC断线
                    DataManager.Instance.InsertHistoryWarningMc("无人值守PLC2断线", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.D2PLC2CommunicationState), "无人值守PLC2断线",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.D2PLC2CommunicationState &&
                         _systemVariables.D2PLC2CommunicationState == false)
                {
                    //堆取料机PLC1断线解除
                    DataManager.Instance.InsertHistoryWarningMc("无人值守PLC2断线解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.D2PLC2CommunicationState));
                }

                if (newSystemVariables.DriverRoomEmergencyStopButton_2 &&
                    _systemVariables.DriverRoomEmergencyStopButton_2 == false)
                {
                    //司机室急停
                    DataManager.Instance.InsertHistoryWarningMc("司机室急停", GetUserName(),
                        Machine.BucketWheel);
                    // AddOrUpdateWarningDesQueue("司机室急停", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DriverRoomEmergencyStopButton_2), "司机室急停",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DriverRoomEmergencyStopButton_2 == false &&
                         _systemVariables.DriverRoomEmergencyStopButton_2 == true)
                {
                    //司机室急停解除
                    DataManager.Instance.InsertHistoryWarningMc("司机室急停解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DriverRoomEmergencyStopButton_2));
                }

                if (newSystemVariables.ElectricalRoomEmergencyStopButton_2 &&
                    _systemVariables.ElectricalRoomEmergencyStopButton_2 == false)
                {
                    //电气室急停
                    DataManager.Instance.InsertHistoryWarningMc("电气室急停", GetUserName(),
                        Machine.BucketWheel);
                    // AddOrUpdateWarningDesQueue("电气室急停", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ElectricalRoomEmergencyStopButton_2), "电气室急停",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.ElectricalRoomEmergencyStopButton_2 == false &&
                         _systemVariables.ElectricalRoomEmergencyStopButton_2 == true)
                {
                    //电气室急停解除
                    DataManager.Instance.InsertHistoryWarningMc("电气室急停解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.ElectricalRoomEmergencyStopButton_2));
                }

                if (newSystemVariables.EmergencyStopRelay_2 == false && _systemVariables.EmergencyStopRelay_2 == true)
                {
                    //急停继电器
                    DataManager.Instance.InsertHistoryWarningMc("急停继电器", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("急停继电器", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.EmergencyStopRelay_2), "急停继电器",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.EmergencyStopRelay_2 == true &&
                         _systemVariables.EmergencyStopRelay_2 == false)
                {
                    //急停继电器解除
                    DataManager.Instance.InsertHistoryWarningMc("急停继电器解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.EmergencyStopRelay_2));
                }

                if (newSystemVariables.RemoteEmergencyStop_2 && _systemVariables.RemoteEmergencyStop_2 == false)
                {
                    //远程急停
                    DataManager.Instance.InsertHistoryWarningMc("远程急停", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("远程急停", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RemoteEmergencyStop_2), "远程急停",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RemoteEmergencyStop_2 == false &&
                         _systemVariables.RemoteEmergencyStop_2 == true)
                {
                    //远程急停解除
                    DataManager.Instance.InsertHistoryWarningMc("远程急停解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RemoteEmergencyStop_2));
                }

                if (newSystemVariables.BucketWheelFault_2 && _systemVariables.BucketWheelFault_2 == false)
                {
                    //斗轮机故障
                    DataManager.Instance.InsertHistoryWarningMc("斗轮机故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("斗轮机故障", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelFault_2), "斗轮机故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelFault_2 == false && _systemVariables.BucketWheelFault_2 == true)
                {
                    //斗轮机故障解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮机故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelFault_2));
                }

                if (newSystemVariables.CentralControlRoomNoStackingOrDiversionCommand_2 &&
                    _systemVariables.CentralControlRoomNoStackingOrDiversionCommand_2 == false)
                {
                    //中控室没有允许取料命令命令
                    DataManager.Instance.InsertHistoryWarningMc("中控室没有允许取料命令", GetUserName(),
                        Machine.BucketWheel);
                    // AddOrUpdateWarningDesQueue("斗轮机故障", Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.CentralControlRoomNoStackingOrDiversionCommand_2),
                        "中控室没有允许取料命令",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.CentralControlRoomNoStackingOrDiversionCommand_2 == false &&
                         _systemVariables.CentralControlRoomNoStackingOrDiversionCommand_2 == true)
                {
                    //中控室没有允许取料命令解除
                    DataManager.Instance.InsertHistoryWarningMc("中控室没有允许取料命令解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.CentralControlRoomNoStackingOrDiversionCommand_2));
                }

                if (newSystemVariables.LargeCarFault_2 && _systemVariables.LargeCarFault_2 == false)
                {
                    //大车-大车故障
                    DataManager.Instance.InsertHistoryWarningMc("大车-大车故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("大车-大车故障", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarFault_2), "大车-大车故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarFault_2 == false && _systemVariables.LargeCarFault_2 == true)
                {
                    //大车-大车故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-大车故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarFault_2));
                }

                if (newSystemVariables.LargeCarFrequencyConverterFault_2 &&
                    _systemVariables.LargeCarFrequencyConverterFault_2 == false)
                {
                    //大车-变频器故障
                    DataManager.Instance.InsertHistoryWarningMc("大车-变频器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("大车-变频器故障", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarFrequencyConverterFault_2), "大车-变频器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarFrequencyConverterFault_2 == false &&
                         _systemVariables.LargeCarFrequencyConverterFault_2 == true)
                {
                    //大车-变频器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-变频器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarFrequencyConverterFault_2));
                }

                if (newSystemVariables.LargeCarBrakeResistorOverheatSwitch_2 == false &&
                    _systemVariables.LargeCarBrakeResistorOverheatSwitch_2 == true)
                {
                    //大车-制动电阻超温
                    DataManager.Instance.InsertHistoryWarningMc("大车-制动电阻超温", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("大车-制动电阻超温", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarBrakeResistorOverheatSwitch_2),
                        "大车-制动电阻超温",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarBrakeResistorOverheatSwitch_2 == true &&
                         _systemVariables.LargeCarBrakeResistorOverheatSwitch_2 == false)
                {
                    //大车-制动电阻超温解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-制动电阻超温解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarBrakeResistorOverheatSwitch_2));
                }

                // if (newSystemVariables.LargeCarCentralizedLubricationLowOilLevel_2 &&
                //     _systemVariables.LargeCarCentralizedLubricationLowOilLevel_2 == false)
                // {
                //     //大车-大车集中润滑低油位
                //     DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑低油位", GetUserName(),
                //         Machine.BucketWheel);
                //     AddOrUpdateWarningDesQueue("大车-大车集中润滑低油位", Machine.BucketWheel);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarCentralizedLubricationLowOilLevel_2),
                //         "大车-大车集中润滑低油位",
                //         Machine.BucketWheel, false, "");
                // }
                // else if (newSystemVariables.LargeCarCentralizedLubricationLowOilLevel_2 == false &&
                //          _systemVariables.LargeCarCentralizedLubricationLowOilLevel_2 == true)
                // {
                //     //大车-大车集中润滑低油位解除
                //     DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑低油位解除", GetUserName(),
                //         Machine.BucketWheel);
                //     RemoveWarningDesDict(nameof(newSystemVariables.LargeCarCentralizedLubricationLowOilLevel_2));
                // }

                // if (newSystemVariables.LargeCarCentralizedLubricationOilBlockage_2 &&
                //     _systemVariables.LargeCarCentralizedLubricationOilBlockage_2 == false)
                // {
                //     //大车-大车集中润滑堵油
                //     DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑堵油", GetUserName(),
                //         Machine.BucketWheel);
                //     AddOrUpdateWarningDesQueue("大车-大车集中润滑堵油", Machine.BucketWheel);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarCentralizedLubricationOilBlockage_2),
                //         "大车-大车集中润滑堵油",
                //         Machine.BucketWheel, false, "");
                // }
                // else if (newSystemVariables.LargeCarCentralizedLubricationOilBlockage_2 == false &&
                //          _systemVariables.LargeCarCentralizedLubricationOilBlockage_2 == true)
                // {
                //     //大车-大车集中润滑堵油解除
                //     DataManager.Instance.InsertHistoryWarningMc("大车-大车集中润滑堵油解除", GetUserName(),
                //         Machine.BucketWheel);
                //     RemoveWarningDesDict(nameof(newSystemVariables.LargeCarCentralizedLubricationOilBlockage_2));
                // }

                if (newSystemVariables.LargeCarForwardLimit_2 && _systemVariables.LargeCarForwardLimit_2 == false)
                {
                    //大车-前进限位
                    DataManager.Instance.InsertHistoryWarningMc("大车-前进限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("大车-前进限位", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarForwardLimit_2), "大车-前进限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarForwardLimit_2 == false &&
                         _systemVariables.LargeCarForwardLimit_2 == true)
                {
                    //大车-前进限位解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-前进限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarForwardLimit_2));
                }

                if (newSystemVariables.LargeCarForwardExtremeLimit_2 &&
                    _systemVariables.LargeCarForwardExtremeLimit_2 == false)
                {
                    //大车-前进极限
                    DataManager.Instance.InsertHistoryWarningMc("大车-前进极限", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("大车-前进极限", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarForwardExtremeLimit_2), "大车-前进极限",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarForwardExtremeLimit_2 == false &&
                         _systemVariables.LargeCarForwardExtremeLimit_2 == true)
                {
                    //大车-前进极限解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-前进极限解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarForwardExtremeLimit_2));
                }

                if (newSystemVariables.LargeCarReverseLimit_2 && _systemVariables.LargeCarReverseLimit_2 == false)
                {
                    //大车-后退限位
                    DataManager.Instance.InsertHistoryWarningMc("大车-后退限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("大车-后退限位", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarReverseLimit_2), "大车-后退限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarReverseLimit_2 == false &&
                         _systemVariables.LargeCarReverseLimit_2 == true)
                {
                    //大车-后退限位解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-后退限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarReverseLimit_2));
                }

                if (newSystemVariables.LargeCarReverseExtremeLimit_2 &&
                    _systemVariables.LargeCarReverseExtremeLimit_2 == false)
                {
                    //大车-后退极限
                    DataManager.Instance.InsertHistoryWarningMc("大车-后退极限", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("大车-后退极限", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarReverseExtremeLimit_2), "大车-后退极限",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarReverseExtremeLimit_2 == false &&
                         _systemVariables.LargeCarReverseExtremeLimit_2 == true)
                {
                    //大车-后退极限解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-后退极限解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarReverseExtremeLimit_2));
                }

                if (newSystemVariables.TwoMachineCollisionAlarm_2 &&
                    _systemVariables.TwoMachineCollisionAlarm_2 == false)
                {
                    //大车-两车碰撞报警
                    DataManager.Instance.InsertHistoryWarningMc("大车-两车碰撞报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("大车-两车碰撞报警", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TwoMachineCollisionAlarm_2), "大车-两车碰撞报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.TwoMachineCollisionAlarm_2 == false &&
                         _systemVariables.TwoMachineCollisionAlarm_2 == true)
                {
                    //大车-两车碰撞报警解除
                    DataManager.Instance.InsertHistoryWarningMc("大车-两车碰撞报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.TwoMachineCollisionAlarm_2));
                }

                if (newSystemVariables.VariableAmplitudeMotorOverload_2 &&
                    _systemVariables.VariableAmplitudeMotorOverload_2 == false)
                {
                    //变幅-主电机过载
                    DataManager.Instance.InsertHistoryWarningMc("变幅-主电机过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("变幅-主电机过载", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeMotorOverload_2), "变幅-主电机过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeMotorOverload_2 == false &&
                         _systemVariables.VariableAmplitudeMotorOverload_2 == true)
                {
                    //变幅-主电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-主电机过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeMotorOverload_2));
                }

                if (newSystemVariables.VariableAmplitudeUpperLimit_2 &&
                    _systemVariables.VariableAmplitudeUpperLimit_2 == false)
                {
                    //变幅-上仰限位
                    DataManager.Instance.InsertHistoryWarningMc("变幅-上仰限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("变幅-上仰限位", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeUpperLimit_2), "变幅-上仰限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeUpperLimit_2 == false &&
                         _systemVariables.VariableAmplitudeUpperLimit_2 == true)
                {
                    //变幅-上仰限位解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-上仰限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeUpperLimit_2));
                }

                if (newSystemVariables.VariableAmplitudeUpperExtremeLimit_2 &&
                    _systemVariables.VariableAmplitudeUpperExtremeLimit_2 == false)
                {
                    //变幅-上仰极限
                    DataManager.Instance.InsertHistoryWarningMc("变幅-上仰极限", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("变幅-上仰极限", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeUpperExtremeLimit_2),
                        "变幅-上仰极限",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeUpperExtremeLimit_2 == false &&
                         _systemVariables.VariableAmplitudeUpperExtremeLimit_2 == true)
                {
                    //变幅-上仰极限解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-上仰极限解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeUpperExtremeLimit_2));
                }

                if (newSystemVariables.VariableAmplitudeLowerLimit_2 &&
                    _systemVariables.VariableAmplitudeLowerLimit_2 == false)
                {
                    //变幅-下俯限位
                    DataManager.Instance.InsertHistoryWarningMc("变幅-下俯限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("变幅-下俯限位", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLowerLimit_2), "变幅-下俯限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeLowerLimit_2 == false &&
                         _systemVariables.VariableAmplitudeLowerLimit_2 == true)
                {
                    //变幅-下俯限位解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-下俯限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLowerLimit_2));
                }

                if (newSystemVariables.VariableAmplitudeLowerExtremeLimit_2 &&
                    _systemVariables.VariableAmplitudeLowerExtremeLimit_2 == false)
                {
                    //变幅-下俯极限
                    DataManager.Instance.InsertHistoryWarningMc("变幅-下俯极限", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("变幅-下俯极限", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLowerExtremeLimit_2),
                        "变幅-下俯极限",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeLowerExtremeLimit_2 == false &&
                         _systemVariables.VariableAmplitudeLowerExtremeLimit_2 == true)
                {
                    //变幅-下俯极限解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-下俯极限解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLowerExtremeLimit_2));
                }

                if (newSystemVariables.VariableAmplitudeLowerForbiddenZoneLimit_2 &&
                    _systemVariables.VariableAmplitudeLowerForbiddenZoneLimit_2 == false)
                {
                    //变幅-下俯禁区
                    DataManager.Instance.InsertHistoryWarningMc("变幅-下俯禁区", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("变幅-下俯禁区", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLowerForbiddenZoneLimit_2),
                        "变幅-下俯禁区",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeLowerForbiddenZoneLimit_2 == false &&
                         _systemVariables.VariableAmplitudeLowerForbiddenZoneLimit_2 == true)
                {
                    //变幅-下俯禁区解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-下俯禁区解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLowerForbiddenZoneLimit_2));
                }

                if (newSystemVariables.VariableAmplitudePumpStationOverheatAlarm_2 &&
                    _systemVariables.VariableAmplitudePumpStationOverheatAlarm_2 == false)
                {
                    //变幅-泵站高温报警
                    DataManager.Instance.InsertHistoryWarningMc("变幅-泵站高温报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("变幅-泵站高温报警", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudePumpStationOverheatAlarm_2),
                        "变幅-泵站高温报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableAmplitudePumpStationOverheatAlarm_2 == false &&
                         _systemVariables.VariableAmplitudePumpStationOverheatAlarm_2 == true)
                {
                    //变幅-泵站高温报警解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-泵站高温报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudePumpStationOverheatAlarm_2));
                }

                if (newSystemVariables.VariableAmplitudeOilLevelVeryLowSignal_2 &&
                    _systemVariables.VariableAmplitudeOilLevelVeryLowSignal_2 == false)
                {
                    //变幅-油液位低信号
                    DataManager.Instance.InsertHistoryWarningMc("变幅-油液位低信号", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("变幅-油液位低信号", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilLevelVeryLowSignal_2),
                        "变幅-油液位低信号",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeOilLevelVeryLowSignal_2 == false &&
                         _systemVariables.VariableAmplitudeOilLevelVeryLowSignal_2 == true)
                {
                    //变幅-油液位低信号解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-油液位低信号解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilLevelVeryLowSignal_2));
                }

                if (newSystemVariables.VariableAmplitudeOilLevelLowSignal_2 &&
                    _systemVariables.VariableAmplitudeOilLevelLowSignal_2 == false)
                {
                    //变幅-液位超低信号
                    DataManager.Instance.InsertHistoryWarningMc("变幅-液位超低信号", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("变幅-液位超低信号", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilLevelLowSignal_2),
                        "变幅-液位超低信号",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeOilLevelLowSignal_2 == false &&
                         _systemVariables.VariableAmplitudeOilLevelLowSignal_2 == true)
                {
                    //变幅-液位超低信号解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-液位超低信号解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilLevelLowSignal_2));
                }

                if (newSystemVariables.VariableFrequencyOilBlockageSignal_2 &&
                    _systemVariables.VariableFrequencyOilBlockageSignal_2 == false)
                {
                    //变幅-泵站堵油信号
                    DataManager.Instance.InsertHistoryWarningMc("变幅-泵站堵油信号", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("变幅-泵站堵油信号", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableFrequencyOilBlockageSignal_2),
                        "变幅-泵站堵油信号",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableFrequencyOilBlockageSignal_2 == false &&
                         _systemVariables.VariableFrequencyOilBlockageSignal_2 == true)
                {
                    //变幅-泵站堵油信号解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅-泵站堵油信号解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableFrequencyOilBlockageSignal_2));
                }

                if (newSystemVariables.RotaryFrequencyConverterFault_2 &&
                    _systemVariables.RotaryFrequencyConverterFault_2 == false)
                {
                    //回转-变频器故障
                    DataManager.Instance.InsertHistoryWarningMc("回转-变频器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("回转-变频器故障", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryFrequencyConverterFault_2), "回转-变频器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryFrequencyConverterFault_2 == false &&
                         _systemVariables.RotaryFrequencyConverterFault_2 == true)
                {
                    //回转-变频器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-变频器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryFrequencyConverterFault_2));
                }

                if (newSystemVariables.RotaryBrakeOverload_2 && _systemVariables.RotaryBrakeOverload_2 == false)
                {
                    //回转-制动器过载
                    DataManager.Instance.InsertHistoryWarningMc("回转-制动器过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("回转-制动器过载", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryBrakeOverload_2), "回转-制动器过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryBrakeOverload_2 == false &&
                         _systemVariables.RotaryBrakeOverload_2 == true)
                {
                    //回转-制动器过载解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-制动器过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryBrakeOverload_2));
                }

                if (newSystemVariables.RotaryFanOverload_2 && _systemVariables.RotaryFanOverload_2 == false)
                {
                    //回转-风机过载
                    DataManager.Instance.InsertHistoryWarningMc("回转-风机过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("回转-风机过载", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryFanOverload_2), "回转-风机过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryFanOverload_2 == false &&
                         _systemVariables.RotaryFanOverload_2 == true)
                {
                    //回转-风机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-风机过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryFanOverload_2));
                }

                if (newSystemVariables.RotaryBrakeResistorOverheatSwitch_2 == false &&
                    _systemVariables.RotaryBrakeResistorOverheatSwitch_2 == true)
                {
                    //回转-制动电阻超温
                    DataManager.Instance.InsertHistoryWarningMc("回转-制动电阻超温", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("回转-制动电阻超温", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryBrakeResistorOverheatSwitch_2),
                        "回转-制动电阻超温",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryBrakeResistorOverheatSwitch_2 == true &&
                         _systemVariables.RotaryBrakeResistorOverheatSwitch_2 == false)
                {
                    //回转-制动电阻超温解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-制动电阻超温解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryBrakeResistorOverheatSwitch_2));
                }

                if (newSystemVariables.RotaryFault_2 && _systemVariables.RotaryFault_2 == false)
                {
                    //回转-回转故障
                    DataManager.Instance.InsertHistoryWarningMc("回转-回转故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("回转-回转故障", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryFault_2), "回转-回转故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryFault_2 == false && _systemVariables.RotaryFault_2 == true)
                {
                    //回转-回转故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-回转故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryFault_2));
                }

                if (newSystemVariables.RotaryLeftTurnLimit_2 && _systemVariables.RotaryLeftTurnLimit_2 == false)
                {
                    //回转-左转限位
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("回转-左转限位", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnLimit_2), "回转-左转限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryLeftTurnLimit_2 == false &&
                         _systemVariables.RotaryLeftTurnLimit_2 == true)
                {
                    //回转-左转限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnLimit_2));
                }

                if (newSystemVariables.RotaryLeftTurnExtremeLimit_2 &&
                    _systemVariables.RotaryLeftTurnExtremeLimit_2 == false)
                {
                    //回转-左转极限
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转极限", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("回转-左转极限", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnExtremeLimit_2), "回转-左转极限",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryLeftTurnExtremeLimit_2 == false &&
                         _systemVariables.RotaryLeftTurnExtremeLimit_2 == true)
                {
                    //回转-左转极限解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转极限解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnExtremeLimit_2));
                }

                if (newSystemVariables.RotaryLeftTurnForbiddenZoneLimit_2 &&
                    _systemVariables.RotaryLeftTurnForbiddenZoneLimit_2 == false)
                {
                    //回转-左转禁区限位
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转禁区限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("回转-左转禁区限位", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnForbiddenZoneLimit_2),
                        "回转-左转禁区限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryLeftTurnForbiddenZoneLimit_2 == false &&
                         _systemVariables.RotaryLeftTurnForbiddenZoneLimit_2 == true)
                {
                    //回转-左转禁区限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转禁区限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnForbiddenZoneLimit_2));
                }

                if (newSystemVariables.RotaryRightTurnLimit_2 && _systemVariables.RotaryRightTurnLimit_2 == false)
                {
                    //回转-右转限位
                    DataManager.Instance.InsertHistoryWarningMc("回转-右转限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("回转-右转限位", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryRightTurnLimit_2), "回转-右转限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryRightTurnLimit_2 == false &&
                         _systemVariables.RotaryRightTurnLimit_2 == true)
                {
                    //回转-右转限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-右转限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryRightTurnLimit_2));
                }

                if (newSystemVariables.RotaryRightTurnExtremeLimit_2 &&
                    _systemVariables.RotaryRightTurnExtremeLimit_2 == false)
                {
                    //回转-右转极限
                    DataManager.Instance.InsertHistoryWarningMc("回转-右转极限", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("回转-右转极限", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryRightTurnExtremeLimit_2), "回转-右转极限",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryRightTurnExtremeLimit_2 == false &&
                         _systemVariables.RotaryRightTurnExtremeLimit_2 == true)
                {
                    //回转-右转极限解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-右转极限解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryRightTurnExtremeLimit_2));
                }
                //要求删除
                // if (newSystemVariables.RotaryRightTurnForbiddenZoneLimit_2 &&
                //     _systemVariables.RotaryRightTurnForbiddenZoneLimit_2 == false)
                // {
                //     //回转-右转禁区限位
                //     DataManager.Instance.InsertHistoryWarningMc("回转-右转禁区限位", GetUserName(),
                //         Machine.BucketWheel);
                //     AddOrUpdateWarningDesQueue("回转-右转禁区限位", Machine.BucketWheel);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryRightTurnForbiddenZoneLimit_2),
                //         "回转-右转禁区限位",
                //         Machine.BucketWheel, false, "");
                // }
                // else if (newSystemVariables.RotaryRightTurnForbiddenZoneLimit_2 == false &&
                //          _systemVariables.RotaryRightTurnForbiddenZoneLimit_2 == true)
                // {
                //     //回转-右转禁区限位解除
                //     DataManager.Instance.InsertHistoryWarningMc("回转-右转禁区限位解除", GetUserName(),
                //         Machine.BucketWheel);
                //     RemoveWarningDesDict(nameof(newSystemVariables.RotaryRightTurnForbiddenZoneLimit_2));
                // }

                if (newSystemVariables.RotaryRightTurnForbiddenLimit_2 &&
                    _systemVariables.RotaryRightTurnForbiddenLimit_2 == false)
                {
                    //回转-右转防撞限位
                    DataManager.Instance.InsertHistoryWarningMc("回转-右转防撞限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("回转-右转防撞限位", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryRightTurnForbiddenLimit_2), "回转-右转防撞限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryRightTurnForbiddenLimit_2 == false &&
                         _systemVariables.RotaryRightTurnForbiddenLimit_2 == true)
                {
                    //回转-右转防撞限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-右转防撞限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryRightTurnForbiddenLimit_2));
                }

                if (newSystemVariables.RotaryOverTorque_2 && _systemVariables.RotaryOverTorque_2 == false)
                {
                    //回转-回转过力矩
                    DataManager.Instance.InsertHistoryWarningMc("回转-回转过力矩", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("回转-回转过力矩", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryOverTorque_2), "回转-回转过力矩",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryOverTorque_2 == false && _systemVariables.RotaryOverTorque_2 == true)
                {
                    //回转-回转过力矩解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-回转过力矩解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryOverTorque_2));
                }

                // if (newSystemVariables.RotaryCentralizedLubricationOilBlockageFault_2 &&
                //     _systemVariables.RotaryCentralizedLubricationOilBlockageFault_2 == false)
                // {
                //     //回转-回转集中润滑堵油
                //     DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑堵油", GetUserName(),
                //         Machine.BucketWheel);
                //     AddOrUpdateWarningDesQueue("回转-回转集中润滑堵油", Machine.BucketWheel);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationOilBlockageFault_2),
                //         "回转-回转集中润滑堵油",
                //         Machine.BucketWheel, false, "");
                // }
                // else if (newSystemVariables.RotaryCentralizedLubricationOilBlockageFault_2 == false &&
                //          _systemVariables.RotaryCentralizedLubricationOilBlockageFault_2 == true)
                // {
                //     //回转-回转集中润滑堵油解除
                //     DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑堵油解除", GetUserName(),
                //         Machine.BucketWheel);
                //     RemoveWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationOilBlockageFault_2));
                // }

                // if (newSystemVariables.RotaryCentralizedLubricationLowOilLevelFault_2 &&
                //     _systemVariables.RotaryCentralizedLubricationLowOilLevelFault_2 == false)
                // {
                //     //回转-回转集中润滑低油位
                //     DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑低油位", GetUserName(),
                //         Machine.BucketWheel);
                //     AddOrUpdateWarningDesQueue("回转-回转集中润滑低油位", Machine.BucketWheel);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationLowOilLevelFault_2),
                //         "回转-回转集中润滑低油位",
                //         Machine.BucketWheel, false, "");
                // }
                // else if (newSystemVariables.RotaryCentralizedLubricationLowOilLevelFault_2 == false &&
                //          _systemVariables.RotaryCentralizedLubricationLowOilLevelFault_2 == true)
                // {
                //     //回转-回转集中润滑低油位解除
                //     DataManager.Instance.InsertHistoryWarningMc("回转-回转集中润滑低油位解除", GetUserName(),
                //         Machine.BucketWheel);
                //     RemoveWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationLowOilLevelFault_2));
                // }

                if (newSystemVariables.BucketWheelMotorOverload_2 &&
                    _systemVariables.BucketWheelMotorOverload_2 == false)
                {
                    //斗轮/槽-电机过载
                    DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-电机过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("斗轮/槽-电机过载", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelMotorOverload_2), "斗轮/槽-电机过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelMotorOverload_2 == false &&
                         _systemVariables.BucketWheelMotorOverload_2 == true)
                {
                    //斗轮/槽-电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-电机过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelMotorOverload_2));
                }

                if (newSystemVariables.BucketWheelOverTorqueSwitch_2 &&
                    _systemVariables.BucketWheelOverTorqueSwitch_2 == false)
                {
                    //斗轮/槽-斗轮过力矩开关
                    DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-斗轮过力矩开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("斗轮/槽-斗轮过力矩开关", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelOverTorqueSwitch_2), "斗轮/槽-斗轮过力矩开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelOverTorqueSwitch_2 == false &&
                         _systemVariables.BucketWheelOverTorqueSwitch_2 == true)
                {
                    //斗轮/槽-斗轮过力矩开关解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮/槽-斗轮过力矩开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelOverTorqueSwitch_2));
                }

                if (newSystemVariables.BucketWheelSlotMotorOverload_2 &&
                    _systemVariables.BucketWheelSlotMotorOverload_2 == false)
                {
                    //斗轮导料槽-电机过载
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽-电机过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("斗轮导料槽-电机过载", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelSlotMotorOverload_2), "斗轮导料槽-电机过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelSlotMotorOverload_2 == false &&
                         _systemVariables.BucketWheelSlotMotorOverload_2 == true)
                {
                    //斗轮导料槽-电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽-电机过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelSlotMotorOverload_2));
                }

                if (newSystemVariables.SuspensionBeltMotorOverload_2 &&
                    _systemVariables.SuspensionBeltMotorOverload_2 == false)
                {
                    //悬胶/挡板-电机过载
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-电机过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("悬胶/挡板-电机过载", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltMotorOverload_2), "悬胶/挡板-电机过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspensionBeltMotorOverload_2 == false &&
                         _systemVariables.SuspensionBeltMotorOverload_2 == true)
                {
                    //悬胶/挡板-电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-电机过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltMotorOverload_2));
                }

                if (newSystemVariables.SuspensionBeltFirstLevelDeviationSwitch_2 &&
                    _systemVariables.SuspensionBeltFirstLevelDeviationSwitch_2 == false)
                {
                    //悬胶/挡板-一级跑偏开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-一级跑偏开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("悬胶/挡板-一级跑偏开关", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltFirstLevelDeviationSwitch_2),
                        "悬胶/挡板-一级跑偏开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspensionBeltFirstLevelDeviationSwitch_2 == false &&
                         _systemVariables.SuspensionBeltFirstLevelDeviationSwitch_2 == true)
                {
                    //悬胶/挡板-一级跑偏开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-一级跑偏开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltFirstLevelDeviationSwitch_2));
                }

                if (newSystemVariables.SuspensionBeltSecondLevelDeviationSwitch_2 &&
                    _systemVariables.SuspensionBeltSecondLevelDeviationSwitch_2 == false)
                {
                    //悬胶/挡板-二级跑偏开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-二级跑偏开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("悬胶/挡板-二级跑偏开关", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltSecondLevelDeviationSwitch_2),
                        "悬胶/挡板-二级跑偏开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspensionBeltSecondLevelDeviationSwitch_2 == false &&
                         _systemVariables.SuspensionBeltSecondLevelDeviationSwitch_2 == true)
                {
                    //悬胶/挡板-二级跑偏开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-二级跑偏开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltSecondLevelDeviationSwitch_2));
                }

                if (newSystemVariables.SuspendedBeltSlip_2 && _systemVariables.SuspendedBeltSlip_2 == false)
                {
                    //悬胶/挡板-打滑检测开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-打滑检测开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("悬胶/挡板-打滑检测开关", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltSlip_2), "悬胶/挡板-打滑检测开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspendedBeltSlip_2 == false &&
                         _systemVariables.SuspendedBeltSlip_2 == true)
                {
                    //悬胶/挡板-打滑检测开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-打滑检测开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltSlip_2));
                }

                if (newSystemVariables.SuspensionBeltLongitudinalTearSwitch_2 &&
                    _systemVariables.SuspensionBeltLongitudinalTearSwitch_2 == false)
                {
                    //悬胶/挡板-纵向撕裂开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-纵向撕裂开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("悬胶/挡板-纵向撕裂开关", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltLongitudinalTearSwitch_2),
                        "悬胶/挡板-纵向撕裂开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspensionBeltLongitudinalTearSwitch_2 == false &&
                         _systemVariables.SuspensionBeltLongitudinalTearSwitch_2 == true)
                {
                    //悬胶/挡板-纵向撕裂开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-纵向撕裂开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltLongitudinalTearSwitch_2));
                }

                if (newSystemVariables.SuspensionBeltEmergencyStopSwitch_2 &&
                    _systemVariables.SuspensionBeltEmergencyStopSwitch_2 == false)
                {
                    //悬胶/挡板-急停拉线开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-急停拉线开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("悬胶/挡板-急停拉线开关", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltEmergencyStopSwitch_2),
                        "悬胶/挡板-急停拉线开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspensionBeltEmergencyStopSwitch_2 == false &&
                         _systemVariables.SuspensionBeltEmergencyStopSwitch_2 == true)
                {
                    //悬胶/挡板-急停拉线开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-急停拉线开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltEmergencyStopSwitch_2));
                }

                if (newSystemVariables.SuspensionBeltMaterialFlowDetectionSwitch_2 &&
                    _systemVariables.SuspensionBeltMaterialFlowDetectionSwitch_2 == false)
                {
                    //悬胶/挡板-料流检测开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-料流检测开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("悬胶/挡板-料流检测开关", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltMaterialFlowDetectionSwitch_2),
                        "悬胶/挡板-料流检测开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspensionBeltMaterialFlowDetectionSwitch_2 == false &&
                         _systemVariables.SuspensionBeltMaterialFlowDetectionSwitch_2 == true)
                {
                    //悬胶/挡板-料流检测开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-料流检测开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltMaterialFlowDetectionSwitch_2));
                }

                if (newSystemVariables.CentralMaterialDustDetectionSwitch_2 == false &&
                    _systemVariables.CentralMaterialDustDetectionSwitch_2 == true)
                {
                    //悬胶/挡板-中部料斗堵煤
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-中部料斗堵煤", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("悬胶/挡板-中部料斗堵煤", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CentralMaterialDustDetectionSwitch_2),
                        "悬胶/挡板-中部料斗堵煤",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.CentralMaterialDustDetectionSwitch_2 == true &&
                         _systemVariables.CentralMaterialDustDetectionSwitch_2 == false)
                {
                    //悬胶/挡板-中部料斗堵煤解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-中部料斗堵煤解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.CentralMaterialDustDetectionSwitch_2));
                }

                if (newSystemVariables.DiversionBaffleMotorOverload_2 &&
                    _systemVariables.DiversionBaffleMotorOverload_2 == false)
                {
                    //分流挡板-电机过载
                    DataManager.Instance.InsertHistoryWarningMc("分流挡板-电机过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("分流挡板-电机过载", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DiversionBaffleMotorOverload_2), "分流挡板-电机过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DiversionBaffleMotorOverload_2 == false &&
                         _systemVariables.DiversionBaffleMotorOverload_2 == true)
                {
                    //分流挡板-电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("分流挡板-电机过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DiversionBaffleMotorOverload_2));
                }

                if (newSystemVariables.DiversionPlateTimeout_2 &&
                    _systemVariables.DiversionPlateTimeout_2 == false)
                {
                    //分流挡板运行超时
                    DataManager.Instance.InsertHistoryWarningMc("分流挡板运行超时", GetUserName(),
                        Machine.BucketWheel);
                    // AddOrUpdateWarningDesQueue("分流挡板-电机过载", Machine.BucketWheelStackerReclaimer);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DiversionPlateTimeout_2), "分流挡板运行超时",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DiversionPlateTimeout_2 == false &&
                         _systemVariables.DiversionPlateTimeout_2 == true)
                {
                    //分流挡板运行超时
                    DataManager.Instance.InsertHistoryWarningMc("分流挡板运行超时解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DiversionPlateTimeout_2));
                }

                if (newSystemVariables.CableReelMotorOverload_2 && _systemVariables.CableReelMotorOverload_2 == false)
                {
                    //夹轨/卷筒-电缆卷筒-卷筒电机过载
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒电机过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("电缆卷筒-卷筒电机过载", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CableReelMotorOverload_2), "电缆卷筒-卷筒电机过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.CableReelMotorOverload_2 == false &&
                         _systemVariables.CableReelMotorOverload_2 == true)
                {
                    //夹轨/卷筒-电缆卷筒-卷筒电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒电机过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.CableReelMotorOverload_2));
                }

                if (newSystemVariables.ReelOverTensionLimit1_2 == false && newSystemVariables.RollerOverTightLimit2_2 &&
                    (_systemVariables.ReelOverTensionLimit1_2 == true ||
                     _systemVariables.RollerOverTightLimit2_2 == false))
                {
                    //夹轨/卷筒-电缆卷筒-卷筒过紧限位1
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧1过紧2限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ReelOverTensionLimit1_2), "电缆卷筒-卷筒过紧1过紧2限位",
                        Machine.BucketWheel, false, "");
                }
                else if ((newSystemVariables.ReelOverTensionLimit1_2 == true &&
                          _systemVariables.ReelOverTensionLimit1_2 == false) ||
                         (newSystemVariables.RollerOverTightLimit2_2 == false &&
                          _systemVariables.RollerOverTightLimit2_2 == true))
                {
                    if (WarningCellDataDict.ContainsKey(nameof(newSystemVariables.ReelOverTensionLimit1_2)))
                    {
                        //夹轨/卷筒-电缆卷筒-卷筒过紧限位1解除
                        DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧1过紧2限位解除", GetUserName(),
                            Machine.BucketWheel);
                    }

                    RemoveWarningDesDict(nameof(newSystemVariables.ReelOverTensionLimit1_2));
                }

                if (newSystemVariables.ReelOverLooseLimit1_2 && newSystemVariables.RollerOverLooseLimit2_2 &&
                    (_systemVariables.ReelOverLooseLimit1_2 == false ||
                     _systemVariables.RollerOverLooseLimit2_2 == false))
                {
                    //夹轨/卷筒-电缆卷筒-卷筒过松限位1
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松1过松2限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ReelOverLooseLimit1_2), "电缆卷筒-卷筒过松1过松2限位",
                        Machine.BucketWheel, false, "");
                }
                else if ((newSystemVariables.ReelOverLooseLimit1_2 == false &&
                          _systemVariables.ReelOverLooseLimit1_2 == true) ||
                         (newSystemVariables.RollerOverLooseLimit2_2 == false &&
                          _systemVariables.RollerOverLooseLimit2_2 == true))
                {
                    if (WarningCellDataDict.ContainsKey(nameof(newSystemVariables.ReelOverLooseLimit1_2)))
                    {
                        //夹轨/卷筒-电缆卷筒-卷筒过松限位1解除
                        DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松1过松2限位解除", GetUserName(),
                            Machine.BucketWheel);
                    }

                    RemoveWarningDesDict(nameof(newSystemVariables.ReelOverLooseLimit1_2));
                }
                // if (newSystemVariables.ReelOverTensionLimit1_2 == false &&
                //     _systemVariables.ReelOverTensionLimit1_2 == true)
                // {
                //     //夹轨/卷筒-电缆卷筒-卷筒过紧限位1
                //     DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧限位1", GetUserName(),
                //         Machine.BucketWheel);
                //     AddOrUpdateWarningDesQueue("电缆卷筒-卷筒过紧限位1", Machine.BucketWheel);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.ReelOverTensionLimit1_2), "电缆卷筒-卷筒过紧限位1",
                //         Machine.BucketWheel, false, "");
                // }
                // else if (newSystemVariables.ReelOverTensionLimit1_2 == true &&
                //          _systemVariables.ReelOverTensionLimit1_2 == false)
                // {
                //     //夹轨/卷筒-电缆卷筒-卷筒过紧限位1解除
                //     DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧限位1解除", GetUserName(),
                //         Machine.BucketWheel);
                //     RemoveWarningDesDict(nameof(newSystemVariables.ReelOverTensionLimit1_2));
                // }
                //
                // if (newSystemVariables.ReelOverLooseLimit1_2 && _systemVariables.ReelOverLooseLimit1_2 == false)
                // {
                //     //夹轨/卷筒-电缆卷筒-卷筒过松限位1
                //     DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松限位1", GetUserName(),
                //         Machine.BucketWheel);
                //     AddOrUpdateWarningDesQueue("电缆卷筒-卷筒过松限位1", Machine.BucketWheel);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.ReelOverLooseLimit1_2), "电缆卷筒-卷筒过松限位1",
                //         Machine.BucketWheel, false, "");
                // }
                // else if (newSystemVariables.ReelOverLooseLimit1_2 == false &&
                //          _systemVariables.ReelOverLooseLimit1_2 == true)
                // {
                //     //夹轨/卷筒-电缆卷筒-卷筒过松限位1解除
                //     DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松限位1解除", GetUserName(),
                //         Machine.BucketWheel);
                //     RemoveWarningDesDict(nameof(newSystemVariables.ReelOverLooseLimit1_2));
                // }

                // if (newSystemVariables.RollerOverTightLimit2_2 && _systemVariables.RollerOverTightLimit2_2 == false)
                // {
                //     //夹轨/卷筒-电缆卷筒-卷筒过紧限位2
                //     DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧限位2", GetUserName(),
                //         Machine.BucketWheel);
                //     AddOrUpdateWarningDesQueue("电缆卷筒-卷筒过紧限位2", Machine.BucketWheel);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.RollerOverTightLimit2_2), "电缆卷筒-卷筒过紧限位2",
                //         Machine.BucketWheel, false, "");
                // }
                // else if (newSystemVariables.RollerOverTightLimit2_2 == false &&
                //          _systemVariables.RollerOverTightLimit2_2 == true)
                // {
                //     //夹轨/卷筒-电缆卷筒-卷筒过紧限位2解除
                //     DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过紧限位2解除", GetUserName(),
                //         Machine.BucketWheel);
                //     RemoveWarningDesDict(nameof(newSystemVariables.RollerOverTightLimit2_2));
                // }
                //
                // if (newSystemVariables.RollerOverLooseLimit2_2 && _systemVariables.RollerOverLooseLimit2_2 == false)
                // {
                //     //夹轨/卷筒-电缆卷筒-卷筒过松限位2
                //     DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松限位2", GetUserName(),
                //         Machine.BucketWheel);
                //     AddOrUpdateWarningDesQueue("电缆卷筒-卷筒过松限位2", Machine.BucketWheel);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.RollerOverLooseLimit2_2), "电缆卷筒-卷筒过松限位2",
                //         Machine.BucketWheel, false, "");
                // }
                // else if (newSystemVariables.RollerOverLooseLimit2_2 == false &&
                //          _systemVariables.RollerOverLooseLimit2_2 == true)
                // {
                //     //夹轨/卷筒-电缆卷筒-卷筒过松限位2解除
                //     DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒过松限位2解除", GetUserName(),
                //         Machine.BucketWheel);
                //     RemoveWarningDesDict(nameof(newSystemVariables.RollerOverLooseLimit2_2));
                // }

                if (newSystemVariables.ReelEmptySwitch_2 && _systemVariables.ReelEmptySwitch_2 == false)
                {
                    //夹轨/卷筒-电缆卷筒-卷筒空盘开关
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒空盘开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("电缆卷筒-卷筒空盘开关", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ReelEmptySwitch_2), "电缆卷筒-卷筒空盘开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.ReelEmptySwitch_2 == false && _systemVariables.ReelEmptySwitch_2 == true)
                {
                    //夹轨/卷筒-电缆卷筒-卷筒空盘开关解除
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒-卷筒空盘开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.ReelEmptySwitch_2));
                }

                if (newSystemVariables.DryFogSystemLowAirPressure_2 &&
                    _systemVariables.DryFogSystemLowAirPressure_2 == false)
                {
                    //抑尘振打-洒水抑尘-干雾系统气压低
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统气压低", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("洒水抑尘-干雾系统气压低", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DryFogSystemLowAirPressure_2), "洒水抑尘-干雾系统气压低",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DryFogSystemLowAirPressure_2 == false &&
                         _systemVariables.DryFogSystemLowAirPressure_2 == true)
                {
                    //抑尘振打-洒水抑尘-干雾系统气压低解除
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统气压低解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DryFogSystemLowAirPressure_2));
                }

                if (newSystemVariables.DryFogSystemLowWaterPressure_2 &&
                    _systemVariables.DryFogSystemLowWaterPressure_2 == false)
                {
                    //抑尘振打-洒水抑尘-干雾系统水压低
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统水压低", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("洒水抑尘-干雾系统水压低", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DryFogSystemLowWaterPressure_2), "洒水抑尘-干雾系统水压低",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DryFogSystemLowWaterPressure_2 == false &&
                         _systemVariables.DryFogSystemLowWaterPressure_2 == true)
                {
                    //抑尘振打-洒水抑尘-干雾系统水压低解除
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统水压低解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DryFogSystemLowWaterPressure_2));
                }

                if (newSystemVariables.DryFogSystemFilterClogged_2 &&
                    _systemVariables.DryFogSystemFilterClogged_2 == false)
                {
                    //抑尘振打-洒水抑尘-干雾系统过滤器堵塞
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统过滤器堵塞", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("洒水抑尘-干雾系统过滤器堵塞", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DryFogSystemFilterClogged_2), "洒水抑尘-干雾系统过滤器堵塞",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DryFogSystemFilterClogged_2 == false &&
                         _systemVariables.DryFogSystemFilterClogged_2 == true)
                {
                    //抑尘振打-洒水抑尘-干雾系统过滤器堵塞解除
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-干雾系统过滤器堵塞解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DryFogSystemFilterClogged_2));
                }

                if (newSystemVariables.WaterTankLowLevelSwitch_2 && _systemVariables.WaterTankLowLevelSwitch_2 == false)
                {
                    //抑尘振打-洒水抑尘-水箱液位低开关
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-水箱液位低开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("洒水抑尘-水箱液位低开关", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.WaterTankLowLevelSwitch_2), "洒水抑尘-水箱液位低开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.WaterTankLowLevelSwitch_2 == false &&
                         _systemVariables.WaterTankLowLevelSwitch_2 == true)
                {
                    //抑尘振打-洒水抑尘-水箱液位低开关解除
                    DataManager.Instance.InsertHistoryWarningMc("洒水抑尘-水箱液位低开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.WaterTankLowLevelSwitch_2));
                }

                if (newSystemVariables.VibrationMotorOverload_2 && _systemVariables.VibrationMotorOverload_2 == false)
                {
                    //抑尘振打-振打电机-振打电机过载
                    DataManager.Instance.InsertHistoryWarningMc("振打电机-振打电机过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("振打电机-振打电机过载", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VibrationMotorOverload_2), "振打电机-振打电机过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VibrationMotorOverload_2 == false &&
                         _systemVariables.VibrationMotorOverload_2 == true)
                {
                    //抑尘振打-振打电机-振打电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("振打电机-振打电机过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VibrationMotorOverload_2));
                }

                if (newSystemVariables.ClampingDeviceMotorOverload_2 &&
                    _systemVariables.ClampingDeviceMotorOverload_2 == false)
                {
                    //夹轨/卷筒-夹轨器-电机过载
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器-电机过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("夹轨器-电机过载", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ClampingDeviceMotorOverload_2), "夹轨器-电机过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.ClampingDeviceMotorOverload_2 == false &&
                         _systemVariables.ClampingDeviceMotorOverload_2 == true)
                {
                    //夹轨/卷筒-夹轨器-电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器-电机过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.ClampingDeviceMotorOverload_2));
                }

                if (newSystemVariables.ClampFault_2 && _systemVariables.ClampFault_2 == false)
                {
                    //夹轨/卷筒-夹轨器故障
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("夹轨器故障", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ClampFault_2), "夹轨器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.ClampFault_2 == false && _systemVariables.ClampFault_2 == true)
                {
                    //夹轨/卷筒-夹轨器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.ClampFault_2));
                }

                if (newSystemVariables.TailCarDrivenRollerBearingUpperLimitAlarm_2 &&
                    _systemVariables.TailCarDrivenRollerBearingUpperLimitAlarm_2 == false)
                {
                    //尾车胶带-尾车从动滚筒轴承上限报警
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承上限报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("尾车胶带-尾车从动滚筒轴承上限报警", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarDrivenRollerBearingUpperLimitAlarm_2),
                        "尾车胶带-尾车从动滚筒轴承上限报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.TailCarDrivenRollerBearingUpperLimitAlarm_2 == false &&
                         _systemVariables.TailCarDrivenRollerBearingUpperLimitAlarm_2 == true)
                {
                    //尾车胶带-尾车从动滚筒轴承上限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承上限报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarDrivenRollerBearingUpperLimitAlarm_2));
                }

                if (newSystemVariables.TailCarDrivenRollerBearingLowerLimitAlarm_2 &&
                    _systemVariables.TailCarDrivenRollerBearingLowerLimitAlarm_2 == false)
                {
                    //尾车胶带-尾车从动滚筒轴承下限报警
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承下限报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("尾车胶带-尾车从动滚筒轴承下限报警", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarDrivenRollerBearingLowerLimitAlarm_2),
                        "尾车胶带-尾车从动滚筒轴承下限报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.TailCarDrivenRollerBearingLowerLimitAlarm_2 == false &&
                         _systemVariables.TailCarDrivenRollerBearingLowerLimitAlarm_2 == true)
                {
                    //尾车胶带-尾车从动滚筒轴承下限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车从动滚筒轴承下限报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarDrivenRollerBearingLowerLimitAlarm_2));
                }

                if (newSystemVariables.TailCarFirstLevelDeviationSwitch_2 &&
                    _systemVariables.TailCarFirstLevelDeviationSwitch_2 == false)
                {
                    //尾车胶带-尾车一级跑偏开关
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车一级跑偏开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("尾车胶带-尾车一级跑偏开关", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarFirstLevelDeviationSwitch_2),
                        "尾车胶带-尾车一级跑偏开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.TailCarFirstLevelDeviationSwitch_2 == false &&
                         _systemVariables.TailCarFirstLevelDeviationSwitch_2 == true)
                {
                    //尾车胶带-尾车一级跑偏开关解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车一级跑偏开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarFirstLevelDeviationSwitch_2));
                }

                if (newSystemVariables.TailCarSecondLevelDeviationSwitch_2 &&
                    _systemVariables.TailCarSecondLevelDeviationSwitch_2 == false)
                {
                    //尾车胶带-尾车二级跑偏开关
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车二级跑偏开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("尾车胶带-尾车二级跑偏开关", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarSecondLevelDeviationSwitch_2),
                        "尾车胶带-尾车二级跑偏开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.TailCarSecondLevelDeviationSwitch_2 == false &&
                         _systemVariables.TailCarSecondLevelDeviationSwitch_2 == true)
                {
                    //尾车胶带-尾车二级跑偏开关解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车二级跑偏开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarSecondLevelDeviationSwitch_2));
                }

                if (newSystemVariables.TailCarEmergencyStopSwitch_2 &&
                    _systemVariables.TailCarEmergencyStopSwitch_2 == false)
                {
                    //尾车胶带-尾车急停拉线开关
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车急停拉线开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("尾车胶带-尾车急停拉线开关", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarEmergencyStopSwitch_2), "尾车胶带-尾车急停拉线开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.TailCarEmergencyStopSwitch_2 == false &&
                         _systemVariables.TailCarEmergencyStopSwitch_2 == true)
                {
                    //尾车胶带-尾车急停拉线开关解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车急停拉线开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarEmergencyStopSwitch_2));
                }

                if (newSystemVariables.TailCarBeltLongitudinalTearing_2 &&
                    _systemVariables.TailCarBeltLongitudinalTearing_2 == false)
                {
                    //尾车胶带-尾车胶带纵向撕裂
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车胶带纵向撕裂", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesQueue("尾车胶带-尾车胶带纵向撕裂", Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarBeltLongitudinalTearing_2),
                        "尾车胶带-尾车胶带纵向撕裂",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.TailCarBeltLongitudinalTearing_2 == false &&
                         _systemVariables.TailCarBeltLongitudinalTearing_2 == true)
                {
                    //尾车胶带-尾车胶带纵向撕裂解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带-尾车胶带纵向撕裂解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarBeltLongitudinalTearing_2));
                }
                //333333333333333333

                if (newSystemVariables.OverBelt_R_Limit_2 &&
                    _systemVariables.OverBelt_R_Limit_2 == false)
                {
                    //回转右转过皮带保护限位
                    DataManager.Instance.InsertHistoryWarningMc("回转右转过皮带保护限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.OverBelt_R_Limit_2), "回转右转过皮带保护限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.OverBelt_R_Limit_2 == false &&
                         _systemVariables.OverBelt_R_Limit_2 == true)
                {
                    //回转右转过皮带保护限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转右转过皮带保护限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.OverBelt_R_Limit_2));
                }

                if (newSystemVariables.OverBelt_L_Limit_2 &&
                    _systemVariables.OverBelt_L_Limit_2 == false)
                {
                    //回转左转过皮带保护限位
                    DataManager.Instance.InsertHistoryWarningMc("回转左转过皮带保护限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.OverBelt_L_Limit_2), "回转左转过皮带保护限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.OverBelt_L_Limit_2 == false &&
                         _systemVariables.OverBelt_L_Limit_2 == true)
                {
                    //回转左转过皮带保护限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转左转过皮带保护限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.OverBelt_L_Limit_2));
                }

                if (newSystemVariables.OverBelt_D_Limit_2 &&
                    _systemVariables.OverBelt_D_Limit_2 == false)
                {
                    //回转过皮带俯仰下限位保护
                    DataManager.Instance.InsertHistoryWarningMc("回转过皮带俯仰下限位保护", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.OverBelt_D_Limit_2), "回转过皮带俯仰下限位保护",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.OverBelt_D_Limit_2 == false &&
                         _systemVariables.OverBelt_D_Limit_2 == true)
                {
                    //回转过皮带俯仰下限位保护解除
                    DataManager.Instance.InsertHistoryWarningMc("回转过皮带俯仰下限位保护解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.OverBelt_D_Limit_2));
                }

                if (newSystemVariables.OverBelt_R_SoftLimit_2 &&
                    _systemVariables.OverBelt_R_SoftLimit_2 == false)
                {
                    //回转右转过皮带保护软限位
                    DataManager.Instance.InsertHistoryWarningMc("回转右转过皮带保护软限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.OverBelt_R_SoftLimit_2), "回转右转过皮带保护软限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.OverBelt_R_SoftLimit_2 == false &&
                         _systemVariables.OverBelt_R_SoftLimit_2 == true)
                {
                    //回转右转过皮带保护软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转右转过皮带保护软限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.OverBelt_R_SoftLimit_2));
                }

                if (newSystemVariables.OverBelt_L_SoftLimit_2 &&
                    _systemVariables.OverBelt_L_SoftLimit_2 == false)
                {
                    //回转左转过皮带保护软限位
                    DataManager.Instance.InsertHistoryWarningMc("回转左转过皮带保护软限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.OverBelt_L_SoftLimit_2), "回转左转过皮带保护软限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.OverBelt_L_SoftLimit_2 == false &&
                         _systemVariables.OverBelt_L_SoftLimit_2 == true)
                {
                    //回转左转过皮带保护软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转左转过皮带保护软限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.OverBelt_L_SoftLimit_2));
                }

                if (newSystemVariables.OverBelt_D_SoftLimit_2 &&
                    _systemVariables.OverBelt_D_SoftLimit_2 == false)
                {
                    //回转过皮带俯仰下限软限位
                    DataManager.Instance.InsertHistoryWarningMc("回转过皮带俯仰下限软限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.OverBelt_D_SoftLimit_2), "回转过皮带俯仰下限软限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.OverBelt_D_SoftLimit_2 == false &&
                         _systemVariables.OverBelt_D_SoftLimit_2 == true)
                {
                    //回转过皮带俯仰下限软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("回转过皮带俯仰下限软限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.OverBelt_D_SoftLimit_2));
                }

                if (newSystemVariables.DC_FWD_SoftLimit_2 &&
                    _systemVariables.DC_FWD_SoftLimit_2 == false)
                {
                    //大车前进停止软限位
                    DataManager.Instance.InsertHistoryWarningMc("大车前进停止软限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_FWD_SoftLimit_2), "大车前进停止软限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DC_FWD_SoftLimit_2 == false &&
                         _systemVariables.DC_FWD_SoftLimit_2 == true)
                {
                    //大车前进停止软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("大车前进停止软限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_FWD_SoftLimit_2));
                }

                if (newSystemVariables.DcFWD_LimitStatus_2 &&
                    _systemVariables.DcFWD_LimitStatus_2 == false)
                {
                    //大车前进限位集合
                    DataManager.Instance.InsertHistoryWarningMc("大车前进限位集合", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DcFWD_LimitStatus_2), "大车前进限位集合",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DcFWD_LimitStatus_2 == false &&
                         _systemVariables.DcFWD_LimitStatus_2 == true)
                {
                    //大车前进限位集合解除
                    DataManager.Instance.InsertHistoryWarningMc("大车前进限位集合解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DcFWD_LimitStatus_2));
                }

                if (newSystemVariables.DC_REV_SoftLimit_2 &&
                    _systemVariables.DC_REV_SoftLimit_2 == false)
                {
                    //大车后退停止软限位
                    DataManager.Instance.InsertHistoryWarningMc("大车后退停止软限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_REV_SoftLimit_2), "大车后退停止软限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DC_REV_SoftLimit_2 == false &&
                         _systemVariables.DC_REV_SoftLimit_2 == true)
                {
                    //大车后退停止软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("大车后退停止软限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_REV_SoftLimit_2));
                }

                if (newSystemVariables.DcREV_LimitStatus_2 &&
                    _systemVariables.DcREV_LimitStatus_2 == false)
                {
                    //大车后退限位集合
                    DataManager.Instance.InsertHistoryWarningMc("大车后退限位集合", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DcREV_LimitStatus_2), "大车后退限位集合",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DcREV_LimitStatus_2 == false &&
                         _systemVariables.DcREV_LimitStatus_2 == true)
                {
                    //大车后退限位集合解除
                    DataManager.Instance.InsertHistoryWarningMc("大车后退限位集合解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DcREV_LimitStatus_2));
                }

                if (newSystemVariables.Slew_R_SoftLimit_2 &&
                    _systemVariables.Slew_R_SoftLimit_2 == false)
                {
                    //悬臂右转软限位
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右转软限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_R_SoftLimit_2), "悬臂右转软限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Slew_R_SoftLimit_2 == false &&
                         _systemVariables.Slew_R_SoftLimit_2 == true)
                {
                    //悬臂右转软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右转软限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_R_SoftLimit_2));
                }

                if (newSystemVariables.Slew_R_LimitStatus_2 &&
                    _systemVariables.Slew_R_LimitStatus_2 == false)
                {
                    //悬臂右转限位集合
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右转限位集合", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_R_LimitStatus_2), "悬臂右转限位集合",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Slew_R_LimitStatus_2 == false &&
                         _systemVariables.Slew_R_LimitStatus_2 == true)
                {
                    //悬臂右转限位集合解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右转限位集合解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_R_LimitStatus_2));
                }

                if (newSystemVariables.Slew_L_SoftLimit_2 &&
                    _systemVariables.Slew_L_SoftLimit_2 == false)
                {
                    //悬臂左转软限位
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左转软限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_L_SoftLimit_2), "悬臂左转软限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Slew_L_SoftLimit_2 == false &&
                         _systemVariables.Slew_L_SoftLimit_2 == true)
                {
                    //悬臂左转软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左转软限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_L_SoftLimit_2));
                }

                if (newSystemVariables.Slew_L_LimitStatus_2 &&
                    _systemVariables.Slew_L_LimitStatus_2 == false)
                {
                    //悬臂左转限位集合
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左转限位集合", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_L_LimitStatus_2), "悬臂左转限位集合",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Slew_L_LimitStatus_2 == false &&
                         _systemVariables.Slew_L_LimitStatus_2 == true)
                {
                    //悬臂左转限位集合解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左转限位集合解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_L_LimitStatus_2));
                }

                if (newSystemVariables.Luff_Up_SoftLimit_2 &&
                    _systemVariables.Luff_Up_SoftLimit_2 == false)
                {
                    //俯仰上极限软限位
                    DataManager.Instance.InsertHistoryWarningMc("俯仰上极限软限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Luff_Up_SoftLimit_2), "俯仰上极限软限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Luff_Up_SoftLimit_2 == false &&
                         _systemVariables.Luff_Up_SoftLimit_2 == true)
                {
                    //俯仰上极限软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("俯仰上极限软限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Luff_Up_SoftLimit_2));
                }

                if (newSystemVariables.LuffUp_LimitStatus_2 &&
                    _systemVariables.LuffUp_LimitStatus_2 == false)
                {
                    //俯仰上极限限位集合
                    DataManager.Instance.InsertHistoryWarningMc("俯仰上极限限位集合", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LuffUp_LimitStatus_2), "俯仰上极限限位集合",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LuffUp_LimitStatus_2 == false &&
                         _systemVariables.LuffUp_LimitStatus_2 == true)
                {
                    //俯仰上极限限位集合解除
                    DataManager.Instance.InsertHistoryWarningMc("俯仰上极限限位集合解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LuffUp_LimitStatus_2));
                }

                if (newSystemVariables.Luff_Down_SoftLimit_2 &&
                    _systemVariables.Luff_Down_SoftLimit_2 == false)
                {
                    //俯仰下极限软限位
                    DataManager.Instance.InsertHistoryWarningMc("俯仰下极限软限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Luff_Down_SoftLimit_2), "俯仰下极限软限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Luff_Down_SoftLimit_2 == false &&
                         _systemVariables.Luff_Down_SoftLimit_2 == true)
                {
                    //俯仰下极限软限位解除
                    DataManager.Instance.InsertHistoryWarningMc("俯仰下极限软限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Luff_Down_SoftLimit_2));
                }

                if (newSystemVariables.LuffDown_LimitStatus_2 &&
                    _systemVariables.LuffDown_LimitStatus_2 == false)
                {
                    //俯仰下极限限位集合
                    DataManager.Instance.InsertHistoryWarningMc("俯仰下极限限位集合", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LuffDown_LimitStatus_2), "俯仰下极限限位集合",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LuffDown_LimitStatus_2 == false &&
                         _systemVariables.LuffDown_LimitStatus_2 == true)
                {
                    //俯仰下极限限位集合解除
                    DataManager.Instance.InsertHistoryWarningMc("俯仰下极限限位集合解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LuffDown_LimitStatus_2));
                }

                if (newSystemVariables.Slew_SAS_L_Alarm_2 &&
                    _systemVariables.Slew_SAS_L_Alarm_2 == false)
                {
                    //悬臂左侧防撞保护动作
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左侧防撞保护动作", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_SAS_L_Alarm_2), "悬臂左侧防撞保护动作",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Slew_SAS_L_Alarm_2 == false &&
                         _systemVariables.Slew_SAS_L_Alarm_2 == true)
                {
                    //悬臂左侧防撞保护动作解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左侧防撞保护动作解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_SAS_L_Alarm_2));
                }

                if (newSystemVariables.Slew_SAS_R_Alarm_2 &&
                    _systemVariables.Slew_SAS_R_Alarm_2 == false)
                {
                    //悬臂右侧防撞保护动作
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右侧防撞保护动作", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_SAS_R_Alarm_2), "悬臂右侧防撞保护动作",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Slew_SAS_R_Alarm_2 == false &&
                         _systemVariables.Slew_SAS_R_Alarm_2 == true)
                {
                    //悬臂右侧防撞保护动作解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右侧防撞保护动作解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_SAS_R_Alarm_2));
                }

                if (newSystemVariables.DC_SAS_F_Alarm_2 &&
                    _systemVariables.DC_SAS_F_Alarm_2 == false)
                {
                    //大车前进防撞保护动作
                    DataManager.Instance.InsertHistoryWarningMc("大车前进防撞保护动作", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_SAS_F_Alarm_2), "大车前进防撞保护动作",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DC_SAS_F_Alarm_2 == false &&
                         _systemVariables.DC_SAS_F_Alarm_2 == true)
                {
                    //大车前进防撞保护动作解除
                    DataManager.Instance.InsertHistoryWarningMc("大车前进防撞保护动作解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_SAS_F_Alarm_2));
                }

                if (newSystemVariables.DC_SAS_B_Alarm_2 &&
                    _systemVariables.DC_SAS_B_Alarm_2 == false)
                {
                    //大车后退防撞保护动作
                    DataManager.Instance.InsertHistoryWarningMc("大车后退防撞保护动作", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_SAS_B_Alarm_2), "大车后退防撞保护动作",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DC_SAS_B_Alarm_2 == false &&
                         _systemVariables.DC_SAS_B_Alarm_2 == true)
                {
                    //大车后退防撞保护动作解除
                    DataManager.Instance.InsertHistoryWarningMc("大车后退防撞保护动作解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_SAS_B_Alarm_2));
                }

                if (newSystemVariables.Slew_SAS_RR_Alarm_2 &&
                    _systemVariables.Slew_SAS_RR_Alarm_2 == false)
                {
                    //悬臂右雷达前方有煤垛碰撞警告
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右雷达前方有煤垛碰撞警告", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_SAS_RR_Alarm_2), "悬臂右雷达前方有煤垛碰撞警告",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Slew_SAS_RR_Alarm_2 == false &&
                         _systemVariables.Slew_SAS_RR_Alarm_2 == true)
                {
                    //悬臂右雷达前方有煤垛碰撞警告解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右雷达前方有煤垛碰撞警告解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_SAS_RR_Alarm_2));
                }

                if (newSystemVariables.Slew_SAS_LR_Alarm_2 &&
                    _systemVariables.Slew_SAS_LR_Alarm_2 == false)
                {
                    //悬臂左雷达前方有煤垛碰撞警告
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左雷达前方有煤垛碰撞警告", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_SAS_LR_Alarm_2), "悬臂左雷达前方有煤垛碰撞警告",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Slew_SAS_LR_Alarm_2 == false &&
                         _systemVariables.Slew_SAS_LR_Alarm_2 == true)
                {
                    //悬臂左雷达前方有煤垛碰撞警告解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左雷达前方有煤垛碰撞警告解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_SAS_LR_Alarm_2));
                }

                if (newSystemVariables.Slew_SAS_RU_Alarm_2 &&
                    _systemVariables.Slew_SAS_RU_Alarm_2 == false)
                {
                    //悬臂右超声波前方有煤垛碰撞警告
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右超声波前方有煤垛碰撞警告", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_SAS_RU_Alarm_2), "悬臂右超声波前方有煤垛碰撞警告",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Slew_SAS_RU_Alarm_2 == false &&
                         _systemVariables.Slew_SAS_RU_Alarm_2 == true)
                {
                    //悬臂右超声波前方有煤垛碰撞警告解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右超声波前方有煤垛碰撞警告解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_SAS_RU_Alarm_2));
                }

                if (newSystemVariables.Slew_SAS_LU_Alarm_2 &&
                    _systemVariables.Slew_SAS_LU_Alarm_2 == false)
                {
                    //悬臂左超声波前方有煤垛碰撞警告
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左超声波前方有煤垛碰撞警告", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_SAS_LU_Alarm_2), "悬臂左超声波前方有煤垛碰撞警告",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Slew_SAS_LU_Alarm_2 == false &&
                         _systemVariables.Slew_SAS_LU_Alarm_2 == true)
                {
                    //悬臂左超声波前方有煤垛碰撞警告解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左超声波前方有煤垛碰撞警告解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_SAS_LU_Alarm_2));
                }

                if (newSystemVariables.DC_SAS_RF_Alrm_2 &&
                    _systemVariables.DC_SAS_RF_Alrm_2 == false)
                {
                    //大车右前方有障碍
                    DataManager.Instance.InsertHistoryWarningMc("大车右前方有障碍", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_SAS_RF_Alrm_2), "大车右前方有障碍",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DC_SAS_RF_Alrm_2 == false &&
                         _systemVariables.DC_SAS_RF_Alrm_2 == true)
                {
                    //大车右前方有障碍解除
                    DataManager.Instance.InsertHistoryWarningMc("大车右前方有障碍解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_SAS_RF_Alrm_2));
                }

                if (newSystemVariables.DC_SAS_RB_Alrm_2 &&
                    _systemVariables.DC_SAS_RB_Alrm_2 == false)
                {
                    //大车右后方有障碍
                    DataManager.Instance.InsertHistoryWarningMc("大车右后方有障碍", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_SAS_RB_Alrm_2), "大车右后方有障碍",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DC_SAS_RB_Alrm_2 == false &&
                         _systemVariables.DC_SAS_RB_Alrm_2 == true)
                {
                    //大车右后方有障碍解除
                    DataManager.Instance.InsertHistoryWarningMc("大车右后方有障碍解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_SAS_RB_Alrm_2));
                }

                if (newSystemVariables.DC_SAS_LF_Alrm_2 &&
                    _systemVariables.DC_SAS_LF_Alrm_2 == false)
                {
                    //大车左前方有障碍
                    DataManager.Instance.InsertHistoryWarningMc("大车左前方有障碍", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_SAS_LF_Alrm_2), "大车左前方有障碍",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DC_SAS_LF_Alrm_2 == false &&
                         _systemVariables.DC_SAS_LF_Alrm_2 == true)
                {
                    //大车左前方有障碍解除
                    DataManager.Instance.InsertHistoryWarningMc("大车左前方有障碍解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_SAS_LF_Alrm_2));
                }

                if (newSystemVariables.DC_SAS_LB_Alrm_2 &&
                    _systemVariables.DC_SAS_LB_Alrm_2 == false)
                {
                    //大车左后方有障碍
                    DataManager.Instance.InsertHistoryWarningMc("大车左后方有障碍", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_SAS_LB_Alrm_2), "大车左后方有障碍",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DC_SAS_LB_Alrm_2 == false &&
                         _systemVariables.DC_SAS_LB_Alrm_2 == true)
                {
                    //大车左后方有障碍解除
                    DataManager.Instance.InsertHistoryWarningMc("大车左后方有障碍解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_SAS_LB_Alrm_2));
                }

                if (newSystemVariables.FWD_Limit_Waring_2 &&
                    _systemVariables.FWD_Limit_Waring_2 == false)
                {
                    //大车前进限位两米预警
                    DataManager.Instance.InsertHistoryWarningMc("大车前进限位两米预警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.FWD_Limit_Waring_2), "大车前进限位两米预警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.FWD_Limit_Waring_2 == false &&
                         _systemVariables.FWD_Limit_Waring_2 == true)
                {
                    //大车前进限位两米预警解除
                    DataManager.Instance.InsertHistoryWarningMc("大车前进限位两米预警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.FWD_Limit_Waring_2));
                }

                if (newSystemVariables.REV_Limit_Waring_2 &&
                    _systemVariables.REV_Limit_Waring_2 == false)
                {
                    //大车后退限位两米预警
                    DataManager.Instance.InsertHistoryWarningMc("大车后退限位两米预警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.REV_Limit_Waring_2), "大车后退限位两米预警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.REV_Limit_Waring_2 == false &&
                         _systemVariables.REV_Limit_Waring_2 == true)
                {
                    //大车后退限位两米预警解除
                    DataManager.Instance.InsertHistoryWarningMc("大车后退限位两米预警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.REV_Limit_Waring_2));
                }

                if (newSystemVariables.Slew_R_Limit_Waring_2 &&
                    _systemVariables.Slew_R_Limit_Waring_2 == false)
                {
                    //回转右转限位两度预警
                    DataManager.Instance.InsertHistoryWarningMc("回转右转限位两度预警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_R_Limit_Waring_2), "回转右转限位两度预警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Slew_R_Limit_Waring_2 == false &&
                         _systemVariables.Slew_R_Limit_Waring_2 == true)
                {
                    //回转右转限位两度预警解除
                    DataManager.Instance.InsertHistoryWarningMc("回转右转限位两度预警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_R_Limit_Waring_2));
                }

                if (newSystemVariables.Slew_L_Limit_Waring_2 &&
                    _systemVariables.Slew_L_Limit_Waring_2 == false)
                {
                    //回转左转限位两度预警
                    DataManager.Instance.InsertHistoryWarningMc("回转左转限位两度预警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_L_Limit_Waring_2), "回转左转限位两度预警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Slew_L_Limit_Waring_2 == false &&
                         _systemVariables.Slew_L_Limit_Waring_2 == true)
                {
                    //回转左转限位两度预警解除
                    DataManager.Instance.InsertHistoryWarningMc("回转左转限位两度预警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_L_Limit_Waring_2));
                }

                if (newSystemVariables.Luff_U_Limit_Waring_2 &&
                    _systemVariables.Luff_U_Limit_Waring_2 == false)
                {
                    //俯仰上仰限位两度预警
                    DataManager.Instance.InsertHistoryWarningMc("俯仰上仰限位两度预警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Luff_U_Limit_Waring_2), "俯仰上仰限位两度预警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Luff_U_Limit_Waring_2 == false &&
                         _systemVariables.Luff_U_Limit_Waring_2 == true)
                {
                    //俯仰上仰限位两度预警解除
                    DataManager.Instance.InsertHistoryWarningMc("俯仰上仰限位两度预警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Luff_U_Limit_Waring_2));
                }

                if (newSystemVariables.Luff_D_Limit_Waring_2 &&
                    _systemVariables.Luff_D_Limit_Waring_2 == false)
                {
                    //俯仰下俯限位两度预警
                    DataManager.Instance.InsertHistoryWarningMc("俯仰下俯限位两度预警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Luff_D_Limit_Waring_2), "俯仰下俯限位两度预警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Luff_D_Limit_Waring_2 == false &&
                         _systemVariables.Luff_D_Limit_Waring_2 == true)
                {
                    //俯仰下俯限位两度预警解除
                    DataManager.Instance.InsertHistoryWarningMc("俯仰下俯限位两度预警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Luff_D_Limit_Waring_2));
                }

                if (newSystemVariables.DC_Encoder_ERR_2 == true && _systemVariables.DC_Encoder_ERR_2 == false)
                {
                    // 行走编码器异常
                    DataManager.Instance.InsertHistoryWarningMc("行走编码器异常", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DC_Encoder_ERR_2), "行走编码器异常",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DC_Encoder_ERR_2 == false && _systemVariables.DC_Encoder_ERR_2 == true)
                {
                    // 行走编码器异常解除
                    DataManager.Instance.InsertHistoryWarningMc("行走编码器异常解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DC_Encoder_ERR_2));
                }

                if (newSystemVariables.Slew_Encoder_ERR_2 == true && _systemVariables.Slew_Encoder_ERR_2 == false)
                {
                    // 回转编码器异常
                    DataManager.Instance.InsertHistoryWarningMc("回转编码器异常", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_Encoder_ERR_2), "回转编码器异常",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Slew_Encoder_ERR_2 == false && _systemVariables.Slew_Encoder_ERR_2 == true)
                {
                    // 回转编码器异常解除
                    DataManager.Instance.InsertHistoryWarningMc("回转编码器异常解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_Encoder_ERR_2));
                }

                if (newSystemVariables.QJY_CH_FAULT_2 == true && _systemVariables.QJY_CH_FAULT_2 == false)
                {
                    // 倾角仪异常
                    DataManager.Instance.InsertHistoryWarningMc("倾角仪异常", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.QJY_CH_FAULT_2), "倾角仪异常",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.QJY_CH_FAULT_2 == false && _systemVariables.QJY_CH_FAULT_2 == true)
                {
                    //倾角仪异常解除
                    DataManager.Instance.InsertHistoryWarningMc("倾角仪异常解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.QJY_CH_FAULT_2));
                }

                if (newSystemVariables.XBTB_LWJ_CH_FAULT_2 == true && _systemVariables.XBTB_LWJ_CH_FAULT_2 == false)
                {
                    // 垂直料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("垂直料位计异常", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.XBTB_LWJ_CH_FAULT_2), "垂直料位计异常",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.XBTB_LWJ_CH_FAULT_2 == false &&
                         _systemVariables.XBTB_LWJ_CH_FAULT_2 == true)
                {
                    // 垂直料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("垂直料位计异常解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.XBTB_LWJ_CH_FAULT_2));
                }

                if (newSystemVariables.DCZQ_FZ_CH_FAULT_2 == true && _systemVariables.DCZQ_FZ_CH_FAULT_2 == false)
                {
                    // 大车左前料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("大车左前料位计异常", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DCZQ_FZ_CH_FAULT_2), "大车左前料位计异常",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DCZQ_FZ_CH_FAULT_2 == false && _systemVariables.DCZQ_FZ_CH_FAULT_2 == true)
                {
                    // 大车左前料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("大车左前料位计异常解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DCZQ_FZ_CH_FAULT_2));
                }

                if (newSystemVariables.DCZH_FZ_CH_FAULT_2 == true && _systemVariables.DCZH_FZ_CH_FAULT_2 == false)
                {
                    // 大车左后料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("大车左后料位计异常", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DCZH_FZ_CH_FAULT_2), "大车左后料位计异常",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DCZH_FZ_CH_FAULT_2 == false && _systemVariables.DCZH_FZ_CH_FAULT_2 == true)
                {
                    // 大车左后料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("大车左后料位计异常解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DCZH_FZ_CH_FAULT_2));
                }

                if (newSystemVariables.DCYQ_FZ_CH_FAULT_2 == true && _systemVariables.DCYQ_FZ_CH_FAULT_2 == false)
                {
                    // 大车右前料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("大车右前料位计异常", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DCYQ_FZ_CH_FAULT_2), "大车右前料位计异常",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DCYQ_FZ_CH_FAULT_2 == false && _systemVariables.DCYQ_FZ_CH_FAULT_2 == true)
                {
                    // 大车右前料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("大车右前料位计异常解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DCYQ_FZ_CH_FAULT_2));
                }

                if (newSystemVariables.DCYH_FZ_CH_FAULT_2 == true && _systemVariables.DCYH_FZ_CH_FAULT_2 == false)
                {
                    // 大车右后料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("大车右后料位计异常", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DCYH_FZ_CH_FAULT_2), "大车右后料位计异常",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DCYH_FZ_CH_FAULT_2 == false && _systemVariables.DCYH_FZ_CH_FAULT_2 == true)
                {
                    // 大车右后料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("大车右后料位计异常解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DCYH_FZ_CH_FAULT_2));
                }

                if (newSystemVariables.XBZQ_FZ_CH_FAULT_2 == true && _systemVariables.XBZQ_FZ_CH_FAULT_2 == false)
                {
                    // 悬臂左前料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左前料位计异常", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.XBZQ_FZ_CH_FAULT_2), "悬臂左前料位计异常",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.XBZQ_FZ_CH_FAULT_2 == false && _systemVariables.XBZQ_FZ_CH_FAULT_2 == true)
                {
                    // 悬臂左前料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左前料位计异常解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.XBZQ_FZ_CH_FAULT_2));
                }

                if (newSystemVariables.XBZZ_FZ_CH_FAULT_2 == true && _systemVariables.XBZZ_FZ_CH_FAULT_2 == false)
                {
                    // 悬臂左中料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左中料位计异常", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.XBZZ_FZ_CH_FAULT_2), "悬臂左中料位计异常",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.XBZZ_FZ_CH_FAULT_2 == false && _systemVariables.XBZZ_FZ_CH_FAULT_2 == true)
                {
                    // 悬臂左中料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左中料位计异常解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.XBZZ_FZ_CH_FAULT_2));
                }

                if (newSystemVariables.XBZH_FZ_CH_FAULT_2 == true && _systemVariables.XBZH_FZ_CH_FAULT_2 == false)
                {
                    // 悬臂左后料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左后料位计异常", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.XBZH_FZ_CH_FAULT_2), "悬臂左后料位计异常",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.XBZH_FZ_CH_FAULT_2 == false && _systemVariables.XBZH_FZ_CH_FAULT_2 == true)
                {
                    // 悬臂左后料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂左后料位计异常解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.XBZH_FZ_CH_FAULT_2));
                }

                if (newSystemVariables.XBYQ_FZ_CH_FAULT_2 == true && _systemVariables.XBYQ_FZ_CH_FAULT_2 == false)
                {
                    // 悬臂右前料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右前料位计异常", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.XBYQ_FZ_CH_FAULT_2), "悬臂右前料位计异常",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.XBYQ_FZ_CH_FAULT_2 == false && _systemVariables.XBYQ_FZ_CH_FAULT_2 == true)
                {
                    // 悬臂右前料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右前料位计异常解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.XBYQ_FZ_CH_FAULT_2));
                }

                if (newSystemVariables.XBYZ_FZ_CH_FAULT_2 == true && _systemVariables.XBYZ_FZ_CH_FAULT_2 == false)
                {
                    // 悬臂右中料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右中料位计异常", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.XBYZ_FZ_CH_FAULT_2), "悬臂右中料位计异常",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.XBYZ_FZ_CH_FAULT_2 == false && _systemVariables.XBYZ_FZ_CH_FAULT_2 == true)
                {
                    // 悬臂右中料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右中料位计异常解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.XBYZ_FZ_CH_FAULT_2));
                }

                if (newSystemVariables.XBYH_FZ_CH_FAULT_2 == true && _systemVariables.XBYH_FZ_CH_FAULT_2 == false)
                {
                    // 悬臂右后料位计异常
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右后料位计异常", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.XBYH_FZ_CH_FAULT_2), "悬臂右后料位计异常",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.XBYH_FZ_CH_FAULT_2 == false && _systemVariables.XBYH_FZ_CH_FAULT_2 == true)
                {
                    // 悬臂右后料位计异常解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂右后料位计异常解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.XBYH_FZ_CH_FAULT_2));
                }

                if (newSystemVariables.RightAnchorNotLifted_2 == true &&
                    _systemVariables.RightAnchorNotLifted_2 == false)
                {
                    // 右侧锚锭没有抬起
                    DataManager.Instance.InsertHistoryWarningMc("右侧锚锭没有抬起", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RightAnchorNotLifted_2), "右侧锚锭没有抬起",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RightAnchorNotLifted_2 == false &&
                         _systemVariables.RightAnchorNotLifted_2 == true)
                {
                    // 右侧锚锭没有抬起解除
                    DataManager.Instance.InsertHistoryWarningMc("右侧锚锭没有抬起解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RightAnchorNotLifted_2));
                }

                if (newSystemVariables.ClampNotRelaxed_2 == true && _systemVariables.ClampNotRelaxed_2 == false)
                {
                    // 夹轨器没有放松
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器没有放松", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ClampNotRelaxed_2), "夹轨器没有放松",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.ClampNotRelaxed_2 == false && _systemVariables.ClampNotRelaxed_2 == true)
                {
                    // 夹轨器没有放松解除
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器没有放松解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.ClampNotRelaxed_2));
                }

                if (newSystemVariables.LargeCarBrakeNotOpen_2 == true &&
                    _systemVariables.LargeCarBrakeNotOpen_2 == false)
                {
                    // 大车制动器没有打开
                    DataManager.Instance.InsertHistoryWarningMc("大车制动器没有打开", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarBrakeNotOpen_2), "大车制动器没有打开",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarBrakeNotOpen_2 == false &&
                         _systemVariables.LargeCarBrakeNotOpen_2 == true)
                {
                    // 大车制动器没有打开解除
                    DataManager.Instance.InsertHistoryWarningMc("大车制动器没有打开解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarBrakeNotOpen_2));
                }

                if (newSystemVariables.LargeCarFrequencyConverterNotPowered_2 == true &&
                    _systemVariables.LargeCarFrequencyConverterNotPowered_2 == false)
                {
                    // 大车变频器没有投入
                    DataManager.Instance.InsertHistoryWarningMc("大车变频器没有投入", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarFrequencyConverterNotPowered_2),
                        "大车变频器没有投入",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarFrequencyConverterNotPowered_2 == false &&
                         _systemVariables.LargeCarFrequencyConverterNotPowered_2 == true)
                {
                    // 大车变频器没有投入解除
                    DataManager.Instance.InsertHistoryWarningMc("大车变频器没有投入解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarFrequencyConverterNotPowered_2));
                }

                if (newSystemVariables.LargeCarBrakeContactAuxiliaryFault_2 == true &&
                    _systemVariables.LargeCarBrakeContactAuxiliaryFault_2 == false)
                {
                    // 大车制动器接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("大车制动器接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarBrakeContactAuxiliaryFault_2),
                        "大车制动器接触器辅助触点故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarBrakeContactAuxiliaryFault_2 == false &&
                         _systemVariables.LargeCarBrakeContactAuxiliaryFault_2 == true)
                {
                    // 大车制动器接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车制动器接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarBrakeContactAuxiliaryFault_2));
                }

                if (newSystemVariables.LargeCarFrequencyConverterContactAuxiliaryFault_2 == true &&
                    _systemVariables.LargeCarFrequencyConverterContactAuxiliaryFault_2 == false)
                {
                    // 大车变频器接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("大车变频器接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.LargeCarFrequencyConverterContactAuxiliaryFault_2),
                        "大车变频器接触器辅助触点故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarFrequencyConverterContactAuxiliaryFault_2 == false &&
                         _systemVariables.LargeCarFrequencyConverterContactAuxiliaryFault_2 == true)
                {
                    // 大车变频器接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车变频器接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarFrequencyConverterContactAuxiliaryFault_2));
                }

                if (newSystemVariables.RotaryFrequencyConverterNotPowered_2 == true &&
                    _systemVariables.RotaryFrequencyConverterNotPowered_2 == false)
                {
                    // 回转变频器没有投入
                    DataManager.Instance.InsertHistoryWarningMc("回转变频器没有投入", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryFrequencyConverterNotPowered_2),
                        "回转变频器没有投入",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryFrequencyConverterNotPowered_2 == false &&
                         _systemVariables.RotaryFrequencyConverterNotPowered_2 == true)
                {
                    // 回转变频器没有投入解除
                    DataManager.Instance.InsertHistoryWarningMc("回转变频器没有投入解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryFrequencyConverterNotPowered_2));
                }

                if (newSystemVariables.RotaryFrequencyConverterContactAuxiliaryFault_2 == true &&
                    _systemVariables.RotaryFrequencyConverterContactAuxiliaryFault_2 == false)
                {
                    // 回转变频器接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("回转变频器接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.RotaryFrequencyConverterContactAuxiliaryFault_2),
                        "回转变频器接触器辅助触点故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryFrequencyConverterContactAuxiliaryFault_2 == false &&
                         _systemVariables.RotaryFrequencyConverterContactAuxiliaryFault_2 == true)
                {
                    // 回转变频器接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转变频器接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryFrequencyConverterContactAuxiliaryFault_2));
                }

                if (newSystemVariables.RotaryBrakeContactAuxiliaryFault_2 == true &&
                    _systemVariables.RotaryBrakeContactAuxiliaryFault_2 == false)
                {
                    // 回转制动器接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("回转制动器接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryBrakeContactAuxiliaryFault_2),
                        "回转制动器接触器辅助触点故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryBrakeContactAuxiliaryFault_2 == false &&
                         _systemVariables.RotaryBrakeContactAuxiliaryFault_2 == true)
                {
                    // 回转制动器接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转制动器接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryBrakeContactAuxiliaryFault_2));
                }

                if (newSystemVariables.SuspensionBeltBrakeContactAuxiliaryFault_2 == true &&
                    _systemVariables.SuspensionBeltBrakeContactAuxiliaryFault_2 == false)
                {
                    // 悬臂胶带制动器接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带制动器接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltBrakeContactAuxiliaryFault_2),
                        "悬臂胶带制动器接触器辅助触点故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspensionBeltBrakeContactAuxiliaryFault_2 == false &&
                         _systemVariables.SuspensionBeltBrakeContactAuxiliaryFault_2 == true)
                {
                    // 悬臂胶带制动器接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带制动器接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltBrakeContactAuxiliaryFault_2));
                }

                if (newSystemVariables.SuspensionBeltLoadingContactAuxiliaryFault_2 == true &&
                    _systemVariables.SuspensionBeltLoadingContactAuxiliaryFault_2 == false)
                {
                    //  悬臂胶带堆料接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带堆料接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltLoadingContactAuxiliaryFault_2),
                        "悬臂胶带堆料接触器辅助触点故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspensionBeltLoadingContactAuxiliaryFault_2 == false &&
                         _systemVariables.SuspensionBeltLoadingContactAuxiliaryFault_2 == true)
                {
                    // 悬臂胶带堆料接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带堆料接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltLoadingContactAuxiliaryFault_2));
                }

                if (newSystemVariables.SuspensionBeltUnloadingContactAuxiliaryFault_2 == true &&
                    _systemVariables.SuspensionBeltUnloadingContactAuxiliaryFault_2 == false)
                {
                    // 悬臂胶带取料接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带取料接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltUnloadingContactAuxiliaryFault_2),
                        "悬臂胶带取料接触器辅助触点故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspensionBeltUnloadingContactAuxiliaryFault_2 == false &&
                         _systemVariables.SuspensionBeltUnloadingContactAuxiliaryFault_2 == true)
                {
                    // 悬臂胶带取料接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带取料接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltUnloadingContactAuxiliaryFault_2));
                }

                if (newSystemVariables.SuspensionBeltFirstLevelDeviation_2 == true &&
                    _systemVariables.SuspensionBeltFirstLevelDeviation_2 == false)
                {
                    // 悬臂胶带一级跑偏
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带一级跑偏", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltFirstLevelDeviation_2),
                        "悬臂胶带一级跑偏",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspensionBeltFirstLevelDeviation_2 == false &&
                         _systemVariables.SuspensionBeltFirstLevelDeviation_2 == true)
                {
                    // 悬臂胶带一级跑偏解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带一级跑偏解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltFirstLevelDeviation_2));
                }

                if (newSystemVariables.BucketWheelLubricationPumpContactAuxiliaryFault_2 == true &&
                    _systemVariables.BucketWheelLubricationPumpContactAuxiliaryFault_2 == false)
                {
                    // 斗轮润滑油泵接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("斗轮润滑油泵接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.BucketWheelLubricationPumpContactAuxiliaryFault_2),
                        "斗轮润滑油泵接触器辅助触点故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelLubricationPumpContactAuxiliaryFault_2 == false &&
                         _systemVariables.BucketWheelLubricationPumpContactAuxiliaryFault_2 == true)
                {
                    // 斗轮润滑油泵接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮润滑油泵接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelLubricationPumpContactAuxiliaryFault_2));
                }

                if (newSystemVariables.WindproofSystemCableLimit1_2 == true &&
                    _systemVariables.WindproofSystemCableLimit1_2 == false)
                {
                    // 防风系缆限位 1
                    DataManager.Instance.InsertHistoryWarningMc("防风系缆限位 1", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.WindproofSystemCableLimit1_2), "防风系缆限位 1",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.WindproofSystemCableLimit1_2 == false &&
                         _systemVariables.WindproofSystemCableLimit1_2 == true)
                {
                    // 防风系缆限位 1 解除
                    DataManager.Instance.InsertHistoryWarningMc("防风系缆限位 1 解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.WindproofSystemCableLimit1_2));
                }

                if (newSystemVariables.BucketWheelMotorContactAuxiliaryFault_2 == true &&
                    _systemVariables.BucketWheelMotorContactAuxiliaryFault_2 == false)
                {
                    // 斗轮电机接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电机接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelMotorContactAuxiliaryFault_2),
                        "斗轮电机接触器辅助触点故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelMotorContactAuxiliaryFault_2 == false &&
                         _systemVariables.BucketWheelMotorContactAuxiliaryFault_2 == true)
                {
                    // 斗轮电机接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电机接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelMotorContactAuxiliaryFault_2));
                }

                if (newSystemVariables.TailCarOilPumpMotorContactAuxiliaryFault_2 == true &&
                    _systemVariables.TailCarOilPumpMotorContactAuxiliaryFault_2 == false)
                {
                    // 尾车油泵电机接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("尾车油泵电机接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarOilPumpMotorContactAuxiliaryFault_2),
                        "尾车油泵电机接触器辅助触点故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.TailCarOilPumpMotorContactAuxiliaryFault_2 == false &&
                         _systemVariables.TailCarOilPumpMotorContactAuxiliaryFault_2 == true)
                {
                    // 尾车油泵电机接触器辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车油泵电机接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarOilPumpMotorContactAuxiliaryFault_2));
                }

                if (newSystemVariables.VibrationMotorFault_2 == true && _systemVariables.VibrationMotorFault_2 == false)
                {
                    // 振打电机故障
                    DataManager.Instance.InsertHistoryWarningMc("振打电机故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VibrationMotorFault_2), "振打电机故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VibrationMotorFault_2 == false &&
                         _systemVariables.VibrationMotorFault_2 == true)
                {
                    // 振打电机故障解除
                    DataManager.Instance.InsertHistoryWarningMc("振打电机故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VibrationMotorFault_2));
                }

                if (newSystemVariables.WindproofSystemCableNotOpen_2 == true &&
                    _systemVariables.WindproofSystemCableNotOpen_2 == false)
                {
                    // 防风系缆没有打开
                    DataManager.Instance.InsertHistoryWarningMc("防风系缆没有打开", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.WindproofSystemCableNotOpen_2), "防风系缆没有打开",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.WindproofSystemCableNotOpen_2 == false &&
                         _systemVariables.WindproofSystemCableNotOpen_2 == true)
                {
                    // 防风系缆没有打开解除
                    DataManager.Instance.InsertHistoryWarningMc("防风系缆没有打开解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.WindproofSystemCableNotOpen_2));
                }

                if (newSystemVariables.RotaryLimitAction_2 == true && _systemVariables.RotaryLimitAction_2 == false)
                {
                    // 回转限位动作
                    DataManager.Instance.InsertHistoryWarningMc("回转限位动作", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryLimitAction_2), "回转限位动作",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryLimitAction_2 == false &&
                         _systemVariables.RotaryLimitAction_2 == true)
                {
                    // 回转限位动作解除
                    DataManager.Instance.InsertHistoryWarningMc("回转限位动作解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryLimitAction_2));
                }

                if (newSystemVariables.VariableAmplitudeLimitAction_2 == true &&
                    _systemVariables.VariableAmplitudeLimitAction_2 == false)
                {
                    // 变幅限位动作
                    DataManager.Instance.InsertHistoryWarningMc("变幅限位动作", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLimitAction_2), "变幅限位动作",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeLimitAction_2 == false &&
                         _systemVariables.VariableAmplitudeLimitAction_2 == true)
                {
                    // 变幅限位动作解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅限位动作解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeLimitAction_2));
                }

                if (newSystemVariables.ForbiddenZoneLimitAction_2 == true &&
                    _systemVariables.ForbiddenZoneLimitAction_2 == false)
                {
                    // 禁区限位动作
                    DataManager.Instance.InsertHistoryWarningMc("禁区限位动作", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ForbiddenZoneLimitAction_2), "禁区限位动作",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.ForbiddenZoneLimitAction_2 == false &&
                         _systemVariables.ForbiddenZoneLimitAction_2 == true)
                {
                    // 禁区限位动作解除
                    DataManager.Instance.InsertHistoryWarningMc("禁区限位动作解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.ForbiddenZoneLimitAction_2));
                }

                if (newSystemVariables.RotaryCrashSwitchAction_2 == true &&
                    _systemVariables.RotaryCrashSwitchAction_2 == false)
                {
                    // 回转防撞开关动作
                    DataManager.Instance.InsertHistoryWarningMc("回转防撞开关动作", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryCrashSwitchAction_2), "回转防撞开关动作",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryCrashSwitchAction_2 == false &&
                         _systemVariables.RotaryCrashSwitchAction_2 == true)
                {
                    // 回转防撞开关动作解除
                    DataManager.Instance.InsertHistoryWarningMc("回转防撞开关动作解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryCrashSwitchAction_2));
                }

                if (newSystemVariables.LargeCarCentralizedLubricationLowOilLevelAlarm_2 == true &&
                    _systemVariables.LargeCarCentralizedLubricationLowOilLevelAlarm_2 == false)
                {
                    // 大车集中润滑低油位报警
                    DataManager.Instance.InsertHistoryWarningMc("大车集中润滑低油位报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.LargeCarCentralizedLubricationLowOilLevelAlarm_2),
                        "大车集中润滑低油位报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarCentralizedLubricationLowOilLevelAlarm_2 == false &&
                         _systemVariables.LargeCarCentralizedLubricationLowOilLevelAlarm_2 == true)
                {
                    // 大车集中润滑低油位报警解除
                    DataManager.Instance.InsertHistoryWarningMc("大车集中润滑低油位报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarCentralizedLubricationLowOilLevelAlarm_2));
                }

                if (newSystemVariables.LargeCarCentralizedLubricationOilBlockageAlarm_2 == true &&
                    _systemVariables.LargeCarCentralizedLubricationOilBlockageAlarm_2 == false)
                {
                    // 大车集中润滑堵油报警
                    DataManager.Instance.InsertHistoryWarningMc("大车集中润滑堵油报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.LargeCarCentralizedLubricationOilBlockageAlarm_2),
                        "大车集中润滑堵油报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarCentralizedLubricationOilBlockageAlarm_2 == false &&
                         _systemVariables.LargeCarCentralizedLubricationOilBlockageAlarm_2 == true)
                {
                    // 大车集中润滑堵油报警解除
                    DataManager.Instance.InsertHistoryWarningMc("大车集中润滑堵油报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarCentralizedLubricationOilBlockageAlarm_2));
                }

                if (newSystemVariables.RotaryCentralizedLubricationLowOilLevelAlarm_2 == true &&
                    _systemVariables.RotaryCentralizedLubricationLowOilLevelAlarm_2 == false)
                {
                    // 回转集中润滑低油位报警
                    DataManager.Instance.InsertHistoryWarningMc("回转集中润滑低油位报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationLowOilLevelAlarm_2),
                        "回转集中润滑低油位报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryCentralizedLubricationLowOilLevelAlarm_2 == false &&
                         _systemVariables.RotaryCentralizedLubricationLowOilLevelAlarm_2 == true)
                {
                    // 回转集中润滑低油位报警解除
                    DataManager.Instance.InsertHistoryWarningMc("回转集中润滑低油位报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationLowOilLevelAlarm_2));
                }

                if (newSystemVariables.RotaryCentralizedLubricationOilBlockageAlarm_2 == true &&
                    _systemVariables.RotaryCentralizedLubricationOilBlockageAlarm_2 == false)
                {
                    // 回转集中润滑堵油报警
                    DataManager.Instance.InsertHistoryWarningMc("回转集中润滑堵油报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationOilBlockageAlarm_2),
                        "回转集中润滑堵油报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryCentralizedLubricationOilBlockageAlarm_2 == false &&
                         _systemVariables.RotaryCentralizedLubricationOilBlockageAlarm_2 == true)
                {
                    // 回转集中润滑堵油报警解除
                    DataManager.Instance.InsertHistoryWarningMc("回转集中润滑堵油报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryCentralizedLubricationOilBlockageAlarm_2));
                }

                if (newSystemVariables.BucketWheelCentralizedLubricationLowOilLevelAlarm_2 == true &&
                    _systemVariables.BucketWheelCentralizedLubricationLowOilLevelAlarm_2 == false)
                {
                    // 斗轮集中润滑低油位报警
                    DataManager.Instance.InsertHistoryWarningMc("斗轮集中润滑低油位报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.BucketWheelCentralizedLubricationLowOilLevelAlarm_2),
                        "斗轮集中润滑低油位报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelCentralizedLubricationLowOilLevelAlarm_2 == false &&
                         _systemVariables.BucketWheelCentralizedLubricationLowOilLevelAlarm_2 == true)
                {
                    // 斗轮集中润滑低油位报警解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮集中润滑低油位报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables
                        .BucketWheelCentralizedLubricationLowOilLevelAlarm_2));
                }

                if (newSystemVariables.BucketWheelCentralizedLubricationOilBlockageAlarm_2 == true &&
                    _systemVariables.BucketWheelCentralizedLubricationOilBlockageAlarm_2 == false)
                {
                    // 斗轮集中润滑堵油报警
                    DataManager.Instance.InsertHistoryWarningMc("斗轮集中润滑堵油报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.BucketWheelCentralizedLubricationOilBlockageAlarm_2),
                        "斗轮集中润滑堵油报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelCentralizedLubricationOilBlockageAlarm_2 == false &&
                         _systemVariables.BucketWheelCentralizedLubricationOilBlockageAlarm_2 == true)
                {
                    // 斗轮集中润滑堵油报警解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮集中润滑堵油报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables
                        .BucketWheelCentralizedLubricationOilBlockageAlarm_2));
                }

                if (newSystemVariables.ElectricRoomEmergencyStopButtonAction_2 == true &&
                    _systemVariables.ElectricRoomEmergencyStopButtonAction_2 == false)
                {
                    // 电气室急停按钮动作
                    DataManager.Instance.InsertHistoryWarningMc("电气室急停按钮动作", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ElectricRoomEmergencyStopButtonAction_2),
                        "电气室急停按钮动作",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.ElectricRoomEmergencyStopButtonAction_2 == false &&
                         _systemVariables.ElectricRoomEmergencyStopButtonAction_2 == true)
                {
                    // 电气室急停按钮动作解除
                    DataManager.Instance.InsertHistoryWarningMc("电气室急停按钮动作解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.ElectricRoomEmergencyStopButtonAction_2));
                }

                if (newSystemVariables.CabinEmergencyStopButtonAction_2 == true &&
                    _systemVariables.CabinEmergencyStopButtonAction_2 == false)
                {
                    // 司机室急停按钮动作
                    DataManager.Instance.InsertHistoryWarningMc("司机室急停按钮动作", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CabinEmergencyStopButtonAction_2), "司机室急停按钮动作",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.CabinEmergencyStopButtonAction_2 == false &&
                         _systemVariables.CabinEmergencyStopButtonAction_2 == true)
                {
                    // 司机室急停按钮动作解除
                    DataManager.Instance.InsertHistoryWarningMc("司机室急停按钮动作解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.CabinEmergencyStopButtonAction_2));
                }

                if (newSystemVariables.EmergencyStopRelayNot_2 == true &&
                    _systemVariables.EmergencyStopRelayNot_2 == false)
                {
                    // 急停继电器没有吸合
                    DataManager.Instance.InsertHistoryWarningMc("急停继电器没有吸合", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.EmergencyStopRelayNot_2), "急停继电器没有吸合",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.EmergencyStopRelayNot_2 == false &&
                         _systemVariables.EmergencyStopRelayNot_2 == true)
                {
                    // 急停继电器没有吸合解除
                    DataManager.Instance.InsertHistoryWarningMc("急停继电器没有吸合解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.EmergencyStopRelayNot_2));
                }

                if (newSystemVariables.TransformerOverheatAlarm_2 == true &&
                    _systemVariables.TransformerOverheatAlarm_2 == false)
                {
                    // 变压器超温报警
                    DataManager.Instance.InsertHistoryWarningMc("变压器超温报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TransformerOverheatAlarm_2), "变压器超温报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.TransformerOverheatAlarm_2 == false &&
                         _systemVariables.TransformerOverheatAlarm_2 == true)
                {
                    // 变压器超温报警解除
                    DataManager.Instance.InsertHistoryWarningMc("变压器超温报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.TransformerOverheatAlarm_2));
                }

                if (newSystemVariables.ElectricRoomPLCModulePowerFault_2 == true &&
                    _systemVariables.ElectricRoomPLCModulePowerFault_2 == false)
                {
                    // 电气室 PLC 模块电源故障
                    DataManager.Instance.InsertHistoryWarningMc("电气室PLC模块电源故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ElectricRoomPLCModulePowerFault_2),
                        "电气室 PLC 模块电源故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.ElectricRoomPLCModulePowerFault_2 == false &&
                         _systemVariables.ElectricRoomPLCModulePowerFault_2 == true)
                {
                    // 电气室 PLC 模块电源故障解除
                    DataManager.Instance.InsertHistoryWarningMc("电气室PLC模块电源故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.ElectricRoomPLCModulePowerFault_2));
                }

                if (newSystemVariables.CabinPLCModulePowerFault_2 == true &&
                    _systemVariables.CabinPLCModulePowerFault_2 == false)
                {
                    // 司机室 PLC 模块电源故障
                    DataManager.Instance.InsertHistoryWarningMc("司机室PLC模块电源故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CabinPLCModulePowerFault_2), "司机室PLC模块电源故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.CabinPLCModulePowerFault_2 == false &&
                         _systemVariables.CabinPLCModulePowerFault_2 == true)
                {
                    // 司机室 PLC 模块电源故障解除
                    DataManager.Instance.InsertHistoryWarningMc("司机室PLC模块电源故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.CabinPLCModulePowerFault_2));
                }

                if (newSystemVariables.ElectricRoomFireAlarm_2 == true &&
                    _systemVariables.ElectricRoomFireAlarm_2 == false)
                {
                    // 电气室火灾报警
                    DataManager.Instance.InsertHistoryWarningMc("电气室火灾报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ElectricRoomFireAlarm_2), "电气室火灾报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.ElectricRoomFireAlarm_2 == false &&
                         _systemVariables.ElectricRoomFireAlarm_2 == true)
                {
                    // 电气室火灾报警解除
                    DataManager.Instance.InsertHistoryWarningMc("电气室火灾报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.ElectricRoomFireAlarm_2));
                }

                if (newSystemVariables.CabinFireAlarm_2 == true && _systemVariables.CabinFireAlarm_2 == false)
                {
                    // 司机室火灾报警
                    DataManager.Instance.InsertHistoryWarningMc("司机室火灾报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CabinFireAlarm_2), "司机室火灾报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.CabinFireAlarm_2 == false && _systemVariables.CabinFireAlarm_2 == true)
                {
                    // 司机室火灾报警解除
                    DataManager.Instance.InsertHistoryWarningMc("司机室火灾报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.CabinFireAlarm_2));
                }

                if (newSystemVariables.SuspensionBeltEmergencyStop_2 == true &&
                    _systemVariables.SuspensionBeltEmergencyStop_2 == false)
                {
                    // 悬臂胶带急停拉线
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带急停拉线", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltEmergencyStop_2), "悬臂胶带急停拉线",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspensionBeltEmergencyStop_2 == false &&
                         _systemVariables.SuspensionBeltEmergencyStop_2 == true)
                {
                    // 悬臂胶带急停拉线解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带急停拉线解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltEmergencyStop_2));
                }

                if (newSystemVariables.TailCarBeltEmergencyStopSwitch_2 == true &&
                    _systemVariables.TailCarBeltEmergencyStopSwitch_2 == false)
                {
                    // 尾车胶带急停拉线
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带急停拉线", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarBeltEmergencyStopSwitch_2), "尾车胶带急停拉线",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.TailCarBeltEmergencyStopSwitch_2 == false &&
                         _systemVariables.TailCarBeltEmergencyStopSwitch_2 == true)
                {
                    // 尾车胶带急停拉线解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带急停拉线解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarBeltEmergencyStopSwitch_2));
                }

                if (newSystemVariables.LargeCarMainCircuitBreakerFault_2 == true &&
                    _systemVariables.LargeCarMainCircuitBreakerFault_2 == false)
                {
                    // 大车主断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("大车主断路器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarMainCircuitBreakerFault_2), "大车主断路器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarMainCircuitBreakerFault_2 == false &&
                         _systemVariables.LargeCarMainCircuitBreakerFault_2 == true)
                {
                    // 大车主断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车主断路器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarMainCircuitBreakerFault_2));
                }

                if (newSystemVariables.LargeCarMotorCircuitBreakerFault_2 == true &&
                    _systemVariables.LargeCarMotorCircuitBreakerFault_2 == false)
                {
                    // 大车电机断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("大车电机断路器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarMotorCircuitBreakerFault_2),
                        "大车电机断路器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarMotorCircuitBreakerFault_2 == false &&
                         _systemVariables.LargeCarMotorCircuitBreakerFault_2 == true)
                {
                    // 大车电机断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车电机断路器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarMotorCircuitBreakerFault_2));
                }

                if (newSystemVariables.LargeCarBrakeCircuitBreakerFault_2 == true &&
                    _systemVariables.LargeCarBrakeCircuitBreakerFault_2 == false)
                {
                    // 大车制动器断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("大车制动器断路器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarBrakeCircuitBreakerFault_2),
                        "大车制动器断路器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarBrakeCircuitBreakerFault_2 == false &&
                         _systemVariables.LargeCarBrakeCircuitBreakerFault_2 == true)
                {
                    // 大车制动器断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车制动器断路器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarBrakeCircuitBreakerFault_2));
                }

                if (newSystemVariables.CarFrequencyConverterFault_2 == true &&
                    _systemVariables.CarFrequencyConverterFault_2 == false)
                {
                    // 大车变频器故障
                    DataManager.Instance.InsertHistoryWarningMc("大车变频器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CarFrequencyConverterFault_2), "大车变频器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.CarFrequencyConverterFault_2 == false &&
                         _systemVariables.CarFrequencyConverterFault_2 == true)
                {
                    // 大车变频器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车变频器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.CarFrequencyConverterFault_2));
                }

                if (newSystemVariables.CableReelMainCircuitBreakerFault_2 == true &&
                    _systemVariables.CableReelMainCircuitBreakerFault_2 == false)
                {
                    // 电缆卷筒主断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒主断路器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CableReelMainCircuitBreakerFault_2),
                        "电缆卷筒主断路器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.CableReelMainCircuitBreakerFault_2 == false &&
                         _systemVariables.CableReelMainCircuitBreakerFault_2 == true)
                {
                    // 电缆卷筒主断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒主断路器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.CableReelMainCircuitBreakerFault_2));
                }

                if (newSystemVariables.CableReelMotorOverloading_2 == true &&
                    _systemVariables.CableReelMotorOverloading_2 == false)
                {
                    // 电缆卷筒电机过载
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒电机过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CableReelMotorOverloading_2), "电缆卷筒电机过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.CableReelMotorOverloading_2 == false &&
                         _systemVariables.CableReelMotorOverloading_2 == true)
                {
                    // 电缆卷筒电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒电机过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.CableReelMotorOverloading_2));
                }

                if (newSystemVariables.PowerReelCableOverLooseAlarm_2 == true &&
                    _systemVariables.PowerReelCableOverLooseAlarm_2 == false)
                {
                    // 动力卷筒电缆过松报警
                    DataManager.Instance.InsertHistoryWarningMc("动力卷筒电缆过松报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.PowerReelCableOverLooseAlarm_2), "动力卷筒电缆过松报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.PowerReelCableOverLooseAlarm_2 == false &&
                         _systemVariables.PowerReelCableOverLooseAlarm_2 == true)
                {
                    // 动力卷筒电缆过松报警解除
                    DataManager.Instance.InsertHistoryWarningMc("动力卷筒电缆过松报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.PowerReelCableOverLooseAlarm_2));
                }

                if (newSystemVariables.PowerReelCableOverTightAlarm_2 == true &&
                    _systemVariables.PowerReelCableOverTightAlarm_2 == false)
                {
                    // 动力卷筒电缆过紧报警
                    DataManager.Instance.InsertHistoryWarningMc("动力卷筒电缆过紧报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.PowerReelCableOverTightAlarm_2), "动力卷筒电缆过紧报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.PowerReelCableOverTightAlarm_2 == false &&
                         _systemVariables.PowerReelCableOverTightAlarm_2 == true)
                {
                    // 动力卷筒电缆过紧报警解除
                    DataManager.Instance.InsertHistoryWarningMc("动力卷筒电缆过紧报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.PowerReelCableOverTightAlarm_2));
                }

                if (newSystemVariables.PowerReelFullDiskAlarm_2 == true &&
                    _systemVariables.PowerReelFullDiskAlarm_2 == false)
                {
                    // 动力电缆卷筒满盘报警
                    DataManager.Instance.InsertHistoryWarningMc("动力电缆卷筒满盘报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.PowerReelFullDiskAlarm_2), "动力电缆卷筒满盘报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.PowerReelFullDiskAlarm_2 == false &&
                         _systemVariables.PowerReelFullDiskAlarm_2 == true)
                {
                    // 动力电缆卷筒满盘报警解除
                    DataManager.Instance.InsertHistoryWarningMc("动力电缆卷筒满盘报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.PowerReelFullDiskAlarm_2));
                }

                if (newSystemVariables.PowerReelEmptyDiskAlarm_2 == true &&
                    _systemVariables.PowerReelEmptyDiskAlarm_2 == false)
                {
                    // 动力电缆卷筒空盘报警
                    DataManager.Instance.InsertHistoryWarningMc("动力电缆卷筒空盘报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.PowerReelEmptyDiskAlarm_2), "动力电缆卷筒空盘报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.PowerReelEmptyDiskAlarm_2 == false &&
                         _systemVariables.PowerReelEmptyDiskAlarm_2 == true)
                {
                    // 动力电缆卷筒空盘报警解除
                    DataManager.Instance.InsertHistoryWarningMc("动力电缆卷筒空盘报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.PowerReelEmptyDiskAlarm_2));
                }

                if (newSystemVariables.LargeCarOperationHandleFault_2 == true &&
                    _systemVariables.LargeCarOperationHandleFault_2 == false)
                {
                    // 大车操作手柄故障
                    DataManager.Instance.InsertHistoryWarningMc("大车操作手柄故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarOperationHandleFault_2), "大车操作手柄故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarOperationHandleFault_2 == false &&
                         _systemVariables.LargeCarOperationHandleFault_2 == true)
                {
                    // 大车操作手柄故障解除
                    DataManager.Instance.InsertHistoryWarningMc("大车操作手柄故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarOperationHandleFault_2));
                }

                if (newSystemVariables.RotaryMainCircuitBreakerFault_2 == true &&
                    _systemVariables.RotaryMainCircuitBreakerFault_2 == false)
                {
                    // 回转主断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("回转主断路器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryMainCircuitBreakerFault_2), "回转主断路器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryMainCircuitBreakerFault_2 == false &&
                         _systemVariables.RotaryMainCircuitBreakerFault_2 == true)
                {
                    // 回转主断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转主断路器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryMainCircuitBreakerFault_2));
                }

                if (newSystemVariables.RotaryBrakeOverloadAlarm_2 == true &&
                    _systemVariables.RotaryBrakeOverloadAlarm_2 == false)
                {
                    // 回转制动器过载报警
                    DataManager.Instance.InsertHistoryWarningMc("回转制动器过载报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryBrakeOverloadAlarm_2), "回转制动器过载报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryBrakeOverloadAlarm_2 == false &&
                         _systemVariables.RotaryBrakeOverloadAlarm_2 == true)
                {
                    // 回转制动器过载报警解除
                    DataManager.Instance.InsertHistoryWarningMc("回转制动器过载报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryBrakeOverloadAlarm_2));
                }

                if (newSystemVariables.RotaryFanOverloadAlarm_2 == true &&
                    _systemVariables.RotaryFanOverloadAlarm_2 == false)
                {
                    // 回转风机过载报警
                    DataManager.Instance.InsertHistoryWarningMc("回转风机过载报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryFanOverloadAlarm_2), "回转风机过载报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryFanOverloadAlarm_2 == false &&
                         _systemVariables.RotaryFanOverloadAlarm_2 == true)
                {
                    // 回转风机过载报警解除
                    DataManager.Instance.InsertHistoryWarningMc("回转风机过载报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryFanOverloadAlarm_2));
                }

                if (newSystemVariables.RotaryFrequencyConverterFaulting_2 == true &&
                    _systemVariables.RotaryFrequencyConverterFaulting_2 == false)
                {
                    // 回转变频器故障
                    DataManager.Instance.InsertHistoryWarningMc("回转变频器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryFrequencyConverterFaulting_2), "回转变频器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryFrequencyConverterFaulting_2 == false &&
                         _systemVariables.RotaryFrequencyConverterFaulting_2 == true)
                {
                    // 回转变频器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转变频器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryFrequencyConverterFaulting_2));
                }

                if (newSystemVariables.RotaryBrakeResistorOverheatSwitching_2 == true &&
                    _systemVariables.RotaryBrakeResistorOverheatSwitching_2 == false)
                {
                    // 回转制动电阻超温开关
                    DataManager.Instance.InsertHistoryWarningMc("回转制动电阻超温开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryBrakeResistorOverheatSwitching_2),
                        "回转制动电阻超温开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryBrakeResistorOverheatSwitching_2 == false &&
                         _systemVariables.RotaryBrakeResistorOverheatSwitching_2 == true)
                {
                    // 回转制动电阻超温开关解除
                    DataManager.Instance.InsertHistoryWarningMc("回转制动电阻超温开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryBrakeResistorOverheatSwitching_2));
                }

                if (newSystemVariables.RotaryOverTorqueSwitch_2 == true &&
                    _systemVariables.RotaryOverTorqueSwitch_2 == false)
                {
                    // 回转过力矩开关
                    DataManager.Instance.InsertHistoryWarningMc("回转过力矩开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryOverTorqueSwitch_2), "回转过力矩开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryOverTorqueSwitch_2 == false &&
                         _systemVariables.RotaryOverTorqueSwitch_2 == true)
                {
                    // 回转过力矩开关解除
                    DataManager.Instance.InsertHistoryWarningMc("回转过力矩开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryOverTorqueSwitch_2));
                }

                if (newSystemVariables.ReversalHandleFault_2 == true && _systemVariables.ReversalHandleFault_2 == false)
                {
                    // 回转操作手柄故障
                    DataManager.Instance.InsertHistoryWarningMc("回转操作手柄故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ReversalHandleFault_2), "回转操作手柄故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.ReversalHandleFault_2 == false &&
                         _systemVariables.ReversalHandleFault_2 == true)
                {
                    // 回转操作手柄故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转操作手柄故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.ReversalHandleFault_2));
                }

                if (newSystemVariables.LinkedBucketWheelNotRunning_2 == true &&
                    _systemVariables.LinkedBucketWheelNotRunning_2 == false)
                {
                    // 联动斗轮未运行禁止回转
                    DataManager.Instance.InsertHistoryWarningMc("联动斗轮未运行禁止回转", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LinkedBucketWheelNotRunning_2), "联动斗轮未运行禁止回转",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LinkedBucketWheelNotRunning_2 == false &&
                         _systemVariables.LinkedBucketWheelNotRunning_2 == true)
                {
                    // 联动斗轮未运行禁止回转解除
                    DataManager.Instance.InsertHistoryWarningMc("联动斗轮未运行禁止回转解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LinkedBucketWheelNotRunning_2));
                }

                if (newSystemVariables.VariableFrequencyMainCircuitBreakerFault_2 == true &&
                    _systemVariables.VariableFrequencyMainCircuitBreakerFault_2 == false)
                {
                    // 变幅主断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("变幅主断路器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableFrequencyMainCircuitBreakerFault_2),
                        "变幅主断路器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableFrequencyMainCircuitBreakerFault_2 == false &&
                         _systemVariables.VariableFrequencyMainCircuitBreakerFault_2 == true)
                {
                    // 变幅主断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅主断路器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableFrequencyMainCircuitBreakerFault_2));
                }

                if (newSystemVariables.VariableFrequencyMotorOverload_2 == true &&
                    _systemVariables.VariableFrequencyMotorOverload_2 == false)
                {
                    // 变幅主电机过载
                    DataManager.Instance.InsertHistoryWarningMc("变幅主电机过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableFrequencyMotorOverload_2), "变幅主电机过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableFrequencyMotorOverload_2 == false &&
                         _systemVariables.VariableFrequencyMotorOverload_2 == true)
                {
                    // 变幅主电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅主电机过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableFrequencyMotorOverload_2));
                }

                if (newSystemVariables.VariableFrequencyPumpClogged_2 == true &&
                    _systemVariables.VariableFrequencyPumpClogged_2 == false)
                {
                    // 变幅油泵堵油
                    DataManager.Instance.InsertHistoryWarningMc("变幅油泵堵油", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableFrequencyPumpClogged_2), "变幅油泵堵油",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableFrequencyPumpClogged_2 == false &&
                         _systemVariables.VariableFrequencyPumpClogged_2 == true)
                {
                    // 变幅油泵堵油解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅油泵堵油解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableFrequencyPumpClogged_2));
                }

                if (newSystemVariables.VariableFrequencyPumpStationHighTemperatureAlarm_2 == true &&
                    _systemVariables.VariableFrequencyPumpStationHighTemperatureAlarm_2 == false)
                {
                    // 变幅泵站高温报警信号
                    DataManager.Instance.InsertHistoryWarningMc("变幅泵站高温报警信号", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.VariableFrequencyPumpStationHighTemperatureAlarm_2),
                        "变幅泵站高温报警信号",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableFrequencyPumpStationHighTemperatureAlarm_2 == false &&
                         _systemVariables.VariableFrequencyPumpStationHighTemperatureAlarm_2 == true)
                {
                    // 变幅泵站高温报警信号解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅泵站高温报警信号解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableFrequencyPumpStationHighTemperatureAlarm_2));
                }

                if (newSystemVariables.VariableFrequencyOilTankLowLevelAlarm_2 == true &&
                    _systemVariables.VariableFrequencyOilTankLowLevelAlarm_2 == false)
                {
                    // 变幅油箱油位超低报警信号
                    DataManager.Instance.InsertHistoryWarningMc("变幅油箱油位超低报警信号", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableFrequencyOilTankLowLevelAlarm_2),
                        "变幅油箱油位超低报警信号", Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableFrequencyOilTankLowLevelAlarm_2 == false &&
                         _systemVariables.VariableFrequencyOilTankLowLevelAlarm_2 == true)
                {
                    // 变幅油箱油位超低报警信号解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅油箱油位超低报警信号解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableFrequencyOilTankLowLevelAlarm_2));
                }

                if (newSystemVariables.VariableFrequencyHandleFault_2 == true &&
                    _systemVariables.VariableFrequencyHandleFault_2 == false)
                {
                    // 变幅操作手柄故障
                    DataManager.Instance.InsertHistoryWarningMc("变幅操作手柄故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableFrequencyHandleFault_2), "变幅操作手柄故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableFrequencyHandleFault_2 == false &&
                         _systemVariables.VariableFrequencyHandleFault_2 == true)
                {
                    // 变幅操作手柄故障解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅操作手柄故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableFrequencyHandleFault_2));
                }

                if (newSystemVariables.SuspendedBeltCircuitBreakerFault_2 == true &&
                    _systemVariables.SuspendedBeltCircuitBreakerFault_2 == false)
                {
                    // 悬臂胶带断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带断路器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltCircuitBreakerFault_2),
                        "悬臂胶带断路器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspendedBeltCircuitBreakerFault_2 == false &&
                         _systemVariables.SuspendedBeltCircuitBreakerFault_2 == true)
                {
                    // 悬臂胶带断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带断路器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltCircuitBreakerFault_2));
                }

                if (newSystemVariables.SuspendedBeltMotorOverload_2 == true &&
                    _systemVariables.SuspendedBeltMotorOverload_2 == false)
                {
                    // 悬臂胶带电机过载
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带电机过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltMotorOverload_2), "悬臂胶带电机过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspendedBeltMotorOverload_2 == false &&
                         _systemVariables.SuspendedBeltMotorOverload_2 == true)
                {
                    // 悬臂胶带电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带电机过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltMotorOverload_2));
                }

                if (newSystemVariables.SuspendedBeltEmergencyStop_2 == true &&
                    _systemVariables.SuspendedBeltEmergencyStop_2 == false)
                {
                    // 悬臂胶带急停拉线
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带急停拉线", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltEmergencyStop_2), "悬臂胶带急停拉线",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspendedBeltEmergencyStop_2 == false &&
                         _systemVariables.SuspendedBeltEmergencyStop_2 == true)
                {
                    // 悬臂胶带急停拉线解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带急停拉线解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltEmergencyStop_2));
                }

                if (newSystemVariables.SuspendedBeltLongitudinalTearSwitch_2 == true &&
                    _systemVariables.SuspendedBeltLongitudinalTearSwitch_2 == false)
                {
                    // 悬胶纵向撕裂开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶纵向撕裂开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltLongitudinalTearSwitch_2),
                        "悬胶纵向撕裂开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspendedBeltLongitudinalTearSwitch_2 == false &&
                         _systemVariables.SuspendedBeltLongitudinalTearSwitch_2 == true)
                {
                    // 悬胶纵向撕裂开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶纵向撕裂开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltLongitudinalTearSwitch_2));
                }

                if (newSystemVariables.CentralHopperCloggedDetectionSwitch_2 == true &&
                    _systemVariables.CentralHopperCloggedDetectionSwitch_2 == false)
                {
                    // 中部料斗堵煤检测开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-中部料斗堵煤检测开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CentralHopperCloggedDetectionSwitch_2),
                        "悬胶/挡板-中部料斗堵煤检测开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.CentralHopperCloggedDetectionSwitch_2 == false &&
                         _systemVariables.CentralHopperCloggedDetectionSwitch_2 == true)
                {
                    // 中部料斗堵煤检测开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶/挡板-中部料斗堵煤检测开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.CentralHopperCloggedDetectionSwitch_2));
                }

                if (newSystemVariables.StackingSwitchFault_2 == true && _systemVariables.StackingSwitchFault_2 == false)
                {
                    // 取料开关故障
                    DataManager.Instance.InsertHistoryWarningMc("取料开关故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.StackingSwitchFault_2), "取料开关故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.StackingSwitchFault_2 == false &&
                         _systemVariables.StackingSwitchFault_2 == true)
                {
                    // 取料开关故障解除
                    DataManager.Instance.InsertHistoryWarningMc("取料开关故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.StackingSwitchFault_2));
                }

                // if (newSystemVariables.CentralControlRoomNoStackingCommand_2 == true &&
                //     _systemVariables.CentralControlRoomNoStackingCommand_2 == false)
                // {
                //     // 中控室没有允许取料命令
                //     DataManager.Instance.InsertHistoryWarningMc("中控室没有允许取料命令", GetUserName(),
                //         Machine.BucketWheel);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.CentralControlRoomNoStackingCommand_2),
                //         "中控室没有允许取料命令",
                //         Machine.BucketWheel, false, "");
                // }
                // else if (newSystemVariables.CentralControlRoomNoStackingCommand_2 == false &&
                //          _systemVariables.CentralControlRoomNoStackingCommand_2 == true)
                // {
                //     // 中控室没有允许取料命令解除
                //     DataManager.Instance.InsertHistoryWarningMc("中控室没有允许取料命令解除", GetUserName(),
                //         Machine.BucketWheel);
                //     RemoveWarningDesDict(nameof(newSystemVariables.CentralControlRoomNoStackingCommand_2));
                // }

                if (newSystemVariables.BucketWheelMotorMainCircuitBreakerFault_2 == true &&
                    _systemVariables.BucketWheelMotorMainCircuitBreakerFault_2 == false)
                {
                    // 斗轮电机主断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电机主断路器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelMotorMainCircuitBreakerFault_2),
                        "斗轮电机主断路器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelMotorMainCircuitBreakerFault_2 == false &&
                         _systemVariables.BucketWheelMotorMainCircuitBreakerFault_2 == true)
                {
                    // 斗轮电机主断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电机主断路器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelMotorMainCircuitBreakerFault_2));
                }

                if (newSystemVariables.BucketWheelMotorOverloading_2 == true &&
                    _systemVariables.BucketWheelMotorOverloading_2 == false)
                {
                    // 斗轮电机过载
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电机过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelMotorOverloading_2), "斗轮电机过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelMotorOverloading_2 == false &&
                         _systemVariables.BucketWheelMotorOverloading_2 == true)
                {
                    // 斗轮电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电机过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelMotorOverloading_2));
                }

                if (newSystemVariables.BucketWheelOverTorqueSwitching_2 == true &&
                    _systemVariables.BucketWheelOverTorqueSwitching_2 == false)
                {
                    // 斗轮过力矩开关
                    DataManager.Instance.InsertHistoryWarningMc("斗轮过力矩开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelOverTorqueSwitching_2), "斗轮过力矩开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelOverTorqueSwitching_2 == false &&
                         _systemVariables.BucketWheelOverTorqueSwitching_2 == true)
                {
                    // 斗轮过力矩开关解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮过力矩开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelOverTorqueSwitching_2));
                }

                if (newSystemVariables.BucketWheelTemperatureUpperLimitAlarm_2 == true &&
                    _systemVariables.BucketWheelTemperatureUpperLimitAlarm_2 == false)
                {
                    // 斗轮测温上限报警
                    DataManager.Instance.InsertHistoryWarningMc("斗轮测温上限报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelTemperatureUpperLimitAlarm_2),
                        "斗轮测温上限报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelTemperatureUpperLimitAlarm_2 == false &&
                         _systemVariables.BucketWheelTemperatureUpperLimitAlarm_2 == true)
                {
                    // 斗轮测温上限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮测温上限报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelTemperatureUpperLimitAlarm_2));
                }

                if (newSystemVariables.ClampingDeviceMainCircuitBreakerFault_2 == true &&
                    _systemVariables.ClampingDeviceMainCircuitBreakerFault_2 == false)
                {
                    // 夹轨器主断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器主断路器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.ClampingDeviceMainCircuitBreakerFault_2),
                        "夹轨器主断路器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.ClampingDeviceMainCircuitBreakerFault_2 == false &&
                         _systemVariables.ClampingDeviceMainCircuitBreakerFault_2 == true)
                {
                    // 夹轨器主断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("夹轨器主断路器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.ClampingDeviceMainCircuitBreakerFault_2));
                }

                if (newSystemVariables.LeftClampingDeviceTimeout_2 == true &&
                    _systemVariables.LeftClampingDeviceTimeout_2 == false)
                {
                    // 左夹轨器运行超时
                    DataManager.Instance.InsertHistoryWarningMc("左夹轨器运行超时", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LeftClampingDeviceTimeout_2), "左夹轨器运行超时",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LeftClampingDeviceTimeout_2 == false &&
                         _systemVariables.LeftClampingDeviceTimeout_2 == true)
                {
                    // 左夹轨器运行超时解除
                    DataManager.Instance.InsertHistoryWarningMc("左夹轨器运行超时解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LeftClampingDeviceTimeout_2));
                }

                if (newSystemVariables.RightClampingDeviceTimeout_2 == true &&
                    _systemVariables.RightClampingDeviceTimeout_2 == false)
                {
                    // 右夹轨器运行超时
                    DataManager.Instance.InsertHistoryWarningMc("右夹轨器运行超时", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RightClampingDeviceTimeout_2), "右夹轨器运行超时",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RightClampingDeviceTimeout_2 == false &&
                         _systemVariables.RightClampingDeviceTimeout_2 == true)
                {
                    // 右夹轨器运行超时解除
                    DataManager.Instance.InsertHistoryWarningMc("右夹轨器运行超时解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RightClampingDeviceTimeout_2));
                }

                // if (newSystemVariables.StrongWindAlarm_2 == true && _systemVariables.StrongWindAlarm_2 == false)
                // {
                //     // 大风报警信号
                //     DataManager.Instance.InsertHistoryWarningMc("大风报警信号", GetUserName(),
                //         Machine.BucketWheel);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.StrongWindAlarm_2), "大风报警信号",
                //         Machine.BucketWheel, false, "");
                // }
                // else if (newSystemVariables.StrongWindAlarm_2 == false && _systemVariables.StrongWindAlarm_2 == true)
                // {
                //     // 大风报警信号解除
                //     DataManager.Instance.InsertHistoryWarningMc("大风报警信号解除", GetUserName(),
                //         Machine.BucketWheel);
                //     RemoveWarningDesDict(nameof(newSystemVariables.StrongWindAlarm_2));
                // }

                if (newSystemVariables.DryFogSystemWaterTankLowLevel_2 == true &&
                    _systemVariables.DryFogSystemWaterTankLowLevel_2 == false)
                {
                    // 干雾系统水箱液位低
                    DataManager.Instance.InsertHistoryWarningMc("干雾系统水箱液位低", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DryFogSystemWaterTankLowLevel_2), "干雾系统水箱液位低",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DryFogSystemWaterTankLowLevel_2 == false &&
                         _systemVariables.DryFogSystemWaterTankLowLevel_2 == true)
                {
                    // 干雾系统水箱液位低解除
                    DataManager.Instance.InsertHistoryWarningMc("干雾系统水箱液位低解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DryFogSystemWaterTankLowLevel_2));
                }

                if (newSystemVariables.DiversionPlateCircuitBreakerFault_2 == true &&
                    _systemVariables.DiversionPlateCircuitBreakerFault_2 == false)
                {
                    // 分流挡板断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("分流挡板断路器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DiversionPlateCircuitBreakerFault_2),
                        "分流挡板断路器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DiversionPlateCircuitBreakerFault_2 == false &&
                         _systemVariables.DiversionPlateCircuitBreakerFault_2 == true)
                {
                    // 分流挡板断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("分流挡板断路器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DiversionPlateCircuitBreakerFault_2));
                }

                if (newSystemVariables.BucketWheelFeederCircuitBreakerFault_2 == true &&
                    _systemVariables.BucketWheelFeederCircuitBreakerFault_2 == false)
                {
                    // 斗轮导料槽断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽断路器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelFeederCircuitBreakerFault_2),
                        "斗轮导料槽断路器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelFeederCircuitBreakerFault_2 == false &&
                         _systemVariables.BucketWheelFeederCircuitBreakerFault_2 == true)
                {
                    // 斗轮导料槽断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽断路器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelFeederCircuitBreakerFault_2));
                }

                if (newSystemVariables.BucketWheelFeederMotorOverload_2 == true &&
                    _systemVariables.BucketWheelFeederMotorOverload_2 == false)
                {
                    // 斗轮导料槽电机过载
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽电机过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelFeederMotorOverload_2), "斗轮导料槽电机过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelFeederMotorOverload_2 == false &&
                         _systemVariables.BucketWheelFeederMotorOverload_2 == true)
                {
                    // 斗轮导料槽电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽电机过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelFeederMotorOverload_2));
                }

                if (newSystemVariables.BucketWheelFeederTimeout_2 == true &&
                    _systemVariables.BucketWheelFeederTimeout_2 == false)
                {
                    // 斗轮导料槽运行超时
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽运行超时", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelFeederTimeout_2), "斗轮导料槽运行超时",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelFeederTimeout_2 == false &&
                         _systemVariables.BucketWheelFeederTimeout_2 == true)
                {
                    // 斗轮导料槽运行超时解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮导料槽运行超时解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelFeederTimeout_2));
                }

                // if (newSystemVariables.CentralControlRoomNoStackingUnloadingCommand_2 == true &&
                //     _systemVariables.CentralControlRoomNoStackingUnloadingCommand_2 == false)
                // {
                //     // 中控室没有允许取料命令
                //     DataManager.Instance.InsertHistoryWarningMc("中控室没有允许取料命令", GetUserName(),
                //         Machine.BucketWheel);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.CentralControlRoomNoStackingUnloadingCommand_2),
                //         "中控室没有允许取料命令",
                //         Machine.BucketWheel, false, "");
                // }
                // else if (newSystemVariables.CentralControlRoomNoStackingUnloadingCommand_2 == false &&
                //          _systemVariables.CentralControlRoomNoStackingUnloadingCommand_2 == true)
                // {
                //     // 中控室没有允许取料命令解除
                //     DataManager.Instance.InsertHistoryWarningMc("中控室没有允许取料命令解除", GetUserName(),
                //         Machine.BucketWheel);
                //     RemoveWarningDesDict(nameof(newSystemVariables.CentralControlRoomNoStackingUnloadingCommand_2));
                // }

                if (newSystemVariables.TailCarBeltFirstLevelDeviation_2 == true &&
                    _systemVariables.TailCarBeltFirstLevelDeviation_2 == false)
                {
                    // 尾车胶带一级跑偏
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带一级跑偏", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarBeltFirstLevelDeviation_2), "尾车胶带一级跑偏",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.TailCarBeltFirstLevelDeviation_2 == false &&
                         _systemVariables.TailCarBeltFirstLevelDeviation_2 == true)
                {
                    // 尾车胶带一级跑偏解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带一级跑偏解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarBeltFirstLevelDeviation_2));
                }

                if (newSystemVariables.TailCarBeltSecondLevelDeviation_2 == true &&
                    _systemVariables.TailCarBeltSecondLevelDeviation_2 == false)
                {
                    // 尾车胶带二级跑偏
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带二级跑偏", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.TailCarBeltSecondLevelDeviation_2), "尾车胶带二级跑偏",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.TailCarBeltSecondLevelDeviation_2 == false &&
                         _systemVariables.TailCarBeltSecondLevelDeviation_2 == true)
                {
                    // 尾车胶带二级跑偏解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车胶带二级跑偏解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.TailCarBeltSecondLevelDeviation_2));
                }

                if (newSystemVariables.VibrationMotorCircuitBreakerFault_2 == true &&
                    _systemVariables.VibrationMotorCircuitBreakerFault_2 == false)
                {
                    // 振打电机断路器故障
                    DataManager.Instance.InsertHistoryWarningMc("振打电机断路器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VibrationMotorCircuitBreakerFault_2),
                        "振打电机断路器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VibrationMotorCircuitBreakerFault_2 == false &&
                         _systemVariables.VibrationMotorCircuitBreakerFault_2 == true)
                {
                    // 振打电机断路器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("振打电机断路器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VibrationMotorCircuitBreakerFault_2));
                }

                if (newSystemVariables.VibrationMotorOverloading_2 == true &&
                    _systemVariables.VibrationMotorOverloading_2 == false)
                {
                    // 振打电机过载
                    DataManager.Instance.InsertHistoryWarningMc("振打电机过载", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VibrationMotorOverloading_2), "振打电机过载",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VibrationMotorOverloading_2 == false &&
                         _systemVariables.VibrationMotorOverloading_2 == true)
                {
                    // 振打电机过载解除
                    DataManager.Instance.InsertHistoryWarningMc("振打电机过载解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VibrationMotorOverloading_2));
                }

                // if (newSystemVariables.BucketWheelMotorContactor_2 == true &&
                //     _systemVariables.BucketWheelMotorContactor_2 == false)
                // {
                //     // 斗轮电机接触器
                //     DataManager.Instance.InsertHistoryWarningMc("斗轮电机接触器", GetUserName(),
                //         Machine.BucketWheel);
                //     AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelMotorContactor_2), "斗轮电机接触器",
                //         Machine.BucketWheel, false, "");
                // }
                // else if (newSystemVariables.BucketWheelMotorContactor_2 == false &&
                //          _systemVariables.BucketWheelMotorContactor_2 == true)
                // {
                //     // 斗轮电机接触器解除
                //     DataManager.Instance.InsertHistoryWarningMc("斗轮电机接触器解除", GetUserName(),
                //         Machine.BucketWheel);
                //     RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelMotorContactor_2));
                // }

                if (newSystemVariables.PowerCableRollerNotRunning_2 == true &&
                    _systemVariables.PowerCableRollerNotRunning_2 == false)
                {
                    // 动力电缆卷筒没有运行
                    DataManager.Instance.InsertHistoryWarningMc("动力电缆卷筒没有运行", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.PowerCableRollerNotRunning_2), "动力电缆卷筒没有运行",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.PowerCableRollerNotRunning_2 == false &&
                         _systemVariables.PowerCableRollerNotRunning_2 == true)
                {
                    // 动力电缆卷筒没有运行解除
                    DataManager.Instance.InsertHistoryWarningMc("动力电缆卷筒没有运行解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.PowerCableRollerNotRunning_2));
                }

                if (newSystemVariables.TailCarDrivenRollerBearingTemperatureUpperLimitAlarm_2 == true &&
                    _systemVariables.TailCarDrivenRollerBearingTemperatureUpperLimitAlarm_2 == false)
                {
                    // 尾车从动滚筒轴承测温上限报警
                    DataManager.Instance.InsertHistoryWarningMc("尾车从动滚筒轴承测温上限报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.TailCarDrivenRollerBearingTemperatureUpperLimitAlarm_2),
                        "尾车从动滚筒轴承测温上限报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.TailCarDrivenRollerBearingTemperatureUpperLimitAlarm_2 == false &&
                         _systemVariables.TailCarDrivenRollerBearingTemperatureUpperLimitAlarm_2 == true)
                {
                    // 尾车从动滚筒轴承测温上限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车从动滚筒轴承测温上限报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables
                        .TailCarDrivenRollerBearingTemperatureUpperLimitAlarm_2));
                }

                if (newSystemVariables.TailCarDrivenRollerBearingTemperatureLowerLimitAlarm_2 == true &&
                    _systemVariables.TailCarDrivenRollerBearingTemperatureLowerLimitAlarm_2 == false)
                {
                    // 尾车从动滚筒轴承测温下限报警
                    DataManager.Instance.InsertHistoryWarningMc("尾车从动滚筒轴承测温下限报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.TailCarDrivenRollerBearingTemperatureLowerLimitAlarm_2),
                        "尾车从动滚筒轴承测温下限报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.TailCarDrivenRollerBearingTemperatureLowerLimitAlarm_2 == false &&
                         _systemVariables.TailCarDrivenRollerBearingTemperatureLowerLimitAlarm_2 == true)
                {
                    // 尾车从动滚筒轴承测温下限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("尾车从动滚筒轴承测温下限报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables
                        .TailCarDrivenRollerBearingTemperatureLowerLimitAlarm_2));
                }

                if (newSystemVariables.LargeVehicleMotor1OvertemperatureAlarm_2 == true &&
                    _systemVariables.LargeVehicleMotor1OvertemperatureAlarm_2 == false)
                {
                    // 大车电机 1 超温报警
                    DataManager.Instance.InsertHistoryWarningMc("大车电机1超温报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeVehicleMotor1OvertemperatureAlarm_2),
                        "大车电机1超温报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeVehicleMotor1OvertemperatureAlarm_2 == false &&
                         _systemVariables.LargeVehicleMotor1OvertemperatureAlarm_2 == true)
                {
                    // 大车电机1超温报警解除
                    DataManager.Instance.InsertHistoryWarningMc("大车电机1超温报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeVehicleMotor1OvertemperatureAlarm_2));
                }

                if (newSystemVariables.DriverRoomBalancePumpMotorNotRunning_2 == true &&
                    _systemVariables.DriverRoomBalancePumpMotorNotRunning_2 == false)
                {
                    // 司机室平衡油泵电机没有运行
                    DataManager.Instance.InsertHistoryWarningMc("司机室平衡油泵电机没有运行", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.DriverRoomBalancePumpMotorNotRunning_2),
                        "司机室平衡油泵电机没有运行",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DriverRoomBalancePumpMotorNotRunning_2 == false &&
                         _systemVariables.DriverRoomBalancePumpMotorNotRunning_2 == true)
                {
                    // 司机室平衡油泵电机没有运行解除
                    DataManager.Instance.InsertHistoryWarningMc("司机室平衡油泵电机没有运行解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DriverRoomBalancePumpMotorNotRunning_2));
                }

                if (newSystemVariables.DriverRoomBalancePumpMotorAuxiliaryContactFault_2 == true &&
                    _systemVariables.DriverRoomBalancePumpMotorAuxiliaryContactFault_2 == false)
                {
                    // 司机室平衡油泵电机辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("司机室平衡油泵电机辅助触点故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(
                        nameof(newSystemVariables.DriverRoomBalancePumpMotorAuxiliaryContactFault_2),
                        "司机室平衡油泵电机辅助触点故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.DriverRoomBalancePumpMotorAuxiliaryContactFault_2 == false &&
                         _systemVariables.DriverRoomBalancePumpMotorAuxiliaryContactFault_2 == true)
                {
                    // 司机室平衡油泵电机辅助触点故障解除
                    DataManager.Instance.InsertHistoryWarningMc("司机室平衡油泵电机辅助触点故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.DriverRoomBalancePumpMotorAuxiliaryContactFault_2));
                }

                if (newSystemVariables.SuspendedBeltSecondLevelDeviationSwitch_2 == true &&
                    _systemVariables.SuspendedBeltSecondLevelDeviationSwitch_2 == false)
                {
                    // 悬胶二级跑偏开关
                    DataManager.Instance.InsertHistoryWarningMc("悬胶二级跑偏开关", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltSecondLevelDeviationSwitch_2),
                        "悬胶二级跑偏开关",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspendedBeltSecondLevelDeviationSwitch_2 == false &&
                         _systemVariables.SuspendedBeltSecondLevelDeviationSwitch_2 == true)
                {
                    // 悬胶二级跑偏开关解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶二级跑偏开关解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltSecondLevelDeviationSwitch_2));
                }

                if (newSystemVariables.LargeCarBrakeResistorOverheatJump_2 == true &&
                    _systemVariables.LargeCarBrakeResistorOverheatJump_2 == false)
                {
                    // 大车制动电阻超温跳闸
                    DataManager.Instance.InsertHistoryWarningMc("大车制动电阻超温跳闸", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarBrakeResistorOverheatJump_2),
                        "大车制动电阻超温跳闸",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarBrakeResistorOverheatJump_2 == false &&
                         _systemVariables.LargeCarBrakeResistorOverheatJump_2 == true)
                {
                    // 大车制动电阻超温跳闸解除
                    DataManager.Instance.InsertHistoryWarningMc("大车制动电阻超温跳闸解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarBrakeResistorOverheatJump_2));
                }

                if (newSystemVariables.StrongWindPreAlarm_2 == true && _systemVariables.StrongWindPreAlarm_2 == false)
                {
                    // 大风预报警
                    DataManager.Instance.InsertHistoryWarningMc("大风预报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.StrongWindPreAlarm_2), "大风预报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.StrongWindPreAlarm_2 == false &&
                         _systemVariables.StrongWindPreAlarm_2 == true)
                {
                    // 大风预报警解除
                    DataManager.Instance.InsertHistoryWarningMc("大风预报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.StrongWindPreAlarm_2));
                }

                if (newSystemVariables.LargeCarLimitAction_2 == true && _systemVariables.LargeCarLimitAction_2 == false)
                {
                    // 大车限位动作
                    DataManager.Instance.InsertHistoryWarningMc("大车限位动作", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LargeCarLimitAction_2), "大车限位动作",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LargeCarLimitAction_2 == false &&
                         _systemVariables.LargeCarLimitAction_2 == true)
                {
                    // 大车限位动作解除
                    DataManager.Instance.InsertHistoryWarningMc("大车限位动作解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LargeCarLimitAction_2));
                }

                if (newSystemVariables.VariableAmplitudeOilPumpMotorContactFault_2 == true &&
                    _systemVariables.VariableAmplitudeOilPumpMotorContactFault_2 == false)
                {
                    // 变幅油泵电机接触器故障
                    DataManager.Instance.InsertHistoryWarningMc("变幅油泵电机接触器故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilPumpMotorContactFault_2),
                        "变幅油泵电机接触器故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeOilPumpMotorContactFault_2 == false &&
                         _systemVariables.VariableAmplitudeOilPumpMotorContactFault_2 == true)
                {
                    // 变幅油泵电机接触器故障解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅油泵电机接触器故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilPumpMotorContactFault_2));
                }

                if (newSystemVariables.VariableAmplitudeOilPumpMotorNotRunning_2 == true &&
                    _systemVariables.VariableAmplitudeOilPumpMotorNotRunning_2 == false)
                {
                    // 变幅油泵电机没有运行
                    DataManager.Instance.InsertHistoryWarningMc("变幅油泵电机没有运行", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilPumpMotorNotRunning_2),
                        "变幅油泵电机没有运行",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.VariableAmplitudeOilPumpMotorNotRunning_2 == false &&
                         _systemVariables.VariableAmplitudeOilPumpMotorNotRunning_2 == true)
                {
                    // 变幅油泵电机没有运行解除
                    DataManager.Instance.InsertHistoryWarningMc("变幅油泵电机没有运行解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.VariableAmplitudeOilPumpMotorNotRunning_2));
                }

                if (newSystemVariables.LeftAnchorNotLifted_2 == true && _systemVariables.LeftAnchorNotLifted_2 == false)
                {
                    // 左侧锚锭没有抬起
                    DataManager.Instance.InsertHistoryWarningMc("左侧锚锭没有抬起", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.LeftAnchorNotLifted_2), "左侧锚锭没有抬起",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.LeftAnchorNotLifted_2 == false &&
                         _systemVariables.LeftAnchorNotLifted_2 == true)
                {
                    // 左侧锚锭没有抬起解除
                    DataManager.Instance.InsertHistoryWarningMc("左侧锚锭没有抬起解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.LeftAnchorNotLifted_2));
                }

                if (newSystemVariables.SR1_REMOTE_PLANT_COMM_FAULT_0_2 == true &&
                    _systemVariables.SR1_REMOTE_PLANT_COMM_FAULT_0_2 == false)
                {
                    // 远程任务规划子系统通讯异常
                    DataManager.Instance.InsertHistoryWarningMc("远程任务规划子系统通讯异常", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SR1_REMOTE_PLANT_COMM_FAULT_0_2),
                        "远程任务规划子系统通讯异常",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SR1_REMOTE_PLANT_COMM_FAULT_0_2 == false &&
                         _systemVariables.SR1_REMOTE_PLANT_COMM_FAULT_0_2 == true)
                {
                    // 远程任务规划子系统通讯异常解除
                    DataManager.Instance.InsertHistoryWarningMc("远程任务规划子系统通讯异常解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SR1_REMOTE_PLANT_COMM_FAULT_0_2));
                }

                if (newSystemVariables.SuspensionBeltFault_2 == true &&
                    _systemVariables.SuspensionBeltFault_2 == false)
                {
                    // 悬臂胶带故障
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspensionBeltFault_2),
                        "悬臂胶带故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspensionBeltFault_2 == false &&
                         _systemVariables.SuspensionBeltFault_2 == true)
                {
                    //悬臂胶带故障解除
                    DataManager.Instance.InsertHistoryWarningMc("悬臂胶带故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspensionBeltFault_2));
                }

                if (newSystemVariables.BUCKET_Current_Pause_SLEW_2 == true &&
                    _systemVariables.BUCKET_Current_Pause_SLEW_2 == false)
                {
                    // 斗轮电流过大暂停回转
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电流过大暂停回转", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BUCKET_Current_Pause_SLEW_2),
                        "斗轮电流过大暂停回转",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BUCKET_Current_Pause_SLEW_2 == false &&
                         _systemVariables.BUCKET_Current_Pause_SLEW_2 == true)
                {
                    //斗轮电流过大暂停回转解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮电流过大暂停回转解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BUCKET_Current_Pause_SLEW_2));
                }

                if (newSystemVariables.Slew_Current_Pause_Slew_2 == true &&
                    _systemVariables.Slew_Current_Pause_Slew_2 == false)
                {
                    // 回转电流过大暂停回转
                    DataManager.Instance.InsertHistoryWarningMc("回转电流过大暂停回转", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.Slew_Current_Pause_Slew_2),
                        "回转电流过大暂停回转",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.Slew_Current_Pause_Slew_2 == false &&
                         _systemVariables.Slew_Current_Pause_Slew_2 == true)
                {
                    //悬臂胶带故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转电流过大暂停回转解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.Slew_Current_Pause_Slew_2));
                }
                
                if (newSystemVariables.RotaryLeftTurnForbiddenLimit_2  == true &&
                    _systemVariables.RotaryLeftTurnForbiddenLimit_2  == false)
                {
                    // 回转左转防撞限位
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转防撞限位", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnForbiddenLimit_2 ),
                        "回转-左转防撞限位",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.RotaryLeftTurnForbiddenLimit_2  == false &&
                         _systemVariables.RotaryLeftTurnForbiddenLimit_2  == true)
                {
                    //悬臂胶带故障解除
                    DataManager.Instance.InsertHistoryWarningMc("回转-左转防撞限位解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.RotaryLeftTurnForbiddenLimit_2));
                }
                
                if (newSystemVariables.CableRollerContactorAuxiliaryContactFault_2 == true &&
                    _systemVariables.CableRollerContactorAuxiliaryContactFault_2 == false)
                {
                    // 电缆卷筒接触器辅助触点故障
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒接触器辅助触点故障", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.CableRollerContactorAuxiliaryContactFault_2),
                        "电缆卷筒接触器辅助触点故障",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.CableRollerContactorAuxiliaryContactFault_2 == false &&
                         _systemVariables.CableRollerContactorAuxiliaryContactFault_2 == true)
                {
                    //回转左转防撞限位解除
                    DataManager.Instance.InsertHistoryWarningMc("电缆卷筒接触器辅助触点故障解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.CableRollerContactorAuxiliaryContactFault_2));
                }
                
                //1.22
                if (newSystemVariables.SuspendedBeltTemperatureUpperLimitAlarm_2 == true &&
                    _systemVariables.SuspendedBeltTemperatureUpperLimitAlarm_2 == false)
                {
                    // 悬胶测温上限报警
                    DataManager.Instance.InsertHistoryWarningMc("悬胶测温上限报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltTemperatureUpperLimitAlarm_2),
                        "悬胶测温上限报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspendedBeltTemperatureUpperLimitAlarm_2 == false &&
                         _systemVariables.SuspendedBeltTemperatureUpperLimitAlarm_2 == true)
                {
                    //悬胶测温上限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶测温上限报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltTemperatureUpperLimitAlarm_2));
                }
                
                if (newSystemVariables.SuspendedBeltTemperatureLowerLimitAlarm_2 == true &&
                    _systemVariables.SuspendedBeltTemperatureLowerLimitAlarm_2 == false)
                {
                    // 悬胶测温下限报警
                    DataManager.Instance.InsertHistoryWarningMc("悬胶测温下限报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltTemperatureLowerLimitAlarm_2),
                        "悬胶测温下限报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspendedBeltTemperatureLowerLimitAlarm_2 == false &&
                         _systemVariables.SuspendedBeltTemperatureLowerLimitAlarm_2 == true)
                {
                    //悬胶测温下限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶测温下限报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltTemperatureLowerLimitAlarm_2));
                }
                
                if (newSystemVariables.SuspendedBeltRollerBearingTemperatureUpperLimitAlarm_2 == true &&
                    _systemVariables.SuspendedBeltRollerBearingTemperatureUpperLimitAlarm_2 == false)
                {
                    // 悬胶滚筒轴承测温上限报警
                    DataManager.Instance.InsertHistoryWarningMc("悬胶滚筒轴承测温上限报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltRollerBearingTemperatureUpperLimitAlarm_2),
                        "悬胶滚筒轴承测温上限报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspendedBeltRollerBearingTemperatureUpperLimitAlarm_2 == false &&
                         _systemVariables.SuspendedBeltRollerBearingTemperatureUpperLimitAlarm_2 == true)
                {
                    //悬胶滚筒轴承测温上限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶滚筒轴承测温上限报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltRollerBearingTemperatureUpperLimitAlarm_2));
                }
                
                if (newSystemVariables.SuspendedBeltRollerBearingTemperatureLowerLimitAlarm_2 == true &&
                    _systemVariables.SuspendedBeltRollerBearingTemperatureLowerLimitAlarm_2 == false)
                {
                    // 悬胶滚筒轴承测温下限报警
                    DataManager.Instance.InsertHistoryWarningMc("悬胶滚筒轴承测温下限报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.SuspendedBeltRollerBearingTemperatureLowerLimitAlarm_2),
                        "悬胶滚筒轴承测温下限报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.SuspendedBeltRollerBearingTemperatureLowerLimitAlarm_2 == false &&
                         _systemVariables.SuspendedBeltRollerBearingTemperatureLowerLimitAlarm_2 == true)
                {
                    //悬胶滚筒轴承测温下限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("悬胶滚筒轴承测温下限报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.SuspendedBeltRollerBearingTemperatureLowerLimitAlarm_2));
                }
                
                if (newSystemVariables.BucketWheelTemperatureLowerLimitAlarm_2 == true &&
                    _systemVariables.BucketWheelTemperatureLowerLimitAlarm_2 == false)
                {
                    // 斗轮测温下限报警
                    DataManager.Instance.InsertHistoryWarningMc("斗轮测温下限报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BucketWheelTemperatureLowerLimitAlarm),
                        "斗轮测温下限报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BucketWheelTemperatureLowerLimitAlarm_2 == false &&
                         _systemVariables.BucketWheelTemperatureLowerLimitAlarm_2 == true)
                {
                    //斗轮测温下限报警解除
                    DataManager.Instance.InsertHistoryWarningMc("斗轮测温下限报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BucketWheelTemperatureLowerLimitAlarm_2));
                }
                
                if (newSystemVariables.BrokenBeltCaptureAlarming_2 == true &&
                    _systemVariables.BrokenBeltCaptureAlarming_2 == false)
                {
                    // 断带抓捕报警
                    DataManager.Instance.InsertHistoryWarningMc("断带抓捕报警", GetUserName(),
                        Machine.BucketWheel);
                    AddOrUpdateWarningDesDict(nameof(newSystemVariables.BrokenBeltCaptureAlarming_2),
                        "断带抓捕报警",
                        Machine.BucketWheel, false, "");
                }
                else if (newSystemVariables.BrokenBeltCaptureAlarming_2 == false &&
                         _systemVariables.BrokenBeltCaptureAlarming_2 == true)
                {
                    //斗断带抓捕报警解除
                    DataManager.Instance.InsertHistoryWarningMc("断带抓捕报警解除", GetUserName(),
                        Machine.BucketWheel);
                    RemoveWarningDesDict(nameof(newSystemVariables.BrokenBeltCaptureAlarming_2));
                }
            }

            // Debug.LogError($">>>>>>>>>{isUpdate} {LastMcWarningRecord!=null}");
            if (isUpdate == true && LastMcWarningRecord != null)
            {
                //同步历史警告信息操作
                UpdateWarningByLastMcWarningRecord();
            }

            if (IsUpdatePlcWarningRecord)
            {
                UpdatePlcWarningRecordData();
            }
        }

        public void UpdateWarningByLastMcWarningRecord()
        {
            if (LastMcWarningRecord != null && LastMcWarningRecord.WarningCellDataDict.Count > 0)
            {
                foreach (var data in LastMcWarningRecord.WarningCellDataDict)
                {
                    if (data.Value.IsDataSynchronized)
                    {
                        AddOrUpdateWarningDesDict(data.Value.Key, data.Value.Des, data.Value.Machine, false,
                            data.Value.TriggerDateTime, data.Value.IsConfirm, data.Value.ConfirmTime);
                    }
                }
            }
        }
    }