using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
            string addToMonsters = "INSERT INTO Monsters(SystemID,MonsterName,StandardDamage,HP)" +
                " VALUES(@systemID,@monsterName,@damage,@hp)";
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
                                cmd.Parameters.AddWithValue("@hp", monster.HP);
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
                                cmd.Parameters.AddWithValue("@hp", monster.HP);
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
                " SET SystemID=@systemID MonsterName=@monsterName StandardDamage=@damage,HP=@hp)" +
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
                                cmd.Parameters.AddWithValue("@hp", monster.HP);
                                cmd.Parameters.AddWithValue("@oldMonsterName", monster.MonsterName);
                                cmd.Parameters.AddWithValue("@oldSystemID", monster.SystemID);
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
                                cmd.Parameters.AddWithValue("@hp", monster.HP);
                                cmd.Parameters.AddWithValue("@oldMonsterName", monster.MonsterName);
                                cmd.Parameters.AddWithValue("@oldSystemID", monster.SystemID);
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
        public void Delete (string monsterName, int systemID)
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
                            cmd.Parameters.AddWithValue("@monsterName", monsterName);
                            cmd.Parameters.AddWithValue("@systemID", systemID);
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
                            cmd.Parameters.AddWithValue("@monsterName", monsterName);
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
        public ObservableCollection<TTRPGMonster> GetMonsters(int systemID)
        {
            ObservableCollection<TTRPGMonster> monsters = new ObservableCollection<TTRPGMonster>();
            string cmdString = "SELECT SystemID,MonsterName,StandardDamage,HP" +
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
                                if (Int32.TryParse(reader["HP"].ToString(), out tempInt))
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
                                if (Int32.TryParse(reader["HP"].ToString(), out tempInt))
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

        public TTRPGMonster SearchMonster(string monsterName, int systemID)
        {
            TTRPGMonster monster = new TTRPGMonster();
            string cmdString = "SELECT SystemID, MonsterName, StandardDamage, HP," +
                " Coalesce((SELECT STRING_AGG(TagName, '|') as [Tags] FROM Monsters_Tags WHERE MonsterName=Monsters.MonsterName),'') as Tags," +
                " Coalesce((SELECT STRING_AGG(AttributeName, '|') as [Attributes] FROM Attributes_Monsters WHERE MonsterName=Monsters.MonsterName GROUP BY MonsterName),'') as Attributes," +
                " Coalesce((SELECT STRING_AGG(AttributeValue, '|') as [AttributeValues] FROM Attributes_Monsters WHERE MonsterName=Monsters.MonsterName GROUP BY MonsterName),'') as AttributeValues," +
                " Coalesce((SELECT STRING_AGG(GearName, '|') as [Gear] FROM Monsters_Gear WHERE MonsterName=Monsters.MonsterName GROUP BY MonsterName),'') as Gear," +
                " Coalesce((SELECT STRING_AGG(CapabilityName, '|') as [Capabilities] FROM Monsters_Capabilities WHERE MonsterName=Monsters.MonsterName GROUP BY MonsterName),'') as Capabilities" +
                " FROM Monsters WHERE MonsterName=@monsterName AND SystemID=@systemID";
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
                            cmd.Parameters.AddWithValue("@monsterName", monsterName);
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                monster.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                monster.MonsterName = reader["MonsterName"].ToString()!;
                                monster.StandardDamage = Int32.Parse(reader["StandardDamage"].ToString() ?? "0");
                                monster.HP = Int32.Parse(reader["HP"].ToString() ?? "0");
                                string temp = reader["Tags"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp))
                                {
                                    foreach (string s in temp.Split('|'))
                                    {
                                        TTRPGTag tempTag = new TTRPGTag();
                                        tempTag.TagName = s;
                                        tempTag.SystemID = monster.SystemID;
                                        monster.Tags.Add(tempTag);
                                    }
                                }
                                temp = reader["Attributes"].ToString() ?? string.Empty;
                                string temp2 = reader["AttributeValues"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp) && !String.IsNullOrEmpty(temp2))
                                {
                                    string[] attributeList = temp.Split('|');
                                    string[] valueList = temp2.Split('|');
                                    for (int i = 0; i < attributeList.Length; i++)
                                    {
                                        TTRPGAttribute tempAttribute = new TTRPGAttribute { SystemID = monster.SystemID, AttributeName = attributeList[i] };
                                        monster.Attributes.Add(tempAttribute, Int32.Parse(valueList[i] ?? "0"));
                                    }
                                }
                                temp = reader["Gear"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp))
                                {
                                    foreach (string s in temp.Split('|'))
                                    {
                                        TTRPGGear tempGear = new TTRPGGear() { GearName = s, SystemID=monster.SystemID };
                                        monster.Gear.Add(tempGear);
                                    }
                                }
                                temp = reader["Capabilities"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp))
                                {
                                    foreach (string s in temp.Split('|'))
                                    {
                                        TTRPGCapability tempCapability = new TTRPGCapability() { CapabilityName = s, SystemID = monster.SystemID };
                                        monster.Capabilities.Add(tempCapability);
                                    }
                                }

                            }
                            if (reader.Read()) throw new DataException("multiple rows returned from query");
                        }
                    }
                }
                else
                {
                    cmdString = "SELECT SystemID, MonsterName, StandardDamage, HP," +
                    " Coalesce((SELECT group_concat(TagName, '|') as [Tags] FROM Monsters_Tags WHERE MonsterName=Monsters.MonsterName),'') as Tags," +
                    " Coalesce((SELECT group_concat(AttributeName, '|') as [Attributes] FROM Attributes_Monsters WHERE MonsterName=Monsters.MonsterName GROUP BY MonsterName),'') as Attributes," +
                    " Coalesce((SELECT group_concat(AttributeValue, '|') as [AttributeValues] FROM Attributes_Monsters WHERE MonsterName=Monsters.MonsterName GROUP BY MonsterName),'') as AttributeValues," +
                    " Coalesce((SELECT group_concat(GearName, '|') as [Gear] FROM Monsters_Gear WHERE MonsterName=Monsters.MonsterName GROUP BY MonsterName),'') as Gear," +
                    " Coalesce((SELECT group_concat(CapabilityName, '|') as [Capabilities] FROM Monsters_Capabilities WHERE MonsterName=Monsters.MonsterName GROUP BY MonsterName),'') as Capabilities" +
                    " FROM Monsters WHERE MonsterName=@monsterName AND SystemID=@systemID";
                    using (SqliteConnection conn = new SqliteConnection(_userConnection.sqliteString))
                    {
                        using (SqliteCommand cmd = new SqliteCommand(cmdString, conn))
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@systemID", systemID);
                            cmd.Parameters.AddWithValue("@monsterName", monsterName);
                            SqliteDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                monster.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                monster.MonsterName = reader["MonsterName"].ToString()!;
                                monster.StandardDamage = Int32.Parse(reader["StandardDamage"].ToString() ?? "0");
                                monster.HP = Int32.Parse(reader["HP"].ToString() ?? "0");
                                string temp = reader["Tags"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp))
                                {
                                    foreach (string s in temp.Split('|'))
                                    {
                                        TTRPGTag tempTag = new TTRPGTag();
                                        tempTag.TagName = s;
                                        tempTag.SystemID = monster.SystemID;
                                        monster.Tags.Add(tempTag);
                                    }
                                }
                                temp = reader["Attributes"].ToString() ?? string.Empty;
                                string temp2 = reader["AttributeValues"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp) && !String.IsNullOrEmpty(temp2))
                                {
                                    string[] attributeList = temp.Split('|');
                                    string[] valueList = temp2.Split('|');
                                    for (int i = 0; i < attributeList.Length; i++)
                                    {
                                        TTRPGAttribute tempAttribute = new TTRPGAttribute { SystemID = monster.SystemID, AttributeName = attributeList[i] };
                                        monster.Attributes.Add(tempAttribute, Int32.Parse(valueList[i] ?? "0"));
                                    }
                                }
                                temp = reader["Gear"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp))
                                {
                                    foreach (string s in temp.Split('|'))
                                    {
                                        TTRPGGear tempGear = new TTRPGGear() { GearName = s, SystemID = monster.SystemID };
                                        monster.Gear.Add(tempGear);
                                    }
                                }
                                temp = reader["Capabilities"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp))
                                {
                                    foreach (string s in temp.Split('|'))
                                    {
                                        TTRPGCapability tempCapability = new TTRPGCapability() { CapabilityName = s, SystemID = monster.SystemID };
                                        monster.Capabilities.Add(tempCapability);
                                    }
                                }

                            }
                            if (reader.Read()) throw new DataException("multiple rows returned from query");
                        }
                    }
                }
                return monster;
            }
            catch (SqlException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new TTRPGMonster();
            }
            catch (SqliteException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new TTRPGMonster();
            }
        }
    }
}
