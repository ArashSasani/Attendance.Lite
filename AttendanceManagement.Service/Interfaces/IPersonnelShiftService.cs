using AttendanceManagement.Service.Dtos.PersonnelShift;
using AttendanceManagement.Service.Dtos.Shift;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IPersonnelShiftService
    {
        IPaging<PersonnelShiftDtoForPaging> Get(int? shiftId, string searchTerm, string sortItem
            , string sortOrder, PagingQueryString pagingQueryString);
        PersonnelShiftDto GetById(int id);
        bool IsAssignmentAvailable(int shiftId, DateTime date);
        Task<List<ShiftDDLDto>> GetPersonnelShifts(string username);
        List<ShiftDDLDto> GetReplacedPersonnelShifts(int personnelId);
        CustomResult CreateByPattern(CreatePersonnelShiftByPatternDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
