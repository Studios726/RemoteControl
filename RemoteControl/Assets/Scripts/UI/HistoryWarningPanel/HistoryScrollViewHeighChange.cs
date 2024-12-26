using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryScrollViewHeighChange : MonoBehaviour
{
    public RectTransform RectTransform;
    public RectTransform ScrollViewRectTransform;
    public ScrollRect ScrollRect;
    public float RectTransformHeight;
    public float ScrollViewRectTransformHeight;
    private void Start()
    {
        RectTransformHeight = RectTransform.rect.height;
        ScrollViewRectTransformHeight = ScrollViewRectTransform.rect.height;
    }
    public void ChangeHeigh(float heigh)
    {
        SetScrollRectPosition(1);
        if (heigh==0)
        {
             
            RectTransform.sizeDelta=new Vector2(RectTransform.sizeDelta.x, RectTransformHeight);
            ScrollViewRectTransform.sizeDelta = new Vector2(ScrollViewRectTransform.sizeDelta.x, ScrollViewRectTransformHeight);
        }
        else
        {
             
            RectTransform.sizeDelta=new Vector2(RectTransform.sizeDelta.x, RectTransformHeight+heigh+13);
            ScrollViewRectTransform.sizeDelta = new Vector2(ScrollViewRectTransform.sizeDelta.x, ScrollViewRectTransformHeight + heigh+13);
        }
     
    }
    public void SetScrollRectPosition(float value)
    {
        if (ScrollRect)
        {
            ScrollRect.verticalNormalizedPosition = value;
        }
     
    }
}
