using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.EmployeementType;
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
    public class EmployeementTypeService : IEmployeementTypeService
    {
        private readonly IRepository<EmployeementType> _employeementTypeRepository;

        private readonly IExceptionLogger _logger;

        public EmployeementTypeService(IRepository<EmployeementType> employeementTypeRepository
            , IExceptionLogger logger)
        {
            _employeementTypeRepository = employeementTypeRepository;

            _logger = logger;
        }


        public IPaging<EmployeementTypeDto> Get(string searchTerm, string sortItem, string sortOrder
            , PagingQueryString pagingQueryString)
        {
            IPaging<EmployeementTypeDto> model = new EmployeementTypeDtoPagingList();

            var query = !string.IsNullOrEmpty(searchTerm)
                ? _employeementTypeRepository.Get(q => q.Title.Contains(searchTerm.ToLower()))
                : _employeementTypeRepository.Get();
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

            List<EmployeementType> queryResult;
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
            model.PagingList = Mapper.Map<List<EmployeementTypeDto>>(queryResult);

            return model;
        }

        public List<EmployeementTypeDto> GetForDDL()
        {
            var employeementTypes = _employeementTypeRepository.Get().ToList();
            return Mapper.Map<List<EmployeementTypeDto>>(employeementTypes);
        }

        public EmployeementTypeDto GetById(int id)
        {
            var employeementType = _employeementTypeRepository.GetById(id);
            if (employeementType == null)
            {
                return null;
            }
            return Mapper.Map<EmployeementTypeDto>(employeementType);
        }

        public void Create(CreateEmployeementTypeDto dto)
        {
            var employeementType = new EmployeementType
            {
                Title = dto.Title
            };

            _employeementTypeRepository.Insert(employeementType);
        }

        public void Update(UpdateEmployeementTypeDto dto)
        {
            var employeementType = _employeementTypeRepository.GetById(dto.Id);
            if (employeementType != null)
            {
                employeementType.Title = dto.Title;

                _employeementTypeRepository.Update(employeementType);
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
                        (ex, "EmployeementType entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public PartialUpdateEmployeementTypeDto PrepareForPartialUpdate(int id)
        {
            var employeementType = _employeementTypeRepository.GetById(id);
            if (employeementType != null)
            {
                return new PartialUpdateEmployeementTypeDto
                {
                    PatchDto = Mapper.Map<EmployeementTypePatchDto>(employeementType),
                    EmployeementTypeEntity = employeementType
                };
            }
            else
            {
                return null;
            }
        }

        public void ApplyPartialUpdate(PartialUpdateEmployeementTypeDto dto)
        {
            dto.EmployeementTypeEntity.Title = dto.PatchDto.Title;

            _employeementTypeRepository.Update(dto.EmployeementTypeEntity);
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var employeementType = _employeementTypeRepository.GetById(id);
            if (employeementType != null)
            {
                _employeementTypeRepository.Delete(employeementType, deleteState);
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
                        (ex, "EmployeementType entity with the id: '{0}', is not available." +
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
                idsToRemove.ForEach(i => _employeementTypeRepository.Delete(i, DeleteState.SoftDelete));

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
