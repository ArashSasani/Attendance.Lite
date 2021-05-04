using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.PersonnelDuty
{
    public class PersonnelDutyDto
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public string PersonnelFullName { get; set; }
        public int DutyId { get; set; }
        public string DutyTitle { get; set; }
        public string SubmittedDate { get; set; }
        public RequestDuration DutyDuration { get; set; }
        public string DutyDurationTitle { get; set; }
        public string RequestDescription { get; set; }
        public string FileUploadPath { get; set; }

        #region confirmation or rejection
        public string ActionDate { get; set; }
        public RequestAction RequestAction { get; set; }
        public string RequestActionTitle { get; set; }
        public string ActionDescription { get; set; }
        #endregion

        #region Daily
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        #endregion

        #region Hourly
        public string Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        #endregion
    }
}
