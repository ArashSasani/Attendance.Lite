using CMS.Core.Model;
using CMS.Data.Identity;
using CMS.Data.Repositories;
using System;
using System.Net.Http;
using System.Security.Principal;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http.Filters;
using WebApplication.Infrastructure.Logging;

namespace WebApplication.API.Infrastructure
{
    public class LogUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var actionContext = actionExecutedContext.ActionContext;
            var request = actionContext.Request;
            var identity = actionContext.RequestContext.Principal.Identity;

            string userIP = GetClientIp(request) ?? "?";
            if (IgnoreLogging(request, identity, userIP))
                return;

            if (identity != null && identity.IsAuthenticated)
            {
                var userAgent = request.Headers.UserAgent.ToString();

                string apiRoute = actionContext.ControllerContext.RouteData.Route.RouteTemplate;
                string httpMethod = actionContext.ControllerContext.Request.Method.Method;
                string path = httpMethod + " " + apiRoute;

                using (var _authRepo = new AuthRepository())
                {
                    var user = _authRepo.FindUserByUsername(identity.Name);
                    if (user != null)
                    {
                        //get operation title by path
                        var accessPath = _authRepo.GetAccessPath(path);
                        if (accessPath != null)
                        {
                            string operation = accessPath.Title;

                            using (var _context = new AuthContext())
                            {
                                using (var _loggerRepo = new Repository<UserLog>
                                (_context, new ExceptionLogger(new CustomLogger())))
                                {
                                    _loggerRepo.Insert(new UserLog
                                    {
                                        UserId = user.Id,
                                        Date = DateTime.Now,
                                        IP = userIP,
                                        UserAgent = userAgent,
                                        Operation = operation,
                                    });
                                }
                            }
                        }

                        return;
                    }
                };
            }
        }

        private bool IgnoreLogging(HttpRequestMessage request, IIdentity identity, string ip)
        {
            //don't log local ips -> remove for local prod
            if (ip == "::1" || ip.StartsWith("192.168"))
                return true;

            return false;
        }

        private string GetClientIp(HttpRequestMessage request = null)
        {
            if (request != null)
            {
                if (request.Properties.ContainsKey("MS_HttpContext"))
                {
                    return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                }
                else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
                {
                    RemoteEndpointMessageProperty prop
                        = (RemoteEndpointMessageProperty)request
                        .Properties[RemoteEndpointMessageProperty.Name];
                    return prop.Address;
                }
                else if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Request.UserHostAddress;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
    }
}