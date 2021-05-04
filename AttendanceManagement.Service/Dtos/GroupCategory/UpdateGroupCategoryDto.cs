using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.GroupCategory
{
    public class UpdateGroupCategoryDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
    }
}
