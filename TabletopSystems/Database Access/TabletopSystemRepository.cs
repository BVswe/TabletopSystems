

using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace TabletopSystems.Database_Access;

public class TabletopSystemRepository : ITabletopSystemRepository
{
    private readonly UserConnection _userConnection;
    public TabletopSystemRepository(UserConnection conn)
    {
        _userConnection = conn;
    }
    /// <summary>
    /// Adds a system to the database
    /// </summary>
    /// <param name="systemToAdd"></param>
    public void Add(TabletopSystem systemToAdd)
    {
        string cmdString = "INSERT INTO Systems(SystemName) VALUES (@systemName)";
        try
        {
            if (_userConnection.connectedToSqlServer)
            {
                using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdString, conn))
                    {
                        conn.Open();
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
                        conn.Open();
                        cmd.Parameters.AddWithValue("@systemName", systemToAdd.SystemName);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Trace.WriteLine("An exception occured: " + e.ToString());
        }
    }
    /// <summary>
    /// Edits a system in the database
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
                        conn.Open();
                        cmd.Parameters.AddWithValue("@systemName", systemToEdit.SystemName);
                        cmd.Parameters.AddWithValue("@systemID", systemToEdit.SystemID);
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
                        conn.Open();
                        cmd.Parameters.AddWithValue("@systemName", systemToEdit.SystemName);
                        cmd.Parameters.AddWithValue("@systemID", systemToEdit.SystemID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Trace.WriteLine("An exception occured: " + e.ToString());
        }
    }
    /// <summary>
    /// Deletes a system from the database
    /// </summary>
    /// <param name="objectToRemove"></param>
    public void Delete(TabletopSystem systemToDelete)
    {
        string cmdString = "DELETE FROM Systems WHERE SystemName=@systemName AND SystemID=@systemID";
        try
        {
            if (_userConnection.connectedToSqlServer)
            {
                using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdString, conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@systemName", systemToDelete.SystemName);
                        cmd.Parameters.AddWithValue("@systemID", systemToDelete.SystemID);
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
                        conn.Open();
                        cmd.Parameters.AddWithValue("@systemName", systemToDelete.SystemName);
                        cmd.Parameters.AddWithValue("@systemID", systemToDelete.SystemID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Trace.WriteLine("An exception occured: " + e.ToString());
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
        string cmdString = "SELECT SystemID FROM Systems WHERE SystemName=@systemName AND SystemID=@systemID";
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
                    }
                }
            }
            return i;
        }
        catch (SqlException e)
        {
            Trace.WriteLine("An exception occured: " + e.ToString());
            return 0;
        }
    }
    /// <summary>
    /// Gets all systems from the database
    /// </summary>
    /// <returns></returns>
    public ObservableCollection<TabletopSystem> GetSystems()
    {
        string cmdString = "SELECT * FROM Systems";
        ObservableCollection<TabletopSystem> systems = new ObservableCollection<TabletopSystem>();
        try
        {
            if (_userConnection.connectedToSqlServer)
            {
                using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdString, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            TabletopSystem temp = new TabletopSystem();
                            temp.SystemID = Int32.Parse(reader["SystemID"].ToString());
                            temp.SystemName = reader["SystemName"].ToString();
                            systems.Add(temp);
                        }
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
                        SqliteDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            TabletopSystem temp = new TabletopSystem();
                            temp.SystemID = Int32.Parse(reader["SystemID"].ToString());
                            temp.SystemName = reader["SystemName"].ToString();
                            systems.Add(temp);
                        }
                    }
                }
            }
            return systems;
        }
        catch (SqlException e)
        {
            Trace.WriteLine("An exception occured: " + e.ToString());
            return new ObservableCollection<TabletopSystem>();
        }
    }
}
