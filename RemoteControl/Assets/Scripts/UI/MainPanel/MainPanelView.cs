using ShenYangRemoteSystem.Subclass;
using System;
using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using ShangHaiPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;
using Random = UnityEngine.Random;

public class MainPanelView : UIView<MainPanelCtr>
{
    private BucketWheelStateBase _bucketWheelState2;
    private BucketWheelStackerReclaimerState _bucketWheelState1;
    private BucketWheelCtrMoveBase _bucketWheelCtrMove2;
    private BucketWheelStackerReclaimerCtrMove _bucketWheelCtrMove1;
    public BucketWheelTaskBase _bucketWheelTask2;
    public BucketWheelStackerReclaimerTask _bucketWheelTask1;
    private HideButtonCtrBase _bucketWheelHideBtnCtr2;
    private BucketWheelStackerReclaimerHideBtnCtr _bucketWheelHideBtnCtr1;
    private Button updateModelBtn;
    private Button updateTaskBtn;
    public List<string> TaskMessageList = new List<string>();

    public override void InitUIElements(UIArgs uiArgs)
    {
        _bucketWheelState2 = RootObj.transform.FindComponent<BucketWheelStateBase>("machine_2/Bg_1");
        _bucketWheelCtrMove2 = RootObj.transform.FindComponent<BucketWheelCtrMoveBase>("machine_2/Bg_2");
        _bucketWheelTask2 = RootObj.transform.FindComponent<BucketWheelTaskBase>("machine_2/Bg_3");
        _bucketWheelHideBtnCtr2 = RootObj.transform.FindComponent<HideButtonCtrBase>("machine_2/hideCtrBtns");

        _bucketWheelState1 = RootObj.transform.FindComponent<BucketWheelStackerReclaimerState>("machine_1/Bg_1");
        _bucketWheelCtrMove1 = RootObj.transform.FindComponent<BucketWheelStackerReclaimerCtrMove>("machine_1/Bg_2");
        _bucketWheelTask1 = RootObj.transform.FindComponent<BucketWheelStackerReclaimerTask>("machine_1/Bg_3");
        _bucketWheelHideBtnCtr1 =
            RootObj.transform.FindComponent<BucketWheelStackerReclaimerHideBtnCtr>("machine_1/hideCtrBtns");

        updateModelBtn = RootObj.transform.FindComponent<Button>("updateModel");
        updateTaskBtn = RootObj.transform.FindComponent<Button>("updateTaskArgs");
        _bucketWheelCtrMove2.hideBtn.onClick.AddListener(ActiveHideBtnCtr2);
        updateModelBtn.onClick.AddListener(() =>
        {
            // GameDataManager.Instance.PileTakeMaterPop(TaskType.None, 5, Machine.BucketWheel);
            GameDataManager.Instance.PileTakeMaterPop(TaskType.PILEMATER, 15, Machine.BucketWheel);
            // if (GameDataManager.Instance.GameMain.connectionSCA.isConnect == false)
            // {
            //     UIManager.Instance.OpenUI(UIID.ConfirmPanel,
            //         new ConfirmPanelArgs(ConstStr.SCA_SERVER_CONNECTION_FAIL_TIP,"三维扫描"));
            //     return;
            // }
            //
            // GameDataManager.Instance.UpdateSCAData(30);
        });
        updateTaskBtn.onClick.AddListener((() =>
        {
            if (GameDataManager.Instance.GameMain.connectionPC.isConnect == false)
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,
                    new ConfirmPanelArgs(ConstStr.RC_SERVER_CONNECTION_FAIL_TIP,"任务规划"));
                return;
            }

            TaskDataManager.Instance.UpdateTaskData();
        }));
        _bucketWheelCtrMove1.hideBtn.onClick.AddListener(ActiveHideBtnCtr1);
        UpdateData(GameDataManager.Instance.SystemVariables);
        // TaskDataManager.Instance.GetNearestTaskDataDic();
        // GameDataManager.Instance.DeleteThreeMonthData();
        TaskDataManager.Instance.UpdateTaskData();
    }

    private void ActiveHideBtnCtr2()
    {
        _bucketWheelHideBtnCtr2.gameObject.SetActive(!_bucketWheelHideBtnCtr2.gameObject.activeSelf);
        _bucketWheelCtrMove2.show.SetActive(!_bucketWheelHideBtnCtr2.gameObject.activeSelf);
        _bucketWheelCtrMove2.hide.SetActive(_bucketWheelHideBtnCtr2.gameObject.activeSelf);
    }

    private void ActiveHideBtnCtr1()
    {
        _bucketWheelHideBtnCtr1.gameObject.SetActive(!_bucketWheelHideBtnCtr1.gameObject.activeSelf);
        _bucketWheelCtrMove1.show.SetActive(!_bucketWheelHideBtnCtr1.gameObject.activeSelf);
        _bucketWheelCtrMove1.hide.SetActive(_bucketWheelHideBtnCtr1.gameObject.activeSelf);
    }

    public void UpdateData(SystemVariables data)
    {
        if (data == null)
        {
            return;
        }

        _bucketWheelState2.UpdateData(data);
        _bucketWheelState1.UpdateData(data);
        _bucketWheelCtrMove2.UpdateData(data);
        _bucketWheelCtrMove1.UpdateData(data);
        if (_bucketWheelHideBtnCtr1.gameObject.activeSelf == true)
        {
            _bucketWheelHideBtnCtr1.UpdateData(data);
        }

        if (_bucketWheelHideBtnCtr2.gameObject.activeSelf == true)
        {
            _bucketWheelHideBtnCtr2.UpdateData(data);
        }

        _bucketWheelTask2.UpdatePlc(data);
        _bucketWheelTask1.UpdatePlc(data);
    }

    public void UpdateData(object o, EventArgs eventArgs)
    {
        if (GameDataManager.Instance.SystemVariables == null)
        {
            return;
        }

        UpdateData(GameDataManager.Instance.SystemVariables);
    }

    public void UpdatePcData(object o, EventArgs eventArgs)
    {
        TaskVariables taskVariables = TaskDataManager.Instance.TaskVariables;
        if (taskVariables == null)
        {
            return;
        }
        
        if (taskVariables.McData.Count > 0)
        {
            if (taskVariables.McData.Count == 1)
            {
                if (taskVariables.McData[0].Machine == Machine.BucketWheel)
                {
                    _bucketWheelTask2.UpdateData(taskVariables.McData[0]);
                    _bucketWheelTask1?.ResetState();
                }
                else
                {
                    _bucketWheelTask1.UpdateData(taskVariables.McData[0]);
                    _bucketWheelTask2?.ResetState();
                }
            }
            else
            {
                for (int i = 0; i < taskVariables.McData.Count; i++)
                {
                    if (taskVariables.McData[i].Machine == Machine.BucketWheel)
                    {
                        _bucketWheelTask2.UpdateData(taskVariables.McData[i]);
                    }
                    else
                    {
                        _bucketWheelTask1.UpdateData(taskVariables.McData[i]);
                    }
                }
            }
        }
        else
        {
            _bucketWheelTask2?.ResetState();
            _bucketWheelTask1?.ResetState();
        }
    }
    
    public void AddOnClickListener(Button btn, UnityAction action)
    {
        btn.onClick.AddListener(action);
    }

    public void AddOnClickListener(ButtonCell btn, UnityAction action)
    {
        btn.AddListener(action);
    }
}