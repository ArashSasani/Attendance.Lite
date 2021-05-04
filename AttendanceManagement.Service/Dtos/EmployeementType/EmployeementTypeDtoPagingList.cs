using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service.Dtos.EmployeementType
{
    public class EmployeementTypeDtoPagingList : IPaging<EmployeementTypeDto>
    {
        public List<EmployeementTypeDto> PagingList { get; set; }
            = new List<EmployeementTypeDto>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }
}
