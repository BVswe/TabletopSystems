using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TabletopSystems;
using TabletopSystems.Models;

namespace TabletopTags.Database_Access
{
    public class TagRepository
    {
        private UserConnection _userConnection;
        public TagRepository(UserConnection conn)
        {
            _userConnection = conn;
        }
        /// <summary>
        /// Adds a system to the database
        /// </summary>
        /// <param name="tagToAdd"></param>
        public void Add(TTRPGTag tagToAdd)
        {
            string cmdString = "INSERT INTO Tags(TagName, SystemID) VALUES (@tagName,@systemID)";
            try
            {
                if (_userConnection.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                    {
                        using (SqlCommand cmd = new SqlCommand(cmdString, conn))
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@tagName", tagToAdd.TagName);
                            cmd.Parameters.AddWithValue("@systemID", tagToAdd.SystemID);
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
                            cmd.Parameters.AddWithValue("@tagName", tagToAdd.TagName);
                            cmd.Parameters.AddWithValue("@systemID", tagToAdd.SystemID);
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
        /// Edits a system in the database
        /// </summary>
        /// <param name="tagToAdd">System containing NEW tagName and OLD SystemID</param>
        public void EdittagName(TTRPGTag tagToEdit)
        {
            string cmdString = "UPDATE Tags SET TagName=@tagName WHERE SystemID=@systemID";
            try
            {
                if (_userConnection.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                    {
                        using (SqlCommand cmd = new SqlCommand(cmdString, conn))
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@tagName", tagToEdit.TagName);
                            cmd.Parameters.AddWithValue("@systemID", tagToEdit.SystemID);
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
                            cmd.Parameters.AddWithValue("@tagName", tagToEdit.TagName);
                            cmd.Parameters.AddWithValue("@systemID", tagToEdit.SystemID);
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
        /// Deletes a system from the database
        /// </summary>
        /// <param name="objectToRemove"></param>
        public void Delete(TTRPGTag tagToDelete)
        {
            string cmdString = "DELETE FROM Tags WHERE TagName=@tagName AND SystemID=@systemID";
            try
            {
                if (_userConnection.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                    {
                        using (SqlCommand cmd = new SqlCommand(cmdString, conn))
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@tagName", tagToDelete.TagName);
                            cmd.Parameters.AddWithValue("@systemID", tagToDelete.SystemID);
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
                            cmd.Parameters.AddWithValue("@tagName", tagToDelete.TagName);
                            cmd.Parameters.AddWithValue("@systemID", tagToDelete.SystemID);
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
        /// Gets all Tags from the database
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<TTRPGTag> GetTags()
        {
            string cmdString = "SELECT * FROM Tags";
            ObservableCollection<TTRPGTag> Tags = new ObservableCollection<TTRPGTag>();
            try
            {
                if (_userConnection.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                    {
                        using (SqlCommand cmd = new SqlCommand(cmdString, conn))
                        {
                            conn.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                TTRPGTag temp = new TTRPGTag();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString());
                                temp.TagName = reader["tagName"].ToString();
                                Tags.Add(temp);
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
                            SqliteDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                TTRPGTag temp = new TTRPGTag();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString());
                                temp.TagName = reader["tagName"].ToString();
                                Tags.Add(temp);
                            }
                        }
                    }
                }
                return Tags;
            }
            catch (SqlException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new ObservableCollection<TTRPGTag>();
            }
            catch (SqliteException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new ObservableCollection<TTRPGTag>();
            }
        }
    }
}
