using JsonPatch.Formatting;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApplication.API.Infrastructure;
using WebApplication.Infrastructure;

namespace WebApplication.API
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            #region enabled CORS
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            #endregion

            #region routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            #endregion

            #region media formatter and serialization
            //use HTTP Patch in web api
            config.Formatters.Add(new JsonPatchFormatter());

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));
            #endregion

            #region logging user
            if (AppSettings.IS_USER_LOGGING_ENABLED)
                config.Filters.Add(new LogUserAttribute());
            #endregion

            //for showing explicit details of internal server error on server
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }
    }
}