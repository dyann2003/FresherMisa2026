using FresherMisa2026.Entities.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace FresherMisa2026.Entities.Employee
{
    [ConfigTable("employee", false, "EmployeeCode")]
    public class Employee : BaseModel
    {
        [Key]
        public Guid EmployeeID { get; set; }

        [IRequired]
        public string? EmployeeCode { get; set; }

        [IRequired]
        public string? EmployeeName { get; set; }

        public int? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        [IRequired]
        public Guid? DepartmentID { get; set; }

        [IRequired]
        public Guid? PositionID { get; set; }

        public decimal? Salary { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}