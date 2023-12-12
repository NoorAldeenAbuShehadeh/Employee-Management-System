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
        private ILogger<EmployeeServices> _logger;
        public EmployeeServices(IDLeave dLeave, IDAttendance dAttendance, IDEmployees dEmployees, IDUsers dUsers, ILogger<EmployeeServices> logger) 
        {
            _dLeave = dLeave;
            _dAttendance = dAttendance;
            _dEmployee = dEmployees;
            _dUsers = dUsers;
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
                Console.Write("Enter attendance status enter 0 if Present, 1 if Absent, 2 if Vacation, 3 if Remote): ");
                AttendanceStatus status = Enum.Parse<AttendanceStatus>(Console.ReadLine());
                DateTime? checkIn = null;
                DateTime? checkOut = null;
                if(status == AttendanceStatus.Remote || status == AttendanceStatus.Present)
                { 
                    Console.Write("Enter start date and time (yyyy-MM-dd HH:mm:ss): ");
                    checkIn = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None);
                    Console.Write("Enter end date and time (yyyy-MM-dd HH:mm:ss): ");
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
                    Console.WriteLine("1. Change phone number");
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
                                _dUsers.UpdateUser(userDTO);
                            }
                            else
                            {
                                Console.WriteLine("Wrong password");
                            }
                            break;
                        case 2:
                            Console.Write("Enter new address: ");
                            emp.Address = Console.ReadLine();
                            _dEmployee.UpdateEmployee(emp);
                            break;
                        case 3:
                            Console.Write("Enter new phone number: ");
                            emp.PhoneNumber = Console.ReadLine();
                            _dEmployee.UpdateEmployee(emp);
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

    }
}
