using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.PersonnelDismissal
{
    public class PersonnelDismissalDtoPagingList : IPaging<PersonnelDismissalDtoForPaging>
    {
        public List<PersonnelDismissalDtoForPaging> PagingList { get; set; }
            = new List<PersonnelDismissalDtoForPaging>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }

    public class PersonnelDismissalDtoForPaging
    {
        public int Id { get; set; }
        public string PersonnelFullName { get; set; }
        public string DismissalTitle { get; set; }
        public string SubmittedDate { get; set; }
        public string DismissalDurationTitle { get; set; }
        public string RequestActionTitle { get; set; }
        public long RowNumber { get; set; }
    }
}
