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
        /// <summary>
        /// Add capability to database
        /// </summary>
        /// <param name="capability"></param>
        public void Add(TTRPGCapability capability)
        {
            string addToCapabilities = "INSERT INTO Capabilities(CapabilityName, SystemID, CapabilityDescription, CapabilityArea, CapabilityRange, CapabilityUseTime, CapabilityCost)" +
                " VALUES (@capabilityName,@systemID,@capabilityDescription,@capabilityArea,@capabilityRange,@capabilityUseTime,@capabilityCost)";
            string attachTags = "INSERT INTO Capabilities_Tags(Capabilities_SystemID, CapabilityName, Tags_SystemID, TagName)" +
                " VALUES (@capabilitySystemID,@capabilityName,@tagSystemID,@tagName)";
            try
            {
                //Add using sql if conencted to sql, else add using sqlite
                if (_userConnection.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                    {
                        conn.Open();
                        using (var transaction = conn.BeginTransaction())
                        {
                            using (SqlCommand cmd = new SqlCommand(addToCapabilities, conn))
                            {
                                cmd.Transaction = transaction;
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
                        conn.Open();
                        using (var transaction = conn.BeginTransaction())
                        {
                            using (SqliteCommand cmd = new SqliteCommand(addToCapabilities, conn))
                            {
                                cmd.Transaction = transaction;
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
        /// <summary>
        /// Delete a capability from the database
        /// </summary>
        /// <param name="capability"></param>
        public void Delete(TTRPGCapability capability)
        {
            string cmdString = "DELETE FROM Capabilities WHERE CapabilityName=@capabilityName AND SystemID=@systemID";
            try
            {
                //Add using sql if conencted to sql, else add using sqlite
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
        /// <summary>
        /// Edit an exisitng capability in the database
        /// </summary>
        /// <param name="capability"></param>
        /// <param name="oldCapability"></param>
        public void Edit(TTRPGCapability capability, TTRPGCapability oldCapability)
        {
            string updateCapability = "UPDATE Capabilities" +
                " SET CapabilityName=@capabilityName, SystemID=@systemID, CapabilityDescription=@capabilityDescription, CapabilityArea=@capabilityArea," +
                " CapabilityRange=@capabilityRange, CapabilityUseTime=@capabilityUseTime, CapabilityCost=@capabilityCost)" +
                " WHERE CapabilityName=@oldCapabilityName AND SystemID=@oldSystemID";
            string attachTags = "INSERT INTO Capabilities_Tags(Capabilities_SystemID, CapabilityName, Tags_SystemID, TagName)" +
                " VALUES (@capabilitySystemID,@capabilityName,@tagSystemID,@tagName)";
            string removeTags = "DELETE FROM Capabilities_Tags" +
                " WHERE Capabilities_SystemID=@oldCapabilitySystemID AND CapabilityName=@oldCapabilityName AND Tags_SystemID=@oldTagSystemID AND TagName=@oldTagName";
            List<TTRPGTag> tagsToAdd = capability.Tags.Except(oldCapability.Tags).ToList();
            List<TTRPGTag> tagsToRemove = oldCapability.Tags.Except(capability.Tags).ToList();
            try
            {
                //Add using sql if conencted to sql, else add using sqlite
                if (_userConnection.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                    {
                        conn.Open();
                        using (var transaction = conn.BeginTransaction())
                        {
                            using (SqlCommand cmd = new SqlCommand(updateCapability, conn))
                            {
                                cmd.Transaction = transaction;
                                //Remove tags if necessary
                                if (tagsToRemove.Count > 0)
                                {
                                    cmd.CommandText = removeTags;
                                    cmd.Parameters.AddWithValue("@oldCapabilitySystemID", oldCapability.SystemID);
                                    cmd.Parameters.AddWithValue("@oldCapabilityName", oldCapability.CapabilityName);
                                    cmd.Parameters.AddWithValue("@oldTagSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@oldTagName", DBNull.Value);
                                    foreach (TTRPGTag currentTag in tagsToRemove)
                                    {
                                        cmd.Parameters["@oldTagSystemID"].Value = currentTag.SystemID;
                                        cmd.Parameters["@oldTagName"].Value = currentTag.TagName;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                cmd.CommandText = updateCapability;
                                cmd.Parameters.AddWithValue("@capabilityName", capability.CapabilityName);
                                cmd.Parameters.AddWithValue("@systemID", capability.SystemID);
                                cmd.Parameters.AddWithValue("@capabilityDescription", capability.Description);
                                cmd.Parameters.AddWithValue("@capabilityArea", capability.Area);
                                cmd.Parameters.AddWithValue("@capabilityRange", capability.Range);
                                cmd.Parameters.AddWithValue("@capabilityUseTime", capability.UseTime);
                                cmd.Parameters.AddWithValue("@capabilityCost", capability.Cost);
                                cmd.Parameters.AddWithValue("@oldCapabilityName", oldCapability.CapabilityName);
                                cmd.Parameters.AddWithValue("@oldSystemID", oldCapability.SystemID);
                                cmd.ExecuteNonQuery();
                                if (tagsToAdd.Count > 0)
                                {
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
                            conn.Open();
                            using (SqliteCommand cmd = new SqliteCommand(updateCapability, conn))
                            {
                                cmd.Transaction = transaction;
                                //Remove tags if necessary
                                if (tagsToRemove.Count > 0)
                                {
                                    cmd.CommandText = removeTags;
                                    cmd.Parameters.AddWithValue("@oldCapabilitySystemID", oldCapability.SystemID);
                                    cmd.Parameters.AddWithValue("@oldCapabilityName", oldCapability.CapabilityName);
                                    cmd.Parameters.AddWithValue("@oldTagSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@oldTagName", DBNull.Value);
                                    foreach (TTRPGTag currentTag in tagsToRemove)
                                    {
                                        cmd.Parameters["@oldTagSystemID"].Value = currentTag.SystemID;
                                        cmd.Parameters["@oldTagName"].Value = currentTag.TagName;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                //Update capability (do after removing to avoid needless update)
                                cmd.CommandText = updateCapability;
                                cmd.Parameters.AddWithValue("@capabilityName", capability.CapabilityName);
                                cmd.Parameters.AddWithValue("@systemID", capability.SystemID);
                                cmd.Parameters.AddWithValue("@capabilityDescription", capability.Description);
                                cmd.Parameters.AddWithValue("@capabilityArea", capability.Area);
                                cmd.Parameters.AddWithValue("@capabilityRange", capability.Range);
                                cmd.Parameters.AddWithValue("@capabilityUseTime", capability.UseTime);
                                cmd.Parameters.AddWithValue("@capabilityCost", capability.Cost);
                                cmd.Parameters.AddWithValue("@oldCapabilityName", oldCapability.CapabilityName);
                                cmd.Parameters.AddWithValue("@oldSystemID", oldCapability.SystemID);
                                cmd.ExecuteNonQuery();
                                //Add tags if necessary
                                if (tagsToAdd.Count > 0)
                                {
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

        public ObservableCollection<TTRPGCapability> GetTTRPGCapabilities(int systemID)
        {
            string cmdString = "SELECT CapabilityName, CapabilityDescription, CapabilityArea, CapabilityRange, CapabilityUseTime, CapabilityCost" +
                " FROM Capabilities WHERE SystemID=@systemID";
            ObservableCollection<TTRPGCapability> capabilities = new ObservableCollection<TTRPGCapability>();
            try
            {
                if (_userConnection.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                    {
                        using (SqlCommand cmd = new SqlCommand(cmdString, conn))
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@systemID", systemID);
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                TTRPGCapability temp = new TTRPGCapability();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                temp.CapabilityName = reader["CapabilityName"].ToString()!;
                                temp.Description = reader["CapabilityDescription"].ToString() ?? string.Empty;
                                temp.Area = reader["CapabilityArea"].ToString() ?? string.Empty;
                                temp.Range = reader["CapabilityRange"].ToString() ?? string.Empty;
                                temp.UseTime = reader["CapabilityUseTime"].ToString() ?? string.Empty;
                                temp.Cost = reader["CapabilityCost"].ToString() ?? string.Empty;
                                capabilities.Add(temp);
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
                            cmd.Parameters.AddWithValue("@systemID", systemID);
                            SqliteDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                TTRPGCapability temp = new TTRPGCapability();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                temp.CapabilityName = reader["AttributeName"].ToString()!;
                                temp.Description = reader["CapabilityDescription"].ToString() ?? string.Empty;
                                temp.Area = reader["CapabilityArea"].ToString() ?? string.Empty;
                                temp.Range = reader["CapabilityRange"].ToString() ?? string.Empty;
                                temp.UseTime = reader["CapabilityUseTime"].ToString() ?? string.Empty;
                                temp.Cost = reader["CapabilityCost"].ToString() ?? string.Empty;
                                capabilities.Add(temp);
                            }
                        }
                    }
                }
                return capabilities;
            }
            catch (SqlException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new ObservableCollection<TTRPGCapability>();
            }
            catch (SqliteException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new ObservableCollection<TTRPGCapability>();
            }
        }

    }
}
