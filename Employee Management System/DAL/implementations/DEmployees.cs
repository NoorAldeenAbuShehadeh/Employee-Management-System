using Employee_Management_System.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.DAL
{
    internal class DEmployees : IDEmployees
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DEmployees> _logger;
        public DEmployees(AppDbContext context, ILogger<DEmployees> logger)
        {
            _context = context;
            _logger = logger;
        }
        public bool AddEmployee(EmployeeDTO employeeDTO)
        {
            try
            {
                if (employeeDTO == null)
                {
                    Console.WriteLine("Bad Request should has a employee to add it");
                    _logger.LogError("Bad Request should has a employee to add it");
                    return false;
                }
                else
                {
                    ValidateEmployee(employeeDTO);
                    Employee employee = new Employee()
                    {
                        UserEmail = employeeDTO.UserEmail,
                        Address = employeeDTO.Address,
                        DepartmentName = employeeDTO.DepartmentName,
                        PhoneNumber = employeeDTO.PhoneNumber
                    };
                    _context.Employees.Add(employee);
                    Console.WriteLine($"Employee Added");
                    _logger.LogInformation($"Added new Employee: {employee.UserEmail}");
                    return true;
                }
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Validation Error: {ex.Message}");
                _logger.LogError($"Validation Error while add new employee: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add new employee: {ex.Message}");
                _logger.LogError($"Error while add new employee: {ex.Message}");
                return false;
            }
        }
        public bool UpdateEmployee(EmployeeDTO employeeDTO)
        {
            try
            {
                Employee? employee = _context.Employees.FirstOrDefault(e => e.UserEmail == employeeDTO.UserEmail);
                if (employee == null)
                {
                    Console.WriteLine($"Employee with email {employeeDTO?.UserEmail} not found.");
                    _logger.LogError($"Employee with email {employeeDTO?.UserEmail} not found.");
                    return false;
                }
                else
                {
                    ValidateEmployee(employeeDTO);
                    employee.PhoneNumber = employeeDTO.PhoneNumber;
                    employee.Address = employeeDTO.Address;
                    employee.DepartmentName = employeeDTO.DepartmentName;
                    _context.SaveChanges();
                    _logger.LogInformation($"Employee with email {employeeDTO.UserEmail} updated.");
                    return true;
                }
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Validation Error: {ex.Message}");
                _logger.LogError($"Validation Error while update employee: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update employee: {ex.Message}");
                _logger.LogError($"Error while update employee: {ex.Message}");
                return false;
            }
        }
        public List<EmployeeDTO>? GetEmployees()
        {
            try
            {
                var employeeDTOs = _context.Employees
                    .Select(e => new EmployeeDTO
                    {
                        UserEmail = e.UserEmail,
                        Address = e.Address,
                        DepartmentName = e.DepartmentName,
                        PhoneNumber = e.PhoneNumber
                    })
                    .ToList();
                _logger.LogInformation("retrived all employees");
                return employeeDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrive all employees: {ex.Message}");
                _logger.LogError($"Error while retrive all employees: {ex.Message}");
                return null;
            }
        }
        public List<EmployeeDTO>? GetEmployees(string departmentName)
        {
            try
            {
                var employeeDTOs = (from emp in _context.Employees
                                    join user in _context.Users on emp.DepartmentName equals departmentName
                                    where (emp.UserEmail == user.Email && user.Role=="employee")
                                    select new EmployeeDTO
                                    {
                                        UserEmail = emp.UserEmail,
                                        Address = emp.Address,
                                        DepartmentName = emp.DepartmentName,
                                        PhoneNumber = emp.PhoneNumber,
                                    }).ToList();
                _logger.LogInformation("retrived all employees");
                return employeeDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrive employees in a department: {ex.Message}");
                _logger.LogError($"Error while retrive employees in a department: {ex.Message}");
                return null;
            }
        }
        public EmployeeDTO? GetEmployee(string email)
        {
            try
            {
                Employee? employee = _context.Employees.FirstOrDefault(e => e.UserEmail == email);
                if (employee == null)
                {
                    Console.WriteLine($"Employee with email {email} not found.");
                    _logger.LogError($"Employee with email {email} not found.");
                    return null;
                }
                EmployeeDTO employeeDTO = new EmployeeDTO
                {
                    UserEmail = employee.UserEmail,
                    Address = employee.Address,
                    DepartmentName = employee.DepartmentName,
                    PhoneNumber = employee.PhoneNumber
                };
                _logger.LogInformation($"Get data for employee");
                return employeeDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get employee: {ex.Message}");
                _logger.LogError($"Error while get employee: {ex.Message}");
                return null;
            }
        }
        private void ValidateEmployee(EmployeeDTO employeeDTO)
        {
            var validationContext = new ValidationContext(employeeDTO, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(employeeDTO, validationContext, validationResults, validateAllProperties: true))
            {
                var errorMessages = validationResults.Select(result => result.ErrorMessage);
                throw new ValidationException(string.Join(Environment.NewLine, errorMessages));
            }
        }
    }
}
