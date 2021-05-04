using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.EmployeementType
{
    public class UpdateEmployeementTypeDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
    }
}
