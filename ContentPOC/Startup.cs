using ContentPOC.Converter;
using ContentPOC.DAL;
using ContentPOC.HostedService;
using ContentPOC.NewsIngestor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ContentPOC
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLogging()                
                .AddSwaggerGen()
                .AddMvc()
                .AddXmlDataContractSerializerFormatters()
                .AddXmlSerializerFormatters();

            services.AddMvcCore();

            services
                .AddSingleton<IConverter<Unit.News>>(x => new NewsConverter())
                .AddSingleton<IRepository, InMemoryStore>()
                .AddTransient<NewsManager>()
                .AddSingleton<NewsConverter>()
                .AddTransient<IHostedService, NotificationHubService>()
                .AddSingleton<IUnitNotificationQueue, InMemoryUnitNotificationQueue>()
                .AddTransient<INotificationHub, SimulationNotificationHub>();
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            (env.IsDevelopment() ? app.UseDeveloperExceptionPage() : app.UseStatusCodePages())
                .UseStaticFiles()
                .UseSwagger()                
                .UseSwaggerUI()                
                .UseMvc();
        }
    }
}
