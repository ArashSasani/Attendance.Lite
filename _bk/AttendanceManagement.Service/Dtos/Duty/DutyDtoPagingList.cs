using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.Duty
{
    public class DutyDtoPagingList : IPaging<DutyDto>
    {
        public List<DutyDto> PagingList { get; set; } = new List<DutyDto>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }
}
