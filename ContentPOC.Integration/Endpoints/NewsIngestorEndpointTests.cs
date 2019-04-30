using FluentAssertions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ContentPOC.Integration.Endpoints
{
    // TODO: Re-think the verbs we use

    public class NewsIngestorEndpointTests : IngestNewsItemTestSetup
    {
        [Fact]
        public void ShouldReturn200Response_WhenPostingXml() =>
            XmlPostResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        [Fact]
        public async Task ShouldReturnNewsResponse_WhenPostingXml() => 
            await XmlPostResponse.Content.ReadAsAsync<NewsDto>()
            .ContinueWith(content => AssertResponse(content.Result));
        
        [Fact]
        public void ShouldReturnUri_WhenPostingXml() =>
            XmlPostResponse.Headers.Location.ToString()
            .Should().Be($"news/{NEWS_ID}");

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

        private void AssertResponse(NewsDto content)
        {
            content.meta.href.Should().Be("news/A357D733");
            content.children[0].meta.href.Should().Be("news/headlines/A357D733");
            content.children[0].value.Should().Be("This is a headlines");
            content.children[1].meta.href.Should().Be("news/story-summaries/A357D733");
            content.children[1].value.Should().Be("This is a summary");
            content.children[2].meta.href.Should().Be("news/story-text/A357D733");
            content.children[2].value.Should().Be("Lorem ipsum");
        }

        public class NewsDto
        {
            public string _namespace { get; set; }
            public Meta meta { get; set; }
            public Child[] children { get; set; }
        }
    }
}
