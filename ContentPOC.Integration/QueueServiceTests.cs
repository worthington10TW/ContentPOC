using ContentPOC.HostedService;
using ContentPOC.Unit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using Xunit;

namespace ContentPOC.Integration
{
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
        public void ShouldNotify_WhenUnitIsAddedToQueue()
        {
            var queue = _testServer.Host.Services.GetService<IUnitNotificationQueue>();
            var unit = new News { Headline = "BEST HEADLINE EVER" };
            queue.Queue(unit);

            _mockHub.Verify(x => x.Alert(unit));
        }

        public void Dispose() => _testServer.Dispose();
    }
}
