using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace MVCSampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                //Specify Kestrel as the server to be used by the web host.
                .UseKestrel()
                //Specify the content root directory to be used by the web host.
                .UseContentRoot(Directory.GetCurrentDirectory())
                //Configures the port and base path the server should listen on when running behind
                //AspNetCoreModule. The app will also be configured to capture startup errors.
                .UseIISIntegration()
                //Specify the startup type to be used by the web host.
                .UseStartup<Startup>()
                //Builds an Microsoft.AspNetCore.Hosting.IWebHost which hosts a web application.
                .Build();

            host.Run();
        }
    }
}
