using CMS.Service.Dtos.UserInRole;
using CMS.Service.Interfaces;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;

namespace WebApplication.API.Controllers.CMS
{
    [AuthorizeUser]
    [RoutePrefix("api/cms/user/in/roles")]
    public class UserInRolesController : ApiController
    {
        private readonly IAuthService _authService;

        public UserInRolesController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("{userId}")]
        public async Task<IHttpActionResult> Get(string userId)
        {
            var rolesForUser = await _authService.GetRolesForUserAsync(userId);
            return Ok(rolesForUser);
        }

        [Route("{userId}/{roleId}")]
        public async Task<IHttpActionResult> Get(string userId, string roleId)
        {
            var userInRole = await _authService.GetUserRoleAsync(userId, roleId);
            return Ok(userInRole);
        }

        [Route("create")]
        [HttpPost]
        public async Task<IHttpActionResult> AddUserToRole([FromBody] CreateUserInRoleDto userInRole)
        {
            try
            {
                var result = await _authService.AddUserToRoleAsync(userInRole.UserId, userInRole.RoleId);

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
                return BadRequest(WebApplication.Infrastructure.AppSettings
                    .INTERNAL_SERVER_ERROR_MESSAGE);
            }
            return Ok();
        }

        [Route("{userId}/{roleId}")]
        [HttpDelete]
        public async Task<IHttpActionResult> RemoveUserFromRole(string userId, string roleId)
        {
            try
            {
                var result = await _authService.RemoveUserFromRoleAsync(userId, roleId);

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
                return BadRequest(WebApplication.Infrastructure.AppSettings
                    .INTERNAL_SERVER_ERROR_MESSAGE);
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
