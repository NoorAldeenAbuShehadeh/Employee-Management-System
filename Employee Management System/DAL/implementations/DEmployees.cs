using Employee_Management_System.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        public void AddEmployee(EmployeeDTO employeeDTO)
        {
            try
            {
                if (employeeDTO == null)
                {
                    Console.WriteLine("Bad Request should has a employee to add it");
                    _logger.LogError("Bad Request should has a employee to add it");
                }
                else
                {
                    Employee employee = new Employee()
                    {
                        UserEmail = employeeDTO.UserEmail,
                        Address = employeeDTO.Address,
                        DepartmentName = employeeDTO.DepartmentName,
                        PhoneNumber = employeeDTO.PhoneNumber
                    };
                    _context.Employees.Add(employee);
                    _context.SaveChanges();
                    Console.WriteLine($"Employee Added");
                    _logger.LogInformation($"Added new Employee: {employee.UserEmail}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add new employee: {ex.Message}");
                _logger.LogError($"Error while add new employee: {ex.Message}");
            }
        }
        public void UpdateEmployee(EmployeeDTO employeeDTO)
        {
            try
            {
                Employee? employee = _context.Employees.FirstOrDefault(e => e.UserEmail == employeeDTO.UserEmail);
                if (employee == null)
                {
                    Console.WriteLine($"Employee with email {employeeDTO?.UserEmail} not found.");
                    _logger.LogError($"Employee with email {employeeDTO?.UserEmail} not found.");
                }
                else
                {
                    employee.PhoneNumber = employeeDTO.PhoneNumber;
                    employee.Address = employeeDTO.Address;
                    employee.DepartmentName = employeeDTO.DepartmentName;
                    _context.SaveChanges();
                    _logger.LogInformation($"Employee with email {employeeDTO.UserEmail} updated.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update employee: {ex.Message}");
                _logger.LogError($"Error while update employee: {ex.Message}");
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
                        PhoneNumber= e.PhoneNumber
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
                    UserEmail= employee.UserEmail,
                    Address = employee.Address,
                    DepartmentName = employee.DepartmentName,
                    PhoneNumber= employee.PhoneNumber
                };
                _logger.LogError($"Get data for employee");
                return employeeDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get employee: {ex.Message}");
                _logger.LogError($"Error while get employee: {ex.Message}");
                return null;
            }
        }

    }
}
