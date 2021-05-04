using AttendanceManagement.Service.Dtos.PersonnelProfile;
using System.Threading.Tasks;
using WebApplication.SharedKernel;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IPersonnelProfileService
    {
        Task<PersonnelProfileDto> Get(string username);
        Task<CustomResult> Update(UpdatePersonnelProfileDto dto, string username);
    }
}
