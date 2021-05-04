using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.Personnel
{
    public class PersonnelDtoPagingList : IPaging<PersonnelDtoForPaging>
    {
        public List<PersonnelDtoForPaging> PagingList { get; set; }
            = new List<PersonnelDtoForPaging>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }

    public class PersonnelDtoForPaging
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string GroupCategoryTitle { get; set; }
        public string ActiveStateTitle { get; set; }
        public long RowNumber { get; set; }
    }
}
