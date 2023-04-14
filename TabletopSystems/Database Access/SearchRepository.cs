using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TabletopSystems.Database_Access
{
    public class SearchRepository
    {
        private UserConnection _userConnection;
        public SearchRepository(UserConnection conn)
        {
            _userConnection = conn;
        }
        /// <summary>
        /// Searches database using given query and search term
        /// </summary>
        /// <param name="query">Query to be executed</param>
        /// <param name="searchTerm">Term to be searched</param>
        /// <returns></returns>
        public DataTable SearchDatabase(string query, string searchTerm)
        {
            DataTable table = new DataTable();
            try
            {
                if (_userConnection.connectedToSqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(_userConnection.sqlString))
                    {
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                        {
                            adapter.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                            adapter.Fill(table);
                        }
                    }
                }
                else
                {
                    using (SqliteConnection conn = new SqliteConnection(_userConnection.sqlString))
                    {
                        conn.Open();
                        using (SqliteCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                            using (SqliteDataReader reader = cmd.ExecuteReader())
                            {
                                table.Load(reader);
                            }
                        }
                        
                    }
                }
                return table;
            }
            catch (SqlException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new DataTable();
            }
            catch (SqliteException e)
            {
                MessageBox.Show("An exception occured: " + e.ToString());
                return new DataTable();
            }
        }
    }
}
