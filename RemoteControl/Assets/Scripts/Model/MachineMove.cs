using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MachineMove : MonoBehaviour
{
    public Transform rotationGo_z;
    public Transform rotationGo_y;
    public Transform currentCanvasTransform;
    public TMP_Text errorText;
    public Transform bucketWheelCanvasTransform;
    public TMP_Text bucketWheelPosText;
    public Transform heighCanvasTransform;
    public TMP_Text bucketWheelHeighText;
    public Transform modelCameraTransform;
    public Animation rotClip;
    public Machine machine;
    public float speed;
    private void Start()
    {
        // mainCameraTransform = Camera.main?.transform;
        if (rotClip!=null)
        {
            foreach (AnimationState state in rotClip)
            {
                state.speed = speed;
            }
        }
       
    }

    public void UpdatePosAndRotaionByMeter(float meter, float rotAngleY, float rotAngleZ)
    {
        if (machine==Machine.BucketWheelStackerReclaimer)
        {
            rotationGo_z.localRotation = Quaternion.Euler(new Vector3(0, 0, rotAngleY));
            rotationGo_y.localRotation = Quaternion.Euler(new Vector3(0, rotAngleZ, 0));
        }
        else
        {
            rotationGo_z.localRotation = Quaternion.Euler(new Vector3(0, rotAngleZ, rotAngleY));
        }
       


        transform.localPosition = new Vector3(meter, transform.localPosition.y, transform.localPosition.z);
    }

    public void UpdateErrorText(string error)
    {
        if (error == errorText.text)
        {
            return;
        }

        if (errorText.gameObject.activeSelf == false)
        {
            errorText.gameObject.SetActive(true);
        }

        errorText.text = error;
    }
    
    public void UpdateBucketWheelPosText(string pos)
    {
        bucketWheelPosText.text = pos;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="SLEW_Angle"> 俯仰角度</param>
    public void UpdateBucketWheelHeighText(float SLEW_Angle)
    {
        bucketWheelHeighText.text = (40 * Mathf.Sin(SLEW_Angle * Mathf.Deg2Rad) + ConstStr.InitBucketWheelHeigh).ToString("F2");;
    }
    
    public void PlayRotationClip(bool isPlay)
    {
        if (rotClip == null)
        {
            Debug.Log(" rotClip is null");
            return;
        }
        if (isPlay && rotClip.isPlaying==false)
        {
            rotClip.Play("wheelRotClip");
        }else if (isPlay==false && rotClip.isPlaying==true)
        {
            rotClip.Stop("wheelRotClip");
        }
        else
        {
            //不处理
        }
       
    }
    private void Update()
    {
        if (errorText.text != "" && modelCameraTransform != null)
        {
            currentCanvasTransform.LookAt(
                currentCanvasTransform.position + modelCameraTransform.rotation * Vector3.forward,
                modelCameraTransform.rotation * Vector3.up);
        }
        if (bucketWheelPosText.text != "" && modelCameraTransform != null)
        {
            bucketWheelCanvasTransform.LookAt(
                bucketWheelCanvasTransform.position + modelCameraTransform.rotation * Vector3.forward,
                modelCameraTransform.rotation * Vector3.up);
        }

        if (bucketWheelHeighText.text != "" && modelCameraTransform != null)
        {
            heighCanvasTransform.LookAt(
                heighCanvasTransform.position + modelCameraTransform.rotation * Vector3.forward,
                modelCameraTransform.rotation * Vector3.up);
        }
    }
}