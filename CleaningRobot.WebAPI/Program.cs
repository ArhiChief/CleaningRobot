using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CleaningRobot.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var enviroment = hostingContext.HostingEnvironment;
                        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{enviroment.EnvironmentName}.json", true, true); // merge with enviroment configuration file
                        config.AddEnvironmentVariables();
                        if (args != null && args.Length > 0)
                        {
                            config.AddCommandLine(args);
                        }
                    })
                .UseIISIntegration()
                .UseDefaultServiceProvider((context, options) =>
                    {
                        options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                    })
                .UseStartup<Startup>()
                .Build();
    }
}
