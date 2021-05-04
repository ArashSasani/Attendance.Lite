using AttendanceManagement.Service.Dtos.PersonnelApprovalProc;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IPersonnelApprovalProcService
    {
        PersonnelApprovalProcDto GetById(int personnelId);
        void CreateOrUpdate(PersonnelApprovalProcDto dto);
    }
}
