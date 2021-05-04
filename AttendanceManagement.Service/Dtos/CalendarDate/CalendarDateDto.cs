using System;

namespace AttendanceManagement.Service.Dtos.CalendarDate
{
    public class CalendarDateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public bool IsHoliday { get; set; }
        public string DateType { get; set; }
    }
}
