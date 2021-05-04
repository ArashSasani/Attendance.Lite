using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.PersonnelShift;
using AttendanceManagement.Service.Dtos.PersonnelShiftAssignment;
using AttendanceManagement.Service.Dtos.Shift;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using CMS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.Infrastructure.Parser;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class PersonnelShiftService : IPersonnelShiftService
    {
        private readonly IRepository<PersonnelShift> _personnelShiftRepository;
        private readonly IRepository<PersonnelShiftAssignment> _personnelShiftAssignmentRepository;
        private readonly IAuthRepository _authRepository;

        private readonly ICalendarDateService _calendarDateService;

        private readonly IExceptionLogger _logger;

        public PersonnelShiftService(IRepository<PersonnelShift> personnelShiftRepository
            , IRepository<PersonnelShiftAssignment> personnelShiftAssignmentRepository
            , IAuthRepository authRepository, ICalendarDateService calendarDateService
            , IExceptionLogger logger)
        {
            _personnelShiftRepository = personnelShiftRepository;
            _personnelShiftAssignmentRepository = personnelShiftAssignmentRepository;
            _authRepository = authRepository;

            _calendarDateService = calendarDateService;

            _logger = logger;
        }

        public IPaging<PersonnelShiftDtoForPaging> Get(int? shiftId, string searchTerm
            , string sortItem, string sortOrder, PagingQueryString pagingQueryString)
        {
            IPaging<PersonnelShiftDtoForPaging> model = new PersonnelShiftDtoPagingList();

            IQueryable<PersonnelShift> query = null;
            if (shiftId.HasValue)
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelShiftRepository.Get(q => q.ShiftId == shiftId.Value
                    && q.Shift.Title.Contains(searchTerm.ToLower())
                        || q.Personnel.Name.ToLower().Contains(searchTerm.ToLower())
                        || q.Personnel.LastName.ToLower().Contains(searchTerm.ToLower())
                    , includeProperties: "Personnel,Shift,PersonnelShiftAssignments")
                : _personnelShiftRepository.Get(q => q.ShiftId == shiftId.Value
                    , includeProperties: "Personnel,Shift,PersonnelShiftAssignments");
            }
            else
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelShiftRepository.Get(q => q.Shift.Title.Contains(searchTerm.ToLower())
                        || q.Personnel.Name.ToLower().Contains(searchTerm.ToLower())
                        || q.Personnel.LastName.ToLower().Contains(searchTerm.ToLower())
                    , includeProperties: "Personnel,Shift,PersonnelShiftAssignments")
                : _personnelShiftRepository.Get(includeProperties: "Personnel,Shift,PersonnelShiftAssignments");
            }

            //total number of items
            int queryCount = query.Count();
            switch (sortItem)
            {
                case "shift_title":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Shift.Title)
                        : query.OrderByDescending(o => o.Shift.Title);
                    break;
                case "personnel_name":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Personnel.Name)
                        : query.OrderByDescending(o => o.Personnel.Name);
                    break;
                case "personnel_last_name":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Personnel.LastName)
                        : query.OrderByDescending(o => o.Personnel.LastName);
                    break;
                default:
                    query = query.OrderByDescending(o => o.Id);
                    break;
            }

            List<PersonnelShift> queryResult;
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
            model.PagingList = Mapper.Map<List<PersonnelShiftDtoForPaging>>(queryResult);

            return model;
        }

        public PersonnelShiftDto GetById(int id)
        {
            var personnelShift = _personnelShiftRepository.Get(q => q.Id == id
                , includeProperties: "Personnel, Shift").SingleOrDefault();
            if (personnelShift == null)
            {
                return new PersonnelShiftDto();
            }
            var dto = Mapper.Map<PersonnelShiftDto>(personnelShift);
            //calendar display data
            var assignments = _personnelShiftAssignmentRepository
                .Get(q => q.PersonnelShiftId == personnelShift.Id
                , includeProperties: "PersonnelShift").OrderBy(q => q.Date).ToList();
            dto.ShiftAssignments =
                Mapper.Map<List<PersonnelShiftAssignmentDisplayDto>>(assignments);

            return dto;
        }

        public bool IsAssignmentAvailable(int shiftId, DateTime date)
        {
            return _personnelShiftRepository
                     .Get(q => q.ShiftId == shiftId, includeProperties: "PersonnelShiftAssignments")
                         .SelectMany(x => x.PersonnelShiftAssignments).AsEnumerable()
                         .Any(q => q.Date.Date == date.Date);
        }

        public async Task<List<ShiftDDLDto>> GetPersonnelShifts(string username)
        {
            var user = await _authRepository.FindUserByUsernameAsync(username);
            if (user != null)
            {
                var shifts = _personnelShiftRepository
                    .Get(q => q.Personnel.Code == user.UserName, includeProperties: "Personnel,Shift")
                    .Select(x => x.Shift).Distinct().ToList();
                return Mapper.Map<List<ShiftDDLDto>>(shifts);
            }
            try
            {
                throw new LogicalException();
            }
            catch (LogicalException ex)
            {
                _logger.LogLogicalError(ex, "corresponding personnel user with username: {0}" +
                    "does not exist!", username);
                throw;
            }
        }

        public List<ShiftDDLDto> GetReplacedPersonnelShifts(int personnelId)
        {
            var shifts = _personnelShiftRepository
                .Get(q => q.PersonnelId == personnelId, includeProperties: "Shift")
                .Select(x => x.Shift).Distinct().ToList();
            return Mapper.Map<List<ShiftDDLDto>>(shifts);
        }

        public CustomResult CreateByPattern(CreatePersonnelShiftByPatternDto dto)
        {
            if (dto.DaysOfWeek.Count == 0 && !dto.RepeatPattern.HasValue)
            {
                return new CustomResult
                {
                    Message = "please choose repeat pattern or days of week"
                };
            }
            if (dto.PersonnelIdList.Count == 0)
            {
                return new CustomResult
                {
                    Message = "please choose the personnel"
                };
            }
            foreach (var personnelId in dto.PersonnelIdList)
            {
                var shiftAssignments = new List<PersonnelShiftAssignment>();

                //build assignments
                var fromDate = dto.FromDate;
                var toDate = dto.ToDate;

                if (dto.RepeatPattern.HasValue) //check repeat patterns
                {
                    switch (dto.RepeatPattern)
                    {
                        case RepeatPattern.EveryDay:
                            shiftAssignments = BuildPattern(fromDate, toDate, dto.IncludeFridays
                                , dto.IncludeHolidays, 1);
                            break;
                        case RepeatPattern.OneDayAfter:
                            shiftAssignments = BuildPattern(fromDate, toDate, dto.IncludeFridays
                                , dto.IncludeHolidays, 2);
                            break;
                        case RepeatPattern.TowDaysAfter:
                            shiftAssignments = BuildPattern(fromDate, toDate, dto.IncludeFridays
                                , dto.IncludeHolidays, 3);
                            break;
                        case RepeatPattern.WeekAfter:
                            shiftAssignments = BuildPattern(fromDate, toDate, dto.IncludeFridays
                                , dto.IncludeHolidays, 7);
                            break;
                        case RepeatPattern.TwoWeeksAfter:
                            shiftAssignments = BuildPattern(fromDate, toDate, dto.IncludeFridays
                                , dto.IncludeHolidays, 14);
                            break;
                        default:
                            break;
                    }
                }
                else if (dto.DaysOfWeek.Count > 0) //check day of week
                {
                    for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
                    {
                        if (dto.DaysOfWeek.Contains(date.DayOfWeek))
                        {
                            if (!dto.IncludeFridays && date.DayOfWeek == DayOfWeek.Friday)
                            {
                                continue;
                            }
                            if (!dto.IncludeHolidays && _calendarDateService.IsHoliday(date))
                            {
                                continue;
                            }
                            shiftAssignments.Add(new PersonnelShiftAssignment
                            {
                                Date = date.Date
                            });
                        }
                    }

                }
                //check for same exists
                if (AnotherShiftForSameDayExists(personnelId, shiftAssignments))
                {
                    return new CustomResult
                    {
                        Message = "cannot add another shift for the same day"
                    };
                }
                if (shiftAssignments.Count > 0)
                {
                    //save personnel shift
                    var personnelShift = new PersonnelShift
                    {
                        PersonnelId = personnelId,
                        ShiftId = dto.ShiftId,
                        DateAssigned = DateTime.Now
                    };
                    _personnelShiftRepository.Insert(personnelShift);

                    //save shift assignments
                    foreach (var personnelShiftAssignment in shiftAssignments)
                    {
                        personnelShiftAssignment.PersonnelShiftId = personnelShift.Id;
                        _personnelShiftAssignmentRepository.Insert(personnelShiftAssignment);
                    }
                }
                else
                {
                    return new CustomResult
                    {
                        Message = "according to your filtering choices, could not assign shifts"
                    };
                }
            }
            return new CustomResult
            {
                IsValid = true
            };
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var personnelShift = _personnelShiftRepository.GetById(id);
            if (personnelShift != null)
            {
                _personnelShiftRepository.Delete(personnelShift, deleteState);
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
                        (ex, "PersonnelShift entity with the id: '{0}', is not available." +
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
                idsToRemove.ForEach(i => _personnelShiftRepository.Delete(i, DeleteState.SoftDelete));

                return idsToRemove.Count;
            }
            catch (LogicalException ex)
            {
                _logger.LogRunTimeError(ex, ex.Message);
                throw;
            }
        }

        #region Helpers
        private bool AnotherShiftForSameDayExists(int personnelId
            , List<PersonnelShiftAssignment> shiftAssignments)
        {
            bool exists = false;
            foreach (var assignment in shiftAssignments)
            {
                exists = _personnelShiftAssignmentRepository
                    .Get(q => q.PersonnelShift.PersonnelId == personnelId
                        && q.PersonnelShift.DeleteState != DeleteState.SoftDelete
                        && q.Date == assignment.Date
                        , includeProperties: "PersonnelShift").Any();
            }
            return exists;
        }
        private List<PersonnelShiftAssignment> BuildPattern(DateTime fromDate, DateTime toDate
            , bool includeFridays, bool includeHolidays, int numberOfDaysShouldAdd)
        {
            var assignments = new List<PersonnelShiftAssignment>();
            while (fromDate < toDate)
            {
                if (!includeFridays && fromDate.DayOfWeek == DayOfWeek.Friday)
                {
                    fromDate = fromDate.AddDays(numberOfDaysShouldAdd);
                    continue;
                }
                if (!includeHolidays && _calendarDateService.IsHoliday(fromDate))
                {
                    fromDate = fromDate.AddDays(numberOfDaysShouldAdd);
                    continue;
                }
                assignments.Add(new PersonnelShiftAssignment
                {
                    Date = fromDate.Date
                });
                fromDate = fromDate.AddDays(numberOfDaysShouldAdd);
            }
            return assignments;
        }
        #endregion
    }
}
