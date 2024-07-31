using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct ShuntPlateData
{
    /// <summary>
    /// ����·�� 
    /// </summary>
    public bool isMainCircuitBreaker;
    /// <summary>
    /// ������� 
    /// </summary>
    public bool isMotorOverload;
    /// <summary>
    /// ̧����λ 
    /// </summary>
    public bool isLiftLimit;
    /// <summary>
    ///  ������λ 
    /// </summary>
    public bool isLowerLimit;
    /// <summary>
    /// ̧������ 
    /// </summary>
    public bool isLiftRunning;
    /// <summary>
    /// ��������
    /// </summary>
    public bool isLowerRunning;
}
public class ShuntPlateItem :StatusParmItemBase<ShuntPlateData>
{
    /// <summary>
    /// ����·�� 
    /// </summary>
    public Toggle MainCircuitBreaker;
    /// <summary>
    /// ������� 
    /// </summary>
    public Toggle MotorOverload;
    /// <summary>
    /// ̧����λ 
    /// </summary>
    public Toggle LiftLimit;
    /// <summary>
    ///  ������λ 
    /// </summary>
    public Toggle LowerLimit;
    /// <summary>
    /// ̧������ 
    /// </summary>
    public Toggle LiftRunning;
    /// <summary>
    /// ��������
    /// </summary>
    public Toggle LowerRunning;
    public override void UpdateData(ShuntPlateData data)
    {
        SetToggleState(MainCircuitBreaker, data.isMainCircuitBreaker);
        SetToggleState(MotorOverload, data.isMotorOverload);
        SetToggleState(LiftLimit, data.isLiftLimit);
        SetToggleState(LowerLimit, data.isLowerLimit);
        SetToggleState(LiftRunning, data.isLiftRunning);
        SetToggleState(LowerRunning, data.isLowerRunning);
    }
}
