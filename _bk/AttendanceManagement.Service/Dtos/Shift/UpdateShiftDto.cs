using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.Shift
{
    public class UpdateShiftDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
    }
}
