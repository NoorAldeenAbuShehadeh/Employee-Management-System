using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model
{
    public class DepartmentDTO
    {
        [Key]
        public string Name { get; set; }

        public string ManagerEmail { get; set; }
    }
}
