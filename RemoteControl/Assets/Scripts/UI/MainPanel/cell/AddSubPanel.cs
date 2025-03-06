using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddSubPanel :PanelBase
{
    public RectTransform targetUIElement; // 拖入目标UI元素
    public ButtonCell buttonCell_1;
    public ButtonCell buttonCell_2;
    public ButtonCell buttonCell_3;
    public ButtonCell buttonCell_4;
    public Action<int, InputFieldType, SymbolType> action;
    private InputFieldType inputFieldType;
    private SymbolType symbolType;
    private bool isOnClick;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isOnClick = false;
            // 检测是否点击到UI
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // 执行射线检测
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                eventData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);
              
                foreach (var result in results)
                {
                    if (result.gameObject == targetUIElement.gameObject)
                    {
                        isOnClick=true;
                        break;
                    }
                }
            }
            gameObject.SetActive(isOnClick);
        }
    }
    private void Start()
    {
        AddOnClickListener(buttonCell_1, (() =>
        {
            action?.Invoke(0, inputFieldType, symbolType);
        }));
        AddOnClickListener(buttonCell_2, (() =>
        {
            action?.Invoke(1, inputFieldType, symbolType);
        }));
        AddOnClickListener(buttonCell_3, (() =>
        {
            action?.Invoke(2, inputFieldType, symbolType);
        }));
        AddOnClickListener(buttonCell_4, (() =>
        {
            action?.Invoke(3, inputFieldType, symbolType);
        }));
    }
    public void SetDataByAddSubBtn(InputFieldType inputFieldType, SymbolType symbolType,Vector3 position,List<float> list,Action<int, InputFieldType, SymbolType> action)
    {
        gameObject.SetActive(true);
        this.inputFieldType=inputFieldType;
        this.symbolType=symbolType;
        position.y = position.y - 0.079f;
        transform.position = position;
        this.action=action;
        buttonCell_1.SetText(list[0].ToString());
        buttonCell_2.SetText(list[1].ToString());
        buttonCell_3.SetText(list[2].ToString());
        buttonCell_4.SetText(list[3].ToString());
    }
}
