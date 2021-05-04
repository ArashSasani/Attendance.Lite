using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.PersonnelShiftReplacement;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using CMS.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class PersonnelShiftReplacementService : IPersonnelShiftReplacementService
    {
        private readonly IRepository<Personnel> _personnelRepository;
        private readonly IRepository<PersonnelShiftReplacement> _personnelShiftReplacementRepository;

        private readonly IAuthService _authService;
        private readonly IPersonnelShiftService _personnelShiftService;
        private readonly IApprovalProcService _approvalProcService;
        private readonly IMessageService _messageService;
        private readonly IRequestMessageHandlerService _requestMessageHandlerService;

        private readonly IExceptionLogger _logger;

        public PersonnelShiftReplacementService(
            IRepository<PersonnelShiftReplacement> personnelShiftReplacementRepository
            , IRepository<Personnel> personnelRepository
            , IAuthService authService
            , IPersonnelShiftService personnelShiftService
            , IApprovalProcService approvalProcService, IMessageService messageService
            , IRequestMessageHandlerService requestMessageHandlerService
            , IExceptionLogger logger)
        {
            _personnelRepository = personnelRepository;
            _personnelShiftReplacementRepository = personnelShiftReplacementRepository;

            _authService = authService;
            _personnelShiftService = personnelShiftService;
            _approvalProcService = approvalProcService;
            _messageService = messageService;
            _requestMessageHandlerService = requestMessageHandlerService;

            _logger = logger;
        }


        public IPaging<PersonnelShiftReplacementDtoForPaging> Get(string username
            , string searchTerm, string sortItem, string sortOrder, PagingQueryString pagingQueryString)
        {
            IPaging<PersonnelShiftReplacementDtoForPaging> model = new PersonnelShiftReplacementDtoPagingList();

            IQueryable<PersonnelShiftReplacement> query = null;

            var personnel = _personnelRepository.Get(q => q.Code == username).SingleOrDefault();
            if (personnel != null)
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelShiftReplacementRepository.Get(q => q.PersonnelId == personnel.Id
                        && (q.Personnel.Name.Contains(searchTerm.ToLower())
                        || q.Personnel.LastName.Contains(searchTerm.ToLower())
                        || q.ReplacedPersonnel.Name.Contains(searchTerm.ToLower())
                        || q.ReplacedPersonnel.LastName.Contains(searchTerm.ToLower())
                        || q.WorkingHour.Title.Contains(searchTerm.ToLower())
                        || q.ReplacedWorkingHour.Title.Contains(searchTerm.ToLower()))
                    , includeProperties: "Personnel,ReplacedPersonnel,WorkingHour,ReplacedWorkingHour")
                : _personnelShiftReplacementRepository
                    .Get(q => q.PersonnelId == personnel.Id
                    , includeProperties: "Personnel,ReplacedPersonnel,WorkingHour,ReplacedWorkingHour");
            }
            else
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelShiftReplacementRepository.Get(q => q.Personnel.Name.Contains(searchTerm.ToLower())
                        || q.Personnel.LastName.Contains(searchTerm.ToLower())
                        || q.ReplacedPersonnel.Name.Contains(searchTerm.ToLower())
                        || q.ReplacedPersonnel.LastName.Contains(searchTerm.ToLower())
                        || q.WorkingHour.Title.Contains(searchTerm.ToLower())
                        || q.ReplacedWorkingHour.Title.Contains(searchTerm.ToLower())
                    , includeProperties: "Personnel,ReplacedPersonnel,WorkingHour,ReplacedWorkingHour")
                : _personnelShiftReplacementRepository
                    .Get(includeProperties: "Personnel,ReplacedPersonnel,WorkingHour,ReplacedWorkingHour");
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
                case "working_hour_title":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.WorkingHour.Title)
                        : query.OrderByDescending(o => o.WorkingHour.Title);
                    break;
                default:
                    query = query.OrderByDescending(o => o.Id);
                    break;
            }

            List<PersonnelShiftReplacement> queryResult;
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
            model.PagingList = Mapper.Map<List<PersonnelShiftReplacementDtoForPaging>>
                (queryResult);


            return model;
        }

        public PersonnelShiftReplacementDto GetById(int id)
        {
            var personnelShiftReplacement = _personnelShiftReplacementRepository
                .Get(q => q.Id == id
                , includeProperties: "Personnel,ReplacedPersonnel,WorkingHour,ReplacedWorkingHour")
                .SingleOrDefault();
            if (personnelShiftReplacement == null)
            {
                return null;
            }
            return Mapper.Map<PersonnelShiftReplacementDto>(personnelShiftReplacement);
        }

        public List<ReplaceShiftDto> GetReplacedShifts(int personnelId, DateTime fromDate, DateTime toDate)
        {
            var replacedShift = _personnelShiftReplacementRepository
                .Get(q => q.ReplacementDate >= fromDate && q.ReplacementDate <= toDate
                    && q.ActionDate.HasValue && q.RequestAction == RequestAction.Accept
                    && (q.PersonnelId == personnelId || q.ReplacedPersonnelId == personnelId)
                    , includeProperties: "WorkingHour,ReplacedWorkingHour")
                .ToList();

            return Mapper.Map<List<ReplaceShiftDto>>(replacedShift);
        }

        public async Task<CustomResult<string>> Create(CreatePersonnelShiftReplacementDto dto
            , string username)
        {
            var user = await _authService.FindUserByUsernameAsync(username);
            if (user != null)
            {
                #region check if both personnel are available and have the same work unit
                var personnel = _personnelRepository.Get(q => q.Code == user.Username
                    , includeProperties: "Position").SingleOrDefault();
                if (personnel == null)
                {
                    return new CustomResult<string>
                    {
                        Message = "person info is not available"
                    };
                }
                var replacedPersonnel = _personnelRepository.Get(q => q.Id == dto.ReplacedPersonnelId
                    , includeProperties: "Position").SingleOrDefault();
                if (replacedPersonnel == null)
                {
                    return new CustomResult<string>
                    {
                        Message = "replaced person info is not available"
                    };
                }
                if (personnel.Position.WorkUnitId != replacedPersonnel.Position.WorkUnitId)
                {
                    return new CustomResult<string>
                    {
                        Message = "work unit for both personnel should be the same"
                    };
                }
                #endregion

                var replacementDate = dto.ReplacementDate;

                if (AlreadyRequested(personnel.Id, replacementDate))
                {
                    return new CustomResult<string>
                    {
                        Message = "cannot add multiple replacement in the same day"
                    };
                }

                #region check if replacement date is valid for any of the selected personnel
                var assignmentAvailable = _personnelShiftService.IsAssignmentAvailable(dto.ShiftId, replacementDate);
                if (!assignmentAvailable)
                {
                    return new CustomResult<string>
                    {
                        Message = "there is no shift for the personnel in the requested date"
                    };
                }
                #endregion

                //ok
                var personnelShiftReplacement = new PersonnelShiftReplacement
                {
                    PersonnelId = personnel.Id,
                    ReplacedPersonnelId = dto.ReplacedPersonnelId,
                    WorkingHourId = dto.WorkingHourId,
                    ReplacedWorkingHourId = dto.ReplacedWorkingHourId,
                    RequestedDate = DateTime.Now,
                    ReplacementDate = replacementDate
                };
                _personnelShiftReplacementRepository.Insert(personnelShiftReplacement);
                int personnelShiftReplacementId = personnelShiftReplacement.Id;

                //Send Request Message to Approval Proc
                var notificationReceiverId = await _requestMessageHandlerService
                    .HandleShiftReplacementRequest(personnelShiftReplacementId, user.Username);

                if (notificationReceiverId == null)
                {
                    return new CustomResult<string>
                    {
                        Message = "shift replacement procedure not found"
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

        public async Task<CustomResult<string>> Update(UpdatePersonnelShiftReplacementDto dto)
        {
            var personnelShiftReplacement = _personnelShiftReplacementRepository.GetById(dto.Id);
            if (personnelShiftReplacement != null)
            {
                dto.PersonnelId = personnelShiftReplacement.PersonnelId;

                #region check if both personnel are available and have the same work unit
                var personnel = _personnelRepository.Get(q => q.Id == dto.PersonnelId
                    , includeProperties: "Position").SingleOrDefault();
                if (personnel == null)
                {
                    return new CustomResult<string>
                    {
                        Message = "person info is not available"
                    };
                }
                var replacedPersonnel = _personnelRepository.Get(q => q.Id == dto.ReplacedPersonnelId
                    , includeProperties: "Position").SingleOrDefault();
                if (replacedPersonnel == null)
                {
                    return new CustomResult<string>
                    {
                        Message = "replaced person info is not available"
                    };
                }
                if (personnel.Position.WorkUnitId != replacedPersonnel.Position.WorkUnitId)
                {
                    return new CustomResult<string>
                    {
                        Message = "work unit for both personnel should be the same"
                    };
                }
                var user = await _authService.FindUserByUsernameAsync(personnel.Code);
                #endregion

                var replacementDate = dto.ReplacementDate;

                if (AlreadyRequested(personnel.Id, replacementDate, personnelShiftReplacement.Id))
                {
                    return new CustomResult<string>
                    {
                        Message = "cannot add multiple replacement in the same day"
                    };
                }

                #region check if replacement date is valid for any of the selected personnel
                var assignmentAvailable = _personnelShiftService.IsAssignmentAvailable(dto.ShiftId, replacementDate);
                if (!assignmentAvailable)
                {
                    return new CustomResult<string>
                    {
                        Message = "there is no shift for the personnel in the requested date"
                    };
                }
                #endregion

                //ok
                if (personnelShiftReplacement.RequestAction == RequestAction.Unknown)
                {
                    personnelShiftReplacement.PersonnelId = dto.PersonnelId;
                    personnelShiftReplacement.ReplacedPersonnelId = dto.ReplacedPersonnelId;
                    personnelShiftReplacement.WorkingHourId = dto.WorkingHourId;
                    personnelShiftReplacement.ReplacedWorkingHourId = dto.ReplacedWorkingHourId;
                    personnelShiftReplacement.ReplacementDate = replacementDate;

                    _personnelShiftReplacementRepository.Update(personnelShiftReplacement);

                    //Send Request Message to Approval Proc
                    //remove prev
                    _messageService.RemoveRequest(RequestType.ShiftReplacement, personnelShiftReplacement.Id);
                    //insert new
                    var notificationReceiverId = await _requestMessageHandlerService
                        .HandleShiftReplacementRequest(personnelShiftReplacement.Id, user.Username);

                    if (notificationReceiverId == null)
                    {
                        return new CustomResult<string>
                        {
                            Message = "shift replacement procedure not found"
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
                    string requestAction = personnelShiftReplacement.RequestAction == RequestAction.Accept
                        ? "confirmed" : personnelShiftReplacement.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : "rejected";
                    return new CustomResult<string>
                    {
                        IsValid = false,
                        Message = "cannot update the shift replacement info, status:"
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
                        (ex, "PersonnelShiftReplacement entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public async Task<CustomResult> Action(PersonnelShiftReplacementActionDto dto)
        {
            var personnelShiftReplacement = _personnelShiftReplacementRepository
                .Get(q => q.Id == dto.PersonnelShiftReplacementId, includeProperties: "Personnel")
                .SingleOrDefault();
            if (personnelShiftReplacement != null)
            {
                if (personnelShiftReplacement.ActionDate.HasValue
                    && personnelShiftReplacement.RequestAction != RequestAction.PartialAccept)
                {
                    return new CustomResult
                    {
                        Message = "your request is processed before"
                    };
                }

                personnelShiftReplacement.ActionDate = DateTime.Now;

                //still needs to confirm with superior in the proc
                if (dto.ParentApprovalProcId.HasValue)
                {
                    var receiverInfo = _approvalProcService
                        .GetNextReceiverId(dto.ParentApprovalProcId.Value);
                    if (dto.RequestAction == RequestAction.Accept)
                    {
                        await _requestMessageHandlerService.SubmitMessage(dto.MessageId, receiverInfo
                                , dto.RequestId, dto.RequestType, personnelShiftReplacement.RequestAction, personnelShiftReplacement.Personnel.Code);

                        personnelShiftReplacement.RequestAction = RequestAction.PartialAccept;
                        personnelShiftReplacement.ActionDescription = dto.ActionDescription;
                    }
                    else if (dto.RequestAction == RequestAction.Reject)
                    {
                        personnelShiftReplacement.RequestAction = RequestAction.Reject;
                        personnelShiftReplacement.ActionDescription = dto.ActionDescription;
                    }
                }
                else
                {
                    //no superior
                    personnelShiftReplacement.RequestAction = dto.RequestAction;
                    personnelShiftReplacement.ActionDescription = dto.ActionDescription;
                }

                _personnelShiftReplacementRepository.Update(personnelShiftReplacement);
                //update request message action
                _requestMessageHandlerService.UpdateRequestAction(dto.MessageId, personnelShiftReplacement.RequestAction);
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
                        (ex, "PersonnelShiftReplacement entity with the id: '{0}', is not available." +
                        " action operation failed.", dto.PersonnelShiftReplacementId);
                    throw;
                }
            }

            return new CustomResult
            {
                IsValid = true
            };
        }

        public async Task<CustomResult> Action(List<PersonnelShiftReplacementActionDto> dto)
        {
            foreach (var action in dto)
            {
                var personnelShiftReplacement = _personnelShiftReplacementRepository
                    .Get(q => q.Id == action.PersonnelShiftReplacementId, includeProperties: "Personnel")
                    .SingleOrDefault();
                if (personnelShiftReplacement != null)
                {
                    if (personnelShiftReplacement.ActionDate.HasValue
                    && personnelShiftReplacement.RequestAction != RequestAction.PartialAccept)
                    {
                        return new CustomResult
                        {
                            Message = "your request is processed before"
                        };
                    }

                    personnelShiftReplacement.ActionDate = DateTime.Now;

                    //still needs to confirm with superior in the proc
                    if (action.ParentApprovalProcId.HasValue)
                    {
                        var receiverInfo = _approvalProcService
                            .GetNextReceiverId(action.ParentApprovalProcId.Value);
                        if (action.RequestAction == RequestAction.Accept)
                        {
                            await _requestMessageHandlerService.SubmitMessage(action.MessageId, receiverInfo
                                , action.RequestId, action.RequestType, personnelShiftReplacement.RequestAction
                                , personnelShiftReplacement.Personnel.Code);

                            personnelShiftReplacement.RequestAction = RequestAction.PartialAccept;
                            personnelShiftReplacement.ActionDescription = action.ActionDescription;
                        }
                        else if (action.RequestAction == RequestAction.Reject)
                        {
                            personnelShiftReplacement.RequestAction = RequestAction.Reject;
                            personnelShiftReplacement.ActionDescription = action.ActionDescription;
                        }
                    }
                    else
                    {
                        //no superior
                        personnelShiftReplacement.RequestAction = action.RequestAction;
                        personnelShiftReplacement.ActionDescription = action.ActionDescription;
                    }

                    _personnelShiftReplacementRepository.Update(personnelShiftReplacement);
                    //update request message action
                    _requestMessageHandlerService.UpdateRequestAction(action.MessageId, personnelShiftReplacement.RequestAction);
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
                            (ex, "PersonnelShiftReplacement entity with the id: '{0}', is not available." +
                            " action operation failed.", action.PersonnelShiftReplacementId);
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
            var personnelShiftReplacement = _personnelShiftReplacementRepository.GetById(id);
            if (personnelShiftReplacement != null)
            {
                if (personnelShiftReplacement.RequestAction == RequestAction.Unknown)
                {
                    _personnelShiftReplacementRepository
                        .Delete(personnelShiftReplacement, deleteState);
                }
                else
                {
                    string requestAction = personnelShiftReplacement.RequestAction == RequestAction.Accept
                        ? "confirmed" : personnelShiftReplacement.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : "rejected";
                    return new CustomResult
                    {
                        IsValid = false,
                        Message = "cannot delete shift replacement info, status "
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
                        (ex, "PersonnelShiftReplacement entity with the id: '{0}', is not available." +
                        " delete operation failed.", id);
                    throw;
                }
            }

            return new CustomResult
            {
                IsValid = true
            };
        }

        #region helpers
        private bool AlreadyRequested(int personnelId, DateTime replacementDate, int? exceptionId = null)
        {
            if (exceptionId.HasValue)
            {
                return _personnelShiftReplacementRepository
                    .Get(q => q.Id != exceptionId && q.PersonnelId == personnelId)
                .AsEnumerable()
                .Any(q => q.ReplacementDate.Date == replacementDate.Date);
            }
            else
            {
                return _personnelShiftReplacementRepository.Get(q => q.PersonnelId == personnelId)
                .AsEnumerable()
                .Any(q => q.ReplacementDate.Date == replacementDate.Date);
            }
        }
        #endregion
    }
}
