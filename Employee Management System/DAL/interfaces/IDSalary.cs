using Employee_Management_System.Model;

namespace Employee_Management_System.DAL
{
    public interface IDSalary
    {
        public bool AddSalary(SalaryDTO salaryDTO);
        public bool UpdateSalary(SalaryDTO salaryDTO);
        public SalaryDTO? GetSalary(string employeeEmail);
        public List<SalaryDTO>? GetSalaries();
    }
}
