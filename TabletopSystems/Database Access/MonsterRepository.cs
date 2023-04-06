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
    public class MonsterRepository
    {
        private UserConnection _userConnection;
        public MonsterRepository(UserConnection conn)
        {
            _userConnection = conn;
        }
        /// <summary>
        /// Add monster to database
        /// </summary>
        /// <param name="monster"></param>
        public void Add(TTRPGMonster monster)
        {
            string addToMonsters = "INSERT INTO Monsters(SystemID,MonsterName,StandardDamage)" +
                " VALUES(@systemID,@monsterName,@damage)";
            string attachTags = "INSERT INTO Monsters_Tags(Monsters_SystemID,MonsterName,Tags_SystemID,TagName)" +
                " VALUES(@systemID,@monsterName,@tagSystemID,@tagName)";
            string attachAttributes = "INSERT INTO Attributes_Monsters(Monsters_SystemID,MonsterName,Attributes_SystemID,AttributeName,AttributeValue)" +
                " VALUES(@systemID,@monsterName,@attributeSystemID,@attributeName,@attributeValue)";
            string attachGear = "INSERT INTO Monsters_Gear(Monsters_SystemID,MonsterName,Gear_SystemID,GearName)" +
                " VALUES(@systemID,@monsterName,@gearSystemID,@gearName)";
            string attachCapabilities = "INSERT INTO Monsters_Capabilities(Monsters_SystemID,MonsterName,Capabilities_SystemID,CapabilityName)" +
                " VALUES(@systemID,@monsterName,@capabilitySystemID,@capabilityName)";
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
                            using (SqlCommand cmd = new SqlCommand(addToMonsters, conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                cmd.Parameters.AddWithValue("@damage", monster.StandardDamage);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachTags;
                                cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                cmd.Parameters.AddWithValue("@tagSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@tagName", DBNull.Value);
                                foreach (TTRPGTag tag in monster.Tags)
                                {
                                    cmd.Parameters["@tagSystemID"].Value = tag.SystemID;
                                    cmd.Parameters["@tagName"].Value = tag.TagName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachAttributes;
                                cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeValue", DBNull.Value);
                                foreach (KeyValuePair<TTRPGAttribute, int> kvp in monster.Attributes)
                                {
                                    cmd.Parameters["@attributeSystemID"].Value = kvp.Key.SystemID;
                                    cmd.Parameters["@attributeName"].Value = kvp.Key.AttributeName;
                                    cmd.Parameters["@attributeValue"].Value = kvp.Value;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachGear;
                                cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                cmd.Parameters.AddWithValue("@gearSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@gearName", DBNull.Value);
                                foreach(TTRPGGear gear in monster.Gear)
                                {
                                    cmd.Parameters["@gearSystemID"].Value = gear.SystemID;
                                    cmd.Parameters["@gearName"].Value = gear.GearName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachCapabilities;
                                cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                foreach(TTRPGCapability capa in monster.Capabilities)
                                {
                                    cmd.Parameters["@capabilitySystemID"].Value = capa.SystemID;
                                    cmd.Parameters["@capabilityName"].Value = capa.CapabilityName;
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
                            using (SqliteCommand cmd = new SqliteCommand(addToMonsters, conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                cmd.Parameters.AddWithValue("@damage", monster.StandardDamage);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachTags;
                                cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                cmd.Parameters.AddWithValue("@tagSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@tagName", DBNull.Value);
                                foreach (TTRPGTag tag in monster.Tags)
                                {
                                    cmd.Parameters["@tagSystemID"].Value = tag.SystemID;
                                    cmd.Parameters["@tagName"].Value = tag.TagName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachAttributes;
                                cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeValue", DBNull.Value);
                                foreach (KeyValuePair<TTRPGAttribute, int> kvp in monster.Attributes)
                                {
                                    cmd.Parameters["@attributeSystemID"].Value = kvp.Key.SystemID;
                                    cmd.Parameters["@attributeName"].Value = kvp.Key.AttributeName;
                                    cmd.Parameters["@attributeValue"].Value = kvp.Value;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachGear;
                                cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                cmd.Parameters.AddWithValue("@gearSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@gearName", DBNull.Value);
                                foreach (TTRPGGear gear in monster.Gear)
                                {
                                    cmd.Parameters["@gearSystemID"].Value = gear.SystemID;
                                    cmd.Parameters["@gearName"].Value = gear.GearName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachCapabilities;
                                cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                foreach (TTRPGCapability capa in monster.Capabilities)
                                {
                                    cmd.Parameters["@capabilitySystemID"].Value = capa.SystemID;
                                    cmd.Parameters["@capabilityName"].Value = capa.CapabilityName;
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
        /// Edit monster in database
        /// </summary>
        /// <param name="oldMonster"></param>
        /// <param name="monster"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Edit (TTRPGMonster oldMonster, TTRPGMonster monster)
        {
            #region Command Strings
            string editMonster = "UPDATE Monsters" +
                " SET SystemID=@systemID MonsterName=@monsterName StandardDamage=@damage)" +
                " WHERE SystemID=@oldSystemID AND MonsterName=@oldMonsterName";
            string attachTags = "INSERT INTO Monsters_Tags(Monsters_SystemID,MonsterName,Tags_SystemID,TagName)" +
                " VALUES(@systemID,@monsterName,@tagSystemID,@tagName)";
            string removeTags = "DELETE FROM Monsters_Tags" +
                " WHERE Monsters_SystemID=@oldSystemID AND MonsterName=@oldMonsterName AND Tags_SystemID=@tagSystemID AND TagName=@tagName)";
            string attachAttributes = "INSERT INTO Attributes_Monsters(Monsters_SystemID,MonsterName,Attributes_SystemID,AttributeName,AttributeValue)" +
                " VALUES(@systemID,@monsterName,@attributeSystemID,@attributeName,@attributeValue)";
            string removeAttributes = "DELETE FROM Attributes_Monsters" +
                " WHERE Monsters_SystemID=@oldSystemID AND MonsterName=@oldMonsterName AND Attributes_SystemID=@attributeSystemID AND AttributeName=@attributeName)";
            string editAttributes = "UPDATE Attributes_Monsters" +
                " SET AttributeValue=@attributeValue" +
                " WHERE Monsters_SystemID=@oldSystemID AND MonsterName=@oldMonsterName AND Attributes_SystemID=@attributeSystemID AND AttributeName=@attributeName)";
            string attachGear = "INSERT INTO Monsters_Gear(Monsters_SystemID,MonsterName,Gear_SystemID,GearName)" +
                " VALUES(@systemID,@monsterName,@gearSystemID,@gearName)";
            string removeGear = "DELETE FROM Monsters_Gear" +
                " WHERE Monsters_SystemID=@oldSystemID AND MonsterName=@oldMonsterName AND Gear_SystemID=@gearSystemID AND GearName=@gearName)";
            string attachCapabilities = "INSERT INTO Monsters_Capabilities(Monsters_SystemID,MonsterName,Capabilities_SystemID,CapabilityName)" +
                " VALUES(@systemID,@monsterName,@capabilitySystemID,@capabilityName)";
            string removeCapabilities = "DELETE FROM Monsters_Capabilities" +
                " WHERE Monsters_SystemID=@oldSystemID AND MonsterName=@oldMonsterName AND Capabilities_SystemID=@capabilitySystemID AND CapabilityName=@capabilityName)";
            #endregion
            #region Lists/Dictionaries of differences
            List<TTRPGTag> tagsToAdd = monster.Tags.Except(oldMonster.Tags).ToList();
            List<TTRPGTag> tagsToRemove = oldMonster.Tags.Except(monster.Tags).ToList();
            List<TTRPGGear> gearToAdd = monster.Gear.Except(oldMonster.Gear).ToList();
            List<TTRPGGear> gearToRemove = oldMonster.Gear.Except(monster.Gear).ToList();
            List<TTRPGCapability> capabilitiesToAdd = monster.Capabilities.Except(oldMonster.Capabilities).ToList();
            List<TTRPGCapability> capabilitiesToRemove = oldMonster.Capabilities.Except(monster.Capabilities).ToList();
            Dictionary<TTRPGAttribute, int> attributesToAdd = new Dictionary<TTRPGAttribute, int>();
            Dictionary<TTRPGAttribute, int> attributesToRemove = new Dictionary<TTRPGAttribute, int>();
            Dictionary<TTRPGAttribute, int> attributesToEdit = new Dictionary<TTRPGAttribute, int>();
            #endregion
            #region Populating dictionaries with differences (add/edit/remove)
            foreach (KeyValuePair<TTRPGAttribute, int> kvp in monster.Attributes)
            {
                if (!oldMonster.Attributes.ContainsKey(kvp.Key))
                {
                    attributesToAdd.Add(kvp.Key, kvp.Value);
                }
                else if (oldMonster.Attributes.ContainsKey(kvp.Key) && oldMonster.Attributes[kvp.Key] != monster.Attributes[kvp.Key])
                {
                    attributesToEdit.Add(kvp.Key, kvp.Value);
                }
            }
            foreach (KeyValuePair<TTRPGAttribute, int> kvp in oldMonster.Attributes)
            {
                if (!monster.Attributes.ContainsKey(kvp.Key))
                {
                    attributesToRemove.Add(kvp.Key, kvp.Value);
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
                                    cmd.Parameters.AddWithValue("@oldMonsterName", oldMonster.MonsterName);
                                    cmd.Parameters.AddWithValue("@oldSystemID", oldMonster.SystemID);
                                    cmd.Parameters.AddWithValue("@tagSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@tagName", DBNull.Value);
                                    foreach (TTRPGTag tag in tagsToRemove)
                                    {
                                        cmd.Parameters["@tagSystemID"].Value = tag.SystemID;
                                        cmd.Parameters["@tagName"].Value = tag.TagName;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                if (attributesToRemove.Count > 0)
                                {
                                    cmd.CommandText = removeAttributes;
                                    cmd.Parameters.AddWithValue("@oldMonsterName", oldMonster.MonsterName);
                                    cmd.Parameters.AddWithValue("@oldSystemID", oldMonster.SystemID);
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
                                if (gearToRemove.Count > 0)
                                {
                                    cmd.CommandText = removeGear;
                                    cmd.Parameters.AddWithValue("@oldMonsterName", oldMonster.MonsterName);
                                    cmd.Parameters.AddWithValue("@oldSystemID", oldMonster.SystemID);
                                    cmd.Parameters.AddWithValue("@gearSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@gearName", DBNull.Value);
                                    foreach (TTRPGGear gear in gearToRemove)
                                    {
                                        cmd.Parameters["@gearSystemID"].Value = gear.SystemID;
                                        cmd.Parameters["@gearName"].Value = gear.GearName;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                if (capabilitiesToRemove.Count > 0)
                                {
                                    cmd.CommandText = removeCapabilities;
                                    cmd.Parameters.AddWithValue("@oldMonsterName", oldMonster.MonsterName);
                                    cmd.Parameters.AddWithValue("@oldSystemID", oldMonster.SystemID);
                                    cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                    foreach (TTRPGCapability capability in capabilitiesToRemove)
                                    {
                                        cmd.Parameters["@capabilitySystemID"].Value = capability.SystemID;
                                        cmd.Parameters["@capabilityName"].Value = capability.CapabilityName;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                cmd.CommandText = editMonster;
                                cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                cmd.Parameters.AddWithValue("@damage", monster.StandardDamage);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                if (tagsToAdd.Count > 0)
                                {
                                    cmd.CommandText = attachTags;
                                    cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                    cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
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
                                    cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                    cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
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
                                    cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                    cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
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
                                if (gearToAdd.Count > 0)
                                {
                                    cmd.CommandText = attachGear;
                                    cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                    cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                    cmd.Parameters.AddWithValue("@gearSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@gearName", DBNull.Value);
                                    foreach (TTRPGGear gear in gearToAdd)
                                    {
                                        cmd.Parameters["@gearSystemID"].Value = gear.SystemID;
                                        cmd.Parameters["@gearName"].Value = gear.GearName;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                if (capabilitiesToAdd.Count > 0)
                                {
                                    cmd.CommandText = attachCapabilities;
                                    cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                    cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                    cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                    foreach (TTRPGCapability capa in capabilitiesToAdd)
                                    {
                                        cmd.Parameters["@capabilitySystemID"].Value = capa.SystemID;
                                        cmd.Parameters["@capabilityName"].Value = capa.CapabilityName;
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
                                    cmd.Parameters.AddWithValue("@oldMonsterName", oldMonster.MonsterName);
                                    cmd.Parameters.AddWithValue("@oldSystemID", oldMonster.SystemID);
                                    cmd.Parameters.AddWithValue("@tagSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@tagName", DBNull.Value);
                                    foreach (TTRPGTag tag in tagsToRemove)
                                    {
                                        cmd.Parameters["@tagSystemID"].Value = tag.SystemID;
                                        cmd.Parameters["@tagName"].Value = tag.TagName;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                if (attributesToRemove.Count > 0)
                                {
                                    cmd.CommandText = removeAttributes;
                                    cmd.Parameters.AddWithValue("@oldMonsterName", oldMonster.MonsterName);
                                    cmd.Parameters.AddWithValue("@oldSystemID", oldMonster.SystemID);
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
                                if (gearToRemove.Count > 0)
                                {
                                    cmd.CommandText = removeGear;
                                    cmd.Parameters.AddWithValue("@oldMonsterName", oldMonster.MonsterName);
                                    cmd.Parameters.AddWithValue("@oldSystemID", oldMonster.SystemID);
                                    cmd.Parameters.AddWithValue("@gearSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@gearName", DBNull.Value);
                                    foreach (TTRPGGear gear in gearToRemove)
                                    {
                                        cmd.Parameters["@gearSystemID"].Value = gear.SystemID;
                                        cmd.Parameters["@gearName"].Value = gear.GearName;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                if (capabilitiesToRemove.Count > 0)
                                {
                                    cmd.CommandText = removeCapabilities;
                                    cmd.Parameters.AddWithValue("@oldMonsterName", oldMonster.MonsterName);
                                    cmd.Parameters.AddWithValue("@oldSystemID", oldMonster.SystemID);
                                    cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                    foreach (TTRPGCapability capability in capabilitiesToRemove)
                                    {
                                        cmd.Parameters["@capabilitySystemID"].Value = capability.SystemID;
                                        cmd.Parameters["@capabilityName"].Value = capability.CapabilityName;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                cmd.CommandText = editMonster;
                                cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                cmd.Parameters.AddWithValue("@damage", monster.StandardDamage);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                if (tagsToAdd.Count > 0)
                                {
                                    cmd.CommandText = attachTags;
                                    cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                    cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
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
                                    cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                    cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
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
                                    cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                    cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
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
                                if (gearToAdd.Count > 0)
                                {
                                    cmd.CommandText = attachGear;
                                    cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                    cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                    cmd.Parameters.AddWithValue("@gearSystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@gearName", DBNull.Value);
                                    foreach (TTRPGGear gear in gearToAdd)
                                    {
                                        cmd.Parameters["@gearSystemID"].Value = gear.SystemID;
                                        cmd.Parameters["@gearName"].Value = gear.GearName;
                                        cmd.ExecuteNonQuery();
                                    }
                                    cmd.Parameters.Clear();
                                }
                                if (capabilitiesToAdd.Count > 0)
                                {
                                    cmd.CommandText = attachCapabilities;
                                    cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                                    cmd.Parameters.AddWithValue("@monsterName", monster.MonsterName);
                                    cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                    foreach (TTRPGCapability capa in capabilitiesToAdd)
                                    {
                                        cmd.Parameters["@capabilitySystemID"].Value = capa.SystemID;
                                        cmd.Parameters["@capabilityName"].Value = capa.CapabilityName;
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
        /// <summary>
        /// Delete monster from database
        /// </summary>
        /// <param name="monster"></param>
        public void Delete (TTRPGMonster monster)
        {
            string deleteMonster = "DELETE FROM Monsters" +
                " WHERE SystemID=@systemID AND MonsterName=@monsterName";
            try
            {
                //Add using sql if conencted to sql, else add using sqlite
                if (_userConnection.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(deleteMonster, conn))
                        {
                            cmd.Parameters.AddWithValue("@gearName", monster.MonsterName);
                            cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    using (SqliteConnection conn = new SqliteConnection(_userConnection.sqliteString))
                    {
                        conn.Open();
                        using (SqliteCommand cmd = new SqliteCommand(deleteMonster, conn))
                        {
                            cmd.Parameters.AddWithValue("@gearName", monster.MonsterName);
                            cmd.Parameters.AddWithValue("@systemID", monster.SystemID);
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
        public ObservableCollection<TTRPGMonster> GetMonsters(int systemID)
        {
            ObservableCollection<TTRPGMonster> monsters = new ObservableCollection<TTRPGMonster>();
            string cmdString = "SELECT SystemID,MonsterName,StandardDamage" +
                " FROM Monsters WHERE SystemID=@systemID";
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
                                TTRPGMonster temp = new TTRPGMonster();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                temp.MonsterName = reader["MonsterName"].ToString()!;
                                int tempInt;
                                if (Int32.TryParse(reader["StandardDamage"].ToString(), out tempInt))
                                {
                                    temp.StandardDamage = tempInt;
                                }
                                else
                                {
                                    temp.StandardDamage = 0;
                                }
                                monsters.Add(temp);
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
                                TTRPGMonster temp = new TTRPGMonster();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                temp.MonsterName = reader["MonsterName"].ToString()!;
                                int tempInt;
                                if (Int32.TryParse(reader["StandardDamage"].ToString(), out tempInt))
                                {
                                    temp.StandardDamage = tempInt;
                                }
                                else
                                {
                                    temp.StandardDamage = 0;
                                }
                                monsters.Add(temp);
                            }
                        }
                    }
                }
                return monsters;
            }
            catch (SqlException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new ObservableCollection<TTRPGMonster>();
            }
            catch (SqliteException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new ObservableCollection<TTRPGMonster>();
            }
        }
    }
}
