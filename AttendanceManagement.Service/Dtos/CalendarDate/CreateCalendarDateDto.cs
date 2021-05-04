using System;

namespace AttendanceManagement.Service.Dtos.CalendarDate
{
    public class CreateCalendarDateDto
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public bool IsHoliday { get; set; }
    }
}
