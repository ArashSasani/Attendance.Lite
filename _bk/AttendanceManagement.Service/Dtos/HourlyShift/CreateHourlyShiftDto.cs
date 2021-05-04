using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.HourlyShift
{
    public class CreateHourlyShiftDto
    {
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        public int? HoursShouldWorkInDay { get; set; }
        public int? HoursShouldWorkInWeek { get; set; }
        public int? HoursShouldWorkInMonth { get; set; }
    }
}
