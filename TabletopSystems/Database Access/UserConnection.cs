

using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;

namespace TabletopSystems;

public class UserConnection
{
    private string _connectionString;
    public bool connectedToSqlServer { get; set; }
    public SqlConnection userSqlConnection { get; set; }
    public SqliteConnection userSqliteConnection { get; set; }

    public void setConnectionTarget(string s)
    {
        _connectionString = s;
    }

    public UserConnection(string s)
    {
        _connectionString = "Data Source=" + s + "; Initial Catalog=TabletopSystems; Integrated Security=true; Encrypt=false";
        tryConnection();
    }

    /// <summary>
    /// Returns an Sql connection if available, else returns Sqlite connection. Caller is responsible for opening and closing.
    /// </summary>
    /// <returns></returns>
    public object getConnection()
    {
        if (connectedToSqlServer)
        {
            return userSqlConnection;
        }
        else
        {
            return userSqliteConnection;
        }
    }
    /// <summary>
    /// Sets SqlConnection if a connection using string s is available, otherwise sets SqliteConnection to documents folder
    /// </summary>
    /// <param name="s">Give a database connection string</param>
    public void tryConnection()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString + ";Connection Timeout = 3"))
        {
            try
            {
                connection.Open();
                connectedToSqlServer = true;
                if (userSqlConnection != null && userSqlConnection.ConnectionString != _connectionString) {
                    return;
                }
                userSqlConnection = new SqlConnection(_connectionString + ";Connection Timeout = 3");
            }
            catch (SqlException)
            {
                connectedToSqlServer = false;
                if (userSqliteConnection != null)
                {
                    return;
                }
                userSqliteConnection = new SqliteConnection("Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString() + "TabletopSystemsData.sqlite");
            }
        }
    }
}
