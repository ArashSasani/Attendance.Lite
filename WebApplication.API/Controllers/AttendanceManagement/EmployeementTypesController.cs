using AttendanceManagement.Service.Dtos.EmployeementType;
using AttendanceManagement.Service.Interfaces;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers.AttendanceManagement
{
    [RoutePrefix("api/attendance/management/employeement/types")]
    public class EmployeementTypesController : ApiController
    {
        private readonly IEmployeementTypeService _employeementTypeService;
        private readonly IExceptionLogger _logger;

        public EmployeementTypesController(IEmployeementTypeService employeementTypeService
            , IExceptionLogger logger)
        {
            _employeementTypeService = employeementTypeService;
            _logger = logger;
        }

        [AuthorizeUser]
        [Route("")]
        public IHttpActionResult Get([FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var employeementTypes = _employeementTypeService.Get(searchTerm, sortItem, sortOrder
                , pagingQueryString);
            return Ok(employeementTypes);
        }

        [AuthorizeUser]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var employeementType = _employeementTypeService.GetById(id);
            if (employeementType == null)
            {
                return NotFound();
            }
            return Ok(employeementType);
        }

        [Authorize]
        [Route("ddl")]
        public IHttpActionResult GetForDDL()
        {
            var employeementTypes = _employeementTypeService.GetForDDL();
            return Ok(employeementTypes);
        }

        [AuthorizeUser]
        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] CreateEmployeementTypeDto employeementType)
        {
            if (employeementType == null)
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
                _employeementTypeService.Create(employeementType);
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
        public IHttpActionResult Update([FromBody] UpdateEmployeementTypeDto employeementType)
        {
            if (employeementType == null)
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
                _employeementTypeService.Update(employeementType);
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
            JsonPatch.JsonPatchDocument<EmployeementTypePatchDto> patchDoc, int id)
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
            var _partialUpdateDto = _employeementTypeService.PrepareForPartialUpdate(id);
            if (_partialUpdateDto == null)
            {
                return BadRequest(AppSettings.NOT_FOUND_ERROR_MESSAGE);
            }
            try
            {
                patchDoc.ApplyUpdatesTo(_partialUpdateDto.PatchDto);
                _employeementTypeService.ApplyPartialUpdate(_partialUpdateDto);
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
                _employeementTypeService.Delete(id, DeleteState.SoftDelete);
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
                _employeementTypeService.Delete(id, DeleteState.Permanent);
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
                    rowsAffected = _employeementTypeService.DeleteAll(items);
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
