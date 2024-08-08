using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Search;
using UnityEngine;
using Utility.DesignPatterns;

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
            if (info.password.Equals(password))
            {
                _currentAccount = info;
                GameDataManager.Instance.curAccountInfo = info;
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
        string query = $"SELECT `{ConstStr.DATA_DEFAULT_PASSWORD}` FROM `{ConstStr.DATABASE_LOGIN_TABLE}` WHERE `{ConstStr.DATA_USERNAME}` = \"{account}\"";
        DataSet res = MySqlHelper.GetDataSet(query);
        if (res != null)
        {
            try
            {
                return res.Tables[0].Rows[0][ConstStr.DATA_DEFAULT_PASSWORD] as string;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
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
        string query = $"INSERT INTO `{ConstStr.DATABASE_LOGIN_TABLE}` (`{ConstStr.DATA_USERNAME}`, `{ConstStr.DATA_PASSWORD}`, `{ConstStr.DATA_IS_ADMIN}`, `{ConstStr.DATA_NAME}`, `{ConstStr.DATA_DEPARTMENT}`, `{ConstStr.DATA_JOB}`, `{ConstStr.DATA_DEFAULT_PASSWORD}`, `{ConstStr.DATA_INDEX}`) VALUES ('{account.account}', '{account.password}', 0, '{account.name}', '{account.department}', '{account.job}', '{account.defaultPassword}', {account.index})";
        return MySqlHelper.ExecuteSql(query) > 0;
    }
    public bool UpdateAccount(AccountInfo account)
    {
        string query = $"UPDATE `{ConstStr.DATABASE_LOGIN_TABLE}` SET `{ConstStr.DATA_DEFAULT_PASSWORD}` = '{account.defaultPassword}', `{ConstStr.DATA_NAME}` = '{account.name}', `{ConstStr.DATA_DEPARTMENT}` = '{account.department}', `{ConstStr.DATA_JOB}` = '{account.job}', `{ConstStr.DATA_INDEX}` = {account.index} WHERE `{ConstStr.DATA_USERNAME}` = '{account.account}'";
        return MySqlHelper.ExecuteSql(query) > 0;
    }
    public AccountInfo GetAccountInfo(string account)
    {
        string query = $"SELECT * FROM `{ConstStr.DATABASE_LOGIN_TABLE}` WHERE `{ConstStr.DATA_USERNAME}` = \"{account}\"";
        DataSet dataSet = MySqlHelper.GetDataSet(query);
        try
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            ulong val = (ulong)row[ConstStr.DATA_IS_ADMIN];
            return new AccountInfo
            {
                account = row[ConstStr.DATA_USERNAME] as string,
                defaultPassword =row[ConstStr.DATA_DEFAULT_PASSWORD] as string,
                password = row[ConstStr.DATA_PASSWORD] as string,
                department = row[ConstStr.DATA_DEPARTMENT] as string,
                job = row[ConstStr.DATA_JOB] as string,
                index = (int)row[ConstStr.DATA_INDEX],
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
                    defaultPassword = row[ConstStr.DATA_DEFAULT_PASSWORD] as string,
                    password = row[ConstStr.DATA_PASSWORD] as string,
                    department = row[ConstStr.DATA_DEPARTMENT] as string,
                    job = row[ConstStr.DATA_JOB] as string,
                    index = (int)row[ConstStr.DATA_INDEX],
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
    public bool InsertHistoryTaskMc(TaskCommand taskCommand,string userName,string state)
    {
        string query = $"INSERT INTO {ConstStr.DATABASE_HISTORY_TASK_MC} (`{ConstStr.DATA_OPERATO_RSYSTEM}`,`{ConstStr.DATA_TASK_CREATE_TIME}`,`{ConstStr.DATA_MACHINE}`,`{ConstStr.DATA_TASK_TYPE}`,`{ConstStr.DATA_MATERIAL_RANGE_START}`,`{ConstStr.DATA_MATERIAL_RANGE_END}`,`{ConstStr.DATA_SIDE_SELECTION}`,`{ConstStr.DATA_LEFT_RIGHT_RANGE_START}`,`{ConstStr.DATA_LEFT_RIGHT_RANGE_END}`,`{ConstStr.DATA_STEP_LENGTH}`,`{ConstStr.DATA_IS_TIMED}`,`{ConstStr.DATA_TIMEDAT}`,`{ConstStr.DATA_IS_QUANTIFIED}`,`{ConstStr.DATA_QUANTITY}`,`{ConstStr.DATA_OPERATOR}`,`{ConstStr.DATA_TASK_STATE}`,`{ConstStr.DATA_TASK_ID}`) " +
           $"VALUES ('{taskCommand.QuerySystem}','{DateTime.Now}','{taskCommand.Machine.ToString()}','{taskCommand.TaskType.ToString()}','{taskCommand.MaterialRange.startValue}','{taskCommand.MaterialRange.endValue}','{taskCommand.SideSelection}','{taskCommand.LeftRightRange.startValue}','{taskCommand.LeftRightRange.endValue}','{taskCommand.StepLength}','{0}','{taskCommand.TimedAt}','{1}','{taskCommand.Quantity}','{userName}','{state}','{taskCommand.TaskID}')";
        return MySqlHelper.ExecuteSql(query) > 0; ;
    }
    public bool UpdateHistoryTaskMc(string taskID,string state)
    {
        string query = $"UPDATE {ConstStr.DATABASE_HISTORY_TASK_MC} SET {ConstStr.DATA_TASK_STATE} = {state} WHERE {ConstStr.DATA_TASK_ID} = {taskID}";
        return MySqlHelper.ExecuteSql(query) > 0; 
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

}