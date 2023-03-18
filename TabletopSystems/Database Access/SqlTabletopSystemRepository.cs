

using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TabletopSystems.Database_Access;

public class SqlTabletopSystemRepository : ITabletopSystemRepository
{
    private readonly UserConnection userConnection;
    public SqlTabletopSystemRepository(UserConnection conn)
    {
        userConnection = conn;
    }
    /// <summary>
    /// Adds a system to the sql database
    /// </summary>
    /// <param name="systemToAdd"></param>
    public void Add(TabletopSystem systemToAdd)
    {
        try
        {
            string cmdString = "INSERT INTO Systems VALUES (@systemName)";
            using (SqlCommand cmd = new SqlCommand(cmdString, userConnection.userSqlConnection))
            {
                userConnection.userSqlConnection.Open();
                cmd.Parameters.AddWithValue("@systemName", systemToAdd.SystemName);
                cmd.ExecuteNonQuery();
                userConnection.userSqlConnection.Close();
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
        try
        {
            string cmdString = "UPDATE Systems SET SystemName=@systemName WHERE SystemID=@systemID";
            using (SqlCommand cmd = new SqlCommand(cmdString, userConnection.userSqlConnection))
            {
                userConnection.userSqlConnection.Open();
                cmd.Parameters.AddWithValue("@systemName", systemToEdit.SystemName);
                cmd.Parameters.AddWithValue("@systemID", systemToEdit.SystemID);
                cmd.ExecuteNonQuery();
                userConnection.userSqlConnection.Close();
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
        try
        {
            string cmdString = "DELETE FROM Systems WHERE SystemID=@systemID";
            using (SqlCommand cmd = new SqlCommand(cmdString, userConnection.userSqlConnection))
            {
                userConnection.userSqlConnection.Open();
                cmd.Parameters.AddWithValue("@systemID", systemToDelete.SystemID);
                cmd.ExecuteNonQuery();
                userConnection.userSqlConnection.Close();
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
        try
        {
            int i;
            string cmdString = "SELECT SystemID FROM Systems WHERE SystemName=@systemName";
            using (SqlCommand cmd = new SqlCommand(cmdString, userConnection.userSqlConnection))
            {
                userConnection.userSqlConnection.Open();
                cmd.Parameters.AddWithValue("@systemName", s);
                var firstColumn = cmd.ExecuteScalar();
                if (firstColumn != null)
                {
                    if (!Int32.TryParse(firstColumn.ToString(), out i))
                    {
                        Trace.Write("Invalid Integer!");
                    }
                }
                else
                {
                    Trace.Write("System not found!");
                    return 0;
                }
                userConnection.userSqlConnection.Close();
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
        try
        {
            string cmdString = "SELECT * FROM Systems";
            Dictionary<string, int> systems = new Dictionary<string, int>();
            using (SqlCommand cmd = new SqlCommand(cmdString, userConnection.userSqlConnection))
            {
                userConnection.userSqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    systems.Add(reader["SystemName"].ToString(), Int32.Parse(reader["SystemID"].ToString()));
                }
                userConnection.userSqlConnection.Close();
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
