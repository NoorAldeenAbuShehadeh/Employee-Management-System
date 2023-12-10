using Employee_Management_System.Model;

namespace Employee_Management_System.DAL
{
    public interface IDAttendance
    {
        public void AddAttendance(AttendanceDTO attendanceDTO);
        public void UpdateAttendance(AttendanceDTO attendanceDTO);
        public List<AttendanceDTO>? GetAttendances(string employeeEmail);
        public List<AttendanceDTO>? GetAttendances();
    }
}
