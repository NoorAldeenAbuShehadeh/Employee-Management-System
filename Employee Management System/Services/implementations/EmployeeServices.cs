using Employee_Management_System.DAL;
using Employee_Management_System.Model;
using Microsoft.Extensions.Logging;

namespace Employee_Management_System.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        private IDLeave _dLeave;
        private IDAttendance _dAttendance;
        private ILogger<EmployeeServices> _logger;
        public EmployeeServices(IDLeave dLeave, IDAttendance dAttendance, ILogger<EmployeeServices> logger) 
        {
            _dLeave = dLeave;
            _dAttendance = dAttendance;
            _logger = logger;
        }

        public void AddLeave(UserDTO userDTO)
        {
            try
            {
                Console.WriteLine("Enter leave description: ");
                string description = Console.ReadLine();
                Console.WriteLine("Enter start date and time (yyyy-MM-dd HH:mm:ss): ");
                DateTime startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None);
                Console.WriteLine("Enter end date and time (yyyy-MM-dd HH:mm:ss): ");
                DateTime endDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None);
                LeaveDTO leave = new LeaveDTO 
                {
                    EmployeeEmail = userDTO.Email,
                    Description = description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Status = LeaveStatus.Pending,
                };
                _dLeave.AddLeave(leave);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add leave: {ex.Message}");
                _logger.LogError($"Error while add leave: {ex.Message}");
            }
        }
        public void AddAttendance(UserDTO userDTO)
        {
            try
            {
                Console.WriteLine("Enter leave status enter 0 if Present, 1 if Absent, 2 if Vacation, 3 if Remote): ");
                AttendanceStatus status = Enum.Parse<AttendanceStatus>(Console.ReadLine());
                DateTime? checkIn = null;
                DateTime? checkOut = null;
                if(status == AttendanceStatus.Remote || status == AttendanceStatus.Present)
                { 
                    Console.WriteLine("Enter start date and time (yyyy-MM-dd HH:mm:ss): ");
                    checkIn = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None);
                    Console.WriteLine("Enter end date and time (yyyy-MM-dd HH:mm:ss): ");
                    checkOut = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None);
                }
                AttendanceDTO attendance = new AttendanceDTO
                {
                    EmployeeEmail = userDTO.Email,
                    CheckIn = checkIn,
                    CheckOut = checkOut,
                    Status = status
                };
                _dAttendance.AddAttendance(attendance);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add leave: {ex.Message}");
                _logger.LogError($"Error while add leave: {ex.Message}");
            }
        }
    }
}
