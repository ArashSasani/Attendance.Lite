using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.PersonnelShiftReplacement
{
    public class PersonnelShiftReplacementDtoPagingList : IPaging<PersonnelShiftReplacementDtoForPaging>
    {
        public List<PersonnelShiftReplacementDtoForPaging> PagingList { get; set; }
            = new List<PersonnelShiftReplacementDtoForPaging>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }

    public class PersonnelShiftReplacementDtoForPaging
    {
        public int Id { get; set; }
        public string PersonnelFullName { get; set; }
        public string ReplacedPersonnelFullName { get; set; }
        public string ReplacementDate { get; set; }
        public string ShiftTitle { get; set; }
        public string ReplacedShiftTitle { get; set; }
        public string RequestActionTitle { get; set; }
        public long RowNumber { get; set; }
    }
}
