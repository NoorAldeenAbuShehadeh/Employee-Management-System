using Employee_Management_System.Model;

namespace Employee_Management_System.DAL
{
    public interface IDEmployees
    {
        public bool AddEmployee(EmployeeDTO employeeDTO);
        public bool UpdateEmployee(EmployeeDTO employeeDTO);
        public List<EmployeeDTO>? GetEmployees();
        public EmployeeDTO? GetEmployee(string email);
        public List<EmployeeDTO>? GetEmployees(string departmentName);
        public List<EmployeeDTO>? GetEmployees(decimal minSalary);
        public List<EmployeeDTO>? SerchEmployeesbyCityName(string cityName);
    }
}
