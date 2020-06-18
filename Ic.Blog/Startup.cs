using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Consul;
using Ic.Blog.Controllers;
using Microsoft.IdentityModel.Logging;
using Ic.Blog.DbContexts;
using Microsoft.EntityFrameworkCore;
using Westwind.AspNetCore.Markdown;
using Markdig;
using Markdig.Extensions.AutoIdentifiers;

namespace Ic.Blog
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
            services.AddMarkdown(options =>
            {
                options.ConfigureMarkdigPipeline = config =>
                {
                    config
                    .UseEmphasisExtras()
                    .UsePipeTables()
                    .UseGridTables()
                    .UseFooters()
                    .UseFootnotes()
                    .UseCitations()
                    .UseAutoLinks() // URLs are parsed into anchors
                    .UseAutoIdentifiers(AutoIdentifierOptions.Default) // Headers get id="name" 
                    .UseAbbreviations()
                    .UseYamlFrontMatter()
                    .UseEmojiAndSmiley(true)
                    .UseMediaLinks()
                    .UseListExtras()
                    .UseFigures()
                    .UseTaskLists()
                    .UseCustomContainers()
                    .UseGenericAttributes();
                };
            });

            //services.AddMarkdown();

            services.AddDbContextPool<BlogDbContext>(options =>
            {
                options.UseMySql(Configuration["DB:MySql"]);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc(options =>
            {
                //定义全局路由前缀
                options.UseCentralRoutePrefix(new RouteAttribute("blog/"));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //IdentityServer
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // ensure not change any return Claims from Authorization Server
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc"; // oidc => open ID connect
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = $"http://{Configuration["Identity:IP"]}:{Configuration["Identity:Port"]}";
                options.RequireHttpsMetadata = false; // please use https in production env
                options.ClientId = "cas.sg.web.implicit";
                options.ResponseType = "id_token token"; // allow to return access token
                options.SaveTokens = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            IdentityModelEventSource.ShowPII = true;

            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Add("/blog/home/index");    //将index.html改为需要默认起始页的文件名.
            app.UseDefaultFiles(options);


            //app.UseHttpsRedirection();
            //配置验证
            app.UseAuthentication();
            //Markdown
            app.UseMarkdown();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //***************此处为Consul注册代码********************************************************************
            String ip = Configuration["ip"];//部署到不同服务器的时候不能写成127.0.0.1或者0.0.0.0,因为这是让服务消费者调用的地址
            Int32 port = Int32.Parse(Configuration["port"]);
            //向consul注册服务
            ConsulClient client = new ConsulClient(config => {
                config.Address = new Uri(Configuration["ConsulServer:Uri"]);
                config.Datacenter = Configuration["ConsulServer:Datacenter"];
            });
            string newIp = "127.0.0.1";
            Task<WriteResult> result = client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "BlogService_" + Guid.NewGuid().ToString().Substring(0, 7),//服务编号，不能重复，用Guid最简单
                Name = "BlogService",//服务的名字
                Address = newIp,//我的ip地址(可以被其他应用访问的地址，本地测试可以用127.0.0.1，机房环境中一定要写自己的内网ip地址)
                Port = port,//我的端口
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务停止多久后反注册
                    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                    HTTP = $"http://{newIp}:{port}/blog/health",//健康检查地址,
                    Timeout = TimeSpan.FromSeconds(5)
                }
            });
            //*******************************************************************************************************
        }
    }
}
