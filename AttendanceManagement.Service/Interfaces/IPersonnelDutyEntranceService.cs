using AttendanceManagement.Service.Dtos.PersonnelDutyEntrance;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IPersonnelDutyEntranceService
    {
        IPaging<PersonnelDutyEntranceDto> Get(string username, string fromDate, string toDate, string searchTerm
            , string sortItem, string sortOrder, PagingQueryString pagingQueryString);
        PersonnelDutyEntranceDto GetById(int id);
        CustomResult Update(UpdatePersonnelDutyEntranceDto dto);
        PartialUpdatePersonnelDutyEntranceDto PrepareForPartialUpdate(int id);
        void ApplyPartialUpdate(PartialUpdatePersonnelDutyEntranceDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
