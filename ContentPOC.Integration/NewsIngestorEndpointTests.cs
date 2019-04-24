using ContentPOC.Unit;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using System;
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

        public NewsIngestorEndpointTests()
        {
            _testServer = new TestServer(Program.WebHostBuilder());
            _client = _testServer.CreateClient();
            var content = new StringContent(
                    _testXml,
                    System.Text.Encoding.UTF8,
                    "application/xml");
            _response = _client
                .PostAsync("/api/news-ingestor", content)
                .GetAwaiter()
                .GetResult();
        }

        public void Dispose()
        {
            _testServer.Dispose();
            _client.Dispose();
        }

        [Fact]
        public void ShouldReturn200Response_WhenPostingXml() =>
            _response.StatusCode.Should().Be(HttpStatusCode.Created);

        [Fact]
        public async Task ShouldReturnNewsResponse_WhenPostingXml()
        {
            var content = await _response.Content.ReadAsAsync<News>();
            content.Headline.Should().Be("This is a headline");
            content.Summary.Should().Be("This is a summary");
            content.Story.Should().Be("Lorem ipsum");
            content.Href.Should().Be("news/E3B2F8DA");
        }

        [Fact]
        public void ShouldReturnUri_WhenPostingXml() =>
            _response.Headers.Location.ToString()
            .Should().Be("news/E3B2F8DA");

        private readonly string _testXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<news>
<headline>This is a headline</headline>
<summary>This is a summary</summary>
<story>Lorem ipsum</story>
</news>";
    }
}
