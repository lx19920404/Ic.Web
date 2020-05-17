using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ic.Identity
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
            var config = new ConfigurationBuilder().AddCommandLine(args).Build();
            string ip = config["ip"];
            string port = config["port"];
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>().UseUrls($"http://{ip}:{port}");
        }
    }
}
