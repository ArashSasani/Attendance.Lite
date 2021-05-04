using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.PersonnelDutyEntrance
{
    public class PersonnelDutyEntranceDtoPagingList : IPaging<PersonnelDutyEntranceDto>
    {
        public List<PersonnelDutyEntranceDto> PagingList { get; set; }
            = new List<PersonnelDutyEntranceDto>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }
}
