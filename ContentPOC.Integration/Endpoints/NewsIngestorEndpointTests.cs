using FluentAssertions;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
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
        public async Task ShouldReturnNewsResponse_WhenPostingXml()
        {
            var content = await XmlPostResponse.Content.ReadAsStringAsync();
            AssertResponse(JObject.Parse(content));

        }
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
            var content = await getResponse.Content.ReadAsStringAsync();
            AssertResponse(JObject.Parse(content));
        }

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
