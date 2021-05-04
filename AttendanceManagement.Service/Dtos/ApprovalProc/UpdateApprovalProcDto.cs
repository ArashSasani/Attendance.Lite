using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.ApprovalProc
{
    public class UpdateApprovalProcDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        public int FirstPriorityId { get; set; }
        public int? SecondPriorityId { get; set; }
        public int? ThirdPriorityId { get; set; }
        public ActiveState ActiveState { get; set; }
    }
}
