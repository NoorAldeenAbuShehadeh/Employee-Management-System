using Employee_Management_System.Model;
using Microsoft.Extensions.Logging;

namespace Employee_Management_System.DAL
{
    public class DAttendance : IDAttendance
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DAttendance> _logger;

        public DAttendance(AppDbContext context, ILogger<DAttendance> logger)
        {
            _context = context;
            _logger = logger;
        }
        public void AddAttendance(AttendanceDTO attendanceDTO)
        {
            try
            {
                if (attendanceDTO == null)
                {
                    Console.WriteLine("Bad Request should has a attendance to add it");
                    _logger.LogError("Bad Request should has a attendance to add it");
                }
                else
                {
                    Attendance attendance = new Attendance()
                    {
                        EmployeeEmail = attendanceDTO.EmployeeEmail,
                        CheckIn = attendanceDTO.CheckIn,
                        CheckOut = attendanceDTO.CheckOut,
                        Status = attendanceDTO.Status,
                    };
                    _context.Attendances.Add(attendance);
                    _context.SaveChanges();
                    Console.WriteLine($"Attendance Added");
                    _logger.LogInformation($"Added new Attendance for: {attendanceDTO.EmployeeEmail}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add new attendance: {ex.Message}");
                _logger.LogError($"Error while add new attendance: {ex.Message}");
            }
        }

        public void UpdateAttendance(AttendanceDTO attendanceDTO)
        {
            try
            {
                Attendance? attendance = _context.Attendances.FirstOrDefault(a => a.Id == attendanceDTO.Id);
                if (attendance == null)
                {
                    Console.WriteLine($"Attendance with Id {attendanceDTO.Id} not found.");
                    _logger.LogError($"Attendance with Id {attendanceDTO.Id} not found.");
                }
                else
                {
                    attendance.Status = attendanceDTO.Status;
                    attendance.CheckIn = attendanceDTO.CheckIn;
                    attendance.CheckOut = attendanceDTO.CheckOut;
                    _context.SaveChanges();
                    _logger.LogInformation($"Attendance with Id {attendanceDTO.Id} updated.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update attendance: {ex.Message}");
                _logger.LogError($"Error while update attendance: {ex.Message}");
            }
        }

        public List<AttendanceDTO>? GetAttendances(string employeeEmail)
        {
            try
            {
                var attendanceDTOs = _context.Attendances
                    .Where(a => a.EmployeeEmail == employeeEmail)
                    .Select(a => new AttendanceDTO
                    {
                        Id = a.Id,
                        EmployeeEmail = a.EmployeeEmail,
                        CheckOut = a.CheckOut,
                        CheckIn = a.CheckIn,
                        Status = a.Status,
                    })
                    .ToList();

                _logger.LogInformation($"Retrieved attendances for employee with email {employeeEmail}");
                return attendanceDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving attendances for employee: {ex.Message}");
                _logger.LogError($"Error while retrieving attendances for employee: {ex.Message}");
                return null;
            }
        }

        public List<AttendanceDTO>? GetAttendances()
        {
            try
            {
                var attendanceDTOs = _context.Attendances
                   .Select(a => new AttendanceDTO
                   {
                       Id = a.Id,
                       EmployeeEmail = a.EmployeeEmail,
                       CheckOut = a.CheckOut,
                       CheckIn = a.CheckIn,
                       Status = a.Status,
                   })
                   .ToList();
                _logger.LogInformation("retrived all attendances");
                return attendanceDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrive all attendances: {ex.Message}");
                _logger.LogError($"Error while retrive all attendances: {ex.Message}");
                return null;
            }
        }
    }
}
