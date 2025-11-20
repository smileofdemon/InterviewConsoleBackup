using EmployeeService.Models;

namespace EmployeeService.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        EmployeeDto GetById(string id);

        void Enable(string id, EmployeeEnableDto employeeEnable);
    }
}
