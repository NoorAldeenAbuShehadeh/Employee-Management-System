namespace Employee_Management_System.Services
{
    public interface IManagerServices
    {
        public void GetPendingLeaves(string departmentName);
        public void UpdateLeave();
        public void GetEmployeesInDepartment(string departmentName);
        public void GetAttendanceForDepartment(string departmentName);
    }
}
