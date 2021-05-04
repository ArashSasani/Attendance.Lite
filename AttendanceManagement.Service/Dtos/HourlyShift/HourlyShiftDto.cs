namespace AttendanceManagement.Service.Dtos.HourlyShift
{
    public class HourlyShiftDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? HoursShouldWorkInDay { get; set; }
        public string HoursShouldWorkInDayTitle { get; set; }
        public int? HoursShouldWorkInWeek { get; set; }
        public string HoursShouldWorkInWeekTitle { get; set; }
        public int? HoursShouldWorkInMonth { get; set; }
        public string HoursShouldWorkInMonthTitle { get; set; }
    }
}
