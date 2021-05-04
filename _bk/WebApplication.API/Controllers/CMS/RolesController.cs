using CMS.Service.Dtos.Role;
using CMS.Service.Interfaces;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;

namespace WebApplication.API.Controllers.CMS
{
    [RoutePrefix("api/cms/roles")]
    public class RolesController : ApiController
    {
        private readonly IAuthService _authService;

        public RolesController(IAuthService authService)
        {
            _authService = authService;
        }

        [AuthorizeUser]
        [Route("")]
        public IHttpActionResult Get([FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var roles = _authService.GetRoles(searchTerm, sortItem, sortOrder);
            return Ok(roles);
        }

        [AuthorizeUser]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var role = _authService.GetRoleById(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [Authorize]
        [Route("check/new")]
        [HttpGet]
        public IHttpActionResult CheckNew(string rolename)
        {
            var userExists = _authService.RoleExists(rolename);
            return Ok(new { roleNameInUse = userExists });
        }

        [Authorize]
        [Route("check")]
        [HttpGet]
        public IHttpActionResult Check(string rolename, string id)
        {
            var userExists = _authService.RoleExists(rolename, id);
            return Ok(new { roleNameInUse = userExists });
        }

        [AuthorizeUser]
        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] CreateRoleDto role)
        {
            try
            {
                _authService.AddRole(role.Name);
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
        public IHttpActionResult Update([FromBody] RoleDto role)
        {
            try
            {
                _authService.UpdateRole(role.Id, role.Name);
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
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                _authService.RemoveRole(id);
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
    }
}
