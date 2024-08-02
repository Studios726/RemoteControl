using System;
using System.Collections;
using System.Collections.Generic;
using RemoteControl;
using UnityEngine;

public class AppLauncher : MonoBehaviour
{
    private GameMain _gameMain;
    private void Awake()
    {
        //
        Application.targetFrameRate = 60;
        UIInit();
        _gameMain=this.gameObject.AddComponent<GameMain>();
        GameStart();
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
