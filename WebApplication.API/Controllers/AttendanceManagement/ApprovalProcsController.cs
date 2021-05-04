using AttendanceManagement.Service.Dtos.ApprovalProc;
using AttendanceManagement.Service.Dtos.Personnel;
using AttendanceManagement.Service.Interfaces;
using System.Collections.Generic;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers.AttendanceManagement
{
    [RoutePrefix("api/attendance/management/approval/procs")]
    public class ApprovalProcsController : ApiController
    {
        private readonly IApprovalProcService _ApprovalProcService;
        private readonly IGroupCategoryService _groupCategoriesService;
        private readonly IPersonnelService _personnelService;
        private readonly IExceptionLogger _logger;

        public ApprovalProcsController(IApprovalProcService ApprovalProcService
            , IGroupCategoryService groupCategoriesService
            , IPersonnelService personnelService
            , IExceptionLogger logger)
        {
            _ApprovalProcService = ApprovalProcService;
            _groupCategoriesService = groupCategoriesService;
            _personnelService = personnelService;
            _logger = logger;
        }

        [AuthorizeUser]
        [Route("")]
        public IHttpActionResult Get([FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var approvalProcs = _ApprovalProcService.Get(searchTerm, sortItem, sortOrder
                , pagingQueryString);
            return Ok(approvalProcs);
        }

        [AuthorizeUser]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var ApprovalProc = _ApprovalProcService.GetById(id);
            if (ApprovalProc == null)
            {
                return NotFound();
            }
            return Ok(ApprovalProc);
        }

        [Route("ddls")]
        [Authorize]
        public IHttpActionResult GetDDLs([FromUri] int? exceptionId = null)
        {
            var approvalProcs = _ApprovalProcService.GetForDDL(exceptionId);
            var groupCategories = _groupCategoriesService.GetForDDL();
            //get personnel
            ApprovalProcDto approvalProc = null;
            var firstPriorities = new List<PersonnelRecordDto>();
            var secondPriorities = new List<PersonnelRecordDto>();
            var thirdPriorities = new List<PersonnelRecordDto>();
            if (exceptionId.HasValue)
            {
                approvalProc = _ApprovalProcService.GetById(exceptionId.Value);
                firstPriorities = _personnelService
                    .GetByGroupCategory(approvalProc.FirstPriorityGroupCategoryId);
                if (approvalProc.SecondPriorityGroupCategoryId.HasValue)
                {
                    secondPriorities = _personnelService
                        .GetByGroupCategory(approvalProc.SecondPriorityGroupCategoryId.Value);
                }
                if (approvalProc.ThirdPriorityGroupCategoryId.HasValue)
                {
                    thirdPriorities = _personnelService
                        .GetByGroupCategory(approvalProc.ThirdPriorityGroupCategoryId.Value);
                }
            }

            return Ok(new
            {
                approvalProcs,
                groupCategories,
                firstPriorities,
                secondPriorities,
                thirdPriorities
            });
        }

        [Route("ddl")]
        [Authorize]
        public IHttpActionResult GetForDDL([FromUri] int? exceptionId)
        {
            var approvalProcs = _ApprovalProcService.GetForDDL(exceptionId);
            return Ok(approvalProcs);
        }

        [AuthorizeUser]
        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] CreateApprovalProcDto ApprovalProc)
        {
            if (ApprovalProc == null)
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
                _ApprovalProcService.Create(ApprovalProc);
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
        public IHttpActionResult Update([FromBody] UpdateApprovalProcDto ApprovalProc)
        {
            if (ApprovalProc == null)
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
                _ApprovalProcService.Update(ApprovalProc);
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
            JsonPatch.JsonPatchDocument<ApprovalProcPatchDto> patchDoc, int id)
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
            var _partialUpdateDto = _ApprovalProcService.PrepareForPartialUpdate(id);
            if (_partialUpdateDto == null)
            {
                return BadRequest(AppSettings.NOT_FOUND_ERROR_MESSAGE);
            }
            try
            {
                patchDoc.ApplyUpdatesTo(_partialUpdateDto.PatchDto);
                _ApprovalProcService.ApplyPartialUpdate(_partialUpdateDto);
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
                _ApprovalProcService.Delete(id, DeleteState.SoftDelete);
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
                _ApprovalProcService.Delete(id, DeleteState.Permanent);
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
                    rowsAffected = _ApprovalProcService.DeleteAll(items);
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
