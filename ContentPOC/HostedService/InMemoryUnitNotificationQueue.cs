using ContentPOC.Model;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ContentPOC.HostedService
{
    public interface IUnitNotificationQueue
    {
        void Queue(IUnit unit);

        Task<IUnit> DequeueAsync(CancellationToken cancellationToken);
    }

    public class RawNewsIngestedContentQueue : IUnitNotificationQueue
    {
        private readonly ConcurrentQueue<IUnit> _units = new ConcurrentQueue<IUnit>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public void Queue(IUnit unit)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit));

            _units.Enqueue(unit);
            _signal.Release();
        }

        public async Task<IUnit> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _units.TryDequeue(out var unit);

            return unit;
        }
    }

}
