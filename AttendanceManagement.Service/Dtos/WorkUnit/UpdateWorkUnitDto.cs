using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.WorkUnit
{
    public class UpdateWorkUnitDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
    }
}
