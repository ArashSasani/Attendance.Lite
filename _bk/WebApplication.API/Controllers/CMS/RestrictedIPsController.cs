using CMS.Service.Dtos.RestrictedIP;
using CMS.Service.Interfaces;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers.CMS
{
    [AuthorizeUser]
    [RoutePrefix("api/cms/restricted/ips")]
    public class RestrictedIPsController : ApiController
    {
        private readonly IRestrictedIPService _restrictedIPService;

        public RestrictedIPsController(IRestrictedIPService restrictedIPService)
        {
            _restrictedIPService = restrictedIPService;
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var restrictedIPs = _restrictedIPService.Get();
            return Ok(restrictedIPs);
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var restrictedIP = _restrictedIPService.GetById(id);
            return Ok(restrictedIP);
        }

        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] CreateRestrictedIPDto restrictedIP)
        {
            if (restrictedIP == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _restrictedIPService.Create(restrictedIP);
            }
            catch (LogicalException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest(AppSettings
                    .INTERNAL_SERVER_ERROR_MESSAGE);
            }
            return Ok();
        }

        [Route("update")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] UpdateRestrictedDto restrictedIP)
        {
            if (restrictedIP == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _restrictedIPService.Update(restrictedIP);
            }
            catch (LogicalException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest(AppSettings
                    .INTERNAL_SERVER_ERROR_MESSAGE);
            }
            return Ok();
        }

        [Route("soft/{id:int}")]
        [HttpDelete]
        public IHttpActionResult SoftDelete(int id)
        {
            try
            {
                _restrictedIPService.Delete(id, DeleteState.SoftDelete);
            }
            catch (LogicalException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest(AppSettings
                    .INTERNAL_SERVER_ERROR_MESSAGE);
            }
            return Ok();
        }

        [Route("permanent/{id:int}")]
        [HttpDelete]
        public IHttpActionResult PermanentDelete(int id)
        {
            try
            {
                _restrictedIPService.Delete(id, DeleteState.Permanent);
            }
            catch (LogicalException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest(AppSettings
                    .INTERNAL_SERVER_ERROR_MESSAGE);
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
                    rowsAffected = _restrictedIPService.DeleteAll(items);
                }
                catch (LogicalException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch
                {
                    return BadRequest(AppSettings
                        .INTERNAL_SERVER_ERROR_MESSAGE);
                }
                return Ok(new { rowsAffected });
            }

            return BadRequest(AppSettings
                .BAD_REQUEST_ERROR_MESSAGE);
        }
    }
}
