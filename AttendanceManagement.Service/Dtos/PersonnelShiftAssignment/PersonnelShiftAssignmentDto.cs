using System;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.PersonnelShiftAssignment
{
    public class PersonnelShiftAssignmentDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime Date { get; set; }
    }
}
