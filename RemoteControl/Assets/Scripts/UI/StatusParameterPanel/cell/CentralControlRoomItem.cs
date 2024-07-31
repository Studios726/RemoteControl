using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ”Î÷–øÿ “¡¨À¯
/// </summary>
public class CentralControlRoomItem : StatusParmItemBase<CentralControlData>
{
    public Toggle UnlockToggle;
    public Toggle LockToggle;
    public override void UpdateData(CentralControlData centralControlData)
    {
        SetToggleState(UnlockToggle, centralControlData.isUnlock);
        SetToggleState(LockToggle, centralControlData.isLock);
    }
}
public struct CentralControlData
{
    public bool isUnlock;
    public bool isLock;
}
