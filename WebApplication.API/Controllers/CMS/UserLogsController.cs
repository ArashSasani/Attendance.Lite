using CMS.Service.Interfaces;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure.Paging;

namespace WebApplication.API.Controllers.CMS
{
    [AuthorizeUser]
    [RoutePrefix("api/cms/user/logs")]
    public class UserLogsController : ApiController
    {
        private readonly IUserLoggerService _userLoggerService;
        public UserLogsController(IUserLoggerService userLoggerService)
        {
            _userLoggerService = userLoggerService;
        }

        [Route("{userId}")]
        public IHttpActionResult Get(string userId, [FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var userLogs = _userLoggerService.Get(userId, searchTerm, sortItem
                , sortOrder, pagingQueryString);
            return Ok(userLogs);
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var userLog = _userLoggerService.GetById(id);
            if (userLog == null)
            {
                return NotFound();
            }
            return Ok(userLog);
        }
    }
}
