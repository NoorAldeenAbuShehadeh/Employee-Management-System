using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model
{
    public class UserDTO
    {
        [Key]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MinLength(5, ErrorMessage = "Name must be at least 5 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [RegularExpression("^(manager|employee)$", ErrorMessage = "Role must be 'manager' or 'employee'")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(active|inActive)$", ErrorMessage = "Status must be 'active' or 'inActive'")]
        public string Status { get; set; }
    }
}
