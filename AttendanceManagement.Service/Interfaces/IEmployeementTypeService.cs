using AttendanceManagement.Service.Dtos.EmployeementType;
using System.Collections.Generic;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IEmployeementTypeService
    {
        IPaging<EmployeementTypeDto> Get(string searchTerm, string sortItem, string sortOrder
                  , PagingQueryString pagingQueryString);
        List<EmployeementTypeDto> GetForDDL();
        EmployeementTypeDto GetById(int id);
        void Create(CreateEmployeementTypeDto dto);
        void Update(UpdateEmployeementTypeDto dto);
        PartialUpdateEmployeementTypeDto PrepareForPartialUpdate(int id);
        void ApplyPartialUpdate(PartialUpdateEmployeementTypeDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
