using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.Dismissal
{
    public class DismissalDtoPagingList : IPaging<DismissalDtoForPaging>
    {
        public List<DismissalDtoForPaging> PagingList { get; set; }
            = new List<DismissalDtoForPaging>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }

    public class DismissalDtoForPaging
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DismissalTypeTitle { get; set; }
        public string DismissalSystemTypeTitle { get; set; }
        public string DismissalExcessiveReactionTitle { get; set; }
        public long RowNumber { get; set; }
    }
}
