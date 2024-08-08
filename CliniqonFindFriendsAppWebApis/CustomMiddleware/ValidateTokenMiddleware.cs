// Ignore Spelling: Middleware

using CliniqonFindFriendsAppBussinessLogic.Interfaces;
using CliniqonFindFriendsAppDataAccess.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DrCleanerAppWebApis.CustomMiddleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ValidateTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISecurityB _securityB;
        public ValidateTokenMiddleware(RequestDelegate next, ISecurityB securityB)
        {
            _next = next;
            _securityB = securityB;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            ////getting the token from the header
            //var bearerToken = httpContext.Request.Headers.Authorization.ToString();
            //string token = "";
            //if (AuthenticationHeaderValue.TryParse(bearerToken, out var headerValue))
            //{
            //    // we have a valid AuthenticationHeaderValue that has the following details:
            //    token = headerValue.Parameter;
            //    // parmameter will be the token itself.
            //}
            var token = httpContext.Request.Cookies["refreshToken"];
            var apiPath = httpContext.Request.Path.ToUriComponent();
            if (token != null && apiPath != "/user/login" && apiPath!= "/user/registration")
            {
                if (_securityB == null)
                    Debug.WriteLine("_securityB NUll");
                //checking the token is valid on DB
                bool dbRefreshToken = await _securityB?.GetActiveStatusOfToken(token);
                if (!dbRefreshToken)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await httpContext.Response.WriteAsync(GeneralDTO.UnauthorizedMessage);

                }
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ValidateTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseValidateTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidateTokenMiddleware>();
        }
    }
}
