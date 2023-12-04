using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model
{
    public class Department
    {
        [Key]
        public string Name { get; set; }
        public string ManagerEmail { get; set; }
        public Employee Manager { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
