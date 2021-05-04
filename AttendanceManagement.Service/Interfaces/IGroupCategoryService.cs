using AttendanceManagement.Service.Dtos.GroupCategory;
using System.Collections.Generic;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IGroupCategoryService
    {
        IPaging<GroupCategoryDto> Get(string searchTerm, string sortItem, string sortOrder
               , PagingQueryString pagingQueryString);
        List<GroupCategoryDto> GetForDDL();
        GroupCategoryDto GetById(int id);
        void Create(CreateGroupCategoryDto dto);
        void Update(UpdateGroupCategoryDto dto);
        PartialUpdateGroupCategoryDto PrepareForPartialUpdate(int id);
        void ApplyPartialUpdate(PartialUpdateGroupCategoryDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
