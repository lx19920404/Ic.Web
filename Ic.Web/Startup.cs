//using DevExpress.AspNetCore;
//using DevExpress.AspNetCore.Bootstrap;
using Ic.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MVCSampleApp
{
    public class Startup
    {
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IHostingEnvironment env)
        {
            //添加配置文件
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // 这个方法被运行时调用，使用这个方法向容器内注册服务
        public void ConfigureServices(IServiceCollection services)
        {
            //添加配置文件
            services.AddApplicationInsightsTelemetry(Configuration);
            //配置DevExpress
            //services.AddDevExpressControls(options => {
            //    options.Bootstrap(bootstrap =>
            //    {
            //        bootstrap.Mode = BootstrapMode.Bootstrap4;
            //        bootstrap.IconSet = BootstrapIconSet.FontAwesome;
            //    });
            //    options.Resources = ResourcesType.DevExtreme | ResourcesType.ThirdParty;
            //});
            //添加MVC
            services.AddMvc();
            services.AddSingleton(new mNavConfig(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "conf", "NavConfig.xml")));
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<EventsMenusContext>(options =>
                options.UseSqlServer(Configuration["Data:ConnectionStrings:MyConnection"]))
                .AddDbContext<EventsContext>(options =>
                options.UseSqlServer(Configuration["Data:ConnectionStrings:MyConnection"]))
                .AddDbContext<UsersContext>(options =>
                options.UseSqlServer(Configuration["Data:ConnectionStrings:MyConnection"]));
        }
        public IConfigurationRoot Configuration { get; }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //这个方法被运行时调用，使用这个方法配置HTTP请求管道
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            //配置日志
            loggerFactory.AddConsole();
            //var logger = loggerFactory.CreateLogger<Startup>();

            if (env.IsDevelopment())
            {
                //开发环境使用异常界面
                app.UseDeveloperExceptionPage();
            }
            //app.UseDevExpressControls();
            //使用MVC默认路由
            app.UseMvcWithDefaultRoute();
            //使用静态资源
            app.UseStaticFiles();

            //路由 不传递控制器名称
            app.UseMvc(routes => routes.MapRoute(
                name: "default",
                template: "{action}/{id?}",
                defaults: new { controller = "Home", action = "Index" }
                ));
            //路由 添加额外的项或者使用多个参数
            app.UseMvc(routes => routes.MapRoute(
                name: "default",
                template: "{controller}/{action}/{id?}",
                defaults: new { controller = "Home", action = "Index" }
                ).MapRoute(
                name:"language",
                template:"{language}/{controller}/{action}/{id?}",
                defaults: new { controller = "Home", action = "Index"}
                ));
            //使用路由约束
            app.UseMvc(routes => routes.MapRoute(
                name: "language",
                template: "{language}/{controller}/{action}/{id?}",
                defaults: new { controller = "Home", action = "Index" },
                constraints: new { language = @"(en)|(de)" }
                ));
            //使用路由约束-数字正则表达式
            app.UseMvc(routes => routes.MapRoute(
                name: "products",
                template: "{controller}/{action}/{productId?}",
                defaults: new { controller = "Home", action = "Index" },
                constraints: new { productId = @"\d+" }
                ));
            //带多个参数时的路由
            app.UseMvc(routes => routes.MapRoute(
                name: "default",
                template: "{controller}/{action}/{id?}",
                defaults: new { controller = "Home", action = "Index" }
              ).MapRoute(
                name: "multipleparameters",
                template: "{controller}/{action}/{x}/{y}",
                defaults: new { controller = "Home", action = "Index" },
                constraints: new { x = @"\d", y = @"\d" }));

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
