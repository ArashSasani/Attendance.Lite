using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.EmployeementType
{
    public class CreateEmployeementTypeDto
    {
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
    }
}
