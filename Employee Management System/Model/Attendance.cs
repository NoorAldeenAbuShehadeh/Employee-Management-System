using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        public string EmployeeEmail { get; set; }
        public DateOnly Date { get; set; }
        public AttendanceStatus Status { get; set; }
        public Employee Employee { get; set; }
    }
    public enum AttendanceStatus
    {
        Present,
        Absent,
        OnLeave,
        SickLeave,
        Vacation,
        HalfDay,
        Remote
    }
}
