using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.DismissalApproval;
using AttendanceManagement.Service.Dtos.DutyApproval;
using AttendanceManagement.Service.Dtos.Personnel;
using AttendanceManagement.Service.Dtos.PersonnelApprovalProc;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using CMS.Service.Dtos.User;
using CMS.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.Infrastructure.Parser;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class PersonnelService : IPersonnelService
    {
        private readonly IRepository<Personnel> _personnelRepository;

        private readonly IAuthService _authService;
        private readonly IPersonnelApprovalProcService _personnelApprovalProcService;
        private readonly IDismissalApprovalService _dismissalApprovalService;
        private readonly IDutyApprovalService _dutyApprovalService;

        private readonly IExceptionLogger _logger;

        public PersonnelService(IRepository<Personnel> personnelRepository
            , IAuthService authService, IPersonnelApprovalProcService personnelApprovalProcService
            , IDismissalApprovalService dismissalApprovalService, IDutyApprovalService dutyApprovalService
            , IExceptionLogger logger)
        {
            _personnelRepository = personnelRepository;

            _authService = authService;
            _personnelApprovalProcService = personnelApprovalProcService;
            _dismissalApprovalService = dismissalApprovalService;
            _dutyApprovalService = dutyApprovalService;

            _logger = logger;
        }

        public IPaging<PersonnelDtoForPaging> Get(string username, string searchTerm, string sortItem, string sortOrder
            , PagingQueryString pagingQueryString)
        {
            IPaging<PersonnelDtoForPaging> model = new PersonnelDtoPagingList();

            IQueryable<Personnel> query = null;

            var personnel = _personnelRepository.Get(q => q.Code == username).SingleOrDefault();
            if (personnel != null)
            {
                query = !string.IsNullOrEmpty(searchTerm)
                    ? _personnelRepository.Get(q => q.Id == personnel.Id
                        && (q.Name.Contains(searchTerm.ToLower())
                        || q.LastName.Contains(searchTerm.ToLower())), includeProperties: "GroupCategory")
                    : _personnelRepository.Get(q => q.Id == personnel.Id, includeProperties: "GroupCategory");
            }
            else
            {
                query = !string.IsNullOrEmpty(searchTerm)
                    ? _personnelRepository.Get(q => q.Name.Contains(searchTerm.ToLower())
                        || q.LastName.Contains(searchTerm.ToLower()), includeProperties: "GroupCategory")
                    : _personnelRepository.Get(includeProperties: "GroupCategory");
            }

            //total number of items
            int queryCount = query.Count();
            switch (sortItem)
            {
                case "name":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Name)
                        : query.OrderByDescending(o => o.Name);
                    break;
                case "last_name":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.LastName)
                        : query.OrderByDescending(o => o.LastName);
                    break;
                default:
                    query = query.OrderByDescending(o => o.Id);
                    break;
            }

            List<Personnel> queryResult;
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
            model.PagingList = Mapper.Map<List<PersonnelDtoForPaging>>(queryResult);

            return model;
        }

        public int TotalNumberOfPersonnel()
        {
            return _personnelRepository.Get().Count();
        }

        public PersonnelDto GetById(int id, bool includeExtraData = false)
        {
            var personnel = includeExtraData ? _personnelRepository.Get(q => q.Id == id
                    , includeProperties: "GroupCategory,EmployeemnetType,Position" +
                    ",PersonnelApprovalProc,DismissalApprovals,DutyApprovals").SingleOrDefault()
                : _personnelRepository.GetById(id);
            if (personnel == null)
            {
                return null;
            }

            var dto = Mapper.Map<PersonnelDto>(personnel);
            if (personnel.DismissalApprovals != null)
                dto.DismissalApprovals = Mapper.Map<List<DismissalApprovalDto>>(personnel.DismissalApprovals.ToList());
            if (personnel.DutyApprovals != null)
                dto.DutyApprovals = Mapper.Map<List<DutyApprovalDto>>(personnel.DutyApprovals.ToList());

            return dto;
        }

        public PersonnelDto GetByCode(string code, bool includeExtraData = false)
        {
            var personnel = includeExtraData ? _personnelRepository.Get(q => q.Code == code
                    , includeProperties: "GroupCategory,EmployeemnetType,Position" +
                        ",PersonnelApprovalProc,DismissalApprovals,DutyApprovals").SingleOrDefault()
                : _personnelRepository.Get(q => q.Code == code).SingleOrDefault();
            if (personnel == null)
            {
                return null;
            }

            var dto = Mapper.Map<PersonnelDto>(personnel);
            if (personnel.DismissalApprovals != null)
                dto.DismissalApprovals = Mapper.Map<List<DismissalApprovalDto>>(personnel.DismissalApprovals.ToList());
            if (personnel.DutyApprovals != null)
                dto.DutyApprovals = Mapper.Map<List<DutyApprovalDto>>(personnel.DutyApprovals.ToList());

            return dto;
        }

        public bool CodeExists(string code)
        {
            return _personnelRepository.Get(q => q.Code == code).Any();
        }

        public List<PersonnelRecordDto> GetByGroupCategory(int groupCategoryId)
        {
            var personnel = _personnelRepository.Get(q => q.GroupCategoryId == groupCategoryId
                , includeProperties: "GroupCategory").ToList();

            return Mapper.Map<List<PersonnelRecordDto>>(personnel);
        }

        public List<PersonnelRecordDto> GetByWorkUnit(int workUnitId)
        {
            var personnel = _personnelRepository.Get(q => q.Position.WorkUnitId == workUnitId
                , includeProperties: "Position").ToList();

            return Mapper.Map<List<PersonnelRecordDto>>(personnel);
        }

        public List<PersonnelRecordDto> Search(string searchTerm)
        {
            var personnel = _personnelRepository
                .Get(q => q.Name.ToLower().Contains(searchTerm.TrimStart().TrimEnd().ToLower())
                    || q.LastName.ToLower().Contains(searchTerm.TrimStart().TrimEnd().ToLower())
                    || string.Concat(q.Name.ToLower() + " " + q.LastName.ToLower())
                    .Contains(searchTerm.TrimStart().TrimEnd().ToLower()))
                .ToList();

            return Mapper.Map<List<PersonnelRecordDto>>(personnel);
        }

        public List<PersonnelRecordDto> SearchByGroupCategory(string searchTerm
            , int groupCategoryId)
        {
            var personnel = _personnelRepository
                .Get(q => q.GroupCategoryId == groupCategoryId
                    && (q.Name.ToLower().Contains(searchTerm.TrimStart().TrimEnd().ToLower())
                        || q.LastName.ToLower().Contains(searchTerm.TrimStart().TrimEnd().ToLower())
                        || string.Concat(q.Name.ToLower() + " " + q.LastName.ToLower())
                        .Contains(searchTerm.TrimStart().TrimEnd().ToLower()))
                        , includeProperties: "GroupCategory")
                .ToList();

            return Mapper.Map<List<PersonnelRecordDto>>(personnel);
        }

        public List<PersonnelRecordDto> SearchByWorkUnit(string searchTerm
            , int workUnitId)
        {
            var personnel = _personnelRepository
                .Get(q => q.Position.WorkUnitId == workUnitId
                    && (q.Name.ToLower().Contains(searchTerm.TrimStart().TrimEnd().ToLower())
                        || q.LastName.ToLower().Contains(searchTerm.TrimStart().TrimEnd().ToLower())
                        || string.Concat(q.Name.ToLower() + " " + q.LastName.ToLower())
                        .Contains(searchTerm.TrimStart().TrimEnd().ToLower()))
                        , includeProperties: "Position")
                .ToList();

            return Mapper.Map<List<PersonnelRecordDto>>(personnel);
        }

        public async Task<CustomResult> Create(CreatePersonnelDto dto)
        {
            #region Register New Personnel
            if (PersonnelCodeExistsInDB(dto.Code))
            {
                return new CustomResult
                {
                    IsValid = false,
                    Message = "person code is not unique"
                };
            }
            if (NationalCodeExistsInDB(dto.NationalCode))
            {
                return new CustomResult
                {
                    IsValid = false,
                    Message = "national code is not unique"
                };
            }
            var personnel = new Personnel
            {
                Code = dto.Code,
                Name = dto.Name,
                LastName = dto.LastName,
                FathersName = dto.FathersName,
                NationalCode = dto.NationalCode,
                BirthCertificateCode = dto.BirthCertificateCode,
                PlaceOfBirth = dto.PlaceOfBirth,
                State = dto.State,
                City = dto.City,
                PostalCode = dto.PostalCode,
                BirthDate = dto.BirthDate,
                Email = dto.Email,
                Mobile = dto.Mobile,
                Phone = dto.Phone,
                Address = dto.Address,
                Education = dto.Education,
                MilitaryServiceStatus = dto.MilitaryServiceStatus,
                Gender = dto.Gender,
                MaritalStatus = dto.MaritalStatus,
                GroupCategoryId = dto.GroupCategoryId,
                EmployeementTypeId = dto.EmployeementTypeId,
                PositionId = dto.PositionId,
                InsuranceRecordDuration = dto.InsuranceRecordDuration,
                NoneInsuranceRecordDuration = dto.NoneInsuranceRecordDuration,
                BankAccountNumber = dto.BankAccountNumber,
                DateOfEmployeement = dto.DateOfEmployeement,
                FirstDateOfWork = dto.FirstDateOfWork,
                LastDateOfWork = dto.LastDateOfWork,
                LeavingWorkCause = dto.LeavingWorkCause,
                ActiveState = dto.ActiveState,
                IsPresent = true
            };

            _personnelRepository.Insert(personnel);
            #endregion

            #region Create System User For The Personnel
            var identityResult = await _authService.RegisterUserAsync(new RegisterUserDto
            {
                UserName = personnel.Code,
                Password = personnel.NationalCode,
                IsPersonnel = true,
                UserInfo = new RegisterUserInfoDto
                {
                    Name = personnel.Name,
                    LastName = personnel.LastName
                }
            });
            if (!identityResult.Succeeded)
            {
                return new CustomResult
                {
                    Message = "user creation for the person was not successful"
                };
            }
            #endregion

            #region Approvals
            _personnelApprovalProcService.CreateOrUpdate(new PersonnelApprovalProcDto
            {
                Id = personnel.Id,
                DismissalApprovalProcId = dto.DismissalApprovalProcId,
                DutyApprovalProcId = dto.DutyApprovalProcId,
                ShiftReplacementProcId = dto.ShiftReplacementProcId
            });

            _dismissalApprovalService.Create(dto.DismissalApprovals, personnel.Id);

            _dutyApprovalService.Create(dto.DutyApprovals, personnel.Id);
            #endregion

            return new CustomResult
            {
                IsValid = true
            };
        }

        public CustomResult Update(UpdatePersonnelDto dto)
        {
            var personnel = _personnelRepository.GetById(dto.Id);
            if (personnel != null)
            {
                #region Update Personnel Info
                if (PersonnelCodeExistsInDB(dto.Code, personnel.Id))
                {
                    return new CustomResult
                    {
                        IsValid = false,
                        Message = "person code is not unique"
                    };
                }
                if (NationalCodeExistsInDB(dto.NationalCode, personnel.Id))
                {
                    return new CustomResult
                    {
                        IsValid = false,
                        Message = "national code is not unique"
                    };
                }
                personnel.Code = dto.Code;
                personnel.Name = dto.Name;
                personnel.LastName = dto.LastName;
                personnel.FathersName = dto.FathersName;
                personnel.NationalCode = dto.NationalCode;
                personnel.BirthCertificateCode = dto.BirthCertificateCode;
                personnel.PlaceOfBirth = dto.PlaceOfBirth;
                personnel.State = dto.State;
                personnel.City = dto.City;
                personnel.PostalCode = dto.PostalCode;
                personnel.BirthDate = dto.BirthDate;
                personnel.Email = dto.Email;
                personnel.Mobile = dto.Mobile;
                personnel.Phone = dto.Phone;
                personnel.Address = dto.Address;
                personnel.Education = dto.Education;
                personnel.MilitaryServiceStatus = dto.MilitaryServiceStatus;
                personnel.Gender = dto.Gender;
                personnel.MaritalStatus = dto.MaritalStatus;
                personnel.GroupCategoryId = dto.GroupCategoryId;
                personnel.EmployeementTypeId = dto.EmployeementTypeId;
                personnel.PositionId = dto.PositionId;
                personnel.InsuranceRecordDuration = dto.InsuranceRecordDuration;
                personnel.NoneInsuranceRecordDuration = dto.NoneInsuranceRecordDuration;
                personnel.BankAccountNumber = dto.BankAccountNumber;
                personnel.DateOfEmployeement = dto.DateOfEmployeement;
                personnel.FirstDateOfWork = dto.FirstDateOfWork;
                personnel.LastDateOfWork = dto.LastDateOfWork;
                personnel.LeavingWorkCause = dto.LeavingWorkCause;
                personnel.ActiveState = dto.ActiveState;

                _personnelRepository.Update(personnel);
                #endregion

                #region Update Approvals
                _personnelApprovalProcService.CreateOrUpdate(new PersonnelApprovalProcDto
                {
                    Id = dto.Id,
                    DismissalApprovalProcId = dto.DismissalApprovalProcId,
                    DutyApprovalProcId = dto.DutyApprovalProcId,
                    ShiftReplacementProcId = dto.ShiftReplacementProcId
                });

                _dismissalApprovalService.Update(dto.DismissalApprovals, personnel.Id);
                _dutyApprovalService.Update(dto.DutyApprovals, personnel.Id);
                #endregion

                return new CustomResult
                {
                    IsValid = true
                };
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
                        (ex, "Personnel entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public PartialUpdatePersonnelDto PrepareForPartialUpdate(int id)
        {
            var personnel = _personnelRepository.GetById(id);
            if (personnel != null)
            {
                return new PartialUpdatePersonnelDto
                {
                    PatchDto = Mapper.Map<PersonnelPatchDto>(personnel),
                    PersonnelEntity = personnel
                };
            }
            else
            {
                return null;
            }
        }

        public void ApplyPartialUpdate(PartialUpdatePersonnelDto dto)
        {
            dto.PersonnelEntity.Name = dto.PatchDto.Name;
            dto.PersonnelEntity.LastName = dto.PatchDto.LastName;

            _personnelRepository.Update(dto.PersonnelEntity);
        }

        public async Task Delete(int id, DeleteState deleteState)
        {
            var personnel = _personnelRepository.GetById(id);
            if (personnel != null)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    _personnelRepository.Delete(personnel, deleteState);
                    //should also delete auto user for the personnel
                    await _authService.DeleteUserByUsernameAsync(personnel.Code);

                    scope.Complete();
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
                        (ex, "Personnel entity with the id: '{0}', is not available." +
                        " delete operation failed.", id);
                    throw;
                }
            }
        }

        public async Task<int> DeleteAll(string items)
        {
            try
            {
                var idsToRemove = items.ParseToIntArray().ToList();
                foreach (var id in idsToRemove)
                {
                    var person = _personnelRepository.GetById(id);
                    if (person != null)
                    {
                        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                        {
                            _personnelRepository.Delete(person.Id, DeleteState.SoftDelete);
                            //should also delete auto user for the personnel
                            await _authService.DeleteUserByUsernameAsync(person.Code);

                            scope.Complete();
                        }
                    }
                }

                return idsToRemove.Count;
            }
            catch (LogicalException ex)
            {
                _logger.LogRunTimeError(ex, ex.Message);
                throw;
            }
        }

        #region Helpers
        private bool PersonnelCodeExistsInDB(string code, int? personnelExceptionId = null)
        {
            if (personnelExceptionId.HasValue)
            {
                return _personnelRepository.Get(q => q.Code == code && q.Id != personnelExceptionId).Any();
            }
            return _personnelRepository.Get(q => q.Code == code).Any();
        }
        private bool NationalCodeExistsInDB(string nationalCode, int? personnelExceptionId = null)
        {
            if (personnelExceptionId.HasValue)
            {
                return _personnelRepository.Get(q => q.NationalCode == nationalCode && q.Id != personnelExceptionId).Any();
            }
            return _personnelRepository.Get(q => q.NationalCode == nationalCode).Any();
        }
        #endregion
    }
}
