using CMS.Service.Dtos.Message;
using CMS.Service.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.API.Realtime.Interfaces;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers.CMS
{
    [RoutePrefix("api/cms/messages")]
    public class MessagesController : ApiController
    {
        private readonly INotificationService _notificationService;
        private readonly IMessageService _messageService;

        private readonly IExceptionLogger _logger;

        public MessagesController(INotificationService notificationService
            , IMessageService messageService
            , IExceptionLogger logger)
        {
            _notificationService = notificationService;
            _messageService = messageService;

            _logger = logger;
        }

        [AuthorizeUser]
        [Route("")]
        public async Task<IHttpActionResult> Get([FromUri] MessageType messageType
            , [FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var messages = await _messageService.Get(messageType, User.Identity.Name, searchTerm, sortItem
                , sortOrder, pagingQueryString);
            return Ok(messages);
        }

        [Authorize]
        [Route("notifications/count/{messageType}")]
        public async Task<IHttpActionResult> GetNotificationsCount(MessageType messageType)
        {
            int count = await _messageService.GetNotificationsCount(messageType, User.Identity.Name);
            return Ok(count);
        }

        [AuthorizeUser]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var message = _messageService.GetById(id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        [AuthorizeUser]
        [Route("create")]
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody] CreateMessageDto message)
        {
            if (message == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                string errorMessage = new ModelStateError(_logger).OutputMessage(ModelState);
                return BadRequest(errorMessage);
            }
            try
            {
                message.MessageType = MessageType.Normal;
                string receiverId = await _messageService.Create(message, User.Identity.Name);
                if (string.IsNullOrEmpty(receiverId))
                {
                    return Unauthorized();
                }
                await _notificationService.NotifyUpdates(receiverId);
            }
            catch (LogicalException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest(AppSettings.INTERNAL_SERVER_ERROR_MESSAGE);
            }
            return Ok();
        }

        [AuthorizeUser]
        [Route("soft/{id:int}")]
        [HttpDelete]
        public IHttpActionResult SoftDelete(int id)
        {
            try
            {
                _messageService.Delete(id, DeleteState.SoftDelete);
            }
            catch (LogicalException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest(AppSettings.INTERNAL_SERVER_ERROR_MESSAGE);
            }
            return Ok();
        }

        [AuthorizeUser]
        [Route("permanent/{id:int}")]
        [HttpDelete]
        public IHttpActionResult PermanentDelete(int id)
        {
            try
            {
                _messageService.Delete(id, DeleteState.Permanent);
            }
            catch (LogicalException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest(AppSettings.INTERNAL_SERVER_ERROR_MESSAGE);
            }
            return Ok();
        }

        [AuthorizeUser]
        [Route("")]
        public IHttpActionResult DeleteAll(string items)
        {
            if (!string.IsNullOrEmpty(items))
            {
                int rowsAffected = 0;
                try
                {
                    rowsAffected = _messageService.DeleteAll(items);
                }
                catch (LogicalException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch
                {
                    return BadRequest(AppSettings.INTERNAL_SERVER_ERROR_MESSAGE);
                }
                return Ok(new { rowsAffected });
            }

            return BadRequest(AppSettings.BAD_REQUEST_ERROR_MESSAGE);
        }
    }
}
