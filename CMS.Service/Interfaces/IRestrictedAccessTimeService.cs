using CMS.Service.Dtos.RestrictedAccessTime;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace CMS.Service.Interfaces
{
    public interface IRestrictedAccessTimeService
    {
        IPaging<RestrictedAccessTimeDto> Get(string searchTerm, string sortItem, string sortOrder
            , PagingQueryString pagingQueryString);
        RestrictedAccessTimeDto GetById(int id);
        void Create(CreateRestrictedAccessTimeDto dto);
        void Update(UpdateRestrictedAccessTimeDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
