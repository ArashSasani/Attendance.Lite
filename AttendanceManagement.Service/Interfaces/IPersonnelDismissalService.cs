using AttendanceManagement.Service.Dtos.PersonnelDismissal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IPersonnelDismissalService
    {
        IPaging<PersonnelDismissalDtoForPaging> Get(string username, int? dismissalType
            , string fromDate, string toDate, string searchTerm, string sortItem, string sortOrder
            , PagingQueryString pagingQueryString);
        int GetNumberOfRequests(DateTime date);
        PersonnelDismissalDto GetById(int id);
        Task<PersonnelDismissalHistoryDto> GetChartInfo(string username, int dismissalId);
        Task<CustomResult<string>> Create(CreatePersonnelDismissalDto dto, string username);
        Task<CustomResult<string>> Update(UpdatePersonnelDismissalDto dto);
        Task<CustomResult> Action(PersonnelDismissalActionDto dto);
        Task<CustomResult> Action(List<PersonnelDismissalActionDto> dto);
        CustomResult Delete(int id, DeleteState deleteState);
    }
}
