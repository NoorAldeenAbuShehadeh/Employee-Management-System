using Employee_Management_System.Model;

namespace Employee_Management_System.DAL
{
    public interface IDUsers
    {
        public bool AddUser(UserDTO userDTO);
        public bool UpdateUser(UserDTO userDTO);
        public List<UserDTO>? GetUsers();
        public UserDTO? GetUser(string email);
        public bool DeleteUser(string email);
        public UserDTO LogIn(string email, string password);
        public string? EncodePassword(string password);   
    }
}
