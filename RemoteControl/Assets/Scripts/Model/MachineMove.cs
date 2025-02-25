using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MachineMove : MonoBehaviour
{
    public Transform beltAnim;
    public Transform rotationGo_z;
    public Transform rotationGo_y;
    public Transform currentCanvasTransform;
    public GameObject FogfallVfx;
    public ParticleSystem FogfallVfxPs;
    public TMP_Text errorText;
    public RectTransform errorTextRectTransform;
    public RectTransform bgRectTransform;
    public Transform bucketWheelCanvasTransform;
    public TMP_Text bucketWheelPosText;
    public Transform heighCanvasTransform;
    public TMP_Text bucketWheelHeighText;
    public Transform modelCameraTransform;
    public Animation rotClip;
    public Animation cantileverClipTake;
    public Animation cantileverClipPile;
    private Material[] beltMaterials;
    public Machine machine;
    public float speed;
    private void Start()
    {
        //TODO 注释动画播放
        // mainCameraTransform = Camera.main?.transform;
        // if (rotClip!=null)
        // {
        //     foreach (AnimationState state in rotClip)
        //     {
        //         state.speed = speed;
        //     }
        // }
        // if (cantileverClipTake!=null)
        // {
        //     foreach (AnimationState state in cantileverClipTake)
        //     {
        //         state.speed = speed;
        //     }
        // }
        // if (cantileverClipPile!=null)
        // {
        //     foreach (AnimationState state in cantileverClipPile)
        //     {
        //         state.speed = speed;
        //     }
        // }

        AddBeltMaterials();
    }
    public void AddBeltMaterials()
    {
        beltMaterials=new Material[beltAnim.childCount];
        for (int i = 0; i < beltAnim.childCount; i++)
        {
            beltMaterials[i]=beltAnim.GetChild(i).GetChild(0).GetComponent<MeshRenderer>().materials[0];
            beltMaterials[i].SetFloat("_FlowSpeed", 0);
        }
    }
    public void SetFlowSpeed(Material[] materials, float speed)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].GetFloat("_FlowSpeed")!=speed)
            {
                materials[i].SetFloat("_FlowSpeed", speed);
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
        // SetBgHigh();
    }

    public void SetBgHigh()
    {
        if (errorText.text=="")
        {
            bgRectTransform.sizeDelta=new Vector2(bgRectTransform.sizeDelta.x,0) ;
        }
        else
        {
            float y = errorTextRectTransform.rect.height-20.1f+32.7f;
            if (bgRectTransform.sizeDelta.y!=y)
            {
                bgRectTransform.sizeDelta=new Vector2(bgRectTransform.sizeDelta.x,y) ;
                Debug.Log($">>>>>>>>>>>>>> { y } {errorTextRectTransform.rect.height} {bgRectTransform.sizeDelta.y}");
            }
        }
    }
    public void UpdateBucketWheelPosText(string pos)
    {
        // bucketWheelPosText.text = pos;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Luff_Angle"> 俯仰角度</param>
    public void UpdateBucketWheelHeighText(float Luff_Angle)
    {
        bucketWheelHeighText.text = (40 * Mathf.Sin(Luff_Angle * Mathf.Deg2Rad) + ConstStr.InitBucketWheelHeigh).ToString("F2")+" m";
    }
    /// <summary>
    /// 是否播放取料动画
    /// </summary>
    /// <param name="isPlay"></param>
    public void PlayCantileverClipTake(bool isPlay)
    {
        if (cantileverClipTake == null)
        {
            Debug.Log(" cantilever is null");
            return;
        }
        if (isPlay && cantileverClipTake.isPlaying==false)
        {
            cantileverClipTake.gameObject.SetActive(true);
            cantileverClipTake.Play("cantilever");
        }else if (isPlay==false && cantileverClipTake.isPlaying==true)
        {
            cantileverClipTake.Stop("cantilever");
            cantileverClipTake.gameObject.SetActive(false);
        }
        else
        {
            //不处理
        }
       
    }
    public void PlayCantileverClipPile(bool isPlay)
    {
        if (cantileverClipPile == null)
        {
            Debug.Log(" cantilever is null");
            return;
        }
        if (isPlay && cantileverClipPile.isPlaying==false)
        {
            cantileverClipPile.gameObject.SetActive(true);
            cantileverClipPile.Play("cantilever");
        }else if (isPlay==false && cantileverClipPile.isPlaying==true)
        {
            cantileverClipPile.Stop("cantilever");
            cantileverClipPile.gameObject.SetActive(false);
        }
        else
        {
            //不处理
        }
       
    }

    public void PlayBeltAnimation(bool isTake,bool isPile)
    {
        if (isTake)
        {
            SetFlowSpeed(beltMaterials, 1);
        }else if (isPile)
        {
            SetFlowSpeed(beltMaterials, -1);
        }else
        {
            SetFlowSpeed(beltMaterials, 0);
        }
       
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

    private void OnEnable()
    {
        if (FogfallVfx.activeSelf&&FogfallVfxPs.isPlaying==false)
        {
            FogfallVfxPs.Play();
        }
    }

    public void SetFogfallVfxActive(bool isActive)
    {
        if (FogfallVfx.activeSelf != isActive)
        {
            FogfallVfx.SetActive(isActive);
            if (isActive)
            {
                FogfallVfxPs.Play();
            }
            else
            {
                FogfallVfxPs.Stop();
            }
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

        SetBgHigh();
    }
}