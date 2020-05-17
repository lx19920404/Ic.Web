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
            services.AddMarkdown();


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
                //����ȫ��·��ǰ׺
                options.UseCentralRoutePrefix(new RouteAttribute("api/"));
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

            //app.UseHttpsRedirection();
            //������֤
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

            //***************�˴�ΪConsulע�����********************************************************************
            String ip = Configuration["ip"];//���𵽲�ͬ��������ʱ����д��127.0.0.1����0.0.0.0,��Ϊ�����÷��������ߵ��õĵ�ַ
            Int32 port = Int32.Parse(Configuration["port"]);
            //��consulע�����
            ConsulClient client = new ConsulClient(config => {
                config.Address = new Uri(Configuration["ConsulServer:Uri"]);
                config.Datacenter = Configuration["ConsulServer:Datacenter"];
            });
            string newIp = "127.0.0.1";
            Task<WriteResult> result = client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "BlogService_" + Guid.NewGuid().ToString().Substring(0, 7),//�����ţ������ظ�����Guid���
                Name = "BlogService",//���������
                Address = newIp,//�ҵ�ip��ַ(���Ա�����Ӧ�÷��ʵĵ�ַ�����ز��Կ�����127.0.0.1������������һ��Ҫд�Լ�������ip��ַ)
                Port = port,//�ҵĶ˿�
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//����ֹͣ��ú�ע��
                    Interval = TimeSpan.FromSeconds(10),//�������ʱ���������߳�Ϊ�������
                    HTTP = $"http://{newIp}:{port}/api/health",//��������ַ,
                    Timeout = TimeSpan.FromSeconds(5)
                }
            });
            //*******************************************************************************************************
        }
    }
}
