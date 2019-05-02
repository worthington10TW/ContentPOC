using ContentPOC.HostedService;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static ContentPOC.Integration.Endpoints.Dto;

namespace ContentPOC.Integration.Endpoints
{
    // TODO: Re-think the verbs we use

    public class XmlNewsIngestorEndpointTests : IngestNewsItemTestSetup
    {
        protected readonly HttpResponseMessage _xmlPostResponse;
        public XmlNewsIngestorEndpointTests()
        {
            var content = new StringContent(
                    _testXml,
                    System.Text.Encoding.UTF8,
                    "application/xml");
            _xmlPostResponse = HttpClient
                .PostAsync("/api/news", content)
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public void ShouldReturn200Response_WhenPostingXml() =>
            _xmlPostResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        [Fact]
        public async Task ShouldReturnNewsResponse_WhenPostingXml() =>
            await _xmlPostResponse.Content.ReadAsAsync<NewsDto>()
            .ContinueWith(content => AssertResponse(content.Result));

        [Fact]
        public void ShouldReturnUri_WhenPostingXml() =>
            _xmlPostResponse.Headers.Location.ToString()
            .Should().Be($"http://localhost/news/" + NEWS_ID);

        [Fact]
        public async Task ShouldReturnNotFound_WhenIdDoesNotExist() =>
            await HttpClient.GetAsync($"/api/news/{Guid.NewGuid()}")
                .ContinueWith(x => x.Result.StatusCode.Should().Be(HttpStatusCode.NotFound));

        [Fact]
        public async Task ShouldGetInsertedNewsItem()
        {
            var getResponse = await HttpClient.GetAsync($"/api/news/{NEWS_ID}");

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await getResponse.Content.ReadAsAsync<NewsDto>();
            AssertResponse(content);
        }

        [Fact]
        public async Task ShouldPublishEventWhenNewsIsIngested()
        {
            var queue = Services.GetService<INotificationQueue>();
            var rawNewsContentIngestedEvent = await queue.DequeueAsync(CancellationToken.None);
            rawNewsContentIngestedEvent.Location.Should().Be($"news/{NEWS_ID}");
        }

        private void AssertResponse(NewsDto content)
        {
            content.meta.href.Should().Be("news/55F02F12");
            content.children[0].meta.href.Should().Be("news/headlines/58EFB077");
            content.children[0].value.Should().Be("This is a headlines");
            content.children[1].meta.href.Should().Be("news/story-summaries/155A557C");
            content.children[1].value.Should().Be("This is a summary");
            content.children[2].meta.href.Should().Be("news/story-text/2151A298");
            content.children[2].value.Should().Be("Lorem ipsum");
        }

        private readonly string _testXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<news>
<headline>This is a headlines</headline>
<summary>This is a summary</summary>
<story>Lorem ipsum</story>
</news>";
    }
}
