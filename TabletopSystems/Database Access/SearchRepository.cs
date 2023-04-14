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
        public DataTable SearchDatabase(string query)
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
