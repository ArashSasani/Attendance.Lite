using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.PersonnelApprovalProc;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using System.Linq;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class PersonnelApprovalProcService : IPersonnelApprovalProcService
    {
        private readonly IRepository<PersonnelApprovalProc> _personnelApprovalProcRepository;

        private readonly IExceptionLogger _logger;

        public PersonnelApprovalProcService(IRepository<PersonnelApprovalProc>
            personnelApprovalProcRepository, IExceptionLogger logger)
        {
            _personnelApprovalProcRepository = personnelApprovalProcRepository;

            _logger = logger;
        }

        public PersonnelApprovalProcDto GetById(int personnelId)
        {
            var personnelApprovalProc = _personnelApprovalProcRepository
                    .Get(q => q.Id == personnelId
                    , includeProperties: "DismissalApprovalProc,DutyApprovalProc,ShiftReplacementProc")
                    .SingleOrDefault();
            return Mapper.Map<PersonnelApprovalProcDto>(personnelApprovalProc);
        }

        public void CreateOrUpdate(PersonnelApprovalProcDto dto)
        {
            var personnelApprovalProc = _personnelApprovalProcRepository
                    .GetById(dto.Id);
            if (personnelApprovalProc != null)
            {
                personnelApprovalProc.DismissalApprovalProcId = dto.DismissalApprovalProcId;
                personnelApprovalProc.DutyApprovalProcId = dto.DutyApprovalProcId;
                personnelApprovalProc.ShiftReplacementProcId = dto.ShiftReplacementProcId;

                _personnelApprovalProcRepository.Update(personnelApprovalProc);
            }
            else
            {
                _personnelApprovalProcRepository.Insert(new PersonnelApprovalProc
                {
                    Id = dto.Id,
                    DismissalApprovalProcId = dto.DismissalApprovalProcId,
                    DutyApprovalProcId = dto.DutyApprovalProcId,
                    ShiftReplacementProcId = dto.ShiftReplacementProcId
                });
            }
        }
    }
}
