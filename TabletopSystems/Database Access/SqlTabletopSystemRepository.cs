

using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TabletopSystems.Database_Access;

public class SqlTabletopSystemRepository : ITabletopSystemRepository
{
    private readonly UserConnection _userConnection;
    public SqlTabletopSystemRepository(UserConnection conn)
    {
        _userConnection = conn;
    }
    /// <summary>
    /// Adds a system to the sql database
    /// </summary>
    /// <param name="systemToAdd"></param>
    public void Add(TabletopSystem systemToAdd)
    {
        string cmdString = "INSERT INTO Systems VALUES (@systemName)";
        try
        {
            if (_userConnection.connectedToSqlServer)
            {
                using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdString, conn))
                    {
                        cmd.Parameters.AddWithValue("@systemName", systemToAdd.SystemName);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                using (SqliteConnection conn = new SqliteConnection(_userConnection.sqliteString))
                {
                    using (SqliteCommand cmd = new SqliteCommand(cmdString, conn))
                    {
                        cmd.Parameters.AddWithValue("@systemName", systemToAdd.SystemName);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine("An exception occured: " + ex.ToString());
        }
    }
    /// <summary>
    /// Edits a system in the sql database
    /// </summary>
    /// <param name="systemToAdd">System containing NEW SystemName and OLD SystemID</param>
    public void EditSystemName(TabletopSystem systemToEdit)
    {
        string cmdString = "UPDATE Systems SET SystemName=@systemName WHERE SystemID=@systemID";
        try
        {
            if (_userConnection.connectedToSqlServer)
            {
                using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdString, conn))
                    {
                        cmd.Parameters.AddWithValue("@systemName", systemToEdit.SystemName);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                using (SqliteConnection conn = new SqliteConnection(_userConnection.sqliteString))
                {
                    using (SqliteCommand cmd = new SqliteCommand(cmdString, conn))
                    {
                        cmd.Parameters.AddWithValue("@systemName", systemToEdit.SystemName);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine("An exception occured: " + ex.ToString());
        }
    }
    /// <summary>
    /// Deletes a system from the sql database
    /// </summary>
    /// <param name="objectToRemove"></param>
    public void Delete(TabletopSystem systemToDelete)
    {
        string cmdString = "DELETE FROM Systems WHERE SystemID=@systemID";
        try
        {
            if (_userConnection.connectedToSqlServer)
            {
                using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdString, conn))
                    {
                        cmd.Parameters.AddWithValue("@systemName", systemToDelete.SystemName);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                using (SqliteConnection conn = new SqliteConnection(_userConnection.sqliteString))
                {
                    using (SqliteCommand cmd = new SqliteCommand(cmdString, conn))
                    {
                        cmd.Parameters.AddWithValue("@systemName", systemToDelete.SystemName);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine("An exception occured: " + ex.ToString());
        }
    }

    /// <summary>
    /// Returns ID of an existing system
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public int GetIDBySystemName(string s)
    {
        int i;
        string cmdString = "SELECT SystemID FROM Systems WHERE SystemName=@systemName";
        try
        {
            if (_userConnection.connectedToSqlServer)
            {
                using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdString, conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@systemName", s);
                        var firstColumn = cmd.ExecuteScalar();
                        if (firstColumn != null)
                        {
                            if (!Int32.TryParse(firstColumn.ToString(), out i))
                            {
                                Trace.WriteLine("Invalid Integer!");
                            }
                        }
                        else
                        {
                            Trace.WriteLine("System not found!");
                            conn.Close();
                            return 0;
                        }
                        conn.Close();
                    }
                }
            }
            else
            {
                using (SqliteConnection conn = new SqliteConnection(_userConnection.sqliteString))
                {
                    using (SqliteCommand cmd = new SqliteCommand(cmdString, conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@systemName", s);
                        var firstColumn = cmd.ExecuteScalar();
                        if (firstColumn != null)
                        {
                            if (!Int32.TryParse(firstColumn.ToString(), out i))
                            {
                                Trace.WriteLine("Invalid Integer!");
                            }
                        }
                        else
                        {
                            Trace.WriteLine("System not found!");
                            conn.Close();
                            return 0;
                        }
                        conn.Close();
                    }
                }
            }
            return i;
        }
        catch (Exception ex)
        {
            Trace.WriteLine("An exception occured: " + ex.ToString());
            return 0;
        }
    }
    /// <summary>
    /// Gets all systems from the sql database
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, int> GetSystems()
    {
        string cmdString = "SELECT * FROM Systems";
        Dictionary<string, int> systems = new Dictionary<string, int>();
        try
        {
            using (SqlConnection conn = new SqlConnection(cmdString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdString, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        systems.Add(reader["SystemName"].ToString(), Int32.Parse(reader["SystemID"].ToString()));
                    }
                    conn.Close();
                }
            }
            return systems;
        }
        catch (Exception ex)
        {
            Trace.WriteLine("An exception occured: " + ex.ToString());
            return new Dictionary<string, int>();
        }
    }
}
    