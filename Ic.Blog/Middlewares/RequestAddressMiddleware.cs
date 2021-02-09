using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Blog
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RequestAddressMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestAddressMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            string ip = httpContext.Connection.RemoteIpAddress.ToString();
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RequestAddressMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestAddressMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestAddressMiddleware>();
        }
    }
}
