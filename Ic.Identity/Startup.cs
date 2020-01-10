using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Logging;

namespace Ic.Identity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Ioc DbContext
            services.AddDbContextPool<IdentityDbContext>(options =>
            {
                options.UseMySql(Configuration["DB:MySql"]);
            });

            //Ioc Service & Repository
            services.AddScoped<ILoginUserService, LoginUserService>();
            services.AddScoped<ILoginUserRepository, LoginUserRepository>();

            //IdentiryServer4
            string basePath = PlatformServices.Default.Application.ApplicationBasePath;
            InMemoryConfiguration.Configuration = this.Configuration;
            services.AddIdentityServer()
                //对于Token签名需要一对公钥和私钥，不过IdentityServer为开发者提供了一个AddDeveloperSigningCredential()方法，它会帮我们搞定这个事，并默认存到硬盘中。当切换到生产环境时，还是得使用正儿八经的证书，更换为使用AddSigningCredential()方法。
                //.AddDeveloperSigningCredential()
                //生产环境配置的真实证书
                .AddSigningCredential(new X509Certificate2(Path.Combine(basePath, Configuration["Certificates:CerPath"]), Configuration["Certificates:Password"]))
                //.AddTestUsers(InMemoryConfiguration.GetUsers().ToList())
                .AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources())
                .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources())
                .AddInMemoryClients(InMemoryConfiguration.GetClients())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>()
                ;




            //for QuickStart-UI
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            IdentityModelEventSource.ShowPII = true;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();
            //for QuickStart-UI 为IdentityServer4.QuickStart-UI提供支持
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            


            //***************此处为Consul注册代码********************************************************************
            String ip = Configuration["ip"];//部署到不同服务器的时候不能写成127.0.0.1或者0.0.0.0,因为这是让服务消费者调用的地址
            Int32 port = Int32.Parse(Configuration["port"]);
            //向consul注册服务
            ConsulClient client = new ConsulClient(config => {
                config.Address = new Uri(Configuration["ConsulServer:Uri"]);
                config.Datacenter = Configuration["ConsulServer:Datacenter"];
            });
            Task<WriteResult> result = client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "IdentityService_" + Guid.NewGuid().ToString().Substring(0, 7),//服务编号，不能重复，用Guid最简单
                Name = "IdentityService",//服务的名字
                Address = ip,//我的ip地址(可以被其他应用访问的地址，本地测试可以用127.0.0.1，机房环境中一定要写自己的内网ip地址)
                Port = port,//我的端口
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务停止多久后反注册
                    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                    HTTP = $"http://{ip}:{port}/api/health",//健康检查地址,
                    Timeout = TimeSpan.FromSeconds(5)
                }
            });
            //*******************************************************************************************************
        }
    }
}
