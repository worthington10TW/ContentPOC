using ContentPOC.HostedService;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
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
        private readonly string _newsId;
        public XmlNewsIngestorEndpointTests()
        {
            var content = new StringContent(
                    _testXml,
                    System.Text.Encoding.UTF8,
                    "application/xml");
            _xmlPostResponse = HttpClient
                .PostAsync("/news", content)
                .GetAwaiter()
                .GetResult();
            _newsId = _xmlPostResponse.Headers.Location.ToString().Split("/").Last();
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
            .Should().Be($"http://localhost/news/" + _newsId);

        [Fact]
        public async Task ShouldReturnNotFound_WhenIdDoesNotExist() =>
            await HttpClient.GetAsync($"/news/{Guid.NewGuid()}")
                .ContinueWith(x => x.Result.StatusCode.Should().Be(HttpStatusCode.NotFound));

        [Fact]
        public async Task ShouldGetInsertedNewsItem()
        {
            var getResponse = await HttpClient.GetAsync($"/news/{_newsId}");

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await getResponse.Content.ReadAsAsync<NewsDto>();
            AssertResponse(content);
        }

        [Fact]
        public async Task ShouldPublishEventWhenNewsIsIngested()
        {
            var queue = Services.GetService<INotificationQueue>();
            var rawNewsContentIngestedEvent = await queue.DequeueAsync(CancellationToken.None);
            rawNewsContentIngestedEvent.Location.Should().Be($"news/{_newsId}");
        }

        private void AssertResponse(NewsDto content)
        {
            content.meta.href.Should().Be("news/" + _newsId);
            content.children[0].value.Should().Be("What are the practical implications of this case?");
            content.children[1].value.Should().Be(@"
        The case of AJ v DM provides an illustration of the legal complications which can arise on separation, for international families, with multiple litigation taking place across borders. It highlights the financial limitations imposed where jurisdiction for divorce in England and Wales is based on one party’s sole domicile, which was a particularly acute issue in this case, which involved negligible capital and a reasonably big income. Here, although Cohen J expressed sympathy with the wife’s predicament, he was unable to bestow jurisdiction where it did not exist.
      ");
            content.children[2].value.Should().Be(@"Further afield, there were also proceedings in Australia and St Lucia. In Australia, there was jurisdiction for freestanding financial proceedings which could deal with the assets situated there and address the needs, and in St Lucia, while there was no jurisdiction for divorce, the wife had made an application for leave to remove regarding the parties’ child which, if granted, would provide her with the opportunity to re-ignite the array of financial applications in England and Wales that were either not currently available or paused.");
        }

        private readonly string _testXml = File.ReadAllText(Path.Combine("Endpoints", "News.xml"));
    }
}
