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
            await _xmlPostResponse.Content.ReadAsAsync<AllNewsDto>()
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
        
        private void AssertResponse(AllNewsDto content)
        {
            content.meta.href.Should().Be("news/" + _newsId);
            content.value.Should().Be("Examining the impact of Brexit and UK-wide common frameworks on devolution ");
        }

        private void AssertResponse(NewsDto content)
        {
            content.meta.href.Should().Be("news/" + _newsId);
            content.children[0].value.Should().Be("Examining the impact of Brexit and UK-wide common frameworks on devolution ");
            content.children[1].value.Should().Be(@"Public Law analysis: Kenneth Campbell QC, MCIArb, advocate at Arnot Manderson Advocates and barrister at Lamb Building, considers the interaction between Brexit and UK devolution, the use of UK-wide common legislative frameworks to maintain harmonisation in the UK market once powers and competences exercised at EU level are repatriated, and the impact for the devolved administrations in Scotland, Wales and Northern Ireland.");
            content.children[2].value.Should().Be(@"All three devolution structures in the UK involve interaction with areas of EU law and EU competence, and that interaction is complex. As the UK devolution settlements are asymmetrical, a different range of powers is relevant to Scotland, Wales and Northern Ireland.");
        }

        private readonly string _testXml = File.ReadAllText(
            Path.Combine(
                "Endpoints",
                "3227114_Examining the impact of Brexit and UK-wide common frameworks on devolution_preview.xml"));
    }
}
