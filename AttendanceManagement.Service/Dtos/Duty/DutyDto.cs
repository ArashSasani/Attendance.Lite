namespace AttendanceManagement.Service.Dtos.Duty
{
    public class DutyDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ActionLimitDays { get; set; }
        public string ActionLimitDaysTitle { get; set; }
        public long RowNumber { get; set; }
    }
}
