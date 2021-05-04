using CMS.Service.Dtos.User;
using CMS.Service.Interfaces;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;

namespace WebApplication.API.Controllers.CMS
{
    [RoutePrefix("api/cms/users")]
    public class UsersController : ApiController
    {
        private readonly string _deviceUserName = AppSettings.DeviceUserName;

        private readonly IAuthService _authService;

        public UsersController(IAuthService authService)
        {
            _authService = authService;
        }

        [AuthorizeUser]
        [Route("")]
        public IHttpActionResult Get([FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var users = _authService.GetUsersExcept(_deviceUserName, searchTerm, sortItem, sortOrder);
            return Ok(users);
        }

        [Authorize]
        [Route("others")]
        public IHttpActionResult GetOthers([FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var users = _authService.GetUsersExcept(User.Identity.Name, searchTerm
                , sortItem, sortOrder);
            return Ok(users);
        }

        [AuthorizeUser]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var user = await _authService.FindUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [Authorize]
        [Route("check/new")]
        [HttpGet]
        public async Task<IHttpActionResult> CheckNew(string username)
        {
            var userExists = await _authService.UserExistsAsync(username);
            return Ok(new { usernameInUse = userExists });
        }

        [Authorize]
        [Route("check")]
        [HttpGet]
        public async Task<IHttpActionResult> Check(string username, string id)
        {
            var userExists = await _authService.UserExistsAsync(username, id);
            return Ok(new { usernameInUse = userExists });
        }

        [AuthorizeUser]
        [Route("create")]
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody] RegisterUserDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _authService.RegisterUserAsync(user);

                IHttpActionResult errorResult = GetErrorResult(result);

                if (errorResult != null)
                {
                    return errorResult;
                }
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
        [Route("update")]
        [HttpPut]
        public async Task<IHttpActionResult> Update([FromBody] RegisterUserDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _authService.UpdateUserAsync(user);

                IHttpActionResult errorResult = GetErrorResult(result);

                if (errorResult != null)
                {
                    return errorResult;
                }
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
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            try
            {
                var result = await _authService.DeleteUserAsync(id);

                IHttpActionResult errorResult = GetErrorResult(result);

                if (errorResult != null)
                {
                    return errorResult;
                }
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

        #region Helpers

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        #endregion
    }
}
