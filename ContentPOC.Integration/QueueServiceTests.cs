using ContentPOC.HostedService;
using ContentPOC.Model.News;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace ContentPOC.Integration
{

    // TODO: Change to a queue which takes events (with HREF, ID (both meta) and unit type)

    public class QueueServiceTests : IDisposable
    {
        private readonly TestServer _testServer;
        private Mock<INotificationHub> _mockHub = new Mock<INotificationHub>();

        public QueueServiceTests()
        {
            var builder = Program.WebHostBuilder();
            builder.ConfigureTestServices(x => x.AddTransient(s => _mockHub.Object));
            _testServer = new TestServer(builder);
        }

        [Fact]
        public void ShouldNotify_WhenEventIsAddedToQueue()
        {
            var queue = _testServer.Host.Services.GetService<INotificationQueue>();
            var @event = new RawNewsContentIngested { Location = "BEST LOCATION EVER" };
            queue.Queue(@event);
            Thread.Sleep(new TimeSpan(0, 0, 1));

            _mockHub.Verify(x => x.Alert(@event));
        }

        public void Dispose() => _testServer.Dispose();
    }
}
