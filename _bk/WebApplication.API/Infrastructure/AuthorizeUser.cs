using CMS.Data.Repositories;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApplication.API.Infrastructure
{
    public class AuthorizeUser : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var identity = actionContext.RequestContext.Principal.Identity;
            if (identity != null && identity.IsAuthenticated)
            {
                if (IsSysAdmin(identity))
                {
                    return true;
                }

                string apiRoute = actionContext.ControllerContext.RouteData.Route.RouteTemplate;
                string httpMethod = actionContext.ControllerContext.Request.Method.Method;
                string path = httpMethod + " " + apiRoute;

                using (var _repo = new AuthRepository())
                {
                    var user = _repo.FindUserByUsername(identity.Name);
                    if (user != null)
                    {
                        var claimIdentity = (ClaimsIdentity)identity;
                        var roleClaims = claimIdentity.FindAll(c => c.Type == ClaimTypes.Role).ToList();
                        foreach (var roleClaim in roleClaims)
                        {
                            var role = _repo.GetRoleByName(roleClaim.Value);
                            if (role != null) //maybe rolename has changed!
                            {
                                //get role accesses
                                var accesses = _repo.GetAccessPathsByRoleId(role.Id).ToList();
                                if (accesses.Any(a => a.AccessPath.Path.ToLower() == path.ToLower())) //change to hashcode later
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool IsSysAdmin(IIdentity identity)
        {
            var claimIdentity = (ClaimsIdentity)identity;
            return claimIdentity.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "SysAdmin");
        }
    }
}