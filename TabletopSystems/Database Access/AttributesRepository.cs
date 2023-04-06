

using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System;
using System.Security.Cryptography;
using System.Transactions;
using TabletopSystems.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Windows;

namespace TabletopSystems.Database_Access
{
    public class AttributesRepository
    {
        /// <summary>
        /// Add collection of TTRPGAttributes to database
        /// </summary>
        /// <param name="attributes">Attributes to add</param>
        /// <param name="userConn"></param>
        public void Add(ObservableCollection<TTRPGAttribute> attributes, UserConnection userConn)
        {
            string cmdString = "INSERT INTO Attributes(SystemID,AttributeName,AttributeFormula) VALUES(@systemID, @attributeName, @attributeFormula)";
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
                            cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                            cmd.Parameters.AddWithValue("@attributeFormula", DBNull.Value);
                            conn.Open();
                            foreach (TTRPGAttribute currentAttribute in attributes)
                            {
                                if (String.IsNullOrEmpty(currentAttribute.SystemID.ToString()) || String.IsNullOrEmpty(currentAttribute.AttributeName))
                                {
                                    break;
                                }
                                cmd.Parameters["@systemID"].Value = currentAttribute.SystemID;
                                cmd.Parameters["@attributeName"].Value = currentAttribute.AttributeName;
                                cmd.Parameters["@attributeFormula"].Value = currentAttribute.AttributeFormula;
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
                            cmd.Parameters.AddWithValue("@attributeName", DBNull.Value);
                            cmd.Parameters.AddWithValue("@attributeFormula", DBNull.Value);
                            conn.Open();
                            foreach (TTRPGAttribute currentAttribute in attributes)
                            {
                                cmd.Parameters["@systemID"].Value = currentAttribute.SystemID;
                                cmd.Parameters["@attributeName"].Value = currentAttribute.AttributeName;
                                cmd.Parameters["@attributeFormula"].Value = currentAttribute.AttributeFormula;
                                cmd.ExecuteNonQuery();
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

        public void Edit(TTRPGAttribute attribute, TTRPGAttribute oldAttribute, UserConnection userConn)
        {
            string cmdString = "UPDATE Attributes SET AttributeName=@attributeName, AttributeFormula=@attributeFormula " +
                "WHERE SystemID=@oldSystemID AND AttributeName=@oldAttributeName";
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
                            cmd.Parameters.AddWithValue("@oldSystemID", oldAttribute.SystemID);
                            cmd.Parameters.AddWithValue("@oldAttributeName", oldAttribute.AttributeName);
                            cmd.Parameters.AddWithValue("@attributeName", attribute.AttributeName);
                            cmd.Parameters.AddWithValue("@attributeFormula", attribute.AttributeFormula);
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
                            cmd.Parameters.AddWithValue("@oldSystemID", oldAttribute.SystemID);
                            cmd.Parameters.AddWithValue("@oldAttributeName", oldAttribute.AttributeName);
                            cmd.Parameters.AddWithValue("@attributeName", attribute.AttributeName);
                            cmd.Parameters.AddWithValue("@attributeFormula", attribute.AttributeFormula);
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
        /// Deletes a single TTRPGAttribute from the database
        /// </summary>
        /// <param name="attribute">attribute to delete</param>
        /// <param name="userConn"></param>
        public void Delete(TTRPGAttribute attribute, UserConnection userConn)
        {
            string cmdString = "DELETE FROM Attributes WHERE SystemID=@systemID AND AttributeName=@attributeName";
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
                            cmd.Parameters.AddWithValue("@systemID", attribute.SystemID);
                            cmd.Parameters.AddWithValue("@attributeName", attribute.AttributeName);
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
                            cmd.Parameters.AddWithValue("@systemID", attribute.SystemID);
                            cmd.Parameters.AddWithValue("@attributeName", attribute.AttributeName);
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
        /// Gets all TTRPGAttributes from database using a given SystemID
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="SystemID">ID of the system to get attributes from</param>
        /// <returns></returns>
        public ObservableCollection<TTRPGAttribute> GetTTRPGAttributes(UserConnection userConn, int systemID)
        {
            ObservableCollection<TTRPGAttribute> attrToReturn = new ObservableCollection<TTRPGAttribute>();
            string cmdString = "SELECT SystemID, AttributeName, AttributeFormula FROM Attributes WHERE SystemID=@systemID";
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
                                TTRPGAttribute temp = new TTRPGAttribute();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString());
                                temp.AttributeName = reader["AttributeName"].ToString()!;
                                temp.AttributeFormula = reader["AttributeFormula"].ToString() ?? string.Empty;
                                attrToReturn.Add(temp);
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
                                TTRPGAttribute temp = new TTRPGAttribute();
                                temp.SystemID = Int32.Parse(reader["SystemID"].ToString());
                                temp.AttributeName = reader["AttributeName"].ToString()!;
                                temp.AttributeFormula = reader["AttributeFormula"].ToString() ?? string.Empty;
                                attrToReturn.Add(temp);
                            }
                        }
                    }
                }
                return attrToReturn;
            }
            catch (SqlException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new ObservableCollection<TTRPGAttribute>();
            }
            catch (SqliteException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new ObservableCollection<TTRPGAttribute>();
            }
        }
    }
}
