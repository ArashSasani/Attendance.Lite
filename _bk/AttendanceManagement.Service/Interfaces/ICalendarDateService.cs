using AttendanceManagement.Service.Dtos.CalendarDate;
using System;
using System.Collections.Generic;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Interfaces
{
    public interface ICalendarDateService
    {
        List<CalendarDateDto> Get(DateTime fromDate, DateTime toDate);
        CalendarDateDto GetById(int id);
        bool IsHoliday(DateTime date);
        void Create(CreateCalendarDateDto dto);
        void Update(UpdateCalendarDateDto dto);
        void Delete(int id, DeleteState deleteState);
    }
}
