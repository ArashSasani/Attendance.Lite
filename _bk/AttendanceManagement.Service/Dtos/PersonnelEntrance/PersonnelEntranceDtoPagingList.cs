using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.PersonnelEntrance
{
    public class PersonnelEntranceDtoPagingList : IPaging<PersonnelEntranceDto>
    {
        public List<PersonnelEntranceDto> PagingList { get; set; }
        = new List<PersonnelEntranceDto>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }

    public enum EntranceType
    {
        All,
        Present,
        Absent
    }
}
