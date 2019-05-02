using ContentPOC.DAL;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Linq;
using System.Net.Http;

namespace ContentPOC.Integration.Endpoints
{
    public abstract class IngestNewsItemTestSetup : IDisposable
    {
        private readonly TestServer _testServer;
        protected readonly HttpClient HttpClient;
        protected readonly IServiceProvider Services;

        public IngestNewsItemTestSetup()
        {
            var builder = Program.WebHostBuilder();
            builder.ConfigureTestServices(services => RemoveBackgroundService(services));
            _testServer = new TestServer(builder);
            HttpClient = _testServer.CreateClient();
            Services = _testServer.Host.Services;
        }

        private static void RemoveBackgroundService(IServiceCollection services)
        {
            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IHostedService));
            services.Remove(serviceDescriptor);
        }

        public void Dispose()
        {
            if (_testServer.Host.Services.GetService<IRepository>() is InMemoryStore store)
                store.Reset();

            _testServer.Dispose();
            HttpClient.Dispose();
        }
    }
}
