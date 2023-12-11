using Employee_Management_System.Model;

namespace Employee_Management_System.DAL
{
    public interface IDAttendance
    {
        public bool AddAttendance(AttendanceDTO attendanceDTO);
        public bool UpdateAttendance(AttendanceDTO attendanceDTO);
        public List<AttendanceDTO>? GetAttendances(string employeeEmail);
        public List<AttendanceDTO>? GetAttendances();
    }
}
