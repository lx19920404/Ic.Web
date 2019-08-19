using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ic.GateWay
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    CreateHostBuilder(args).Build().Run();
        //}

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            var config = new ConfigurationBuilder().AddCommandLine(args).Build();
        //            string ip = config["ip"];
        //            string port = config["port"];
        //            webBuilder.UseStartup<Startup>()
        //            .UseUrls($"http://{ip}:{port}")
        //            .ConfigureAppConfiguration((hostingContext, builder) =>
        //            {
        //                builder.AddJsonFile("configuration.json", false, true);
        //            });
        //        });

        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder().AddCommandLine(args).Build();
            string ip = config["ip"];
            string port = config["port"];
            return WebHost.CreateDefaultBuilder(args)
                            .UseStartup<Startup>()
                            .UseUrls($"http://{ip}:{port}")
                            .ConfigureAppConfiguration((hostingContext, builder) =>
                            {
                                builder.AddJsonFile("configuration.json", false, true);
                            })
                            .Build();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config1 = new ConfigurationBuilder().AddCommandLine(args).Build();
            string ip = config1["ip"];
            string port = config1["port"];
            return WebHost.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration((hostingContext, config) =>
             {
                 config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                       .AddJsonFile("Ocelot.json", optional: true, reloadOnChange: true)
                       .AddEnvironmentVariables();
             }).UseStartup<Startup>().UseUrls($"http://{ip}:{port}");
        }
    }
}
