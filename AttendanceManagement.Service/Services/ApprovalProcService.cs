using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.ApprovalProc;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using CMS.Core.Interfaces;
using CMS.Core.Model;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.Infrastructure.Parser;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class ApprovalProcService : IApprovalProcService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRepository<ApprovalProc> _approvalProcRepository;

        private readonly IExceptionLogger _logger;

        public ApprovalProcService(IAuthRepository authRepository
            , IRepository<ApprovalProc> approvalProcRepository
            , IExceptionLogger logger)
        {
            _authRepository = authRepository;
            _approvalProcRepository = approvalProcRepository;

            _logger = logger;
        }

        public IPaging<ApprovalProcDtoForPaging> Get(string searchTerm, string sortItem
            , string sortOrder, PagingQueryString pagingQueryString)
        {
            IPaging<ApprovalProcDtoForPaging> model = new ApprovalProcDtoPagingList();

            var query = !string.IsNullOrEmpty(searchTerm)
                ? _approvalProcRepository.Get(q => q.Title.Contains(searchTerm.ToLower())
                    , includeProperties: "ParentProc")
                : _approvalProcRepository.Get(includeProperties: "ParentProc");

            //total number of items
            int queryCount = query.Count();
            switch (sortItem)
            {
                case "title":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Title)
                        : query.OrderByDescending(o => o.Title);
                    break;
                case "parent_title":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.ParentProc.Title)
                        : query.OrderByDescending(o => o.ParentProc.Title);
                    break;
                default:
                    query = query.OrderByDescending(o => o.Id);
                    break;
            }

            List<ApprovalProc> queryResult;
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
            model.PagingList = Mapper.Map<List<ApprovalProcDtoForPaging>>(queryResult);

            return model;
        }

        public ApprovalProcDto GetById(int id)
        {
            var approvalProc = _approvalProcRepository.Get(q => q.Id == id
                , includeProperties: "ParentProc,FirstPriority,SecondPriority,ThirdPriority")
                .SingleOrDefault();
            if (approvalProc == null)
            {
                return null;
            }
            return Mapper.Map<ApprovalProcDto>(approvalProc);
        }

        public List<ApprovalProcDtoForDDL> GetForDDL(int? exceptionId)
        {
            var approvalProcs = new List<ApprovalProc>();
            if (exceptionId.HasValue)
            {
                approvalProcs = _approvalProcRepository.Get(q => q.Id != exceptionId.Value)
                    .ToList();
            }
            else
            {
                approvalProcs = _approvalProcRepository.Get()
                    .ToList();
            }

            return Mapper.Map<List<ApprovalProcDtoForDDL>>(approvalProcs);
        }

        public ReceiverInfoDto GetNextReceiverId(ApprovalProc approvalProc)
        {
            User nextReceiver = null;
            string receiverId = null;
            int? parentProcId = null;

            if (approvalProc != null)
            {
                //first person on the approval proc
                var firstPriority = approvalProc.FirstPriority;
                if (firstPriority.IsPresent)
                    nextReceiver = _authRepository.FindUserByUsername(firstPriority.Code);
                else
                {
                    //second person on the approval proc
                    var secondPriority = approvalProc.SecondPriority;
                    if (secondPriority.IsPresent)
                        nextReceiver = _authRepository.FindUserByUsername(secondPriority.Code);
                    else
                    {
                        //third person on the approval proc
                        var thirdPriority = approvalProc.ThirdPriority;
                        if (thirdPriority.IsPresent)
                            nextReceiver = _authRepository.FindUserByUsername(thirdPriority.Code);
                    }
                }
                if (nextReceiver == null && approvalProc.ParentProc != null)
                {
                    GetNextReceiverId(approvalProc.ParentProc);
                }
                else
                {
                    receiverId = nextReceiver.Id;
                    parentProcId = approvalProc.ParentId;
                }
            }
            return new ReceiverInfoDto
            {
                ReceiverId = receiverId,
                ParentApprovalProcId = parentProcId
            };
        }

        public ReceiverInfoDto GetNextReceiverId(int ParentApprovalProcId)
        {
            var parentProc = _approvalProcRepository.GetById(ParentApprovalProcId);
            return GetNextReceiverId(parentProc);
        }

        public void Create(CreateApprovalProcDto dto)
        {
            var approvalProc = new ApprovalProc
            {
                Title = dto.Title,
                ParentId = dto.ParentId,
                FirstPriorityId = dto.FirstPriorityId,
                SecondPriorityId = dto.SecondPriorityId,
                ThirdPriorityId = dto.ThirdPriorityId,
                ActiveState = dto.ActiveState
            };

            _approvalProcRepository.Insert(approvalProc);
        }

        public void Update(UpdateApprovalProcDto dto)
        {
            var approvalProc = _approvalProcRepository.GetById(dto.Id);
            if (approvalProc != null)
            {
                approvalProc.Title = dto.Title;
                approvalProc.ParentId = dto.ParentId;
                approvalProc.FirstPriorityId = dto.FirstPriorityId;
                approvalProc.SecondPriorityId = dto.SecondPriorityId;
                approvalProc.ThirdPriorityId = dto.ThirdPriorityId;
                approvalProc.ActiveState = dto.ActiveState;

                _approvalProcRepository.Update(approvalProc);
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
                        (ex, "ApprovalProc entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public PartialUpdateApprovalProcDto PrepareForPartialUpdate(int id)
        {
            var approvalProc = _approvalProcRepository.GetById(id);
            if (approvalProc != null)
            {
                return new PartialUpdateApprovalProcDto
                {
                    PatchDto = Mapper.Map<ApprovalProcPatchDto>(approvalProc),
                    ApprovalProcEntity = approvalProc
                };
            }
            else
            {
                return null;
            }
        }

        public void ApplyPartialUpdate(PartialUpdateApprovalProcDto dto)
        {
            dto.ApprovalProcEntity.Title = dto.PatchDto.Title;

            _approvalProcRepository.Update(dto.ApprovalProcEntity);
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var approvalProc = _approvalProcRepository.GetById(id);
            if (approvalProc != null)
            {
                _approvalProcRepository.Delete(approvalProc, deleteState);
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
                        (ex, "ApprovalProc entity with the id: '{0}', is not available." +
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
                idsToRemove.ForEach(i => _approvalProcRepository.Delete(i, DeleteState.SoftDelete));

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
