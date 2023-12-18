namespace Employee_Management_System.Services
{
    public interface IAdminServices
    {
        public void AddDepartment();
        public void AddEmployee();
        public void UpdateDepartment();
        public void UpdateEmployee();
        public void UpdateSalary();
        public void GetDepartments();
        public void GetEmployees();
        public void GetEmployeeSalary();
        public void GetEmployeeSalaries();
        public void LeaveTrend();
        public void GetAttendances();
        public void GetLeaves();
        public void FilterEmployeesBySalary();
        public void DepartmentStatistics();
        public void SerchForEmployeesByCity();
    }
}
