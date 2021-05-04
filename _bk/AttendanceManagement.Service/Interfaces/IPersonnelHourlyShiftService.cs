using AttendanceManagement.Service.Dtos.PersonnelHourlyShift;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IPersonnelHourlyShiftService
    {
        IPaging<PersonnelHourlyShiftDtoForPaging> Get(int? hourlyShiftId, string searchTerm, string sortItem
               , string sortOrder, PagingQueryString pagingQueryString);
        PersonnelHourlyShiftDto GetById(int id);
        CustomResult Create(CreatePersonnelHourlyShiftDto dto);
        void Update(UpdatePersonnelHourlyShiftDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
