using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.WorkUnit;
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
    public class WorkUnitService : IWorkUnitService
    {
        private readonly IRepository<WorkUnit> _workUnitRepository;

        private readonly IExceptionLogger _logger;

        public WorkUnitService(IRepository<WorkUnit> workUnitRepository
            , IExceptionLogger logger)
        {
            _workUnitRepository = workUnitRepository;

            _logger = logger;
        }

        public IPaging<WorkUnitDto> Get(string searchTerm, string sortItem, string sortOrder
            , PagingQueryString pagingQueryString)
        {
            IPaging<WorkUnitDto> model = new WorkUnitDtoPagingList();

            var query = !string.IsNullOrEmpty(searchTerm)
                ? _workUnitRepository.Get(q => q.Title.Contains(searchTerm.ToLower()))
                : _workUnitRepository.Get();
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

            List<WorkUnit> queryResult;
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
            model.PagingList = Mapper.Map<List<WorkUnitDto>>(queryResult);

            return model;
        }

        public List<WorkUnitDto> GetForDDL()
        {
            var workUnits = _workUnitRepository.Get().ToList();
            return Mapper.Map<List<WorkUnitDto>>(workUnits);
        }

        public WorkUnitDto GetById(int id)
        {
            var workUnit = _workUnitRepository.GetById(id);
            if (workUnit == null)
            {
                return null;
            }
            return Mapper.Map<WorkUnitDto>(workUnit);
        }

        public void Create(CreateWorkUnitDto dto)
        {
            var workUnit = new WorkUnit
            {
                Title = dto.Title
            };

            _workUnitRepository.Insert(workUnit);
        }

        public void Update(UpdateWorkUnitDto dto)
        {
            var workUnit = _workUnitRepository.GetById(dto.Id);
            if (workUnit != null)
            {
                workUnit.Title = dto.Title;

                _workUnitRepository.Update(workUnit);
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
                        (ex, "WorkUnit entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public PartialUpdateWorkUnitDto PrepareForPartialUpdate(int id)
        {
            var workUnit = _workUnitRepository.GetById(id);
            if (workUnit != null)
            {
                return new PartialUpdateWorkUnitDto
                {
                    PatchDto = Mapper.Map<WorkUnitPatchDto>(workUnit),
                    WorkUnitEntity = workUnit
                };
            }
            else
            {
                return null;
            }
        }

        public void ApplyPartialUpdate(PartialUpdateWorkUnitDto dto)
        {
            dto.WorkUnitEntity.Title = dto.PatchDto.Title;

            _workUnitRepository.Update(dto.WorkUnitEntity);
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var workUnit = _workUnitRepository.GetById(id);
            if (workUnit != null)
            {
                _workUnitRepository.Delete(workUnit, deleteState);
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
                        (ex, "WorkUnit entity with the id: '{0}', is not available." +
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
                idsToRemove.ForEach(i => _workUnitRepository.Delete(i, DeleteState.SoftDelete));

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
