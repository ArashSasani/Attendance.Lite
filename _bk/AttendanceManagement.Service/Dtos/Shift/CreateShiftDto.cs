using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.Shift
{
    public class CreateShiftDto
    {
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
    }
}
