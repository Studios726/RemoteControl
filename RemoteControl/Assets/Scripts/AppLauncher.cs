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
    private void Awake()
    {

        // Application.targetFrameRate = 60;
        UIInit();
        _gameMain=this.gameObject.AddComponent<GameMain>();
        GameDataManager.Instance.GameMain = _gameMain;
        GameStart();
        OnApplicationQuit();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("是否确定退出远程监控", (() => isQuit = false),()=>
            {
                isQuit = true;
                _gameMain.OnExitGame();
            }));
        }else if (Input.GetKeyDown(KeyCode.Tab))
        {
            EventManager.Instance.TriggerEvent(EventName.KeyCodeTab, null);
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
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("是否确定退出远程监控", (() => isQuit = false),()=>
            {
                isQuit = true;
                _gameMain.OnExitGame();
            }));
        }
      
        return isQuit;
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
    }
    private void GameStart()
    {
        _gameMain.EnterGame();
    }
}
