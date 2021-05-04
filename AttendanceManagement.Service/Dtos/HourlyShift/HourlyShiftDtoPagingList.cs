using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.HourlyShift
{
    public class HourlyShiftDtoPagingList : IPaging<HourlyShiftDtoForPaging>
    {
        public List<HourlyShiftDtoForPaging> PagingList { get; set; }
            = new List<HourlyShiftDtoForPaging>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }

    public class HourlyShiftDtoForPaging
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string HoursShouldWorkInDayTitle { get; set; }
        public string HoursShouldWorkInWeekTitle { get; set; }
        public string HoursShouldWorkInMonthTitle { get; set; }
    }
}
