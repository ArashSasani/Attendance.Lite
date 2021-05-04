using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.PersonnelHourlyShift;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using CMS.Core.Interfaces;
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
    public class PersonnelHourlyShiftService : IPersonnelHourlyShiftService
    {
        private readonly IRepository<PersonnelHourlyShift> _personnelHourlyShiftRepository;
        private readonly IRepository<PersonnelShift> _personnelShiftRepository;
        private readonly IRepository<Personnel> _personnelRepository;
        private readonly IAuthRepository _authRepository;

        private readonly IExceptionLogger _logger;

        public PersonnelHourlyShiftService(IRepository<PersonnelHourlyShift> personnelHourlyShiftRepository
            , IRepository<PersonnelShift> personnelShiftRepository
            , IRepository<Personnel> personnelRepository
            , IAuthRepository authRepository
            , IExceptionLogger logger)
        {
            _personnelHourlyShiftRepository = personnelHourlyShiftRepository;
            _personnelShiftRepository = personnelShiftRepository;
            _personnelRepository = personnelRepository;
            _authRepository = authRepository;

            _logger = logger;
        }

        public IPaging<PersonnelHourlyShiftDtoForPaging> Get(int? hourlyShiftId, string searchTerm
            , string sortItem, string sortOrder, PagingQueryString pagingQueryString)
        {
            IPaging<PersonnelHourlyShiftDtoForPaging> model = new PersonnelHourlyShiftDtoPagingList();

            IQueryable<PersonnelHourlyShift> query = null;
            if (hourlyShiftId.HasValue)
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelHourlyShiftRepository.Get(q => q.HourlyShiftId == hourlyShiftId.Value
                    && q.HourlyShift.Title.Contains(searchTerm.ToLower())
                        || q.Personnel.Name.ToLower().Contains(searchTerm.ToLower())
                        || q.Personnel.LastName.ToLower().Contains(searchTerm.ToLower())
                    , includeProperties: "Personnel,HourlyShift")
                : _personnelHourlyShiftRepository.Get(q => q.HourlyShiftId == hourlyShiftId.Value
                    , includeProperties: "Personnel,HourlyShift");
            }
            else
            {
                query = !string.IsNullOrEmpty(searchTerm)
                ? _personnelHourlyShiftRepository.Get(q => q.HourlyShift.Title.Contains(searchTerm.ToLower())
                        || q.Personnel.Name.ToLower().Contains(searchTerm.ToLower())
                        || q.Personnel.LastName.ToLower().Contains(searchTerm.ToLower())
                    , includeProperties: "Personnel,HourlyShift")
                : _personnelHourlyShiftRepository.Get(includeProperties: "Personnel,HourlyShift");
            }

            //total number of items
            int queryCount = query.Count();
            switch (sortItem)
            {
                case "shift_title":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.HourlyShift.Title)
                        : query.OrderByDescending(o => o.HourlyShift.Title);
                    break;
                case "personnel_name":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Personnel.Name)
                        : query.OrderByDescending(o => o.Personnel.Name);
                    break;
                case "personnel_last_name":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Personnel.LastName)
                        : query.OrderByDescending(o => o.Personnel.LastName);
                    break;
                default:
                    query = query.OrderByDescending(o => o.Id);
                    break;
            }

            List<PersonnelHourlyShift> queryResult;
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
            model.PagingList = Mapper.Map<List<PersonnelHourlyShiftDtoForPaging>>(queryResult);

            return model;
        }

        public PersonnelHourlyShiftDto GetById(int id)
        {
            var personnelHourlyShift = _personnelHourlyShiftRepository.Get(q => q.Id == id
                , includeProperties: "Personnel,HourlyShift").SingleOrDefault();
            if (personnelHourlyShift == null)
            {
                return new PersonnelHourlyShiftDto();
            }
            return Mapper.Map<PersonnelHourlyShiftDto>(personnelHourlyShift);
        }

        public CustomResult Create(CreatePersonnelHourlyShiftDto dto)
        {
            if (dto.PersonnelIdList.Count == 0)
            {
                return new CustomResult
                {
                    Message = "please choose personnel"
                };
            }

            foreach (var personnelId in dto.PersonnelIdList)
            {
                if (HasNormalShift(personnelId))
                {
                    var person = _personnelRepository.GetById(personnelId);
                    return new CustomResult
                    {
                        Message = $"for person: {person.Name + " " + person.LastName}" +
                            $" normal shift has been assigned. cannot add hourly shift. request rejected"
                    };
                }
                if (SameShiftForSameDayExists(personnelId, dto.HourlyShiftId))
                {
                    return new CustomResult
                    {
                        Message = "cannot add the same shift for the same day"
                    };
                }

                var personnelHourlyShift = new PersonnelHourlyShift
                {
                    PersonnelId = personnelId,
                    HourlyShiftId = dto.HourlyShiftId,
                    DateAssigned = DateTime.Now
                };
                _personnelHourlyShiftRepository.Insert(personnelHourlyShift);
            }

            return new CustomResult
            {
                IsValid = true
            };
        }

        public void Update(UpdatePersonnelHourlyShiftDto dto)
        {
            var personnelHourlyShift = _personnelHourlyShiftRepository.GetById(dto.Id);
            if (personnelHourlyShift != null)
            {
                foreach (var personnelId in dto.PersonnelIdList)
                {
                    personnelHourlyShift.Id = dto.Id;
                    personnelHourlyShift.PersonnelId = personnelId;
                    personnelHourlyShift.HourlyShiftId = dto.HourlyShiftId;
                    personnelHourlyShift.DateAssigned = DateTime.Now;

                    _personnelHourlyShiftRepository.Update(personnelHourlyShift);
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
                        (ex, "PersonnelHourlyShift entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var personnelHourlyShift = _personnelHourlyShiftRepository.GetById(id);
            if (personnelHourlyShift != null)
            {
                _personnelHourlyShiftRepository.Delete(personnelHourlyShift, deleteState);
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
                        (ex, "PersonnelHourlyShift entity with the id: '{0}', is not available." +
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
                idsToRemove.ForEach(i => _personnelHourlyShiftRepository.Delete(i, DeleteState.SoftDelete));

                return idsToRemove.Count;
            }
            catch (LogicalException ex)
            {
                _logger.LogRunTimeError(ex, ex.Message);
                throw;
            }
        }

        #region Helpers

        private bool SameShiftForSameDayExists(int personnelId, int shiftId)
        {
            bool exists = _personnelHourlyShiftRepository
                    .Get(q => q.PersonnelId == personnelId
                        && q.HourlyShiftId == shiftId).Any();
            return exists;
        }

        private bool HasNormalShift(int personnelId)
        {
            return _personnelShiftRepository.Get(q => q.PersonnelId == personnelId
                && q.Shift.DeleteState != DeleteState.SoftDelete).Any();
        }
        #endregion
    }
}
