namespace AttendanceManagement.Service.Dtos.PersonnelApprovalProc
{
    public class PersonnelApprovalProcDto
    {
        public int Id { get; set; }
        public int? DismissalApprovalProcId { get; set; }
        public int? DutyApprovalProcId { get; set; }
        public int? ShiftReplacementProcId { get; set; }
    }
}
