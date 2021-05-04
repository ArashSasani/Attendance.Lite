using System.ComponentModel.DataAnnotations.Schema;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.Model
{
    public class PersonnelApprovalProc
    {
        [ForeignKey("Personnel")]
        public int Id { get; set; }
        public int? DismissalApprovalProcId { get; set; }
        public int? DutyApprovalProcId { get; set; }
        public int? ShiftReplacementProcId { get; set; }
        public DeleteState DeleteState { get; set; }

        public Personnel Personnel { get; set; }
        public ApprovalProc DismissalApprovalProc { get; set; }
        public ApprovalProc DutyApprovalProc { get; set; }
        public ApprovalProc ShiftReplacementProc { get; set; }
    }
}
