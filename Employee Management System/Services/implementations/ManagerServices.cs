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
        private ILogger<ManagerServices> _logger;
        public ManagerServices(IDLeave dLeave, IDAttendance dAttendance, ILogger<ManagerServices> logger) 
        { 
            _dLeave = dLeave;
            _dAttendance = dAttendance;
            _logger = logger;
        }

        public void GetPendingLeaves()
        {
            try
            {
                List<LeaveDTO>? pendingLeaves = _dLeave.GetPendingLeaves();
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
        /*public void GetAttendance()
        {
            try
            {
                _dAttendance.GetAttendances().ForEach(a => 
                {
                    Console.WriteLine($"");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving attendance: {ex.Message}");
                _logger.LogError($"Error while retrieving attendance: {ex.Message}");
            }
        }*/
    }
}
