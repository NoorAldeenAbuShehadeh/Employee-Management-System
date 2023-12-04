using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model
{
    public class Employee
    {
        [Key]
        public string UserEmail { get; set; }
        [Required]
        public string DepartmentName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public User User { get; set; }
        public Department Department { get; set; }
        public Salary Salary { get; set; }
        public ICollection<Leave> Leaves { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
    }
}
