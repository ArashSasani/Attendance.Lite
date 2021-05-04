namespace AttendanceManagement.Service.Dtos.ApprovalProc
{
    public class ReceiverInfoDto
    {
        public string ReceiverId { get; set; }
        public int? ParentApprovalProcId { get; set; }
        public bool HasParentProc
        {
            get
            {
                return ParentApprovalProcId.HasValue ? true : false;
            }
        }
    }
}
