using AttendanceManagement.Service.Dtos.Personnel;
using AttendanceManagement.Service.Dtos.Position;
using AttendanceManagement.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers.AttendanceManagement
{
    [RoutePrefix("api/attendance/management/personnel")]
    public class PersonnelController : ApiController
    {
        private readonly IPersonnelService _personnelService;
        private readonly IEmployeementTypeService _employeementTypeService;
        private readonly IGroupCategoryService _groupCategoryService;
        private readonly IWorkUnitService _workUnitService;
        private readonly IPositionService _positionService;
        private readonly IApprovalProcService _approvalProcService;
        private readonly IDismissalService _dismissalService;
        private readonly IDutyService _dutyService;

        private readonly IExceptionLogger _logger;

        public PersonnelController(IPersonnelService personnelService
            , IEmployeementTypeService employeementTypeService
            , IGroupCategoryService groupCategoryService
            , IWorkUnitService workUnitService
            , IPositionService positionService
            , IApprovalProcService approvalProcService
            , IDismissalService dismissalService, IDutyService dutyService
            , IExceptionLogger logger)
        {
            _personnelService = personnelService;
            _employeementTypeService = employeementTypeService;
            _groupCategoryService = groupCategoryService;
            _workUnitService = workUnitService;
            _positionService = positionService;
            _approvalProcService = approvalProcService;
            _dismissalService = dismissalService;
            _dutyService = dutyService;

            _logger = logger;
        }

        [AuthorizeUser]
        [Route("")]
        public IHttpActionResult Get([FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var personnel = _personnelService.Get(null, searchTerm
                , sortItem, sortOrder, pagingQueryString);
            return Ok(personnel);
        }

        [AuthorizeUser]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var personnel = _personnelService.GetById(id, includeExtraData: true);
            if (personnel == null)
            {
                return NotFound();
            }
            return Ok(personnel);
        }

        [Route("group/category/{id:int}")]
        [Authorize]
        public IHttpActionResult GetByGroupCategoryId(int id)
        {
            var personnel = _personnelService.GetByGroupCategory(id);
            return Ok(personnel);
        }


        [Route("work/unit/{id:int}")]
        [Authorize]
        public IHttpActionResult GetByWorkUnitId(int id)
        {
            var personnel = _personnelService.GetByWorkUnit(id);
            return Ok(personnel);
        }

        [Route("search")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult Search(string searchTerm)
        {
            var personnel = _personnelService.Search(searchTerm);
            return Ok(personnel);
        }

        [Route("search/group/category")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult SearchByGroupCategoryId(string searchTerm, int groupCategoryId)
        {
            var personnel = _personnelService.SearchByGroupCategory(searchTerm, groupCategoryId);
            return Ok(personnel);
        }

        [Route("search/work/unit")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult SearchByWorkUnitId(string searchTerm, int workUnitId)
        {
            var personnel = _personnelService.SearchByWorkUnit(searchTerm, workUnitId);
            return Ok(personnel);
        }

        [Route("ddls")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult LoadDDLs()
        {
            var employeementTypes = _employeementTypeService.GetForDDL();
            var groupCategories = _groupCategoryService.GetForDDL();
            var workUnits = _workUnitService.GetForDDL();
            var positions = new List<PositionDDLDto>();
            if (workUnits.Count > 0)
            {
                positions.AddRange(_positionService.GetForDDL(workUnits.First().Id));
            }
            var approvalProcs = _approvalProcService.GetForDDL(null);
            //default dismissals already added for all personnel
            var dismissals = _dismissalService.GetForDDL(DismissalSystemTypeAccess.Customized);
            var duties = _dutyService.GetForDDL();
            return Ok(new
            {
                employeementTypes,
                groupCategories,
                workUnits,
                positions,
                approvalProcs,
                dismissals,
                duties
            });
        }

        [AuthorizeUser]
        [Route("create")]
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody] CreatePersonnelDto personnel)
        {
            if (personnel == null)
            {
                return BadRequest();
            }
            //custom validations
            var validator = new CreatePersonnelDtoValidator();
            var results = validator.Validate(personnel);
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
                var result = await _personnelService.Create(personnel);
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
        public IHttpActionResult Update([FromBody] UpdatePersonnelDto personnel)
        {
            if (personnel == null)
            {
                return BadRequest();
            }
            //custom validations
            var validator = new UpdatePersonnelDtoValidator();
            var results = validator.Validate(personnel);
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
                var result = _personnelService.Update(personnel);
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
        [Route("update/{id:int}")]
        [HttpPatch]
        public IHttpActionResult Update([FromBody]
            JsonPatch.JsonPatchDocument<PersonnelPatchDto> patchDoc, int id)
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
            var _partialUpdateDto = _personnelService.PrepareForPartialUpdate(id);
            if (_partialUpdateDto == null)
            {
                return BadRequest(AppSettings.NOT_FOUND_ERROR_MESSAGE);
            }
            try
            {
                patchDoc.ApplyUpdatesTo(_partialUpdateDto.PatchDto);
                _personnelService.ApplyPartialUpdate(_partialUpdateDto);
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
        public async Task<IHttpActionResult> SoftDelete(int id)
        {
            try
            {
                await _personnelService.Delete(id, DeleteState.SoftDelete);
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
                _personnelService.Delete(id, DeleteState.Permanent);
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
        public async Task<IHttpActionResult> DeleteAll(string items)
        {
            if (!string.IsNullOrEmpty(items))
            {
                int rowsAffected;
                try
                {
                    rowsAffected = await _personnelService.DeleteAll(items);
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
