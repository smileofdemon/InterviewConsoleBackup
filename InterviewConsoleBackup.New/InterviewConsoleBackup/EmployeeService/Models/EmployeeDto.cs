namespace EmployeeService.Models
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public int? ManagerId { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}