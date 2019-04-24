using ContentPOC.HostedService;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ContentPOC.Test
{
    public class InMemoryUnitNotificationQueueTests
    {
        private readonly InMemoryUnitNotificationQueue _queue = new InMemoryUnitNotificationQueue();

        [Fact]
        public async Task ShouldPopFromQueueInCorrectOrder()
        {
            var unit1 = new TestUnit { Id = new Id("1"), Href = "First" };
            _queue.Queue(unit1);
            var unit2 = new TestUnit { Id = new Id("2"), Href = "Second" };
            _queue.Queue(unit2);
            var unit3 = new TestUnit { Id = new Id("3"), Href = "Third" };
            _queue.Queue(unit3);

            var pop1 = await _queue.DequeueAsync(CancellationToken.None);
            pop1.Should().Be(unit1);
            var pop2 = await _queue.DequeueAsync(CancellationToken.None);
            pop2.Should().Be(unit2);
            var pop3 = await _queue.DequeueAsync(CancellationToken.None);
            pop3.Should().Be(unit3);
        }

        [Fact]
        public void ShouldThrowWhenUnitIsNull()
        {
            Action action = () => _queue.Queue(null);
            action.Should().Throw<ArgumentNullException>();
        }

        public class TestUnit : IUnit
        {
            public string Href { get; set; }

            public Id Id { get; set; }
        }
    }
}
