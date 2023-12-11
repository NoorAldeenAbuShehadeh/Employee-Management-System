using Employee_Management_System.DAL;
using Employee_Management_System.Model;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Employee_Management_System.Services
{
    public class AdminServices : IAdminServices
    {
        private IDDepartments _dDepartments;
        private IDEmployees _dEmployees;
        private IDUsers _dUsers;
        private IDSalary _dSalary;
        private IDLeave _dLeave;// implement leave report for admin
        private IDAttendance _dAttendance;// implement attendance report for admin
        private readonly ILogger<AdminServices> _logger;
        public AdminServices(IDDepartments dDepartments, IDEmployees dEmployees, IDUsers dUsers, IDSalary dSalary, IDLeave dLeave, IDAttendance dAttendance, ILogger<AdminServices> logger) 
        {
            _dDepartments = dDepartments;
            _dEmployees = dEmployees;
            _dUsers = dUsers;
            _dSalary = dSalary;
            _dLeave = dLeave;
            _dAttendance = dAttendance;
            _logger = logger;
        }
        public void AddDepartment()
        {
            try
            {
                Console.Clear();
                Console.Write("Enter department name: ");
                string name = Console.ReadLine();
                DepartmentDTO departmentDTO = new DepartmentDTO()
                {
                    Name = name,
                    ManagerEmail = null
                };
                _dDepartments.AddDepartment(departmentDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add new employee: {ex.Message}");
                _logger.LogError($"Error while add new employee: {ex.Message}");
            }
        }
        public void AddEmployee()
        {
            try {
                Console.Clear();
                Console.Write("Enter employee email: ");
                string email = Console.ReadLine();
                Console.Write("Enter employee Name: ");
                string name = Console.ReadLine();
                Console.Write("Enter employee Role (manager, employee): ");
                string role = Console.ReadLine();
                Console.Write("Enter employee password: ");
                string password = Console.ReadLine();
                string encriptedPass = EncodePassword(password);
                UserDTO userDTO = new UserDTO()
                {
                    Email = email,
                    Name = name,
                    Role = role,
                    Password = encriptedPass,
                    Status = "active"
                };
                bool userAdded = _dUsers.AddUser(userDTO);
                if (userAdded)
                {
                    Console.Write("Enter employee department name: ");
                    string departmentName = Console.ReadLine();
                    Console.Write("Enter employee phone number: ");
                    string phoneNumber = Console.ReadLine();
                    Console.Write("Enter employee address: ");
                    string address = Console.ReadLine();
                    EmployeeDTO employeeDTO = new EmployeeDTO()
                    {
                        UserEmail = email,
                        DepartmentName = departmentName,
                        PhoneNumber = phoneNumber,
                        Address = address
                    };
                    bool employeeAdded = _dEmployees.AddEmployee(employeeDTO);
                    if (employeeAdded && role == "manager")
                    {
                        DepartmentDTO departmentDTO = new DepartmentDTO() { Name = departmentName, ManagerEmail = email };
                        _dDepartments.UpdateDepartment(departmentDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add new employee: {ex.Message}");
                _logger.LogError($"Error while add new employee: {ex.Message}");
            }
        }
        public void AddSalary()
        {
            try 
            {
                Console.Clear();
                Console.Write("Enter employee email: ");
                string email = Console.ReadLine();
                Console.Write("Enter salary amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                Console.Write("Enter salary bonuses: ");
                decimal bonuses = decimal.Parse(Console.ReadLine());
                Console.Write("Enter salary deductions: ");
                decimal deductions = decimal.Parse(Console.ReadLine());
                SalaryDTO salaryDTO = new SalaryDTO()
                {
                    EmployeeEmail = email,
                    Amount = amount,
                    Bonuses = bonuses,
                    Deductions = deductions
                };
                _dSalary.AddSalary(salaryDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add salary to employee: {ex.Message}");
                _logger.LogError($"Error while add salary to employee: {ex.Message}");
            }
        }
        public void UpdateDepartment()
        {
            try
            {
                Console.Clear();
                Console.Write("Enter department name: ");
                string departmentName = Console.ReadLine();
                Console.Write("Enter employee email: ");
                string email = Console.ReadLine();
                Console.Write("Enter employee phone number: ");
                DepartmentDTO departmentDTO = new DepartmentDTO()
                {
                    Name = departmentName,
                    ManagerEmail = email
                };
                _dDepartments.UpdateDepartment(departmentDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update department: {ex.Message}");
                _logger.LogError($"Error while update department: {ex.Message}");
            }
        }
        public void UpdateEmployee()
        {
            try
            {
                Console.Clear();
                Console.Write("Enter employee email: ");
                string email = Console.ReadLine();
                EmployeeDTO? employeeDTO = _dEmployees.GetEmployee(email);
                UserDTO? userDTO = _dUsers.GetUser(email);
                if(userDTO != null && employeeDTO!= null) 
                {
                    Console.Write("Enter new Department name: ");
                    string deprtmentName = Console.ReadLine();
                    Console.Write("Enter new role (manager, employee): ");
                    string role = Console.ReadLine();
                    employeeDTO.DepartmentName = deprtmentName;
                    userDTO.Role = role;
                    bool employeeUpdated = _dEmployees.UpdateEmployee(employeeDTO);
                    if (employeeUpdated)
                    {
                        bool userUpdated = _dUsers.UpdateUser(userDTO);
                        if(userUpdated && role == "manager")
                        {
                            _dDepartments.UpdateDepartment(new DepartmentDTO { ManagerEmail = email, Name = deprtmentName });
                        }
                    }
                    
                }
                else
                {
                    Console.WriteLine($"employee with email {email} not found.");
                    _logger.LogError($"employee with email {email} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update employee: {ex.Message}");
                _logger.LogError($"Error while update employee: {ex.Message}");
            }
        }
        public void UpdateSalary()
        {
            try
            {
                Console.Clear();
                Console.Write("Enter employee email: ");
                string email = Console.ReadLine();
                Console.Write("Enter salary amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                Console.Write("Enter salary bonuses: ");
                decimal bonuses = decimal.Parse(Console.ReadLine());
                Console.Write("Enter salary deductions: ");
                decimal deductions = decimal.Parse(Console.ReadLine());
                SalaryDTO salaryDTO = new SalaryDTO()
                {
                    EmployeeEmail = email,
                    Amount = amount,
                    Bonuses = bonuses,
                    Deductions = deductions
                };
                _dSalary.UpdateSalary(salaryDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update salary of employee: {ex.Message}");
                _logger.LogError($"Error while update salary of employee: {ex.Message}");
            }
        }
        public void GetDepartments()
        {
            try
            {
                List<DepartmentDTO>? departments = _dDepartments.GetDepartments();
                departments?.ForEach(department =>
                {
                    Console.Write($"department name: {department.Name}, manager email: {department.ManagerEmail}");
                });
                _logger.LogError($"Get data for all departments");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get departments: {ex.Message}");
                _logger.LogError($"Error while get departments: {ex.Message}");
            }
        }
        public void GetEmployees() 
        {
            try
            {
                List<EmployeeDTO>? employees = _dEmployees.GetEmployees();
                List<UserDTO>? users = _dUsers.GetUsers();
                var employeesData = from emp in employees
                             join user in users on emp.UserEmail equals user.Email
                             where user.Status == "active" select new { emp, user };
                foreach( var employeeData in employeesData)
                {
                    Console.Write($"name: {employeeData.user.Name}, email: {employeeData.user.Email}, role: {employeeData.user.Role}, Department: {employeeData.emp.DepartmentName}");
                }
                _logger.LogError($"Get data for all employees");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get departments: {ex.Message}");
                _logger.LogError($"Error while get departments: {ex.Message}");
            }
        }
        public void GetEmployeeSalaries()
        {
            try
            {
                List<SalaryDTO>? salaries = _dSalary.GetSalaries();
                salaries?.ForEach(salary =>
                {
                    Console.Write($"{salary.EmployeeEmail} => {salary.Amount-salary.Deductions+salary.Bonuses}");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get salaries of employees: {ex.Message}");
                _logger.LogError($"Error while get salaries of employees: {ex.Message}");
            }
        }
        public void GetEmployeeSalary()
        {
            try
            {
                Console.Clear();
                Console.Write("Enter employee email: ");
                string email = Console.ReadLine();
                SalaryDTO? salaryDTO = _dSalary.GetSalary(email);
                if (salaryDTO != null)
                {
                    Console.WriteLine($"Salary information\nemployee email: {salaryDTO.EmployeeEmail}, amount: {salaryDTO.Amount}, bonuses: {salaryDTO.Bonuses}, deductions: {salaryDTO.Deductions}");
                }
                else
                {
                    Console.WriteLine($"No salary information for employee {email}");
                    _logger.LogInformation($"No salary information for employee {email}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get salary of employee: {ex.Message}");
                _logger.LogError($"Error while get salary of employee: {ex.Message}");
            }
        }
        public string? EncodePassword(string password)
        {
            try 
            { 
                byte[] bytes = Encoding.Unicode.GetBytes(password);
                byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
                return Convert.ToBase64String(inArray);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error while encoding password: {ex.Message}");
                return null;
            }
        }
    }
}
