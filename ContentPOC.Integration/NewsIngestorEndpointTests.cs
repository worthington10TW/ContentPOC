using ContentPOC.DAL;
using ContentPOC.HostedService;
using ContentPOC.Model;
using ContentPOC.Model.News;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ContentPOC.Integration
{
    // TODO: Re-think the verbs we use

    public class NewsIngestorEndpointTests : IDisposable
    {
        private readonly TestServer _testServer;
        private readonly HttpClient _client;
        private readonly HttpResponseMessage _response;
        private readonly Mock<INotificationHub> _mockNotificationHub = new Mock<INotificationHub>();
        private const string ID = "A357D733";

        public NewsIngestorEndpointTests()
        {
            var builder = Program.WebHostBuilder();
            builder.ConfigureTestServices(services => RemoveBackgroundService(services));
            builder.ConfigureTestServices(x => x.AddSingleton(_mockNotificationHub.Object));
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
            AssertResponse(JObject.Parse(content));

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
        public async Task ShouldGetInsertedNewsItem()
        {
            var getResponse = await _client.GetAsync($"/api/news/{ID}");

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await getResponse.Content.ReadAsStringAsync();
            AssertResponse(JObject.Parse(content));
        }

        [Fact]
        public async Task ShouldGetInsertedHeadline()
        {
            var getResponse = await _client.GetAsync($"/api/news/headlines/{ID}/");

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await getResponse.Content.ReadAsStringAsync();
            var value = JObject.Parse(content);
            value.Value<string>("namespace").Should().Be("news/headlines");
            value.Value<string>("value").Should().Be("This is a headlines");
        }

        [Fact]
        public async Task ShouldPublishEventWhenNewsIsIngested()
        {
            var queue = _testServer.Host.Services.GetService<INotificationQueue>();
            RawNewsContentIngested rawNewsContentIngestedEvent = await queue.DequeueAsync(CancellationToken.None);
            rawNewsContentIngestedEvent.Location.Should().Be($"news/{ID}");
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
            _client.Dispose();
        }


        private readonly string _testXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<news>
<headline>This is a headlines</headline>
<summary>This is a summary</summary>
<story>Lorem ipsum</story>
</news>";
        
        private static void AssertResponse(JObject result)
        {
            var value = result.Value<JToken>("children");
            value.Count().Should().Be(3);
            value[0].Value<string>("namespace").Should().Be("news/headlines");
            value[0].Value<string>("value").Should().Be("This is a headlines");
            value[1].Value<string>("namespace").Should().Be("news/story-summaries");
            value[1].Value<string>("value").Should().Be("This is a summary");
            value[2].Value<string>("namespace").Should().Be("news/story-text");
            value[2].Value<string>("value").Should().Be("Lorem ipsum");
        }

    }
}
