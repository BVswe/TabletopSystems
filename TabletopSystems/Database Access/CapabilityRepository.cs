﻿using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
            string attachAttributes = "INSERT INTO Attributes_Capabilities(Attributes_SystemID, AttributeName, Capabilities_SystemID, CapabilityName)" +
                " VALUES (@attributeSystemID,@attributeName,@capabilitySystemID,@capabilityName)";
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
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachAttributes;
                                cmd.Parameters.AddWithValue("@capabilitySystemID", capability.SystemID);
                                cmd.Parameters.AddWithValue("@capabilityName", capability.CapabilityName);
                                cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                foreach (TTRPGAttribute attr in capability.Attributes)
                                {
                                    cmd.Parameters["@attributeSystemID"].Value = attr.SystemID;
                                    cmd.Parameters["@attributeName"].Value = attr.AttributeName;
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
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachAttributes;
                                cmd.Parameters.AddWithValue("@capabilitySystemID", capability.SystemID);
                                cmd.Parameters.AddWithValue("@capabilityName", capability.CapabilityName);
                                cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                foreach (TTRPGAttribute attr in capability.Attributes)
                                {
                                    cmd.Parameters["@attributeSystemID"].Value = attr.SystemID;
                                    cmd.Parameters["@attributeName"].Value = attr.AttributeName;
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
        public void Delete(string capabilityName, int systemID)
        {
            //Delete cascades in database automatically, just delete the capability
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
                            cmd.Parameters.AddWithValue("@capabilityName", capabilityName);
                            cmd.Parameters.AddWithValue("@systemID", systemID);
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
                            cmd.Parameters.AddWithValue("@capabilityName", capabilityName);
                            cmd.Parameters.AddWithValue("@systemID", systemID);
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
            string attachAttributes = "INSERT INTO Attributes_Capabilities(Attributes_SystemID, AttributeName, Capabilities_SystemID, CapabilityName)" +
                " VALUES (@attributeSystemID,@attributeName,@capabilitySystemID,@capabilityName";
            string removeAttributes = "DELETE FROM Attributes_Capabilities" +
                " WHERE Attributes_SystemID=@oldAttributeSystemID, AttributeName=@oldAttributeName, Capabilities_SystemID=@oldCapabilitySystemID, CapabilityName=@oldCapabilityName";
            List<TTRPGTag> tagsToAdd = capability.Tags.Except(oldCapability.Tags).ToList();
            List<TTRPGTag> tagsToRemove = oldCapability.Tags.Except(capability.Tags).ToList();
            List<TTRPGAttribute> attributesToAdd = capability.Attributes.Except(oldCapability.Attributes).ToList();
            List<TTRPGAttribute> attributesToRemove = oldCapability.Attributes.Except(capability.Attributes).ToList();
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
                            using (SqlCommand cmd = conn.CreateCommand())
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
                                if (attributesToRemove.Count > 0)
                                {
                                    cmd.CommandText = removeAttributes;
                                    cmd.Parameters.AddWithValue("@oldCapabilitySystemID", capability.SystemID);
                                    cmd.Parameters.AddWithValue("@oldCapabilityName", capability.CapabilityName);
                                    cmd.Parameters.AddWithValue("@oldAttributeSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@oldAttributeName", DBNull.Value);
                                    foreach (TTRPGAttribute attr in capability.Attributes)
                                    {
                                        cmd.Parameters["@oldAttributeSystemID"].Value = attr.SystemID;
                                        cmd.Parameters["@oldAttributeName"].Value = attr.AttributeName;
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
                                if (attributesToAdd.Count > 0)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.CommandText = attachAttributes;
                                    cmd.Parameters.AddWithValue("@capabilitySystemID", capability.SystemID);
                                    cmd.Parameters.AddWithValue("@capabilityName", capability.CapabilityName);
                                    cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                    foreach (TTRPGAttribute attr in capability.Attributes)
                                    {
                                        cmd.Parameters["@attributeSystemID"].Value = attr.SystemID;
                                        cmd.Parameters["@attributeName"].Value = attr.AttributeName;
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
                                if (attributesToRemove.Count > 0)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.CommandText = removeAttributes;
                                    cmd.Parameters.AddWithValue("@oldCapabilitySystemID", capability.SystemID);
                                    cmd.Parameters.AddWithValue("@oldCapabilityName", capability.CapabilityName);
                                    cmd.Parameters.AddWithValue("@oldAttributeSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@oldAttributeName", DBNull.Value);
                                    foreach (TTRPGAttribute attr in capability.Attributes)
                                    {
                                        cmd.Parameters["@oldAttributeSystemID"].Value = attr.SystemID;
                                        cmd.Parameters["@oldAttributeName"].Value = attr.AttributeName;
                                        cmd.ExecuteNonQuery();
                                    }

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
                                if (attributesToAdd.Count > 0)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.CommandText = attachAttributes;
                                    cmd.Parameters.AddWithValue("@capabilitySystemID", capability.SystemID);
                                    cmd.Parameters.AddWithValue("@capabilityName", capability.CapabilityName);
                                    cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                    foreach (TTRPGAttribute attr in capability.Attributes)
                                    {
                                        cmd.Parameters["@attributeSystemID"].Value = attr.SystemID;
                                        cmd.Parameters["@attributeName"].Value = attr.AttributeName;
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
            string cmdString = "SELECT SystemID, CapabilityName, CapabilityDescription, CapabilityArea, CapabilityRange, CapabilityUseTime, CapabilityCost" +
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

        public TTRPGCapability SearchCapability(string capabilityName, int systemID)
        {
            string cmdString = "SELECT SystemID, CapabilityName, CapabilityDescription, CapabilityArea, CapabilityRange, CapabilityUseTime, CapabilityCost," +
                " Coalesce((SELECT STRING_AGG(TagName, '|') as [Tags] FROM Capabilities_Tags WHERE CapabilityName=Capabilities.CapabilityName),'') as [Tags]," +
                " Coalesce((SELECT STRING_AGG(AttributeName, '|') as [Attributes] FROM Attributes_Capabilities WHERE CapabilityName=Capabilities.CapabilityName),'') as [Attributes]" +
                " FROM Capabilities WHERE SystemID=@systemID AND CapabilityName=@capabilityName";
            TTRPGCapability capability = new TTRPGCapability();
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
                            cmd.Parameters.AddWithValue("@capabilityName", capabilityName);
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                capability.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                capability.CapabilityName = reader["CapabilityName"].ToString()!;
                                capability.Description = reader["CapabilityDescription"].ToString() ?? string.Empty;
                                capability.Area = reader["CapabilityArea"].ToString() ?? string.Empty;
                                capability.Range = reader["CapabilityRange"].ToString() ?? string.Empty;
                                capability.UseTime = reader["CapabilityUseTime"].ToString() ?? string.Empty;
                                capability.Cost = reader["CapabilityCost"].ToString() ?? string.Empty;
                                string temp = reader["Tags"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp))
                                {
                                    foreach (string s in temp.Split('|'))
                                    {
                                        TTRPGTag tempTag = new TTRPGTag();
                                        tempTag.TagName = s;
                                        tempTag.SystemID = capability.SystemID;
                                        capability.Tags.Add(tempTag);
                                    }
                                }
                                temp = reader["Attributes"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp))
                                {
                                    foreach (string s in temp.Split('|'))
                                    {
                                        TTRPGAttribute tempAttribute = new TTRPGAttribute();
                                        tempAttribute.AttributeName = s;
                                        tempAttribute.SystemID = capability.SystemID;
                                        capability.Attributes.Add(tempAttribute);
                                    }
                                }
                            }
                            if (reader.Read()) throw new DataException("multiple rows returned from query");
                        }
                    }
                }
                else
                {
                    cmdString = "SELECT SystemID, CapabilityName, CapabilityDescription, CapabilityArea, CapabilityRange, CapabilityUseTime, CapabilityCost," +
                " Coalesce((SELECT group_concat(TagName, '|') as [Tags] FROM Capabilities_Tags WHERE CapabilityName=Capabilities.CapabilityName),'') as [Tags]," +
                " Coalesce((SELECT group_concat(AttributeName, '|') as [Attributes] FROM Attributes_Capabilities WHERE CapabilityName=Capabilities.CapabilityName),'') as [Attributes]" +
                " FROM Capabilities WHERE SystemID=@systemID AND CapabilityName=@capabilityName";
                    using (SqliteConnection conn = new SqliteConnection(_userConnection.sqliteString))
                    {
                        using (SqliteCommand cmd = new SqliteCommand(cmdString, conn))
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@systemID", systemID);
                            cmd.Parameters.AddWithValue("@capabilityName", capabilityName);
                            SqliteDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                capability.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                capability.CapabilityName = reader["CapabilityName"].ToString()!;
                                capability.Description = reader["CapabilityDescription"].ToString() ?? string.Empty;
                                capability.Area = reader["CapabilityArea"].ToString() ?? string.Empty;
                                capability.Range = reader["CapabilityRange"].ToString() ?? string.Empty;
                                capability.UseTime = reader["CapabilityUseTime"].ToString() ?? string.Empty;
                                capability.Cost = reader["CapabilityCost"].ToString() ?? string.Empty;
                                string temp = reader["Tags"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp))
                                {
                                    foreach (string s in temp.Split('|'))
                                    {
                                        TTRPGTag tempTag = new TTRPGTag();
                                        tempTag.TagName = s;
                                        tempTag.SystemID = capability.SystemID;
                                        capability.Tags.Add(tempTag);
                                    }
                                }
                                temp = reader["Attributes"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp))
                                {
                                    foreach (string s in temp.Split('|'))
                                    {
                                        TTRPGAttribute tempAttribute = new TTRPGAttribute();
                                        tempAttribute.AttributeName = s;
                                        tempAttribute.SystemID = capability.SystemID;
                                        capability.Attributes.Add(tempAttribute);
                                    }
                                }
                            }
                            if (reader.Read()) throw new DataException("Multiple rows returned from query.");
                        }
                    }
                }
                return capability;
            }
            catch (SqlException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new TTRPGCapability();
            }
            catch (SqliteException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new TTRPGCapability();
            }
        }
    }
}
