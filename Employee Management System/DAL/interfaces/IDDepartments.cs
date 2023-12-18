using Employee_Management_System.Model;

namespace Employee_Management_System.DAL
{
    public interface IDDepartments
    {
        public bool AddDepartment(DepartmentDTO departmentDTO);
        public bool UpdateDepartment(DepartmentDTO departmentDTO);
        public List<DepartmentDTO>? GetDepartments();
        public List<KeyValuePair<string, int>>? DepartmentsStatistics();
    }
}
