using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.PersonnelEntrance;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.Infrastructure.Parser;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class PersonnelEntranceService : IPersonnelEntranceService
    {
        private readonly IRepository<Personnel> _personnelRepository;
        private readonly IRepository<PersonnelEntrance> _personnelEntranceRepository;

        private readonly IExceptionLogger _logger;

        public PersonnelEntranceService(IRepository<Personnel> personnelRepository
            , IRepository<PersonnelEntrance> personnelEntranceRepository
            , IExceptionLogger logger)
        {
            _personnelRepository = personnelRepository;
            _personnelEntranceRepository = personnelEntranceRepository;

            _logger = logger;
        }

        public IPaging<PersonnelEntranceDto> Get(string username, string fromDate, string toDate
            , EntranceType entranceType, string searchTerm, string sortItem, string sortOrder
            , PagingQueryString pagingQueryString)
        {
            IPaging<PersonnelEntranceDto> model = new PersonnelEntranceDtoPagingList();

            IQueryable<PersonnelEntrance> query = null;

            var personnel = _personnelRepository.Get(q => q.Code == username).SingleOrDefault();
            if (personnel != null)
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelEntranceRepository
                    .Get(q => q.PersonnelId == personnel.Id
                        && (q.Personnel.Name.Contains(searchTerm.ToLower())
                        || q.Personnel.LastName.Contains(searchTerm.ToLower()))
                    , includeProperties: "Personnel")
                : _personnelEntranceRepository.Get(q => q.PersonnelId == personnel.Id
                    , includeProperties: "Personnel");
            }
            else
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelEntranceRepository
                    .Get(q => q.Personnel.Name.Contains(searchTerm.ToLower()) ||
                        q.Personnel.LastName.Contains(searchTerm.ToLower())
                    , includeProperties: "Personnel")
                : _personnelEntranceRepository.Get(includeProperties: "Personnel");
            }

            if (!string.IsNullOrEmpty(fromDate))
            {
                var fromDateConverted = DateTime.Parse(fromDate);
                query = query.Where(q => q.EnterDate >= fromDateConverted);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                var toDateConverted = DateTime.Parse(toDate);
                query = query.Where(q => q.EnterDate <= toDateConverted);
            }

            switch (entranceType)
            {
                case EntranceType.All:
                    //default
                    break;
                case EntranceType.Present:
                    query = query.Where(q => !q.AutoEnter || !q.AutoExit);
                    break;
                case EntranceType.Absent:
                    query = query.Where(q => q.AutoEnter && q.AutoExit);
                    break;
            }

            //total number of items
            int queryCount = query.Count();
            switch (sortItem)
            {
                case "personnel_full_name":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Personnel.Name + " " + o.Personnel.LastName)
                        : query.OrderByDescending(o => o.Personnel.Name + " " + o.Personnel.LastName);
                    break;
                case "personnel_code":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Personnel.Code)
                        : query.OrderByDescending(o => o.Personnel.LastName);
                    break;
                case "entrance_date":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.EnterDate).ThenBy(o => o.Enter)
                        : query.OrderByDescending(o => o.EnterDate).ThenByDescending(o => o.Enter);
                    break;
                default:
                    query = query.OrderByDescending(o => o.EnterDate).ThenByDescending(o => o.Enter);
                    break;
            }

            List<PersonnelEntrance> queryResult;
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
            model.PagingList = Mapper.Map<List<PersonnelEntranceDto>>(queryResult);

            return model;
        }

        public PersonnelEntranceDto GetById(int id)
        {
            var personnelEntrance = _personnelEntranceRepository.Get(q => q.Id == id
                , includeProperties: "Personnel").SingleOrDefault();
            if (personnelEntrance == null)
            {
                return null;
            }
            return Mapper.Map<PersonnelEntranceDto>(personnelEntrance);
        }

        public CustomResult Update(UpdatePersonnelEntranceDto dto)
        {
            var personnelEntrance = _personnelEntranceRepository.GetById(dto.Id);
            if (personnelEntrance != null)
            {
                var exitDate = dto.ExitDate;

                if (personnelEntrance.Enter != dto.Enter
                    || personnelEntrance.ExitDate != exitDate
                    || personnelEntrance.Exit != dto.Exit)
                {
                    if (exitDate.HasValue
                        && exitDate < personnelEntrance.EnterDate.Date)
                    {
                        return new CustomResult
                        {
                            Message = "exit date cannot be less that enter date" +
                            $"{personnelEntrance.EnterDate.ToShortDateString()}"
                        };
                    }

                    personnelEntrance.AutoEnter = personnelEntrance.Enter != dto.Enter
                        ? false : personnelEntrance.AutoEnter;
                    personnelEntrance.Enter = dto.Enter;
                    personnelEntrance.ExitDate = exitDate;
                    personnelEntrance.AutoExit =
                        (personnelEntrance.Exit != dto.Exit || personnelEntrance.ExitDate != exitDate)
                        ? false : personnelEntrance.AutoExit;
                    personnelEntrance.Exit = dto.Exit;
                    personnelEntrance.IsCompleted = dto.Exit.HasValue ? true : false;
                    personnelEntrance.IsEdited = true;
                    personnelEntrance.EditDate = DateTime.Now;

                    _personnelEntranceRepository.Update(personnelEntrance);
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
                        (ex, "PersonnelEntrance entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }

            return new CustomResult
            {
                IsValid = true
            };
        }

        public PartialUpdatePersonnelEntranceDto PrepareForPartialUpdate(int id)
        {
            var personnelEntrance = _personnelEntranceRepository.GetById(id);
            if (personnelEntrance != null)
            {
                return new PartialUpdatePersonnelEntranceDto
                {
                    PatchDto = Mapper.Map<PersonnelEntrancePatchDto>(personnelEntrance),
                    PersonnelEntranceEntity = personnelEntrance
                };
            }
            else
            {
                return null;
            }
        }

        public void ApplyPartialUpdate(PartialUpdatePersonnelEntranceDto dto)
        {
            dto.PersonnelEntranceEntity.AutoEnter =
                dto.PersonnelEntranceEntity.Enter != dto.PatchDto.Enter
                ? false : dto.PersonnelEntranceEntity.AutoEnter;
            dto.PersonnelEntranceEntity.AutoExit =
                dto.PersonnelEntranceEntity.Exit != dto.PatchDto.Exit
                ? false : dto.PersonnelEntranceEntity.AutoExit;
            dto.PersonnelEntranceEntity.Enter = dto.PatchDto.Enter;
            dto.PersonnelEntranceEntity.Exit = dto.PatchDto.Exit;

            dto.PersonnelEntranceEntity.IsCompleted = dto.PatchDto.Exit.HasValue ? true : false;
            dto.PersonnelEntranceEntity.IsEdited = true;
            dto.PersonnelEntranceEntity.EditDate = DateTime.Now;

            _personnelEntranceRepository.Update(dto.PersonnelEntranceEntity);
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var personnelEntrance = _personnelEntranceRepository.GetById(id);
            if (personnelEntrance != null)
            {
                _personnelEntranceRepository.Delete(personnelEntrance, deleteState);
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
                        (ex, "PersonnelEntrance entity with the id: '{0}', is not available." +
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
                idsToRemove.ForEach(i => _personnelEntranceRepository.Delete(i, DeleteState.SoftDelete));

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
