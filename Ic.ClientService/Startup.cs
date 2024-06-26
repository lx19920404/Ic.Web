﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Ic.ClientService.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ic.ClientService
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
            services.AddMvc(options => {
                options.UseCentralRoutePrefix(new RouteAttribute("api/"));
                options.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);
            //IdentityServer
            services.AddMvcCore().AddAuthorization();
            services.AddAuthentication(Configuration["IdentityService:DefaultScheme"])
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["IdentityService:Uri"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(Configuration["IdentityService:UseHttps"]);
                    //options.ApiName = "clientservice";//Configuration["Service:Name"]; // match with configuration in IdentityServer
                });

            //Swagger
            //........
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //UseAuthentication
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();


            //***************此处为Consul注册代码********************************************************************
            //String ip = Configuration["ip"];//部署到不同服务器的时候不能写成127.0.0.1或者0.0.0.0,因为这是让服务消费者调用的地址
            //Int32 port = Int32.Parse(Configuration["port"]);
            ////向consul注册服务
            //ConsulClient client = new ConsulClient(config=> {
            //    config.Address = new Uri(Configuration["ConsulServer:Uri"]);
            //    config.Datacenter = Configuration["ConsulServer:Datacenter"];
            //});
            //Task<WriteResult> result = client.Agent.ServiceRegister(new AgentServiceRegistration()
            //{
            //    ID = "ClientService_" + Guid.NewGuid().ToString().Substring(0,7),//服务编号，不能重复，用Guid最简单
            //    Name = "ClientService",//服务的名字
            //    Address = ip,//我的ip地址(可以被其他应用访问的地址，本地测试可以用127.0.0.1，机房环境中一定要写自己的内网ip地址)
            //    Port = port,//我的端口
            //    Check = new AgentServiceCheck()
            //    {
            //        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务停止多久后反注册
            //        Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
            //        HTTP = $"http://{ip}:{port}/api/health",//健康检查地址,
            //        Timeout = TimeSpan.FromSeconds(5)
            //    }
            //});
            //*******************************************************************************************************
        }
    }
}
