using AttendanceManagement.Service.Dtos.Dismissal;
using System.Collections.Generic;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IDismissalService
    {
        IPaging<DismissalDtoForPaging> Get(DismissalSystemTypeAccess typeAccess
            , string searchTerm, string sortItem, string sortOrder, PagingQueryString pagingQueryString);
        DismissalDto GetById(int id);
        DismissalDto GetDefault(DismissalType dismissalType);
        List<DismissalDtoDDL> GetForDDL(DismissalSystemTypeAccess typeAccess);
        DismissalAllowancesDto GetAllowances(int id);
        DismissalChartDto GetChartInfo(int id);
        void Create(CreateDismissalDto dto);
        void Update(UpdateDismissalDto dto);
        CustomResult UpdateDefault(UpdateDismissalDto dto);
        PartialUpdateDismissalDto PrepareForPartialUpdate(int id);
        void ApplyPartialUpdate(PartialUpdateDismissalDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
