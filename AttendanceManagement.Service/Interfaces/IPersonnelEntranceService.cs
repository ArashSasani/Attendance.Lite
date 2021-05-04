using AttendanceManagement.Service.Dtos.PersonnelEntrance;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IPersonnelEntranceService
    {
        IPaging<PersonnelEntranceDto> Get(string username, string fromDate, string toDate
            , EntranceType entranceType, string searchTerm, string sortItem, string sortOrder
            , PagingQueryString pagingQueryString);
        PersonnelEntranceDto GetById(int id);
        CustomResult Update(UpdatePersonnelEntranceDto dto);
        PartialUpdatePersonnelEntranceDto PrepareForPartialUpdate(int id);
        void ApplyPartialUpdate(PartialUpdatePersonnelEntranceDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
