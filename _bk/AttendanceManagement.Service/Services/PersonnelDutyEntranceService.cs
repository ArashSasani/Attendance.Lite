using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.PersonnelDutyEntrance;
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
    public class PersonnelDutyEntranceService : IPersonnelDutyEntranceService
    {
        private readonly IRepository<Personnel> _personnelRepository;
        private readonly IRepository<PersonnelDutyEntrance> _personnelDutyEntranceRepository;

        private readonly IExceptionLogger _logger;

        public PersonnelDutyEntranceService(IRepository<Personnel> personnelRepository
            , IRepository<PersonnelDutyEntrance> personnelDutyEntranceRepository
            , IExceptionLogger logger)
        {
            _personnelRepository = personnelRepository;
            _personnelDutyEntranceRepository = personnelDutyEntranceRepository;

            _logger = logger;
        }

        public IPaging<PersonnelDutyEntranceDto> Get(string username, string fromDate, string toDate
            , string searchTerm, string sortItem, string sortOrder, PagingQueryString pagingQueryString)
        {
            IPaging<PersonnelDutyEntranceDto> model = new PersonnelDutyEntranceDtoPagingList();

            IQueryable<PersonnelDutyEntrance> query = null;

            var personnel = _personnelRepository.Get(q => q.Code == username).SingleOrDefault();
            if (personnel != null)
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelDutyEntranceRepository
                    .Get(q => q.PersonnelId == personnel.Id
                        && (q.Personnel.Name.Contains(searchTerm.ToLower())
                        || q.Personnel.LastName.Contains(searchTerm.ToLower()))
                    , includeProperties: "Personnel")
                : _personnelDutyEntranceRepository.Get(q => q.PersonnelId == personnel.Id
                    , includeProperties: "Personnel");
            }
            else
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelDutyEntranceRepository
                    .Get(q => q.Personnel.Name.Contains(searchTerm.ToLower()) ||
                        q.Personnel.LastName.Contains(searchTerm.ToLower())
                    , includeProperties: "Personnel")
                : _personnelDutyEntranceRepository.Get(includeProperties: "Personnel");
            }

            if (!string.IsNullOrEmpty(fromDate))
            {
                var fromDateConverted = DateTime.Parse(fromDate);
                query = query.Where(q => q.StartDate >= fromDateConverted);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                var toDateConverted = DateTime.Parse(toDate);
                query = query.Where(q => q.StartDate <= toDateConverted);
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
                case "enter":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.StartDate).ThenBy(o => o.Start)
                        : query.OrderByDescending(o => o.StartDate).ThenByDescending(o => o.Start);
                    break;
                default:
                    query = query.OrderByDescending(o => o.StartDate).ThenByDescending(o => o.Start);
                    break;
            }

            List<PersonnelDutyEntrance> queryResult;
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
            model.PagingList = Mapper.Map<List<PersonnelDutyEntranceDto>>(queryResult);

            return model;
        }

        public PersonnelDutyEntranceDto GetById(int id)
        {
            var personnelDutyEntrance = _personnelDutyEntranceRepository.Get(q => q.Id == id
                , includeProperties: "Personnel").SingleOrDefault();
            if (personnelDutyEntrance == null)
            {
                return null;
            }
            return Mapper.Map<PersonnelDutyEntranceDto>(personnelDutyEntrance);
        }

        public CustomResult Update(UpdatePersonnelDutyEntranceDto dto)
        {
            var personnelDutyEntrance = _personnelDutyEntranceRepository.GetById(dto.Id);
            if (personnelDutyEntrance != null)
            {
                var endDate = dto.EndDate;

                if (personnelDutyEntrance.Start != dto.Start
                    || personnelDutyEntrance.EndDate != endDate
                    || personnelDutyEntrance.End != dto.End)
                {
                    if (endDate.HasValue
                        && endDate < personnelDutyEntrance.StartDate.Date)
                    {
                        return new CustomResult
                        {
                            Message = "end date cannot be less that start date" +
                            $"{personnelDutyEntrance.StartDate.ToShortDateString()}"
                        };
                    }

                    personnelDutyEntrance.Start = dto.Start;
                    personnelDutyEntrance.EndDate = endDate;
                    personnelDutyEntrance.End = dto.End;
                    personnelDutyEntrance.IsCompleted = dto.End.HasValue;

                    _personnelDutyEntranceRepository.Update(personnelDutyEntrance);
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
                        (ex, "PersonnelDutyEntrance entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }

            return new CustomResult
            {
                IsValid = true
            };
        }

        public PartialUpdatePersonnelDutyEntranceDto PrepareForPartialUpdate(int id)
        {
            var personnelDutyEntrance = _personnelDutyEntranceRepository.GetById(id);
            if (personnelDutyEntrance != null)
            {
                return new PartialUpdatePersonnelDutyEntranceDto
                {
                    PatchDto = Mapper.Map<PersonnelDutyEntrancePatchDto>(personnelDutyEntrance),
                    PersonnelDutyEntranceEntity = personnelDutyEntrance
                };
            }
            else
            {
                return null;
            }
        }

        public void ApplyPartialUpdate(PartialUpdatePersonnelDutyEntranceDto dto)
        {
            dto.PersonnelDutyEntranceEntity.Start = dto.PatchDto.Start;
            dto.PersonnelDutyEntranceEntity.End = dto.PatchDto.End;

            dto.PersonnelDutyEntranceEntity.IsCompleted = dto.PatchDto.End.HasValue;

            _personnelDutyEntranceRepository.Update(dto.PersonnelDutyEntranceEntity);
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var personnelDutyEntrance = _personnelDutyEntranceRepository.GetById(id);
            if (personnelDutyEntrance != null)
            {
                _personnelDutyEntranceRepository.Delete(personnelDutyEntrance, deleteState);
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
                        (ex, "PersonnelDutyEntrance entity with the id: '{0}', is not available." +
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
                idsToRemove.ForEach(i => _personnelDutyEntranceRepository.Delete(i, DeleteState.SoftDelete));

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
