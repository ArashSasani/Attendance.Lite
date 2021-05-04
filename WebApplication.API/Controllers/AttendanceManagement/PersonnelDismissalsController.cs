using AttendanceManagement.Service.Dtos.PersonnelDismissal;
using AttendanceManagement.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.API.Realtime.Interfaces;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Localization;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers.AttendanceManagement
{
    [RoutePrefix("api/attendance/management/personnel/dismissals")]
    public class PersonnelDismissalsController : ApiController
    {
        private readonly IPersonnelDismissalService _personnelDismissalService;
        private readonly IDismissalApprovalService _dismissalApprovalService;
        private readonly IDismissalService _dismissalService;

        private readonly INotificationService _notificationService;

        private readonly IExceptionLogger _logger;

        public PersonnelDismissalsController(IPersonnelDismissalService personnelDismissalService
            , IDismissalApprovalService dismissalApprovalService
            , IDismissalService dismissalService
            , INotificationService notificationService
            , IExceptionLogger logger)
        {
            _personnelDismissalService = personnelDismissalService;
            _dismissalApprovalService = dismissalApprovalService;
            _dismissalService = dismissalService;

            _notificationService = notificationService;

            _logger = logger;
        }

        [AuthorizeUser]
        [Route("")]
        public IHttpActionResult Get([FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "", [FromUri] int? dismissalType = null
            , [FromUri] string fromDate = "", [FromUri] string toDate = "")
        {
            var personnelDismissals = _personnelDismissalService
                .Get(User.Identity.Name, dismissalType, fromDate, toDate, searchTerm
                , sortItem, sortOrder, pagingQueryString);
            return Ok(personnelDismissals);
        }

        [AuthorizeUser]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var personnelDismissal = _personnelDismissalService.GetById(id);
            if (personnelDismissal == null)
            {
                return NotFound();
            }
            return Ok(personnelDismissal);
        }

        [Route("ddl")]
        [Authorize]
        public async Task<IHttpActionResult> GetApprovalsDDL()
        {
            var approvals = await _dismissalApprovalService.GetForDDL(User.Identity.Name);
            return Ok(approvals);
        }

        [Authorize]
        [Route("charts/{dismissalId:int}")]
        public async Task<IHttpActionResult> GetChartsInfo(int dismissalId)
        {
            //the logic shoud not be in controller!!!
            var dismissal = _dismissalService.GetById(dismissalId);
            if (dismissal == null)
            {
                return BadRequest();
            }
            var usedChartInfo = await _personnelDismissalService
                .GetChartInfo(User.Identity.Name, dismissalId);
            var limitChartInfo = _dismissalService.GetChartInfo(dismissalId);

            switch (dismissal.DismissalType)
            {
                case DismissalType.Demanded:
                    usedChartInfo.MonthData.UsedDismissalPercentage =
                        (((float)usedChartInfo.MonthData.UsedDismissalDuration.GetSecondsFromDuration() /
                        limitChartInfo.LimitValueForMonth.GetSecondsFromDuration()) * 100).ToString("n2");
                    usedChartInfo.YearData.UsedDismissalPercentage =
                        (((float)usedChartInfo.YearData.UsedDismissalDuration.GetSecondsFromDuration() /
                        limitChartInfo.LimitValueForYear.GetSecondsFromDuration()) * 100).ToString("n2");
                    break;
                case DismissalType.Sickness:
                    usedChartInfo.YearData.UsedDismissalPercentage =
                        (((float)usedChartInfo.YearData.UsedDismissalDuration.GetSecondsFromDuration() /
                        limitChartInfo.LimitValueForYear.GetSecondsFromDuration()) * 100).ToString("n2");
                    break;
                case DismissalType.WithoutSalary:
                    usedChartInfo.MonthData.UsedDismissalPercentage =
                        (((float)usedChartInfo.MonthData.UsedDismissalDuration.GetSecondsFromDuration() /
                        limitChartInfo.LimitValueForMonth.GetSecondsFromDuration()) * 100).ToString("n2");
                    break;
                case DismissalType.Encouragement:
                    break;
                case DismissalType.BreastFeeding:
                    usedChartInfo.DayData.UsedDismissalPercentage =
                        (((float)usedChartInfo.DayData.UsedDismissalDuration.GetSecondsFromDuration() /
                        limitChartInfo.LimitValueForDay.GetSecondsFromDuration()) * 100).ToString("n2");
                    usedChartInfo.TotalData.UsedDismissalPercentage =
                        (((float)usedChartInfo.TotalData.UsedDismissalDuration.GetSecondsFromDuration() /
                        limitChartInfo.LimitValueForTotal.GetSecondsFromDuration()) * 100).ToString("n2");
                    break;
                case DismissalType.Marriage:
                case DismissalType.ChildBirth:
                case DismissalType.DeathOfRelatives:
                    usedChartInfo.TotalData.UsedDismissalPercentage =
                        (((float)usedChartInfo.TotalData.UsedDismissalDuration.GetSecondsFromDuration() /
                        limitChartInfo.LimitValueForTotal.GetSecondsFromDuration()) * 100).ToString("n2");
                    break;
            }

            return Ok(new
            {
                usedChartInfo,
                limitChartInfo,
                dismissalType = dismissal.DismissalType
            });
        }

        [AuthorizeUser]
        [Route("create")]
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody]CreatePersonnelDismissalDto
            personnelDismissal)
        {
            if (personnelDismissal == null)
            {
                return BadRequest();
            }
            //custom validations
            var validator = new CreatePersonnelDismissalDtoValidator();
            var results = validator.Validate(personnelDismissal);
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
                var result = await _personnelDismissalService
                    .Create(personnelDismissal, User.Identity.Name);
                if (result.ReturnId != null)
                {
                    await _notificationService.NotifyUpdates(result.ReturnId);
                }
                if (!result.IsValid)
                    return BadRequest(result.Message);
                if (result.IsValid && result.Message != null) //alarm
                    return Ok(new { message = result.Message });
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
        public async Task<IHttpActionResult> Update([FromBody]UpdatePersonnelDismissalDto
            personnelDismissal)
        {
            if (personnelDismissal == null)
            {
                return BadRequest();
            }
            //custom validations
            var validator = new UpdatePersonnelDismissalDtoValidator();
            var results = validator.Validate(personnelDismissal);
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
                var result = await _personnelDismissalService.Update(personnelDismissal);
                if (result.ReturnId != null)
                {
                    await _notificationService.NotifyUpdates(result.ReturnId);
                }
                if (!result.IsValid)
                    return BadRequest(result.Message);
                if (result.IsValid && result.Message != null) //alarm
                    return Ok(new { message = result.Message });
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
        public async Task<IHttpActionResult> Action(PersonnelDismissalActionDto personnelDismissalAction)
        {
            if (personnelDismissalAction == null)
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
                var result = await _personnelDismissalService.Action(personnelDismissalAction);
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
        public async Task<IHttpActionResult> ActionList(List<PersonnelDismissalActionDto> personnelDismissalActions)
        {
            if (personnelDismissalActions.Count == 0)
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
                var result = await _personnelDismissalService.Action(personnelDismissalActions);
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
                var result = _personnelDismissalService.Delete(id, DeleteState.SoftDelete);
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
                var result = _personnelDismissalService.Delete(id, DeleteState.Permanent);
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
