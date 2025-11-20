using System;
using System.Data.SqlClient;
using System.ServiceModel.Web;
using EmployeeService.Models;
using EmployeeService.Repositories.Interfaces;

namespace EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public EmployeeDto GetEmployeeById(string id)
        {
            try
            {
                var employee = _employeeRepository.GetById(id);
                if (employee == null)
                    throw new WebFaultException<string>("Employee not found", System.Net.HttpStatusCode.NotFound);

                return employee;
            }
            catch (SqlException ex)
            {
                throw new WebFaultException<string>($"Database error: {ex.Message}", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public EmployeeEnableDto EnableEmployee(string id, EmployeeEnableDto dto)
        {
            try
            {
                _employeeRepository.Enable(id, dto);
                return dto;
            }
            catch (InvalidOperationException ex)
            {
                throw new WebFaultException<string>(ex.Message, System.Net.HttpStatusCode.NotFound);
            }
            catch (SqlException ex)
            {
                throw new WebFaultException<string>($"Database error: {ex.Message}", System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}