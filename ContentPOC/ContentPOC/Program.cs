// /*----------------------------------------------------------------------------------------------*/
// /*                                                                                              */
// /*    Copyright © 2017 LexisNexis.  All rights reserved.                                        */
// /*    RELX Group plc trading as LexisNexis - Registered in England - Number 2746621.            */
// /*    Registered Office 1 - 3 Strand, London WC2N 5JR.                                          */
// /*                                                                                              */
// /*----------------------------------------------------------------------------------------------*/

using System;
using System.IO;
using System.Net;
using System.Reflection;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Compact;

namespace ContentPOC
{
    public class Program
    {
        public static readonly string ApplicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static IConfiguration Configuration { get; private set; } 

        public static void Main(string[] args)
        {
            SetupConfiguration(args);

            CreateLogger();

            try
            {
                Log.Information("Starting application");
                WebHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Stopped due to exception");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }
        private static void SetupConfiguration(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
        }

        private static void CreateLogger()
        {
            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext();

            if (Configuration.GetSection("Serilog").GetValue("OutputAsJson", false))
            {
                loggerConfiguration.WriteTo.Console(new RenderedCompactJsonFormatter());
            }
            else
            {
                loggerConfiguration.WriteTo.Console(
                    outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff}|{RequestId}|{Level:u3}|{SourceContext}|{Message:lj}{NewLine}{Exception}");
            }

            Log.Logger = loggerConfiguration.CreateLogger();
        }



        public static IWebHostBuilder WebHostBuilder(params string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any, 5000);
                    options.Listen(IPAddress.Any, 5001);
                })
                .ConfigureMetrics()
                .UseMetrics(options =>
                {
                    options.EndpointOptions = endpointsOptions =>
                    {
                        endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                        endpointsOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                    };
                })
                //.UseMetrics()
                .UseSerilog()
                .UseDefaultServiceProvider(
                    (context, options) => 
                        options.ValidateScopes = context.HostingEnvironment.IsDevelopment())
                .UseStartup<Startup>();
    }
}
