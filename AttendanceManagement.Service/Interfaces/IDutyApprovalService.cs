using AttendanceManagement.Service.Dtos.DutyApproval;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IDutyApprovalService
    {
        Task<List<DutyApprovalDtoDDL>> GetForDDL(string username);
        void Create(List<int> dutyIds, int personnelId);
        void Update(List<int> dutyIds, int personnelId);
    }
}
