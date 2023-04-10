using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TabletopSystems.Models;

namespace TabletopSystems.Database_Access
{
    public class GearRepository
    {
        private UserConnection _userConnection;
        public GearRepository(UserConnection conn)
        {
            _userConnection = conn;
        }
        /// <summary>
        /// Add gear to the database
        /// </summary>
        /// <param name="gear"></param>
        public void Add(TTRPGGear gear)
        {
            string addToGear = "INSERT INTO Gear(SystemID,GearName,GearDescription)" +
                " VALUES(@systemID,@gearName,@description)";
            string attachTags = "INSERT INTO Gear_Tags(Gear_SystemID,GearName,Tags_SystemID,TagName)" +
                " VALUES(@systemID,@gearName,@tagSystemID,@tagName)";
            string attachAttributes = "INSERT INTO Attributes_Gear(Gear_SystemID,GearName,Attributes_SystemID,AttributeName,AttributeValue,AttributeUsed)" +
                " VALUES(@systemID,@gearName,@attributeSystemID,@attributeName,@attributeValue,@attributeUsed)";
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
                            using (SqlCommand cmd = new SqlCommand(addToGear, conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                                cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
                                cmd.Parameters.AddWithValue("@description", gear.Description);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachTags;
                                cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
                                cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                                cmd.Parameters.AddWithValue("@tagSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@tagName", DBNull.Value);
                                foreach (TTRPGTag tag in gear.Tags)
                                {
                                    cmd.Parameters["@tagSystemID"].Value = tag.SystemID;
                                    cmd.Parameters["@tagName"].Value = tag.TagName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachAttributes;
                                cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
                                cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                                cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeValue", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeUsed", DBNull.Value);
                                foreach (AttributeValueAndBool attr in gear.Attributes)
                                {
                                    cmd.Parameters["@attributeSystemID"].Value = attr.Attribute.SystemID;
                                    cmd.Parameters["@attributeName"].Value = attr.Attribute.AttributeName;
                                    cmd.Parameters["@attributeValue"].Value = attr.Value;
                                    cmd.Parameters["@attributeUsed"].Value = attr.BoolValue;
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
                            using (SqliteCommand cmd = new SqliteCommand(addToGear, conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                                cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
                                cmd.Parameters.AddWithValue("@description", gear.Description);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachTags;
                                cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
                                cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                                cmd.Parameters.AddWithValue("@tagSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@tagName", DBNull.Value);
                                foreach (TTRPGTag tag in gear.Tags)
                                {
                                    cmd.Parameters["@tagSystemID"].Value = tag.SystemID;
                                    cmd.Parameters["@tagName"].Value = tag.TagName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachAttributes;
                                cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
                                cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                                cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeValue", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeUsed", DBNull.Value);
                                foreach (AttributeValueAndBool attr in gear.Attributes)
                                {
                                    cmd.Parameters["@attributeSystemID"].Value = attr.Attribute.SystemID;
                                    cmd.Parameters["@attributeName"].Value = attr.Attribute.AttributeName;
                                    cmd.Parameters["@attributeValue"].Value = attr.Value;
                                    cmd.Parameters["@attributeUsed"].Value = attr.BoolValue;
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

        public void Edit(TTRPGGear gear, TTRPGGear oldGear)
        {
            #region Command strings
            string editGear = "UPDATE Gear" +
                " SET SystemID=@systemID,GearName=@gearName,GearDescription=@description" +
                " WHERE SystemID=@oldSystemID AND GearName=@oldGearName";
            string attachTags = "INSERT INTO Gear_Tags(Gear_SystemID,GearName,Tags_SystemID,TagName)" +
                " VALUES(@systemID,@gearName,@tagSystemID,@tagName)";
            string removeTags = "DELETE FROM Gear_Tags" +
                " WHERE Gear_SystemID=@oldSystemID AND GearName=@oldGearName AND Tags_SystemID=@oldTagSystemID AND TagName=@oldTagName";
            string attachAttributes = "INSERT INTO Attributes_Gear(Gear_SystemID,GearName,Attributes_SystemID,AttributeName,AttributeValue,AttributeUsed)" +
                " VALUES(@systemID,@gearName,@attributeSystemID,@attributeName,@attributeValue,@attributeUsed)";
            string removeAttributes = "DELETE FROM Attributes_Gear" +
                " WHERE Gear_SystemID=@oldSystemID AND GearName=@oldGearName AND Attributes_SystemID=@oldAttributeSystemID AND AttributeName=@oldAttributeName";
            string editAttributeValues = "UPDATE Attributes_Gear" +
                " SET AttributeValue=@attributeValue, AttributeUsed=@attributeUsed" +
                " WHERE Gear_SystemID=@systemID AND GearName=@gearName AND Attributes_SystemID=@attributeSystemID AND AttributeName=@attributeName";
            #endregion
            #region Lists/Dictionaries of differences
            List<TTRPGTag> tagsToAdd = gear.Tags.Except(oldGear.Tags).ToList();
            List<TTRPGTag> tagsToRemove = oldGear.Tags.Except(gear.Tags).ToList();
            List<AttributeValueAndBool> attributesToAdd = new List<AttributeValueAndBool>();
            List<AttributeValueAndBool> attributesToRemove = new List<AttributeValueAndBool>();
            List<AttributeValueAndBool> attributesToEdit = new List<AttributeValueAndBool>();
            #endregion
            #region Populating dictionaries with differences (add/edit/remove)
            foreach (AttributeValueAndBool attr in gear.Attributes)
            {
                if (!oldGear.Attributes.Contains(attr))
                {
                    attributesToAdd.Add(attr);
                }
                else if (oldGear.Attributes.Contains(attr) && (oldGear.Attributes.Find(x => x == attr).Value != attr.Value || oldGear.Attributes.Find(x => x == attr).BoolValue != attr.BoolValue))
                {
                    attributesToEdit.Add(attr);
                }
            }
            foreach (AttributeValueAndBool attr in oldGear.Attributes)
            {
                if (!gear.Attributes.Contains(attr))
                {
                    attributesToRemove.Add(attr);
                }
            }
            #endregion
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
                            if (tagsToRemove.Count > 0)
                            {
                                cmd.CommandText = removeTags;
                                cmd.Parameters.AddWithValue("@oldGearName", oldGear.GearName);
                                cmd.Parameters.AddWithValue("@oldSystemID", oldGear.SystemID);
                                cmd.Parameters.AddWithValue("@oldTagSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@oldTagName", DBNull.Value);
                                foreach(TTRPGTag tag in tagsToRemove)
                                {
                                    cmd.Parameters["@oldTagSystemID"].Value = tag.SystemID;
                                    cmd.Parameters["@oldTagName"].Value = tag.TagName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                            }
                            if (attributesToRemove.Count > 0)
                            {
                                cmd.CommandText = removeAttributes;
                                cmd.Parameters.AddWithValue("@oldGearName", oldGear.GearName);
                                cmd.Parameters.AddWithValue("@oldSystemID", oldGear.SystemID);
                                cmd.Parameters.AddWithValue("@oldAttributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@oldAttributeName", DBNull.Value);
                                foreach (AttributeValueAndBool attr in attributesToRemove)
                                {
                                    cmd.Parameters["@oldAttributeSystemID"].Value = attr.Attribute.SystemID;
                                    cmd.Parameters["@oldAttributeName"].Value = attr.Attribute.AttributeName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                            }
                            cmd.CommandText = editGear;
                            cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                            cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
                            cmd.Parameters.AddWithValue("@oldGearName", gear.GearName);
                            cmd.Parameters.AddWithValue("@oldSystemID", gear.SystemID);
                            cmd.Parameters.AddWithValue("@description", gear.Description);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            if (tagsToAdd.Count > 0)
                            {
                                cmd.CommandText = attachTags;
                                cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
                                cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                                cmd.Parameters.AddWithValue("@tagSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@tagName", DBNull.Value);
                                foreach (TTRPGTag tag in tagsToAdd)
                                {
                                    cmd.Parameters["@tagSystemID"].Value = tag.SystemID;
                                    cmd.Parameters["@tagName"].Value = tag.TagName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                            }
                            if (attributesToAdd.Count > 0)
                            {
                                cmd.CommandText = attachAttributes;
                                cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
                                cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                                cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeValue", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeUsed", DBNull.Value);
                                foreach (AttributeValueAndBool attr in gear.Attributes)
                                {
                                    cmd.Parameters["@attributeSystemID"].Value = attr.Attribute.SystemID;
                                    cmd.Parameters["@attributeName"].Value = attr.Attribute.AttributeName;
                                    cmd.Parameters["@attributeValue"].Value = attr.Value;
                                    cmd.Parameters["@attributeUsed"].Value = attr.BoolValue;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                            }
                            if (attributesToEdit.Count > 0)
                            {
                                cmd.CommandText = editAttributeValues;
                                cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
                                cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                                cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeUsed", DBNull.Value);
                                foreach (AttributeValueAndBool attr in gear.Attributes)
                                {
                                    cmd.Parameters["@attributeSystemID"].Value = attr.Attribute.SystemID;
                                    cmd.Parameters["@attributeName"].Value = attr.Attribute.AttributeName;
                                    cmd.Parameters["@attributeValue"].Value = attr.Value;
                                    cmd.Parameters["@attributeUsed"].Value = attr.BoolValue;
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
                            if (tagsToRemove.Count > 0)
                            {
                                cmd.CommandText = removeTags;
                                cmd.Parameters.AddWithValue("@oldGearName", oldGear.GearName);
                                cmd.Parameters.AddWithValue("@oldSystemID", oldGear.SystemID);
                                cmd.Parameters.AddWithValue("@oldTagSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@oldTagName", DBNull.Value);
                                foreach (TTRPGTag tag in tagsToRemove)
                                {
                                    cmd.Parameters["@oldTagSystemID"].Value = tag.SystemID;
                                    cmd.Parameters["@oldTagName"].Value = tag.TagName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                            }
                            if (attributesToRemove.Count > 0)
                            {
                                cmd.CommandText = removeAttributes;
                                cmd.Parameters.AddWithValue("@oldGearName", oldGear.GearName);
                                cmd.Parameters.AddWithValue("@oldSystemID", oldGear.SystemID);
                                cmd.Parameters.AddWithValue("@oldAttributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@oldAttributeName", DBNull.Value);
                                foreach (AttributeValueAndBool attr in attributesToRemove)
                                {
                                    cmd.Parameters["@oldAttributeSystemID"].Value = attr.Attribute.SystemID;
                                    cmd.Parameters["@oldAttributeName"].Value = attr.Attribute.AttributeName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                            }
                            cmd.CommandText = editGear;
                            cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                            cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
                            cmd.Parameters.AddWithValue("@oldGearName", gear.GearName);
                            cmd.Parameters.AddWithValue("@oldSystemID", gear.SystemID);
                            cmd.Parameters.AddWithValue("@description", gear.Description);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            if (tagsToAdd.Count > 0)
                            {
                                cmd.CommandText = attachTags;
                                cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
                                cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                                cmd.Parameters.AddWithValue("@tagSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@tagName", DBNull.Value);
                                foreach (TTRPGTag tag in tagsToAdd)
                                {
                                    cmd.Parameters["@tagSystemID"].Value = tag.SystemID;
                                    cmd.Parameters["@tagName"].Value = tag.TagName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                            }
                            if (attributesToAdd.Count > 0)
                            {
                                cmd.CommandText = attachAttributes;
                                cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
                                cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                                cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeValue", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeUsed", DBNull.Value);
                                foreach (AttributeValueAndBool attr in gear.Attributes)
                                {
                                    cmd.Parameters["@attributeSystemID"].Value = attr.Attribute.SystemID;
                                    cmd.Parameters["@attributeName"].Value = attr.Attribute.AttributeName;
                                    cmd.Parameters["@attributeValue"].Value = attr.Value;
                                    cmd.Parameters["@attributeUsed"].Value = attr.BoolValue;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                            }
                            if (attributesToEdit.Count > 0)
                            {
                                cmd.CommandText = editAttributeValues;
                                cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
                                cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                                cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeUsed", DBNull.Value);
                                foreach (AttributeValueAndBool attr in gear.Attributes)
                                {
                                    cmd.Parameters["@attributeSystemID"].Value = attr.Attribute.SystemID;
                                    cmd.Parameters["@attributeName"].Value = attr.Attribute.AttributeName;
                                    cmd.Parameters["@attributeValue"].Value = attr.Value;
                                    cmd.Parameters["@attributeUsed"].Value = attr.BoolValue;
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

        public void Delete (TTRPGGear gear)
        {
            string deleteGear = "DELETE FROM Gear" +
                " WHERE SystemID=@systemID AND GearName=@gearName";
            try
            {
                //Add using sql if conencted to sql, else add using sqlite
                if (_userConnection.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(deleteGear, conn))
                        {
                            cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                            cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    using (SqliteConnection conn = new SqliteConnection(_userConnection.sqliteString))
                    {
                        conn.Open();
                        using (SqliteCommand cmd = new SqliteCommand(deleteGear, conn))
                        {
                            cmd.Parameters.AddWithValue("@gearName", gear.GearName);
                            cmd.Parameters.AddWithValue("@systemID", gear.SystemID);
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

        public ObservableCollection<TTRPGGear> GetGear(int systemID)
        {
            ObservableCollection<TTRPGGear> gear = new ObservableCollection<TTRPGGear>();
            string cmdString = "SELECT SystemID,GearName,GearDescription" +
                " FROM Gear WHERE SystemID=@systemID";
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
                                TTRPGGear temp = new TTRPGGear();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                temp.GearName = reader["GearName"].ToString()!;
                                temp.Description = reader["GearDescription"].ToString() ?? string.Empty;
                                gear.Add(temp);
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
                                TTRPGGear temp = new TTRPGGear();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                temp.GearName = reader["GearName"].ToString()!;
                                temp.Description = reader["GearDescription"].ToString() ?? string.Empty;
                                gear.Add(temp);
                            }
                        }
                    }
                }
                return gear;
            }
            catch (SqlException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new ObservableCollection<TTRPGGear>();
            }
            catch (SqliteException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new ObservableCollection<TTRPGGear>();
            }
        }
    }
}
