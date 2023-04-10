using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using TabletopSystems.Models;

namespace TabletopSystems.Database_Access
{
    public class ClassRepository
    {
        UserConnection _userConnection;
        public ClassRepository (UserConnection conn)
        {
            _userConnection = conn;
        }
        public void Add(TTRPGClass rpgClass)
        {
            string addToClasses = "INSERT INTO Classes(SystemID,ClassName,ClassDescription)" +
                " VALUES(@systemID,@className,@description)";
            string attachAttributes = "INSERT INTO Attributes_Classes(Classes_SystemID,ClassName,Attributes_SystemID,AttributeName,AttributeValue)" +
                " VALUES(@systemID,@className,@attributeSystemID,@attributeName,@attributeValue)";
            string attachCapabilities = "INSERT INTO Classes_Capabilities(Classes_SystemID,ClassName,Capabilities_SystemID,CapabilityName,LevelGained)" +
                " VALUES(@systemID,@className,@capabilitySystemID,@capabilityName,@levelGained)";
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
                            using (SqlCommand cmd = new SqlCommand(addToClasses, conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                cmd.Parameters.AddWithValue("@description", rpgClass.ClassDescription);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachAttributes;
                                cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeValue", DBNull.Value);
                                foreach (KeyValuePair<TTRPGAttribute, int> kvp in rpgClass.Attributes)
                                {
                                    cmd.Parameters["@attributeSystemID"].Value = kvp.Key.SystemID;
                                    cmd.Parameters["@attributeName"].Value = kvp.Key.AttributeName;
                                    cmd.Parameters["@attributeValue"].Value = kvp.Value;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachCapabilities;
                                cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@levelGained", DBNull.Value);
                                foreach (KeyValuePair<TTRPGCapability, int> kvp in rpgClass.Capabilities)
                                {
                                    cmd.Parameters["@capabilitySystemID"].Value = kvp.Key.SystemID;
                                    cmd.Parameters["@capabilityName"].Value = kvp.Key.CapabilityName;
                                    cmd.Parameters["@levelGained"].Value = kvp.Value;
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
                            using (SqliteCommand cmd = new SqliteCommand(addToClasses, conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                cmd.Parameters.AddWithValue("@description", rpgClass.ClassDescription);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachAttributes;
                                cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeValue", DBNull.Value);
                                foreach (KeyValuePair<TTRPGAttribute, int> kvp in rpgClass.Attributes)
                                {
                                    cmd.Parameters["@attributeSystemID"].Value = kvp.Key.SystemID;
                                    cmd.Parameters["@attributeName"].Value = kvp.Key.AttributeName;
                                    cmd.Parameters["@attributeValue"].Value = kvp.Value;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachCapabilities;
                                cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@levelGained", DBNull.Value);
                                foreach (KeyValuePair<TTRPGCapability, int> kvp in rpgClass.Capabilities)
                                {
                                    cmd.Parameters["@capabilitySystemID"].Value = kvp.Key.SystemID;
                                    cmd.Parameters["@capabilityName"].Value = kvp.Key.CapabilityName;
                                    cmd.Parameters["@levelGained"].Value = kvp.Value;
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
        public void Edit(TTRPGClass rpgClass, TTRPGClass oldClass)
        {
            #region Command Strings
            string editClasses = "UPDATE Classes" +
                " SET SystemID=@systemID,ClassName=@className,ClassDescription=@description)" +
                " WHERE SystemID=@oldSystemID AND ClassName=@oldClassName";
            string attachAttributes = "INSERT INTO Attributes_Classes(Classes_SystemID,ClassName,Attributes_SystemID,AttributeName,AttributeValue)" +
                " VALUES(@systemID,@className,@attributeSystemID,@attributeName,@attributeValue)";
            string removeAttributes = "DELETE FROM Attributes_Classes" +
                " WHERE Classes_SystemID=@systemID AND ClassName=@className AND Attributes_SystemID=@attributeSystemID AND AttributeName=@attributeName)";
            string editAttributes = "UPDATE Attributes_Classes" +
                " SET AttributeValue=@attributeValue" +
                " WHERE Classes_SystemID=@oldSystemID AND ClassName=@oldClassName AND Attributes_SystemID=@attributeSystemID AND AttributeName=@attributeName)";
            string attachCapabilities = "INSERT INTO Classes_Capabilities(Classes_SystemID,ClassName,Capabilities_SystemID,CapabilityName,LevelGained)" +
                " VALUES(@systemID,@className,@capabilitySystemID,@capabilityName,@levelGained)";
            string removeCapabilities = "DELETE FROM Classes_Capabilities" +
                " WHERE Classes_SystemID=@systemID AND ClassName=@className AND Capabilities_SystemID=@capabilitySystemID AND CapabilityName=@capabilityName)";
            string editCapabilities = "UPDATE Classes_Capabilities" +
                " SET LevelGained=@levelGained" +
                " WHERE Classes_SystemID=@systemID AND ClassName=@className AND Capabilities_SystemID=@capabilitySystemID AND CapabilityName=@capabilityName)";
            #endregion

            #region Dictionaries of differences
            Dictionary<TTRPGCapability, int> capabilitiesToAdd = new Dictionary<TTRPGCapability, int>();
            Dictionary<TTRPGCapability, int> capabilitiesToRemove = new Dictionary<TTRPGCapability, int>();
            Dictionary<TTRPGCapability, int> capabilitiesToEdit = new Dictionary<TTRPGCapability, int>();
            Dictionary<TTRPGAttribute, int> attributesToAdd = new Dictionary<TTRPGAttribute, int>();
            Dictionary<TTRPGAttribute, int> attributesToRemove = new Dictionary<TTRPGAttribute, int>();
            Dictionary<TTRPGAttribute, int> attributesToEdit = new Dictionary<TTRPGAttribute, int>();
            #endregion

            #region Populating dictionaries with differences (add/edit/remove)
            //Attributes
            foreach (KeyValuePair<TTRPGAttribute, int> kvp in rpgClass.Attributes)
            {
                if (!oldClass.Attributes.ContainsKey(kvp.Key))
                {
                    attributesToAdd.Add(kvp.Key, kvp.Value);
                }
                else if (oldClass.Attributes.ContainsKey(kvp.Key) && oldClass.Attributes[kvp.Key] != rpgClass.Attributes[kvp.Key])
                {
                    attributesToEdit.Add(kvp.Key, kvp.Value);
                }
            }
            foreach (KeyValuePair<TTRPGAttribute, int> kvp in oldClass.Attributes)
            {
                if (!rpgClass.Attributes.ContainsKey(kvp.Key))
                {
                    attributesToRemove.Add(kvp.Key, kvp.Value);
                }
            }
            //Capabilities
            foreach (KeyValuePair<TTRPGCapability, int> kvp in rpgClass.Capabilities)
            {
                if (!oldClass.Capabilities.ContainsKey(kvp.Key))
                {
                    capabilitiesToAdd.Add(kvp.Key, kvp.Value);
                }
                else if (oldClass.Capabilities.ContainsKey(kvp.Key) && oldClass.Capabilities[kvp.Key] != rpgClass.Capabilities[kvp.Key])
                {
                    capabilitiesToEdit.Add(kvp.Key, kvp.Value);
                }
            }
            foreach (KeyValuePair<TTRPGCapability, int> kvp in oldClass.Capabilities)
            {
                if (!rpgClass.Capabilities.ContainsKey(kvp.Key))
                {
                    capabilitiesToRemove.Add(kvp.Key, kvp.Value);
                }
            }
            #endregion
            try
            {
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
                                if (attributesToRemove.Count > 0)
                                {
                                    cmd.CommandText = removeAttributes;
                                    cmd.Parameters.AddWithValue("@oldClassName", oldClass.ClassName);
                                    cmd.Parameters.AddWithValue("@oldSystemID", oldClass.SystemID);
                                    cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                    foreach (KeyValuePair<TTRPGAttribute, int> attr in attributesToRemove)
                                    {
                                        cmd.Parameters["@attributeSystemID"].Value = attr.Key.SystemID;
                                        cmd.Parameters["@attributeName"].Value = attr.Key.AttributeName;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                if (capabilitiesToRemove.Count > 0)
                                {
                                    cmd.CommandText = removeCapabilities;
                                    cmd.Parameters.AddWithValue("@oldClassName", oldClass.ClassName);
                                    cmd.Parameters.AddWithValue("@oldSystemID", oldClass.SystemID);
                                    cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                    foreach (KeyValuePair<TTRPGCapability, int> kvp in capabilitiesToRemove)
                                    {
                                        cmd.Parameters["@capabilitySystemID"].Value = kvp.Key.SystemID;
                                        cmd.Parameters["@capabilityName"].Value = kvp.Key.CapabilityName;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                cmd.CommandText = editClasses;
                                cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                cmd.Parameters.AddWithValue("@description", rpgClass.ClassDescription);
                                cmd.Parameters.AddWithValue("@oldClassName", oldClass.ClassName);
                                cmd.Parameters.AddWithValue("@oldSystemID", oldClass.SystemID);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                if (attributesToAdd.Count > 0)
                                {
                                    cmd.CommandText = attachAttributes;
                                    cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                    cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                    cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@attributeValue", DBNull.Value);
                                    foreach (KeyValuePair<TTRPGAttribute, int> kvp in attributesToAdd)
                                    {
                                        cmd.Parameters["@attributeSystemID"].Value = kvp.Key.SystemID;
                                        cmd.Parameters["@attributeName"].Value = kvp.Key.AttributeName;
                                        cmd.Parameters["@attributeValue"].Value = kvp.Value;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                if (attributesToEdit.Count > 0)
                                {
                                    cmd.CommandText = editAttributes;
                                    cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                    cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                    cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@attributeValue", DBNull.Value);
                                    foreach (KeyValuePair<TTRPGAttribute, int> kvp in attributesToAdd)
                                    {
                                        cmd.Parameters["@attributeSystemID"].Value = kvp.Key.SystemID;
                                        cmd.Parameters["@attributeName"].Value = kvp.Key.AttributeName;
                                        cmd.Parameters["@attributeValue"].Value = kvp.Value;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                if (capabilitiesToAdd.Count > 0)
                                {
                                    cmd.CommandText = attachCapabilities;
                                    cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                    cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                    cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@levelGained", DBNull.Value);
                                    foreach (KeyValuePair<TTRPGCapability, int> kvp in capabilitiesToAdd)
                                    {
                                        cmd.Parameters["@capabilitySystemID"].Value = kvp.Key.SystemID;
                                        cmd.Parameters["@capabilityName"].Value = kvp.Key.CapabilityName;
                                        cmd.Parameters["@levelGained"].Value = kvp.Value;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                if (capabilitiesToEdit.Count > 0)
                                {
                                    cmd.CommandText = editCapabilities;
                                    cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                    cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                    cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@levelGained", DBNull.Value);
                                    foreach (KeyValuePair<TTRPGCapability, int> kvp in capabilitiesToEdit)
                                    {
                                        cmd.Parameters["@capabilitySystemID"].Value = kvp.Key.SystemID;
                                        cmd.Parameters["@capabilityName"].Value = kvp.Key.CapabilityName;
                                        cmd.Parameters["@levelGained"].Value = kvp.Value;
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
                        conn.Open();
                        using (var transaction = conn.BeginTransaction())
                        {
                            using (SqliteCommand cmd = conn.CreateCommand())
                            {
                                cmd.Transaction = transaction;
                                if (attributesToRemove.Count > 0)
                                {
                                    cmd.CommandText = removeAttributes;
                                    cmd.Parameters.AddWithValue("@oldClassName", oldClass.ClassName);
                                    cmd.Parameters.AddWithValue("@oldSystemID", oldClass.SystemID);
                                    cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                    foreach (KeyValuePair<TTRPGAttribute, int> attr in attributesToRemove)
                                    {
                                        cmd.Parameters["@attributeSystemID"].Value = attr.Key.SystemID;
                                        cmd.Parameters["@attributeName"].Value = attr.Key.AttributeName;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                if (capabilitiesToRemove.Count > 0)
                                {
                                    cmd.CommandText = removeCapabilities;
                                    cmd.Parameters.AddWithValue("@oldClassName", oldClass.ClassName);
                                    cmd.Parameters.AddWithValue("@oldSystemID", oldClass.SystemID);
                                    cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                    foreach (KeyValuePair<TTRPGCapability, int> kvp in capabilitiesToRemove)
                                    {
                                        cmd.Parameters["@capabilitySystemID"].Value = kvp.Key.SystemID;
                                        cmd.Parameters["@capabilityName"].Value = kvp.Key.CapabilityName;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                cmd.CommandText = editClasses;
                                cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                cmd.Parameters.AddWithValue("@description", rpgClass.ClassDescription);
                                cmd.Parameters.AddWithValue("@oldClassName", oldClass.ClassName);
                                cmd.Parameters.AddWithValue("@oldSystemID", oldClass.SystemID);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                if (attributesToAdd.Count > 0)
                                {
                                    cmd.CommandText = attachAttributes;
                                    cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                    cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                    cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@attributeValue", DBNull.Value);
                                    foreach (KeyValuePair<TTRPGAttribute, int> kvp in attributesToAdd)
                                    {
                                        cmd.Parameters["@attributeSystemID"].Value = kvp.Key.SystemID;
                                        cmd.Parameters["@attributeName"].Value = kvp.Key.AttributeName;
                                        cmd.Parameters["@attributeValue"].Value = kvp.Value;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                if (attributesToEdit.Count > 0)
                                {
                                    cmd.CommandText = editAttributes;
                                    cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                    cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                    cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@attributeValue", DBNull.Value);
                                    foreach (KeyValuePair<TTRPGAttribute, int> kvp in attributesToAdd)
                                    {
                                        cmd.Parameters["@attributeSystemID"].Value = kvp.Key.SystemID;
                                        cmd.Parameters["@attributeName"].Value = kvp.Key.AttributeName;
                                        cmd.Parameters["@attributeValue"].Value = kvp.Value;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                if (capabilitiesToAdd.Count > 0)
                                {
                                    cmd.CommandText = attachCapabilities;
                                    cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                    cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                    cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@levelGained", DBNull.Value);
                                    foreach (KeyValuePair<TTRPGCapability, int> kvp in capabilitiesToAdd)
                                    {
                                        cmd.Parameters["@capabilitySystemID"].Value = kvp.Key.SystemID;
                                        cmd.Parameters["@capabilityName"].Value = kvp.Key.CapabilityName;
                                        cmd.Parameters["@levelGained"].Value = kvp.Value;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                if (capabilitiesToEdit.Count > 0)
                                {
                                    cmd.CommandText = editCapabilities;
                                    cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                                    cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                                    cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@levelGained", DBNull.Value);
                                    foreach (KeyValuePair<TTRPGCapability, int> kvp in capabilitiesToEdit)
                                    {
                                        cmd.Parameters["@capabilitySystemID"].Value = kvp.Key.SystemID;
                                        cmd.Parameters["@capabilityName"].Value = kvp.Key.CapabilityName;
                                        cmd.Parameters["@levelGained"].Value = kvp.Value;
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
        public void Delete(TTRPGClass rpgClass)
        {
            string deleteClasses = "DELETE FROM Classes" +
               " WHERE SystemID=@systemID AND ClassName=@className";
            try
            {
                //Add using sql if conencted to sql, else add using sqlite
                if (_userConnection.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(deleteClasses, conn))
                        {
                            cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                            cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    using (SqliteConnection conn = new SqliteConnection(_userConnection.sqliteString))
                    {
                        conn.Open();
                        using (SqliteCommand cmd = new SqliteCommand(deleteClasses, conn))
                        {
                            cmd.Parameters.AddWithValue("@className", rpgClass.ClassName);
                            cmd.Parameters.AddWithValue("@systemID", rpgClass.SystemID);
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
        public ObservableCollection<TTRPGClass> GetClasses(int systemID)
        {
            ObservableCollection<TTRPGClass> classes = new ObservableCollection<TTRPGClass>();
            string cmdString = "SELECT SystemID,ClassName,ClassDescription" +
                " FROM Classes WHERE SystemID=@systemID";
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
                                TTRPGClass temp = new TTRPGClass();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                temp.ClassName = reader["ClassName"].ToString()!;
                                temp.ClassDescription = reader["ClassDescription"].ToString()!;
                                classes.Add(temp);
                            }
                        }
                    }
                }
                else
                {
                    using (SqliteConnection conn = new SqliteConnection(_userConnection.sqlString))
                    {
                        using (SqliteCommand cmd = new SqliteCommand(cmdString, conn))
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@systemID", systemID);
                            SqliteDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                TTRPGClass temp = new TTRPGClass();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                temp.ClassName = reader["ClassName"].ToString()!;
                                temp.ClassDescription = reader["ClassDescription"].ToString()!;
                                classes.Add(temp);
                            }
                        }
                    }
                }
                return classes;
            }
            catch (SqlException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new ObservableCollection<TTRPGClass>();
            }
            catch (SqliteException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new ObservableCollection<TTRPGClass>();
            }
        }
    }
}
