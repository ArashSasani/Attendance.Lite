using CMS.Service.Dtos.RestrictedAccessTime;
using CMS.Service.Interfaces;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers.CMS
{
    [AuthorizeUser]
    [RoutePrefix("api/cms/restricted/access/times")]
    public class RestrictedAccessTimesController : ApiController
    {
        private readonly IRestrictedAccessTimeService _restrictedAccessTimeService;
        private readonly IExceptionLogger _logger;

        public RestrictedAccessTimesController(IRestrictedAccessTimeService restrictedAccessTimeService
            , IExceptionLogger logger)
        {
            _restrictedAccessTimeService = restrictedAccessTimeService;
            _logger = logger;
        }

        [Route("")]
        public IHttpActionResult Get([FromUri] PagingQueryString pagingQueryString
            , [FromUri] string searchTerm = "", [FromUri] string sortItem = ""
            , [FromUri] string sortOrder = "")
        {

            var restrictedAccessTimes = _restrictedAccessTimeService
                .Get(searchTerm, sortItem, sortOrder, pagingQueryString);

            return Ok(restrictedAccessTimes);
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var restrictedAccessTime = _restrictedAccessTimeService.GetById(id);
            if (restrictedAccessTime == null)
            {
                return NotFound();
            }
            return Ok(restrictedAccessTime);
        }

        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] CreateRestrictedAccessTimeDto restrictedAccessTime)
        {
            if (restrictedAccessTime == null)
            {
                return BadRequest();
            }
            //custom validations
            var validator = new CreateRestrictedAccessTimeDtoValidator();
            var results = validator.Validate(restrictedAccessTime);
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
                _restrictedAccessTimeService.Create(restrictedAccessTime);
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

        [Route("update")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] UpdateRestrictedAccessTimeDto restrictedAccessTime)
        {
            if (restrictedAccessTime == null)
            {
                return BadRequest();
            }
            //custom validations
            var validator = new UpdateRestrictedAccessTimeDtoValidator();
            var results = validator.Validate(restrictedAccessTime);
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
                _restrictedAccessTimeService.Update(restrictedAccessTime);
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

        [Route("soft/{id:int}")]
        [HttpDelete]
        public IHttpActionResult SoftDelete(int id)
        {
            try
            {
                _restrictedAccessTimeService.Delete(id, DeleteState.SoftDelete);
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
                _restrictedAccessTimeService.Delete(id, DeleteState.Permanent);
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
                    rowsAffected = _restrictedAccessTimeService.DeleteAll(items);
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
