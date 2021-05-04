using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.ApprovalProc;
using System.Collections.Generic;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IApprovalProcService
    {
        IPaging<ApprovalProcDtoForPaging> Get(string searchTerm, string sortItem, string sortOrder
            , PagingQueryString pagingQueryString);
        ApprovalProcDto GetById(int id);
        List<ApprovalProcDtoForDDL> GetForDDL(int? exceptionId);
        ReceiverInfoDto GetNextReceiverId(ApprovalProc approvalProc);
        ReceiverInfoDto GetNextReceiverId(int ParentApprovalProcId);
        void Create(CreateApprovalProcDto dto);
        void Update(UpdateApprovalProcDto dto);
        PartialUpdateApprovalProcDto PrepareForPartialUpdate(int id);
        void ApplyPartialUpdate(PartialUpdateApprovalProcDto dto);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
    }
}
