using FluentAssertions;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ContentPOC.Integration.Endpoints
{
    public class NewsChildEndpointTests : IngestNewsItemTestSetup
    {
        [Fact]
        public async Task ShouldGetInsertedHeadline()
        {
            var getResponse = await HttpClient.GetAsync($"/api/news/headlines/{NEWS_ID}/");

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await getResponse.Content.ReadAsStringAsync();
            var value = JObject.Parse(content);
            value.Value<string>("namespace").Should().Be("news/headlines");
            value.Value<string>("value").Should().Be("This is a headlines");
        }
    }
}
