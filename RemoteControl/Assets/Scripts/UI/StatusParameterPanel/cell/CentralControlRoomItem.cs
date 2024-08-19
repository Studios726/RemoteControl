using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 与中控室连锁
/// </summary>
public class CentralControlRoomItem : StatusParmItemBase<CentralControlData>
{
    public ToggleDIY UnlockToggle;
    public ToggleDIY LockToggle;
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
