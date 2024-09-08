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
    public Transform mainCameraTransform;
    public Animation rotClip;
    public float speed;
    private void Start()
    {
        mainCameraTransform = Camera.main?.transform;
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
        rotationGo_z.localRotation = Quaternion.Euler(new Vector3(0, 0, rotAngleY));
        rotationGo_y.localRotation = Quaternion.Euler(new Vector3(0, rotAngleZ, 0));


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
        if (errorText.text != "" && mainCameraTransform != null)
        {
            currentCanvasTransform.LookAt(
                currentCanvasTransform.position + mainCameraTransform.rotation * Vector3.forward,
                mainCameraTransform.rotation * Vector3.up);
        }
    }
}