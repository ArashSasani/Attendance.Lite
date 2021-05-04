using AttendanceManagement.Service.Dtos.PersonnelShift;
using AttendanceManagement.Service.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers.AttendanceManagement
{
    [RoutePrefix("api/attendance/management/personnel/shifts")]
    public class PersonnelShiftsController : ApiController
    {
        private readonly IPersonnelShiftService _personnelShiftService;
        private readonly IExceptionLogger _logger;

        public PersonnelShiftsController(IPersonnelShiftService personnelShiftService
            , IExceptionLogger logger)
        {
            _personnelShiftService = personnelShiftService;
            _logger = logger;
        }

        [AuthorizeUser]
        [Route("")]
        public IHttpActionResult Get(int shiftId, [FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var personnelShifts = _personnelShiftService.Get(shiftId, searchTerm, sortItem
                , sortOrder, pagingQueryString);
            return Ok(personnelShifts);
        }

        [AuthorizeUser]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var personnelShift = _personnelShiftService.GetById(id);
            if (personnelShift == null)
            {
                return NotFound();
            }
            return Ok(personnelShift);
        }

        [Authorize]
        [Route("ddl")]
        public async Task<IHttpActionResult> GetPersonnelShifts()
        {
            var shifts = await _personnelShiftService.GetPersonnelShifts(User.Identity.Name);
            return Ok(shifts);
        }

        [Authorize]
        [Route("ddl/{personnelId:int}")]
        public IHttpActionResult GetReplacedPersonnelShifts(int personnelId)
        {
            var shifts = _personnelShiftService.GetReplacedPersonnelShifts(personnelId);
            return Ok(shifts);
        }

        [AuthorizeUser]
        [Route("create/pattern")]
        [HttpPost]
        public IHttpActionResult CreateByPattern([FromBody]CreatePersonnelShiftByPatternDto
            personnelShift)
        {
            if (personnelShift == null)
            {
                return BadRequest();
            }
            //custom validations
            var validator = new CreatePersonnelShiftByPatternDtoValidator();
            var results = validator.Validate(personnelShift);
            if (!results.IsValid)
            {
                foreach (var failure in results.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
            }
            if (!ModelState.IsValid)
            {
                string errorMessage = new ModelStateError(_logger).OutputMessage(ModelState);
                return BadRequest(errorMessage);
            }
            try
            {
                var result = _personnelShiftService.CreateByPattern(personnelShift);
                if (!result.IsValid)
                {
                    return BadRequest(result.Message);
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
        [Route("soft/{id:int}")]
        [HttpDelete]
        public IHttpActionResult SoftDelete(int id)
        {
            try
            {
                _personnelShiftService.Delete(id, DeleteState.SoftDelete);
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
                _personnelShiftService.Delete(id, DeleteState.Permanent);
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
                    rowsAffected = _personnelShiftService.DeleteAll(items);
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
