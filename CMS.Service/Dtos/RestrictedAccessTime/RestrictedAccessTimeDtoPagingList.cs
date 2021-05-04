using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace CMS.Service.Dtos.RestrictedAccessTime
{
    public class RestrictedAccessTimeDtoPagingList : IPaging<RestrictedAccessTimeDto>
    {
        public List<RestrictedAccessTimeDto> PagingList { get; set; }
            = new List<RestrictedAccessTimeDto>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }
}
