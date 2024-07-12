using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public static class Utility
    {
        // Start is called before the first frame update
        public static T FindComponent<T>(this Transform transform, string name) where T : MonoBehaviour
        {
            return transform.Find(name)?.GetComponent<T>();
        }

        public static void LoadSprite(this Image image,string url)
        {
            image.sprite = Resources.Load<Sprite>(url);
        }

    
 
    }

}
