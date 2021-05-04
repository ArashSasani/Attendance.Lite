using AttendanceManagement.Service.Dtos.PersonnelProfile;
using AttendanceManagement.Service.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;

namespace WebApplication.API.Controllers.AttendanceManagement
{
    [AuthorizeUser]
    [RoutePrefix("api/attendance/management/personnel/profile")]
    public class PersonnelProfileController : ApiController
    {
        private readonly IPersonnelProfileService _personnelProfileService;

        private readonly IExceptionLogger _logger;

        public PersonnelProfileController(IPersonnelProfileService personnelProfileService
            , IExceptionLogger logger)
        {
            _personnelProfileService = personnelProfileService;

            _logger = logger;
        }

        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var personnel = await _personnelProfileService.Get(User.Identity.Name);
            if (personnel == null)
            {
                return NotFound();
            }
            return Ok(personnel);
        }

        [Route("update")]
        [HttpPut]
        public async Task<IHttpActionResult> Update([FromBody] UpdatePersonnelProfileDto personnel)
        {
            if (personnel == null)
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
                var username = User.Identity.Name;

                var result = await _personnelProfileService.Update(personnel, username);
                if (!result.IsValid)
                    return BadRequest(result.Message);
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
