using ContentPOC.HostedService;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ContentPOC.Integration.Endpoints
{
    public class PublishEventTests : IngestNewsItemTestSetup
    {
        [Fact]
        public async Task ShouldPublishEventWhenNewsIsIngested()
        {
            var queue = Services.GetService<INotificationQueue>();
            var rawNewsContentIngestedEvent = await queue.DequeueAsync(CancellationToken.None);
            rawNewsContentIngestedEvent.Location.Should().Be($"news/{NEWS_ID}");
        }
    }
}
