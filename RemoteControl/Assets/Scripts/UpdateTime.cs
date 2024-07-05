using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateTime : MonoBehaviour
{
    readonly string[] Day = new string[] { "������", "����һ", "���ڶ�", "������", "������", "������", "������" };
    private TMP_Text _time;

    // Start is called before the first frame update
    void Start()
    {
        _time = this.gameObject.transform.GetComponent<TMP_Text>();
        _time.transform.localPosition += new Vector3(200f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //ʱ�任�в��ƶ� ljz
        _time.text = DateTime.Now.ToString("yyyy��MM��dd��\n");
        string week = Day[Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"))].ToString();
        _time.text += week;
        _time.text += DateTime.Now.ToString("HH:mm:ss");
    }
}
