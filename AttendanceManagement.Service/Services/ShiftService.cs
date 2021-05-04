using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.Shift;
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
    public class ShiftService : IShiftService
    {
        private readonly IRepository<Shift> _shiftRepository;

        private readonly IExceptionLogger _logger;

        public ShiftService(IRepository<Shift> shiftRepository
            , IExceptionLogger logger)
        {
            _shiftRepository = shiftRepository;

            _logger = logger;
        }

        public IPaging<ShiftDto> Get(string searchTerm, string sortItem
            , string sortOrder, PagingQueryString pagingQueryString)
        {
            IPaging<ShiftDto> model = new ShiftDtoPagingList();

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

            List<Shift> queryResult;
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
            model.PagingList = Mapper.Map<List<ShiftDto>>(queryResult);

            return model;
        }

        public ShiftDto GetById(int id)
        {
            var shift = _shiftRepository.GetById(id);
            if (shift == null)
            {
                return null;
            }
            return Mapper.Map<ShiftDto>(shift);
        }

        public List<ShiftDDLDto> GetForDDL()
        {
            var shifts = _shiftRepository.Get().ToList();
            return Mapper.Map<List<ShiftDDLDto>>(shifts);
        }

        public void Create(CreateShiftDto dto)
        {
            var shift = new Shift
            {
                Title = dto.Title
            };

            _shiftRepository.Insert(shift);
        }

        public void Update(UpdateShiftDto dto)
        {
            var shift = _shiftRepository.GetById(dto.Id);
            if (shift != null)
            {
                shift.Title = dto.Title;

                _shiftRepository.Update(shift);
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
                        (ex, "Shift entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public PartialUpdateShiftDto PrepareForPartialUpdate(int id)
        {
            var shift = _shiftRepository.GetById(id);
            if (shift != null)
            {
                return new PartialUpdateShiftDto
                {
                    PatchDto = Mapper.Map<ShiftPatchDto>(shift),
                    ShiftEntity = shift
                };
            }
            else
            {
                return null;
            }
        }

        public void ApplyPartialUpdate(PartialUpdateShiftDto dto)
        {
            dto.ShiftEntity.Title = dto.PatchDto.Title;

            _shiftRepository.Update(dto.ShiftEntity);
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var shift = _shiftRepository.GetById(id);
            if (shift != null)
            {
                _shiftRepository.Delete(shift, deleteState);
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
                        (ex, "Shift entity with the id: '{0}', is not available." +
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
