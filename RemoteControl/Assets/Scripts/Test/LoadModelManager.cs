using System;

using RemoteControl.Event;
using UnityEngine;

public class LoadModelManager : MonoBehaviour
{
    public Material red;
    public Transform parent;
    public GameObject model;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public MeshCollider meshCollider;
    public Mesh cachedMesh;
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
            meshFilter=model.AddComponent<MeshFilter>();
            meshRenderer= model.AddComponent<MeshRenderer>();
            meshCollider= model.AddComponent<MeshCollider>();
        }
        if (cachedMesh == null)
        {
            cachedMesh = new Mesh();
            cachedMesh.MarkDynamic(); // 标记为动态更新
        }
        await GameDataManager.Instance.SpawnCoalModel(parent, red, GameDataManager.Instance.SendDataReportAndDEM,model, meshFilter, meshRenderer, meshCollider,cachedMesh);
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
