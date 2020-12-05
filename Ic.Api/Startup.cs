using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ic.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }
        
        public static Task GetRequestSchemeMiddleware(HttpContext context,Func<Task> next)
        {
            string line = $"Request Method:{context.Request.Scheme}";
            next.Invoke();
            return context.Response.WriteAsync(line + "\r\n");
        }
        public static async Task GetRequestMethodMiddleware(HttpContext context, Func<Task> next)
        {
            string line = $"Request Method:{context.Request.Method}";
            await next.Invoke();
            await context.Response.WriteAsync(line + "\r\n");
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(GetRequestSchemeMiddleware);
            app.Use(GetRequestMethodMiddleware);
            app.Use(async (httpContext, func) =>
            {
                await func.Invoke();
            });
            app.Use((httpContext, func) =>
            {
                if (httpContext.Request.Path.Value.Contains("snapped"))
                    return httpContext.Response.WriteAsync("be snapped\r\n");
                return func.Invoke();
            });
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Terminal\r\n");
            });



            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseHttpsRedirection();

            //app.UseRouting();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
