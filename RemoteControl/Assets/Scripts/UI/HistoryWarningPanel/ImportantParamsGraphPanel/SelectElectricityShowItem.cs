using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectElectricityShowItem : MonoBehaviour
{
    public Toggle BucketWheelToggle;
    public Toggle TrolleyToggle;
    public Toggle SlewingToggle;
    public Toggle SuspendedGelToggle;
    public Action<bool>BucketWheelToggleAction;
    public Action<bool>TrolleyToggleAction;
    public Action<bool>SlewingToggleAction;
    public Action<bool>SuspendedGelToggleAction;

    private void Awake()
    {
        BucketWheelToggle.onValueChanged.AddListener((arg0 =>
        {
            BucketWheelToggleAction?.Invoke(arg0);
        } ));
        TrolleyToggle.onValueChanged.AddListener((arg0 =>
        {
            TrolleyToggleAction?.Invoke(arg0);
        } ));
        SlewingToggle.onValueChanged.AddListener((arg0 =>
        {
            SlewingToggleAction?.Invoke(arg0);
        } ));
        SuspendedGelToggle.onValueChanged.AddListener((arg0 =>
        {
            SuspendedGelToggleAction?.Invoke(arg0);
        } ));
    }
}
