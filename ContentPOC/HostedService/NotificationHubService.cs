using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContentPOC.HostedService
{
    public class NotificationHubService : BackgroundService
    {
        private readonly IUnitNotificationQueue _taskQueue;
        private readonly INotificationHub _hub;

        public NotificationHubService(IUnitNotificationQueue taskQueue, INotificationHub hub)
        {
            _taskQueue = taskQueue;
            _hub = hub;
        }
            

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Queued Hosted Service is starting.");

            while (!cancellationToken.IsCancellationRequested)
            {
                var unit = await _taskQueue.DequeueAsync(cancellationToken);

                try
                {
                    _hub.Alert(unit);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.ToString() +
                       $"Error occurred executing {nameof(unit)}.");
                }
            }

            Console.WriteLine("Queued Hosted Service is stopping.");
        }
    }  
}