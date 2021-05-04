using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.Duty
{
    public class UpdateDutyDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        public int? ActionLimitDays { get; set; }
    }
}
