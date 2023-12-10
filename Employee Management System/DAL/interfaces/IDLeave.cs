using Employee_Management_System.Model;
namespace Employee_Management_System.DAL
{
    public interface IDLeave
    {
        public void AddLeave(LeaveDTO leaveDTO);
        public void UpdateLeave(LeaveDTO leaveDTO);
        public List<LeaveDTO>? GetLeaves(string employeeEmail);
        public List<LeaveDTO>? GetLeaves();
        public List<LeaveDTO>? GetPendingLeaves();
    }
}
