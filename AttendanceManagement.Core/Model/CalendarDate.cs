using System;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Core.Model
{
    public class CalendarDate : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public bool IsHoliday { get; set; }
        public DeleteState DeleteState { get; set; }
    }
}
