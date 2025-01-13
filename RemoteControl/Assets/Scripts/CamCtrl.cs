using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
//摄像机操作   
//删减版   在实际的使用中可能会有限制的需求  比如最大远离多少  最近距离多少   不能旋转到地面以下等
public class CamCtrl : MonoBehaviour
{
    public float sensitivity = 1.0f; // 鼠标灵敏度
    public Transform CenObj;//围绕的物体
    private Vector3 Rotion_Transform;
    private new Camera camera;
    void Start()
    {
        camera = GetComponent<Camera>();
        Rotion_Transform = CenObj.position;
    }
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Cam_Ctrl_Rotation();
        }

        // Ctrl_Cam_Move();
      
    }
    //镜头的远离和接近
    public void Ctrl_Cam_Move()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.Translate(Vector3.forward * 1f);//速度可调  自行调整
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.Translate(Vector3.forward * -1f);//速度可调  自行调整
        }
    }
    //摄像机的旋转
    public void Cam_Ctrl_Rotation()
    {
        var mouseY = -Input.GetAxis("Mouse Y"); // 获取鼠标Y轴移动
        float currentRotationX = transform.rotation.eulerAngles.x;
        float newRotationX = currentRotationX + mouseY * sensitivity;
        // 应用旋转限制
        newRotationX = Mathf.Clamp(newRotationX, 10, 89);
        transform.RotateAround(Rotion_Transform, transform.right, newRotationX - currentRotationX);
    }
}