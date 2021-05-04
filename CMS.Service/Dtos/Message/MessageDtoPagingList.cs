using System.Collections.Generic;
using WebApplication.SharedKernel.Interfaces;

namespace CMS.Service.Dtos.Message
{
    public class MessageDtoPagingList : IPaging<MessageDtoForPaging>
    {
        public List<MessageDtoForPaging> PagingList { get; set; }
            = new List<MessageDtoForPaging>();
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }
        public int PagesCount { get; set; }
    }

    public class MessageDtoForPaging
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string SenderUsername { get; set; }
        public string SenderFullName { get; set; }
        public string Title { get; set; }
        public string IsReadTitle { get; set; }
        public string RequestActionTitle { get; set; }
    }
}
