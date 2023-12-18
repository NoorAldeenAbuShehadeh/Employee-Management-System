using Employee_Management_System.DAL;
using Employee_Management_System.Model;
using Microsoft.Extensions.Logging;

namespace Employee_Management_System.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        private IDLeave _dLeave;
        private IDAttendance _dAttendance;
        private IDEmployees _dEmployee;
        private IDUsers _dUsers;
        private IDSalary _dSalary;
        private IDCommitDBChanges _dCommitDBChanges;
        private ILogger<EmployeeServices> _logger;
        public EmployeeServices(IDLeave dLeave, IDAttendance dAttendance, IDEmployees dEmployees, IDUsers dUsers, IDSalary dSalary, IDCommitDBChanges dCommitDBChanges, ILogger<EmployeeServices> logger) 
        {
            _dLeave = dLeave;
            _dAttendance = dAttendance;
            _dEmployee = dEmployees;
            _dUsers = dUsers;
            _dSalary = dSalary;
            _dCommitDBChanges = dCommitDBChanges;
            _logger = logger;
        }

        public void AddLeave(UserDTO userDTO)
        {
            try
            {
                Console.Write("Enter leave description: ");
                string description = Console.ReadLine();
                Console.Write("Enter start date and time (yyyy-MM-dd HH:mm:ss): ");
                DateTime startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None);
                Console.Write("Enter end date and time (yyyy-MM-dd HH:mm:ss): ");
                DateTime endDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None);
                LeaveDTO leave = new LeaveDTO 
                {
                    EmployeeEmail = userDTO.Email,
                    Description = description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Status = LeaveStatus.Pending,
                };
                _dLeave.AddLeave(leave);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add leave: {ex.Message}");
                _logger.LogError($"Error while add leave: {ex.Message}");
            }
        }
        public void AddAttendance(UserDTO userDTO)
        {
            try
            {
                Console.Write("Enter attendance status enter 0 if Present, 1 if Absent, 2 if Vacation, 3 if Remote: ");
                AttendanceStatus status = Enum.Parse<AttendanceStatus>(Console.ReadLine());
                DateTime? checkIn = null;
                DateTime? checkOut = null;
                Console.Write("Enter check in date and time (yyyy-MM-dd HH:mm:ss): ");
                checkIn = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None);
                if (status == AttendanceStatus.Remote || status == AttendanceStatus.Present)
                { 
                    Console.Write("Enter check out date and time (yyyy-MM-dd HH:mm:ss): ");
                    checkOut = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None);
                }
                AttendanceDTO attendance = new AttendanceDTO
                {
                    EmployeeEmail = userDTO.Email,
                    CheckIn = checkIn,
                    CheckOut = checkOut,
                    Status = status
                };
                _dAttendance.AddAttendance(attendance);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while add leave: {ex.Message}");
                _logger.LogError($"Error while add leave: {ex.Message}");
            }
        }
        public void UpdateInformations(UserDTO userDTO)
        {
            try
            {
                EmployeeDTO? emp = _dEmployee.GetEmployee(userDTO.Email);
                if (userDTO != null && emp!= null)
                {
                    Console.WriteLine("1. Change password");
                    Console.WriteLine("2. Change address");
                    Console.WriteLine("3. Change phone number");
                    Console.Write("Enter your choice: ");
                    int choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            Console.Write("Enter old password: ");
                            string oldPassword = _dUsers.EncodePassword(Console.ReadLine());
                            if(oldPassword == userDTO.Password)
                            {
                                Console.Write("Enter new password: ");
                                userDTO.Password = _dUsers.EncodePassword(Console.ReadLine());
                                bool userUpdated = _dUsers.UpdateUser(userDTO);
                                if(userUpdated) _dCommitDBChanges.SaveChanges();
                            }
                            else
                            {
                                Console.WriteLine("Wrong password");
                            }
                            break;
                        case 2:
                            Console.Write("Enter new address: ");
                            emp.Address = Console.ReadLine();
                            if (_dEmployee.UpdateEmployee(emp)) _dCommitDBChanges.SaveChanges();
                            break;
                        case 3:
                            Console.Write("Enter new phone number: ");
                            emp.PhoneNumber = Console.ReadLine();
                            if (_dEmployee.UpdateEmployee(emp)) _dCommitDBChanges.SaveChanges();
                            break;
                        default:
                            Console.WriteLine("Wrong choice");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update employee info: {ex.Message}");
                _logger.LogError($"Error while update employee info: {ex.Message}");
            }
        }
        public void GetAttendanceReport(UserDTO userDTO)
        {
            try
            {
                Console.Write("Enter start date (yyyy-MM-dd): ");
                DateTime startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None);
                _dAttendance.GetAttendanceReport(userDTO.Email, startDate)?.ForEach(attendance => 
                {
                    Console.WriteLine($"status: {attendance.Status.ToString()}, check in: {attendance.CheckIn}, check out: {attendance.CheckOut}");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get attendance report: {ex.Message}");
                _logger.LogError($"Error while get attendance report: {ex.Message}");
            }
        }
        public void GetLeaves(UserDTO userDTO)
        {
            try
            {
                _dLeave.GetLeaves(userDTO.Email)?.ForEach(l =>
                {
                    Console.WriteLine($"Id: {l.Id}, description: {l.Description}, start date: {l.StartDate}, end date: {l.EndDate}, status: {l.Status.ToString()}");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get employee leaves: {ex.Message}");
                _logger.LogError($"Error while get employee leaves: {ex.Message}");
            }
        }
        public void GetSalary(UserDTO userDTO)
        {
            try
            {
                SalaryDTO? salaryDTO = _dSalary.GetSalary(userDTO.Email);
                if (salaryDTO != null)
                {
                    Console.WriteLine($"Salary amount: {salaryDTO.Amount}, bonuses: {salaryDTO.Bonuses}, deductions: {salaryDTO.Deductions}");
                }
                else
                {
                    Console.WriteLine($"No salary information");
                    _logger.LogInformation($"No salary information");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get employee salary: {ex.Message}");
                _logger.LogError($"Error while get employee salary: {ex.Message}");
            }
        }
        public void GetInformation(UserDTO userDTO)
        {
            try
            {
                EmployeeDTO? emp = _dEmployee.GetEmployee(userDTO.Email);
                SalaryDTO? salary = _dSalary.GetSalary(userDTO.Email);
                if (emp != null)
                {
                    Console.WriteLine($"Email: {emp.UserEmail}, Address: {emp.Address}, Phone number: {emp.PhoneNumber}, Department name: {emp.DepartmentName}, Role: {userDTO.Role}, Salary: {salary?.Amount+salary?.Bonuses-salary?.Deductions}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error while get employee information: {ex.Message}");
                _logger.LogError($"Error while get employee information: {ex.Message}");
            }
        }
    }
}
