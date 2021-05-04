using AttendanceManagement.Service.Dtos.WorkingHour;
using AttendanceManagement.Service.Interfaces;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers.AttendanceManagement
{
    [RoutePrefix("api/attendance/management/working/hours")]
    public class WorkingHoursController : ApiController
    {
        private readonly IWorkingHourService _workingHourService;
        private readonly IExceptionLogger _logger;

        public WorkingHoursController(IWorkingHourService workingHourService
            , IExceptionLogger logger)
        {
            _workingHourService = workingHourService;
            _logger = logger;
        }

        [AuthorizeUser]
        [Route("")]
        public IHttpActionResult Get(int shiftId, [FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var workingHours = _workingHourService.Get(shiftId, searchTerm, sortItem, sortOrder
                , pagingQueryString);
            return Ok(workingHours);
        }

        [Authorize]
        [Route("ddl/{shiftId:int}")]
        public IHttpActionResult GetForDDL(int shiftId)
        {
            var workingHours = _workingHourService.GetForDDL(shiftId);
            return Ok(workingHours);
        }

        [AuthorizeUser]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var workingHour = _workingHourService.GetById(id);
            if (workingHour == null)
            {
                return NotFound();
            }
            return Ok(workingHour);
        }

        [AuthorizeUser]
        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] CreateWorkingHourDto workingHour)
        {
            if (workingHour == null)
            {
                return BadRequest();
            }
            //custom validations
            var validator = new CreateWorkingHourDtoValidator();
            var results = validator.Validate(workingHour);
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
                var result = _workingHourService.Create(workingHour);
                if (!result.IsValid)
                {
                    return BadRequest(result.Message);
                }
                return Ok();
            }
            catch (LogicalException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest(AppSettings.INTERNAL_SERVER_ERROR_MESSAGE);
            }
        }

        [AuthorizeUser]
        [Route("update")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] UpdateWorkingHourDto workingHour)
        {
            if (workingHour == null)
            {
                return BadRequest();
            }
            //custom validations
            var validator = new UpdateWorkingHourDtoValidator();
            var results = validator.Validate(workingHour);
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
                var result = _workingHourService.Update(workingHour);
                if (!result.IsValid)
                {
                    return BadRequest(result.Message);
                }
                return Ok();
            }
            catch (LogicalException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest(AppSettings.INTERNAL_SERVER_ERROR_MESSAGE);
            }
        }

        [AuthorizeUser]
        [Route("update/{id:int}")]
        [HttpPatch]
        public IHttpActionResult Update([FromBody]
            JsonPatch.JsonPatchDocument<WorkingHourPatchDto> patchDoc, int id)
        {
            if (patchDoc == null || !patchDoc.HasOperations)
            {
                return BadRequest(AppSettings.PATCH_REQUEST_ERROR_MESSAGE);
            }
            if (patchDoc.Operations
                .Exists(q => q.Operation != JsonPatch.JsonPatchOperationType.replace))
            {
                return BadRequest(AppSettings.PATCH_OPERATION_ERROR_MESSAGE);
            }
            if (!ModelState.IsValid)
            {
                string errorMessage = new ModelStateError(_logger).OutputMessage(ModelState);
                return BadRequest(errorMessage);
            }
            var _partialUpdateDto = _workingHourService.PrepareForPartialUpdate(id);
            if (_partialUpdateDto == null)
            {
                return BadRequest(AppSettings.NOT_FOUND_ERROR_MESSAGE);
            }
            try
            {
                patchDoc.ApplyUpdatesTo(_partialUpdateDto.PatchDto);
                _workingHourService.ApplyPartialUpdate(_partialUpdateDto);
            }
            catch (LogicalException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (JsonPatch.JsonPatchException ex)
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
                _workingHourService.Delete(id, DeleteState.SoftDelete);
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
                _workingHourService.Delete(id, DeleteState.Permanent);
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
                    rowsAffected = _workingHourService.DeleteAll(items);
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
