using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModelAnglesDisplayer : MonoBehaviour
{
    public Camera modelCam;
    public Transform stackerArmRotationPoint;
    public Transform materialArmRotationPoint;
    public Transform inclinometerPoint;

    public TMP_Text stackerArmRotationText;
    public TMP_Text materialArmRotationText;
    public TMP_Text inclinometerText;

    public Vector2 stackerArmRotationOffset;
    public Vector2 materialArmRotationOffset;
    public Vector2 inclinometerOffset;

    public void UpdateTexts(float stackerArmRotation, float materialArmRotation, float inclinometer)
    {
        UpdateText(stackerArmRotationPoint, stackerArmRotationText, "���ϻ�ת��", stackerArmRotation, stackerArmRotationOffset);
        UpdateText(materialArmRotationPoint, materialArmRotationText, "ȡ�ϻ�ת��", materialArmRotation, materialArmRotationOffset);
        UpdateText(inclinometerPoint, inclinometerText, "ȡ�ϸ�����", inclinometer, inclinometerOffset);
    }

    void UpdateText(Transform point, TMP_Text text, string prefix, float angle, Vector2 offset)
    {
        Vector3 pointInCam = modelCam.WorldToViewportPoint(point.position);
        pointInCam += new Vector3(offset.x, offset.y);
        Ray ray = modelCam.ViewportPointToRay(pointInCam);
        text.text = prefix + angle.ToString("F2") + "��";
        text.transform.rotation = modelCam.transform.rotation;
        text.transform.position = modelCam.transform.position + ray.direction * 100;
    }
}