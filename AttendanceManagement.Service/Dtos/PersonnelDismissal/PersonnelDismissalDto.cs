using WebApplication.Infrastructure.Localization;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.PersonnelDismissal
{
    public class PersonnelDismissalDto
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public string PersonnelFullName { get; set; }
        public int DismissalId { get; set; }
        public string DismissalTitle { get; set; }
        public string SubmittedDate { get; set; }
        public RequestDuration DismissalDuration { get; set; }
        public string DismissalDurationTitle { get; set; }
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

    public class PersonnelDismissalHistoryDto
    {
        public PersonnelDismissalRecordDto DayData { get; set; }
        public PersonnelDismissalRecordDto MonthData { get; set; }
        public PersonnelDismissalRecordDto YearData { get; set; }
        public PersonnelDismissalRecordDto TotalData { get; set; }
    }

    //used dismissal record
    public class PersonnelDismissalRecordDto
    {
        public string UsedDismissalTitle { get; set; }
        public Duration UsedDismissalDuration { get; set; }
        public string UsedDismissalDurationTitle
        {
            get
            {
                return UsedDismissalDuration.GetDurationFormatted();
            }
        }
        public string UsedDismissalPercentage { get; set; }
        public string UsedDismissalCountTitle { get; set; }
        public int UsedDismissalCountValue { get; set; }
    }
}
