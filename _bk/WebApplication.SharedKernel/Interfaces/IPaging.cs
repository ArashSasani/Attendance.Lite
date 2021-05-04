using System.Collections.Generic;

namespace WebApplication.SharedKernel.Interfaces
{
    public interface IPaging<T>
    {
        List<T> PagingList { get; set; }

        int? NextPage { get; set; }
        int? PrevPage { get; set; }
        int PagesCount { get; set; }
    }
}
