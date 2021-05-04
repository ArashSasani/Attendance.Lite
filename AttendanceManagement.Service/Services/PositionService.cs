using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.Position;
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
    public class PositionService : IPositionService
    {
        private readonly IRepository<Position> _positionRepository;

        private readonly IExceptionLogger _logger;

        public PositionService(IRepository<Position> positionRepository
            , IExceptionLogger logger)
        {
            _positionRepository = positionRepository;

            _logger = logger;
        }

        public IPaging<PositionDto> Get(string searchTerm, string sortItem
            , string sortOrder, PagingQueryString pagingQueryString)
        {
            IPaging<PositionDto> model = new PositionDtoPagingList();

            var query = !string.IsNullOrEmpty(searchTerm)
                ? _positionRepository.Get(q => q.Title.Contains(searchTerm.ToLower())
                    || q.WorkUnit.Title.Contains(searchTerm.ToLower()), includeProperties: "WorkUnit")
                : _positionRepository.Get(includeProperties: "WorkUnit");

            //total number of items
            int queryCount = query.Count();
            switch (sortItem)
            {
                case "title":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Title)
                        : query.OrderByDescending(o => o.Title);
                    break;
                case "work_uni_title":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.WorkUnit.Title)
                        : query.OrderByDescending(o => o.WorkUnit.Title);
                    break;
                default:
                    query = query.OrderByDescending(o => o.Id);
                    break;
            }

            List<Position> queryResult;
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
            model.PagingList = Mapper.Map<List<PositionDto>>(queryResult);

            return model;
        }

        public List<PositionDDLDto> GetForDDL(int workUnitId)
        {
            var positions = _positionRepository.Get(q => q.WorkUnitId == workUnitId).ToList();
            return Mapper.Map<List<PositionDDLDto>>(positions);
        }

        public PositionDto GetById(int id)
        {
            var position = _positionRepository.Get(q => q.Id == id, includeProperties: "WorkUnit")
                .SingleOrDefault();
            if (position == null)
            {
                return null;
            }
            return Mapper.Map<PositionDto>(position);
        }

        public void Create(CreatePositionDto dto)
        {
            var position = new Position
            {
                WorkUnitId = dto.WorkUnitId,
                Title = dto.Title
            };

            _positionRepository.Insert(position);
        }

        public void Update(UpdatePositionDto dto)
        {
            var position = _positionRepository.GetById(dto.Id);
            if (position != null)
            {
                position.WorkUnitId = dto.WorkUnitId;
                position.Title = dto.Title;

                _positionRepository.Update(position);
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
                        (ex, "Position entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public PartialUpdatePositionDto PrepareForPartialUpdate(int id)
        {
            var position = _positionRepository.GetById(id);
            if (position != null)
            {
                return new PartialUpdatePositionDto
                {
                    PatchDto = Mapper.Map<PositionPatchDto>(position),
                    PositionEntity = position
                };
            }
            else
            {
                return null;
            }
        }

        public void ApplyPartialUpdate(PartialUpdatePositionDto dto)
        {
            dto.PositionEntity.Title = dto.PatchDto.Title;

            _positionRepository.Update(dto.PositionEntity);
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var position = _positionRepository.GetById(id);
            if (position != null)
            {
                _positionRepository.Delete(position, deleteState);
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
                        (ex, "Position entity with the id: '{0}', is not available." +
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
                idsToRemove.ForEach(i => _positionRepository.Delete(i, DeleteState.SoftDelete));

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
