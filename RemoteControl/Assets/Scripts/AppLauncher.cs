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
        //Game 初始化
        _gameMain=this.gameObject.AddComponent<GameMain>();
        GameStart();
    }

    private void GameStart()
    {
        _gameMain.EnterGame();
    }
}
