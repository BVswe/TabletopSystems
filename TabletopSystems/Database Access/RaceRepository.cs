using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

        public void Delete(string raceName, int systemID)
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
                            cmd.Parameters.AddWithValue("@raceName", raceName);
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
                        using (SqliteCommand cmd = new SqliteCommand(deleteRace, conn))
                        {
                            cmd.Parameters.AddWithValue("@raceName", raceName);
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

        public TTRPGRace SearchRace(string raceName, int systemID)
        {
            TTRPGRace race = new TTRPGRace();
            string cmdString = "SELECT SystemID, RaceName, RaceDescription," +
                        " Coalesce((SELECT STRING_AGG(TagName, '|') as [Tags] FROM Races_Tags WHERE RaceName=Races.RaceName),'') as Tags," +
                        " Coalesce((SELECT STRING_AGG(Subrace, '|') as [Subraces] FROM (SELECT DISTINCT Subrace FROM Attributes_Races WHERE RaceName=Races.RaceName) as subs), '') as Subraces," +
                        " Coalesce((SELECT STRING_AGG(CapabilityName, '|') as [Capabilities] FROM Races_Capabilities WHERE RaceName=Races.RaceName GROUP BY RaceName),'') as Capabilities," +
                        " Coalesce((SELECT STRING_AGG(LevelGained, '|') as [LevelGained] FROM Races_Capabilities WHERE RaceName=Races.RaceName GROUP BY RaceName),'') as LevelGained" +
                        " FROM Races WHERE RaceName=@raceName AND SystemID=@systemID;";
            string subraceString = "SELECT AttributeName, AttributeValue" +
                " FROM Attributes_Races" +
                " WHERE RaceName=@raceName AND Races_SystemID=@systemID AND Subrace=@subrace";
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
                            cmd.Parameters.AddWithValue("@raceName", raceName);
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                race.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                race.RaceName = reader["RaceName"].ToString()!;
                                race.RaceDescription = reader["RaceDescription"].ToString() ?? string.Empty;
                                string temp = reader["Tags"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp))
                                {
                                    foreach (string s in temp.Split('|'))
                                    {
                                        TTRPGTag tempTag = new TTRPGTag();
                                        tempTag.TagName = s;
                                        tempTag.SystemID = race.SystemID;
                                        race.Tags.Add(tempTag);
                                    }
                                }
                                temp = reader["Subraces"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(reader["Subraces"].ToString()))
                                {
                                    foreach (string subrace in temp.Split('|'))
                                    {
                                        Dictionary<TTRPGAttribute, ObservableInt> subraceAttrDict = new Dictionary<TTRPGAttribute, ObservableInt>();
                                        using (SqlCommand subraceCommand = new SqlCommand(subraceString, conn))
                                        {
                                            subraceCommand.Parameters.AddWithValue("@raceName", raceName);
                                            subraceCommand.Parameters.AddWithValue("@systemID", systemID);
                                            subraceCommand.Parameters.AddWithValue("@subrace", subrace);
                                            SqlDataReader subraceReader = subraceCommand.ExecuteReader();
                                            while (subraceReader.Read())
                                            {
                                                subraceAttrDict.Add(new TTRPGAttribute()
                                                { AttributeName = subraceReader["AttributeName"].ToString()!, SystemID = race.SystemID },
                                                      new ObservableInt(Int32.Parse(subraceReader["AttributeValue"].ToString() ?? "0"))
                                                );
                                            }
                                        }
                                        race.SubraceAttributes.Add(new SubraceStats(subrace, subraceAttrDict));
                                    }
                                }

                                temp = reader["Capabilities"].ToString() ?? string.Empty;
                                string temp2 = reader["LevelGained"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp))
                                {
                                    List<string> capabilities = new List<string>(temp.Split("|"));
                                    List<string> levelGained = new List<string>(temp2.Split("|"));
                                    for (int i = 0; i < capabilities.Count; i++)
                                    {
                                        TTRPGCapability tempCapability = new TTRPGCapability() { CapabilityName = capabilities[i], SystemID = race.SystemID };
                                        race.Capabilities.Add(tempCapability, Int32.Parse(levelGained[i] ?? "0"));
                                    }
                                }
                            }
                            if (reader.Read()) throw new DataException("multiple rows returned from query");
                        }
                    }
                }
                else
                {
                    cmdString = "SELECT SystemID, RaceName, RaceDescription," +
                        " Coalesce((SELECT group_concat(TagName, '|') as [Tags] FROM Races_Tags WHERE RaceName=Races.RaceName),'') as Tags," +
                        " Coalesce((SELECT group_concat(Subrace, '|') as [Subraces] FROM (SELECT DISTINCT Subrace FROM Attributes_Races WHERE RaceName=Races.RaceName)), '') as Subraces," +
                        " Coalesce((SELECT group_concat(CapabilityName, '|') as [Capabilities] FROM Races_Capabilities WHERE RaceName=Races.RaceName GROUP BY RaceName),'') as Capabilities," +
                        " Coalesce((SELECT group_concat(LevelGained, '|') as [LevelGained] FROM Races_Capabilities WHERE RaceName=Races.RaceName GROUP BY RaceName),'') as LevelGained" +
                        " FROM Races WHERE RaceName=@raceName AND SystemID=@systemID;";
                    using (SqliteConnection conn = new SqliteConnection(_userConnection.sqliteString))
                    {
                        using (SqliteCommand cmd = new SqliteCommand(cmdString, conn))
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@systemID", systemID);
                            cmd.Parameters.AddWithValue("@raceName", raceName);
                            SqliteDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                race.SystemID = Int32.Parse(reader["SystemID"].ToString()!);
                                race.RaceName = reader["RaceName"].ToString()!;
                                race.RaceDescription = reader["RaceDescription"].ToString()!;
                                string temp = reader["Tags"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp))
                                {
                                    foreach (string s in temp.Split('|'))
                                    {
                                        TTRPGTag tempTag = new TTRPGTag();
                                        tempTag.TagName = s;
                                        tempTag.SystemID = race.SystemID;
                                        race.Tags.Add(tempTag);
                                    }
                                }
                                temp = reader["Subraces"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(reader["Subraces"].ToString()))
                                {
                                    foreach (string subrace in temp.Split('|'))
                                    {
                                        Dictionary<TTRPGAttribute, ObservableInt> subraceAttrDict = new Dictionary<TTRPGAttribute, ObservableInt>();
                                        using (SqliteCommand subraceCommand = new SqliteCommand(subraceString, conn))
                                        {
                                            subraceCommand.Parameters.AddWithValue("@raceName", raceName);
                                            subraceCommand.Parameters.AddWithValue("@systemID", systemID);
                                            subraceCommand.Parameters.AddWithValue("@subrace", subrace);
                                            SqliteDataReader subraceReader = subraceCommand.ExecuteReader();
                                            while (subraceReader.Read())
                                            {
                                                subraceAttrDict.Add(new TTRPGAttribute()
                                                { AttributeName = subraceReader["AttributeName"].ToString()!, SystemID = race.SystemID },
                                                      new ObservableInt(Int32.Parse(subraceReader["AttributeValue"].ToString() ?? "0"))
                                                );
                                            }
                                        }
                                        race.SubraceAttributes.Add(new SubraceStats(subrace, subraceAttrDict));
                                    }
                                }

                                temp = reader["Capabilities"].ToString() ?? string.Empty;
                                string temp2 = reader["LevelGained"].ToString() ?? string.Empty;
                                if (!String.IsNullOrEmpty(temp))
                                {
                                    List<string> capabilities = new List<string>(temp.Split("|"));
                                    List<string> levelGained = new List<string>(temp2.Split("|"));
                                    for (int i = 0; i < capabilities.Count; i++)
                                    {
                                        TTRPGCapability tempCapability = new TTRPGCapability() { CapabilityName = capabilities[i], SystemID = race.SystemID };
                                        race.Capabilities.Add(tempCapability, Int32.Parse(levelGained[i] ?? "0"));
                                    }
                                }
                            }
                            if (reader.Read()) throw new DataException("multiple rows returned from query");
                        }
                    }
                }
                return race;
            }
            catch (SqlException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new TTRPGRace();
            }
            catch (SqliteException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new TTRPGRace();
            }
        }
    }
}
