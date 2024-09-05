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

    public async  void RefreshModel(object o, EventArgs eventArgs)
    {
        if (model==null)
        {
            model = new GameObject("coalModel");
            model.transform.SetParent(parent);
             model.AddComponent<MeshFilter>();
             model.AddComponent<MeshRenderer>();
             model.AddComponent<MeshCollider>();
        }
        await GameDataManager.Instance.SpawnCoalModel(parent, red, GameDataManager.Instance.SendDataReportAndDEM,model);
        model.name = "coalModel";
    }

    private void OnDestroy()
    {
        if (model!=null)
        {
            Destroy(model);
        }
    }
}
