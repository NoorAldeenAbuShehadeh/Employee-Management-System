namespace Employee_Management_System.Services
{
    public interface IAdminServices
    {
        public void AddDepartment();
        public void AddEmployee();
        public void AddSalary();
        public void UpdateDepartment();
        public void UpdateEmployee();
        public void UpdateSalary();
        public void GetDepartments();
        public void GetEmployees();
        public void GetEmployeeSalary();
        public void GetEmployeeSalaries();
        public string? EncodePassword(string password);
    }
}
