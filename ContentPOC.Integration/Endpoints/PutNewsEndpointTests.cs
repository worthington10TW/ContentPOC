using ContentPOC.DAL;
using ContentPOC.Unit.Model.News;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ContentPOC.Integration.Endpoints
{
    public class PutNewsEndpointTests : IngestNewsItemTestSetup
    {
        private readonly HttpResponseMessage _response;
        private readonly IRepository _repository;
        private readonly NewsItem news = new NewsItem();

        public PutNewsEndpointTests()
        {
            _repository = Services.GetService<IRepository>();
            _repository.SaveAsync(new NewsItem { });
        }

        [Fact]
        public async Task WhenIdDoesNotExist_ShouldReturnErrorResponse()
        {

        }

        [Fact]
        public async Task WhenIdDoesExist_ShouldUpdateResult()
        {

        }
    }
}