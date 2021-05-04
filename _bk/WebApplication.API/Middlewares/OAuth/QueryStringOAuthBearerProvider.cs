﻿using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace WebApplication.API.Middlewares.OAuth
{
    [EnableCors("*", "*", "*")]
    public class QueryStringOAuthBearerProvider : OAuthBearerAuthenticationProvider
    {
        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var value = context.Request.Query.Get("access_token");

            if (!string.IsNullOrEmpty(value))
            {
                context.Token = value;
            }

            return Task.FromResult<object>(null);
        }
    }
}