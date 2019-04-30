using ContentPOC.HostedService;
using ContentPOC.Model;
using ContentPOC.Model.News;
using ContentPOC.Unit;
using ContentPOC.Unit.Model;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ContentPOC.Test
{
    public class InMemoryUnitNotificationQueueTests
    {
        private readonly RawNewsContentIngestedQueue _queue = new RawNewsContentIngestedQueue();

        [Fact]
        public async Task ShouldPopFromQueueInCorrectOrder()
        {
            var event1 = new RawNewsContentIngested { Location = "First" };
            _queue.Queue(event1);
            var event2 = new RawNewsContentIngested { Location = "Second" };
            _queue.Queue(event2);
            var event3 = new RawNewsContentIngested { Location = "Third" };
            _queue.Queue(event3);

            var pop1 = await _queue.DequeueAsync(CancellationToken.None);
            pop1.Should().Be(event1);
            var pop2 = await _queue.DequeueAsync(CancellationToken.None);
            pop2.Should().Be(event2);
            var pop3 = await _queue.DequeueAsync(CancellationToken.None);
            pop3.Should().Be(event3);
        }

        [Fact]
        public void ShouldThrowWhenUnitIsNull()
        {
            Action action = () => _queue.Queue(null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
