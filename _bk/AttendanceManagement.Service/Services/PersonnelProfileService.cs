using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.ApprovalProc;
using AttendanceManagement.Service.Dtos.PersonnelProfile;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using CMS.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class PersonnelProfileService : IPersonnelProfileService
    {
        private readonly IRepository<Personnel> _personnelRepository;
        private readonly IRepository<PersonnelApprovalProc> _personnelApprovalProcRepository;
        private readonly IRepository<ApprovalProc> _approvalProcRepository;
        private readonly IAuthRepository _authRepository;

        private readonly IExceptionLogger _logger;

        public PersonnelProfileService(IRepository<Personnel> personnelRepository
            , IRepository<PersonnelApprovalProc> personnelApprovalProcRepository
            , IRepository<ApprovalProc> approvalProcRepository
            , IAuthRepository authRepository
            , IExceptionLogger logger)
        {
            _personnelRepository = personnelRepository;
            _personnelApprovalProcRepository = personnelApprovalProcRepository;
            _approvalProcRepository = approvalProcRepository;
            _authRepository = authRepository;

            _logger = logger;
        }

        public async Task<PersonnelProfileDto> Get(string username)
        {
            var profileInfo = new PersonnelProfileDto();

            var connectedUser = await _authRepository.FindUserByUsernameAsync(username);
            if (connectedUser == null)
            {
                try
                {
                    throw new LogicalException();
                }
                catch (LogicalException ex)
                {
                    _logger.LogLogicalError(ex, "personnel with code: {0}, " +
                        "corresponding user is not available!", username);
                    throw;
                }
            }

            var personnel = _personnelRepository.Get(q => q.Code == username).SingleOrDefault();
            if (personnel != null)
            {
                profileInfo.Id = personnel.Id;
                profileInfo.Name = personnel.Name;
                profileInfo.LastName = personnel.LastName;
                profileInfo.Mobile = personnel.Mobile;
                profileInfo.IsPresent = personnel.IsPresent;

                //approval procs
                var personnelApprovalProc = _personnelApprovalProcRepository
                    .Get(q => q.Id == personnel.Id).ToList();
                if (personnelApprovalProc != null)
                {
                    //dismissal proc
                    foreach (var item in personnelApprovalProc)
                    {
                        var dismissalApprovalProc = _approvalProcRepository
                            .Get(q => q.Id == item.DismissalApprovalProcId
                            , includeProperties: "ParentProc").ToList();
                        profileInfo.DismissalApprovalProcDto
                            = Mapper.Map<List<ApprovalProcDtoForProfile>>(dismissalApprovalProc);
                    }
                    //duty proc
                    foreach (var item in personnelApprovalProc)
                    {
                        var dutyApprovalProc = _approvalProcRepository
                            .Get(q => q.Id == item.DutyApprovalProcId
                            , includeProperties: "ParentProc").ToList();
                        profileInfo.DutyApprovalProcDto
                            = Mapper.Map<List<ApprovalProcDtoForProfile>>(dutyApprovalProc);
                    }
                    //shift replacement proc
                    foreach (var item in personnelApprovalProc)
                    {
                        var shiftReplacementApprovalProc = _approvalProcRepository
                            .Get(q => q.Id == item.ShiftReplacementProcId
                            , includeProperties: "ParentProc").ToList();
                        profileInfo.ShiftReplacementApprovalProcDto
                            = Mapper.Map<List<ApprovalProcDtoForProfile>>(shiftReplacementApprovalProc);
                    }
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
                    _logger.LogLogicalError(ex, "Personnel with Code: {0}, is not available!"
                        , username);
                    throw;
                }
            }

            return profileInfo;
        }

        public async Task<CustomResult> Update(UpdatePersonnelProfileDto dto, string username)
        {
            var connectedUser = await _authRepository.FindUserByUsernameAsync(username);
            if (connectedUser == null)
            {
                try
                {
                    throw new LogicalException();
                }
                catch (LogicalException ex)
                {
                    _logger.LogLogicalError(ex, "personnel with code: {0}, " +
                        "corresponding user is not available!", username);
                    throw;
                }
            }

            var personnel = _personnelRepository.Get(q => q.Code == username).SingleOrDefault();
            if (personnel != null)
            {
                personnel.Name = dto.Name;
                personnel.LastName = dto.LastName;
                personnel.Mobile = dto.Mobile;
                personnel.IsPresent = dto.IsPresent;
                _personnelRepository.Update(personnel);

                if (!string.IsNullOrEmpty(dto.Password)
                    && !string.IsNullOrEmpty(dto.ConfirmPassword))
                {
                    var result = await _authRepository.UpdateUserAsync(connectedUser, dto.Password);
                    if (!result.Succeeded)
                    {
                        return new CustomResult
                        {
                            Message = "updating password was not successful"
                        };
                    }
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
                        " update operation failed.", personnel.Id);
                    throw;
                }
            }

            return new CustomResult
            {
                IsValid = true
            };
        }
    }
}
