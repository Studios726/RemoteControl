using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILayer
{
    public string layerName;
    public int layerNum;

    public UILayer(string name, int layerNum)
    {
        this.layerName = name;
        this.layerNum = layerNum;
    }
}
