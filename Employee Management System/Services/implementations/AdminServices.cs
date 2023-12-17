using Employee_Management_System.DAL;
using Employee_Management_System.Model;
using Microsoft.Extensions.Logging;

namespace Employee_Management_System.Services
{
    public class AdminServices : IAdminServices
    {
        private IDDepartments _dDepartments;
        private IDEmployees _dEmployees;
        private IDUsers _dUsers;
        private IDSalary _dSalary;
        private IDLeave _dLeave;
        private IDAttendance _dAttendance;
        private IDCommitDBChanges _dCommitDBChanges;
        private readonly ILogger<AdminServices> _logger;
        public AdminServices(IDDepartments dDepartments, IDEmployees dEmployees, IDUsers dUsers, IDSalary dSalary, IDLeave dLeave, IDAttendance dAttendance, IDCommitDBChanges dCommitDBChanges, ILogger<AdminServices> logger)
        {
            _dDepartments = dDepartments;
            _dEmployees = dEmployees;
            _dUsers = dUsers;
            _dSalary = dSalary;
            _dLeave = dLeave;
            _dAttendance = dAttendance;
            _dCommitDBChanges = dCommitDBChanges;
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
            try
            {
                Console.Clear();
                Console.Write("Enter employee email: ");
                string? email = Console.ReadLine();
                Console.Write("Enter employee Name: ");
                string? name = Console.ReadLine();
                Console.Write("Enter employee Role (manager, employee): ");
                string? role = Console.ReadLine();
                Console.Write("Enter employee password: ");
                string? password = Console.ReadLine();
                Console.Write("Enter employee department name: ");
                string? departmentName = Console.ReadLine();
                Console.Write("Enter employee phone number: ");
                string? phoneNumber = Console.ReadLine();
                Console.Write("Enter employee address: ");
                string? address = Console.ReadLine();
                Console.Write("Enter salary amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                Console.Write("Enter salary bonuses: ");
                decimal bonuses = decimal.Parse(Console.ReadLine());
                Console.Write("Enter salary deductions: ");
                decimal deductions = decimal.Parse(Console.ReadLine());

                string? encriptedPass = _dUsers.EncodePassword(password);
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
                    EmployeeDTO employeeDTO = new EmployeeDTO()
                    {
                        UserEmail = email,
                        DepartmentName = departmentName,
                        PhoneNumber = phoneNumber,
                        Address = address
                    };
                    bool employeeAdded = _dEmployees.AddEmployee(employeeDTO);
                    if (employeeAdded)
                    {
                        SalaryDTO salaryDTO = new SalaryDTO()
                        {
                            EmployeeEmail = email,
                            Amount = amount,
                            Bonuses = bonuses,
                            Deductions = deductions
                        };
                        bool salaryAdded = _dSalary.AddSalary(salaryDTO);
                        if (salaryAdded)
                        {
                            _dCommitDBChanges.SaveChanges();
                        }
                        if (salaryAdded&&role == "manager")
                        {
                            DepartmentDTO departmentDTO = new DepartmentDTO() { Name = departmentName, ManagerEmail = email };
                            bool departmentUpdated = _dDepartments.UpdateDepartment(departmentDTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add new employee: {ex.Message}");
                _logger.LogError($"Error while add new employee: {ex.Message}");
            }
        }
        public void UpdateDepartment()
        {
            try
            {
                Console.Clear();
                Console.Write("Enter department name: ");
                string departmentName = Console.ReadLine();
                Console.Write("Enter new manager email: ");
                string email = Console.ReadLine();
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
                if (userDTO != null && employeeDTO != null)
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
                        if (userUpdated)
                        {
                            if(role == "manager")
                            {
                                bool departmentUpdated = _dDepartments.UpdateDepartment(new DepartmentDTO { ManagerEmail = email, Name = deprtmentName });
                                if (departmentUpdated) _dCommitDBChanges.SaveChanges();
                            }
                            else 
                            {
                                _dCommitDBChanges.SaveChanges();
                            }
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
                    Console.WriteLine($"department name: {department.Name}, manager email: {department.ManagerEmail}");
                });
                _logger.LogInformation($"Get data for all departments");
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
                List<UserDTO>? users = new List<UserDTO>();
                employees?.ForEach(employee =>
                {
                    users.Add(_dUsers.GetUser(employee.UserEmail));
                });
                for(int i = 0; i < users.Count; i++)
                {
                    Console.WriteLine($"name: {users[i]?.Name}, email: {users[i]?.Email}, Department: {employees[i].DepartmentName}, role: {users[i]?.Role}");
                }
                _logger.LogInformation($"Get data for all employees");
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
                    Console.WriteLine($"employee email: {salary.EmployeeEmail}, total salary: {salary.Amount - salary.Deductions + salary.Bonuses}");
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
        public void LeaveTrend()
        {
            try
            {
                _dLeave.GetApprovedLeaves()?
                    .GroupBy(l => l.EmployeeEmail)?
                    .OrderByDescending(lg => lg.Count())
                    .ToList()?
                    .ForEach(lg =>
                    {
                        var employeeEmail = lg.Key;
                        var totalLeaves = lg.Count();
                        Console.WriteLine($"EmployeeEmail: {employeeEmail}, TotalLeaves: {totalLeaves}");
                    });

                _logger.LogInformation($"Get leave trend");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get leave trend: {ex.Message}");
                _logger.LogError($"Error while get leave trend: {ex.Message}");
            }
        }
        public void GetAttendances()
        {
            try
            {
                Console.Clear();
                Console.Write("Enter start date (yyyy-MM-dd): ");
                DateTime startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None);
                _dAttendance.GetAttendances(startDate)?.ForEach(a =>
                {
                    Console.WriteLine($"employee email: {a.EmployeeEmail}, status: {a.Status}, check in: {a.CheckIn}, check out: {a.CheckOut}");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving attendances: {ex.Message}");
                _logger.LogError($"Error while retrieving attendances: {ex.Message}");
            }
        }
        public void GetLeaves()
        {
            try
            {
                _dLeave.GetLeaves()?.OrderBy(l=>l.EmployeeEmail).ToList().ForEach(l =>
                {
                    Console.WriteLine($"Leave Id: {l.Id}, Employee Email: {l.EmployeeEmail}, Description: {l.Description}, StartDate: {l.StartDate}, EndDate: {l.EndDate}, Status: {l.Status.ToString()}");
                });

                _logger.LogInformation($"Get data for all leaves");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get all leaves: {ex.Message}");
                _logger.LogError($"Error while get all leaves: {ex.Message}");
            }
        }
        public void FilterEmployeesBySalary()
        {
            try
            {
                _dEmployees.GetEmployees(1000)?.ForEach(e =>
                {
                    Console.WriteLine($"email: {e.UserEmail}, Department: {e.DepartmentName}");
                });
                _logger.LogInformation($"Get data for all employees");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get departments: {ex.Message}");
                _logger.LogError($"Error while get departments: {ex.Message}");
            }
        }
    }
}
