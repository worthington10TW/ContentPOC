using FluentAssertions;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ContentPOC.Integration.Endpoints
{
    public class NewsChildEndpointTests
    {
        public class HeadlineTests : IngestNewsItemTestSetup
        {
            private readonly HttpResponseMessage _response;
            
            public HeadlineTests()
            {
                _response = HttpClient.GetAsync($"/api/news/headlines/{NEWS_ID}/")
                    .GetAwaiter()
                    .GetResult();
            }

            [Fact]
            public void ShouldReturn200Response_WhenExist() =>
                _response.StatusCode.Should().Be(HttpStatusCode.OK);

            [Fact]
            public async Task ShouldGetInsertedHeadline()
            {
                var value = await _response.Content.ReadAsAsync<Child>();
                value.meta.href.Should().Be($"news/headlines/{NEWS_ID}");
                value.value.Should().Be("This is a headlines");
            }

            [Fact]
            public async Task ShouldNotRenderChildren_WhenChildrenDoNotExist() =>
                 JObject.Parse(await _response.Content.ReadAsStringAsync())
                    .ContainsKey("children")
                    .Should()
                    .BeFalse();
        }
       
    }
}
