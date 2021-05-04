using CMS.Service.Dtos.Message;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace CMS.Service.Interfaces
{
    public interface IMessageService
    {
        Task<IPaging<MessageDtoForPaging>> Get(MessageType messageType
            , string receiverUsername, string searchTerm, string sortItem, string sortOrder
            , PagingQueryString pagingQueryString);
        Task<List<NotificationDto>> GetNotifications(string userId);
        Task<int> GetNotificationsCount(MessageType messageType, string username);
        MessageDto GetById(int id);
        Task<string> Create(CreateMessageDto dto, string senderUsername);
        void UpdateRequest(int id, RequestAction requestAction);
        void Delete(int id, DeleteState deleteState);
        int DeleteAll(string items);
        void RemoveRequest(RequestType requestType, int requestId);
    }
}
