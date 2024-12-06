using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using RemoteControl.Event;
using UnityEngine;


public partial class DataManager
{
    public List<AccountInfo> AccountInfos
    {
        get => GetAccountList();
    }
    public bool  CheckLoginInfo(string account, string password)
    {
        AccountInfo info = GetAccountInfo(account);
        if (info == null)
        {
            return false;
        }
        else
        {
            bool passVer = BCrypt.Net.BCrypt.Verify(password, info.password);
            if (passVer)
            {
                _currentAccount = info;
                GameDataManager.Instance.curAccountInfo = info;
                EventManager.Instance.TriggerEvent(EventName.UpdateAccountData,null);
                return true;
            } 
            else
            {
                return false;
            }
        }
    }
    public bool UpdatePassword(string account, string newPassword)
    {
        string query = $"UPDATE `{ConstStr.DATABASE_LOGIN_TABLE}` SET `{ConstStr.DATA_PASSWORD}` = '{newPassword}' WHERE `{ConstStr.DATA_USERNAME}` = '{account}'";
        return MySqlHelper.ExecuteSql(query) > 0;
    }
    public string GetDefaultPassword(string account)
    {
        // string query = $"SELECT `{ConstStr.DATA_DEFAULT_PASSWORD}` FROM `{ConstStr.DATABASE_LOGIN_TABLE}` WHERE `{ConstStr.DATA_USERNAME}` = \"{account}\"";
        // DataSet res = MySqlHelper.GetDataSet(query);
        // if (res != null)
        // {
        //     try
        //     {
        //         return res.Tables[0].Rows[0][ConstStr.DATA_DEFAULT_PASSWORD] as string;
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.LogException(e);
        //     }
        // }
        return null;
    }
    public bool ResetPassword(string account)
    {
        string defaultPassword = GetDefaultPassword(account);
        if (defaultPassword != null)
        {
            string setPasswordQuery = $"UPDATE `{ConstStr.DATABASE_LOGIN_TABLE}` SET `{ConstStr.DATA_PASSWORD}` = '{defaultPassword}' WHERE `{ConstStr.DATA_USERNAME}` = '{account}'";
            return MySqlHelper.ExecuteSql(setPasswordQuery) > 0;
        }
        return false;
    }
    public bool RemoveAccount(string account)
    {
        string query = $"DELETE FROM `{ConstStr.DATABASE_LOGIN_TABLE}` WHERE `{ConstStr.DATA_USERNAME}` = '{account}'";
        return MySqlHelper.ExecuteSql(query) > 0;
    }
    public bool InsertAccount(AccountInfo account)
    {
        return false;
        // string query = $"INSERT INTO `{ConstStr.DATABASE_LOGIN_TABLE}` (`{ConstStr.DATA_USERNAME}`, `{ConstStr.DATA_PASSWORD}`, `{ConstStr.DATA_IS_ADMIN}`, `{ConstStr.DATA_NAME}`, `{ConstStr.DATA_DEPARTMENT}`, `{ConstStr.DATA_JOB}`, `{ConstStr.DATA_DEFAULT_PASSWORD}`, `{ConstStr.DATA_INDEX}`) VALUES ('{account.account}', '{account.password}', 0, '{account.name}', '{account.department}', '{account.job}', '{account.defaultPassword}', {account.index})";
        // return MySqlHelper.ExecuteSql(query) > 0;
    }
    public bool UpdateAccount(AccountInfo account)
    {
        return false;
        // string query = $"UPDATE `{ConstStr.DATABASE_LOGIN_TABLE}` SET `{ConstStr.DATA_DEFAULT_PASSWORD}` = '{account.defaultPassword}', `{ConstStr.DATA_NAME}` = '{account.name}', `{ConstStr.DATA_DEPARTMENT}` = '{account.department}', `{ConstStr.DATA_JOB}` = '{account.job}', `{ConstStr.DATA_INDEX}` = {account.index} WHERE `{ConstStr.DATA_USERNAME}` = '{account.account}'";
        // return MySqlHelper.ExecuteSql(query) > 0;
    }
    public AccountInfo GetAccountInfo(string account)
    {
        string query = $"SELECT * FROM `{ConstStr.DATABASE_LOGIN_TABLE}` WHERE `{ConstStr.DATA_USERNAME}` = \"{account}\"";
        DataSet dataSet = MySqlHelper.GetDataSet(query);
        try
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            int val =int.Parse(row[ConstStr.DATA_IS_ADMIN].ToString());
            return new AccountInfo
            {
                account = row[ConstStr.DATA_USERNAME] as string,
                // defaultPassword =row[ConstStr.DATA_DEFAULT_PASSWORD] as string,
                password = row[ConstStr.DATA_PASSWORD] as string,
                // department = row[ConstStr.DATA_DEPARTMENT] as string,
                // job = row[ConstStr.DATA_JOB] as string,
                // index = (int)row[ConstStr.DATA_INDEX],
                name = row[ConstStr.DATA_NAME] as string,
                isAdmin =(val == 1)
            };
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }
    public List<AccountInfo> GetAccountList()
    {
        List<AccountInfo> res = new List<AccountInfo>();
        string query = $"SELECT * FROM `{ConstStr.DATABASE_LOGIN_TABLE}`";
        DataSet dataSet = MySqlHelper.GetDataSet(query);
        try
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                ulong val = (ulong)row[ConstStr.DATA_IS_ADMIN];
                res.Add(new AccountInfo
                {
                    account = row[ConstStr.DATA_USERNAME] as string,
                    // defaultPassword = row[ConstStr.DATA_DEFAULT_PASSWORD] as string,
                    password = row[ConstStr.DATA_PASSWORD] as string,
                    // department = row[ConstStr.DATA_DEPARTMENT] as string,
                    // job = row[ConstStr.DATA_JOB] as string,
                    // index = (int)row[ConstStr.DATA_INDEX],
                    name = row[ConstStr.DATA_NAME] as string,
                    isAdmin = (val == 1)
                });
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        return res;
    }
    public bool InsertHistoryTaskMc(TaskCommand taskCommand,string userName,string taskState,string completeState="1")
    {
        if (GameDataManager.Instance.IsAdmin())
        {
            string query = $"INSERT INTO {ConstStr.DATABASE_HISTORY_TASK_MC} (`{ConstStr.DATA_OPERATO_RSYSTEM}`,`{ConstStr.DATA_TASK_CREATE_TIME}`,`{ConstStr.DATA_MACHINE}`,`{ConstStr.DATA_TASK_TYPE}`,`{ConstStr.DATA_MATERIAL_RANGE_START}`,`{ConstStr.DATA_MATERIAL_RANGE_END}`,`{ConstStr.DATA_SIDE_SELECTION}`,`{ConstStr.DATA_LEFT_RIGHT_RANGE_START}`,`{ConstStr.DATA_LEFT_RIGHT_RANGE_END}`,`{ConstStr.DATA_STEP_LENGTH}`,`{ConstStr.DATA_IS_TIMED}`,`{ConstStr.DATA_TIMEDAT}`,`{ConstStr.DATA_IS_QUANTIFIED}`,`{ConstStr.DATA_QUANTITY}`,`{ConstStr.DATA_OPERATOR}`,`{ConstStr.DATA_TASK_STATE}`,`{ConstStr.DATA_TASK_ID}`,`{ConstStr.DATA_TASK_TAKE_MATE_HIGH}`,`{ConstStr.DATA_TASK_LAYER_HIGH}`,`{ConstStr.DATA_TASK_AUTO_MODE}`,`{ConstStr.DATA_TASK_ANGLE_ENTRY_MODE}`,`{ConstStr.DATA_TASK_STATE2}`) " +
                           $"VALUES ('{taskCommand.QuerySystem}','{taskCommand.TaskCreateTime}','{taskCommand.Machine}','{taskCommand.TaskType}','{taskCommand.MaterialRange.startValue}','{taskCommand.MaterialRange.endValue}','{taskCommand.SideSelection}','{taskCommand.LeftRightRange.startValue}','{taskCommand.LeftRightRange.endValue}','{taskCommand.StepLength}','{0}','{taskCommand.TimedAt}','{1}','{0}','{userName}','{taskState}','{taskCommand.TaskID}','{0}','{0}','{taskCommand.AutoMode}','{(int)taskCommand.AngleEntryMode}','{completeState}')";
            return MySqlHelper.ExecuteSql(query) > 0; ;
        }
        else
        {
            return false;
        }
       
    }
    public bool UpdateHistoryTaskMcCompleteState(string taskID,TaskStatus completeState,DateTime dateTime)
    {
        if (GameDataManager.Instance.IsAdmin())
        {
            string state = "0";
            if (completeState == TaskStatus.InProgress)
            {
                state = "1";
            }else if (completeState == TaskStatus.Completed)
            {
                state ="2";
            }
            string query = $"UPDATE `{ConstStr.DATABASE_HISTORY_TASK_MC}` SET `{ConstStr.DATA_TASK_STATE2}` = '{state}' , `{ConstStr.DATA_TASK_END_TIME}`='{dateTime}' WHERE `{ConstStr.DATA_TASK_ID}` = '{taskID}'";
            bool success=MySqlHelper.ExecuteSql(query) > 0; 
            return success; 
        }
        else
        {
            return false; 
        }
     
    }
    
    public bool UpdateHistoryTaskMc(string taskID,string taskState)
    {
        if (GameDataManager.Instance.IsAdmin())
        {
            string query = $"UPDATE {ConstStr.DATABASE_HISTORY_TASK_MC} SET {ConstStr.DATA_TASK_STATE} = {taskState} WHERE {ConstStr.DATA_TASK_ID} = {taskID}";
            bool success=MySqlHelper.ExecuteSql(query) > 0; 
            // Debug.LogError($"更新任务状态 { taskID } { state} {success}");
            return success; 
        }
        else
        {
            return false; 
        }
    
    }
    public MySqlDataReader GetHistoryTaskMc(int limit)
    {
        string query = "";
        if (limit == 0)
        {
            query= $"Select * from {ConstStr.DATABASE_HISTORY_TASK_MC}";

        }
        else
        {
            query = $"Select * from {ConstStr.DATABASE_HISTORY_TASK_MC} ORDER BY id DESC LIMIT {limit};";
        }
        MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(query);
        return mySqlDataReader;
    }

    public MySqlDataReader GetHistoryTaskMcBySql(string sql)
    {
        MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(sql);
        return mySqlDataReader;
    }

    public MySqlDataReader GetHistoryCartelectricity(string machine,int limit=100,bool isUseTime=false,string startTime="",string endTime="") {
        string query = "";
        if (isUseTime == false)
        {
            query= $"SELECT * FROM {ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC} WHERE {ConstStr.DATA_HISTORY_CARTELECTRICITY_MACHINE}={machine}  ORDER BY id DESC LIMIT {limit};";
        }
        else
        {
            query = $"SELECT * FROM {ConstStr.DATABASE_HISTORY_CARTELECTRICITY_MC} WHERE {ConstStr.DATA_HISTORY_CARTELECTRICITY_TIME} BETWEEN '{startTime}' AND '{endTime}' AND ({ConstStr.DATA_HISTORY_CARTELECTRICITY_MACHINE} = {machine}) ORDER BY id DESC LIMIT {limit}";
        }
        MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(query);
        return mySqlDataReader;
    }
    public MySqlDataReader GetHistoryChartData(string chartName, string machine, int limit = 100, bool isUseTime = false, string startTime = "", string endTime = "")
    {
        string query = "";
        if (isUseTime == false)
        {
            query = $"SELECT * FROM {chartName} WHERE {ConstStr.DATA_HISTORY_CARTELECTRICITY_MACHINE}={machine}  ORDER BY id DESC LIMIT {limit};";
        }
        else
        {
            query = $"SELECT * FROM {chartName} WHERE {ConstStr.DATA_HISTORY_CARTELECTRICITY_TIME} BETWEEN '{startTime}' AND '{endTime}' AND ({ConstStr.DATA_HISTORY_CARTELECTRICITY_MACHINE} = {machine}) ORDER BY id DESC LIMIT {limit}";
        }
        // Debug.Log($"命令 {query}");
        MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(query);
        return mySqlDataReader;
    }
    public MySqlDataReader GetHistoryChartData(string chartName, string machine, bool isUseTime = false, string startTime = "", string endTime = "")
    {
        string query = "";
        if (isUseTime == false)
        {
            query = $"SELECT * FROM {chartName} WHERE {ConstStr.DATA_HISTORY_CARTELECTRICITY_MACHINE}={machine}  ORDER BY id DESC;";
        }
        else
        {
            query = $"SELECT * FROM {chartName} WHERE {ConstStr.DATA_HISTORY_CARTELECTRICITY_TIME} BETWEEN '{startTime}' AND '{endTime}' AND ({ConstStr.DATA_HISTORY_CARTELECTRICITY_MACHINE} = {machine}) ORDER BY id DESC";
        }
        // Debug.Log($"命令 {query}");
        MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(query);
        return mySqlDataReader;
    }

    public DataSet GetHistoryChartDataSet(string chartName, string machine, int limit = 100, bool isUseTime = false, string startTime = "", string endTime = "")
    {
        string query = "";
        if (isUseTime == false)
        {
            query = $"SELECT * FROM {chartName} WHERE {ConstStr.DATA_HISTORY_CARTELECTRICITY_MACHINE}={machine}  ORDER BY id DESC LIMIT {limit};";
        }
        else
        {
            query = $"SELECT * FROM {chartName} WHERE {ConstStr.DATA_HISTORY_CARTELECTRICITY_TIME} BETWEEN '{startTime}' AND '{endTime}' AND ({ConstStr.DATA_HISTORY_CARTELECTRICITY_MACHINE} = {machine}) ORDER BY id DESC LIMIT {limit}";
        }
        // Debug.Log($"命令 {query}");
        DataSet dataSet = MySqlHelper.GetDataSet(query);
        return  dataSet;
    }
    
    public DataSet GetHistoryChartDataSet(string chartName, string machine, bool isUseTime = false, string startTime = "", string endTime = "")
    {
        string query = "";
        if (isUseTime == false)
        {
            query = $"SELECT * FROM {chartName} WHERE {ConstStr.DATA_HISTORY_CARTELECTRICITY_MACHINE}={machine}  ORDER BY id DESC;";
        }
        else
        {
            query = $"SELECT * FROM {chartName} WHERE {ConstStr.DATA_HISTORY_CARTELECTRICITY_TIME} BETWEEN '{startTime}' AND '{endTime}' AND ({ConstStr.DATA_HISTORY_CARTELECTRICITY_MACHINE} = {machine}) ORDER BY id DESC";
        }
        DataSet dataSet = MySqlHelper.GetDataSet(query);
        return  dataSet;
    }
    public bool InsertHistoryChartData(string tabName,float value,string des,Machine machine)
    {
        int id =machine==(int)Machine.BucketWheelStackerReclaimer?0:1;
        string query = $"INSERT INTO {tabName} (`{ConstStr.DATA_HISTORY_CARTELECTRICITY_NAME}`,`{ConstStr.DATA_HISTORY_CARTELECTRICITY_MACHINE}`,`{ConstStr.DATA_HISTORY_CARTELECTRICITY_TIME}`,`{ConstStr.DATA_HISTORY_CARTELECTRICITY_VALUE}`) " +
                       $"VALUES ('{des}','{id}','{DateTime.Now}','{value}')";
        return MySqlHelper.ExecuteSql(query) > 0;
    }
    public bool InsertHistoryLogMc(string des,string userName,Machine machine)
    {
        string tabName =machine==Machine.BucketWheelStackerReclaimer?ConstStr.DATABASE_HISTORY_LOG1_MC:ConstStr.DATABASE_HISTORY_LOG2_MC;
        
        string query = $"INSERT INTO {tabName} (`{ConstStr.DATA_HISTORY_LOGS_TIME}`,`{ConstStr.DATA_HISTORY_LOGS_INFO}`,`{ConstStr.DATA_HISTORY_LOGS_OPERATOR}`) " +
                       $"VALUES ('{DateTime.Now}','{des}','{userName}')";
        return MySqlHelper.ExecuteSql(query) > 0;
    }
    
    public bool InsertHistoryWarningMc(string des,string userName,Machine machine,bool isRecord=false)
    {
        if (GameDataManager.Instance.IsAdmin()||isRecord)
        {
            string tabName =machine==Machine.BucketWheelStackerReclaimer?ConstStr.DATABASE_HISTORY_WARNING1_MC:ConstStr.DATABASE_HISTORY_WARNING2_MC;
        
            string query = $"INSERT INTO {tabName} (`{ConstStr.DATA_HISTORY_WARNING_TIME}`,`{ConstStr.DATA_HISTORY_WARNING_INFO}`,`{ConstStr.DATA_HISTORY_WARNING_OPERATOR}`) " +
                           $"VALUES ('{DateTime.Now}','{des}','{userName}')";
            return MySqlHelper.ExecuteSql(query) > 0;
        }
        else
        {
            return false;
        }
      
    }
    
    public MySqlDataReader GetTaskConfigMc(Machine machine)
    {
        int id = machine == Machine.BucketWheelStackerReclaimer ? 1 : 2;
        string tabName = ConstStr.DATABASE_TASK_CONFIG;
        string query = $"SELECT * FROM {tabName} WHERE {ConstStr.DATA_TASK_CONFIG_ID}={id}";
        MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(query);
        return mySqlDataReader;
    }
    public bool UpdateTaskConfig(string name,string value,Machine machine)
    {
        int id = machine == Machine.BucketWheelStackerReclaimer ? 1 : 2;
        string query = $"UPDATE {ConstStr.DATABASE_TASK_CONFIG} SET {name} = {value} WHERE {ConstStr.DATA_TASK_CONFIG_ID} = {id}";
        bool success=MySqlHelper.ExecuteSql(query) > 0; 
        return success; 
    }

    public bool DeleTabData(string tabName,int month=6)
    {
        string query = $"DELETE FROM {tabName} WHERE time < DATE_SUB(CURRENT_DATE, INTERVAL {month} MONTH)";
        return MySqlHelper.ExecuteSql(query) > 0;
    }
    
}