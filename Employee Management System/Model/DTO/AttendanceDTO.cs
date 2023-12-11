using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model
{
    public class AttendanceDTO
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Employee email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string EmployeeEmail { get; set; }

        [Required(ErrorMessage = "Check In is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format for Check In")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CheckIn { get; set; }

        [Required(ErrorMessage = "Check Out is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format for Check Out")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CheckOut { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(AttendanceStatus), ErrorMessage = "Invalid value for Status")]
        public AttendanceStatus Status { get; set; }
    }
}
