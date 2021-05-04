using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.WorkUnit
{
    public class CreateWorkUnitDto
    {
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
    }
}
