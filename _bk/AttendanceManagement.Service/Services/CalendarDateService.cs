using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.CalendarDate;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class CalendarDateService : ICalendarDateService
    {
        private readonly IRepository<CalendarDate> _calendarDateRepository;

        private readonly IExceptionLogger _logger;

        public CalendarDateService(IRepository<CalendarDate> calendarDateRepository
            , IExceptionLogger logger)
        {
            _calendarDateRepository = calendarDateRepository;

            _logger = logger;
        }

        public List<CalendarDateDto> Get(DateTime fromDate, DateTime toDate)
        {
            var calendarDates = _calendarDateRepository
                .Get(q => q.Date >= fromDate && q.Date <= toDate).ToList();

            return Mapper.Map<List<CalendarDateDto>>(calendarDates);
        }

        public CalendarDateDto GetById(int id)
        {
            var calendarDate = _calendarDateRepository.GetById(id);
            if (calendarDate == null)
            {
                return null;
            }
            return Mapper.Map<CalendarDateDto>(calendarDate);
        }

        public bool IsHoliday(DateTime date)
        {
            return _calendarDateRepository.Get(q => q.Date == date.Date && q.IsHoliday).Any();
        }

        public void Create(CreateCalendarDateDto dto)
        {
            var calendarDate = new CalendarDate
            {
                Title = dto.Title,
                Date = dto.Date.Date,
                IsHoliday = dto.IsHoliday
            };
            _calendarDateRepository.Insert(calendarDate);
        }

        public void Update(UpdateCalendarDateDto dto)
        {
            var calendarDate = _calendarDateRepository.GetById(dto.Id);
            if (calendarDate != null)
            {
                calendarDate.Title = dto.Title;
                calendarDate.IsHoliday = dto.IsHoliday;

                _calendarDateRepository.Update(calendarDate);
            }
            else
            {
                try
                {
                    throw new LogicalException();
                }
                catch (LogicalException ex)
                {
                    _logger.LogLogicalError
                        (ex, "CalendarDate entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var calendarDate = _calendarDateRepository.GetById(id);
            if (calendarDate != null)
            {
                _calendarDateRepository.Delete(calendarDate, deleteState);
            }
            else
            {
                try
                {
                    throw new LogicalException();
                }
                catch (LogicalException ex)
                {
                    _logger.LogLogicalError
                        (ex, "Calendar Date entity with the id: '{0}', is not available." +
                        " delete operation failed.", id);
                    throw;
                }
            }
        }
    }
}
