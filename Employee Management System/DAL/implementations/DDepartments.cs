using Employee_Management_System.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

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

        public bool AddDepartment(DepartmentDTO departmentDTO)
        {
            try
            {
                if (departmentDTO == null)
                {
                    Console.WriteLine("Bad Request should has a department to add it");
                    _logger.LogError("Bad Request should has a department to add it");
                    return false;
                }
                else
                {
                    ValidateDepartment(departmentDTO);
                    Department department = new Department()
                    {
                        Name = departmentDTO.Name,
                        ManagerEmail = departmentDTO.ManagerEmail,
                    };
                    _context.Departments.Add(department);
                    _context.SaveChanges();
                    Console.WriteLine($"Department Added");
                    _logger.LogInformation($"Added new Department: {department.Name}");
                    return true;
                }
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Validation Error: {ex.Message}");
                _logger.LogError($"Validation Error while add new department: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add new department: {ex.Message}");
                _logger.LogError($"Error while add new department: {ex.Message}");
                return false;
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
        public bool UpdateDepartment(DepartmentDTO departmentDTO)
        {
            try
            {
                Department? department = _context.Departments.FirstOrDefault(d => d.Name == departmentDTO.Name);
                if (department == null)
                {
                    Console.WriteLine($"Department with Name {departmentDTO?.Name} not found.");
                    _logger.LogError($"Department with Name {departmentDTO?.Name} not found.");
                    return false;
                }
                else
                {
                    User? user = _context.Users.FirstOrDefault(u => u.Email == departmentDTO.ManagerEmail);
                    if(user == null || user.Role != "manager")
                    {
                        Console.WriteLine($"Employee with Name {departmentDTO?.ManagerEmail} not found or not Manager.");
                        _logger.LogError($"Employee with Name {departmentDTO?.ManagerEmail} not found or not Manager.");
                        return false;
                    }
                    else
                    {
                        ValidateDepartment(departmentDTO);
                        department.ManagerEmail = departmentDTO.ManagerEmail;
                        _context.SaveChanges();
                        _logger.LogInformation($"Department with Name {department?.Name} updated.");
                        return true;
                    }
                }
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Validation Error: {ex.Message}");
                _logger.LogError($"Validation Error while update department: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update department: {ex.Message}");
                _logger.LogError($"Error while update department: {ex.Message}");
                return false;
            }
        }
        private void ValidateDepartment(DepartmentDTO departmentDTO)
        {
            var validationContext = new ValidationContext(departmentDTO, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(departmentDTO, validationContext, validationResults, validateAllProperties: true))
            {
                var errorMessages = validationResults.Select(result => result.ErrorMessage);
                throw new ValidationException(string.Join(Environment.NewLine, errorMessages));
            }
        }
    }
}
