using Employee_Management_System.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

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
        public bool AddLeave(LeaveDTO leaveDTO)
        {
            try
            {
                if (leaveDTO == null)
                {
                    Console.WriteLine("Bad Request should has a leave to add it");
                    _logger.LogError("Bad Request should has a leave to add it");
                    return false;
                }
                else
                {
                    ValidateLeave(leaveDTO);
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
                    return true;
                }
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Validation Error: {ex.Message}");
                _logger.LogError($"Validation Error while add new leave: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add new leave: {ex.Message}");
                _logger.LogError($"Error while add new leave: {ex.Message}");
                return true;
            }
        }
        public bool UpdateLeave(LeaveDTO leaveDTO)
        {
            try
            {
                Leave? leave = _context.Leaves.FirstOrDefault(l => l.Id == leaveDTO.Id);
                if (leave == null)
                {
                    Console.WriteLine($"leave with Id {leaveDTO?.Id} not found.");
                    _logger.LogError($"leave with Id {leaveDTO?.Id} not found.");
                    return false;
                }
                else
                {
                    ValidateLeave(leaveDTO);
                    leave.StartDate = leaveDTO.StartDate;
                    leave.EndDate = leaveDTO.EndDate;
                    leave.Description = leaveDTO.Description;
                    leave.Status = leaveDTO.Status;
                    _context.SaveChanges();
                    _logger.LogInformation($"leave with Id {leaveDTO?.Id} updated.");
                    return true;
                }
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Validation Error: {ex.Message}");
                _logger.LogError($"Validation Error while update leave: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update leave: {ex.Message}");
                _logger.LogError($"Error while update leave: {ex.Message}");
                return false;
            }
        }
        public LeaveDTO? GetLeave(int id)
        {
            try
            {
                var leave = _context.Leaves.FirstOrDefault(l => l.Id == id);
                if (leave == null)
                {
                    Console.WriteLine($"leave with id = {id} not found");
                    _logger.LogInformation($"leave with id = {id} not found");
                    return null;
                }
                var leaveDTO = new LeaveDTO 
                {
                    Id= leave.Id,
                    Description= leave.Description,
                    EmployeeEmail= leave.EmployeeEmail,
                    StartDate= leave.StartDate,
                    EndDate= leave.EndDate,
                    Status = leave.Status
                };
                _logger.LogInformation($"Retrieved leave id = {id}");
                return leaveDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving leaves for employee: {ex.Message}");
                _logger.LogError($"Error while retrieving leaves for employee: {ex.Message}");
                return null;
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
        public List<LeaveDTO> GetApprovedLeaves()
        {
            try
            {
                var leaveDTOs = _context.Leaves.
                    Where(l => l.Status == LeaveStatus.Approved)
                   .Select(l => new LeaveDTO
                   {
                       Id = l.Id,
                       EmployeeEmail = l.EmployeeEmail,
                       Description = l.Description,
                       StartDate = l.StartDate,
                       EndDate = l.EndDate,
                   })
                   .ToList();
                _logger.LogInformation("retrived approved leaves");
                return leaveDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrive approved leaves: {ex.Message}");
                _logger.LogError($"Error while retrive approved leaves: {ex.Message}");
                return null;
            }
        }
        public List<LeaveDTO>? GetPendingLeaves(string departmentName)
        {
            try
            {
                var leaveDTOs = (from emp in _context.Employees
                                 join leave in _context.Leaves on emp.UserEmail equals leave.EmployeeEmail
                                 where (leave.Status == LeaveStatus.Pending && emp.DepartmentName == departmentName)
                                 select new LeaveDTO
                                 {
                                     Id = leave.Id,
                                     EmployeeEmail = leave.EmployeeEmail,
                                     Description = leave.Description,
                                     StartDate = leave.StartDate,
                                     EndDate = leave.EndDate,
                                     Status = leave.Status,
                                 }).ToList();

                _logger.LogInformation($"Retrieved leaves that status pending for department {departmentName}");
                return leaveDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving pending leaves for department: {ex.Message}");
                _logger.LogError($"Error while retrieving pending leaves for department: {ex.Message}");
                return null;
            }
        }
        public List<LeaveDTO>? GetLeavesForDepartment(string departmentName)
        {
            try
            {
                var leaveDTOs = (from emp in _context.Employees
                                 join leave in _context.Leaves on emp.UserEmail equals leave.EmployeeEmail
                                 where (emp.DepartmentName == departmentName)
                                 select new LeaveDTO
                                 {
                                     Id = leave.Id,
                                     EmployeeEmail = leave.EmployeeEmail,
                                     Description = leave.Description,
                                     StartDate = leave.StartDate,
                                     EndDate = leave.EndDate,
                                     Status = leave.Status,
                                 }).ToList();

                _logger.LogInformation($"Retrieved leaves for department {departmentName}");
                return leaveDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving leaves for department: {ex.Message}");
                _logger.LogError($"Error while retrieving leaves for department: {ex.Message}");
                return null;
            }
        }
        private void ValidateLeave(LeaveDTO leaveDTO)
        {
            var validationContext = new ValidationContext(leaveDTO, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(leaveDTO, validationContext, validationResults, validateAllProperties: true))
            {
                var errorMessages = validationResults.Select(result => result.ErrorMessage);
                throw new ValidationException(string.Join(Environment.NewLine, errorMessages));
            }
        }
    }
}
