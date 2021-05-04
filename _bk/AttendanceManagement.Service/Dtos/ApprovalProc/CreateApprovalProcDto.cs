using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.ApprovalProc
{
    public class CreateApprovalProcDto
    {
        public int? ParentId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        public int FirstPriorityId { get; set; }
        public int? SecondPriorityId { get; set; }
        public int? ThirdPriorityId { get; set; }
        public ActiveState ActiveState { get; set; }
    }
}
