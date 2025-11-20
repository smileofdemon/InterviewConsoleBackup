using EmployeeService.Models;
using EmployeeService.Repositories.Interfaces;
using System;
using System.Data.SqlClient;

namespace EmployeeService.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public EmployeeDto GetById(string id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT Id, ManagerId, Name, Enable FROM Employee WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;

                    return new EmployeeDto
                    {
                        Id = (int)reader["Id"],
                        ManagerId = reader["ManagerId"] != DBNull.Value ? (int?)reader["ManagerId"] : null,
                        Name = reader["Name"].ToString(),
                        IsEnabled = (bool)reader["Enable"]
                    };
                }
            }
        }

        public void Enable(string id, EmployeeEnableDto employeeEnable)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("UPDATE Employee SET Enable = @IsEnabled WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IsEnabled", employeeEnable.IsEnabled);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}