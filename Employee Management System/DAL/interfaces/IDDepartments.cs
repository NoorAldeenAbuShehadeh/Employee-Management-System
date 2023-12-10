using Employee_Management_System.Model;

namespace Employee_Management_System.DAL
{
    public interface IDDepartments
    {
        public void AddDepartment(DepartmentDTO departmentDTO);
        public void UpdateDepartment(DepartmentDTO departmentDTO);
        public List<DepartmentDTO>? GetDepartments();
    }
}
