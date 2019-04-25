using ContentPOC.DAL;
using ContentPOC.HostedService;
using ContentPOC.Unit;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ContentPOC.Integration
{
    public class NewsIngestorEndpointTests : IDisposable
    {
        private readonly TestServer _testServer;
        private readonly HttpClient _client;
        private readonly HttpResponseMessage _response;
        private readonly Mock<IUnitNotificationQueue> _mockHub = new Mock<IUnitNotificationQueue>();
        private const string ID = "17867F64";

        public NewsIngestorEndpointTests()
        {
            var builder = Program.WebHostBuilder();
            builder.ConfigureTestServices(services => RemoveBackgroundService(services));
            builder.ConfigureTestServices(x => x.AddSingleton(_mockHub.Object));
            _testServer = new TestServer(builder);
            _client = _testServer.CreateClient();
            var content = new StringContent(
                    _testXml,
                    System.Text.Encoding.UTF8,
                    "application/xml");
            _response = _client
                .PostAsync("/api/news", content)
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public void ShouldReturn200Response_WhenPostingXml() =>
            _response.StatusCode.Should().Be(HttpStatusCode.Created);

        [Fact]
        public async Task ShouldReturnNewsResponse_WhenPostingXml()
        {
            var content = await _response.Content.ReadAsStringAsync();
            content.Should().Be(_responseJson);
        }

        [Fact]
        public void ShouldReturnUri_WhenPostingXml() =>
            _response.Headers.Location.ToString()
            .Should().Be($"news/{ID}");

        [Fact]
        public async Task ShouldReturnNotFound_WhenIdDoesNotExist()
        {
            var getResponse = await _client.GetAsync($"/api/news/{Guid.NewGuid()}");

            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldGetInsertedUnit()
        {
            var getResponse = await _client.GetAsync($"/api/news/{ID}");

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await getResponse.Content.ReadAsStringAsync();
            content.Should().Be(_responseJson);
        }

        [Fact]
        public void ShouldNotifyWhenSuccessfullyPosted() =>
             _mockHub.Verify(x => x.Queue(It.Is<IUnit>(unit => unit.Meta.Id.Value == ID)));
        
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
            _client.Dispose();
        }


        private readonly string _testXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<news>
<headline>This is a headline</headline>
<summary>This is a summary</summary>
<story>Lorem ipsum</story>
</news>";

        private readonly string _responseJson = @"[{""unitType"":""Headline"",""value"":""This is a headline"",""meta"":{}},{""unitType"":""Summary"",""value"":""This is a summary"",""meta"":{}},{""unitType"":""Story"",""value"":""Lorem ipsum"",""meta"":{}}]";
    }
}
