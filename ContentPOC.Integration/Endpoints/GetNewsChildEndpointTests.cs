using ContentPOC.DAL;
using ContentPOC.Extensions;
using ContentPOC.Model;
using ContentPOC.NewsIngestor;
using ContentPOC.Unit.Model;
using ContentPOC.Unit.Model.News;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static ContentPOC.Integration.Endpoints.Dto;

namespace ContentPOC.Integration.Endpoints
{
    public class GetNewsChildEndpointTests
    {
        public class HeadlineTests : IngestNewsItemTestSetup
        {
            private readonly HttpResponseMessage _response;
            private readonly IRepository _repository;
            private readonly IUnit[] units = {
                new Headline("This is a headline"),
                new Headline("This is another headline"),
            };

            public HeadlineTests()
            {
                _repository = Services.GetService<IRepository>();

                units[0].Meta.SetId(units[0].ToGuid());
                units[1].Meta.SetId(units[1].ToGuid());

                SaveSampleData().GetAwaiter().GetResult();
                _response = HttpClient.GetAsync($"/news/headlines/" + units[0].Meta.Id)
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
                value.meta.href.Should().Be(units[0].Meta.Href);
                value.value.Should().Be((units[0] as Headline).Value);
            }

            [Fact]
            public async Task ShouldNotRenderChildren_WhenChildrenDoNotExist() =>
                 JObject.Parse(await _response.Content.ReadAsStringAsync())
                    .ContainsKey("children")
                    .Should()
                    .BeFalse();

            private async Task SaveSampleData() =>
                await Task.WhenAll(units.Select(h => _repository.SaveAsync(h)));
        }
    }
}
