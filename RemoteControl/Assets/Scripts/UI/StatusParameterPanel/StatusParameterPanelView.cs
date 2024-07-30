using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public enum StatusParameterChildID
{
    State_1_1, State_1_2, State_1_3, State_2_1, State_2_2, State_2_3
}
public class StatusParameterPanelView : UIView<StatusParameterPanelCtr>
{
    /// <summary>
    /// 1#堆取料系统状态-off
    /// </summary>
    public Button stateBtn_1_1_off;
    /// <summary>
    /// 1#堆取料系统状态二off
    /// </summary>
    public Button stateBtn_1_2_off;
    /// <summary>
    /// 1#堆取料系统参数off
    /// </summary>
    public Button stateBtn_1_3_off;
    /// <summary>
    /// 2#堆取料系统状态一off
    /// </summary>
    public Button stateBtn_2_1_off;
    /// <summary>
    ///  2#堆取料系统状态二off
    /// </summary>
    public Button stateBtn_2_2_off;
    /// <summary>
    /// 2#堆取料系统参数off
    /// </summary>
    public Button stateBtn_2_3_off;
    /// <summary>
    /// 1#堆取料系统状态-on
    /// </summary>
    public Button stateBtn_1_1_on;
    /// <summary>
    /// 1#堆取料系统状态二on
    /// </summary>
    public Button stateBtn_1_2_on;
    /// <summary>
    /// 1#堆取料系统参数on
    /// </summary>
    public Button stateBtn_1_3_on;
    /// <summary>
    /// 2#堆取料系统状态一on
    /// </summary>
    public Button stateBtn_2_1_on;
    /// <summary>
    ///  2#堆取料系统状态二on
    /// </summary>
    public Button stateBtn_2_2_on;
    /// <summary>
    /// 2#堆取料系统参数on
    /// </summary>
    public Button stateBtn_2_3_on;

    public GameObject state_1_1;
    public GameObject state_1_2;
    public GameObject state_1_3;
    public GameObject state_2_1;
    public GameObject state_2_2;
    public GameObject state_2_3;

    public GameObject curOffBtn;
    public GameObject curOnBtn;


    public override void InitUIElements(UIArgs uiArgs = null)
    {
        stateBtn_1_1_off = RootObj.transform.FindComponent<Button>("btns/btn_1_1_off");
        stateBtn_1_2_off = RootObj.transform.FindComponent<Button>("btns/btn_1_2_off");
        stateBtn_1_3_off = RootObj.transform.FindComponent<Button>("btns/btn_1_3_off");
        stateBtn_2_1_off = RootObj.transform.FindComponent<Button>("btns/btn_2_1_off");
        stateBtn_2_2_off = RootObj.transform.FindComponent<Button>("btns/btn_2_2_off");
        stateBtn_2_3_off = RootObj.transform.FindComponent<Button>("btns/btn_2_3_off");

        stateBtn_1_1_on = RootObj.transform.FindComponent<Button>("btns/btn_1_1_on");
        stateBtn_1_2_on = RootObj.transform.FindComponent<Button>("btns/btn_1_2_on");
        stateBtn_1_3_on = RootObj.transform.FindComponent<Button>("btns/btn_1_3_on");
        stateBtn_2_1_on = RootObj.transform.FindComponent<Button>("btns/btn_2_1_on");
        stateBtn_2_2_on = RootObj.transform.FindComponent<Button>("btns/btn_2_2_on");
        stateBtn_2_3_on = RootObj.transform.FindComponent<Button>("btns/btn_2_3_on");

        stateBtn_1_1_off.onClick.AddListener(()=> ShowStatePane(StatusParameterChildID.State_1_1));
        stateBtn_1_2_off.onClick.AddListener(() => ShowStatePane(StatusParameterChildID.State_1_2));
        stateBtn_1_3_off.onClick.AddListener(() => ShowStatePane(StatusParameterChildID.State_1_3));
        stateBtn_2_1_off.onClick.AddListener(() => ShowStatePane(StatusParameterChildID.State_2_1));
        stateBtn_2_2_off.onClick.AddListener(() => ShowStatePane(StatusParameterChildID.State_2_2));
        stateBtn_2_3_off.onClick.AddListener(() => ShowStatePane(StatusParameterChildID.State_2_3));

        state_1_1 = RootObj.transform.Find("status_1_1").gameObject;
        state_1_2 = RootObj.transform.Find("status_1_2").gameObject;
        curOffBtn = stateBtn_1_1_off.gameObject;
        curOnBtn = stateBtn_1_1_on.gameObject;

    }

    public void ShowStatePane(StatusParameterChildID id) {

        Debug.Log($"状态参数打开 {id}");
        if (id == StatusParameterChildID.State_1_1)
        {
            RestCurBtn(stateBtn_1_1_off.gameObject,stateBtn_1_1_on.gameObject);
            state_1_1?.SetActive(true);
        }
        else if(id == StatusParameterChildID.State_1_2)
        {
            RestCurBtn(stateBtn_1_2_off.gameObject, stateBtn_1_2_on.gameObject);
        }
        else if( id == StatusParameterChildID.State_1_3)
        {
            RestCurBtn(stateBtn_1_3_off.gameObject, stateBtn_1_3_on.gameObject);
        }
        else if (id == StatusParameterChildID.State_2_1)
        {
            RestCurBtn(stateBtn_2_1_off.gameObject, stateBtn_2_1_on.gameObject);
        }
        else if (id == StatusParameterChildID.State_2_2){
            RestCurBtn(stateBtn_2_2_off.gameObject, stateBtn_2_2_on.gameObject);
        }
        else if (id == StatusParameterChildID.State_2_3)
        {
            RestCurBtn(stateBtn_2_3_off.gameObject, stateBtn_2_3_on.gameObject);
        }

    }
    public void RestCurBtn(GameObject offgo,GameObject ongo)
    {
        if (curOffBtn != null) {
            curOffBtn.SetActive(true);
        }
        if (curOnBtn != null) {
            curOnBtn.SetActive(false);
        }
        offgo.SetActive(false);
        ongo.SetActive(true);
        curOffBtn = offgo;
        curOnBtn = ongo;
    }
}
