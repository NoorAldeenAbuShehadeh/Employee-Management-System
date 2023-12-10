using Employee_Management_System.Model;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace Employee_Management_System.DAL
{
    public class DUsers : IDUsers
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DUsers> _logger;
        public DUsers(AppDbContext context, ILogger<DUsers> logger)
        {
            _context = context;
            _logger = logger;
        }
        public void AddUser(UserDTO userDTO)
        {
            try
            {
                if (userDTO == null)
                {
                    Console.WriteLine("Bad Request should has a user to add it");
                    _logger.LogError("Bad Request should has a user to add it");
                }
                else
                {
                    User user = new User()
                    {
                        Email = userDTO.Email,
                        Name = userDTO.Name,
                        Password = userDTO.Password,
                        Role = userDTO.Role,
                        Status = userDTO.Status
                    };
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    Console.WriteLine($"user Added");
                    _logger.LogInformation($"Added new user: {user.Email}");
                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"Error while add new user: {ex.Message}");
                _logger.LogError($"Error while add new user: {ex.Message}");
            }
        }
        public List<UserDTO>? GetUsers()
        {
            try
            {
                var userDTOs = _context.Users
                    .Select(u => new UserDTO
                    {
                        Email = u.Name,
                        Name = u.Name,
                        Role = u.Role,
                    })
                    .ToList();
                _logger.LogInformation("retrived all users");
                return userDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrive all departments: {ex.Message}");
                _logger.LogError($"Error while retrive all departments: {ex.Message}");
                return null;
            }
        }
        public UserDTO? GetUser(string email)
        {
            try
            {
                User? user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    Console.WriteLine($"user with email {email} not found.");
                    _logger.LogError($"user with email {email} not found.");
                    return null;
                }
                UserDTO userDTO = new UserDTO
                {
                    Email = user.Email,
                    Name = user.Name,
                    Role = user.Role,
                };
                _logger.LogError($"Get data for user");
                return userDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while get user: {ex.Message}");
                _logger.LogError($"Error while get user: {ex.Message}");
                return null;
            }
        }
        public void DeleteUser(UserDTO userDTO)
        {
            try
            {
                User? user = _context.Users.FirstOrDefault(u => u.Email == userDTO.Email);
                if (user == null)
                {
                    Console.WriteLine($"User with email {userDTO?.Email} not found.");
                    _logger.LogError($"User with email {userDTO?.Email} not found.");
                }
                else
                {
                    user.Status = "inActive";
                    _context.SaveChanges();
                    _logger.LogInformation($"User with email {userDTO.Email} deleted.");
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Error while delete user: {ex.Message}");
                _logger.LogError($"Error while delete user: {ex.Message}");
            }
        }
        public UserDTO? LogIn(string email, string password)
        {
            try
            {
                User? user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    Console.WriteLine($"User with email {email} not found.");
                    _logger.LogError($"User with email {email} not found.");
                    return null;
                }
                else
                {
                    string userPassword = user.Password;
                    if (userPassword == password) 
                    {
                        _logger.LogInformation($"User with email {email} logged in.");
                        UserDTO userDTO = new UserDTO() 
                            {  
                                Email = user.Email,
                                Role=user.Role,
                                Name=user.Name,
                                Status=user.Status,
                                Password=user.Password 
                            };
                        return userDTO;
                    }
                    else
                    {
                        Console.WriteLine("wrong password.");
                        _logger.LogInformation($"User with email {email} try to logg in but enter wrong password.");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update user: {ex.Message}");
                _logger.LogError($"Error while update user: {ex.Message}");
                return null;
            }
        }
        public void UpdateUser(UserDTO userDTO)
        {
           try
            {
                User? user = _context.Users.FirstOrDefault(u => u.Email == userDTO.Email);
                if (user == null)
                {
                    Console.WriteLine($"User with email {userDTO?.Email} not found.");
                    _logger.LogError($"User with email {userDTO?.Email} not found.");
                }
                else
                {
                    user.Password = userDTO.Password;
                    user.Name = userDTO.Name;
                    user.Role = userDTO.Role;
                    _context.SaveChanges();
                    _logger.LogInformation($"User with email {userDTO.Email} updated.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error while update user: {ex.Message}");
                _logger.LogError($"Error while update user: {ex.Message}");
            }
        }
    }
}
