using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.PersonnelDismissal;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using CMS.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Enums;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Localization;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class PersonnelDismissalService : IPersonnelDismissalService
    {
        private readonly IRepository<CalendarDate> _calendarDatesRepository;
        private readonly IRepository<Personnel> _personnelRepository;
        private readonly IRepository<PersonnelDismissal> _personnelDismissalRepository;
        private readonly IRepository<PersonnelDailyDismissal> _personnelDailyDismissalRepository;
        private readonly IRepository<PersonnelHourlyDismissal> _personnelHourlyDismissalRepository;

        private readonly IAuthService _authService;
        private readonly IDismissalService _dismissalService;
        private readonly IApprovalProcService _approvalProcService;
        private readonly IMessageService _messageService;
        private readonly IRequestMessageHandlerService _requestMessageHandlerService;

        private readonly IExceptionLogger _logger;

        public PersonnelDismissalService(IRepository<CalendarDate> calendarDatesRepository
            , IRepository<Personnel> personnelRepository
            , IRepository<PersonnelDismissal> personnelDismissalRepository
            , IRepository<PersonnelDailyDismissal> personnelDailyDismissal
            , IRepository<PersonnelHourlyDismissal> personnelHourlyDismissal
            , IAuthService authService
            , IDismissalService dismissalService
            , IApprovalProcService approvalProcService, IMessageService messageService
            , IRequestMessageHandlerService requestMessageHandlerService
            , IExceptionLogger logger)
        {
            _calendarDatesRepository = calendarDatesRepository;
            _personnelRepository = personnelRepository;
            _personnelDismissalRepository = personnelDismissalRepository;
            _personnelDailyDismissalRepository = personnelDailyDismissal;
            _personnelHourlyDismissalRepository = personnelHourlyDismissal;

            _authService = authService;
            _dismissalService = dismissalService;
            _approvalProcService = approvalProcService;
            _messageService = messageService;
            _requestMessageHandlerService = requestMessageHandlerService;

            _logger = logger;
        }


        public IPaging<PersonnelDismissalDtoForPaging> Get(string username
            , int? dismissalType, string fromDate, string toDate
            , string searchTerm, string sortItem, string sortOrder
            , PagingQueryString pagingQueryString)
        {
            IPaging<PersonnelDismissalDtoForPaging> model = new PersonnelDismissalDtoPagingList();

            IQueryable<PersonnelDismissal> query = null;

            var personnel = _personnelRepository.Get(q => q.Code == username).SingleOrDefault();
            if (personnel != null)
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelDismissalRepository.Get(q => q.PersonnelId == personnel.Id
                    && (q.Personnel.Name.Contains(searchTerm.ToLower())
                    || q.Personnel.LastName.Contains(searchTerm.ToLower())
                    || q.Dismissal.Title.Contains(searchTerm.ToLower()))
                    , includeProperties: "Personnel,Dismissal")
                : _personnelDismissalRepository.Get(q => q.PersonnelId == personnel.Id
                    , includeProperties: "Personnel,Dismissal");
            }
            else
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelDismissalRepository.Get(q => q.Personnel.Name.Contains(searchTerm.ToLower())
                    || q.Personnel.LastName.Contains(searchTerm.ToLower())
                    || q.Dismissal.Title.Contains(searchTerm.ToLower())
                    , includeProperties: "Personnel,Dismissal")
                : _personnelDismissalRepository.Get(includeProperties: "Personnel,Dismissal");
            }

            if (dismissalType.HasValue)
            {
                query = query.Where(q => q.DismissalId == dismissalType.Value);
            }

            if (!string.IsNullOrEmpty(fromDate))
            {
                var fromDateParsed = DateTime.Parse(fromDate);
                query = query.Where(q => q.SubmittedDate >= fromDateParsed);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                var toDateParsed = DateTime.Parse(toDate);
                query = query.Where(q => q.SubmittedDate <= toDateParsed);
            }

            //total number of items
            int queryCount = query.Count();
            switch (sortItem)
            {
                case "personnel_name":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Personnel.Name)
                        : query.OrderByDescending(o => o.Personnel.Name);
                    break;
                case "personnel_last_name":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Personnel.LastName)
                        : query.OrderByDescending(o => o.Personnel.LastName);
                    break;
                case "dismissal_title":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Dismissal.Title)
                        : query.OrderByDescending(o => o.Dismissal.Title);
                    break;
                default:
                    query = query.OrderByDescending(o => o.Id);
                    break;
            }

            List<PersonnelDismissal> queryResult;
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
            model.PagingList = Mapper.Map<List<PersonnelDismissalDtoForPaging>>(queryResult);


            return model;
        }

        public int GetNumberOfRequests(DateTime date)
        {
            var currentDate = date.Date;
            var dailyRequests = _personnelDailyDismissalRepository
                .Get(q => q.FromDate <= currentDate && currentDate <= q.ToDate).Count();
            var hourlyRequests = _personnelHourlyDismissalRepository
                .Get(q => q.Date == currentDate).Count();
            return dailyRequests + hourlyRequests;
        }

        public PersonnelDismissalDto GetById(int id)
        {
            var personnelDismissal = _personnelDismissalRepository
                .Get(q => q.Id == id, includeProperties: "Personnel,Dismissal")
                .SingleOrDefault();
            if (personnelDismissal == null)
            {
                return null;
            }
            switch (personnelDismissal.DismissalDuration)
            {
                case RequestDuration.Daily:
                    var daily = personnelDismissal as PersonnelDailyDismissal;
                    return Mapper.Map<PersonnelDismissalDto>(daily);
                case RequestDuration.Hourly:
                    var hourly = personnelDismissal as PersonnelHourlyDismissal;
                    return Mapper.Map<PersonnelDismissalDto>(hourly);
                default:
                    break;
            }
            return Mapper.Map<PersonnelDismissalDto>(personnelDismissal);
        }

        public async Task<PersonnelDismissalHistoryDto> GetChartInfo(string username, int dismissalId)
        {
            var user = await _authService.FindUserByUsernameAsync(username);
            if (user != null)
            {
                var personnel = _personnelRepository.Get(q => q.Code == user.Username)
                    .SingleOrDefault();
                if (personnel != null)
                    return GetPersonnelDismissalHistory(personnel.Id, dismissalId);
            }
            return new PersonnelDismissalHistoryDto();
        }

        public async Task<CustomResult<string>> Create(CreatePersonnelDismissalDto dto
            , string username)
        {
            var user = await _authService.FindUserByUsernameAsync(username);
            if (user != null)
            {
                var personnel = _personnelRepository.Get(q => q.Code == user.Username)
                    .SingleOrDefault();
                if (personnel == null)
                {
                    return new CustomResult<string>
                    {
                        Message = "person's info is not available"
                    };
                }

                if (AlreadyRequested(personnel.Id, dto.DailyDismissal, dto.HourlyDismissal))
                {
                    return new CustomResult<string>
                    {
                        Message = "cannot add multiple dismissals for the same time period"
                    };
                }

                #region Get Selected Dismissal's Settings
                var selectedDismissal = _dismissalService.GetById(dto.DismissalId);
                var dismissalSettings = _dismissalService.GetAllowances(selectedDismissal.Id);
                #endregion

                //history
                var usedDismissalHistory = GetPersonnelDismissalHistory(personnel.Id, dto.DismissalId);
                //new request
                double requestDurationInSeconds = 0;
                switch (dto.DismissalDuration)
                {
                    case RequestDuration.Daily:
                        requestDurationInSeconds = (dto.DailyDismissal.ToDate
                            - dto.DailyDismissal.FromDate)
                            .Days.GetDaysInSeconds();
                        break;
                    case RequestDuration.Hourly:
                        requestDurationInSeconds = (dto.HourlyDismissal.ToTime - dto.HourlyDismissal.FromTime)
                            .TotalSeconds;
                        break;
                }

                #region Excessive Check Filters
                var filterResult = new CustomResult<string>();
                //limit
                filterResult = CheckLimitFilters(usedDismissalHistory.TotalData, requestDurationInSeconds
                    , dismissalSettings.AllowanceInTotal, "total", selectedDismissal.DismissalExcessiveReaction, selectedDismissal.Title);
                if (!filterResult.IsValid)
                    return filterResult;
                filterResult = CheckLimitFilters(usedDismissalHistory.YearData, requestDurationInSeconds
                   , dismissalSettings.AllowanceInYear, "year(s)", selectedDismissal.DismissalExcessiveReaction, selectedDismissal.Title);
                if (!filterResult.IsValid)
                    return filterResult;
                filterResult = CheckLimitFilters(usedDismissalHistory.MonthData, requestDurationInSeconds
                    , dismissalSettings.AllowanceInMonth, "month(s)", selectedDismissal.DismissalExcessiveReaction, selectedDismissal.Title);
                if (!filterResult.IsValid)
                    return filterResult;
                filterResult = CheckLimitFilters(usedDismissalHistory.DayData, requestDurationInSeconds
                    , dismissalSettings.AllowanceInDay, "day(s)", selectedDismissal.DismissalExcessiveReaction, selectedDismissal.Title);
                if (!filterResult.IsValid)
                    return filterResult;
                //count
                filterResult = CheckCountFilters(usedDismissalHistory.TotalData, dismissalSettings.CountInTotal, "total"
                    , selectedDismissal.DismissalExcessiveReaction, selectedDismissal.Title);
                if (!filterResult.IsValid)
                    return filterResult;
                filterResult = CheckCountFilters(usedDismissalHistory.YearData, dismissalSettings.CountInYear, "year(s)"
                    , selectedDismissal.DismissalExcessiveReaction, selectedDismissal.Title);
                if (!filterResult.IsValid)
                    return filterResult;
                filterResult = CheckCountFilters(usedDismissalHistory.MonthData, dismissalSettings.CountInMonth, "month(s)"
                    , selectedDismissal.DismissalExcessiveReaction, selectedDismissal.Title);
                if (!filterResult.IsValid)
                    return filterResult;
                filterResult = CheckCountFilters(usedDismissalHistory.DayData, dismissalSettings.CountInDay, "day(s)"
                    , selectedDismissal.DismissalExcessiveReaction, selectedDismissal.Title);
                if (!filterResult.IsValid)
                    return filterResult;
                #endregion

                #region Create Operation
                int personnelDismissalId = 0;
                DateTime from = DateTime.Now;
                DateTime to = DateTime.Now;
                switch (dto.DismissalDuration)
                {
                    case RequestDuration.Daily:
                        if (dto.DailyDismissal != null)
                        {
                            var fromDate = dto.DailyDismissal.FromDate;
                            var toDate = dto.DailyDismissal.ToDate;

                            if (_calendarDatesRepository.Get(q => q.Date == fromDate).Any())
                            {
                                return new CustomResult<string>
                                {
                                    Message = "cannot add dismissal in holidays"
                                };
                            }

                            var dailyDismissal = new PersonnelDailyDismissal
                            {
                                PersonnelId = personnel.Id,
                                DismissalId = dto.DismissalId,
                                SubmittedDate = DateTime.Now,
                                DismissalDuration = dto.DismissalDuration,
                                RequestDescription = dto.RequestDescription,
                                FromDate = fromDate,
                                ToDate = toDate
                            };
                            _personnelDailyDismissalRepository.Insert(dailyDismissal);
                            personnelDismissalId = dailyDismissal.Id;
                            from = dailyDismissal.FromDate;
                            to = dailyDismissal.ToDate;
                        }
                        else
                        {
                            PersonnelDismissalDetailsError(dto.DismissalDuration, "create");
                        }
                        break;
                    case RequestDuration.Hourly:
                        if (dto.HourlyDismissal != null)
                        {
                            var date = dto.HourlyDismissal.Date;

                            var hourlyDismissal = new PersonnelHourlyDismissal
                            {
                                PersonnelId = personnel.Id,
                                DismissalId = dto.DismissalId,
                                SubmittedDate = DateTime.Now,
                                DismissalDuration = dto.DismissalDuration,
                                RequestDescription = dto.RequestDescription,
                                Date = date,
                                FromTime = dto.HourlyDismissal.FromTime,
                                ToTime = dto.HourlyDismissal.ToTime
                            };
                            _personnelHourlyDismissalRepository.Insert(hourlyDismissal);
                            personnelDismissalId = hourlyDismissal.Id;
                            from = hourlyDismissal.Date.Add(hourlyDismissal.FromTime);
                            to = hourlyDismissal.Date.Add(hourlyDismissal.ToTime);
                        }
                        else
                        {
                            PersonnelDismissalDetailsError(dto.DismissalDuration, "create");
                        }
                        break;
                    default:
                        break;
                }
                #endregion

                //Send Request Message to Approval Proc
                var notificationReceiverId = await _requestMessageHandlerService
                    .HandleDismisalRequest(personnelDismissalId, dto.DismissalDuration, from, to
                        , user.Username);

                if (notificationReceiverId == null)
                {
                    return new CustomResult<string>
                    {
                        Message = "approval procedure not found"
                    };
                }

                return new CustomResult<string>
                {
                    Message = !string.IsNullOrEmpty(filterResult.Message)
                        ? filterResult.Message : null,
                    IsValid = true,
                    ReturnId = notificationReceiverId
                };
            }
            return new CustomResult<string>
            {
                Message = "user not found"
            };
        }

        public async Task<CustomResult<string>> Update(UpdatePersonnelDismissalDto dto)
        {
            var personnelDismissal = _personnelDismissalRepository.GetById(dto.Id);
            if (personnelDismissal != null)
            {
                dto.PersonnelId = personnelDismissal.PersonnelId;
                var personnel = _personnelRepository.GetById(dto.PersonnelId);
                if (personnel == null)
                {
                    return new CustomResult<string>
                    {
                        Message = "person's info is not available"
                    };
                }

                var user = await _authService.FindUserByUsernameAsync(personnel.Code);
                if (user == null)
                {
                    return new CustomResult<string>
                    {
                        Message = "user for the person is not available"
                    };
                }

                if (AlreadyRequested(personnel.Id, dto.DailyDismissal, dto.HourlyDismissal
                    , personnelDismissal.Id))
                {
                    return new CustomResult<string>
                    {
                        Message = "cannot add multiple dismissals for the same time period"
                    };
                }

                if (personnelDismissal.RequestAction == RequestAction.Unknown)
                {
                    #region Get Selected Dismissal's Settings
                    var selectedDismissal = _dismissalService.GetById(dto.DismissalId);
                    var dismissalSettings = _dismissalService.GetAllowances(selectedDismissal.Id);
                    #endregion

                    //history
                    var usedDismissalHistory = GetPersonnelDismissalHistory(dto.PersonnelId, dto.DismissalId);
                    //new request
                    double requestDurationInSeconds = 0;
                    int selectedDurationInSeconds = 0;
                    switch (dto.DismissalDuration)
                    {
                        case RequestDuration.Daily:
                            requestDurationInSeconds = (dto.DailyDismissal.ToDate
                                - dto.DailyDismissal.FromDate)
                                .Days.GetDaysInSeconds();
                            //we should consider the duration between the already available 
                            //personnel dismissal and the update request
                            var selectedDailyDismissal = personnelDismissal as PersonnelDailyDismissal;
                            selectedDurationInSeconds = selectedDailyDismissal != null
                                ? (selectedDailyDismissal.ToDate - selectedDailyDismissal.FromDate)
                                    .Days.GetDaysInSeconds()
                                : 0;
                            requestDurationInSeconds = requestDurationInSeconds - selectedDurationInSeconds;
                            break;
                        case RequestDuration.Hourly:
                            requestDurationInSeconds = (dto.HourlyDismissal.ToTime - dto.HourlyDismissal.FromTime)
                                .TotalSeconds;
                            var selectedHourlyDismissal = personnelDismissal as PersonnelHourlyDismissal;
                            selectedDurationInSeconds = selectedHourlyDismissal != null
                                ? (selectedHourlyDismissal.ToTime - selectedHourlyDismissal.FromTime)
                                    .Hours.GetHoursInSeconds()
                                : 0;
                            requestDurationInSeconds = requestDurationInSeconds - selectedDurationInSeconds;
                            break;
                    }

                    #region Excessive Check Filters
                    var filterResult = new CustomResult<string>();
                    //limit
                    filterResult = CheckLimitFilters(usedDismissalHistory.TotalData, requestDurationInSeconds
                        , dismissalSettings.AllowanceInTotal, "total", selectedDismissal.DismissalExcessiveReaction, selectedDismissal.Title);
                    if (!filterResult.IsValid)
                        return filterResult;
                    filterResult = CheckLimitFilters(usedDismissalHistory.YearData, requestDurationInSeconds
                       , dismissalSettings.AllowanceInYear, "year(s)", selectedDismissal.DismissalExcessiveReaction, selectedDismissal.Title);
                    if (!filterResult.IsValid)
                        return filterResult;
                    filterResult = CheckLimitFilters(usedDismissalHistory.MonthData, requestDurationInSeconds
                        , dismissalSettings.AllowanceInMonth, "month(s)", selectedDismissal.DismissalExcessiveReaction, selectedDismissal.Title);
                    if (!filterResult.IsValid)
                        return filterResult;
                    filterResult = CheckLimitFilters(usedDismissalHistory.DayData, requestDurationInSeconds
                        , dismissalSettings.AllowanceInDay, "day(s)", selectedDismissal.DismissalExcessiveReaction, selectedDismissal.Title);
                    if (!filterResult.IsValid)
                        return filterResult;
                    #endregion

                    #region Update Operation
                    personnelDismissal.PersonnelId = personnelDismissal.PersonnelId;

                    DateTime from = DateTime.Now;
                    DateTime to = DateTime.Now;
                    switch (dto.DismissalDuration)
                    {
                        case RequestDuration.Daily:

                            var fromDate = dto.DailyDismissal.FromDate;
                            var toDate = dto.DailyDismissal.ToDate;

                            if (_calendarDatesRepository.Get(q => q.Date == fromDate).Any())
                            {
                                return new CustomResult<string>
                                {
                                    Message = "cannot add dismissal in holidays"
                                };
                            }

                            if (personnelDismissal.DismissalDuration == dto.DismissalDuration) //update the same type
                            {
                                var dailyDismissal = personnelDismissal as PersonnelDailyDismissal;
                                dailyDismissal.PersonnelId = personnelDismissal.PersonnelId;
                                dailyDismissal.DismissalId = dto.DismissalId;
                                dailyDismissal.SubmittedDate = DateTime.Now;
                                dailyDismissal.DismissalDuration = dto.DismissalDuration;
                                dailyDismissal.RequestDescription = dto.RequestDescription;
                                dailyDismissal.FromDate = fromDate;
                                dailyDismissal.ToDate = toDate;

                                from = dailyDismissal.FromDate;
                                to = dailyDismissal.ToDate;
                            }
                            else //update entity type -> remove and insert again
                            {
                                try
                                {
                                    await DeleteAndInsertAgain(dto);
                                }
                                catch (TransactionAbortedException ex)
                                {
                                    _logger.LogRunTimeError(ex, ex.Message
                                        + " Either update or delete operation failed.");
                                    throw;
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogRunTimeError(ex, ex.Message);
                                    throw;
                                }
                            }
                            break;
                        case RequestDuration.Hourly:

                            var date = dto.HourlyDismissal.Date;

                            if (personnelDismissal.DismissalDuration == dto.DismissalDuration) //update the same type
                            {
                                var hourlyDismissal = personnelDismissal as PersonnelHourlyDismissal;
                                hourlyDismissal.PersonnelId = personnelDismissal.PersonnelId;
                                hourlyDismissal.DismissalId = dto.DismissalId;
                                hourlyDismissal.SubmittedDate = DateTime.Now;
                                hourlyDismissal.DismissalDuration = dto.DismissalDuration;
                                hourlyDismissal.RequestDescription = dto.RequestDescription;
                                hourlyDismissal.Date = date;
                                hourlyDismissal.FromTime = dto.HourlyDismissal.FromTime;
                                hourlyDismissal.ToTime = dto.HourlyDismissal.ToTime;

                                from = hourlyDismissal.Date.Add(hourlyDismissal.FromTime);
                                to = hourlyDismissal.Date.Add(hourlyDismissal.ToTime);
                            }
                            else //update entity type -> remove and insert again
                            {
                                try
                                {
                                    await DeleteAndInsertAgain(dto);
                                }
                                catch (TransactionAbortedException ex)
                                {
                                    _logger.LogRunTimeError(ex, ex.Message
                                        + " Either update or delete operation failed.");
                                    throw;
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogRunTimeError(ex, ex.Message);
                                    throw;
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    #endregion

                    //Send Request Message to Approval Proc
                    //remove prev
                    _messageService.RemoveRequest(RequestType.Dismissal, personnelDismissal.Id);
                    //insert new
                    var notificationReceiverId = await _requestMessageHandlerService
                        .HandleDismisalRequest(personnelDismissal.Id, personnelDismissal.DismissalDuration
                            , from, to, user.Username);

                    if (notificationReceiverId == null)
                    {
                        return new CustomResult<string>
                        {
                            Message = "approval procedure not found"
                        };
                    }

                    return new CustomResult<string>
                    {
                        Message = !string.IsNullOrEmpty(filterResult.Message)
                            ? filterResult.Message : null,
                        IsValid = true,
                        ReturnId = notificationReceiverId
                    };
                }
                else
                {
                    string requestAction = personnelDismissal.RequestAction == RequestAction.Accept
                        ? "confirmed" : personnelDismissal.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : "rejected";
                    return new CustomResult<string>
                    {
                        IsValid = false,
                        Message = "cannot edit dismissal info, status: "
                            + requestAction
                    };
                }
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
                        (ex, "PersonnelDismissal entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public async Task<CustomResult> Action(PersonnelDismissalActionDto dto)
        {
            var personnelDismissal = _personnelDismissalRepository
                .Get(q => q.Id == dto.PersonnelDismissalId, includeProperties: "Personnel,Dismissal")
                .SingleOrDefault();
            if (personnelDismissal != null)
            {
                if (personnelDismissal.ActionDate.HasValue
                    && personnelDismissal.RequestAction != RequestAction.PartialAccept)
                {
                    return new CustomResult
                    {
                        Message = "your request has processed before"
                    };
                }

                if (LimitPassed(personnelDismissal))
                {
                    personnelDismissal.RequestAction = RequestAction.Reject;
                    personnelDismissal.ActionDescription =
                        "action time period is finished";
                }
                else
                {
                    personnelDismissal.ActionDate = DateTime.Now;

                    //still needs to confirm with superior in the proc
                    if (dto.ParentApprovalProcId.HasValue)
                    {
                        var receiverInfo = _approvalProcService
                            .GetNextReceiverId(dto.ParentApprovalProcId.Value);
                        if (dto.RequestAction == RequestAction.Accept)
                        {
                            personnelDismissal.RequestAction = RequestAction.PartialAccept;
                            personnelDismissal.ActionDescription = dto.ActionDescription;

                            await _requestMessageHandlerService.SubmitMessage(dto.MessageId, receiverInfo
                                , dto.RequestId, dto.RequestType, personnelDismissal.RequestAction, personnelDismissal.Personnel.Code);
                        }
                        else if (dto.RequestAction == RequestAction.Reject)
                        {
                            personnelDismissal.RequestAction = RequestAction.Reject;
                            personnelDismissal.ActionDescription = dto.ActionDescription;
                        }
                    }
                    else
                    {
                        //no superior
                        personnelDismissal.RequestAction = dto.RequestAction;
                        personnelDismissal.ActionDescription = dto.ActionDescription;
                    }
                }

                _personnelDismissalRepository.Update(personnelDismissal);
                //update request message action
                _requestMessageHandlerService.UpdateRequestAction(dto.MessageId, personnelDismissal.RequestAction);
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
                        (ex, "PersonnelDismissal entity with the id: '{0}', is not available." +
                        " action operation failed.", dto.PersonnelDismissalId);
                    throw;
                }
            }

            return new CustomResult
            {
                IsValid = true
            };
        }

        public async Task<CustomResult> Action(List<PersonnelDismissalActionDto> dto)
        {
            foreach (var action in dto)
            {
                var personnelDismissal = _personnelDismissalRepository
                    .Get(q => q.Id == action.PersonnelDismissalId, includeProperties: "Personnel,Dismissal")
                    .SingleOrDefault();
                if (personnelDismissal != null)
                {
                    if (personnelDismissal.ActionDate.HasValue
                        && personnelDismissal.RequestAction != RequestAction.PartialAccept)
                    {
                        return new CustomResult
                        {
                            Message = "your request has processed before"
                        };
                    }

                    if (LimitPassed(personnelDismissal))
                    {
                        personnelDismissal.RequestAction = RequestAction.Reject;
                        personnelDismissal.ActionDescription =
                            "action time period is finished";
                    }
                    else
                    {
                        personnelDismissal.ActionDate = DateTime.Now;

                        //still needs to confirm with superior in the proc
                        if (action.ParentApprovalProcId.HasValue)
                        {
                            var receiverInfo = _approvalProcService
                                .GetNextReceiverId(action.ParentApprovalProcId.Value);
                            if (action.RequestAction == RequestAction.Accept)
                            {
                                personnelDismissal.RequestAction = RequestAction.PartialAccept;
                                personnelDismissal.ActionDescription = action.ActionDescription;

                                await _requestMessageHandlerService.SubmitMessage(action.MessageId, receiverInfo
                                , action.RequestId, action.RequestType, personnelDismissal.RequestAction, personnelDismissal.Personnel.Code);
                            }
                            else if (action.RequestAction == RequestAction.Reject)
                            {
                                personnelDismissal.RequestAction = RequestAction.Reject;
                                personnelDismissal.ActionDescription = action.ActionDescription;
                            }
                        }
                        else
                        {
                            //no superior
                            personnelDismissal.RequestAction = action.RequestAction;
                            personnelDismissal.ActionDescription = action.ActionDescription;
                        }
                    }

                    _personnelDismissalRepository.Update(personnelDismissal);
                    //update request message action
                    _requestMessageHandlerService.UpdateRequestAction(action.MessageId, personnelDismissal.RequestAction);
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
                            (ex, "PersonnelDismissal entity with the id: '{0}', is not available." +
                            " action operation failed.", action.PersonnelDismissalId);
                        throw;
                    }
                }
            }

            return new CustomResult
            {
                IsValid = true
            };
        }

        public CustomResult Delete(int id, DeleteState deleteState)
        {
            var personnelDismissal = _personnelDismissalRepository.GetById(id);
            if (personnelDismissal != null)
            {
                if (personnelDismissal.RequestAction == RequestAction.Unknown)
                {
                    _personnelDismissalRepository.Delete(personnelDismissal, deleteState);
                }
                else
                {
                    string requestAction = personnelDismissal.RequestAction == RequestAction.Accept
                        ? "confirmed" : personnelDismissal.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : "rejected";
                    return new CustomResult
                    {
                        IsValid = false,
                        Message = "cannot delete dismissal info. status: "
                            + requestAction
                    };
                }
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
                        (ex, "PersonnelDismissal entity with the id: '{0}', is not available." +
                        " delete operation failed.", id);
                    throw;
                }
            }

            return new CustomResult
            {
                IsValid = true
            };
        }

        #region Helpers
        private bool LimitPassed(PersonnelDismissal personnelDismissal)
        {
            var selectedDismissal = personnelDismissal.Dismissal;
            if (selectedDismissal.ActionLimitDays.HasValue
                    && (DateTime.Now > personnelDismissal.SubmittedDate
                        .AddDays(selectedDismissal.ActionLimitDays.Value)))
            {
                return true;
            }
            return false;
        }
        private bool AlreadyRequested(int personnelId, DailyDismissal dailyDismissal
            , HourlyDismissal hourlyDismissal, int? exceptionId = null)
        {
            bool result = false;
            if (exceptionId.HasValue)
            {
                if (dailyDismissal != null)
                {
                    result = _personnelDailyDismissalRepository.Get(q => q.Id != exceptionId
                        && q.PersonnelId == personnelId
                        && (q.RequestAction == RequestAction.Accept || q.RequestAction == RequestAction.PartialAccept))
                        .AsEnumerable()
                        .Any(q => q.FromDate <= dailyDismissal.FromDate
                            && q.ToDate >= dailyDismissal.ToDate);
                }
                else if (hourlyDismissal != null)
                {
                    result = _personnelHourlyDismissalRepository.Get(q => q.Id != exceptionId
                        && q.PersonnelId == personnelId
                        && (q.RequestAction == RequestAction.Accept || q.RequestAction == RequestAction.PartialAccept))
                        .AsEnumerable()
                        .Any(q => q.Date == hourlyDismissal.Date
                            && q.FromTime <= hourlyDismissal.FromTime
                            && q.ToTime >= hourlyDismissal.ToTime);
                }
            }
            else
            {
                if (dailyDismissal != null)
                {
                    result = _personnelDailyDismissalRepository.Get(q => q.PersonnelId == personnelId
                        && (q.RequestAction == RequestAction.Accept || q.RequestAction == RequestAction.PartialAccept))
                        .AsEnumerable()
                        .Any(q => q.FromDate <= dailyDismissal.FromDate
                            && q.ToDate >= dailyDismissal.ToDate);
                }
                else if (hourlyDismissal != null)
                {
                    result = _personnelHourlyDismissalRepository.Get(q => q.PersonnelId == personnelId
                        && (q.RequestAction == RequestAction.Accept || q.RequestAction == RequestAction.PartialAccept))
                        .AsEnumerable()
                        .Any(q => q.Date == hourlyDismissal.Date
                            && q.FromTime <= hourlyDismissal.FromTime
                            && q.ToTime >= hourlyDismissal.ToTime);
                }
            }
            return result;
        }
        private CustomResult<string> CheckLimitFilters(PersonnelDismissalRecordDto personnelDismissalRecord
            , double requestedDismissalDuration, int? dismissalAllowance, string durationTitle
            , DismissalExcessiveReaction dismissalExcessiveReaction, string dismissalTitle)
        {
            if (dismissalAllowance.HasValue)
            {
                if (dismissalAllowance.Value
                    < personnelDismissalRecord.UsedDismissalDuration.GetSecondsFromDuration()
                    + requestedDismissalDuration) //personnel dismissal's history + new request duration
                {
                    switch (dismissalExcessiveReaction)
                    {
                        case DismissalExcessiveReaction.Nothing:
                            break;
                        case DismissalExcessiveReaction.Alarm:
                            return new CustomResult<string>
                            {
                                IsValid = true,
                                Message = " alarm, limit is exceeded for: " + dismissalTitle
                            };
                        case DismissalExcessiveReaction.Forbid:
                            return new CustomResult<string>
                            {
                                IsValid = false,
                                Message = "not permitted, limit is exceeded for: " + dismissalTitle
                            };
                    }
                }
            }
            return new CustomResult<string>
            {
                IsValid = true
            };
        }
        private CustomResult<string> CheckCountFilters(PersonnelDismissalRecordDto personnelDismissalRecord
            , int? count, string durationTitle
            , DismissalExcessiveReaction dismissalExcessiveReaction, string dismissalTitle)
        {
            if (count.HasValue)
            {
                //personnel dismissal's history + new requets
                if (count.Value < personnelDismissalRecord.UsedDismissalCountValue + 1)
                {
                    switch (dismissalExcessiveReaction)
                    {
                        case DismissalExcessiveReaction.Nothing:
                            break;
                        case DismissalExcessiveReaction.Alarm:
                            return new CustomResult<string>
                            {
                                IsValid = false,
                                Message = "alarm, count limit is exceeded for: " + dismissalTitle
                            };
                        case DismissalExcessiveReaction.Forbid:
                            return new CustomResult<string>
                            {
                                IsValid = false,
                                Message = "not permitted, count limit is exceeded for: " + dismissalTitle
                            };
                    }
                }
            }
            return new CustomResult<string>
            {
                IsValid = true
            };
        }
        private PersonnelDismissalHistoryDto GetPersonnelDismissalHistory(int personnelId, int dismissalId)
        {
            DateTime now = DateTime.Now.Date;

            int month = now.Month;
            int year = now.Year;

            #region Total
            //daily
            var usedDismissalsDaysInTotal = _personnelDailyDismissalRepository
                .Get(q => q.PersonnelId == personnelId
                    && q.DismissalId == dismissalId && q.ActionDate.HasValue
                    && q.RequestAction == RequestAction.Accept)
                .ToList();
            //hourly
            var usedDismissalsHoursInTotal = _personnelHourlyDismissalRepository
                .Get(q => q.PersonnelId == personnelId
                    && q.DismissalId == dismissalId && q.ActionDate.HasValue
                    && q.RequestAction == RequestAction.Accept)
                .ToList();

            var totalDto = new PersonnelDismissalRecordDto
            {
                UsedDismissalTitle = "used dismissal in total",
                UsedDismissalDuration = new Duration
                {
                    Days = usedDismissalsDaysInTotal.Count > 0
                        ? usedDismissalsDaysInTotal.GroupBy(grp => grp.DismissalId)
                        .Select(x => new
                        {
                            TotalDays = x.Sum(q => (q.ToDate - q.FromDate).Days)
                        }).SingleOrDefault().TotalDays
                        : 0,
                    Hours = usedDismissalsHoursInTotal.Count > 0
                        ? usedDismissalsHoursInTotal.GroupBy(grp => grp.DismissalId)
                        .Select(x => new
                        {
                            TotalHours = x.Sum(q => (q.ToTime - q.FromTime).Hours)
                        }).SingleOrDefault().TotalHours
                        : 0
                },
                UsedDismissalCountTitle = "used count of dismissal in total",
                UsedDismissalCountValue =
                (usedDismissalsDaysInTotal.Count > 0
                    ? usedDismissalsDaysInTotal.GroupBy(grp => grp.DismissalId)
                    .Select(x => new
                    {
                        Count = x.Count()
                    }).SingleOrDefault().Count
                    : 0)
                +
                (usedDismissalsHoursInTotal.Count > 0
                    ? usedDismissalsHoursInTotal.GroupBy(grp => grp.DismissalId)
                    .Select(x => new
                    {
                        Count = x.Count()
                    }).SingleOrDefault().Count
                    : 0)
            };
            #endregion

            #region Year
            //daily
            var usedDismissalsDaysInYear = usedDismissalsDaysInTotal
                .Where(q => q.ActionDate.Value.Year == year).ToList();
            //hourly
            var usedDismissalsHoursInYear = usedDismissalsHoursInTotal
                .Where(q => q.ActionDate.Value.Year == year).ToList();

            var yearDto = new PersonnelDismissalRecordDto
            {
                UsedDismissalTitle = "used dismissal in year " + year,
                UsedDismissalDuration = new Duration
                {
                    Days = usedDismissalsDaysInYear.Count > 0
                        ? usedDismissalsDaysInYear.GroupBy(grp => grp.DismissalId)
                        .Select(x => new
                        {
                            TotalDays = x.Sum(q => (q.ToDate - q.FromDate).Days)
                        }).SingleOrDefault().TotalDays
                        : 0,
                    Hours = usedDismissalsHoursInYear.Count > 0
                        ? usedDismissalsHoursInYear.GroupBy(grp => grp.DismissalId)
                        .Select(x => new
                        {
                            TotalHours = x.Sum(q => (q.ToTime - q.FromTime).Hours)
                        }).SingleOrDefault().TotalHours
                        : 0
                },
                UsedDismissalCountTitle = "used count of dismissal in year " + year,
                UsedDismissalCountValue =
                (usedDismissalsDaysInYear.Count > 0
                    ? usedDismissalsDaysInYear.GroupBy(grp => grp.DismissalId)
                    .Select(x => new
                    {
                        Count = x.Count()
                    }).SingleOrDefault().Count
                    : 0)
                +
                (usedDismissalsHoursInYear.Count > 0
                    ? usedDismissalsHoursInYear.GroupBy(grp => grp.DismissalId)
                    .Select(x => new
                    {
                        Count = x.Count()
                    }).SingleOrDefault().Count
                    : 0)
            };
            #endregion

            #region Month
            //daily
            var usedDismissalsDaysInMonth = usedDismissalsDaysInTotal
                .Where(q => q.ActionDate.Value.Year == year
                    && q.ActionDate.Value.Month == month).ToList();
            //hourly
            var usedDismissalsHoursInMonth = usedDismissalsHoursInTotal
                .Where(q => q.ActionDate.Value.Year == year
                    && q.ActionDate.Value.Month == month).ToList();

            var monthDto = new PersonnelDismissalRecordDto
            {
                UsedDismissalTitle = "used dismissal in month: "
                    + month.GetMonthInNameGC(CultureInfoTag.English_US, OutputDateFormat.Complete),
                UsedDismissalDuration = new Duration
                {
                    Days = usedDismissalsDaysInMonth.Count > 0
                        ? usedDismissalsDaysInMonth.GroupBy(grp => grp.DismissalId)
                        .Select(x => new
                        {
                            TotalDays = x.Sum(q => (q.ToDate - q.FromDate).Days)
                        }).SingleOrDefault().TotalDays
                        : 0,
                    Hours = usedDismissalsHoursInMonth.Count > 0
                        ? usedDismissalsHoursInMonth.GroupBy(grp => grp.DismissalId)
                        .Select(x => new
                        {
                            TotalHours = x.Sum(q => (q.ToTime - q.FromTime).Hours)
                        }).SingleOrDefault().TotalHours
                        : 0,
                },
                UsedDismissalCountTitle = "used count of dismissal in month "
                    + month.GetMonthInNameGC(CultureInfoTag.English_US, OutputDateFormat.Complete),
                UsedDismissalCountValue =
                (usedDismissalsDaysInMonth.Count > 0
                    ? usedDismissalsDaysInMonth.GroupBy(grp => grp.DismissalId)
                    .Select(x => new
                    {
                        Count = x.Count()
                    }).SingleOrDefault().Count
                    : 0)
                +
                (usedDismissalsHoursInMonth.Count > 0
                    ? usedDismissalsHoursInMonth.GroupBy(grp => grp.DismissalId)
                    .Select(x => new
                    {
                        Count = x.Count()
                    }).SingleOrDefault().Count
                    : 0)
            };
            #endregion

            #region Day
            //hourly
            var usedDismissalsHoursInDay = usedDismissalsHoursInTotal
                .Where(q => q.SubmittedDate.Date == DateTime.Now.Date).ToList();

            var dayDto = new PersonnelDismissalRecordDto
            {
                UsedDismissalTitle = "used dismissal in day ",
                UsedDismissalDuration = new Duration
                {
                    Days = usedDismissalsHoursInDay.Count > 0
                    ? usedDismissalsHoursInDay.GroupBy(grp => grp.DismissalId)
                    .Select(x => new
                    {
                        TotalHours = x.Sum(q => (q.ToTime - q.FromTime).Hours)
                    }).SingleOrDefault().TotalHours
                    : 0,
                },
                UsedDismissalCountTitle = "used count of dismissal in day ",
                UsedDismissalCountValue = (usedDismissalsHoursInDay.Count > 0
                    ? usedDismissalsHoursInDay.GroupBy(grp => grp.DismissalId)
                    .Select(x => new
                    {
                        Count = x.Count()
                    }).SingleOrDefault().Count
                    : 0)
            };
            #endregion

            return new PersonnelDismissalHistoryDto
            {
                DayData = dayDto,
                MonthData = monthDto,
                YearData = yearDto,
                TotalData = totalDto
            };
        }
        private async Task DeleteAndInsertAgain(UpdatePersonnelDismissalDto dto)
        {
            var personnel = _personnelRepository.GetById(dto.PersonnelId);

            using (var scope = new TransactionScope())
            {
                _personnelDismissalRepository.Delete(dto.Id, DeleteState.Permanent);
                var newItem = new CreatePersonnelDismissalDto
                {
                    DismissalId = dto.DismissalId,
                    DismissalDuration = dto.DismissalDuration,
                    FileUploadPath = dto.FileUploadPath,
                    RequestDescription = dto.RequestDescription
                };
                switch (dto.DismissalDuration)
                {
                    case RequestDuration.Hourly:
                        newItem.HourlyDismissal = new HourlyDismissal
                        {
                            Date = dto.HourlyDismissal.Date,
                            FromTime = dto.HourlyDismissal.FromTime,
                            ToTime = dto.HourlyDismissal.ToTime
                        };
                        break;
                    case RequestDuration.Daily:
                        newItem.DailyDismissal = new DailyDismissal
                        {
                            FromDate = dto.DailyDismissal.FromDate,
                            ToDate = dto.DailyDismissal.ToDate
                        };
                        break;
                }
                await Create(newItem, personnel.Code);

                scope.Complete();
            }
        }
        private void PersonnelDismissalDetailsError(RequestDuration dismissalDuration, string operation)
        {
            try
            {
                throw new LogicalException();
            }
            catch (LogicalException ex)
            {
                _logger.LogRunTimeError(ex, "The PersonnelDismissal dto that has passed to " +
                    "the service should also have '{0}' personnel dismissal properties." +
                    " {1} operation failed.", dismissalDuration, operation);
                throw;
            }
        }
        private void RequestDateCheck(DateTime date, string operation)
        {
            if (date.Date < DateTime.Now.Date)
                throw new LogicalException();
        }
        #endregion
    }
}
