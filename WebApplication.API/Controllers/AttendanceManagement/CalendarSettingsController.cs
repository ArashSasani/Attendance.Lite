using AttendanceManagement.Service.Dtos.CalendarDate;
using AttendanceManagement.Service.Interfaces;
using System;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers.AttendanceManagement
{
    [AuthorizeUser]
    [RoutePrefix("api/attendance/management/calendar/settings")]
    public class CalendarSettingsController : ApiController
    {
        private readonly ICalendarDateService _calendarDateService;
        private readonly IExceptionLogger _logger;

        public CalendarSettingsController(ICalendarDateService calendarDateService
            , IExceptionLogger logger)
        {
            _calendarDateService = calendarDateService;
            _logger = logger;
        }

        [Route("")]
        public IHttpActionResult Get([FromUri] DateTime fromDate, [FromUri] DateTime toDate)
        {
            var calendarDates = _calendarDateService.Get(fromDate, toDate);
            return Ok(calendarDates);
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var calendarDate = _calendarDateService.GetById(id);
            if (calendarDate == null)
            {
                return NotFound();
            }
            return Ok(calendarDate);
        }

        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] CreateCalendarDateDto calendarDate)
        {
            if (calendarDate == null)
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
                _calendarDateService.Create(calendarDate);
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

        [Route("update")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] UpdateCalendarDateDto calendarDate)
        {
            if (calendarDate == null)
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
                _calendarDateService.Update(calendarDate);
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
                _calendarDateService.Delete(id, DeleteState.SoftDelete);
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
                _calendarDateService.Delete(id, DeleteState.Permanent);
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
