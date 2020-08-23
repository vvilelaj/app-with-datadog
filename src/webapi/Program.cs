using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace VVJ.AppWithDataDog.WebApi
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            Console.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_URLS"));
            Console.WriteLine(Environment.GetEnvironmentVariable("DD_TRACE_AGENT_URL"));
            Console.WriteLine(Environment.GetEnvironmentVariable("DD_ENV"));
            Console.WriteLine(Environment.GetEnvironmentVariable("DD_SERVICE"));
            Console.WriteLine(Environment.GetEnvironmentVariable("DD_VERSION"));
            Console.WriteLine(Environment.GetEnvironmentVariable("DD_TAGS"));

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration,sectionName: "Serilog")
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
