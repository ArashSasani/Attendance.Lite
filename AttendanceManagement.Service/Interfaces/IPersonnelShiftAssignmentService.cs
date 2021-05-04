using AttendanceManagement.Service.Dtos.PersonnelShiftAssignment;
using System.Collections.Generic;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IPersonnelShiftAssignmentService
    {
        List<PersonnelShiftAssignmentDisplayDto> Get(int personnelId);
        PersonnelShiftAssignmentDisplayDto GetById(int id);
        CustomResult Update(UpdatePersonnelShiftAssignmentDto dto);
        void Delete(int id, DeleteState deleteState);
    }
}
