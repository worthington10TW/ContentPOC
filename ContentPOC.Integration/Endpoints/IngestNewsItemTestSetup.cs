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
        protected readonly HttpResponseMessage XmlPostResponse;
        protected readonly IServiceProvider Services;
        protected const string NEWS_ID = "A357D733";

        public IngestNewsItemTestSetup(params IMock<object>[] mockOverrides)
        {
            var builder = Program.WebHostBuilder();
            builder.ConfigureTestServices(services => RemoveBackgroundService(services));
            mockOverrides.ToList().ForEach(mock =>
            builder.ConfigureTestServices(x => x.AddSingleton(mock.Object)));

            _testServer = new TestServer(builder);
            HttpClient = _testServer.CreateClient();
            var content = new StringContent(
                    _testXml,
                    System.Text.Encoding.UTF8,
                    "application/xml");
            XmlPostResponse = HttpClient
                .PostAsync("/api/news", content)
                .GetAwaiter()
                .GetResult();
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
        
        private readonly string _testXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<news>
<headline>This is a headlines</headline>
<summary>This is a summary</summary>
<story>Lorem ipsum</story>
</news>";
        
        public class Meta
        {
            public string href { get; set; }
        }

        public class Child
        {
            public string value { get; set; }
            public Meta meta { get; set; }
        }
    }
}
