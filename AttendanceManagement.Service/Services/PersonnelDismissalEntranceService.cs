using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.PersonnelDismissalEntrance;
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
    public class PersonnelDismissalEntranceService : IPersonnelDismissalEntranceService
    {
        private readonly IRepository<Personnel> _personnelRepository;
        private readonly IRepository<PersonnelDismissalEntrance> _personnelDismissalEntranceRepository;

        private readonly IExceptionLogger _logger;

        public PersonnelDismissalEntranceService(IRepository<Personnel> personnelRepository
            , IRepository<PersonnelDismissalEntrance> personnelDismissalEntranceRepository
            , IExceptionLogger logger)
        {
            _personnelRepository = personnelRepository;
            _personnelDismissalEntranceRepository = personnelDismissalEntranceRepository;

            _logger = logger;
        }

        public IPaging<PersonnelDismissalEntranceDto> Get(string username, string fromDate, string toDate
            , string searchTerm, string sortItem, string sortOrder, PagingQueryString pagingQueryString)
        {
            IPaging<PersonnelDismissalEntranceDto> model = new PersonnelDismissalEntranceDtoPagingList();

            IQueryable<PersonnelDismissalEntrance> query = null;

            var personnel = _personnelRepository.Get(q => q.Code == username).SingleOrDefault();
            if (personnel != null)
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelDismissalEntranceRepository
                    .Get(q => q.PersonnelId == personnel.Id
                        && (q.Personnel.Name.Contains(searchTerm.ToLower())
                        || q.Personnel.LastName.Contains(searchTerm.ToLower()))
                    , includeProperties: "Personnel")
                : _personnelDismissalEntranceRepository.Get(q => q.PersonnelId == personnel.Id
                    , includeProperties: "Personnel");
            }
            else
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelDismissalEntranceRepository
                    .Get(q => q.Personnel.Name.Contains(searchTerm.ToLower()) ||
                        q.Personnel.LastName.Contains(searchTerm.ToLower())
                    , includeProperties: "Personnel")
                : _personnelDismissalEntranceRepository.Get(includeProperties: "Personnel");
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

            List<PersonnelDismissalEntrance> queryResult;
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
            model.PagingList = Mapper.Map<List<PersonnelDismissalEntranceDto>>(queryResult);

            return model;
        }

        public PersonnelDismissalEntranceDto GetById(int id)
        {
            var personnelDismissalEntrance = _personnelDismissalEntranceRepository.Get(q => q.Id == id
                , includeProperties: "Personnel").SingleOrDefault();
            if (personnelDismissalEntrance == null)
            {
                return null;
            }
            return Mapper.Map<PersonnelDismissalEntranceDto>(personnelDismissalEntrance);
        }

        public CustomResult Update(UpdatePersonnelDismissalEntranceDto dto)
        {
            var personnelDismissalEntrance = _personnelDismissalEntranceRepository.GetById(dto.Id);
            if (personnelDismissalEntrance != null)
            {
                var endDate = dto.EndDate;

                if (personnelDismissalEntrance.Start != dto.Start
                    || personnelDismissalEntrance.EndDate != endDate
                    || personnelDismissalEntrance.End != dto.End)
                {
                    if (endDate.HasValue
                        && endDate < personnelDismissalEntrance.StartDate.Date)
                    {
                        return new CustomResult
                        {
                            Message = "end date cannot be less that start date" +
                            $"{personnelDismissalEntrance.StartDate.ToShortDateString()}"
                        };
                    }

                    personnelDismissalEntrance.Start = dto.Start;
                    personnelDismissalEntrance.EndDate = endDate;
                    personnelDismissalEntrance.End = dto.End;
                    personnelDismissalEntrance.IsCompleted = dto.End.HasValue;

                    _personnelDismissalEntranceRepository.Update(personnelDismissalEntrance);
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
                        (ex, "PersonnelDismissalEntrance entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }

            return new CustomResult
            {
                IsValid = true
            };
        }

        public PartialUpdatePersonnelDismissalEntranceDto PrepareForPartialUpdate(int id)
        {
            var personnelDismissalEntrance = _personnelDismissalEntranceRepository.GetById(id);
            if (personnelDismissalEntrance != null)
            {
                return new PartialUpdatePersonnelDismissalEntranceDto
                {
                    PatchDto = Mapper.Map<PersonnelDismissalEntrancePatchDto>(personnelDismissalEntrance),
                    PersonnelDismissalEntranceEntity = personnelDismissalEntrance
                };
            }
            else
            {
                return null;
            }
        }

        public void ApplyPartialUpdate(PartialUpdatePersonnelDismissalEntranceDto dto)
        {
            dto.PersonnelDismissalEntranceEntity.Start = dto.PatchDto.Start;
            dto.PersonnelDismissalEntranceEntity.End = dto.PatchDto.End;

            dto.PersonnelDismissalEntranceEntity.IsCompleted = dto.PatchDto.End.HasValue;

            _personnelDismissalEntranceRepository.Update(dto.PersonnelDismissalEntranceEntity);
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var personnelDismissalEntrance = _personnelDismissalEntranceRepository.GetById(id);
            if (personnelDismissalEntrance != null)
            {
                _personnelDismissalEntranceRepository.Delete(personnelDismissalEntrance, deleteState);
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
                        (ex, "PersonnelDismissalEntrance entity with the id: '{0}', is not available." +
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
                idsToRemove.ForEach(i => _personnelDismissalEntranceRepository.Delete(i, DeleteState.SoftDelete));

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
