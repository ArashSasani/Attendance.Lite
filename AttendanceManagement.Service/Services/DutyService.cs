using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.Duty;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
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
    public class DutyService : IDutyService
    {
        private readonly IRepository<Duty> _dutyRepository;

        private readonly IExceptionLogger _logger;

        public DutyService(IRepository<Duty> dutyRepository
            , IExceptionLogger logger)
        {
            _dutyRepository = dutyRepository;

            _logger = logger;
        }

        public IPaging<DutyDto> Get(string searchTerm, string sortItem
            , string sortOrder, PagingQueryString pagingQueryString)
        {
            IPaging<DutyDto> model = new DutyDtoPagingList();

            var query = !string.IsNullOrEmpty(searchTerm)
                ? _dutyRepository.Get(q => q.Title.Contains(searchTerm.ToLower()))
                : _dutyRepository.Get();

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

            List<Duty> queryResult;
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
            model.PagingList = Mapper.Map<List<DutyDto>>(queryResult);

            return model;
        }

        public DutyDto GetById(int id)
        {
            var duty = _dutyRepository.GetById(id);
            if (duty == null)
            {
                return null;
            }
            return Mapper.Map<DutyDto>(duty);
        }

        public List<DutyDto> GetForDDL()
        {
            var duties = _dutyRepository.Get().ToList();
            return Mapper.Map<List<DutyDto>>(duties);
        }

        public void Create(CreateDutyDto dto)
        {
            var duty = new Duty
            {
                Title = dto.Title,
                ActionLimitDays = dto.ActionLimitDays
            };

            _dutyRepository.Insert(duty);
        }

        public void Update(UpdateDutyDto dto)
        {
            var duty = _dutyRepository.GetById(dto.Id);
            if (duty != null)
            {
                duty.Title = dto.Title;
                duty.ActionLimitDays = dto.ActionLimitDays;

                _dutyRepository.Update(duty);
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
                        (ex, "Duty entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public PartialUpdateDutyDto PrepareForPartialUpdate(int id)
        {
            var duty = _dutyRepository.GetById(id);
            if (duty != null)
            {
                return new PartialUpdateDutyDto
                {
                    PatchDto = Mapper.Map<DutyPatchDto>(duty),
                    DutyEntity = duty
                };
            }
            else
            {
                return null;
            }
        }

        public void ApplyPartialUpdate(PartialUpdateDutyDto dto)
        {
            dto.DutyEntity.Title = dto.PatchDto.Title;

            _dutyRepository.Update(dto.DutyEntity);
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var duty = _dutyRepository.GetById(id);
            if (duty != null)
            {
                _dutyRepository.Delete(duty, deleteState);
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
                        (ex, "Duty entity with the id: '{0}', is not available." +
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
                idsToRemove.ForEach(i => _dutyRepository.Delete(i, DeleteState.SoftDelete));

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
