using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TabletopSystems.Helper_Methods;
using TabletopSystems.Models;

namespace TabletopSystems.Database_Access
{
    public class RaceRepository
    {
        private UserConnection _userConnection;
        public RaceRepository(UserConnection conn)
        {
            _userConnection = conn;
        }
        public void Add(TTRPGRace race)
        {
            string addToRaces = "INSERT INTO Races(SystemID,RaceName,RaceDescription)" +
                " VALUES(@systemID,@raceName,@raceDescription)";
            string attachTags = "INSERT INTO Races_Tags(Races_SystemID,RaceName,Tags_SystemID,TagName)" +
                " VALUES(@systemID,@raceName,@tagSystemID,@tagName)";
            string attachAttributes = "INSERT INTO Attributes_Races(Races_SystemID,RaceName,Attributes_SystemID,AttributeName,AttributeValue,SubRace)" +
                " VALUES(@systemID,@raceName,@attributeSystemID,@attributeName,@attributeValue,@subrace)";
            string attachCapabilities = "INSERT INTO Races_Capabilities(Races_SystemID,RaceName,Capabilities_SystemID,CapabilityName,LevelGained)" +
                " VALUES(@systemID,@raceName,@capabilitySystemID,@capabilityName,@levelGained)";
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
                            using (SqlCommand cmd = new SqlCommand(addToRaces, conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@raceName", race.RaceName);
                                cmd.Parameters.AddWithValue("@systemID", race.SystemID);
                                cmd.Parameters.AddWithValue("@raceDescription", race.RaceDescription);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachTags;
                                cmd.Parameters.AddWithValue("@systemID", race.SystemID);
                                cmd.Parameters.AddWithValue("@raceName", race.RaceName);
                                cmd.Parameters.AddWithValue("@tagSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@tagName", DBNull.Value);
                                foreach (TTRPGTag tag in race.Tags)
                                {
                                    cmd.Parameters["@tagSystemID"].Value = tag.SystemID;
                                    cmd.Parameters["@tagName"].Value = tag.TagName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachAttributes;
                                cmd.Parameters.AddWithValue("@systemID", race.SystemID);
                                cmd.Parameters.AddWithValue("@raceName", race.RaceName);
                                cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeValue", DBNull.Value);
                                cmd.Parameters.AddWithValue("@subrace", DBNull.Value);
                                foreach (SubraceStats subrace in race.SubraceAttributes)
                                {
                                    cmd.Parameters["@subrace"].Value = subrace.SubraceName;
                                    foreach (KeyValuePair<TTRPGAttribute, ObservableInt> kvp in subrace.SubraceAttributes)
                                    {
                                        if (kvp.Value.IntValue == 0)
                                        {
                                            continue;
                                        }
                                        cmd.Parameters["@attributeSystemID"].Value = kvp.Key.SystemID;
                                        cmd.Parameters["@attributeName"].Value = kvp.Key.AttributeName;
                                        cmd.Parameters["@attributeValue"].Value = kvp.Value.IntValue;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachCapabilities;
                                cmd.Parameters.AddWithValue("@systemID", race.SystemID);
                                cmd.Parameters.AddWithValue("@raceName", race.RaceName);
                                cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@levelGained", DBNull.Value);
                                foreach (KeyValuePair<TTRPGCapability, int> kvp in race.Capabilities)
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
                            using (SqliteCommand cmd = new SqliteCommand(addToRaces, conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@raceName", race.RaceName);
                                cmd.Parameters.AddWithValue("@systemID", race.SystemID);
                                cmd.Parameters.AddWithValue("@raceDescription", race.RaceDescription);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachTags;
                                cmd.Parameters.AddWithValue("@systemID", race.SystemID);
                                cmd.Parameters.AddWithValue("@raceName", race.RaceName);
                                cmd.Parameters.AddWithValue("@tagSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@tagName", DBNull.Value);
                                foreach (TTRPGTag tag in race.Tags)
                                {
                                    cmd.Parameters["@tagSystemID"].Value = tag.SystemID;
                                    cmd.Parameters["@tagName"].Value = tag.TagName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachAttributes;
                                cmd.Parameters.AddWithValue("@systemID", race.SystemID);
                                cmd.Parameters.AddWithValue("@raceName", race.RaceName);
                                cmd.Parameters.AddWithValue("@attributeSystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@attributeValue", DBNull.Value);
                                cmd.Parameters.AddWithValue("@subrace", DBNull.Value);
                                foreach (SubraceStats subrace in race.SubraceAttributes)
                                {
                                    cmd.Parameters["@subrace"].Value = subrace.SubraceName;
                                    foreach (KeyValuePair<TTRPGAttribute, ObservableInt> kvp in subrace.SubraceAttributes)
                                    {
                                        if (kvp.Value.IntValue == 0)
                                        {
                                            continue;
                                        }
                                        cmd.Parameters["@attributeSystemID"].Value = kvp.Key.SystemID;
                                        cmd.Parameters["@attributeName"].Value = kvp.Key.AttributeName;
                                        cmd.Parameters["@attributeValue"].Value = kvp.Value.IntValue;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                cmd.Parameters.Clear();
                                cmd.CommandText = attachCapabilities;
                                cmd.Parameters.AddWithValue("@systemID", race.SystemID);
                                cmd.Parameters.AddWithValue("@raceName", race.RaceName);
                                cmd.Parameters.AddWithValue("@capabilitySystemID", DBNull.Value);
                                cmd.Parameters.AddWithValue("@capabilityName", DBNull.Value);
                                cmd.Parameters.AddWithValue("@levelGained", DBNull.Value);
                                foreach (KeyValuePair<TTRPGCapability, int> kvp in race.Capabilities)
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

        public void Delete(TTRPGRace race)
        {
            string deleteRace = "DELETE FROM Races" +
                " WHERE SystemID=@systemID AND RaceName=@raceName";
            try
            {
                //Add using sql if conencted to sql, else add using sqlite
                if (_userConnection.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(deleteRace, conn))
                        {
                            cmd.Parameters.AddWithValue("@raceName", race.RaceName);
                            cmd.Parameters.AddWithValue("@systemID", race.SystemID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    using (SqliteConnection conn = new SqliteConnection(_userConnection.sqliteString))
                    {
                        conn.Open();
                        using (SqliteCommand cmd = new SqliteCommand(deleteRace, conn))
                        {
                            cmd.Parameters.AddWithValue("@raceName", race.RaceName);
                            cmd.Parameters.AddWithValue("@systemID", race.SystemID);
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

        public ObservableCollection<TTRPGRace> GetRaces(int systemID)
        {
            ObservableCollection<TTRPGRace> races = new ObservableCollection<TTRPGRace>();
            string cmdString = "SELECT SystemID,RaceName,RaceDescription" +
                " FROM Races WHERE SystemID=@systemID";
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
                                TTRPGRace temp = new TTRPGRace();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                temp.RaceName = reader["RaceName"].ToString()!;
                                temp.RaceDescription = reader["RaceDescription"].ToString() ?? string.Empty;
                                races.Add(temp);
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
                                TTRPGRace temp = new TTRPGRace();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                temp.RaceName = reader["RaceName"].ToString()!;
                                temp.RaceDescription = reader["RaceDescription"].ToString() ?? string.Empty;
                                races.Add(temp);
                            }
                        }
                    }
                }
                return races;
            }
            catch (SqlException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new ObservableCollection<TTRPGRace>();
            }
            catch (SqliteException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new ObservableCollection<TTRPGRace>();
            }
        }
    }
}
