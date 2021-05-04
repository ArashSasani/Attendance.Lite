using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.HourlyShift;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Localization;
using WebApplication.Infrastructure.Paging;
using WebApplication.Infrastructure.Parser;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class HourlyShiftService : IHourlyShiftService
    {
        private readonly IRepository<HourlyShift> _shiftRepository;

        private readonly IExceptionLogger _logger;

        public HourlyShiftService(IRepository<HourlyShift> shiftRepository
            , IExceptionLogger logger)
        {
            _shiftRepository = shiftRepository;

            _logger = logger;
        }

        public IPaging<HourlyShiftDtoForPaging> Get(string searchTerm, string sortItem
            , string sortOrder, PagingQueryString pagingQueryString)
        {
            IPaging<HourlyShiftDtoForPaging> model = new HourlyShiftDtoPagingList();

            var query = !string.IsNullOrEmpty(searchTerm)
                ? _shiftRepository.Get(q => q.Title.Contains(searchTerm.ToLower()))
                : _shiftRepository.Get();

            //total number of items
            int queryCount = query.Count();
            switch (sortItem)
            {
                case "title":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Title)
                        : query.OrderByDescending(o => o.Title);
                    break;
                default:
                    query = query.OrderByDescending(o => o.Id);
                    break;
            }

            List<HourlyShift> queryResult;
            if (pagingQueryString != null) //with paging
            {
                var pageSetup = new PagingSetup(pagingQueryString.Page, pagingQueryString.PageSize);
                queryResult = query.Skip(pageSetup.Offset).Take(pageSetup.Next).ToList();
                //paging controls
                var controls = pageSetup.GetPagingControls(queryCount, PagingStrategy.ReturnNull);
                if (controls != null)
                {
                    model.PagesCount = controls.PagesCount;
                    model.NextPage = controls.NextPage;
                    model.PrevPage = controls.PrevPage;
                }
            }
            else //without paging
            {
                queryResult = query.ToList();
            }
            model.PagingList = Mapper.Map<List<HourlyShiftDtoForPaging>>(queryResult);

            return model;
        }

        public HourlyShiftDto GetById(int id)
        {
            var hourlyShift = _shiftRepository.GetById(id);
            if (hourlyShift == null)
            {
                return null;
            }
            return Mapper.Map<HourlyShiftDto>(hourlyShift);
        }

        public void Create(CreateHourlyShiftDto dto)
        {
            var hourlyShift = new HourlyShift
            {
                Title = dto.Title,
                HoursShouldWorkInDay = dto.HoursShouldWorkInDay.HasValue
                    ? (int?)dto.HoursShouldWorkInDay.Value.GetHoursInSeconds() : null,
                HoursShouldWorkInWeek = dto.HoursShouldWorkInWeek.HasValue
                    ? (int?)dto.HoursShouldWorkInWeek.Value.GetHoursInSeconds() : null,
                HoursShouldWorkInMonth = dto.HoursShouldWorkInMonth.HasValue
                    ? (int?)dto.HoursShouldWorkInMonth.Value.GetHoursInSeconds() : null
            };

            _shiftRepository.Insert(hourlyShift);
        }

        public void Update(UpdateHourlyShiftDto dto)
        {
            var hourlyShift = _shiftRepository.GetById(dto.Id);
            if (hourlyShift != null)
            {
                hourlyShift.Title = dto.Title;
                hourlyShift.HoursShouldWorkInDay = dto.HoursShouldWorkInDay.HasValue
                    ? (int?)dto.HoursShouldWorkInDay.Value.GetHoursInSeconds() : null;
                hourlyShift.HoursShouldWorkInWeek = dto.HoursShouldWorkInWeek.HasValue
                    ? (int?)dto.HoursShouldWorkInWeek.Value.GetHoursInSeconds() : null;
                hourlyShift.HoursShouldWorkInMonth = dto.HoursShouldWorkInMonth.HasValue
                    ? (int?)dto.HoursShouldWorkInMonth.Value.GetHoursInSeconds() : null;

                _shiftRepository.Update(hourlyShift);
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
                        (ex, "HourlyShift entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public PartialUpdateHourlyShiftDto PrepareForPartialUpdate(int id)
        {
            var hourlyShift = _shiftRepository.GetById(id);
            if (hourlyShift != null)
            {
                return new PartialUpdateHourlyShiftDto
                {
                    PatchDto = Mapper.Map<HourlyShiftPatchDto>(hourlyShift),
                    HourlyShiftEntity = hourlyShift
                };
            }
            else
            {
                return null;
            }
        }

        public void ApplyPartialUpdate(PartialUpdateHourlyShiftDto dto)
        {
            dto.HourlyShiftEntity.Title = dto.PatchDto.Title;

            _shiftRepository.Update(dto.HourlyShiftEntity);
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var hourlyShift = _shiftRepository.GetById(id);
            if (hourlyShift != null)
            {
                _shiftRepository.Delete(hourlyShift, deleteState);
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
                        (ex, "HourlyShift entity with the id: '{0}', is not available." +
                        " delete operation failed.", id);
                    throw;
                }
            }
        }

        public int DeleteAll(string items)
        {
            try
            {
                var idsToRemove = items.ParseToIntArray().ToList();
                idsToRemove.ForEach(i => _shiftRepository.Delete(i, DeleteState.SoftDelete));

                return idsToRemove.Count;
            }
            catch (LogicalException ex)
            {
                _logger.LogRunTimeError(ex, ex.Message);
                throw;
            }
        }
    }
}
