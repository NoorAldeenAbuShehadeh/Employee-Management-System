using Employee_Management_System.Model;

namespace Employee_Management_System.DAL
{
    public interface IDSalary
    {
        public void AddSalary(SalaryDTO salaryDTO);
        public void UpdateSalary(SalaryDTO salaryDTO);
        public SalaryDTO? GetSalary(string employeeEmail);
        public List<SalaryDTO>? GetSalaries();
    }
}
