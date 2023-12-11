using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model
{
    public class LeaveDTO
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "employee email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string EmployeeEmail { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(255, ErrorMessage = "Description must not exceed 255 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format for Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format for End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(LeaveStatus), ErrorMessage = "Invalid value for Status")]
        public LeaveStatus Status { get; set; }
    }
}
