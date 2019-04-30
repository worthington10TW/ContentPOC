using ContentPOC.Converter;
using ContentPOC.DAL;
using ContentPOC.HostedService;
using ContentPOC.NewsIngestor;
using ContentPOC.Unit.Model.News;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
                .AddSingleton<IConverter<NewsItem>>(x => new NewsConverter())
                .AddSingleton<IRepository, InMemoryStore>()
                .AddTransient<NewsManager>()
                .AddSingleton<NewsConverter>()
                .AddSingleton<IUnitNotificationQueue, RawNewsIngestedContentQueue>();

            services
                .AddTransient<IHostedService, NotificationHubService>()
                .AddTransient<INotificationHub, SimulationNotificationHub>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
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
                })
                .UseMvc();
        }
    }
}
