using Employee_Management_System.Model;

namespace Employee_Management_System.DAL
{
    public interface IDEmployees
    {
        public void AddEmployee(EmployeeDTO employeeDTO);
        public void UpdateEmployee(EmployeeDTO employeeDTO);
        public List<EmployeeDTO>? GetEmployees();
        public EmployeeDTO? GetEmployee(string email);
    }
}
