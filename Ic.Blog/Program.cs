using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ic.Blog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("Press any key to continue");
            //Console.ReadKey();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) 
        {
            var config1 = new ConfigurationBuilder().AddCommandLine(args).Build();
            string ip = config1["ip"];
            string port = config1["port"];
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>().UseUrls($"http://{ip}:{port}");
        }
            
    }
}
