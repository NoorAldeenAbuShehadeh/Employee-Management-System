using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model
{
    public class Salary
    {
        [Key]
        public string EmployeeEmail { get; set; }
        public decimal Amount { get; set;}
        public decimal Bonuses { get; set;}
        public decimal Deductions { get; set;}
        public Employee Employee { get; set; }
    }
}
