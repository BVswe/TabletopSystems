

using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;

namespace TabletopSystems;

public class UserConnection
{
    private bool connectedToSqlServer;
    private SqlConnection _userSqlConnection;
    private SqliteConnection _userSqliteConnection;

    public UserConnection(string s)
    {
        tryConnection(s);
    }

    /// <summary>
    /// Returns an Sql connection if available, else returns Sqlite connection. Caller is responsible for opening and closing.
    /// </summary>
    /// <returns></returns>
    public object getConnection()
    {
        if (connectedToSqlServer)
        {
            return _userSqlConnection;
        }
        else
        {
            return _userSqliteConnection;
        }
    }
    /// <summary>
    /// Sets SqlConnection if a connection using string s is available, otherwise sets SqliteConnection to documents folder
    /// </summary>
    /// <param name="s">Give a database connection string</param>
    public void tryConnection(String s)
    {
        using (SqlConnection connection = new SqlConnection(s + ";Connection Timeout = 3"))
        {
            try
            {
                connection.Open();
                connectedToSqlServer = true;
                _userSqlConnection = new SqlConnection(connection.ConnectionString + ";Connection Timeout = 3");
            }
            catch (SqlException)
            {
                connectedToSqlServer = false;
                _userSqliteConnection = new SqliteConnection("Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString() + "TabletopSystemsData.sqlite");
            }
        }
    }
}
