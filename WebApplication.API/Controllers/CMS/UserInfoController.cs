using CMS.Service.Dtos.UserInfo;
using CMS.Service.Interfaces;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;

namespace WebApplication.API.Controllers.CMS
{
    [RoutePrefix("api/cms/user/info")]
    public class UserInfoController : ApiController
    {
        private readonly IAuthService _authService;

        public UserInfoController(IAuthService authService)
        {
            _authService = authService;
        }

        [AuthorizeUser]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var userInfo = await _authService.GetUserInfoAsync(id);
            return Ok(userInfo);
        }

        [Authorize]
        [Route("")]
        public async Task<IHttpActionResult> GetCurrentUserInfo()
        {
            var userInfo = await _authService.GetUserInfoByUsernameAsync(User.Identity.Name);
            return Ok(userInfo);
        }

        [AuthorizeUser]
        [Route("update")]
        [HttpPut]
        public async Task<IHttpActionResult> Update(UpdateUserInfoDto userInfo)
        {
            if (userInfo.UserId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _authService.UpdateUserInfoAsync(userInfo);

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
