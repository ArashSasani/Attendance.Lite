using AttendanceManagement.Service.Dtos.DismissalApproval;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IDismissalApprovalService
    {
        Task<List<DismissalApprovalDtoDDL>> GetForDDL(string username);
        void Create(List<int> dismissalIds, int personnelId);
        void Update(List<int> dismissalIds, int personnelId);
    }
}
