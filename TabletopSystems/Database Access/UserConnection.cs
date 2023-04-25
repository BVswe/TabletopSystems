

using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TabletopSystems;

public class UserConnection
{
    public string sqlString {get; set; }
    public string sqliteString { get; set; }
    public bool connectedToSqlServer { get; set; }

    public UserConnection()
    {
        sqlString = "Data Source=" + "localhost" + "; Initial Catalog=TabletopSystems; Integrated Security=true; Encrypt=false; Connection Timeout=2";
        sqliteString = "Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString() + "\\TabletopSystemsData.sqlite";
        connectedToSqlServer = false;
    }

    /// <summary>
    /// Sets SqlConnection if a connection using string s is available, otherwise sets SqliteConnection to documents folder, Change to async later?
    /// </summary>
    /// <param name="s">Give a database connection string</param>
    public void tryConnection(string hostName)
    {
        sqlString = $"Data Source= {hostName}; Initial Catalog=TabletopSystems; Integrated Security=true; Encrypt=false; Connection Timeout=2";
        try
        {
            using (SqlConnection connection = new SqlConnection(sqlString))
            {
                connection.Open();
                connectedToSqlServer = true;
                connection.Close();
            }
        }
        catch (SqlException ex)
        {
            connectedToSqlServer = false;
        }
    }
}
