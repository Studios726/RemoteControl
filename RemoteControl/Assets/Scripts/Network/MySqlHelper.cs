using System;
using System.Data;
using System.Threading.Tasks;
using UnityEngine;
using MySql.Data.MySqlClient;

public class MySqlHelper
{
    public static string IP = GameDataManager.Instance.IpConfig.DataIP;
    public static string Database = "huarun2";//"huarun";
    public static string Username = "root";
    public static string Password = "123456";
    public static string connstr = "server=" + IP + ";database= " + Database + ";username=" + Username + ";password=" + Password + ";ConnectionTimeout=3;Charset=utf8";
    public static int count;
    private static MySqlConnection connectionReader;
    #region 执行查询语句，返回MySqlDataReader

    /// <summary>
    /// 执行查询语句，返回MySqlDataReader
    /// </summary>
    /// <param name="sqlString"></param>
    /// <returns></returns>
    public static MySqlDataReader ExecuteReader(string sqlString)
    {  
        //临时处理 避免重复创建hard code
        if (connectionReader==null)
        {
            connectionReader = new MySqlConnection(connstr);
        }
        connectionReader.Close();
        count--;
        MySqlCommand cmd = new MySqlCommand(sqlString, connectionReader);
        MySqlDataReader myReader = null;
        try
        {
            connectionReader.Open();
            count++;
            myReader=cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return myReader;
        }
        catch (MySql.Data.MySqlClient.MySqlException e)
        {
            connectionReader.Close();
            Debug.Log($"读取数据库连接错误{DateTime.Now.ToString("g")} {e.Message} {sqlString}");
        }
        finally
        {
            if (myReader == null)
            {
                cmd.Dispose();
                connectionReader.Close();
                connectionReader.Dispose();
                connectionReader = null;
            }
        }
        return myReader;
    }
    #endregion

    #region 执行带参数的查询语句，返回 MySqlDataReader

    /// <summary>
    /// 执行带参数的查询语句，返回MySqlDataReader
    /// </summary>
    /// <param name="sqlString"></param>
    /// <param name="cmdParms"></param>
    /// <returns></returns>
    public static MySqlDataReader ExecuteReader(string sqlString, params MySqlParameter[] cmdParms)
    {
        MySqlConnection connection = new MySqlConnection(connstr);
        MySqlCommand cmd = new MySqlCommand();
        MySqlDataReader myReader = null;
        try
        {
            PrepareCommand(cmd, connection, null, sqlString, cmdParms);
            myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return myReader;
        }
        catch (MySql.Data.MySqlClient.MySqlException e)
        {
            connection.Close();
            throw new Exception(e.Message);
        }
        finally
        {
            if (myReader == null)
            {
                cmd.Dispose();
                connection.Close();
            }
        }
    }
    #endregion

    #region 执行sql语句,返回执行行数

    /// <summary>
    /// 执行sql语句,返回执行行数
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public static int ExecuteSql(string sql)
    {
        ExecuteSqlAsync(sql);
        return 1;
    }
    
    public static async void ExecuteSqlAsync(string sql)
    {
        using (MySqlConnection conn = new MySqlConnection(connstr))
        {
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                try
                {
                    await Task.Run((() =>
                    {
                        conn.Open();
                        count++;
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        conn.Dispose();
                    }));
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    conn.Close();
                    Debug.Log($"异步写入数据库连接错误{DateTime.Now.ToString("g")} {e.Message} {sql}");
                }
                finally
                {
                   cmd.Dispose();
                   conn.Close();
                   conn.Dispose();
                   count--;
                }
            }
        }
    }
    #endregion

    #region 执行带参数的sql语句，并返回执行行数

    /// <summary>
    /// 执行带参数的sql语句，并返回执行行数
    /// </summary>
    /// <param name="sqlString"></param>
    /// <param name="cmdParms"></param>
    /// <returns></returns>
    public static int ExecuteSql(string sqlString, params MySqlParameter[] cmdParms)
    {
        using (MySqlConnection connection = new MySqlConnection(connstr))
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                try
                {
                    PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return rows;
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
    }
    #endregion

    #region 执行查询语句，返回DataSet

    /// <summary>
    /// 执行查询语句，返回DataSet
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public static DataSet GetDataSet(string sql)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                conn.Open();
                count++;
                using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd))
                using (DataSet ds = new DataSet())
                {
                    dataAdapter.Fill(ds);
                    conn.Close();
                    conn.Dispose();
                    count--;
                    return ds;
                }
                conn.Close();
                conn.Dispose();
                count--;
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"读取数据库连接错误: {DateTime.Now.ToString("g")} {ex.Message}\nSQL: {sql}\n连接字符串: {connstr}");
            return null;
        }
    }
    #endregion

    #region 执行带参数的查询语句，返回DataSet

    /// <summary>
    /// 执行带参数的查询语句，返回DataSet
    /// </summary>
    /// <param name="sqlString"></param>
    /// <param name="cmdParms"></param>
    /// <returns></returns>
    public static DataSet GetDataSet(string sqlString, params MySqlParameter[] cmdParms)
    {
        using (MySqlConnection connection = new MySqlConnection(connstr))
        {
            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(cmd, connection, null, sqlString, cmdParms);
            using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                try
                {
                    da.Fill(ds, "ds");
                    cmd.Parameters.Clear();
                }
                catch (MySql.Data.MySqlClient.MySqlException  ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
                return ds;
            }
        }
    }
    #endregion

    #region 执行带参数的sql语句，并返回 object

    /// <summary>
    /// 执行带参数的sql语句，并返回object
    /// </summary>
    /// <param name="sqlString"></param>
    /// <param name="cmdParms"></param>
    /// <returns></returns>
    public static object GetSingle(string sqlString, params MySqlParameter[] cmdParms)
    {
        using (MySqlConnection connection = new MySqlConnection(connstr))
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                try
                {
                    PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if ((System.Object.Equals(obj, null)) || (System.Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException  e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
    }

    #endregion

    /// <summary>
    /// 执行存储过程,返回数据集
    /// </summary>
    /// <param name="storedProcName">存储过程名</param>
    /// <param name="parameters">存储过程参数</param>
    /// <returns>DataSet</returns>
    public static DataSet RunProcedureForDataSet(string storedProcName, IDataParameter[] parameters)
    {
        using (MySqlConnection connection = new MySqlConnection(connstr))
        {
            DataSet dataSet = new DataSet();
            connection.Open();
            count++;
            MySqlDataAdapter sqlDA = new MySqlDataAdapter();
            sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
            sqlDA.Fill(dataSet);
            connection.Close();
            connection.Dispose();
            count--;
            return dataSet;
        }
    }

    /// <summary>
    /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="storedProcName">存储过程名</param>
    /// <param name="parameters">存储过程参数</param>
    /// <returns>SqlCommand</returns>
    private static MySqlCommand BuildQueryCommand(MySqlConnection connection, string storedProcName,
        IDataParameter[] parameters)
    {
        MySqlCommand command = new MySqlCommand(storedProcName, connection);
        command.CommandType = CommandType.StoredProcedure;
        foreach (MySqlParameter parameter in parameters)
        {
            command.Parameters.Add(parameter);
        }
        return command;
    }

    #region 装载MySqlCommand对象

    /// <summary>
    /// 装载MySqlCommand对象
    /// </summary>
    private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText,
        MySqlParameter[] cmdParms)
    {
        if (conn.State != ConnectionState.Open)
        {
            conn.Open();
        }
        cmd.Connection = conn;
        cmd.CommandText = cmdText;
        if (trans != null)
        {
            cmd.Transaction = trans;
        }
        cmd.CommandType = CommandType.Text; //cmdType;
        if (cmdParms != null)
        {
            foreach (MySqlParameter parm in cmdParms)
            {
                cmd.Parameters.Add(parm);
            }
        }
    }
    #endregion
}