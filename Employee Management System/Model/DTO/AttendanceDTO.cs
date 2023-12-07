﻿using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model
{
    public class AttendanceDTO
    {
        [Key]
        public int Id { get; set; }
        public string EmployeeEmail { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
