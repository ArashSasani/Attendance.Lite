namespace AttendanceManagement.Service.Dtos.ApprovalProc
{
    public class PartialUpdateApprovalProcDto
    {
        public Core.Model.ApprovalProc ApprovalProcEntity { get; set; }
        public ApprovalProcPatchDto PatchDto { get; set; }
    }

    public class ApprovalProcPatchDto
    {
        public string Title { get; set; }
    }
}
