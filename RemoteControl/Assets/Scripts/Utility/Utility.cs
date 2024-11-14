using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public enum TextType
    {
        None,
        Meter,
        Angle,
        Electricity,
        Tonne,//吨
        TonneHour

    }
    public static class Utility
    {
        // Start is called before the first frame update
        public static T FindComponent<T>(this Transform transform, string name) where T : Component
        {
            return transform.Find(name)?.GetComponent<T>();
        }

        public static void LoadSprite(this Image image,string url)
        {
            image.sprite = Resources.Load<Sprite>(url);
        }
        /// <summary>
        ///  设置文本符号
        /// </summary>
        /// <param name="text"></param>
        /// <param name="str"></param>
        /// <param name="type">1 m 2 度 3 A </param>
        public static void SetTextSymbol(this Text text,string str , TextType textType)
        {
            if (textType == TextType.Meter)
            {
                str = str + "m";
            }
            else if (textType == TextType.Angle) {
                str = str + "°";
            }
            else if (textType == TextType.Electricity)
            {
                str = str + "A";
            } else if (textType == TextType.Tonne)
            {
                str = str + "t";
            }else if (textType == TextType.TonneHour)
            {
                str = str + "t/h";
            }
            text.text = str;
        }
    
 
    }

}
