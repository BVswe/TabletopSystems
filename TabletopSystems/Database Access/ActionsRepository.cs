

using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System;
using TabletopSystems.Models;

namespace TabletopSystems.Database_Access
{
    public class ActionsRepository
    {
        /// <summary>
        /// Add collection of TTRPGActions to database
        /// </summary>
        /// <param name="actions">Actions to add</param>
        /// <param name="userConn"></param>
        public void Add(ObservableCollection<TTRPGAction> actions, UserConnection userConn)
        {
            string cmdString = "INSERT INTO Actions(SystemID,ActionName,ActionFormula) VALUES(@systemID, @actionName,@actionFormula)";
            try
            {
                if (userConn.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(userConn.sqlString))
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = cmdString;
                            cmd.Parameters.AddWithValue("@systemID", DBNull.Value);
                            cmd.Parameters.AddWithValue("@actionName", DBNull.Value);
                            cmd.Parameters.AddWithValue("@actionFormula", DBNull.Value);
                            conn.Open();
                            foreach (TTRPGAction currentAction in actions)
                            {
                                cmd.Parameters["@systemID"].Value = currentAction.SystemID;
                                cmd.Parameters["@actionName"].Value = currentAction.ActionName;
                                cmd.Parameters["@actionFormula"].Value = currentAction.ActionFormula;
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                else
                {
                    using (SqliteConnection conn = new SqliteConnection(userConn.sqliteString))
                    {
                        using (SqliteCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = cmdString;
                            cmd.Parameters.AddWithValue("@systemID", DBNull.Value);
                            cmd.Parameters.AddWithValue("@actionName", DBNull.Value);
                            cmd.Parameters.AddWithValue("@actionFormula", DBNull.Value);
                            conn.Open();
                            foreach (TTRPGAction currentAction in actions)
                            {
                                cmd.Parameters["@systemID"].Value = currentAction.SystemID;
                                cmd.Parameters["@actionName"].Value = currentAction.ActionName;
                                cmd.Parameters["@actionFormula"].Value = currentAction.ActionFormula;
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Trace.WriteLine("An exception occured: " + e.ToString());
            }
        }

        /// <summary>
        /// Edit a TTRPGAction in the database
        /// </summary>
        /// <param name="action">New Action information</param>
        /// <param name="oldAction">Old Action information</param>
        /// <param name="userConn"></param>
        public void Edit(TTRPGAction action, TTRPGAction oldAction, UserConnection userConn)
        {
            string cmdString = "UPDATE Actions SET ActionName=@actionName, ActionFormula=@actionFormula " +
                "WHERE SystemID=@oldSystemID AND ActionName=@oldActionName";
            try
            {
                if (userConn.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(userConn.sqlString))
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = cmdString;
                            conn.Open();
                            cmd.Parameters.AddWithValue("@oldSystemID", oldAction.SystemID);
                            cmd.Parameters.AddWithValue("@oldActionName", oldAction.ActionName);
                            cmd.Parameters.AddWithValue("@actionName", action.ActionName);
                            cmd.Parameters.AddWithValue("@actionFormula", action.ActionFormula);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    using (SqliteConnection conn = new SqliteConnection(userConn.sqliteString))
                    {
                        using (SqliteCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = cmdString;
                            conn.Open();
                            cmd.Parameters.AddWithValue("@oldSystemID", oldAction.SystemID);
                            cmd.Parameters.AddWithValue("@oldActionName", oldAction.ActionName);
                            cmd.Parameters.AddWithValue("@actionName", action.ActionName);
                            cmd.Parameters.AddWithValue("@actionFormula", action.ActionFormula);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Trace.WriteLine("An exception occured: " + e.ToString());
            }
        }

        /// <summary>
        /// Deletes a TTRPGAction from the database
        /// </summary>
        /// <param name="action">Action to delete</param>
        /// <param name="userConn"></param>
        public void Delete(TTRPGAction action, UserConnection userConn)
        {
            string cmdString = "DELETE FROM Actions WHERE SystemID=@systemID AND ActionName=@actionName";
            try
            {
                if (userConn.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(userConn.sqlString))
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = cmdString;
                            conn.Open();
                            cmd.Parameters.AddWithValue("@systemID", action.SystemID);
                            cmd.Parameters.AddWithValue("@actionName", action.ActionName);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    using (SqliteConnection conn = new SqliteConnection(userConn.sqliteString))
                    {
                        using (SqliteCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = cmdString;
                            conn.Open();
                            cmd.Parameters.AddWithValue("@systemID", action.SystemID);
                            cmd.Parameters.AddWithValue("@actionName", action.ActionName);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Trace.WriteLine("An exception occured: " + e.ToString());
            }
        }
        /// <summary>
        /// Gets all TTRPGActions from database using a given SystemID
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="SystemID">ID of the system to get actions from</param>
        /// <returns></returns>
        public ObservableCollection<TTRPGAction> GetTTRPGActions(UserConnection userConn, int systemID)
        {
            ObservableCollection<TTRPGAction> actionsToReturn = new ObservableCollection<TTRPGAction>();
            string cmdString = "SELECT ActionName, ActionFormula FROM Actions WHERE SystemID=@systemID";
            try
            {
                if (userConn.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(userConn.sqlString))
                    {
                        using (SqlCommand cmd = new SqlCommand(cmdString, conn))
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@systemID", systemID);
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                TTRPGAction temp = new TTRPGAction();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString());
                                temp.ActionName = reader["ActionName"].ToString();
                                temp.ActionFormula = reader["ActionFormula"].ToString();
                                actionsToReturn.Add(temp);
                            }
                        }
                    }
                }
                else
                {
                    using (SqliteConnection conn = new SqliteConnection(userConn.sqliteString))
                    {
                        using (SqliteCommand cmd = new SqliteCommand(cmdString, conn))
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@systemID", systemID);
                            SqliteDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                TTRPGAction temp = new TTRPGAction();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString());
                                temp.ActionName = reader["ActionName"].ToString();
                                temp.ActionFormula = reader["ActionFormula"].ToString();
                                actionsToReturn.Add(temp);
                            }
                        }
                    }
                }
                return actionsToReturn;
            }
            catch (SqlException e)
            {
                Trace.WriteLine("An exception occured: " + e.ToString());
                return new ObservableCollection<TTRPGAction>();
            }
        }
    }
}
