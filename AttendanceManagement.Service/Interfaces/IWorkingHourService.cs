using AttendanceManagement.Service.Dtos.WorkingHour;
using System.Collections.Generic;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IWorkingHourService
    {
        IPaging<WorkingHourDtoForPaging> Get(int? shiftId, string searchTerm, string sortItem
            , string sortOrder, PagingQueryString pagingQueryString);
        WorkingHourDto GetById(int shiftId);
        List<WorkingHourDDLDto> GetForDDL(int id);
        CustomResult Create(CreateWorkingHourDto dto);
        CustomResult Update(UpdateWorkingHourDto dto);
        PartialUpdateWorkingHourDto PrepareForPartialUpdate(int id);
        void ApplyPartialUpdate(PartialUpdateWorkingHourDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
