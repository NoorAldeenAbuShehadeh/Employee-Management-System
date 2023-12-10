using Employee_Management_System.Model;
using Microsoft.Extensions.Logging;

namespace Employee_Management_System.DAL
{
    public class DDepartments : IDDepartments
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DDepartments> _logger;

        public DDepartments(AppDbContext context, ILogger<DDepartments> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddDepartment(DepartmentDTO departmentDTO)
        {
            try
            {
                if (departmentDTO == null)
                {
                    Console.WriteLine("Bad Request should has a department to add it");
                    _logger.LogError("Bad Request should has a department to add it");
                }
                else
                {
                    Department department = new Department()
                    {
                        Name = departmentDTO.Name,
                        ManagerEmail = departmentDTO.ManagerEmail,
                    };
                    _context.Departments.Add(department);
                    _context.SaveChanges();
                    Console.WriteLine($"Department Added");
                    _logger.LogInformation($"Added new Department: {department.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add new department: {ex.Message}");
                _logger.LogError($"Error while add new department: {ex.Message}");
            }
        }

        public List<DepartmentDTO>? GetDepartments()
        {
            try 
            { 
                var departmentDTOs = _context.Departments
                    .Select(d => new DepartmentDTO
                    {
                        ManagerEmail = d.ManagerEmail,
                        Name = d.Name,
                    })
                    .ToList();
                _logger.LogInformation("retrived all departments");
                return departmentDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrive all departments: {ex.Message}");
                _logger.LogError($"Error while retrive all departments: {ex.Message}");
                return null;
            }
        }

        public void UpdateDepartment(DepartmentDTO departmentDTO)
        {
            try
            {
                Department? department = _context.Departments.FirstOrDefault(d => d.Name == departmentDTO.Name);
                if (department == null)
                {
                    Console.WriteLine($"Department with Name {departmentDTO?.Name} not found.");
                    _logger.LogError($"Department with Name {departmentDTO?.Name} not found.");
                }
                else
                {
                    User? user = _context.Users.FirstOrDefault(u => u.Email == departmentDTO.ManagerEmail);
                    if(user == null || user.Role != "manager")
                    {
                        Console.WriteLine($"Employee with Name {departmentDTO?.ManagerEmail} not found or not Manager.");
                        _logger.LogError($"Employee with Name {departmentDTO?.ManagerEmail} not found or not Manager.");
                    }
                    else
                    {
                        department.ManagerEmail = departmentDTO.ManagerEmail;
                        _context.SaveChanges();
                        _logger.LogInformation($"Department with Name {department?.Name} updated.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update department: {ex.Message}");
                _logger.LogError($"Error while update department: {ex.Message}");
            }
        }

    }
}
