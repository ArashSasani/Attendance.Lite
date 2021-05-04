using AttendanceManagement.Service.Dtos.Personnel;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IPersonnelService
    {
        IPaging<PersonnelDtoForPaging> Get(string username, string searchTerm, string sortItem, string sortOrder
            , PagingQueryString pagingQueryString);
        int TotalNumberOfPersonnel();
        PersonnelDto GetById(int id, bool includeExtraData = false);
        PersonnelDto GetByCode(string code, bool includeExtraData = false);
        bool CodeExists(string code);
        List<PersonnelRecordDto> GetByGroupCategory(int groupCategoryId);
        List<PersonnelRecordDto> GetByWorkUnit(int workUnitId);
        List<PersonnelRecordDto> Search(string searchTerm);
        List<PersonnelRecordDto> SearchByGroupCategory(string searchTerm, int groupCategoryId);
        List<PersonnelRecordDto> SearchByWorkUnit(string searchTerm, int workUnitId);
        Task<CustomResult> Create(CreatePersonnelDto dto);
        CustomResult Update(UpdatePersonnelDto dto);
        PartialUpdatePersonnelDto PrepareForPartialUpdate(int id);
        void ApplyPartialUpdate(PartialUpdatePersonnelDto dto);
        Task Delete(int id, DeleteState deleteState);
        /// <summary>
        /// delete selected items in one sql command
        /// </summary>
        /// <param name="items">comma-seperated string representation of ids.
        /// example: "1,2,3"</param>
        /// <returns>number of rows affected</returns>
        Task<int> DeleteAll(string items);
    }
}
