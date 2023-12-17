using Employee_Management_System.Model;

namespace Employee_Management_System.Services
{
    public interface IEmployeeServices
    {
        public void AddLeave(UserDTO userDTO);
        public void AddAttendance(UserDTO userDTO);
        public void UpdateInformations(UserDTO userDTO);
        public void GetAttendanceReport(UserDTO userDTO);
        public void GetLeaves(UserDTO userDTO);
        public void GetSalary(UserDTO userDTO);
        public void GetInformation(UserDTO userDTO);
    }
}
