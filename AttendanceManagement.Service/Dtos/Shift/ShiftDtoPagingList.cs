using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.Shift
{
    public class ShiftDtoPagingList : IPaging<ShiftDto>
    {
        public List<ShiftDto> PagingList { get; set; }
            = new List<ShiftDto>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }

}
