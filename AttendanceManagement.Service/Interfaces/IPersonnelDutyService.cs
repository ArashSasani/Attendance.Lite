using AttendanceManagement.Service.Dtos.PersonnelDuty;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IPersonnelDutyService
    {
        IPaging<PersonnelDutyDtoForPaging> Get(string username, string searchTerm, string sortItem, string sortOrder
                        , PagingQueryString pagingQueryString);
        PersonnelDutyDto GetById(int id);
        Task<CustomResult<string>> Create(CreatePersonnelDutyDto dto, string username);
        Task<CustomResult<string>> Update(UpdatePersonnelDutyDto dto);
        Task<CustomResult> Action(PersonnelDutyActionDto dto);
        Task<CustomResult> Action(List<PersonnelDutyActionDto> dto);
        CustomResult Delete(int id, DeleteState deleteState);
    }
}
