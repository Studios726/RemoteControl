using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
   private Dictionary<UIID, IUIPresenter> _uiPresenter = new Dictionary<UIID, IUIPresenter>();
   private Dictionary<UIID, bool> _uiActive = new Dictionary<UIID, bool>();
   private List<GameObject> _canvasList = new List<GameObject>();
   private GameObject _uiRoot;
   private GameObject _canvasPrefab;
   
   public void HideCanvasParent()
   {
      _uiRoot.SetActive(false);
   }

   public void ShowCanvasParent()
   {
      _uiRoot.SetActive(true);
   }


   
   public void Init(UILayer[] uiLayers)
   {
      _canvasPrefab = Resources.Load<GameObject>("UI/Canvas");
      _uiRoot = new GameObject("UIRoot");
      GameObject.DontDestroyOnLoad(_uiRoot);
      for (int i = 0; i <uiLayers.Length; ++i)
      {
         GameObject canvas = GameObject.Instantiate(_canvasPrefab, _uiRoot.transform);
         canvas.GetComponent<Canvas>().sortingOrder = uiLayers[i].layerNum;
         _canvasList.Add(canvas);
         canvas.name = uiLayers[i].layerName;
      }
   }
   public void OpenUI(UIID id)
   {
      if (!_uiPresenter.TryGetValue(id, out IUIPresenter uiPresenter))
      {
         GameObject prefab = Resources.Load<GameObject>("UI/"+id.Name);
         GameObject rootObj;
         for (int i = 0; i < _canvasList.Count; i++)
         {
            if (_canvasList[i].name==id.LayerName)
            {
               rootObj = GameObject.Instantiate(prefab, _canvasList[i].transform);
               IUIView view = Activator.CreateInstance(id.ViewType) as IUIView;
               IUIPresenter presenter = Activator.CreateInstance(id.PresenterType) as IUIPresenter;

               uiPresenter = presenter;
               _uiPresenter[id] = uiPresenter;

               id.ViewType.GetField("presenter").SetValue(view, presenter);
               id.PresenterType.GetField("view").SetValue(presenter, view);
               id.ViewType.GetField("_rootObj", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(view, rootObj);
               view.InitUIElements();
               break;
            }
         }
      }
      if (!_uiActive.TryGetValue(id, out bool cntActive) || !cntActive)
      {
         uiPresenter.ShowView();
         _uiActive[id] = true;
      }
   }
   public void CloseUI(UIID id)
   {
      if (!_uiActive.TryGetValue(id, out bool cntActive) || cntActive)
      {
         _uiActive[id] = false;
         if  (_uiPresenter.TryGetValue(id, out IUIPresenter uiPresenter))
         {
            uiPresenter.HideView();
         }

      }
   }
}
