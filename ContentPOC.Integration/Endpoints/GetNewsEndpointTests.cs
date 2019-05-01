using AutoFixture;
using ContentPOC.DAL;
using ContentPOC.Model;
using ContentPOC.Unit.Model.News;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

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
            _units = 
                Enumerable.Range(0, 100)
                .Select(i => new NewsItem(_fixture.Create<Headline>()))
                .ToArray();
            SaveSampleData(_units, _repository).GetAwaiter().GetResult();
            _response = HttpClient.GetAsync($"/api/news/")
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public void ShouldReturn200Response_WhenExist() =>
            _response.StatusCode.Should().Be(HttpStatusCode.OK);

        //[Fact]
        //public async Task ShouldGetInsertedHeadline()
        //{
        //    var values = await _response.Content.ReadAsAsync<NewsDto[]>();
        //    foreach (var value in values)
        //        value.meta.href.Should().Be(_units[0].Meta.Href);
        //}

        private static async Task SaveSampleData(IUnit[] units, IRepository repository) =>
            await Task.WhenAll(units.Select(h => repository.SaveAsync(h)));
    }
}
