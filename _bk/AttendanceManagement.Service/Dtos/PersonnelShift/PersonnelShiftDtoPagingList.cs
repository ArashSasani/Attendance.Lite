using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.PersonnelShift
{
    public class PersonnelShiftDtoPagingList : IPaging<PersonnelShiftDtoForPaging>
    {
        public List<PersonnelShiftDtoForPaging> PagingList { get; set; }
            = new List<PersonnelShiftDtoForPaging>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }

    public class PersonnelShiftDtoForPaging
    {
        public int Id { get; set; }
        public string PersonnelFullName { get; set; }
        public string ShiftTitle { get; set; }
        public string DateAssigned { get; set; }
        public int AssignmentsCount { get; set; }
        public long RowNumber { get; set; }
    }
}
