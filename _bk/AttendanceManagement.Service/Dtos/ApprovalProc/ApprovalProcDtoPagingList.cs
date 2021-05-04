using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.ApprovalProc
{
    public class ApprovalProcDtoPagingList : IPaging<ApprovalProcDtoForPaging>
    {
        public List<ApprovalProcDtoForPaging> PagingList { get; set; }
            = new List<ApprovalProcDtoForPaging>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }

    public class ApprovalProcDtoForPaging
    {
        public int Id { get; set; }
        public string ParentTitle { get; set; }
        public string Title { get; set; }
        public string ActiveStateTitle { get; set; }
        public long RowNumber { get; set; }
    }
}
