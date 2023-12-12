using Employee_Management_System.DAL;
using Employee_Management_System.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Employee_Management_System.Services
{
    public class ManagerServices : IManagerServices
    {
        private IDLeave _dLeave;
        private IDAttendance _dAttendance;
        private IDEmployees _dEmployees;
        private ILogger<ManagerServices> _logger;
        public ManagerServices(IDLeave dLeave, IDAttendance dAttendance, IDEmployees dEmployees, ILogger<ManagerServices> logger) 
        { 
            _dLeave = dLeave;
            _dAttendance = dAttendance;
            _dEmployees = dEmployees;
            _logger = logger;
        }

        public void GetPendingLeaves(string departmentName)
        {
            try
            {
                List<LeaveDTO>? pendingLeaves = _dLeave.GetPendingLeaves(departmentName);
                pendingLeaves?.ForEach(l =>
                {
                    Console.WriteLine($"Id: {l.Id}, employee email: {l.EmployeeEmail}, description: {l.Description}, start date: {l.StartDate}, end date: {l.EndDate}");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get pending leaves: {ex.Message}");
                _logger.LogError($"Error while get pending leaves: {ex.Message}");
            }
        }
        public void UpdateLeave()
        {
            try
            {
                Console.WriteLine("Enter leave id: ");
                int id = int.Parse(Console.ReadLine());
                LeaveDTO? leaveDTO = _dLeave.GetLeave(id);
                if ( leaveDTO != null )
                {
                    Console.WriteLine("Enter leave status enter 0 if Pending, 1 if Approved, 2 if Denied): ");
                    LeaveStatus status = Enum.Parse<LeaveStatus>(Console.ReadLine());
                    leaveDTO.Status = status;
                    _dLeave.UpdateLeave(leaveDTO);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update leaves: {ex.Message}");
                _logger.LogError($"Error while update leaves: {ex.Message}");
            }
        }
        public void GetEmployeesInDepartment(string departmentName)
        {
            try
            {
                List<EmployeeDTO>? pendingLeaves = _dEmployees.GetEmployees(departmentName);
                pendingLeaves?.ForEach(e =>
                {
                    Console.WriteLine($"employee email: {e.UserEmail}, phone number: {e.PhoneNumber}, address: {e.Address}");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get employees in department: {ex.Message}");
                _logger.LogError($"Error while get employees in department: {ex.Message}");
            }
        }
        public void GetAttendanceForDepartment(string departmentName)
        {
            try
            {
                Console.Clear();
                Console.Write("Enter start date (yyyy-MM-dd): ");
                DateTime startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None);
                _dAttendance.GetAttendanceReportForDepartment(departmentName, startDate)?.ForEach(a =>
                {
                    Console.WriteLine($"employee email: {a.EmployeeEmail}, status: {a.Status}, check in: {a.CheckIn}, check out: {a.CheckOut}");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving attendance for department: {ex.Message}");
                _logger.LogError($"Error while retrieving attendance for department: {ex.Message}");
            }
        }
    }
} 
