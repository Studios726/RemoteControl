using System;
using System.Collections;
using System.Collections.Generic;
using RemoteControl;
using RemoteControl.Event;
using UnityEngine;

public class AppLauncher : MonoBehaviour
{
    public GameMain _gameMain;
    private bool isQuit;
    private int previousWidth;
    private int previousHeight;
    private int keyCodeCount_k;
    private void Awake()
    {

        Application.targetFrameRate = 60;
        UIInit();
        _gameMain=this.gameObject.AddComponent<GameMain>();
        GameDataManager.Instance.GameMain = _gameMain;
        GameStart();
        OnApplicationQuit();
        previousWidth=Screen.width;
        previousHeight=Screen.height;
        EventManager.Instance.AddListener(EventName.ExitGame, (o, args) =>
        {
            ExitGamePop();
        });
        FullScreen();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("是否确定退出远程监控","退出远程监控", (() => isQuit = false),()=>
            {
                DataManager.Instance.InsertHistoryLogMc("退出登录-ESC", GameDataManager.Instance.GetUserName(), Machine.BucketWheel);
                DataManager.Instance.InsertHistoryLogMc("退出登录-ESC", GameDataManager.Instance.GetUserName(), Machine.BucketWheelStackerReclaimer);
                isQuit = true;
                _gameMain.OnExitGame();
            }));
        }else if (Input.GetKeyDown(KeyCode.Tab))
        {
            EventManager.Instance.TriggerEvent(EventName.KeyCodeTab, null);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            keyCodeCount_k++;
            if (keyCodeCount_k>=3)
            {
                EventManager.Instance.TriggerEvent(EventName.ShowTestEvent);
                keyCodeCount_k = 0;
            }
        }
        if (Screen.width!= previousWidth || Screen.height!= previousHeight)
        {
            Debug.Log("屏幕分辨率发生变化: " + Screen.width + " x " + Screen.height);
            previousWidth = Screen.width;
            previousHeight = Screen.height;
            EventManager.Instance.TriggerEvent(EventName.RefreshScreen, null);
        }
       
    }
    private void OnApplicationQuit()
    {
        Application.wantsToQuit += WantsToQuitEvent;
    }

    public bool WantsToQuitEvent()
    {
        if (isQuit==false)
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("是否确定退出远程监控","退出远程监控", (() => isQuit = false),()=>
            {
                isQuit = true;
                _gameMain.OnExitGame();
            }));
        }
      
        return isQuit;
    }

    public void ExitGamePop()
    {
        UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("是否确定退出远程监控","退出远程监控", (() => isQuit = false),()=>
        {
            DataManager.Instance.InsertHistoryLogMc("退出登录", GameDataManager.Instance.GetUserName(), Machine.BucketWheel);
            DataManager.Instance.InsertHistoryLogMc("退出登录", GameDataManager.Instance.GetUserName(), Machine.BucketWheelStackerReclaimer);
            isQuit = true;
            _gameMain.OnExitGame();
        }));
    }
    public void UIInit()
    {
        UILayer[] uiLayers =new[]
        {
             new UILayer("UILayer",1),
             new UILayer("UITopLayer",60),
             new UILayer("UIPopupLayer",3020),
             new UILayer("UILoadingLayer",3021),

        };
        UIManager.Instance.Init(uiLayers);
        //生成调试工具
        UIManager.Instance.InitDebugTool();
    }

    public void FullScreen()
    {
        Resolution[] resolutions = Screen.resolutions;//获取设置当前屏幕分辩率
        //找到最大分辨率
        int width = resolutions[0].width, height = resolutions[0].height;
        //对数组进行排序
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width > width)
            {
                width = resolutions[i].width;
                height = resolutions[i].height;
            }
            if (resolutions[i].width == width && height > resolutions[i].height)
            {
                width = resolutions[i].width;
                height = resolutions[i].height;
            }
        }
        Screen.SetResolution(width, height, true);

    }
    private void GameStart()
    {
        _gameMain.EnterGame();
    }

    public void Scree()
    {
        // Screen.fullScreenChanged += OnFullScreenChanged;
        // Screen.widthChanged += OnResolutionChanged;
        // Screen.heightChanged += OnResolutionChanged;
        // Screen.orientation
    }
}
