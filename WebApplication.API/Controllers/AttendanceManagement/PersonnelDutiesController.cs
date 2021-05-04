using AttendanceManagement.Service.Dtos.PersonnelDuty;
using AttendanceManagement.Service.Interfaces;
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
    [RoutePrefix("api/attendance/management/personnel/duties")]
    public class PersonnelDutiesController : ApiController
    {
        private readonly IPersonnelDutyService _personnelDutyService;
        private readonly IDutyApprovalService _dutyApprovalService;
        private readonly INotificationService _notificationService;

        private readonly IExceptionLogger _logger;

        public PersonnelDutiesController(IPersonnelDutyService personnelDutyService
            , IDutyApprovalService dutyApprovalService
            , INotificationService notificationService
            , IExceptionLogger logger)
        {
            _personnelDutyService = personnelDutyService;
            _dutyApprovalService = dutyApprovalService;
            _notificationService = notificationService;

            _logger = logger;
        }

        [AuthorizeUser]
        [Route("")]
        public IHttpActionResult Get([FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var personnelDuties = _personnelDutyService.Get(User.Identity.Name
                , searchTerm, sortItem, sortOrder, pagingQueryString);
            return Ok(personnelDuties);
        }

        [AuthorizeUser]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var personnelDuty = _personnelDutyService.GetById(id);
            if (personnelDuty == null)
            {
                return NotFound();
            }
            return Ok(personnelDuty);
        }

        [Route("ddl")]
        [Authorize]
        public async Task<IHttpActionResult> GetApprovalsDDL()
        {
            var approvals = await _dutyApprovalService.GetForDDL(User.Identity.Name);
            return Ok(approvals);
        }

        [AuthorizeUser]
        [Route("create")]
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody]CreatePersonnelDutyDto
            personnelDuty)
        {
            if (personnelDuty == null)
            {
                return BadRequest();
            }
            //custom validations
            var validator = new CreatePersonnelDutyDtoValidator();
            var results = validator.Validate(personnelDuty);
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
                var result = await _personnelDutyService.Create(personnelDuty
                    , User.Identity.Name);
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
        public async Task<IHttpActionResult> Update([FromBody]UpdatePersonnelDutyDto
            personnelDuty)
        {
            if (personnelDuty == null)
            {
                return BadRequest();
            }
            //custom validations
            var validator = new UpdatePersonnelDutyDtoValidator();
            var results = validator.Validate(personnelDuty);
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
                var result = await _personnelDutyService.Update(personnelDuty);
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
        public async Task<IHttpActionResult> Action(PersonnelDutyActionDto personnelDutyAction)
        {
            if (personnelDutyAction == null)
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
                var result = await _personnelDutyService.Action(personnelDutyAction);
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
        public async Task<IHttpActionResult> ActionList(List<PersonnelDutyActionDto> personnelDutyActions)
        {
            if (personnelDutyActions.Count == 0)
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
                var result = await _personnelDutyService.Action(personnelDutyActions);
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
                var result = _personnelDutyService.Delete(id, DeleteState.SoftDelete);
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
                var result = _personnelDutyService.Delete(id, DeleteState.Permanent);
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
