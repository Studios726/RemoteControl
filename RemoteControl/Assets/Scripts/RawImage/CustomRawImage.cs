using System.Collections;
using System.Collections.Generic;
using RemoteControl.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomRawImage: RawImage, IPointerClickHandler
{
     // 点击RawImage时，相对RawImage自身的坐标
        private Vector2 ClickPosInRawImg;
        // 预览映射相机
        private Camera PreviewCamera;
        private Camera UICamera;
        private Canvas canvasa;
        protected override void Start()
        {
            // 初始获取预览映射相机
            if (PreviewCamera == null)
            {
                PreviewCamera = GameObject.Find("ModelCamera").transform.GetComponent<Camera>();
            }
            if (UICamera == null)
            {
                UICamera = GameObject.Find("UICamera").transform.GetComponent<Camera>();
            }
            if (canvasa == null)
            {
                canvasa = GameObject.Find("UIRoot/UILayer").transform.GetComponent<Canvas>();
            }
        }
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            //GetRawImageObj(eventData, rectTransform, PreviewCamera);
            // CheckDrawRayLine(canvasa, eventData.position, this, PreviewCamera, UICamera);
        }
        #region UI不绑定相机
        /// <summary>
        /// 通过点击RawImage中映射的RenderTexture画面，对应的相机发射射线，得到物体
        /// </summary>
        /// <param name="data">rawimage点击的数据</param>
        /// <param name="rawImgRectTransform">rawimage的recttransfotm</param>
        /// <param name="previewCamera">生成rendertexture中画面的相机</param>
        /// <returns>返回射线碰撞到的物体</returns>
        private GameObject GetRawImageObj(PointerEventData data, RectTransform rawImgRectTransform, Camera previewCamera)
        {
            GameObject obj = null;
            var pos = (data.position - (Vector2)rawImgRectTransform.position) / rawImgRectTransform.lossyScale - rawImgRectTransform.rect.position;
            var rate = pos / rawImgRectTransform.rect.size;
            var ray = previewCamera.ViewportPointToRay(rate);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                Debug.Log(raycastHit.transform.name);
                obj = raycastHit.transform.gameObject;
            }
            return obj;
        }
        #endregion
        #region UI有专门的UI相机
        /// <summary>
        /// 射线投射
        /// </summary>
        /// <param name="canvas">画布</param>
        /// <param name="mousePosition">当前Canvas下点击的鼠标位置</param>
        /// <param name="previewImage">预览图</param>
        /// <param name="previewCamera">预览映射图的摄像机</param>
        private void CheckDrawRayLine(Canvas canvas, Vector3 mousePosition, RawImage previewImage, Camera previewCamera, Camera UiCamera)
        {
            Vector2 ClickPosInRawImg;
            // 将UI相机下点击的UI坐标转为相对RawImage的坐标
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform.GetComponent<RectTransform>(), mousePosition, UiCamera, out ClickPosInRawImg))
            {
                //获取预览图的长宽
                float imageWidth = previewImage.rectTransform.rect.width;
                float imageHeight = previewImage.rectTransform.rect.height;
                //获取预览图的坐标，此处RawImage的Pivot需为(0,0)，不然自己再换算下
                float localPositionX = previewImage.rectTransform.localPosition.x;
                float localPositionY = previewImage.rectTransform.localPosition.y;

                //获取在预览映射相机viewport内的坐标（坐标比例）
                float p_x = (ClickPosInRawImg.x - localPositionX) / imageWidth;
                float p_y = (ClickPosInRawImg.y - localPositionY) / imageHeight;

                //从视口坐标发射线
                Ray p_ray = previewCamera.ViewportPointToRay(new Vector2(p_x, p_y));
                RaycastHit p_hitInfo;
                if (Physics.Raycast(p_ray, out p_hitInfo))
                {
                    //显示射线，只有在scene视图中才能看到
                    // Debug.DrawLine(p_ray.origin, p_hitInfo.point);
                    // Debug.LogError(p_hitInfo.transform.name);
                    // Debug.LogError(p_hitInfo.point.x+">>>>>>>>>"+p_hitInfo.point.y+">>>>>>>>>>"+p_hitInfo.point.z);
                    EventManager.Instance.TriggerEvent(EventName.ModelOnClickEvent, null,new UpdateModelOnClickEventArgs(p_hitInfo.point,true));
                }
                else
                {
                    EventManager.Instance.TriggerEvent(EventName.ModelOnClickEvent, null,new UpdateModelOnClickEventArgs(Vector3.zero,false));
                }
                // Debug.DrawLine(p_ray.origin, p_ray.direction,Color.red,1000);
            }
            else
            {
                EventManager.Instance.TriggerEvent(EventName.ModelOnClickEvent, null,new UpdateModelOnClickEventArgs(Vector3.zero,false));
            }
        }
        #endregion
}
