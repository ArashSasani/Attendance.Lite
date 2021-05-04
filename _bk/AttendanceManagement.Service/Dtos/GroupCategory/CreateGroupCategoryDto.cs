using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.GroupCategory
{
    public class CreateGroupCategoryDto
    {
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
    }
}
