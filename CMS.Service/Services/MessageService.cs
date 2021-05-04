using AutoMapper;
using CMS.Core.Interfaces;
using CMS.Core.Model;
using CMS.Service.Dtos.Message;
using CMS.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.Infrastructure.Parser;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace CMS.Service
{
    public class MessageService : IMessageService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<RequestMessage> _requestMessageRepository;

        private readonly IExceptionLogger _logger;

        public MessageService(IAuthRepository authRepository
            , IRepository<Message> messageRepository
            , IRepository<RequestMessage> requestMessageRepository
            , IExceptionLogger logger)
        {
            _authRepository = authRepository;
            _messageRepository = messageRepository;
            _requestMessageRepository = requestMessageRepository;

            _logger = logger;
        }


        public async Task<IPaging<MessageDtoForPaging>> Get(MessageType messageType
            , string receiverUsername, string searchTerm, string sortItem, string sortOrder
            , PagingQueryString pagingQueryString)
        {
            IPaging<MessageDtoForPaging> model = new MessageDtoPagingList();

            var receiver = await _authRepository.FindUserByUsernameAsync(receiverUsername);
            if (receiver != null)
            {

                var query = !string.IsNullOrEmpty(searchTerm)
                ? _messageRepository.Get(q => q.MessageType == messageType && q.ReceiverId == receiver.Id
                    && q.Title.Contains(searchTerm.ToLower()), includeProperties: "Sender,Receiver")
                : _messageRepository.Get(q => q.MessageType == messageType && q.ReceiverId == receiver.Id
                    , includeProperties: "Sender,Receiver");
                //total number of items
                int queryCount = query.Count();
                switch (sortItem)
                {
                    case "title":
                        query = sortOrder == "asc" ? query.OrderBy(o => o.Title)
                            : query.OrderByDescending(o => o.Title);
                        break;
                    case "date":
                        query = sortOrder == "asc" ? query.OrderBy(o => o.Date)
                            : query.OrderByDescending(o => o.Date);
                        break;
                    default:
                        query = query.OrderByDescending(o => o.Id);
                        break;
                }

                List<Message> queryResult;
                if (pagingQueryString != null) //with paging
                {
                    var pageSetup = new PagingSetup(pagingQueryString.Page, pagingQueryString.PageSize);
                    queryResult = query.Skip(pageSetup.Offset).Take(pageSetup.Next).ToList();
                    //paging controls
                    var controls = pageSetup.GetPagingControls(queryCount, PagingStrategy.ReturnNull);
                    if (controls != null)
                    {
                        model.PagesCount = controls.PagesCount;
                        model.NextPage = controls.NextPage;
                        model.PrevPage = controls.PrevPage;
                    }
                }
                else //without paging
                {
                    queryResult = query.ToList();
                }
                model.PagingList = Mapper.Map<List<MessageDtoForPaging>>(queryResult);
            }

            return model;
        }

        public async Task<List<NotificationDto>> GetNotifications(string userId)
        {
            var user = await _authRepository.FindUserAsync(userId);
            if (user != null)
            {
                var messages = _messageRepository
                    .Get(q => q.ReceiverId == user.Id && q.IsRead == false).ToList();
                return Mapper.Map<List<NotificationDto>>(messages);
            }
            return new List<NotificationDto>();
        }

        public async Task<int> GetNotificationsCount(MessageType messageType, string username)
        {
            var user = await _authRepository.FindUserByUsernameAsync(username);
            if (user != null)
            {
                return _messageRepository.Get(q => q.MessageType == messageType
                    && q.ReceiverId == user.Id && q.IsRead == false).Count();
            }
            return 0;
        }

        public MessageDto GetById(int id)
        {
            var message = _messageRepository
                .Get(q => q.Id == id, includeProperties: "Sender,Receiver")
                .SingleOrDefault();
            if (message == null)
            {
                return null;
            }
            //visited -> update read state
            if (!message.IsRead)
            {
                message.IsRead = true;
                _messageRepository.Update(message);
            }
            //return dto
            switch (message.MessageType)
            {
                case MessageType.Normal:
                    var normalMessage = message as Message;
                    return Mapper.Map<MessageDto>(normalMessage);
                case MessageType.Request:
                    var requestMessage = message as RequestMessage;
                    return Mapper.Map<MessageDto>(requestMessage);
            }
            return Mapper.Map<MessageDto>(message);
        }

        public async Task<string> Create(CreateMessageDto dto, string senderUsername)
        {
            string receiverId = "";

            var sender = await _authRepository.FindUserByUsernameAsync(senderUsername);
            if (sender != null)
            {
                switch (dto.MessageType)
                {
                    case MessageType.Normal:
                        var normalMessage = new Message
                        {
                            SenderId = sender.Id,
                            ReceiverId = dto.ReceiverId,
                            Date = DateTime.Now,
                            Title = dto.Title,
                            Content = dto.Content,
                            MessageType = MessageType.Normal
                        };
                        _messageRepository.Insert(normalMessage);
                        receiverId = normalMessage.ReceiverId;
                        break;
                    case MessageType.Request:
                        if (dto.Request != null)
                        {
                            var requestMessage = new RequestMessage
                            {
                                SenderId = sender.Id,
                                ReceiverId = dto.ReceiverId,
                                Date = DateTime.Now,
                                Title = dto.Title,
                                Content = dto.Content,
                                MessageType = MessageType.Request,
                                RequestId = dto.Request.RequestId,
                                RequestType = dto.Request.RequestType,
                                ParentApprovalProcId = dto.Request.ParentApprovalProcId,
                                RequestAction = dto.Request.RequestAction
                            };
                            _requestMessageRepository.Insert(requestMessage);
                            receiverId = requestMessage.ReceiverId;
                        }
                        else
                        {
                            MessageDetailsError(dto.MessageType, "create");
                        }
                        break;
                    default:
                        break;
                }

                return receiverId;
            }
            else
            {
                try
                {
                    throw new LogicalException();
                }
                catch (LogicalException ex)
                {
                    _logger.LogLogicalError(ex, "Sender user with username: {0} is not available!"
                        , senderUsername);
                    throw;
                }
            }
        }

        public void UpdateRequest(int id, RequestAction requestAction)
        {
            var requestMessage = _requestMessageRepository.GetById(id);
            if (requestMessage != null)
            {
                requestMessage.RequestAction = requestAction;
                _requestMessageRepository.Update(requestMessage);
            }
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var message = _messageRepository.GetById(id);
            if (message != null)
            {
                _messageRepository.Delete(message, deleteState);
            }
            else
            {
                try
                {
                    throw new LogicalException();
                }
                catch (LogicalException ex)
                {
                    _logger.LogLogicalError
                        (ex, "Message entity with the id: '{0}', is not available." +
                        " delete operation failed.", id);
                    throw;
                }
            }
        }

        public int DeleteAll(string items)
        {
            try
            {
                var idsToRemove = items.ParseToIntArray().ToList();
                idsToRemove.ForEach(i => _messageRepository.Delete(i, DeleteState.SoftDelete));

                return idsToRemove.Count;
            }
            catch (LogicalException ex)
            {
                _logger.LogRunTimeError(ex, ex.Message);
                throw;
            }
        }

        public void RemoveRequest(RequestType requestType, int requestId)
        {
            var request = _requestMessageRepository
                    .Get(q => q.RequestType == requestType && q.RequestId == requestId)
                    .FirstOrDefault();
            if (request != null)
            {
                _requestMessageRepository.Delete(request, DeleteState.SoftDelete);
            }
        }

        #region Helpers
        private void MessageDetailsError(MessageType messageType, string operation)
        {
            try
            {
                throw new LogicalException();
            }
            catch (LogicalException ex)
            {
                _logger.LogRunTimeError(ex, "The Message dto that has passed to " +
                    "the service should also have '{0}' message properties." +
                    " {1} operation failed.", messageType, operation);
                throw;
            }
        }
        #endregion
    }
}
