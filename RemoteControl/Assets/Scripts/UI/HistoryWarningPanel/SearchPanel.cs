using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.UI;

public enum TimeType
{
    None=0,
    StartTime=1,
    EndTime=2,
}

public enum MechanicalType
{
    Reclaimer=0,
    StackerReclaimer=1
    
}
public class SearchPanel : MonoBehaviour
{
    public Button startYBtn;
    public Button startYBtn2;
    public Text startYText;

    public Button startMBtn;
    public Button startMBtn2;
    public Text startMText;
    
    public Button startDBtn;
    public Button startDBtn2;
    public Text startDText;
    
    public Button endYBtn;
    public Button endYBtn2;
    public Text endYText;

    public Button endMBtn;
    public Button endMBtn2;
    public Text endMText;
    
    public Button endDBtn;
    public Button endDBtn2;
    public Text endDText;

    public Button searchBtn;

    public GameObject scrollviewY ;
    public GameObject scrollviewM ;
    public GameObject scrollviewD ;
    
    public Transform contentY;
    public Transform contentM;
    public Transform contentD;

    public GameObject searchItme;

    public MechanicalType mechanicalType;
    private TimeType timeType = TimeType.None;
    [HideInInspector]
    public bool isUpdateTime;
    [HideInInspector]
    public int firstLogintimestamp;

    private Vector3 scrollviewYPos;
    private Vector3 scrollviewMPos;
    private Vector3 scrollviewDPos;

    private Vector3 offset = new Vector3(0, -6, 0);

    public string Tables = "warning";
    private MySqlDataReader _dataReader = null;
    private Coroutine _readingCoroutine = null;

    private HistoryPanelCtr _historyPanelCtr;
    // Start is called before the first frame update
    void Start()
    {
        scrollviewYPos = scrollviewY.transform.position;
        scrollviewMPos = scrollviewM.transform.position;
        scrollviewDPos = scrollviewD.transform.position;
        startYBtn.onClick.AddListener((() =>
        {
            ResetDateUI();
            SetActive(startYBtn.gameObject,false);
            SetActive(startYBtn2.gameObject,true);
            timeType = TimeType.StartTime;
            ShowScrollView(scrollviewY,scrollviewYPos);
            RefreshScrollviewYData();
        }));
        endYBtn.onClick.AddListener((() =>
        {
            ResetDateUI();
            SetActive(endYBtn.gameObject,false);
            SetActive(endYBtn2.gameObject,true);
            timeType = TimeType.EndTime;
            ShowScrollView(scrollviewY,scrollviewYPos);
            RefreshScrollviewYData();
        }));
        
        startMBtn.onClick.AddListener((() =>
        {
            ResetDateUI();
            SetActive(startMBtn.gameObject,false);
            SetActive(startMBtn2.gameObject,true);
            timeType = TimeType.StartTime;
            ShowScrollView(scrollviewM,scrollviewMPos);
            RefreshScrollViewMData();
        }));
        endMBtn.onClick.AddListener((() =>
        {
            ResetDateUI();
            SetActive(endMBtn.gameObject,false);
            SetActive(endMBtn2.gameObject,true);
            timeType = TimeType.EndTime;
            ShowScrollView(scrollviewM,scrollviewMPos);
            RefreshScrollViewMData();
        }));
        
        startDBtn.onClick.AddListener((() =>
        {
            ResetDateUI();
            timeType = TimeType.StartTime;
            if (startMText.text=="" || startYText.text=="")
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("请确认年份和月份填写正确",null,null));
                return;
            }
            SetActive(startDBtn.gameObject,false);
            SetActive(startDBtn2.gameObject,true);
            ShowScrollView(scrollviewD,scrollviewDPos);
            RefreshScrollViewDData();
        }));
        endDBtn.onClick.AddListener((() =>
        {
            ResetDateUI();
            timeType = TimeType.EndTime;
            if (endMText.text=="" || endYText.text=="")
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("请确认年份和月份填写正确",null,null));
                return;
            }
            SetActive(endDBtn.gameObject,false);
            SetActive(endDBtn2.gameObject,true);
            ShowScrollView(scrollviewD,scrollviewDPos);
            RefreshScrollViewDData();
        }));
        searchBtn.onClick.AddListener(SearchHistroyRecord);
    }

    public void SetHistoryPanel(HistoryPanelCtr ctr)
    {
        _historyPanelCtr = ctr;
    }
    void RefreshScrollviewYData()
    {
        if (isUpdateTime==false)
        {
            DateTime dateTime = DateTime.Now;// GetDateByTimestamp((int)DateTime.Now.ToUniversalTime().Ticks);
            int count = DateTime.Now.Year - dateTime.Year + 1;
            int childCount = contentY.transform.childCount;
            Debug.Log(count+":  "+"contentY.transform.childCount "+ contentY.transform.childCount);
            if (count>childCount)
            {
                for (int i = 0; i < count-childCount+10; i++)
                {
                    GameObject obj =Instantiate(searchItme, contentY);
                    obj.transform.Find("Text").GetComponent<Text>().text = (dateTime.Year+childCount + i).ToString();
                    obj.GetComponent<Button>().onClick.AddListener((() =>
                    {
                        SetActive(scrollviewY, false);
                        if (timeType==TimeType.StartTime)
                        {
                            startYBtn2.onClick.Invoke();
                            startYText.text =obj.transform.Find("Text").GetComponent<Text>().text;
                        }
                        else
                        {
                            endYBtn2.onClick.Invoke();
                            endYText.text=obj.transform.Find("Text").GetComponent<Text>().text;
                        }
                    }));
                }
            }
        }
    }

    void RefreshScrollViewMData()
    {
        if (contentM.transform.childCount==0)
        {
            for (int i = 0; i < 12; i++)
            {
                GameObject obj =Instantiate(searchItme, contentM);
                obj.transform.Find("Text").GetComponent<Text>().text = (i + 1).ToString();
                obj.GetComponent<Button>().onClick.AddListener((() =>
                {
                    SetActive(scrollviewM, false);
                    if (timeType==TimeType.StartTime)
                    {
                        startMBtn2.onClick.Invoke();
                        startMText.text =obj.transform.Find("Text").GetComponent<Text>().text;
                    }
                    else
                    {
                        endMBtn2.onClick.Invoke();
                        endMText.text=obj.transform.Find("Text").GetComponent<Text>().text;
                    }
                }));
            }
        }
    }

    void RefreshScrollViewDData()
    {
        int days = 30;
        if (timeType==TimeType.StartTime)
        {
            days = DateTime.DaysInMonth(int.Parse(startYText.text),int.Parse(startMText.text));
        }
        else
        {
            days = DateTime.DaysInMonth(int.Parse(endYText.text),int.Parse(endMText.text));
        }

        int childCount = contentD.transform.childCount;
        if (childCount<days)
        {
            for (int i = 0; i < days-childCount; i++)
            {
                GameObject obj =Instantiate(searchItme, contentD);
                obj.transform.Find("Text").GetComponent<Text>().text = (childCount +i+ 1).ToString();
                obj.GetComponent<Button>().onClick.AddListener((() =>
                {
                    SetActive(scrollviewD, false);
                    if (timeType==TimeType.StartTime)
                    {
                        startDBtn2.onClick.Invoke();
                        startDText.text =obj.transform.Find("Text").GetComponent<Text>().text;
                    }
                    else
                    {
                        endDBtn2.onClick.Invoke();
                        endDText.text=obj.transform.Find("Text").GetComponent<Text>().text;
                    }
                }));
            }
        }
        else if( childCount>days)
        {
            for (int i = 0; i < childCount-days; i++)
            {
                contentD.transform.GetChild(days + i).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < days; i++)
        {
            contentD.transform.GetChild( i).gameObject.SetActive(true);
        }
    }

    void ShowScrollView(GameObject scrollView,Vector3 pos)
    {
        if (timeType==TimeType.StartTime)
        {
            scrollView.transform.position = pos;
        }
        else
        {
            scrollView.transform.position = pos + offset;
        }
      
        SetActive(scrollView, true);
    }

    public void SetActive(GameObject go, bool isActive)
    {
        go.SetActive(isActive);
    }

    void ResetDateUI()
    {
        SetActive(scrollviewY, false);
        SetActive(scrollviewM, false);
        SetActive(scrollviewD, false);
        
        SetActive(startYBtn.gameObject,true);
        SetActive(startYBtn2.gameObject,false);
        
        SetActive(startDBtn.gameObject,true);
        SetActive(startDBtn2.gameObject,false);
        
        SetActive(startMBtn.gameObject,true);
        SetActive(startMBtn2.gameObject,false);
        
        SetActive(endYBtn.gameObject,true);
        SetActive(endYBtn2.gameObject,false);
        
        SetActive(endDBtn.gameObject,true);
        SetActive(endDBtn2.gameObject,false);
        
        SetActive(endMBtn.gameObject,true);
        SetActive(endMBtn2.gameObject,false);
    }

    public void SearchHistroyRecord()
    {
        Debug.Log("SearchHistroyRecord");
        string OperatorPerson = "管理员";
        string startTime = null, endTime = null;
        
        bool useDate = true, useOperator =OperatorPerson != string.Empty;
        
        try
        {
            int startyear = Convert.ToInt32(startYText.text);
            int startmouth = Convert.ToInt32(startMText.text);
            int startday = Convert.ToInt32(startDText.text);
            int endyear = Convert.ToInt32(endYText.text);
            int endmouth = Convert.ToInt32(endMText.text);
            int endday = Convert.ToInt32(endDText.text);
        
            startTime = startyear + "-" + startmouth + "-" + startday + " 00:00:00";
            endTime = endyear + "-" + endmouth + "-" + endday + " 23:59:59";
            long startTimeStamp = new DateTime(startyear, startmouth, startday).ToFileTime();
            long endTimeStamp = new DateTime(endyear, endmouth, endday).ToFileTime();
            if (startTimeStamp>endTimeStamp)
            {
                UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("结束日期小于开始日期，请重新设置"));
                return;
            }
        }
        catch
        {
            Debug.Log("日期无效");
            useDate = false;
            UIManager.Instance.OpenUI(UIID.ConfirmPanel,new ConfirmPanelArgs("日期无效，请重新检查日期，稍后查询"));
            return;
        }
        Debug.LogError("8888888888888888");
        _historyPanelCtr?.SearchRecord(startTime, endTime, mechanicalType);
        //
        // string sql = $"SELECT * FROM {Tables} WHERE ";
        //
        // if (!useDate && !useOperator)
        // {
        //     sql = "Select * from " + Tables + " ORDER BY id DESC LIMIT 50;";
        // }
        // else if (useDate && useOperator)
        // {
        //     sql += $"`operator` = '{OperatorPerson}' AND `time` BETWEEN '{startTime}' AND '{endTime}' ORDER BY `id` DESC;";
        // }
        // else if (useDate)
        // {
        //     sql += $"`time` BETWEEN '{startTime}' AND '{endTime}' ORDER BY `id` DESC;";
        // }
        // else
        // {
        //     sql += $"`operator` = '{OperatorPerson}' ORDER BY `id` DESC;";
        // }
        //
        // if (_dataReader != null)
        // {
        //     StopCoroutine(_readingCoroutine);
        //     _dataReader.Close();
        // }
        // Debug.LogError($"sql={sql}");
        // _dataReader = MySqlHelper.ExecuteReader(sql);
        // Reader();
    }
    // private void Reader()
    // {
    //     int counter = 0;
    //     while(_dataReader.Read())
    //     {
    //         string _id = _dataReader[0].ToString();
    //         string _time = _dataReader[1].ToString();
    //         string _info = _dataReader[2].ToString();
    //         string _operatorname = _dataReader[3].ToString();
    //         Debug.Log($"{_id}---{_time}---{_info}---{_operatorname}");
    //         ++counter;
    //         if (counter == 1000)
    //         {
    //             counter = 0;
    //             break;
    //         }
    //     }
    //     _dataReader.Close();
    // }
}
