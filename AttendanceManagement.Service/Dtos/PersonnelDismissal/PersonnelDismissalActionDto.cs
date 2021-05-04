using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.PersonnelDismissal
{
    public class PersonnelDismissalActionDto
    {
        public int? ParentApprovalProcId { get; set; }
        public int PersonnelDismissalId { get; set; }
        public string ActionDescription { get; set; }
        public RequestAction RequestAction { get; set; }

        #region Message Info
        public int MessageId { get; set; }
        public string ReceiverId { get; set; }
        public string SenderId { get; set; }
        public int RequestId { get; set; }
        public RequestType RequestType { get; set; }
        #endregion
    }
}
