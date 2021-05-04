using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.PersonnelDismissalEntrance
{
    public class PersonnelDismissalEntranceDtoPagingList : IPaging<PersonnelDismissalEntranceDto>
    {
        public List<PersonnelDismissalEntranceDto> PagingList { get; set; }
            = new List<PersonnelDismissalEntranceDto>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }
}
