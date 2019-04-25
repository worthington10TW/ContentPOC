using ContentPOC.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace ContentPOC.HostedService
{
    public interface INotificationHub
    {
        void Alert(IUnit unit);
    }

    public class SimulationNotificationHub : INotificationHub
    {
        private readonly ILogger<SimulationNotificationHub> _logger;

        public SimulationNotificationHub(ILogger<SimulationNotificationHub> logger) => _logger = logger;

        public void Alert(IUnit unit)
        {
            //simulate notification, sleep 5 seconds
            Thread.Sleep(new TimeSpan(0, 0, 5));
            _logger.LogInformation("****UNIT HAS BEEN UPDATED****");
            _logger.LogInformation($"****{unit.Meta.Id}****");
            _logger.LogInformation("********************************");
            _logger.LogInformation(unit.ToString());
            _logger.LogInformation("********************************");
        }
    }
}
