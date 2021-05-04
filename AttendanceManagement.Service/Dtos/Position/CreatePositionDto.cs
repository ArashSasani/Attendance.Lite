using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.Position
{
    public class CreatePositionDto
    {
        public int WorkUnitId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
    }
}
