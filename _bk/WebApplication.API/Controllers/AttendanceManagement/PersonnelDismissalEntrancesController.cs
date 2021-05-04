using AttendanceManagement.Service.Dtos.PersonnelDismissalEntrance;
using AttendanceManagement.Service.Interfaces;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers.AttendanceManagement
{
    //[AuthorizeUser]
    [RoutePrefix("api/attendance/management/personnel/dismissal/entrances")]
    public class PersonnelDismissalEntrancesController : ApiController
    {
        private readonly IPersonnelDismissalEntranceService _personnelDismissalEntranceService;
        private readonly IExceptionLogger _logger;

        public PersonnelDismissalEntrancesController(
            IPersonnelDismissalEntranceService personnelDismissalEntranceService
            , IExceptionLogger logger)
        {
            _personnelDismissalEntranceService = personnelDismissalEntranceService;
            _logger = logger;
        }

        [Route("")]
        public IHttpActionResult Get([FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = ""
            , [FromUri] string fromDate = null, [FromUri] string toDate = null)
        {
            var personnelEntrances = _personnelDismissalEntranceService
                .Get(User.Identity.Name, fromDate, toDate, searchTerm, sortItem, sortOrder, pagingQueryString);
            return Ok(personnelEntrances);
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var personnelEntrance = _personnelDismissalEntranceService.GetById(id);
            if (personnelEntrance == null)
            {
                return NotFound();
            }
            return Ok(personnelEntrance);
        }

        [Route("update")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] UpdatePersonnelDismissalEntranceDto personnelEntrance)
        {
            if (personnelEntrance == null)
            {
                return BadRequest();
            }
            //custom validations
            var validator = new UpdatePersonnelDismissalEntranceDtoValidator();
            var results = validator.Validate(personnelEntrance);
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
                var result = _personnelDismissalEntranceService.Update(personnelEntrance);
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

        [Route("update/{id:int}")]
        [HttpPatch]
        public IHttpActionResult Update([FromBody]
            JsonPatch.JsonPatchDocument<PersonnelDismissalEntrancePatchDto> patchDoc, int id)
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
            var _partialUpdateDto = _personnelDismissalEntranceService.PrepareForPartialUpdate(id);
            if (_partialUpdateDto == null)
            {
                return BadRequest(AppSettings.NOT_FOUND_ERROR_MESSAGE);
            }
            try
            {
                patchDoc.ApplyUpdatesTo(_partialUpdateDto.PatchDto);
                _personnelDismissalEntranceService.ApplyPartialUpdate(_partialUpdateDto);
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

        [Route("soft/{id:int}")]
        [HttpDelete]
        public IHttpActionResult SoftDelete(int id)
        {
            try
            {
                _personnelDismissalEntranceService.Delete(id, DeleteState.SoftDelete);
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
                _personnelDismissalEntranceService.Delete(id, DeleteState.Permanent);
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
                    rowsAffected = _personnelDismissalEntranceService.DeleteAll(items);
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
