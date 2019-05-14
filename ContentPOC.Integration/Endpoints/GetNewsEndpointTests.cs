using AutoFixture;
using ContentPOC.DAL;
using ContentPOC.Extensions;
using ContentPOC.Model;
using ContentPOC.Unit.Model.News;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static ContentPOC.Integration.Endpoints.Dto;

namespace ContentPOC.Integration.Endpoints
{
    public class GetNewsEndpointTests : IngestNewsItemTestSetup
    {
        private readonly HttpResponseMessage _response;
        private readonly IRepository _repository;
        private readonly Fixture _fixture = new Fixture();
        private readonly IUnit[] _units;

        public GetNewsEndpointTests()
        {
            _repository = Services.GetService<IRepository>();
            (_repository as InMemoryStore).Reset();

            _units = Enumerable.Range(0, 10)
                .Select(i => new NewsItem(_fixture.Create<Headline>()))
                .OrderBy(x => x.Meta.Href)
                .ToArray();
            SaveSampleData(_units, _repository).GetAwaiter().GetResult();

            _response = HttpClient.GetAsync($"/news/")
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public void ShouldReturn200Response_WhenExist() =>
            _response.StatusCode.Should().Be(HttpStatusCode.OK);

        private static async Task SaveSampleData(IUnit[] units, IRepository repository) =>
            await Task.WhenAll(units.Select(repository.SaveAsync));
    }
}
