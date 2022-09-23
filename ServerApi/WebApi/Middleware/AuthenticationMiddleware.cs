using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {

                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic"))
                {
                    //Extract credentials
                    string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                    int seperatorIndex = usernamePassword.IndexOf(':');
                    var username = usernamePassword.Substring(0, seperatorIndex);
                    var password = usernamePassword.Substring(seperatorIndex + 1);

                    if (username == "test" && password == "test")
                        context.User.AddIdentity(new System.Security.Claims.ClaimsIdentity("Ruth", "Auth", "Authentication"));
                    else
                        context.User.AddIdentity(new System.Security.Claims.ClaimsIdentity("Ruth", "NotAuth", "NotAuthentication"));
                    await _next.Invoke(context);
                }
                else
                {
                    context.User.AddIdentity(new System.Security.Claims.ClaimsIdentity("Ruth", "NotAuth", "NotAuthentication"));
                    await _next.Invoke(context);
                }
            }
            catch(Exception ex)
            { 
                context.Response.StatusCode = 401;
            }

            return;
        }
    }
}
