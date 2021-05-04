using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.WorkUnit
{
    public class WorkUnitDtoPagingList : IPaging<WorkUnitDto>
    {
        public List<WorkUnitDto> PagingList { get; set; }
            = new List<WorkUnitDto>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }
}
