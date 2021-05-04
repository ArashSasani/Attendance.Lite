using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.GroupCategory
{
    class GroupCategoryDtoPagingList : IPaging<GroupCategoryDto>
    {
        public List<GroupCategoryDto> PagingList { get; set; }
            = new List<GroupCategoryDto>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }
}
