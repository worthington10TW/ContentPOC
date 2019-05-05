using ContentPOC.DAL;
using ContentPOC.Unit.Model.News;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ContentPOC.Integration.Endpoints
{
    public class PutNewsEndpointTests : IngestNewsItemTestSetup
    {
        private readonly IRepository _repository;
        private readonly NewsItem news = new NewsItem();

        public PutNewsEndpointTests()
        {
            _repository = Services.GetService<IRepository>();
            _repository.SaveAsync(news);
        }

        [Fact]
        public async Task WhenIdDoesNotExist_ShouldReturnErrorResponse()
        {
            var response = await HttpClient.PutAsync("/news/" + Guid.NewGuid(), new StringContent("stuff"));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task WhenIdDoesExist_ShouldReturnUpdatedStatusCode()
        {
            var response = await HttpClient.PutAsync(news.Meta.Href, new StringContent("stuff"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}