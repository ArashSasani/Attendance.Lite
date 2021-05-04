using AttendanceManagement.Service.Dtos.PersonnelShiftReplacement;
using AttendanceManagement.Service.Dtos.WorkingHour;
using AttendanceManagement.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.API.Realtime.Interfaces;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers.AttendanceManagement
{
    [RoutePrefix("api/attendance/management/personnel/shift/replacements")]
    public class PersonnelShiftReplacementsController : ApiController
    {
        private readonly IPersonnelShiftReplacementService _personnelShiftReplacementService;
        private readonly IPersonnelShiftService _personnelShiftService;
        private readonly IWorkingHourService _workingHourService;

        private readonly INotificationService _notificationService;

        private readonly IExceptionLogger _logger;

        public PersonnelShiftReplacementsController(
            IPersonnelShiftReplacementService personnelShiftReplacementService
            , IPersonnelShiftService personnelShiftService, IWorkingHourService workingHourService
            , INotificationService notificationService
            , IExceptionLogger logger)
        {
            _personnelShiftReplacementService = personnelShiftReplacementService;
            _personnelShiftService = personnelShiftService;
            _workingHourService = workingHourService;

            _notificationService = notificationService;

            _logger = logger;
        }

        [AuthorizeUser]
        [Route("")]
        public IHttpActionResult Get([FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var personnelDuties = _personnelShiftReplacementService.Get(User.Identity.Name
                , searchTerm, sortItem, sortOrder, pagingQueryString);
            return Ok(personnelDuties);
        }

        [Authorize]
        [Route("{personnelId:int}")]
        public IHttpActionResult GetReplacedShifts(int personnelId, [FromUri] DateTime fromDate, [FromUri] DateTime toDate)
        {
            var replacedShifts = _personnelShiftReplacementService
                .GetReplacedShifts(personnelId, fromDate, toDate);
            if (replacedShifts == null)
            {
                return NotFound();
            }
            return Ok(replacedShifts);
        }

        [AuthorizeUser]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var personnelShiftReplacement = _personnelShiftReplacementService.GetById(id);
            if (personnelShiftReplacement == null)
            {
                return NotFound();
            }
            return Ok(personnelShiftReplacement);
        }

        [Authorize]
        [Route("ddls")]
        public IHttpActionResult GetDDLs([FromUri] int personnelId, [FromUri] int replacedPersonnelId)
        {
            var shifts = _personnelShiftService.GetReplacedPersonnelShifts(personnelId);
            var replacedShifts = _personnelShiftService.GetReplacedPersonnelShifts(replacedPersonnelId);
            var workingHours = new List<WorkingHourDDLDto>();
            foreach (var shift in shifts)
            {
                workingHours.AddRange(_workingHourService.GetForDDL(shift.Id));
            }
            var replacedWorkingHours = new List<WorkingHourDDLDto>();
            foreach (var shift in replacedShifts)
            {
                replacedWorkingHours.AddRange(_workingHourService.GetForDDL(shift.Id));
            }
            return Ok(new
            {
                shifts,
                replacedShifts,
                workingHours,
                replacedWorkingHours
            });
        }

        [AuthorizeUser]
        [Route("create")]
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody]CreatePersonnelShiftReplacementDto
            personnelShiftReplacement)
        {
            if (personnelShiftReplacement == null)
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
                var result = await _personnelShiftReplacementService
                    .Create(personnelShiftReplacement, User.Identity.Name);
                if (result.ReturnId != null)
                {
                    await _notificationService.NotifyUpdates(result.ReturnId);
                }
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

        [AuthorizeUser]
        [Route("update")]
        [HttpPut]
        public async Task<IHttpActionResult> Update([FromBody]UpdatePersonnelShiftReplacementDto
            personnelShiftReplacement)
        {
            if (personnelShiftReplacement == null)
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
                var result = await _personnelShiftReplacementService.Update(personnelShiftReplacement);
                if (result.ReturnId != null)
                {
                    await _notificationService.NotifyUpdates(result.ReturnId);
                }
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

        [AuthorizeUser]
        [Route("action")]
        [HttpPost]
        public async Task<IHttpActionResult> Action(PersonnelShiftReplacementActionDto
            personnelShiftReplacementAction)
        {
            if (personnelShiftReplacementAction == null)
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
                var result = await _personnelShiftReplacementService.Action(personnelShiftReplacementAction);
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
        [Route("action/list")]
        [HttpPost]
        public async Task<IHttpActionResult> ActionList(List<PersonnelShiftReplacementActionDto>
            personnelShiftReplacementActions)
        {
            if (personnelShiftReplacementActions.Count == 0)
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
                var result = await _personnelShiftReplacementService.Action(personnelShiftReplacementActions);
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
                var result = _personnelShiftReplacementService.Delete(id, DeleteState.SoftDelete);
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
        [Route("permanent/{id:int}")]
        [HttpDelete]
        public IHttpActionResult PermanentDelete(int id)
        {
            try
            {
                var result = _personnelShiftReplacementService.Delete(id, DeleteState.Permanent);
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
    }
}
