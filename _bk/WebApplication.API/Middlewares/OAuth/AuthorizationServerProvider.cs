using CMS.Data.Repositories;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace WebApplication.API.Middlewares.OAuth
{
    [EnableCors("*", "*", "*")]
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            IList<string> roleNames;
            using (var _repo = new AuthRepository())
            {
                var user = await _repo.FindUserAsync(context.UserName, context.Password);
                if (user == null)
                {
                    context.SetError("username or password is invalid.");
                    //context.Rejected();
                    return;
                }

                roleNames = await _repo.GetRolesForUserAsync(user.Id);
            }

            //add roles and claims
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            foreach (var roleName in roleNames)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
            }
            //generate ticket
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "userName", context.UserName
                    }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }
    }
}