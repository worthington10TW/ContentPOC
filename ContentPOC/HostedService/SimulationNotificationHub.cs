using ContentPOC.Model;
using ContentPOC.Model.News;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace ContentPOC.HostedService
{
    public interface INotificationHub
    {
        void Alert(RawNewsContentIngested unit);
    }

    public class SimulationNotificationHub : INotificationHub
    {
        private readonly ILogger<SimulationNotificationHub> _logger;

        public SimulationNotificationHub(ILogger<SimulationNotificationHub> logger) => _logger = logger;

        public void Alert(RawNewsContentIngested @event)
        {
            //simulate notification, sleep 5 seconds
            Thread.Sleep(new TimeSpan(0, 0, 5));
            _logger.LogInformation("****EVENT HAS BEEN UPDATED****");
            _logger.LogInformation($"****{@event.Location}****");
            _logger.LogInformation("********************************");
            _logger.LogInformation(@event.ToString());
            _logger.LogInformation("********************************");
        }
    }
}
