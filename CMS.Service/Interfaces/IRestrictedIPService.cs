using CMS.Service.Dtos.RestrictedIP;
using System.Collections.Generic;
using WebApplication.SharedKernel.Enums;

namespace CMS.Service.Interfaces
{
    public interface IRestrictedIPService
    {
        List<RestrictedIPDto> Get();
        RestrictedIPDto GetById(int id);
        void Create(CreateRestrictedIPDto dto);
        void Update(UpdateRestrictedDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
