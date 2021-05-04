namespace AttendanceManagement.Service.Dtos.DismissalApproval
{
    public class DismissalApprovalDto
    {
        public string PersonnelFullName { get; set; }
        public int DismissalId { get; set; }
        public string DismissalTitle { get; set; }
    }

    public class DismissalApprovalDtoDDL
    {
        public int DismissalId { get; set; }
        public string DismissalTitle { get; set; }
    }
}
