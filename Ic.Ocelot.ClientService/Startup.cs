using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ic.Ocelot.ClientService
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            String ip = Configuration["ip"];//���𵽲�ͬ��������ʱ����д��127.0.0.1����0.0.0.0,��Ϊ�����÷��������ߵ��õĵ�ַ
            Int32 port = Int32.Parse(Configuration["port"]);
            //��consulע�����
            ConsulClient client = new ConsulClient(ConfigurationOverview);
            Task<WriteResult> result = client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "ClientService",// + Guid.NewGuid(),//�����ţ������ظ�����Guid���
                Name = "ClientService",//���������
                Address = ip,//�ҵ�ip��ַ(���Ա�����Ӧ�÷��ʵĵ�ַ�����ز��Կ�����127.0.0.1������������һ��Ҫд�Լ�������ip��ַ)
                Port = port,//�ҵĶ˿�
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//����ֹͣ��ú�ע��
                    Interval = TimeSpan.FromSeconds(10),//�������ʱ���������߳�Ϊ�������
                    HTTP = $"http://{ip}:{port}/api/health",//��������ַ,
                    Timeout = TimeSpan.FromSeconds(5)
                }
            });
        }
        private static void ConfigurationOverview(ConsulClientConfiguration obj)
        {
            obj.Address = new Uri("http://127.0.0.1:8500");
            obj.Datacenter = "dc1";
        }
    }
}
