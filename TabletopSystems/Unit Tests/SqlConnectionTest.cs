using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace TabletopSystems;

public class SqlConnectionTest
{
    public SqlConnectionTest()
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = "DESKTOP-1RS8GPM";
            builder.InitialCatalog = "TabletopSystems";
            builder.IntegratedSecurity = true;
            builder.Encrypt = false;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql = "INSERT INTO Systems VALUES( @test )";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@test", "'test'");
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
