using Employee_Management_System.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

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
        public bool AddAttendance(AttendanceDTO attendanceDTO)
        {
            try
            {
                if (attendanceDTO == null)
                {
                    Console.WriteLine("Bad Request should has a attendance to add it");
                    _logger.LogError("Bad Request should has a attendance to add it");
                    return false;
                }
                else
                {
                    ValidateAttendance(attendanceDTO);
                    Attendance attendance = new Attendance()
                    {
                        EmployeeEmail =attendanceDTO.EmployeeEmail,
                        CheckIn = attendanceDTO.CheckIn,
                        CheckOut = attendanceDTO.CheckOut,
                        Status = attendanceDTO.Status,
                    };
                    _context.Attendances.Add(attendance);
                    _context.SaveChanges();
                    Console.WriteLine($"Attendance Added");
                    _logger.LogInformation($"Added new Attendance for: {attendanceDTO.EmployeeEmail}");
                    return true;
                }
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Validation Error: {ex.Message}");
                _logger.LogError($"Validation Error while add new attendance: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add new attendance: {ex.Message}");
                _logger.LogError($"Error while add new attendance: {ex.Message}");
                return false;
            }
        }
        public bool UpdateAttendance(AttendanceDTO attendanceDTO)
        {
            try
            {
                Attendance? attendance = _context.Attendances.FirstOrDefault(a => a.Id == attendanceDTO.Id);
                if (attendance == null)
                {
                    Console.WriteLine($"Attendance with Id {attendanceDTO.Id} not found.");
                    _logger.LogError($"Attendance with Id {attendanceDTO.Id} not found.");
                    return false;
                }
                else
                {
                    ValidateAttendance(attendanceDTO);
                    attendance.Status = attendanceDTO.Status;
                    attendance.CheckIn = attendanceDTO.CheckIn;
                    attendance.CheckOut = attendanceDTO.CheckOut;
                    _context.SaveChanges();
                    _logger.LogInformation($"Attendance with Id {attendanceDTO.Id} updated.");
                    return true;
                }
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Validation Error: {ex.Message}");
                _logger.LogError($"Validation Error while update attendance: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update attendance: {ex.Message}");
                _logger.LogError($"Error while update attendance: {ex.Message}");
                return false;
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
        public List<AttendanceDTO>? GetAttendances(DateTime startDate)
        {
            try
            {
                var attendanceDTOs = _context.Attendances
                   .Where(a=>a.CheckIn >= startDate)
                   .Select(a => new AttendanceDTO
                   {
                       Id = a.Id,
                       EmployeeEmail = a.EmployeeEmail,
                       CheckOut = a.CheckOut,
                       CheckIn = a.CheckIn,
                       Status = a.Status,
                   })
                   .OrderBy(a => a.EmployeeEmail)
                   .ThenBy(a => a.CheckIn)
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
        public List<AttendanceDTO>? GetAttendanceReport(string employeeEmail, DateTime startDate)
        {
            try
            {
                var attendanceDtos = _context.Attendances
                .Where(a => a.EmployeeEmail == employeeEmail && a.CheckIn >= startDate)
                .Select(a => new AttendanceDTO
                {
                    Id = a.Id,
                    EmployeeEmail = a.EmployeeEmail,
                    CheckOut = a.CheckOut,
                    CheckIn = a.CheckIn,
                    Status = a.Status,
                })
                .ToList();
                return attendanceDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get attendance report: {ex.Message}");
                _logger.LogError($"Error while get attendance report: {ex.Message}");
                return null;
            }
        }
        public List<AttendanceDTO>? GetAttendanceReportForDepartment(string departmentName, DateTime startDate)
        {
            try
            {
                var attendanceDtos = (from emp in _context.Employees
                                 join attendance in _context.Attendances on emp.DepartmentName equals departmentName
                                 where (attendance.CheckIn >= startDate && attendance.EmployeeEmail==emp.UserEmail)
                                 select new AttendanceDTO
                                 {
                                     Id = attendance.Id,
                                     EmployeeEmail = attendance.EmployeeEmail,
                                     CheckOut = attendance.CheckOut,
                                     CheckIn = attendance.CheckIn,
                                     Status = attendance.Status,
                                 }).ToList();
                return attendanceDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get attendance report: {ex.Message}");
                _logger.LogError($"Error while get attendance report: {ex.Message}");
                return null;
            }
        }
        private void ValidateAttendance(AttendanceDTO attendanceDTO)
        {
            var validationContext = new ValidationContext(attendanceDTO, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(attendanceDTO, validationContext, validationResults, validateAllProperties: true))
            {
                var errorMessages = validationResults.Select(result => result.ErrorMessage);
                throw new ValidationException(string.Join(Environment.NewLine, errorMessages));
            }
        }
    }
}
