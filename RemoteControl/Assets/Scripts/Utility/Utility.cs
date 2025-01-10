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

        public static void SetTextByFocused(this InputField inputField, string str)
        {
            if (inputField.isFocused==false)
            {
                inputField.text = str;
            }
        }
        /// <summary>
        /// 根据世界空间位置设置UI在Canvas上的位置。
        /// 如果世界空间位置在世界相机正面，返回 true，否则返回 false 。
        /// </summary>
        /// <param name="worldCamera">世界空间相机（3D）。</param>
        /// <param name="uiCanvas">目标UI所在的Canvas。</param>
        /// <param name="uiTransform">目标UI的RectTransform。</param>
        /// <param name="worldPosition">世界空间位置。</param>
        /// <param name="failedUIPosition">当世界空间位置在世界相机背面时的UI位置。</param>
        /// <returns></returns>
        public static bool SetUIScreenPositionByWorldPosition(Camera worldCamera, Canvas uiCanvas, RectTransform uiTransform, Vector3 worldPosition, Vector2? failedUIPosition = null)
        {
            // 当世界坐标在相机背面时，也能将坐标映射到Canvas上，这是不对的，所以要剔除相机背面的位置
            var camToWorldPos = worldPosition - worldCamera.transform.position;
            if (Vector3.Angle(camToWorldPos, worldCamera.transform.forward) > 90)
            {
                if (failedUIPosition != null)
                {
                    uiTransform.anchoredPosition = failedUIPosition.Value;
                }

                return false;
            }

            // 获取屏幕和Canvas的尺寸
            var canvasSize = ((RectTransform)uiCanvas.transform).sizeDelta;
            var screenSize = new Vector2Int(worldCamera.pixelWidth, worldCamera.pixelHeight);

            // 计算锚点的最大最小最坐标
            var anchorMinPosX = canvasSize.x * uiTransform.anchorMin.x;
            var anchorMaxPosX = canvasSize.x * uiTransform.anchorMax.x;
            var anchorMinPosY = canvasSize.y * uiTransform.anchorMin.y;
            var anchorMaxPosY = canvasSize.y * uiTransform.anchorMax.y;

            // 计算世界空间位置映射到屏幕空间的位置
            var worldCamScreenPos = worldCamera.WorldToScreenPoint(worldPosition);
            // 计算屏幕空间位置映射到UICanvas空间的位置
            worldCamScreenPos.x *= canvasSize.x / screenSize.x;
            worldCamScreenPos.y *= canvasSize.y / screenSize.y;

            // 计算UI在Canvas上的位置
            var uiAnchoredPos = new Vector2()
            {
                x = (worldCamScreenPos.x - anchorMinPosX + worldCamScreenPos.x - anchorMaxPosX) / 2,
                y = (worldCamScreenPos.y - anchorMinPosY + worldCamScreenPos.y - anchorMaxPosY) / 2
            };

            uiTransform.anchoredPosition = uiAnchoredPos;
            return true;
        }
 
    }

}
