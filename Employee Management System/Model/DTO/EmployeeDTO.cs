using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model
{
    public class EmployeeDTO
    {
        [Key]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Department Name is required")]
        public string DepartmentName { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
