using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotateObject : MonoBehaviour
{
    public float sensitivity = 1.0f; // 鼠标灵敏度
    private Vector2 rotation; // 旋转角度
    public Camera modelCamera; // 主相机
    public float fovChangeSpeed = 1.0f; // FOV变化速度
    public float minFov = 60f; // 最小FOV
    public float maxFov =70f; // 最大FOV
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            // 获取鼠标的水平和垂直位移
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // 根据鼠标位移计算旋转角度
            rotation.x += mouseY * sensitivity;
            // rotation.y += mouseX * sensitivity;

            // 限制旋转角度，防止翻转
            rotation.x = Mathf.Clamp(rotation.x, 0, 90f);

            // 应用旋转
            transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
        }
        // 检测鼠标滚轮的滚动
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // 调整相机的视野
        float newFov = modelCamera.fieldOfView + scroll * fovChangeSpeed;

        // 确保新的FOV值在最小和最大值之间
        newFov = Mathf.Clamp(newFov, minFov, maxFov);

        // 更新相机的视野
        modelCamera.fieldOfView = newFov;
       
    }
}
