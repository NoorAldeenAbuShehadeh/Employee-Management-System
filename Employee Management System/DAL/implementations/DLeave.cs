using Employee_Management_System.Model;
using Microsoft.Extensions.Logging;

namespace Employee_Management_System.DAL
{
    public class DLeave : IDLeave
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DLeave> _logger;

        public DLeave(AppDbContext context, ILogger<DLeave> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddLeave(LeaveDTO leaveDTO)
        {
            try
            {
                if (leaveDTO == null)
                {
                    Console.WriteLine("Bad Request should has a leave to add it");
                    _logger.LogError("Bad Request should has a leave to add it");
                }
                else
                {
                    Leave leave = new Leave()
                    {
                        Description = leaveDTO.Description,
                        EmployeeEmail = leaveDTO.EmployeeEmail,
                        StartDate = leaveDTO.StartDate,
                        EndDate = leaveDTO.EndDate,
                        Status = leaveDTO.Status,
                    };
                    _context.Leaves.Add(leave);
                    _context.SaveChanges();
                    Console.WriteLine($"Leave Added");
                    _logger.LogInformation($"Added new Leave to employee: {leaveDTO.EmployeeEmail}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add new leave: {ex.Message}");
                _logger.LogError($"Error while add new leave: {ex.Message}");
            }
        }

        public void UpdateLeave(LeaveDTO leaveDTO)
        {
            try
            {
                Leave? leave = _context.Leaves.FirstOrDefault(l => l.Id == leaveDTO.Id);
                if (leave == null)
                {
                    Console.WriteLine($"leave with Id {leaveDTO?.Id} not found.");
                    _logger.LogError($"leave with Id {leaveDTO?.Id} not found.");
                }
                else
                {
                    leave.StartDate = leaveDTO.StartDate;
                    leave.EndDate = leaveDTO.EndDate;
                    leave.Description = leaveDTO.Description;
                    leave.Status = leaveDTO.Status;
                    _context.SaveChanges();
                    _logger.LogInformation($"leave with Id {leaveDTO?.Id} updated.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update leave: {ex.Message}");
                _logger.LogError($"Error while update leave: {ex.Message}");
            }
        }

        public List<LeaveDTO>? GetLeaves(string employeeEmail)
        {
            try
            {
                var leaveDTOs = _context.Leaves
                    .Where(l => l.EmployeeEmail == employeeEmail)
                    .Select(l => new LeaveDTO
                    {
                        Id = l.Id,
                        EmployeeEmail = l.EmployeeEmail,
                        Description = l.Description,
                        StartDate = l.StartDate,
                        EndDate = l.EndDate,
                        Status = l.Status,
                    })
                    .ToList();

                _logger.LogInformation($"Retrieved leaves for employee with email {employeeEmail}");
                return leaveDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving leaves for employee: {ex.Message}");
                _logger.LogError($"Error while retrieving leaves for employee: {ex.Message}");
                return null;
            }
        }

        public List<LeaveDTO>? GetLeaves()
        {
            try
            {
                var leaveDTOs = _context.Leaves
                    .Select(l => new LeaveDTO
                    {
                        Id = l.Id,
                        EmployeeEmail = l.EmployeeEmail,
                        Description = l.Description,
                        StartDate = l.StartDate,
                        EndDate = l.EndDate,
                        Status = l.Status,
                    })
                    .ToList();

                _logger.LogInformation($"Retrieved all leaves");
                return leaveDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving leaves: {ex.Message}");
                _logger.LogError($"Error while retrieving leaves: {ex.Message}");
                return null;
            }
        }

        public List<LeaveDTO>? GetPendingLeaves()
        {
            try
            {
                var leaveDTOs = _context.Leaves
                    .Where(l => l.Status == LeaveStatus.Pending)
                    .Select(l => new LeaveDTO
                    {
                        Id = l.Id,
                        EmployeeEmail = l.EmployeeEmail,
                        Description = l.Description,
                        StartDate = l.StartDate,
                        EndDate = l.EndDate,
                        Status = l.Status,
                    })
                    .ToList();

                _logger.LogInformation($"Retrieved leaves that status pending");
                return leaveDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving pending leaves: {ex.Message}");
                _logger.LogError($"Error while retrieving pending leaves: {ex.Message}");
                return null;
            }
        }
    }
}
