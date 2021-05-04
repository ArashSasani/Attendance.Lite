using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.PersonnelDuty
{
    class PersonnelDutyDtoPagingList : IPaging<PersonnelDutyDtoForPaging>
    {
        public List<PersonnelDutyDtoForPaging> PagingList { get; set; }
               = new List<PersonnelDutyDtoForPaging>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }

    public class PersonnelDutyDtoForPaging
    {
        public int Id { get; set; }
        public string PersonnelFullName { get; set; }
        public string DutyTitle { get; set; }
        public string SubmittedDate { get; set; }
        public string DutyDurationTitle { get; set; }
        public string RequestActionTitle { get; set; }
        public long RowNumber { get; set; }
    }
}
