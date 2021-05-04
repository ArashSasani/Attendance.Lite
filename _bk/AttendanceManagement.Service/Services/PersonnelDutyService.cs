using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.PersonnelDuty;
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
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class PersonnelDutyService : IPersonnelDutyService
    {
        private readonly IRepository<Personnel> _personnelRepository;
        private readonly IRepository<PersonnelDuty> _personnelDutyRepository;
        private readonly IRepository<PersonnelDailyDuty> _personnelDailyDutyRepository;
        private readonly IRepository<PersonnelHourlyDuty> _personnelHourlyDutyRepository;

        private readonly IAuthService _authService;
        private readonly IApprovalProcService _approvalProcService;
        private readonly IMessageService _messageService;
        private readonly IRequestMessageHandlerService _requestMessageHandlerService;

        private readonly IExceptionLogger _logger;

        public PersonnelDutyService(IRepository<PersonnelDuty> personnelDutyRepository
            , IRepository<Personnel> personnelRepository
            , IRepository<PersonnelDailyDuty> personnelDailyDuty
            , IRepository<PersonnelHourlyDuty> personnelHourlyDuty
            , IAuthService authService
            , IApprovalProcService approvalProcService, IMessageService messageService
            , IRequestMessageHandlerService requestMessageHandlerService
            , IExceptionLogger logger)
        {
            _personnelRepository = personnelRepository;
            _personnelDutyRepository = personnelDutyRepository;
            _personnelDailyDutyRepository = personnelDailyDuty;
            _personnelHourlyDutyRepository = personnelHourlyDuty;

            _authService = authService;
            _approvalProcService = approvalProcService;
            _messageService = messageService;
            _requestMessageHandlerService = requestMessageHandlerService;

            _logger = logger;
        }


        public IPaging<PersonnelDutyDtoForPaging> Get(string username, string searchTerm, string sortItem, string sortOrder
            , PagingQueryString pagingQueryString)
        {
            IPaging<PersonnelDutyDtoForPaging> model = new PersonnelDutyDtoPagingList();

            IQueryable<PersonnelDuty> query = null;

            var personnel = _personnelRepository.Get(q => q.Code == username).SingleOrDefault();
            if (personnel != null)
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelDutyRepository.Get(q => q.PersonnelId == personnel.Id
                    && (q.Personnel.Name.Contains(searchTerm.ToLower())
                    || q.Personnel.LastName.Contains(searchTerm.ToLower())
                    || q.Duty.Title.Contains(searchTerm.ToLower()))
                    , includeProperties: "Personnel,Duty")
                : _personnelDutyRepository.Get(q => q.PersonnelId == personnel.Id
                    , includeProperties: "Personnel,Duty");
            }
            else
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelDutyRepository.Get(q => q.Personnel.Name.Contains(searchTerm.ToLower())
                    || q.Personnel.LastName.Contains(searchTerm.ToLower())
                    || q.Duty.Title.Contains(searchTerm.ToLower())
                    , includeProperties: "Personnel,Duty")
                : _personnelDutyRepository.Get(includeProperties: "Personnel,Duty");
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
                case "duty_title":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Duty.Title)
                        : query.OrderByDescending(o => o.Duty.Title);
                    break;
                default:
                    query = query.OrderByDescending(o => o.Id);
                    break;
            }

            List<PersonnelDuty> queryResult;
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
            model.PagingList = Mapper.Map<List<PersonnelDutyDtoForPaging>>(queryResult);

            return model;
        }

        public PersonnelDutyDto GetById(int id)
        {
            var personnelDuty = _personnelDutyRepository.Get(q => q.Id == id
                , includeProperties: "Duty,Personnel").SingleOrDefault();
            if (personnelDuty == null)
            {
                return null;
            }
            switch (personnelDuty.DutyDuration)
            {
                case RequestDuration.Daily:
                    var daily = personnelDuty as PersonnelDailyDuty;
                    return Mapper.Map<PersonnelDutyDto>(daily);
                case RequestDuration.Hourly:
                    var hourly = personnelDuty as PersonnelHourlyDuty;
                    return Mapper.Map<PersonnelDutyDto>(hourly);
                default:
                    break;
            }
            return Mapper.Map<PersonnelDutyDto>(personnelDuty);
        }

        public async Task<CustomResult<string>> Create(CreatePersonnelDutyDto dto
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
                        Message = "person is not available"
                    };
                }

                if (AlreadyRequested(personnel.Id, dto.DailyDuty, dto.HourlyDuty))
                {
                    return new CustomResult<string>
                    {
                        Message = "cannot add multiple duties in the time period"
                    };
                }

                int personnelDutyId = 0;
                DateTime from = DateTime.Now;
                DateTime to = DateTime.Now;
                switch (dto.DutyDuration)
                {
                    case RequestDuration.Daily:
                        if (dto.DailyDuty != null)
                        {
                            var dailyDuty = new PersonnelDailyDuty
                            {
                                PersonnelId = personnel.Id,
                                DutyId = dto.DutyId,
                                SubmittedDate = DateTime.Now,
                                DutyDuration = dto.DutyDuration,
                                RequestDescription = dto.RequestDescription,
                                FromDate = dto.DailyDuty.FromDate,
                                ToDate = dto.DailyDuty.ToDate
                            };
                            _personnelDailyDutyRepository.Insert(dailyDuty);
                            personnelDutyId = dailyDuty.Id;
                            from = dailyDuty.FromDate;
                            to = dailyDuty.ToDate;
                        }
                        else
                        {
                            PersonnelDutyDetailsError(dto.DutyDuration, "create");
                        }
                        break;
                    case RequestDuration.Hourly:
                        if (dto.HourlyDuty != null)
                        {
                            var hourlyDuty = new PersonnelHourlyDuty
                            {
                                PersonnelId = personnel.Id,
                                DutyId = dto.DutyId,
                                SubmittedDate = DateTime.Now,
                                DutyDuration = dto.DutyDuration,
                                RequestDescription = dto.RequestDescription,
                                Date = dto.HourlyDuty.Date,
                                FromTime = dto.HourlyDuty.FromTime,
                                ToTime = dto.HourlyDuty.ToTime
                            };
                            _personnelHourlyDutyRepository.Insert(hourlyDuty);
                            personnelDutyId = hourlyDuty.Id;
                            from = hourlyDuty.Date.Add(hourlyDuty.FromTime);
                            to = hourlyDuty.Date.Add(hourlyDuty.ToTime);
                        }
                        else
                        {
                            PersonnelDutyDetailsError(dto.DutyDuration, "create");
                        }
                        break;
                    default:
                        break;
                }

                //Send Request Message to Approval Proc
                var notificationReceiverId = await _requestMessageHandlerService
                    .HandleDutyRequest(personnelDutyId, dto.DutyDuration, from, to, user.Username);

                if (notificationReceiverId == null)
                {
                    return new CustomResult<string>
                    {
                        Message = "approval procedure not found"
                    };
                }

                return new CustomResult<string>
                {
                    IsValid = true,
                    ReturnId = notificationReceiverId
                };
            }
            return new CustomResult<string>
            {
                Message = "user not found"
            };
        }

        public async Task<CustomResult<string>> Update(UpdatePersonnelDutyDto dto)
        {
            var personnelDuty = _personnelDutyRepository.GetById(dto.Id);
            if (personnelDuty != null)
            {
                dto.PersonnelId = personnelDuty.PersonnelId;
                var personnel = _personnelRepository.GetById(dto.PersonnelId);
                if (personnel == null)
                {
                    return new CustomResult<string>
                    {
                        Message = "person is not available"
                    };
                }

                var user = await _authService.FindUserByUsernameAsync(personnel.Code);
                if (user == null)
                {
                    return new CustomResult<string>
                    {
                        Message = "user for person not found"
                    };
                }

                if (AlreadyRequested(personnel.Id, dto.DailyDuty, dto.HourlyDuty, personnelDuty.Id))
                {
                    return new CustomResult<string>
                    {
                        Message = "cannot add multiple duties in the time period"
                    };
                }

                if (personnelDuty.RequestAction == RequestAction.Unknown)
                {
                    DateTime from = DateTime.Now;
                    DateTime to = DateTime.Now;
                    switch (dto.DutyDuration)
                    {
                        case RequestDuration.Daily:
                            if (personnelDuty.DutyDuration == dto.DutyDuration) //update the same type
                            {
                                var dailyDuty = personnelDuty as PersonnelDailyDuty;
                                dailyDuty.PersonnelId = dto.PersonnelId;
                                dailyDuty.DutyId = dto.DutyId;
                                dailyDuty.SubmittedDate = DateTime.Now;
                                dailyDuty.DutyDuration = dto.DutyDuration;
                                dailyDuty.RequestDescription = dto.RequestDescription;
                                dailyDuty.FromDate = dto.DailyDuty.FromDate;
                                dailyDuty.ToDate = dto.DailyDuty.ToDate;

                                from = dailyDuty.FromDate;
                                to = dailyDuty.ToDate;
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
                            if (personnelDuty.DutyDuration == dto.DutyDuration) //update the same type
                            {
                                var hourlyDuty = personnelDuty as PersonnelHourlyDuty;
                                hourlyDuty.PersonnelId = dto.PersonnelId;
                                hourlyDuty.DutyId = dto.DutyId;
                                hourlyDuty.SubmittedDate = DateTime.Now;
                                hourlyDuty.DutyDuration = dto.DutyDuration;
                                hourlyDuty.RequestDescription = dto.RequestDescription;
                                hourlyDuty.Date = dto.HourlyDuty.Date;
                                hourlyDuty.FromTime = dto.HourlyDuty.FromTime;
                                hourlyDuty.ToTime = dto.HourlyDuty.ToTime;

                                from = hourlyDuty.Date.Add(hourlyDuty.FromTime);
                                to = hourlyDuty.Date.Add(hourlyDuty.ToTime);
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

                    //Send Request Message to Approval Proc
                    //remove prev
                    _messageService.RemoveRequest(RequestType.Duty, personnelDuty.Id);
                    //insert new
                    var notificationReceiverId = await _requestMessageHandlerService
                        .HandleDutyRequest(personnelDuty.Id, personnelDuty.DutyDuration
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
                        IsValid = true,
                        ReturnId = notificationReceiverId
                    };
                }
                else
                {
                    string requestAction = personnelDuty.RequestAction == RequestAction.Accept
                        ? "confirmed" : personnelDuty.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : "rejected";
                    return new CustomResult<string>
                    {
                        IsValid = false,
                        Message = "cannot update the duty info, status: "
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
                        (ex, "PersonnelDuty entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public async Task<CustomResult> Action(PersonnelDutyActionDto dto)
        {
            var personnelDuty = _personnelDutyRepository
               .Get(q => q.Id == dto.PersonnelDutyId, includeProperties: "Personnel,Duty")
               .SingleOrDefault();
            if (personnelDuty != null)
            {
                if (personnelDuty.ActionDate.HasValue
                    && personnelDuty.RequestAction != RequestAction.PartialAccept)
                {
                    return new CustomResult
                    {
                        Message = "your request is processed before"
                    };
                }

                if (LimitPassed(personnelDuty))
                {
                    personnelDuty.RequestAction = RequestAction.Reject;
                    personnelDuty.ActionDescription =
                        "action time period is finished";
                }
                else
                {
                    personnelDuty.ActionDate = DateTime.Now;

                    //still needs to confirm with superior in the proc
                    if (dto.ParentApprovalProcId.HasValue)
                    {
                        var receiverInfo = _approvalProcService
                            .GetNextReceiverId(dto.ParentApprovalProcId.Value);
                        if (dto.RequestAction == RequestAction.Accept)
                        {
                            personnelDuty.RequestAction = RequestAction.PartialAccept;
                            personnelDuty.ActionDescription = dto.ActionDescription;

                            await _requestMessageHandlerService.SubmitMessage(dto.MessageId, receiverInfo
                                , dto.RequestId, dto.RequestType, personnelDuty.RequestAction, personnelDuty.Personnel.Code);
                        }
                        else if (dto.RequestAction == RequestAction.Reject)
                        {
                            personnelDuty.RequestAction = RequestAction.Reject;
                            personnelDuty.ActionDescription = dto.ActionDescription;
                        }
                    }
                    else
                    {
                        //no superior
                        personnelDuty.RequestAction = dto.RequestAction;
                        personnelDuty.ActionDescription = dto.ActionDescription;
                    }
                }

                _personnelDutyRepository.Update(personnelDuty);
                //update request message action
                _requestMessageHandlerService.UpdateRequestAction(dto.MessageId, personnelDuty.RequestAction);
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
                        (ex, "PersonnelDuty entity with the id: '{0}', is not available." +
                        " action operation failed.", dto.PersonnelDutyId);
                    throw;
                }
            }

            return new CustomResult
            {
                IsValid = true
            };
        }

        public async Task<CustomResult> Action(List<PersonnelDutyActionDto> dto)
        {
            foreach (var action in dto)
            {
                var personnelDuty = _personnelDutyRepository
                   .Get(q => q.Id == action.PersonnelDutyId, includeProperties: "Personnel,Duty")
                   .SingleOrDefault();
                if (personnelDuty != null)
                {
                    if (personnelDuty.ActionDate.HasValue
                        && personnelDuty.RequestAction != RequestAction.PartialAccept)
                    {
                        return new CustomResult
                        {
                            Message = "your request is processed before"
                        };
                    }

                    if (LimitPassed(personnelDuty))
                    {
                        personnelDuty.RequestAction = RequestAction.Reject;
                        personnelDuty.ActionDescription =
                            "action time period is finished";
                    }
                    else
                    {
                        personnelDuty.ActionDate = DateTime.Now;

                        //still needs to confirm with superior in the proc
                        if (action.ParentApprovalProcId.HasValue)
                        {
                            var receiverInfo = _approvalProcService
                                .GetNextReceiverId(action.ParentApprovalProcId.Value);
                            if (action.RequestAction == RequestAction.Accept)
                            {
                                personnelDuty.RequestAction = RequestAction.PartialAccept;
                                personnelDuty.ActionDescription = action.ActionDescription;

                                await _requestMessageHandlerService.SubmitMessage(action.MessageId, receiverInfo,
                                    action.RequestId, action.RequestType, personnelDuty.RequestAction, personnelDuty.Personnel.Code);
                            }
                            else if (action.RequestAction == RequestAction.Reject)
                            {
                                personnelDuty.RequestAction = RequestAction.Reject;
                                personnelDuty.ActionDescription = action.ActionDescription;
                            }
                        }
                        else
                        {
                            //no superior
                            personnelDuty.RequestAction = action.RequestAction;
                            personnelDuty.ActionDescription = action.ActionDescription;
                        }
                    }

                    _personnelDutyRepository.Update(personnelDuty);
                    //update request message action
                    _requestMessageHandlerService.UpdateRequestAction(action.MessageId, personnelDuty.RequestAction);
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
                            (ex, "PersonnelDuty entity with the id: '{0}', is not available." +
                            " action operation failed.", action.PersonnelDutyId);
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
            var personnelDuty = _personnelDutyRepository.GetById(id);
            if (personnelDuty != null)
            {
                if (personnelDuty.RequestAction == RequestAction.Unknown)
                {
                    _personnelDutyRepository.Delete(personnelDuty, deleteState);
                }
                else
                {
                    string requestAction = personnelDuty.RequestAction == RequestAction.Accept
                        ? "confirmed" : personnelDuty.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : "rejected";
                    return new CustomResult
                    {
                        IsValid = false,
                        Message = "cannot delete the duty info, status: "
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
                        (ex, "PersonnelDuty entity with the id: '{0}', is not available." +
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
        private bool LimitPassed(PersonnelDuty personnelDuty)
        {
            var selectedDuty = personnelDuty.Duty;
            if (selectedDuty.ActionLimitDays.HasValue
                    && (DateTime.Now > personnelDuty.SubmittedDate
                        .AddDays(selectedDuty.ActionLimitDays.Value)))
            {
                return true;
            }
            return false;
        }
        private bool AlreadyRequested(int personnelId, DailyDuty dailyDuty
            , HourlyDuty hourlyDuty, int? exceptionId = null)
        {
            bool result = false;
            if (exceptionId.HasValue)
            {
                if (dailyDuty != null)
                {
                    result = _personnelDailyDutyRepository.Get(q => q.Id != exceptionId
                        && q.PersonnelId == personnelId
                        && (q.RequestAction == RequestAction.Accept || q.RequestAction == RequestAction.PartialAccept))
                        .AsEnumerable()
                        .Any(q => q.FromDate <= dailyDuty.FromDate
                            && q.ToDate >= dailyDuty.ToDate);
                }
                else if (hourlyDuty != null)
                {
                    result = _personnelHourlyDutyRepository.Get(q => q.Id != exceptionId
                        && q.PersonnelId == personnelId
                        && (q.RequestAction == RequestAction.Accept || q.RequestAction == RequestAction.PartialAccept))
                        .AsEnumerable()
                        .Any(q => q.Date == hourlyDuty.Date
                            && q.FromTime <= hourlyDuty.FromTime && q.ToTime >= hourlyDuty.ToTime);
                }
            }
            else
            {
                if (dailyDuty != null)
                {
                    result = _personnelDailyDutyRepository.Get(q => q.PersonnelId == personnelId
                        && (q.RequestAction == RequestAction.Accept || q.RequestAction == RequestAction.PartialAccept))
                        .AsEnumerable()
                        .Any(q => q.FromDate <= dailyDuty.FromDate
                            && q.ToDate >= dailyDuty.ToDate);
                }
                else if (hourlyDuty != null)
                {
                    result = _personnelHourlyDutyRepository.Get(q => q.PersonnelId == personnelId
                        && (q.RequestAction == RequestAction.Accept || q.RequestAction == RequestAction.PartialAccept))
                        .AsEnumerable()
                        .Any(q => q.Date == hourlyDuty.Date
                            && q.FromTime <= hourlyDuty.FromTime && q.ToTime >= hourlyDuty.ToTime);
                }
            }
            return result;
        }
        private async Task DeleteAndInsertAgain(UpdatePersonnelDutyDto dto)
        {
            var personnel = _personnelRepository.GetById(dto.PersonnelId);

            using (var scope = new TransactionScope())
            {
                _personnelDutyRepository.Delete(dto.Id, DeleteState.Permanent);
                await Create(new CreatePersonnelDutyDto
                {
                    DutyId = dto.DutyId,
                    DutyDuration = dto.DutyDuration,
                    FileUploadPath = dto.FileUploadPath,
                    RequestDescription = dto.RequestDescription
                }, personnel.Code);

                scope.Complete();
            }
        }
        private void PersonnelDutyDetailsError(RequestDuration dutyDuration, string operation)
        {
            try
            {
                throw new LogicalException();
            }
            catch (LogicalException ex)
            {
                _logger.LogRunTimeError(ex, "The PersonnelDuty dto that has passed to " +
                    "the service should also have '{0}' personnel duty properties." +
                    " {1} operation failed.", dutyDuration, operation);
                throw;
            }
        }
        #endregion
    }
}
