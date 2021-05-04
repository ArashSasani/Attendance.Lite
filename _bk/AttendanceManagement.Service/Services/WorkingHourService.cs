using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.WorkingHour;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Localization;
using WebApplication.Infrastructure.Paging;
using WebApplication.Infrastructure.Parser;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class WorkingHourService : IWorkingHourService
    {
        private readonly IRepository<WorkingHour> _workingHourRepository;

        private readonly IExceptionLogger _logger;

        public WorkingHourService(IRepository<WorkingHour> workingHourRepository
            , IExceptionLogger logger)
        {
            _workingHourRepository = workingHourRepository;

            _logger = logger;
        }

        public IPaging<WorkingHourDtoForPaging> Get(int? shiftId, string searchTerm
            , string sortItem, string sortOrder, PagingQueryString pagingQueryString)
        {
            IPaging<WorkingHourDtoForPaging> model = new WorkingHourDtoPagingList();

            IQueryable<WorkingHour> query = null;
            if (shiftId.HasValue)
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _workingHourRepository.Get(q => q.ShiftId == shiftId.Value
                    && q.Title.Contains(searchTerm.ToLower())
                    , includeProperties: "Shift")
                : _workingHourRepository.Get(q => q.ShiftId == shiftId.Value
                    , includeProperties: "Shift");
            }
            else
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _workingHourRepository.Get(q => q.Title.Contains(searchTerm.ToLower())
                    , includeProperties: "Shift")
                : _workingHourRepository.Get(includeProperties: "Shift");
            }


            //total number of items
            int queryCount = query.Count();
            switch (sortItem)
            {
                case "title":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Title)
                        : query.OrderByDescending(o => o.Title);
                    break;
                case "shift_title":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Shift.Title)
                        : query.OrderByDescending(o => o.Shift.Title);
                    break;
                default:
                    query = query.OrderByDescending(o => o.Id);
                    break;
            }

            List<WorkingHour> queryResult;
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
            model.PagingList = Mapper.Map<List<WorkingHourDtoForPaging>>(queryResult);

            return model;
        }

        public WorkingHourDto GetById(int id)
        {
            var workingHour = _workingHourRepository
                .Get(q => q.Id == id, includeProperties: "Shift").SingleOrDefault();
            if (workingHour == null)
            {
                return null;
            }
            return Mapper.Map<WorkingHourDto>(workingHour);
        }

        public List<WorkingHourDDLDto> GetForDDL(int shiftId)
        {
            var workingHours = _workingHourRepository.Get(q => q.ShiftId == shiftId).ToList();
            return Mapper.Map<List<WorkingHourDDLDto>>(workingHours);
        }

        public CustomResult Create(CreateWorkingHourDto dto)
        {
            var result = CheckFilters(dto.FromTime, dto.ToTime, dto.MealTimeBreakFromTime
                , dto.MealTimeBreakToTime, dto.Break1FromTime, dto.Break1ToTime
                , dto.Break2FromTime, dto.Break2ToTime, dto.Break3FromTime, dto.Break3ToTime
                , dto.WorkingHourDuration);
            if (!result.IsValid)
            {
                return result;
            }

            var workingHour = new WorkingHour
            {
                ShiftId = dto.ShiftId,
                Title = dto.Title,
                FromTime = dto.FromTime,
                ToTime = dto.ToTime,
                WorkingHourDuration = dto.WorkingHourDuration,
                DailyDelay = dto.DailyDelay.GetSecondsFromDuration(),
                MonthlyDelay = dto.MonthlyDelay.GetSecondsFromDuration(),
                DailyRush = dto.DailyRush.GetSecondsFromDuration(),
                MonthlyRush = dto.MonthlyRush.GetSecondsFromDuration(),
                PriorExtraWorkTime = dto.PriorExtraWorkTime.GetSecondsFromDuration(),
                LaterExtraWorkTime = dto.LaterExtraWorkTime.GetSecondsFromDuration(),
                FloatingTime = dto.FloatingTime.GetSecondsFromDuration(),
                MealTimeBreakFromTime = dto.MealTimeBreakFromTime,
                MealTimeBreakToTime = dto.MealTimeBreakToTime,
                Break1FromTime = dto.Break1FromTime,
                Break1ToTime = dto.Break1ToTime,
                Break2FromTime = dto.Break2FromTime,
                Break2ToTime = dto.Break2ToTime,
                Break3FromTime = dto.Break3FromTime,
                Break3ToTime = dto.Break3ToTime
            };

            _workingHourRepository.Insert(workingHour);

            return new CustomResult
            {
                IsValid = true
            };
        }

        public CustomResult Update(UpdateWorkingHourDto dto)
        {
            var workingHour = _workingHourRepository.GetById(dto.Id);
            if (workingHour != null)
            {
                var result = CheckFilters(dto.FromTime, dto.ToTime, dto.MealTimeBreakFromTime
                    , dto.MealTimeBreakToTime, dto.Break1FromTime, dto.Break1ToTime
                    , dto.Break2FromTime, dto.Break2ToTime, dto.Break3FromTime, dto.Break3ToTime
                    , dto.WorkingHourDuration);
                if (!result.IsValid)
                {
                    return result;
                }

                workingHour.ShiftId = dto.ShiftId;
                workingHour.Title = dto.Title;
                workingHour.FromTime = dto.FromTime;
                workingHour.ToTime = dto.ToTime;
                workingHour.WorkingHourDuration = dto.WorkingHourDuration;
                workingHour.DailyDelay = dto.DailyDelay.GetSecondsFromDuration();
                workingHour.MonthlyDelay = dto.MonthlyDelay.GetSecondsFromDuration();
                workingHour.DailyRush = dto.DailyRush.GetSecondsFromDuration();
                workingHour.MonthlyRush = dto.MonthlyRush.GetSecondsFromDuration();
                workingHour.PriorExtraWorkTime = dto.PriorExtraWorkTime.GetSecondsFromDuration();
                workingHour.LaterExtraWorkTime = dto.LaterExtraWorkTime.GetSecondsFromDuration();
                workingHour.FloatingTime = dto.FloatingTime.GetSecondsFromDuration();
                workingHour.MealTimeBreakFromTime = dto.MealTimeBreakFromTime;
                workingHour.MealTimeBreakToTime = dto.MealTimeBreakToTime;
                workingHour.Break1FromTime = dto.Break1FromTime;
                workingHour.Break1ToTime = dto.Break1ToTime;
                workingHour.Break2FromTime = dto.Break2FromTime;
                workingHour.Break2ToTime = dto.Break2ToTime;
                workingHour.Break3FromTime = dto.Break3FromTime;
                workingHour.Break3ToTime = dto.Break3ToTime;

                _workingHourRepository.Update(workingHour);

                return new CustomResult
                {
                    IsValid = true
                };
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
                        (ex, "WorkingHour entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public PartialUpdateWorkingHourDto PrepareForPartialUpdate(int id)
        {
            var workingHour = _workingHourRepository.GetById(id);
            if (workingHour != null)
            {
                return new PartialUpdateWorkingHourDto
                {
                    PatchDto = Mapper.Map<WorkingHourPatchDto>(workingHour),
                    WorkingHourEntity = workingHour
                };
            }
            else
            {
                return null;
            }
        }

        public void ApplyPartialUpdate(PartialUpdateWorkingHourDto dto)
        {
            dto.WorkingHourEntity.Title = dto.PatchDto.Title;

            _workingHourRepository.Update(dto.WorkingHourEntity);
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var workingHour = _workingHourRepository.GetById(id);
            if (workingHour != null)
            {
                _workingHourRepository.Delete(workingHour, deleteState);
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
                        (ex, "WorkingHour entity with the id: '{0}', is not available." +
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
                idsToRemove.ForEach(i => _workingHourRepository.Delete(i, DeleteState.SoftDelete));

                return idsToRemove.Count;
            }
            catch (LogicalException ex)
            {
                _logger.LogRunTimeError(ex, ex.Message);
                throw;
            }
        }

        #region Helpers
        private CustomResult CheckFilters(TimeSpan fromTime, TimeSpan toTime
            , TimeSpan? mealTimeFromTime, TimeSpan? mealTimeToTime
            , TimeSpan? break1FromTime, TimeSpan? break1ToTime
            , TimeSpan? break2FromTime, TimeSpan? break2ToTime
            , TimeSpan? break3FromTime, TimeSpan? break3ToTime
            , WorkingHourDuration workingHourDuration)
        {
            var result = new CustomResult
            {
                IsValid = true
            };

            //toTime should be greater that fromTime if the duration
            //is the current date
            if (workingHourDuration == WorkingHourDuration.OneDay)
            {
                if (fromTime >= toTime)
                {
                    return new CustomResult
                    {
                        Message = "end date cannot be less than start date in the same day"
                    };
                }
            }
            //breaks should be included in between toTime and fromTime
            switch (workingHourDuration)
            {
                case WorkingHourDuration.OneDay:
                    if (mealTimeFromTime < fromTime)
                    {
                        return new CustomResult
                        {
                            Message = "start meal break time cannot be less than star time"
                        };
                    }
                    if (mealTimeToTime > toTime)
                    {
                        return new CustomResult
                        {
                            Message = "end of meal break time cannot be greatetr than end time"
                        };
                    }
                    if (break1FromTime < fromTime)
                    {
                        return new CustomResult
                        {
                            Message = "start first meal break time cannot be less than star time"
                        };
                    }
                    if (break1ToTime > toTime)
                    {
                        return new CustomResult
                        {
                            Message = "end of first meal break time cannot be greater than end time"
                        };
                    }
                    if (break2FromTime < fromTime)
                    {
                        return new CustomResult
                        {
                            Message = "start second meal break time cannot be less than star time"
                        };
                    }
                    if (break2ToTime > toTime)
                    {
                        return new CustomResult
                        {
                            Message = "end of second meal break time cannot be greater than end time"
                        };
                    }
                    if (break3FromTime < fromTime)
                    {
                        return new CustomResult
                        {
                            Message = "start third meal break time cannot be less than star time"
                        };
                    }
                    if (break3ToTime > toTime)
                    {
                        return new CustomResult
                        {
                            Message = "end of third meal break time cannot be greater than end time"
                        };
                    }
                    break;
            }

            return result;
        }
        #endregion
    }
}
