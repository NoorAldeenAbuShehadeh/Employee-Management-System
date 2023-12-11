using Employee_Management_System.Model;
namespace Employee_Management_System.DAL
{
    public interface IDLeave
    {
        public bool AddLeave(LeaveDTO leaveDTO);
        public bool UpdateLeave(LeaveDTO leaveDTO);
        public LeaveDTO? GetLeave(int id);
        public List<LeaveDTO>? GetLeaves(string employeeEmail);
        public List<LeaveDTO>? GetLeaves();
        public List<LeaveDTO>? GetPendingLeaves();
    }
}
