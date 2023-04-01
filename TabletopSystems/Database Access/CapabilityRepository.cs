using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using TabletopSystems.Models;

namespace TabletopSystems.Database_Access
{
    public class CapabilityRepository
    {
        private UserConnection _userConnection;
        public CapabilityRepository(UserConnection conn) {
            _userConnection = conn;
        }

        public void Add(TTRPGCapability capability)
        {
            string addToCapabilities = "INSERT INTO Capabilities(CapabilityName, SystemID, CapabilityDescription, CapabilityArea, CapabilityRange, CapabilityUseTime, CapabilityCost)" +
                " VALUES (@capabilityName,@systemID,@capabilityDescription,@capabilityArea,@capabilityRange,@capabilityUseTime,@capabilityCost)";
            string attachTags = "INSERT INTO Capabilities_Tags(Capabilities_SystemID, CapabilityName, Tags_SystemID, TagName)" +
                " VALUES (@capabilitySystemID,@capabilityName,@tagSystemID,@tagName)";
            try
            {
                if (_userConnection.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                    {
                        using (var transaction = conn.BeginTransaction())
                        {
                            using (SqlCommand cmd = new SqlCommand(addToCapabilities, conn))
                            {
                                cmd.Transaction = transaction;
                                conn.Open();
                                cmd.Parameters.AddWithValue("@capabilityName", capability.CapabilityName);
                                cmd.Parameters.AddWithValue("@systemID", capability.SystemID);
                                cmd.Parameters.AddWithValue("@capabilityDescription", capability.Description);
                                cmd.Parameters.AddWithValue("@capabilityArea", capability.Area);
                                cmd.Parameters.AddWithValue("@capabilityRange", capability.Range);
                                cmd.Parameters.AddWithValue("@capabilityUseTime", capability.UseTime);
                                cmd.Parameters.AddWithValue("@capabilityCost", capability.Cost);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachTags;
                                cmd.Parameters.AddWithValue("@capabilitySystemID", capability.SystemID);
                                cmd.Parameters.AddWithValue("@capabilityName", capability.CapabilityName);
                                cmd.Parameters.AddWithValue("@tagSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@tagName", DBNull.Value);
                                foreach (TTRPGTag tag in capability.Tags)
                                {
                                    cmd.Parameters["@tagSystemID"].Value = tag.SystemID;
                                    cmd.Parameters["@tagName"].Value = tag.TagName;
                                    cmd.ExecuteNonQuery();
                                }
                                transaction.Commit();
                            }
                        }
                    }
                }
                else
                {
                    using (SqliteConnection conn = new SqliteConnection(_userConnection.sqliteString))
                    {
                        using (var transaction = conn.BeginTransaction())
                        {
                            using (SqliteCommand cmd = new SqliteCommand(addToCapabilities, conn))
                            {
                                cmd.Transaction = transaction;
                                conn.Open();
                                cmd.Parameters.AddWithValue("@capabilityName", capability.CapabilityName);
                                cmd.Parameters.AddWithValue("@systemID", capability.SystemID);
                                cmd.Parameters.AddWithValue("@capabilityDescription", capability.Description);
                                cmd.Parameters.AddWithValue("@capabilityArea", capability.Area);
                                cmd.Parameters.AddWithValue("@capabilityRange", capability.Range);
                                cmd.Parameters.AddWithValue("@capabilityUseTime", capability.UseTime);
                                cmd.Parameters.AddWithValue("@capabilityCost", capability.Cost);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachTags;
                                cmd.Parameters.AddWithValue("@capabilitySystemID", capability.SystemID);
                                cmd.Parameters.AddWithValue("@capabilityName", capability.CapabilityName);
                                cmd.Parameters.AddWithValue("@tagSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@tagName", DBNull.Value);
                                foreach (TTRPGTag tag in capability.Tags)
                                {
                                    cmd.Parameters["@tagSystemID"].Value = tag.SystemID;
                                    cmd.Parameters["@tagName"].Value = tag.TagName;
                                    cmd.ExecuteNonQuery();
                                }
                                transaction.Commit();
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
            }
            catch (SqliteException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
            }
        }

        public void Delete(TTRPGCapability capability)
        {
            string cmdString = "DELETE FROM Capabilities WHERE CapabilityName=@capabilityName AND SystemID=@systemID";
            try
            {
                if (_userConnection.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                    {
                        using (SqlCommand cmd = new SqlCommand(cmdString, conn))
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@capabilityName", capability.CapabilityName);
                            cmd.Parameters.AddWithValue("@systemID", capability.SystemID);
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
                            cmd.Parameters.AddWithValue("@capabilityName", capability.CapabilityName);
                            cmd.Parameters.AddWithValue("@systemID", capability.SystemID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
            }
            catch (SqliteException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
            }
        }
    }
}
