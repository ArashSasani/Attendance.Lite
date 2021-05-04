using AttendanceManagement.Service.Dtos.Shift;
using System.Collections.Generic;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IShiftService
    {
        IPaging<ShiftDto> Get(string searchTerm, string sortItem, string sortOrder
               , PagingQueryString pagingQueryString);
        ShiftDto GetById(int id);
        List<ShiftDDLDto> GetForDDL();
        void Create(CreateShiftDto dto);
        void Update(UpdateShiftDto dto);
        PartialUpdateShiftDto PrepareForPartialUpdate(int id);
        void ApplyPartialUpdate(PartialUpdateShiftDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
