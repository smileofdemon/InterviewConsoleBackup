using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace InterviewConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var masterConnectionString = ConfigurationManager.ConnectionStrings["MasterDatabase"].ConnectionString;
            var testConnectionString = ConfigurationManager.ConnectionStrings["TestDatabase"].ConnectionString;

            CreateDatabase(masterConnectionString);
            CreateTable(testConnectionString);
            CreateUser(masterConnectionString);
            CreateEmployees(testConnectionString);

            var dtEmployees = GetQueryResult("SELECT * FROM Employee", testConnectionString);
            if(dtEmployees != null)
            {
                Console.WriteLine($"Contents of DataTable: {dtEmployees.TableName}");

                foreach (DataColumn col in dtEmployees.Columns)
                    Console.Write($"{col.ColumnName}\t");

                Console.WriteLine();
                foreach (DataRow row in dtEmployees.Rows)
                {
                    foreach (DataColumn col in dtEmployees.Columns)
                    {
                        Console.Write($"{row[col]}\t");
                    }
                    Console.WriteLine();
                }

                Console.ReadLine();
            }
        }
        
        private static DataTable GetQueryResult(string query, string connectionString)
        {
            try
            {
                var dt = new DataTable();
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        using (var adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }

                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static bool CreateDatabase(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand($"CREATE DATABASE test", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool CreateTable(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var createTableSql = @"
                    CREATE TABLE Employee (
                        ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(100) NOT NULL,
                        ManagerID INT NULL,
                        Enable BIT
                    );";

                    using (SqlCommand command = new SqlCommand(createTableSql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool CreateUser(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = null;
                try
                {
                    transaction = connection.BeginTransaction();

                    using (SqlCommand command = new SqlCommand("CREATE LOGIN testUser WITH PASSWORD = 'pass@word1', CHECK_POLICY = ON;", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (SqlCommand command = new SqlCommand("USE test; CREATE USER testUser FOR LOGIN testUser;", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (SqlCommand command = new SqlCommand("USE test; GRANT SELECT ON Employee TO testUser;", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction?.Rollback();
                    return false;
                }
            }

            return true;
        }

        private static bool CreateEmployees(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var insertQuery = "INSERT INTO Employee (Name, ManagerID, Enable) VALUES (@Name, @ManagerID, @Enable)";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", "Andrey");
                        command.Parameters.AddWithValue("@ManagerID", DBNull.Value);
                        command.Parameters.AddWithValue("@Enable", 1);

                        command.ExecuteNonQuery();
                    }

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", "Alexey");
                        command.Parameters.AddWithValue("@ManagerID", 2);
                        command.Parameters.AddWithValue("@Enable", 1);

                        command.ExecuteNonQuery();
                    }

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", "Roman");
                        command.Parameters.AddWithValue("@ManagerID", 2);
                        command.Parameters.AddWithValue("@Enable", 1);

                        command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
