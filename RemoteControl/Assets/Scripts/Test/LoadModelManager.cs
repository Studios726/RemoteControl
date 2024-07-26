using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
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
        string jsonData = Resources.Load("Json/info").ToString();

        RefreshModel(jsonData);
    }

    public void RefreshModel(string jsonData)
    {
        if (transform.Find("coalModel"))
        {
            Destroy(transform.Find("coalModel").gameObject);
        }
        SendDataReportAndDEM sendDataReportAndDem = JsonConvert.DeserializeObject<SendDataReportAndDEM>(jsonData);
        GameObject model = GameDataManager.Instance.SpawnCoalModel(parent, red,sendDataReportAndDem );
        model.name = "coalModel";
    }
}
