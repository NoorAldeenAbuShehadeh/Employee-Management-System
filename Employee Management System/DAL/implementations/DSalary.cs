using Employee_Management_System.Model;
using Microsoft.Extensions.Logging;

namespace Employee_Management_System.DAL
{

    public class DSalary : IDSalary
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DSalary> _logger;

        public DSalary(AppDbContext context, ILogger<DSalary> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddSalary(SalaryDTO salaryDTO)
        {
            try
            {
                if (salaryDTO == null)
                {
                    Console.WriteLine("Bad Request should has a salary to add it");
                    _logger.LogError("Bad Request should has a salary to add it");
                }
                else
                {
                    Salary salary = new Salary()
                    {
                        EmployeeEmail = salaryDTO.EmployeeEmail,
                        Amount = salaryDTO.Amount,
                        Bonuses = salaryDTO.Bonuses,
                        Deductions = salaryDTO.Deductions,
                    };
                    _context.Salaries.Add(salary);
                    _context.SaveChanges();
                    Console.WriteLine($"salary Added");
                    _logger.LogInformation($"Added salary for employee: {salary.EmployeeEmail}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add salary: {ex.Message}");
                _logger.LogError($"Error while add salary: {ex.Message}");
            }
        }

        public void UpdateSalary(SalaryDTO salaryDTO)
        {
            try
            {
                Salary? salary = _context.Salaries.FirstOrDefault(s => s.EmployeeEmail == salaryDTO.EmployeeEmail);
                if (salary == null)
                {
                    Console.WriteLine($"salary with employee email {salary?.EmployeeEmail} not found.");
                    _logger.LogError($"salary with employee email {salary?.EmployeeEmail} not found.");
                }
                else
                {
                    salary.Amount = salaryDTO.Amount;
                    salary.Bonuses = salaryDTO.Bonuses;
                    salary.Deductions = salaryDTO.Deductions;
                    _context.SaveChanges();
                    _logger.LogInformation($"salary with employee email {salary?.EmployeeEmail} updated.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update salary: {ex.Message}");
                _logger.LogError($"Error while update salary: {ex.Message}");
            }
        }

        public SalaryDTO? GetSalary(string employeeEmail)
        {
            try
            {
                Salary? salary = _context.Salaries.FirstOrDefault(s => s.EmployeeEmail == employeeEmail);
                if (salary == null)
                {
                    Console.WriteLine($"salary with employee email {employeeEmail} not found.");
                    _logger.LogError($"salary with employee email {employeeEmail} not found.");
                    return null;
                }
                else
                {
                    SalaryDTO salaryDTO = new SalaryDTO() 
                    {
                        EmployeeEmail=salary.EmployeeEmail,
                        Amount=salary.Amount,
                        Bonuses=salary.Bonuses,
                        Deductions=salary.Deductions
                    };
                    _logger.LogInformation($"salary with employee email {employeeEmail} returned.");
                    return salaryDTO;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while find salary: {ex.Message}");
                _logger.LogError($"Error while find salary: {ex.Message}");
                return null;
            }
        }

        public List<SalaryDTO>? GetSalaries()
        {
            try
            {
                var salaryDTOs = _context.Salaries
                    .Select(s => new SalaryDTO
                    {
                        Amount = s.Amount,
                        Bonuses = s.Bonuses,
                        Deductions = s.Deductions,
                        EmployeeEmail = s.EmployeeEmail
                    })
                    .ToList();
                _logger.LogInformation("retrived all salaries");
                return salaryDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrive all salaries: {ex.Message}");
                _logger.LogError($"Error while retrive all salaries: {ex.Message}");
                return null;
            }
        }
    }
}
