using System.ComponentModel.DataAnnotations.Schema;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Core.Model
{
    public class PersonnelApprovalProc : IEntity
    {
        [ForeignKey("Personnel")]
        public int Id { get; set; }
        public int? DismissalApprovalProcId { get; set; }
        public int? DutyApprovalProcId { get; set; }
        public int? ShiftReplacementProcId { get; set; }
        public DeleteState DeleteState { get; set; }

        public Personnel Personnel { get; set; }
        public virtual ApprovalProc DismissalApprovalProc { get; set; }
        public virtual ApprovalProc DutyApprovalProc { get; set; }
        public virtual ApprovalProc ShiftReplacementProc { get; set; }
    }
}
