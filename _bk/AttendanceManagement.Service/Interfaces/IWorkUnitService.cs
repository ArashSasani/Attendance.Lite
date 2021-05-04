using AttendanceManagement.Service.Dtos.WorkUnit;
using System.Collections.Generic;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IWorkUnitService
    {
        IPaging<WorkUnitDto> Get(string searchTerm, string sortItem, string sortOrder
                  , PagingQueryString pagingQueryString);
        List<WorkUnitDto> GetForDDL();
        WorkUnitDto GetById(int id);
        void Create(CreateWorkUnitDto dto);
        void Update(UpdateWorkUnitDto dto);
        PartialUpdateWorkUnitDto PrepareForPartialUpdate(int id);
        void ApplyPartialUpdate(PartialUpdateWorkUnitDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
