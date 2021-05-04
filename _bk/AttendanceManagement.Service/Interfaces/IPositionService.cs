using AttendanceManagement.Service.Dtos.Position;
using System.Collections.Generic;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IPositionService
    {
        IPaging<PositionDto> Get(string searchTerm, string sortItem, string sortOrder
                    , PagingQueryString pagingQueryString);
        List<PositionDDLDto> GetForDDL(int workUnitId);
        PositionDto GetById(int id);
        void Create(CreatePositionDto dto);
        void Update(UpdatePositionDto dto);
        PartialUpdatePositionDto PrepareForPartialUpdate(int id);
        void ApplyPartialUpdate(PartialUpdatePositionDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
