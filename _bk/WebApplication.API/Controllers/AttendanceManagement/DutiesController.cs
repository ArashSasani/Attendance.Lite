using AttendanceManagement.Service.Dtos.Duty;
using AttendanceManagement.Service.Interfaces;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers.AttendanceManagement
{
    [RoutePrefix("api/attendance/management/duties")]
    public class DutiesController : ApiController
    {
        private readonly IDutyService _dutyService;
        private readonly IExceptionLogger _logger;

        public DutiesController(IDutyService dutyService
            , IExceptionLogger logger)
        {
            _dutyService = dutyService;
            _logger = logger;
        }

        [AuthorizeUser]
        [Route("")]
        public IHttpActionResult Get([FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var duties = _dutyService.Get(searchTerm, sortItem, sortOrder
                , pagingQueryString);
            return Ok(duties);
        }

        [AuthorizeUser]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var duty = _dutyService.GetById(id);
            if (duty == null)
            {
                return NotFound();
            }
            return Ok(duty);
        }

        [Route("ddl")]
        [Authorize]
        public IHttpActionResult GetForDDL()
        {
            var duties = _dutyService.GetForDDL();
            return Ok(duties);
        }

        [AuthorizeUser]
        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] CreateDutyDto duty)
        {
            if (duty == null)
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
                _dutyService.Create(duty);
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
        public IHttpActionResult Update([FromBody] UpdateDutyDto duty)
        {
            if (duty == null)
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
                _dutyService.Update(duty);
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
        [Route("update/{id:int}")]
        [HttpPatch]
        public IHttpActionResult Update([FromBody]
            JsonPatch.JsonPatchDocument<DutyPatchDto> patchDoc, int id)
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
            var _partialUpdateDto = _dutyService.PrepareForPartialUpdate(id);
            if (_partialUpdateDto == null)
            {
                return BadRequest(AppSettings.NOT_FOUND_ERROR_MESSAGE);
            }
            try
            {
                patchDoc.ApplyUpdatesTo(_partialUpdateDto.PatchDto);
                _dutyService.ApplyPartialUpdate(_partialUpdateDto);
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
                _dutyService.Delete(id, DeleteState.SoftDelete);
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
                _dutyService.Delete(id, DeleteState.Permanent);
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
                    rowsAffected = _dutyService.DeleteAll(items);
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
