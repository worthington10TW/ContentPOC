using ContentPOC.Converter;
using ContentPOC.DAL;
using ContentPOC.HostedService;
using ContentPOC.NewsIngestor;
using ContentPOC.Seed;
using ContentPOC.Unit.Model.News;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace ContentPOC
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLogging()
                .AddMvc()
                .AddXmlDataContractSerializerFormatters()
                .AddXmlSerializerFormatters();

            services.AddMvcCore();

            services
                .AddSingleton<DynamicNamespaceManager>()
                .AddSingleton<IConverter<NewsItem>, NewsConverter>()
                .AddSingleton<IRepository, InMemoryStore>()
                .AddTransient<IManager<NewsItem>, NewsManager>()
                .AddSingleton<NewsConverter>()
                .AddSingleton<INotificationQueue, RawNewsContentIngestedQueue>();

            services
                .AddTransient<IHostedService, NotificationHubService>()
                .AddTransient<INotificationHub, SimulationNotificationHub>();

            services
                .AddSingleton<XmlSeeder>()
                .AddHttpClient()
                .AddHttpContextAccessor();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Reimagining content", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                    Console.WriteLine($"Applying xml {xmlPath}");
                }
                else Console.Error.WriteLine($"Could not find path {xmlPath}");
            });

        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            (env.IsDevelopment() ? app.UseDeveloperExceptionPage() : app.UseStatusCodePages())
                .UseStaticFiles()
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                })
                .UseMvc();

            app.ApplicationServices.GetService<XmlSeeder>()
                .SeedAsync(Path.Combine("Seed", "Data"))
                .GetAwaiter().GetResult();
        }
    }
}
