using AttendanceManagement.Service.Dtos.WorkUnit;
using AttendanceManagement.Service.Interfaces;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers.AttendanceManagement
{
    [RoutePrefix("api/attendance/management/work/units")]
    public class WorkUnitsController : ApiController
    {
        private readonly IWorkUnitService _workUnitService;
        private readonly IExceptionLogger _logger;

        public WorkUnitsController(IWorkUnitService workUnitService
            , IExceptionLogger logger)
        {
            _workUnitService = workUnitService;
            _logger = logger;
        }

        [AuthorizeUser]
        [Route("")]
        public IHttpActionResult Get([FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var workUnits = _workUnitService.Get(searchTerm, sortItem, sortOrder
                , pagingQueryString);
            return Ok(workUnits);
        }

        [Authorize]
        [Route("ddl")]
        public IHttpActionResult GetForDDL()
        {
            var workUnits = _workUnitService.GetForDDL();
            return Ok(workUnits);
        }

        [AuthorizeUser]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var workUnit = _workUnitService.GetById(id);
            if (workUnit == null)
            {
                return NotFound();
            }
            return Ok(workUnit);
        }

        [AuthorizeUser]
        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] CreateWorkUnitDto workUnit)
        {
            if (workUnit == null)
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
                _workUnitService.Create(workUnit);
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
        public IHttpActionResult Update([FromBody] UpdateWorkUnitDto workUnit)
        {
            if (workUnit == null)
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
                _workUnitService.Update(workUnit);
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
            JsonPatch.JsonPatchDocument<WorkUnitPatchDto> patchDoc, int id)
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
            var _partialUpdateDto = _workUnitService.PrepareForPartialUpdate(id);
            if (_partialUpdateDto == null)
            {
                return BadRequest(AppSettings.NOT_FOUND_ERROR_MESSAGE);
            }
            try
            {
                patchDoc.ApplyUpdatesTo(_partialUpdateDto.PatchDto);
                _workUnitService.ApplyPartialUpdate(_partialUpdateDto);
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
                _workUnitService.Delete(id, DeleteState.SoftDelete);
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
                _workUnitService.Delete(id, DeleteState.Permanent);
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
                    rowsAffected = _workUnitService.DeleteAll(items);
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
