using CMS.Service.Dtos.RoleAccessPath;
using CMS.Service.Interfaces;
using System.Transactions;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;

namespace WebApplication.API.Controllers.CMS
{
    [RoutePrefix("api/cms/role/access/paths")]
    public class RoleAccessPathsController : ApiController
    {
        private readonly IAuthService _authService;

        public RoleAccessPathsController(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize]
        [Route("")]
        public IHttpActionResult GetAccessPaths()
        {
            var accessPaths = _authService.GetAccessPaths();
            return Ok(accessPaths);
        }

        [AuthorizeUser]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var role = _authService.GetRoleById(id);
            if (role != null)
            {
                var accessPathIds = _authService.GetAccessPathIdsForRole(id);
                return Ok(accessPathIds);
            }
            return BadRequest("the role is removed");
        }

        [AuthorizeUser]
        [Route("update")]
        [HttpPut]
        public IHttpActionResult Update(RoleAccessPathsDto roleAccessPaths)
        {
            if (roleAccessPaths.RoleId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _authService.UpdateRoleAccessPaths(roleAccessPaths);

                return Ok(new
                {
                    message = "count: " + roleAccessPaths.AccessPaths.Count
                    + " access path(s) assigned"
                });
            }
            catch (LogicalException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TransactionAbortedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest(AppSettings.INTERNAL_SERVER_ERROR_MESSAGE);
            }
        }
    }
}
