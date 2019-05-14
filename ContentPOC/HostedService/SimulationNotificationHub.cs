using ContentPOC.Model;
using ContentPOC.Model.News;
using ContentPOC.NewsIngestor;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContentPOC.HostedService
{
    public interface INotificationHub
    {
    Task Alert(RawNewsContentIngested unit);
    }

    public class SimulationNotificationHub : INotificationHub
    {
        private readonly ILogger<SimulationNotificationHub> _logger;
        private readonly IHubContext<NotificationHub> hubContext;

        public SimulationNotificationHub(
            ILogger<SimulationNotificationHub> logger,
            IHubContext<NotificationHub> hub) 
        {
            _logger = logger;
            hubContext = hub;
        }

        public async Task Alert(RawNewsContentIngested @event)
        {
            //simulate notification, sleep 5 seconds
            Thread.Sleep(new TimeSpan(0, 0, 5));
            _logger.LogInformation("****EVENT HAS BEEN UPDATED****");
            _logger.LogInformation($"****{@event.Location}****");
            _logger.LogInformation("********************************");
            _logger.LogInformation(@event.ToString());
            _logger.LogInformation("********************************");

            await hubContext.Clients.All
                .SendAsync("NewsUpdate", @event.Location);
        }
    }
}
