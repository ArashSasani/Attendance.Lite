using AttendanceManagement.Service.Dtos.Duty;
using System.Collections.Generic;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IDutyService
    {
        IPaging<DutyDto> Get(string searchTerm, string sortItem, string sortOrder
            , PagingQueryString pagingQueryString);
        DutyDto GetById(int id);
        List<DutyDto> GetForDDL();
        void Create(CreateDutyDto dto);
        void Update(UpdateDutyDto dto);
        PartialUpdateDutyDto PrepareForPartialUpdate(int id);
        void ApplyPartialUpdate(PartialUpdateDutyDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
