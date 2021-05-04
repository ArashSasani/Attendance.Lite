using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.WorkingHour
{
    public class WorkingHourDtoPagingList : IPaging<WorkingHourDtoForPaging>
    {
        public List<WorkingHourDtoForPaging> PagingList { get; set; }
            = new List<WorkingHourDtoForPaging>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }

    public class WorkingHourDtoForPaging
    {
        public int Id { get; set; }
        public string ShiftTitle { get; set; }
        public string Title { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public long RowNumber { get; set; }
    }
}
