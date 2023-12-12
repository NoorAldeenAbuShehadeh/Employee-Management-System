using Employee_Management_System.DAL;
using Employee_Management_System.Model;
using Microsoft.Extensions.Logging;

namespace Employee_Management_System.Services
{
    public class EmployeeManagmentServices : IEmployeeManagmentServices
    {
        private readonly IAdminServices _adminServices;
        private readonly IEmployeeServices _employeeServices;
        private readonly IManagerServices _managerServices;
        private readonly IDUsers _dUsers;
        private readonly IDEmployees _dEmployees;
        private readonly ILogger<EmployeeManagmentServices> _logger;
        public EmployeeManagmentServices(IAdminServices adminServices, IManagerServices managerServices, IEmployeeServices employeeServices, ILogger<EmployeeManagmentServices> logger, IDUsers dUsers, IDEmployees dEmployees)
        {
            _adminServices = adminServices;
            _employeeServices = employeeServices;
            _managerServices = managerServices;
            _logger = logger;
            _dUsers = dUsers;
            _dEmployees = dEmployees;
        }

        public void RenderApp()
        {
            while (true)
            {
                try
                {
                    DisplayMainMenu();
                    int choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            LogIn();
                            break;
                        case 2:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred : {ex.Message}");
                    _logger.LogError(ex, $"An error occurred while choosing choice in main menu: {ex.Message}");
                }
            }
        }
        private void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Log In");
            Console.WriteLine("2. Exit");
            Console.Write("Enter your choice: ");
        }
        private void LogIn()
        {
            try
            {
                Console.Clear();
                Console.Write("Enter your email: ");
                string? email = Console.ReadLine();
                Console.Write("Enter your password: ");
                string? password = Console.ReadLine();
                string? encriptPassword = _dUsers.EncodePassword(password);
                UserDTO user = _dUsers.LogIn(email, encriptPassword);
                switch (user?.Role)
                {
                    case "admin":
                        AdminFunctionalities(user);
                        break;
                    case "manager":
                        ManagerFunctionalities(user);
                        break;
                    case "employee":
                        EmployeeFunctionalities(user);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred : {ex.Message}");
                _logger.LogError(ex, $"An error occurred while user try to log in: {ex.Message}");
            }
        }
        private void AdminFunctionalities(UserDTO user)
        {
            bool logOut = false;
            while (!logOut)
            {
                try
                {
                    Console.Clear();
                    DisplayAdminMenu();
                    int choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            _adminServices.AddDepartment();
                            break;
                        case 2:
                            _adminServices.AddEmployee();
                            break;
                        case 3:
                            _adminServices.GetDepartments();
                            break;
                        case 4:
                            _adminServices.GetEmployees();
                            break;
                        case 5:
                            _adminServices.GetEmployeeSalaries();
                            break;
                        case 6:
                            _adminServices.GetEmployeeSalary();
                            break;
                        case 7:
                            _adminServices.UpdateDepartment();
                            break;
                        case 8:
                            _adminServices.UpdateEmployee();
                            break;
                        case 9:
                            _adminServices.UpdateSalary();
                            break;
                        case 10:
                            _adminServices.LeaveTrend();
                            break;
                        case 11:
                            _adminServices.GetAttendances();
                            break;
                        case 20:
                            logOut = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred : {ex.Message}");
                    _logger.LogError(ex, $"An error occurred while choosing choice in admin menu: {ex.Message}");
                }
            }
        }
        private void DisplayAdminMenu()
        {
            Console.WriteLine("1. Add Department");
            Console.WriteLine("2. Add Employee");
            Console.WriteLine("3. Get Departments");
            Console.WriteLine("4. Get Employees");
            Console.WriteLine("5. Get Salaries");
            Console.WriteLine("6. Get Salary info for employee");
            Console.WriteLine("7. Update Deparment");
            Console.WriteLine("8. Update Employee");
            Console.WriteLine("9. Update Salary");
            Console.WriteLine("10. Leave Trend");
            Console.WriteLine("11. Attendance Report");
            Console.WriteLine("20. Log Out");
            Console.Write("Enter your choice: ");
        }
        private void ManagerFunctionalities(UserDTO user)
        {
            bool logOut = false;
            EmployeeDTO manager = _dEmployees.GetEmployee(user.Email);
            while (!logOut)
            {
                try
                {
                    Console.Clear();
                    DisplayManagerMenu();
                    int choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            _managerServices.GetEmployeesInDepartment(manager.DepartmentName);
                            break;
                        case 2:
                            _managerServices.GetPendingLeaves(manager.DepartmentName);
                            break;
                        case 3:
                            _managerServices.UpdateLeave();
                            break;
                        case 4:
                            _managerServices.GetAttendanceForDepartment(manager.DepartmentName);
                            break;
                        case 5:
                            _employeeServices.UpdateInformations(user);
                            break;
                        case 6:
                            logOut = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred : {ex.Message}");
                    _logger.LogError(ex, $"An error occurred while choosing choice in manager menu: {ex.Message}");
                }
            }
        }
        private void DisplayManagerMenu()
        {
            Console.WriteLine("1. Get Employees In Department");
            Console.WriteLine("2. Get Pending Leaves");
            Console.WriteLine("3. Update Leave For Employee");
            Console.WriteLine("4. Get Attendance For Department");
            Console.WriteLine("5. Update informations");
            Console.WriteLine("6. Log Out");
            Console.Write("Enter your choice: ");
        }
        private void EmployeeFunctionalities(UserDTO user)
        {
            bool logOut = false;
            while (!logOut)
            {
                try
                {
                    Console.Clear();
                    DisplayEmployeeMenu();
                    int choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            _employeeServices.AddAttendance(user); 
                            break;
                        case 2:
                            _employeeServices.AddLeave(user);
                            break;
                        case 3:
                            _employeeServices.UpdateInformations(user);
                            break;
                        case 4:
                            _employeeServices.GetAttendanceReport(user);
                            break;
                        case 5:
                            break;
                        case 6:
                            logOut = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred : {ex.Message}");
                    _logger.LogError(ex, $"An error occurred while choosing choice in employee menu: {ex.Message}");
                }
            }
        }
        private void DisplayEmployeeMenu()
        {
            Console.WriteLine("1. Add Attendance");
            Console.WriteLine("2. Add Leave");
            Console.WriteLine("3. Update informations");
            Console.WriteLine("4. get attendance report");
            Console.WriteLine("5. ");
            Console.WriteLine("6. Log Out");
            Console.Write("Enter your choice: ");
        }
    }
}
