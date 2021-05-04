using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.PersonnelHourlyShift
{
    public class PersonnelHourlyShiftDtoPagingList : IPaging<PersonnelHourlyShiftDtoForPaging>
    {
        public List<PersonnelHourlyShiftDtoForPaging> PagingList { get; set; }
            = new List<PersonnelHourlyShiftDtoForPaging>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }

    public class PersonnelHourlyShiftDtoForPaging
    {
        public int Id { get; set; }
        public string PersonnelFullName { get; set; }
        public string HourlyShiftTitle { get; set; }
        public string DateAssigned { get; set; }
        public long RowNumber { get; set; }
    }
}
