using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Core.Model
{
    public class ApprovalProc : IActiveEntity
    {
        public int Id { get; set; }
        [ForeignKey("ParentProc")]
        public int? ParentId { get; set; }
        [Required]
        public string Title { get; set; }
        public int FirstPriorityId { get; set; }
        public int? SecondPriorityId { get; set; }
        public int? ThirdPriorityId { get; set; }
        public DeleteState DeleteState { get; set; }
        public ActiveState ActiveState { get; set; }

        public virtual ApprovalProc ParentProc { get; set; }
        public virtual Personnel FirstPriority { get; set; }
        public virtual Personnel SecondPriority { get; set; }
        public virtual Personnel ThirdPriority { get; set; }

        public ICollection<ApprovalProc> ChildProcs { get; set; }
        public ICollection<PersonnelApprovalProc> DismissalProcs { get; set; }
        public ICollection<PersonnelApprovalProc> DutyProcs { get; set; }
        public ICollection<PersonnelApprovalProc> ShiftReplacementProcs { get; set; }
    }
}
