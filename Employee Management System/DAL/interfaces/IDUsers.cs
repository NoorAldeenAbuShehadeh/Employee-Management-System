using Employee_Management_System.Model;

namespace Employee_Management_System.DAL
{
    public interface IDUsers
    {
        public void AddUser(UserDTO userDTO);
        public void UpdateUser(UserDTO userDTO);
        public List<UserDTO>? GetUsers();
        public UserDTO? GetUser(string email);
        public void DeleteUser(UserDTO userDTO);
        public UserDTO LogIn(string email, string password);
    }
}
