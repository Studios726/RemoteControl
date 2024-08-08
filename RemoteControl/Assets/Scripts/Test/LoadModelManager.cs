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
    public GameObject go;
    void Start()
    {
        //string jsonData = Resources.Load("Json/info").ToString();

        //RefreshModel(jsonData);
        EventManager.Instance.AddListener(EventName.RefreshModel, RefreshModel);
    }

    public void RefreshModel(object o, EventArgs eventArgs)
    {
        if (transform.Find("coalModel"))
        {
            Destroy(transform.Find("coalModel").gameObject);
        }
        GameObject model = GameDataManager.Instance.SpawnCoalModel(parent, red, GameDataManager.Instance.SendDataReportAndDEM);
        model.name = "coalModel";
    }
}
