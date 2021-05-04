using AttendanceManagement.Service.Interfaces;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;

namespace WebApplication.API.Controllers.AttendanceManagement
{
    [RoutePrefix("api/attendance/management/personnel/shift/assignments")]
    public class PersonnelShiftAssignmentsController : ApiController
    {
        private readonly IPersonnelService _personnelService;
        private readonly IPersonnelShiftAssignmentService _personnelShiftAssignmentService;
        private readonly IExceptionLogger _logger;

        public PersonnelShiftAssignmentsController(IPersonnelService personnelService
            , IPersonnelShiftAssignmentService personnelShiftAssignmentService
            , IExceptionLogger logger)
        {
            _personnelService = personnelService;
            _personnelShiftAssignmentService = personnelShiftAssignmentService;
            _logger = logger;
        }

        [AuthorizeUser]
        [Route("")]
        public IHttpActionResult Get([FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {
            var personnel = _personnelService.Get(User.Identity.Name, searchTerm
               , sortItem, sortOrder, pagingQueryString);
            return Ok(personnel);
        }

        [Route("{personnelId:int}")]
        [Authorize]
        public IHttpActionResult Get(int personnelId)
        {
            var assignments = _personnelShiftAssignmentService.Get(personnelId);
            return Ok(assignments);
        }
    }
}
