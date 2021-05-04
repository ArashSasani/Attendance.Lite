using System;
using System.Web.Http;
using WebApplication.Infrastructure.Enums;
using WebApplication.Infrastructure.Localization;

namespace WebApplication.API.Controllers
{
    [RoutePrefix("api/index")]
    public class IndexController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("date")]
        public IHttpActionResult CurrentDate()
        {
            DateTime now = DateTime.Now;
            string date = now.ToShortDateString();
            string dayOfWeek = now.DayOfWeek.GetDayOfWeekGC(CultureInfoTag.English_US, OutputDateFormat.Complete);
            return Ok(new { date, dayOfWeek });
        }
    }
}
