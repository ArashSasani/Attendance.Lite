using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.Position
{
    public class PositionDtoPagingList : IPaging<PositionDto>
    {
        public List<PositionDto> PagingList { get; set; }
            = new List<PositionDto>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }
}
