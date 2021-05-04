using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System.Web.Http;
using WebApplication.API.Infrastructure;
using WebApplication.API.Middlewares.OAuth;
using WebApplication.Infrastructure.Logging;

[assembly: OwinStartup(typeof(WebApplication.API.Startup))]
namespace WebApplication.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            HttpConfiguration httpConfig = new HttpConfiguration();

            UnityConfig.Register(httpConfig);

            ConfigureAuth(app);

            //enable CORS policy
            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins.
                map.UseCors(CorsOptions.AllowAll);
                map.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
                {
                    Provider = new QueryStringOAuthBearerProvider()
                });
                var hubConfiguration = new HubConfiguration
                {
                    Resolver = GlobalHost.DependencyResolver
                };
                map.RunSignalR(hubConfiguration);
            });

            WebApiConfig.Register(httpConfig);

            app.UseWebApi(httpConfig);

            //config logging system
            LoggingConfig.LogToDb();

            //config automapper for the solution
            DtoMapping.Map();
        }
    }
}