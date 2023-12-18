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
                if (user != null)
                {
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
                        case 12:
                            _adminServices.GetLeaves();
                            break;
                        case 13:
                            _adminServices.FilterEmployeesBySalary();
                            break;
                        case 14:
                            _adminServices.DepartmentStatistics();
                            break;
                        case 15:
                            _adminServices.SerchForEmployeesByCity();
                            break;
                        case 16:
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
            Console.WriteLine("1. Add department");
            Console.WriteLine("2. Add employee");
            Console.WriteLine("3. Get departments");
            Console.WriteLine("4. Get employees");
            Console.WriteLine("5. Get salaries");
            Console.WriteLine("6. Get salary info for employee");
            Console.WriteLine("7. Update deparment");
            Console.WriteLine("8. Update employee");
            Console.WriteLine("9. Update salary");
            Console.WriteLine("10. Leave trend");
            Console.WriteLine("11. Attendance report");
            Console.WriteLine("12. Get all leaves");
            Console.WriteLine("13. Get employees have salary >= 1000");
            Console.WriteLine("14. Get department statistics");
            Console.WriteLine("15. Serch for employees by city");
            Console.WriteLine("16. Log out");
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
                            _employeeServices.GetInformation(user);
                            break;
                        case 7:
                            _managerServices.GetLeavesForDepartment(manager.DepartmentName);
                            break;
                        case 8:
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
            Console.WriteLine("5. Update my informations");
            Console.WriteLine("6. See my informations");
            Console.WriteLine("7. Get all leaves");
            Console.WriteLine("8. Log Out");
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
                            _employeeServices.GetLeaves(user);
                            break;
                        case 6:
                            _employeeServices.GetSalary(user);
                            break;
                        case 7:
                            _employeeServices.GetInformation(user);
                            break;
                        case 8:
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
            Console.WriteLine("1. Add attendance");
            Console.WriteLine("2. Add leave");
            Console.WriteLine("3. Update informations");
            Console.WriteLine("4. get attendance report");
            Console.WriteLine("5. Get leaves");
            Console.WriteLine("6. Get salary");
            Console.WriteLine("7. See my informations");
            Console.WriteLine("8. Log Out");
            Console.Write("Enter your choice: ");
        }
    }
}
