using ContentPOC.Model;
using ContentPOC.Model.News;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ContentPOC.HostedService
{
    public interface INotificationQueue
    {
        void Queue(RawNewsContentIngested @event);

        Task<RawNewsContentIngested> DequeueAsync(CancellationToken cancellationToken);
    }

    public class RawNewsContentIngestedQueue : INotificationQueue
    {
        private readonly ConcurrentQueue<RawNewsContentIngested> _events = new ConcurrentQueue<RawNewsContentIngested>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public void Queue(RawNewsContentIngested @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            _events.Enqueue(@event);
            _signal.Release();
        }

        public async Task<RawNewsContentIngested> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _events.TryDequeue(out var @event);

            return @event;
        }
    }

}
