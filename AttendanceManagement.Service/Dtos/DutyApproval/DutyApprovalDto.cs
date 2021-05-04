namespace AttendanceManagement.Service.Dtos.DutyApproval
{
    public class DutyApprovalDto
    {
        public string PersonnelFullName { get; set; }
        public int DutyId { get; set; }
        public string DutyTitle { get; set; }
    }

    public class DutyApprovalDtoDDL
    {
        public int DutyId { get; set; }
        public string DutyTitle { get; set; }
    }
}
