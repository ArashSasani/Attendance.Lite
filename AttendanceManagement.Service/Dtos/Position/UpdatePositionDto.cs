using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.Position
{
    public class UpdatePositionDto
    {
        public int Id { get; set; }
        public int WorkUnitId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
    }
}
