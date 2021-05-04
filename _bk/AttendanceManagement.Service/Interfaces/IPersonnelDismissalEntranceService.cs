using AttendanceManagement.Service.Dtos.PersonnelDismissalEntrance;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IPersonnelDismissalEntranceService
    {
        IPaging<PersonnelDismissalEntranceDto> Get(string username, string fromDate, string toDate, string searchTerm
            , string sortItem, string sortOrder, PagingQueryString pagingQueryString);
        PersonnelDismissalEntranceDto GetById(int id);
        CustomResult Update(UpdatePersonnelDismissalEntranceDto dto);
        PartialUpdatePersonnelDismissalEntranceDto PrepareForPartialUpdate(int id);
        void ApplyPartialUpdate(PartialUpdatePersonnelDismissalEntranceDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
