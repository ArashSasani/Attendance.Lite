using AttendanceManagement.Service.Dtos.PersonnelShiftReplacement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IPersonnelShiftReplacementService
    {
        IPaging<PersonnelShiftReplacementDtoForPaging> Get(string username
            , string searchTerm, string sortItem, string sortOrder, PagingQueryString pagingQueryString);
        PersonnelShiftReplacementDto GetById(int id);
        List<ReplaceShiftDto> GetReplacedShifts(int personnelId, DateTime fromDate, DateTime toDate);
        Task<CustomResult<string>> Create(CreatePersonnelShiftReplacementDto dto, string username);
        Task<CustomResult<string>> Update(UpdatePersonnelShiftReplacementDto dto);
        Task<CustomResult> Action(PersonnelShiftReplacementActionDto dto);
        Task<CustomResult> Action(List<PersonnelShiftReplacementActionDto> dto);
        CustomResult Delete(int id, DeleteState deleteState);
    }
}
