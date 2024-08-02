using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    public Button sendBtn;
    public Button mysqlBtn;
    void Start()
    {
        sendBtn.onClick.AddListener(SendCommond);
        mysqlBtn.onClick.AddListener((() =>
        {
            AccountInfo accountInfo = new AccountInfo
            {
                index = 2,
                account ="9999",
                name = "gyk22a",
                department = "国航asdga",
                job = "搬运工dd",
                password = "9999",
                defaultPassword = "9999"
            };
            if (DataManager.Instance.InsertAccount(accountInfo))
            {
                UnityEngine.Debug.Log("插入成功");
            }
            else
            {
                UnityEngine.Debug.Log("插入失败");
            }
        }));
    }

    public void SendCommond()
    {
        ServerCommand cmd = SetCommandPlc(6,1,"",0,0);
        MessageCenter.Instance.SendMessage(MessageType.RC, cmd);
    }
    private ServerCommand SetCommandPlc(int TYPE1, int TYPE2, string TYPE3, int TYPE4, float TYPE5)
    {
        ServerCommand command = new ServerCommand();
        command.QUERY_SYSTEM = "MC";
        command.DATA_TYPE = TYPE1;
        command.QUERY_TYPE = TYPE2;
        command.COMMAND_NAME = TYPE3;
        //command.COMMAND_TYPE = TYPE4;
        //command.COMMAND_DATA = TYPE5;
        return command;
    }
    public void Test(string a)
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
