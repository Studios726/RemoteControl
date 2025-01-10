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
    public ToggleDIY localUnlockToggle;
    public ToggleDIY localLockToggle;
    public override void UpdateData(CentralControlData centralControlData,bool isConnect=false)
    {
        SetToggleState(UnlockToggle, centralControlData.isUnlock,false,isConnect);
        SetToggleState(LockToggle, centralControlData.isLock,false,isConnect);
        SetToggleState(localUnlockToggle, centralControlData.islocalUnlock,false,isConnect);
        SetToggleState(localLockToggle, centralControlData.islcoalLock,false,isConnect);
    }
}
public struct CentralControlData
{
    public bool isUnlock;
    public bool isLock;
    public bool islocalUnlock;
    public bool islcoalLock;
}
