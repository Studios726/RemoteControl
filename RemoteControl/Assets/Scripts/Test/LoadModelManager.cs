using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using RemoteControl.Event;
using ShangHaiPro;
using UnityEngine;
using UnityEngine.Rendering;

public class LoadModelManager : MonoBehaviour
{
    public Material red;
    public Transform parent;
    public GameObject model;
    void Start()
    {
        //string jsonData = Resources.Load("Json/info").ToString();

        //RefreshModel(jsonData);
        EventManager.Instance.AddListener(EventName.RefreshModel, RefreshModel);
    }

    public void RefreshModel(object o, EventArgs eventArgs)
    {
        if (model)
        {
            Debug.LogError("删除多余的模型");
            Destroy(model);
        }

        Timer.Register(0.02f, () =>
        {
           model =
                GameDataManager.Instance.SpawnCoalModel(parent, red, GameDataManager.Instance.SendDataReportAndDEM);
            model.name = "coalModel";
        });

    }
}
