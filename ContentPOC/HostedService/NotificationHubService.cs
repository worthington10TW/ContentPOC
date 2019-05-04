using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContentPOC.HostedService
{
    public class NotificationHubService : BackgroundService
    {
        private readonly INotificationQueue _taskQueue;
        private readonly INotificationHub _hub;
        private bool disable = false;

        public NotificationHubService(INotificationQueue taskQueue, INotificationHub hub)
        {
            _taskQueue = taskQueue;
            _hub = hub;
        }

        public void DisableService() => disable = true;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Queued Hosted Service is starting.");

            while (!cancellationToken.IsCancellationRequested || !disable)
            {
                var @event = await _taskQueue.DequeueAsync(cancellationToken);

                try
                {
                    _hub.Alert(@event);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.ToString() +
                       $"Error occurred executing {nameof(@event)}.");
                }
            }

            Console.WriteLine("Queued Hosted Service is stopping.");
        }
    }  
}