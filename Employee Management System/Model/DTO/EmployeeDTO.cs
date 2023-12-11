using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model
{
    public class EmployeeDTO
    {
        [Key]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Department Name is required")]
        [MinLength(2, ErrorMessage = "Department Name must be at least 2 characters")]
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MinLength(5, ErrorMessage = "Address must be at least 5 characters")]
        public string Address { get; set; }
    }
}
