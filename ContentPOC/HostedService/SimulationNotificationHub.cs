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
        public void Alert(IUnit unit)
        {
            //simulate notification, sleep 5 seconds
            Thread.Sleep(new TimeSpan(0, 0, 5));
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("****UNIT HAS BEEN UPDATED****");
            Console.WriteLine($"****{unit.Id}****");
            Console.WriteLine("********");
            Console.WriteLine(unit.ToString());
            Console.WriteLine("********");
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
