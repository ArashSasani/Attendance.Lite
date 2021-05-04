using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.Duty
{
    public class CreateDutyDto
    {
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        public int? ActionLimitDays { get; set; }
    }
}
