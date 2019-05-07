using ContentPOC.DAL;
using ContentPOC.Unit.Model.News;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
        public async Task ShouldReturnUpdatedStatusCode()
        {
            var data = new Dictionary<string, object> 
            { { "Children", new[] { Guid.NewGuid(), Guid.NewGuid() } } };
            var response = await HttpClient.PutAsJsonAsync(
                news.Meta.Href, data);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task WhenBodyIsEmpty_ShouldReturnBadStatusCode()
        {
            var response = await HttpClient.PutAsJsonAsync<Dictionary<string, object>>
                (news.Meta.Href, null);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}