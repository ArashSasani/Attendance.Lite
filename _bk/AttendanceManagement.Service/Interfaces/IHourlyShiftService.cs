using AttendanceManagement.Service.Dtos.HourlyShift;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IHourlyShiftService
    {
        IPaging<HourlyShiftDtoForPaging> Get(string searchTerm, string sortItem, string sortOrder
                 , PagingQueryString pagingQueryString);
        HourlyShiftDto GetById(int id);
        void Create(CreateHourlyShiftDto dto);
        void Update(UpdateHourlyShiftDto dto);
        PartialUpdateHourlyShiftDto PrepareForPartialUpdate(int id);
        void ApplyPartialUpdate(PartialUpdateHourlyShiftDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
