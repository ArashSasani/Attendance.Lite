using AttendanceManagement.Service.Dtos.PersonnelHourlyShift;
using AttendanceManagement.Service.Interfaces;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers.AttendanceManagement
{
    [AuthorizeUser]
    [RoutePrefix("api/attendance/management/personnel/hourly/shifts")]
    public class PersonnelHourlyShiftsController : ApiController
    {
        private readonly IPersonnelHourlyShiftService _personnelShiftService;
        private readonly IExceptionLogger _logger;

        public PersonnelHourlyShiftsController(IPersonnelHourlyShiftService personnelShiftService
            , IExceptionLogger logger)
        {
            _personnelShiftService = personnelShiftService;
            _logger = logger;
        }

        [Route("")]
        public IHttpActionResult Get(int hourlyShiftId, [FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var personnelShifts = _personnelShiftService.Get(hourlyShiftId, searchTerm, sortItem
                , sortOrder, pagingQueryString);
            return Ok(personnelShifts);
        }

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

        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] CreatePersonnelHourlyShiftDto personnelHourlyShift)
        {
            if (personnelHourlyShift == null)
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
                var result = _personnelShiftService.Create(personnelHourlyShift);
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
